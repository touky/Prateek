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
    using static Prateek.ShaderTo.CSharp;

    //-------------------------------------------------------------------------
#if UNITY_EDITOR
    //todo: fix that [InitializeOnLoad]
    class BasicFuncRuleLoader : PrateekScriptBuilder
    {
        static BasicFuncRuleLoader()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
                return;

            NewBasicFunc(Tag.importExtension).Commit();
        }
    }
#endif //UNITY_EDITOR

    //-------------------------------------------------------------------------
    public partial class PrateekScriptBuilder
    {
        //---------------------------------------------------------------------
        protected static BasicFuncCodeRule NewBasicFunc(string extension)
        {
            return new BasicFuncCodeRule(extension);
        }

        //---------------------------------------------------------------------
        public partial class BasicFuncCodeRule : CodeRule
        {
            //-----------------------------------------------------------------
            public override string ScopeTag { get { return "FUNC_BASIC"; } }
            public override GenerationMode GenMode { get { return GenerationMode.ForeachSrc; } }
            public override bool GenerateDefault { get { return false; } }

            //-----------------------------------------------------------------
            public BasicFuncCodeRule(string extension) : base(extension) { }

            //-----------------------------------------------------------------
            #region Rule internal
            protected override void GatherVariants(List<FuncVariant> variants, CodeFile.ContentInfos data, CodeFile.ClassInfos infoSrc, CodeFile.ClassInfos infoDst)
            {
                variants.Clear();
                if (data.funcInfos.Count == 0)
                {
                    variants.Add(new FuncVariant());
                }
                else
                {
                    var variant = new FuncVariant(string.Empty, data.funcInfos.Count - 1);
                    for (int d = 0; d < data.funcInfos.Count; d++)
                    {
                        var funcInfo = data.funcInfos[d];
                        var varsCount = Vars.GetCount(funcInfo.data);
                        for (int v = 0; v < infoSrc.variables.Count; v++)
                        {
                            var funcData = funcInfo.data;
                            for (int n = 0; n < min(varsCount, Vars.Count); n++)
                            {
                                var vars = (Vars[n] + infoSrc.variables[v]);
                                funcData = vars.Apply(funcData);
                            }

                            variant[d] = funcData;
                        }
                    }
                    variants.Add(variant);
                }
            }
            #endregion Rule internal
        }
    }
}
