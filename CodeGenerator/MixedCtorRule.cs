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
    class MixedCTorScriptTemplate : TemplateReplacement
    {
        static MixedCTorScriptTemplate()
        {
            NewMixedCTor(Code.Tag.importExtension).Commit();
        }
    }

    //-------------------------------------------------------------------------
    public partial class TemplateReplacement
    {
        //---------------------------------------------------------------------
        protected static MixedCTorRule NewMixedCTor(string extension)
        {
            return new MixedCTorRule(extension);
        }

        //---------------------------------------------------------------------
        [InitializeOnLoad]
        public partial class MixedCTorRule : CodeRule
        {
            //-----------------------------------------------------------------
            public override string ScopeTag { get { return "MIXED_CTOR"; } }

            //-----------------------------------------------------------------
            public MixedCTorRule(string extension) : base(extension) { }

            //-----------------------------------------------------------------
            #region CodeRule override
            public override Code.Tag.KeyRule GetKeyRule(string keyword, int codeDepth)
            {
                if (keyword == Code.Tag.Macro.OperationClass)
                {
                    return new Code.Tag.KeyRule(keyword, codeDepth == 2) { minArgCount = 2 };
                }
                else
                {
                    return base.GetKeyRule(keyword, codeDepth);
                }
            }

            //-----------------------------------------------------------------
            protected override bool DoTreatData(Code.File.Data activeData, Code.Tag.KeyRule keyRule, List<string> args, string data)
            {
                if (keyRule.key == Code.Tag.Macro.OperationClass)
                {
                    activeData.classInfos.Add(new Code.File.Data.ClassInfo()
                    {
                        names = args.GetRange(0, 2),
                        variables = args.GetRange(2, args.Count - 2)
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
            public override void Generate(Code.File.Data data)
            {
                var variants = new List<Variant>();
                for (int iSrc = 0; iSrc < data.classInfos.Count; iSrc++)
                {
                    var infoSrc = data.classInfos[iSrc];

                    GatherVariants(variants, data, infoSrc);

                    var swapSrc = ClassSrc + infoSrc.names[0];
                    AddCode(data.codePrefix, data, swapSrc);
                    for (int v = 0; v < variants.Count; v++)
                    {
                        var variant = variants[v];
                        var code = data.codeMain;
                        code = (CodeCall + variant.Call).Apply(code);
                        code = (CodeArgs + variant.Args).Apply(code);
                        code = (CodeVars + variant.Vars).Apply(code);
                        AddCode(code, data, swapSrc);
                    }
                    AddCode(data.codePostfix, data, swapSrc);
                }

                data.codeGenerated = data.codeGenerated.Replace(Strings.NewLine(String.Empty), Strings.NewLine(String.Empty) + Code.Tag.Macro.codeGenTabs.Keyword());
            }

            //-----------------------------------------------------------------
            private void GatherVariants(List<Variant> variants, Code.File.Data data, Code.File.Data.ClassInfo infoSrc)
            {
                var slots = new int[infoSrc.variables.Count];
                for (int s = 0; s < slots.Length; s++)
                {
                    slots[s] = 0;
                }

                variants.Clear();
                GatherVariants(0, slots, slots.Length, variants, data, infoSrc);
            }

            //-----------------------------------------------------------------
            private void GatherVariants(int s, int[] slots, int count, List<Variant> variants, Code.File.Data data, Code.File.Data.ClassInfo infoSrc)
            {
                var classCount = data.classInfos.Count + 1;
                for (int c = 0; c < classCount; c++)
                {
                    var varCount = c == 0 ? 1 : data.classInfos[c - 1].variables.Count;
                    slots[s] = c;
                    if (count - varCount > 0)
                    {
                        GatherVariants(s + 1, slots, count - varCount, variants, data, infoSrc);
                    }
                    else if (count - varCount == 0)
                    {
                        var sn = 0;
                        var sv = 0;
                        var variant = new Variant(string.Empty) { Call = infoSrc.names[1] };
                        for (int v = 0; v < slots.Length && v < s + 1; v++)
                        {
                            var sl = slots[v];
                            if (sl == 0)
                            {
                                variant.Args = string.Format(Code.Tag.Code.argsN, data.classDefaultType, sn);
                                variant.Vars = string.Format(Code.Tag.Code.varsN, sn);
                                sn++;
                            }
                            else
                            {
                                sl -= 1;
                                var info = data.classInfos[sl];
                                variant.Args = string.Format(Code.Tag.Code.argsV_, info.names[0], sv);
                                for (int vr = 0; vr < info.variables.Count; vr++)
                                {
                                    variant.Vars = string.Format(Code.Tag.Code.varsV_, sv, info.variables[vr]);
                                }
                                sv++;
                            }
                        }

                        variants.Add(variant);
                    }
                }
            }
            #endregion SwizzleRule internal
        }
    }
}
