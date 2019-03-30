// -BEGIN_PRATEEK_COPYRIGHT-
//
//  Prateek, a library that is "bien pratique"
//  Header last update date: 30/03/2019
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
using Prateek.Manager;

#region Using static
using static Prateek.ShaderTo.CSharp;
#endregion Using static

#region Editor
#if UNITY_EDITOR
using Prateek.CodeGeneration;
#endif //UNITY_EDITOR
#endregion Editor

#if PRATEEK_DEBUG
using Prateek.Debug;
using static Prateek.Debug.Draw.Setup.QuickCTor;
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
    class PrateekSyntaxNPPLoader : ScriptTemplate
    {
        static PrateekSyntaxNPPLoader()
        {
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
    [InitializeOnLoad]
    class PrateekCodegenSyntaxRuleLoader : PrateekScriptBuilder
    {
        static PrateekCodegenSyntaxRuleLoader()
        {
            NewCodegenSyntaxColorFunc(Tag.importExtension).Commit();
            NewCodegenSyntaxAutoCompleteFunc(Tag.importExtension).Commit();
        }
    }

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
