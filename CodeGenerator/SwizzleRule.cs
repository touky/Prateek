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
using Prateek.ScriptTemplating;
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
namespace Prateek.ScriptTemplating
{
    //-------------------------------------------------------------------------
    [InitializeOnLoad]
    class SwizzleScriptTemplate : TemplateReplacement
    {
        static SwizzleScriptTemplate()
        {
            NewSwizzle(Code.Tag.importExtension).Commit();
        }
    }

    //-------------------------------------------------------------------------
    public partial class TemplateReplacement
    {
        //---------------------------------------------------------------------
        protected static SwizzleRule NewSwizzle(string extension)
        {
            return new SwizzleRule(extension);
        }

        //---------------------------------------------------------------------
        [InitializeOnLoad]
        public partial class SwizzleRule : CodeRule
        {
            //-----------------------------------------------------------------
            public override string ScopeTag { get { return "SWIZZLE"; } }
            public override GenerationMode GenMode { get { return GenerationMode.ForeachSrcXDest; } }

            //-----------------------------------------------------------------
            public SwizzleRule(string extension) : base(extension) { }

            //-----------------------------------------------------------------
            #region SwizzleRule internal
            protected override void GatherVariants(List<Variant> variants, Code.File.Data data, Code.File.Data.ClassInfo infoSrc, Code.File.Data.ClassInfo infoDst)
            {
                var slots = new int[infoDst.variables.Count];
                for (int s = 0; s < slots.Length; s++)
                {
                    slots[s] = 0;
                }

                variants.Clear();
                GatherVariantsSlots(0, slots, variants, data, infoSrc, infoDst);
            }

            //-----------------------------------------------------------------
            private void GatherVariantsSlots(int s, int[] slots, List<Variant> variants, Code.File.Data data, Code.File.Data.ClassInfo infoSrc, Code.File.Data.ClassInfo infoDst)
            {
                var varCount = infoSrc.variables.Count + 1;
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
                        var variant = new Variant(string.Empty);
                        variant.Args += Code.Tag.Code.argsV;
                        for (int v = 0; v < slots.Length; v++)
                        {
                            var sv = slots[v];
                            if (sv < infoSrc.variables.Count)
                            {
                                var variable = infoSrc.variables[sv];
                                variant.Call = variable;
                                variant.Vars = string.Format(Code.Tag.Code.varsV, variable);
                            }
                            else
                            {
                                variant.Call = Code.Tag.Code.callN;
                                variant.Args = string.Format(Code.Tag.Code.argsNOpt, data.classDefaultType, sn, data.classDefaultValue);
                                variant.Vars = string.Format(Code.Tag.Code.varsN, sn);
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
            #endregion SwizzleRule internal
        }
    }
}
