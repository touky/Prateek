// -BEGIN_PRATEEK_COPYRIGHT-
//
//  Prateek, a library that is "bien pratique"
//  Header last update date: 20/03/2019
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
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.Serialization;

#if UNITY_PROFILING
using UnityEngine.Profiling;
#endif //UNITY_PROFILING

using Unity.Jobs;
using Unity.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif //UNITY_EDITOR
#endregion Unity

#region Prateek
using Prateek;
using Prateek.Base;
using Prateek.Extensions;
using Prateek.Helpers;
using Prateek.Attributes;

#if UNITY_EDITOR
using Prateek.ScriptTemplating;
#endif //UNITY_EDITOR

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
namespace Prateek.ScriptTemplating
{
    //-------------------------------------------------------------------------
    [InitializeOnLoad]
    class MixedFuncScriptTemplate : TemplateReplacement
    {
        static MixedFuncScriptTemplate()
        {
            NewMixedFunc(Code.Tag.importExtension).Commit();
        }
    }

    //-------------------------------------------------------------------------
    public partial class TemplateReplacement
    {
        //---------------------------------------------------------------------
        protected static MixedFuncRule NewMixedFunc(string extension)
        {
            return new MixedFuncRule(extension);
        }

        //---------------------------------------------------------------------
        [InitializeOnLoad]
        public partial class MixedFuncRule : CodeRule
        {
            //-----------------------------------------------------------------
            public override string ScopeTag { get { return "MIXED_FUNC"; } }
            public override GenerationMode GenMode { get { return GenerationMode.ForeachSrc; } }
            public override bool GenerateDefault { get { return true; } }

            //-----------------------------------------------------------------
            public MixedFuncRule(string extension) : base(extension) { }

            //-----------------------------------------------------------------
            #region CodeRule override
            public override Code.Tag.KeyRule GetKeyRule(string keyword, int codeDepth)
            {
                if (keyword == Code.Tag.Macro.Func)
                {
                    return new Code.Tag.KeyRule(keyword, codeDepth == 2) { args = 1, needOpenScope = true, needScopeData = true };
                }
                else
                {
                    return base.GetKeyRule(keyword, codeDepth);
                }
            }

            //-----------------------------------------------------------------
            protected override bool DoTreatData(Code.File.Data activeData, Code.Tag.KeyRule keyRule, List<string> args, string data)
            {
                if (keyRule.key == Code.Tag.Macro.Func)
                {
                    activeData.funcInfos.Add(new Code.File.Data.FuncInfo()
                    {
                        name = args[0],
                        data = data
                    });
                }
                else
                {
                    return base.DoTreatData(activeData, keyRule, args, data);
                }
                return true;
            }
            #endregion CodeRule override

            //-----------------------------------------------------------------
            #region SwizzleRule internal
            protected override void GatherVariants(List<Variant> variants, Code.File.Data data, Code.File.Data.ClassInfo infoSrc, Code.File.Data.ClassInfo infoDst)
            {
                variants.Clear();

                for (int d = 0; d < data.funcInfos.Count; d++)
                {
                    var funcInfo = data.funcInfos[d];
                    var variant = new Variant(funcInfo.name);
                    var argCount = 0;

                    for (int v = 0; v < VarCount; v++)
                    {
                        if (funcInfo.data.Contains(this[v].Original))
                            argCount++;
                    }

                    var vars = funcInfo.data;
                    for (int a = 0; a < argCount; a++)
                    {
                        if (infoSrc.variables == null || infoSrc.variables.Count == 0)
                        {
                            variant.Args = string.Format(Code.Tag.Code.argsN, data.classDefaultType, a);
                            vars = (this[a] + string.Format(Code.Tag.Code.varsN, a)).Apply(vars);
                        }
                        else
                        {
                            variant.Args = string.Format(Code.Tag.Code.argsV_, infoSrc.names[0], a);
                        }
                    }

                    if (infoSrc.variables == null || infoSrc.variables.Count == 0)
                    {
                        variant.Vars = vars;
                    }
                    else
                    {
                        for (int v = 0; v < infoSrc.variables.Count; v++)
                        {
                            var varsA = vars;
                            for (int a = 0; a < argCount; a++)
                            {
                                varsA = (this[a] + string.Format(Code.Tag.Code.varsV_, a, infoSrc.variables[v])).Apply(varsA);
                            }
                            variant.Vars = varsA;
                        }

                        variant = new Variant(variant.Call)
                        {
                            Args = variant.Args,
                            Vars = Code.Tag.Code.varNew + infoSrc.names[0] + Strings.Separator.Parenthesis.C()[0] + variant.Vars + Strings.Separator.Parenthesis.C()[1]
                        };
                    }

                    variants.Add(variant);
                }
            }
            #endregion SwizzleRule internal
        }
    }
}
