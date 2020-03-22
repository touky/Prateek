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
namespace Prateek.CodeGeneration
{
    using System;
    using System.Collections.Generic;
    using Prateek.Helpers;
    using UnityEditor;

    //-------------------------------------------------------------------------
#if UNITY_EDITOR
    //todo: fix that [InitializeOnLoad]
    class PrateekSyntaxNPPLoader : ScriptTemplate
    {
        static PrateekSyntaxNPPLoader()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
                return;

            NewScript(PrateekScriptBuilder.Tag.importExtension.Extension("xml"), "xml")
            .SetAutorun(false)
            .SetEndsWith("SyntaxAutoComplete")
            .SetTemplateFile(String.Empty)
            .SetFileContent("InternalContent_PrateekCodegenSyntaxAutoComplete.xml.txt")
            .Commit();

            NewScript(PrateekScriptBuilder.Tag.importExtension.Extension("xml"), "xml")
            .SetAutorun(false)
            .SetTemplateFile(String.Empty)
            .SetEndsWith("SyntaxColor")
            .SetFileContent("InternalContent_PrateekCodegenSyntaxColor.xml.txt")
            .Commit();
        }
    }

    //-------------------------------------------------------------------------
    //todo: fix that [InitializeOnLoad]
    class PrateekCodegenSyntaxRuleLoader : PrateekScriptBuilder
    {
        static PrateekCodegenSyntaxRuleLoader()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
                return;

            NewCodegenSyntaxColorFunc(Tag.importExtension).Commit();
            NewCodegenSyntaxAutoCompleteFunc(Tag.importExtension).Commit();
        }
    }
#endif //UNITY_EDITOR

    //-------------------------------------------------------------------------
    public partial class PrateekScriptBuilder
    {
        //---------------------------------------------------------------------
        protected static PrateekCodegenSyntaxColorCodeRule NewCodegenSyntaxColorFunc(string extension)
        {
            return new PrateekCodegenSyntaxColorCodeRule(extension);
        }

        //---------------------------------------------------------------------
        public partial class PrateekCodegenSyntaxColorCodeRule : SyntaxCodeRule
        {
            //-----------------------------------------------------------------
            private List<string> keywords = new List<string>();
            private List<string> identifiers = new List<string>();

            //-----------------------------------------------------------------
            public override string ScopeTag { get { return "SYNTAX_NPP_COLOR"; } }
            public override GenerationMode GenMode { get { return GenerationMode.ForeachSrc; } }
            public override bool GenerateDefault { get { return false; } }

            //-----------------------------------------------------------------
            public PrateekCodegenSyntaxColorCodeRule(string extension) : base(extension) { }

            //-----------------------------------------------------------------
            #region CodeRule override
            public override void AddKeyword(string content)
            {
                keywords.Add(content);
            }

            //-----------------------------------------------------------------
            public override void AddIdentifier(string content)
            {
                identifiers.Add(content);
            }
            #endregion CodeRule override

            //-----------------------------------------------------------------
            #region Rule internal
            protected override void GatherVariants(List<FuncVariant> variants, CodeFile.ContentInfos data, CodeFile.ClassInfos infoSrc, CodeFile.ClassInfos infoDst)
            {
                keywords.Clear();
                identifiers.Clear();
                Tag.Macro.GetTags(this);

                variants.Clear();
                if (data.funcInfos.Count == 0)
                    return;

                var result = string.Empty;
                for (int k = 0; k < keywords.Count; k++)
                {
                    result += data.funcInfos[0].data;
                    result = (Vars[0] + keywords[k]).Apply(result);
                }

                var variant = new FuncVariant(result, 1);
                result = string.Empty;
                for (int i = 0; i < identifiers.Count; i++)
                {
                    result += data.funcInfos[0].data;
                    result = (Vars[0] + identifiers[i]).Apply(result);
                }
                variant[1] = result;
                variants.Add(variant);
            }
            #endregion Rule internal
        }

        //---------------------------------------------------------------------
        protected static PrateekCodegenSyntaxAutoCompleteCodeRule NewCodegenSyntaxAutoCompleteFunc(string extension)
        {
            return new PrateekCodegenSyntaxAutoCompleteCodeRule(extension);
        }

        //---------------------------------------------------------------------
        public partial class PrateekCodegenSyntaxAutoCompleteCodeRule : SyntaxCodeRule
        {
            //-----------------------------------------------------------------
            public struct KeywordInfo
            {
                public string name;
            }

            //-----------------------------------------------------------------
            public List<KeywordInfo> infos = new List<KeywordInfo>();

            //-----------------------------------------------------------------
            public override string ScopeTag { get { return "SYNTAX_NPP_AUTO_COMPLETE"; } }
            public override GenerationMode GenMode { get { return GenerationMode.ForeachSrc; } }
            public override bool GenerateDefault { get { return false; } }

            //-----------------------------------------------------------------
            public PrateekCodegenSyntaxAutoCompleteCodeRule(string extension) : base(extension) { }

            //-----------------------------------------------------------------
            #region CodeRule override
            public override void AddKeyword(string content)
            {
                infos.Add(new KeywordInfo() { name = content });
            }

            //-----------------------------------------------------------------
            public override void AddIdentifier(string content)
            {
                infos.Add(new KeywordInfo() { name = content });
            }
            #endregion CodeRule override

            //-----------------------------------------------------------------
            #region Rule internal
            protected override void GatherVariants(List<FuncVariant> variants, CodeFile.ContentInfos data, CodeFile.ClassInfos infoSrc, CodeFile.ClassInfos infoDst)
            {
                infos.Clear();
                Tag.Macro.GetTags(this);

                variants.Clear();
                if (data.funcInfos.Count == 0)
                    return;

                var result = string.Empty;
                var variant = new FuncVariant(result);
                for (int k = 0; k < infos.Count; k++)
                {
                    result += data.funcInfos[0].data;
                    result = (Vars[0] + infos[k].name).Apply(result);
                }
                variant[0] = result;
                variants.Add(variant);
            }
            #endregion Rule internal
        }
    }
}
