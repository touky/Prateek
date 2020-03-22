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
//-----------------------------------------------------------------------------
#region Prateek Ifdefs

//Auto activate some of the prateek defines
#if UNITY_EDITOR

//Auto activate debug
#if !PRATEEK_DEBUG
#define PRATEEK_DEBUG
#endif //!PRATEEK_DEBUG

#endif //UNITY_EDITOR && !PRATEEK_DEBUG

#endregion Prateek Ifdefs
// -END_PRATEEK_CSHARP_IFDEF-

//-----------------------------------------------------------------------------
namespace Prateek.CodeGenerator.PrateekScriptBuilder
{
    using System.Collections.Generic;
    using UnityEditor;

    //-------------------------------------------------------------------------
#if UNITY_EDITOR
    //todo: fix that [InitializeOnLoad]
    class MixedCTorRuleLoader : PrateekScriptBuilder
    {
        static MixedCTorRuleLoader()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
                return;

            NewMixedCTor(Tag.importExtension).Commit();
        }
    }
#endif //UNITY_EDITOR

    //-------------------------------------------------------------------------
    public partial class PrateekScriptBuilder
    {
        //---------------------------------------------------------------------
        protected static MixedCTorCodeRule NewMixedCTor(string extension)
        {
            return new MixedCTorCodeRule(extension);
        }

        //---------------------------------------------------------------------
        public partial class MixedCTorCodeRule : CodeRule
        {
            //-----------------------------------------------------------------
            public override string ScopeTag { get { return "CTOR_MIXED"; } }
            public override GenerationMode GenMode { get { return GenerationMode.ForeachSrc; } }

            //-----------------------------------------------------------------
            public MixedCTorCodeRule(string extension) : base(extension) { }

            //-----------------------------------------------------------------
            #region Rule internal
            protected override void GatherVariants(List<FuncVariant> variants, CodeFile.ContentInfos data, CodeFile.ClassInfos infoSrc, CodeFile.ClassInfos infoDst)
            {
                var slots = new int[infoSrc.VarCount];
                for (int s = 0; s < slots.Length; s++)
                {
                    slots[s] = 0;
                }

                variants.Clear();
                GatherVariants(0, slots, slots.Length, variants, data, infoSrc);

                //Add Default vec(f)
                var variant = new FuncVariant(infoSrc.names[0], 2);
                variant[1] = string.Format(Tag.Code.argsN, data.classDefaultType, 0);
                for (int v = 0; v < infoSrc.VarCount; v++)
                {
                    variant[2] = string.Format(Tag.Code.varsN, 0);
                }
                variants.Add(variant);
            }

            //-----------------------------------------------------------------
            private void GatherVariants(int s, int[] slots, int count, List<FuncVariant> variants, CodeFile.ContentInfos data, CodeFile.ClassInfos infoSrc)
            {
                var classCount = data.classInfos.Count + 1;
                for (int c = 0; c < classCount; c++)
                {
                    var varCount = c == 0 ? 1 : data.classInfos[c - 1].VarCount;
                    slots[s] = c;
                    if (count - varCount > 0)
                    {
                        GatherVariants(s + 1, slots, count - varCount, variants, data, infoSrc);
                    }
                    else if (count - varCount == 0)
                    {
                        var sn = 0;
                        var sv = 0;
                        var variant = new FuncVariant(infoSrc.names[0], 2);
                        for (int v = 0; v < slots.Length && v < s + 1; v++)
                        {
                            var sl = slots[v];
                            if (sl == 0)
                            {
                                variant[1] = string.Format(Tag.Code.argsN, data.classDefaultType, sn);
                                variant[2] = string.Format(Tag.Code.varsN, sn);
                                sn++;
                            }
                            else
                            {
                                sl -= 1;
                                var info = data.classInfos[sl];
                                variant[1] = string.Format(Tag.Code.argsV_, info.className, sv);
                                for (int vr = 0; vr < info.VarCount; vr++)
                                {
                                    variant[2] = string.Format(Tag.Code.varsV_, sv, info.variables[vr]);
                                }
                                sv++;
                            }
                        }

                        variants.Add(variant);
                    }
                }
            }
            #endregion Rule internal
        }
    }
}
