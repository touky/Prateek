// -BEGIN_PRATEEK_COPYRIGHT-
//
//  Prateek, a library that is "bien pratique"
//  Header last update date: 24/03/2019
//
//  Copyright © 2017-2019 "Touky" <touky@prateek.top>
//
//  Prateek is free software. It comes without any warranty, to
//  the extent permitted by applicable law. You can redistribute it
//  and/or modify it under the terms of the Do What the Fuck You Want
//  to Public License, Version 2, as published by the WTFPL Task Force.
//  See http://www.wtfpl.net/ for more details.
//
// -END_PRATEEK_COPYRIGHT-

// -BEGIN_PRATEEK_CSHARP_NAMESPACE-
//
#region C# Prateek Namespaces
#if UNITY_EDITOR && !PRATEEK_DEBUG
#define PRATEEK_DEBUG
#endif //UNITY_EDITOR && !PRATEEK_DEBUG

#region System
using System;
using System.Collections;
using System.Collections.Generic;
#endregion System

#region Unity
using Unity.Jobs;
using Unity.Collections;

#region Engine
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.Serialization;

#if UNITY_PROFILING
using UnityEngine.Profiling;
#endif //UNITY_PROFILING
#endregion Engine

#region Editor
#if UNITY_EDITOR
using UnityEditor;
#endif //UNITY_EDITOR
#endregion Editor
#endregion Unity

#region Prateek
using Prateek;
using Prateek.Base;
using Prateek.Extensions;
using Prateek.Helpers;
using Prateek.Attributes;

#region Using static
using static Prateek.ShaderTo.CSharp;
#endregion Using static

#region Editor
#if UNITY_EDITOR
using Prateek.CodeGeneration;
#endif //UNITY_EDITOR
#endregion Editor

#if PRATEEK_DEBUGS
using Prateek.Debug;
#endif //PRATEEK_DEBUG
#endregion Prateek

#endregion C# Prateek Namespaces
//
// -END_PRATEEK_CSHARP_NAMESPACE-

//-----------------------------------------------------------------------------
#region File namespaces
using System.IO;
using Prateek.IO;
using System.Text.RegularExpressions;
#endregion File namespaces

//-----------------------------------------------------------------------------
namespace Prateek.CodeGeneration
{
    //-------------------------------------------------------------------------
    [InitializeOnLoad]
    class BasicFuncRuleLoader : PrateekScriptBuilder
    {
        static BasicFuncRuleLoader()
        {
            NewBasicFunc(Tag.importExtension).Commit();
        }
    }

    //-------------------------------------------------------------------------
    public partial class PrateekScriptBuilder
    {
        //---------------------------------------------------------------------
        protected static BasicFuncCodeRule NewBasicFunc(string extension)
        {
            return new BasicFuncCodeRule(extension);
        }

        //---------------------------------------------------------------------
        [InitializeOnLoad]
        public partial class BasicFuncCodeRule : CodeRule
        {
            //-----------------------------------------------------------------
            public override string ScopeTag { get { return "BASIC_FUNC"; } }
            public override GenerationMode GenMode { get { return GenerationMode.ForeachSrc; } }
            public override bool GenerateDefault { get { return false; } }

            //-----------------------------------------------------------------
            public BasicFuncCodeRule(string extension) : base(extension) { }

            //-----------------------------------------------------------------
            #region CodeRule override
            public override Utils.KeyRule GetKeyRule(string keyword, string activeScope)
            {
                if (keyword == Tag.Macro.Func)
                {
                    return new Utils.KeyRule(keyword, activeScope == CodeBlock) { needOpenScope = true, needScopeData = true };
                }
                else
                {
                    return base.GetKeyRule(keyword, activeScope);
                }
            }

            //-----------------------------------------------------------------
            protected override bool DoRetrieveRuleContent(CodeFile.ContentInfos activeData, Utils.KeyRule keyRule, List<string> args, string data)
            {
                if (keyRule.key == Tag.Macro.Func)
                {
                    activeData.funcInfos.Add(new CodeFile.FuncInfos() { data = data });
                }
                else
                {
                    return base.DoRetrieveRuleContent(activeData, keyRule, args, data);
                }
                return true;
            }
            #endregion CodeRule override

            //-----------------------------------------------------------------
            #region Rule internal
            protected override void GatherVariants(List<FuncVariant> variants, CodeFile.ContentInfos data, CodeFile.ClassInfos infoSrc, CodeFile.ClassInfos infoDst)
            {
                variants.Clear();

                for (int d = 0; d < data.funcInfos.Count; d++)
                {
                    var funcInfo = data.funcInfos[d];
                    var content = funcInfo.data;
                    for (int n = 0; n < infoSrc.NameCount + 1; n++)
                    {
                        var vars = (Vars[n] + (n == 0 ? infoSrc.className : infoSrc.names[n - 1]));
                        content = vars.Apply(content);
                    }

                    var variant = new FuncVariant(content);
                    variants.Add(variant);
                }
            }
            #endregion Rule internal
        }
    }
}
