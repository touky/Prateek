// -BEGIN_PRATEEK_COPYRIGHT-
//
//  Prateek, a library that is "bien pratique"
//  Header last update date: 16/04/2019
//
//  Copyright � 2017-2019 "Touky" <touky@prateek.top>
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
//-----------------------------------------------------------------------------
#region C# Prateek Namespaces

//Auto activate some of the prateek defines
#if UNITY_EDITOR

#if !PRATEEK_DEBUG
#define PRATEEK_DEBUG
#endif //!PRATEEK_DEBUG

#endif //UNITY_EDITOR && !PRATEEK_DEBUG

//-----------------------------------------------------------------------------
#region System
using System;
using System.Collections;
using System.Collections.Generic;
#endregion System

//-----------------------------------------------------------------------------
#region Unity
using Unity.Jobs;
using Unity.Collections;

#region Engine
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.Serialization;

//-----------------------------------------------------------------------------
#if UNITY_PROFILING
using UnityEngine.Profiling;
#endif //UNITY_PROFILING

#endregion Engine

//-----------------------------------------------------------------------------
#if UNITY_EDITOR
using UnityEditor;
#endif //UNITY_EDITOR

#endregion Unity

//-----------------------------------------------------------------------------
#region Prateek
using Prateek;
using Prateek.Base;
using Prateek.Extensions;
using Prateek.Helpers;
using Prateek.Attributes;
using Prateek.Manager;

//-----------------------------------------------------------------------------
#region Using static
using static Prateek.ShaderTo.CSharp;
#endregion Using static

//-----------------------------------------------------------------------------
#if UNITY_EDITOR
using Prateek.CodeGeneration;
#endif //UNITY_EDITOR

//-----------------------------------------------------------------------------
#if PRATEEK_DEBUG
using Prateek.Debug;
using static Prateek.Debug.DebugDraw.DebugStyle.QuickCTor;
using DebugDraw = Prateek.Debug.DebugDraw;
using DebugPlace = Prateek.Debug.DebugDraw.DebugPlace;
using DebugStyle = Prateek.Debug.DebugDraw.DebugStyle;
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
#if UNITY_EDITOR
    [InitializeOnLoad]
    class MixedFuncRuleLoader : PrateekScriptBuilder
    {
        static MixedFuncRuleLoader()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
                return;

            NewMixedFunc(Tag.importExtension).Commit();
        }
    }
#endif //UNITY_EDITOR

    //-------------------------------------------------------------------------
    public partial class PrateekScriptBuilder
    {
        //---------------------------------------------------------------------
        protected static MixedFuncCodeRule NewMixedFunc(string extension)
        {
            return new MixedFuncCodeRule(extension);
        }

        //---------------------------------------------------------------------
        public partial class MixedFuncCodeRule : CodeRule
        {
            //-----------------------------------------------------------------
            public override string ScopeTag { get { return "FUNC_MIXED"; } }
            public override GenerationMode GenMode { get { return GenerationMode.ForeachSrc; } }
            public override bool GenerateDefault { get { return true; } }

            //-----------------------------------------------------------------
            public MixedFuncCodeRule(string extension) : base(extension) { }

            //-----------------------------------------------------------------
            #region CodeRule override
            public override Utils.KeyRule GetKeyRule(string keyword, string activeScope)
            {
                var keyRule = new Utils.KeyRule(keyword, activeScope);
                if (keyRule.Match(Tag.Macro.Func, CodeBlock))
                {
                    { keyRule.args = 1; keyRule.needOpenScope = true; keyRule.needScopeData = true; }
                }
                else
                {
                    return base.GetKeyRule(keyword, activeScope);
                }
                return keyRule;
            }

            //-----------------------------------------------------------------
            protected override bool DoRetrieveRuleContent(CodeFile.ContentInfos activeData, Utils.KeyRule keyRule, List<string> args, string data)
            {
                if (keyRule.Match(Tag.Macro.Func, CodeBlock))
                {
                    activeData.funcInfos.Add(new CodeFile.FuncInfos() { funcName = args[0], data = data });
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

                var isDefault = infoSrc.VarCount == 0;
                for (int d = 0; d < data.funcInfos.Count; d++)
                {
                    for (int p = 0; p < (isDefault ? 1 : 2); p++)
                    {
                        if (data.classDefaultExportOnly && (isDefault || p == 0))
                            continue;

                        var funcInfo = data.funcInfos[d];
                        var variant = new FuncVariant(funcInfo.funcName, 2);

                        var varsCount = Vars.GetCount(funcInfo.data);
                        if (p == 1 && varsCount == 1)
                            continue;

                        var vars = funcInfo.data;
                        for (int a = 0; a < varsCount; a++)
                        {
                            if (isDefault)
                            {
                                variant[1] = string.Format(Tag.Code.argsN, data.classDefaultType, a);
                                vars = (Vars[a] + string.Format(Tag.Code.varsN, a)).Apply(vars);
                            }
                            else
                            {
                                variant[1] = (p == 1 && a != 0)
                                                ? string.Format(Tag.Code.argsN, data.classDefaultType, a)
                                                : string.Format(Tag.Code.argsV_, infoSrc.className, a);
                            }
                        }

                        if (isDefault)
                        {
                            variant[2] = vars;
                        }
                        else
                        {
                            for (int v = 0; v < infoSrc.VarCount; v++)
                            {
                                var varsA = vars;
                                for (int a = 0; a < varsCount; a++)
                                {
                                    varsA = (p == 1 && a != 0)
                                             ? (Vars[a] + string.Format(Tag.Code.varsN, a)).Apply(varsA)
                                             : (Vars[a] + string.Format(Tag.Code.varsV_, a, infoSrc.variables[v])).Apply(varsA);
                                }
                                variant[2] = varsA;
                            }

                            var v2 = new FuncVariant(variant.Call, 2);
                            v2[1] = variant[1];
                            v2[2] = Tag.Code.varNew + infoSrc.className + Strings.Separator.ParenthesisOpen.C() + variant[2] + Strings.Separator.ParenthesisClose.C();
                            variant = v2;
                        }

                        variants.Add(variant);
                    }
                }
            }
            #endregion Rule internal
        }
    }
}
