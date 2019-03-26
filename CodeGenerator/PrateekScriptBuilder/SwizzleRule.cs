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
    class SwizzleRuleLoader : PrateekScriptBuilder
    {
        static SwizzleRuleLoader()
        {
            NewSwizzle(Tag.importExtension).Commit();
        }
    }

    //-------------------------------------------------------------------------
    public partial class PrateekScriptBuilder
    {
        //---------------------------------------------------------------------
        protected static SwizzleCodeRule NewSwizzle(string extension)
        {
            return new SwizzleCodeRule(extension);
        }

        //---------------------------------------------------------------------
        [InitializeOnLoad]
        public partial class SwizzleCodeRule : CodeRule
        {
            //-----------------------------------------------------------------
            public override string ScopeTag { get { return "SWIZZLE"; } }
            public override GenerationMode GenMode { get { return GenerationMode.ForeachSrcXDest; } }

            //-----------------------------------------------------------------
            public SwizzleCodeRule(string extension) : base(extension) { }

            //-----------------------------------------------------------------
            #region Rule internal
            protected override void GatherVariants(List<FuncVariant> variants, CodeFile.ContentInfos data, CodeFile.ClassInfos infoSrc, CodeFile.ClassInfos infoDst)
            {
                var slots = new int[infoDst.VarCount];
                for (int s = 0; s < slots.Length; s++)
                {
                    slots[s] = 0;
                }

                variants.Clear();
                GatherVariantsSlots(0, slots, variants, data, infoSrc, infoDst);
            }

            //-----------------------------------------------------------------
            private void GatherVariantsSlots(int s, int[] slots, List<FuncVariant> variants, CodeFile.ContentInfos data, CodeFile.ClassInfos infoSrc, CodeFile.ClassInfos infoDst)
            {
                var varCount = infoSrc.VarCount + 1;
                for (int c = 0; c < varCount; c++)
                {
                    slots[s] = c;
                    if (s + 1 < slots.Length)
                    {
                        GatherVariantsSlots(s + 1, slots, variants, data, infoSrc, infoDst);
                    }
                    else
                    {
                        var sn = 0;
                        var variant = new FuncVariant(string.Empty, 2);
                        variant[1] += Tag.Code.argsV;
                        for (int v = 0; v < slots.Length; v++)
                        {
                            var sv = slots[v];
                            if (sv < infoSrc.VarCount)
                            {
                                var variable = infoSrc.variables[sv];
                                variant.Call = variable;
                                variant[2] = string.Format(Tag.Code.varsV, variable);
                            }
                            else
                            {
                                variant.Call = Tag.Code.callN;
                                variant[1] = string.Format(Tag.Code.argsNOpt, data.classDefaultType, sn, data.classDefaultValue);
                                variant[2] = string.Format(Tag.Code.varsN, sn);
                                sn++;
                            }
                        }

                        if (sn != slots.Length)
                        {
                            variants.Add(variant);
                        }
                    }
                }
            }
            #endregion Rule internal
        }
    }
}
