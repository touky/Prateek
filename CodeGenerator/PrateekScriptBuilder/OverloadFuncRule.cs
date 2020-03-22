// -BEGIN_PRATEEK_COPYRIGHT-// -BEGIN_PRATEEK_COPYRIGHT-
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
// -END_PRATEEK_COPYRIGHT-// -END_PRATEEK_COPYRIGHT-// -END_PRATEEK_COPYRIGHT-

// -BEGIN_PRATEEK_CSHARP_NAMESPACE-// -BEGIN_PRATEEK_CSHARP_NAMESPACE-
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
// -END_PRATEEK_CSHARP_NAMESPACE-// -END_PRATEEK_CSHARP_NAMESPACE-// -END_PRATEEK_CSHARP_NAMESPACE-

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
    class OverloadFuncRuleLoader : PrateekScriptBuilder
    {
        static OverloadFuncRuleLoader()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
                return;

            NewOverloadFunc(Tag.importExtension).Commit();
        }
    }
#endif //UNITY_EDITOR

    //-------------------------------------------------------------------------
    public partial class PrateekScriptBuilder
    {
        //---------------------------------------------------------------------
        protected static OverloadFuncCodeRule NewOverloadFunc(string extension)
        {
            return new OverloadFuncCodeRule(extension);
        }

        //---------------------------------------------------------------------
        public partial class OverloadFuncCodeRule : CodeRule
        {
            //-----------------------------------------------------------------
            public override string ScopeTag { get { return "FUNC_OVERLOAD"; } }
            public override GenerationMode GenMode { get { return GenerationMode.ForeachSrc; } }
            public override bool GenerateDefault { get { return false; } }

            //-----------------------------------------------------------------
            public OverloadFuncCodeRule(string extension) : base(extension) { }

            //-----------------------------------------------------------------
            #region Rule internal
            protected override void GatherVariants(List<FuncVariant> variants, CodeFile.ContentInfos data, CodeFile.ClassInfos infoSrc, CodeFile.ClassInfos infoDst)
            {
                variants.Clear();
                var slots = new int[infoSrc.NameCount / 2];
                GatherVariants(0, slots, variants, data, infoSrc, infoDst);
            }
            //-----------------------------------------------------------------
            private void GatherVariants(int s, int[] slots, List<FuncVariant> variants, CodeFile.ContentInfos data, CodeFile.ClassInfos infoSrc, CodeFile.ClassInfos infoDst)
            {
                if (s < slots.Length)
                {
                    for (int p = 0; p < 2; p++)
                    {
                        slots[s] = p;
                        GatherVariants(s + 1, slots, variants, data, infoSrc, infoDst);
                    }
                }
                else
                {
                    var variant = new FuncVariant(string.Empty, 1);
                    for (int sv = 0; sv < slots.Length; sv++)
                    {
                        if (slots[sv] == 0)
                        {
                            for (int i = 0; i < data.funcInfos.Count; i++)
                            {
                                var info = data.funcInfos[i].data;
                                info = (Names[0] + infoSrc.names[sv * 2 + 0]).Apply(info);
                                info = (Names[1] + infoSrc.names[sv * 2 + 1]).Apply(info);
                                variant[i] = info;
                            }
                        }
                    }

                    if (variant.Call != string.Empty)
                        variants.Add(variant);
                }
            }
            #endregion Rule internal
        }
    }
}
