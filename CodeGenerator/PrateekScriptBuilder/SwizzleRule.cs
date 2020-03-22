// -BEGIN_PRATEEK_COPYRIGHT-
//
//  Prateek, a library that is "bien pratique"
//  Header last update date: 22/03/2020
//
//  Copyright � 2017-2020 "Touky" <touky@prateek.top>
//
//  Prateek is free software. It comes without any warranty, to
//  the extent permitted by applicable law. You can redistribute it
//  and/or modify it under the terms of the Do What the Fuck You Want
//  to Public License, Version 2, as published by the WTFPL Task Force.
//  See http://www.wtfpl.net/ for more details.
//
// -END_PRATEEK_COPYRIGHT-

// -BEGIN_PRATEEK_CSHARP_IFDEF-
//
//-----------------------------------------------------------------------------
#region C# Prateek Namespaces

//Auto activate some of the prateek defines
#if UNITY_EDITOR

#if !PRATEEK_DEBUG
#define PRATEEK_DEBUG
#endif //!PRATEEK_DEBUG

#endif //UNITY_EDITOR && !PRATEEK_DEBUG

#endregion C# Prateek Namespaces
//
// -END_PRATEEK_CSHARP_IFDEF-

//-----------------------------------------------------------------------------
#region File namespaces
using System.IO;
using Prateek.IO;
using System.Text.RegularExpressions;
#endregion File namespaces

//-----------------------------------------------------------------------------
namespace Prateek.CodeGeneration
{
    using System.Collections.Generic;
    using UnityEditor;

    //-------------------------------------------------------------------------
#if UNITY_EDITOR
    //todo: fix that [InitializeOnLoad]
    class SwizzleRuleLoader : PrateekScriptBuilder
    {
        static SwizzleRuleLoader()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
                return;

            NewSwizzle(Tag.importExtension).Commit();
        }
    }
#endif //UNITY_EDITOR

    //-------------------------------------------------------------------------
    public partial class PrateekScriptBuilder
    {
        //---------------------------------------------------------------------
        protected static SwizzleCodeRule NewSwizzle(string extension)
        {
            return new SwizzleCodeRule(extension);
        }

        //---------------------------------------------------------------------
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
                        if (data.classDefaultExportOnly)
                        {
                            bool foundValid = false;
                            for (int exp = 0; exp < slots.Length; exp++)
                            {
                                if (slots[exp] >= infoSrc.VarCount)
                                {
                                    foundValid = true;
                                    break;
                                }
                            }

                            if (!foundValid)
                                continue;
                        }

                        var sn = 0;
                        var variant = new FuncVariant(string.Empty, 2);
                        variant[1] += Tag.Code.argsV;
                        for (int v = 0; v < slots.Length; v++)
                        {
                            var sv = slots[v];
                            if (sv < infoSrc.VarCount)
                            {
                                var name = sv < infoSrc.NameCount ? infoSrc.names[sv] : infoSrc.variables[sv];
                                variant.Call = name;
                                var variable = infoSrc.variables[sv];
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
