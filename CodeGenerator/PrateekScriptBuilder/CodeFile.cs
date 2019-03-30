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
    public partial class PrateekScriptBuilder
    {
        //---------------------------------------------------------------------
        public class CodeFile
        {
            //-----------------------------------------------------------------
            public struct ClassInfos
            {
                //-----------------------------------------------------------------
                public string className;
                public List<string> names;
                public List<string> variables;

                //-----------------------------------------------------------------
                public int NameCount { get { return names == null ? 0 : names.Count; } }
                public int VarCount { get { return variables == null ? 0 : variables.Count; } }
            }

            //-----------------------------------------------------------------
            public struct FuncInfos
            {
                public string name;
                public string data;
            }

            //-----------------------------------------------------------------
            public class ContentInfos
            {
                public CodeRule activeRule;
                public string blockNamespace;
                public string blockClassName;
                public List<string> blockClassPrefix = new List<string>();

                public List<ClassInfos> classInfos = new List<ClassInfos>();
                public List<FuncInfos> funcInfos = new List<FuncInfos>();
                public string classDefaultType;
                public string classDefaultValue;

                public string codePrefix;
                public string codeMain;
                public string codePostfix;

                public string codeGenerated;
            }

            //-----------------------------------------------------------------
            public string fileName;
            public string fileExtension;
            public string fileNamespace;

            //-----------------------------------------------------------------
            private ContentInfos activeData;
            private string codeGenerated;
            private List<ContentInfos> datas = new List<ContentInfos>();

            //-----------------------------------------------------------------
            public string CodeGenerated { get { return codeGenerated; } }
            public ContentInfos ActiveData { get { return activeData; } }
            public int DataCount { get { return datas.Count; } }
            public ContentInfos this[int index] { get { return datas[index]; } }

            //-----------------------------------------------------------------
            public bool AllowRule(CodeRule rule)
            {
                if (activeData == null)
                    return true;

                if (activeData.activeRule == null)
                    return true;

                return activeData.activeRule == rule;
            }

            //-----------------------------------------------------------------
            public ContentInfos NewData(CodeRule codeSettings)
            {
                if (activeData != null)
                    return null;
                activeData = new ContentInfos() { activeRule = codeSettings };
                return activeData;
            }

            //-----------------------------------------------------------------
            public bool Commit()
            {
                var hasSubmitted = activeData != null;
                if (activeData != null)
                    datas.Add(activeData);
                activeData = null;

                return hasSubmitted;
            }

            //-----------------------------------------------------------------
            public BuildResult Generate(string genHeader, string genCode)
            {
                var genNSpc = (Utils.SwapInfo)Tag.Macro.codeGenNSpc.Keyword();
                var genExtn = (Utils.SwapInfo)Tag.Macro.codeGenExtn.Keyword();
                var genPrfx = (Utils.SwapInfo)Tag.Macro.codeGenPrfx.Keyword();
                var genData = (Utils.SwapInfo)Tag.Macro.codeGenData.Keyword();
                var genTabs = (Utils.SwapInfo)Tag.Macro.codeGenTabs.Keyword();


                var i = genCode.IndexOf(genData.Original);
                if (i < 0)
                    return BuildResult.ValueType.PrateekScriptSourceDataTagInvalid;

                var r = genCode.LastIndexOf(Strings.Separator.LineFeed.C(), i);
                if (r >= 0)
                    genTabs = genTabs + genCode.Substring(r + 1, i - (r + 1));

                codeGenerated = genHeader;
                for (int d = 0; d < datas.Count; d++)
                {
                    var data = datas[d];

                    var result = data.activeRule.Generate(data);
                    if (!result)
                    {
                        return result;
                    }

                    var code = genTabs.Apply(data.codeGenerated);
                    genNSpc += data.blockNamespace;
                    genExtn += data.blockClassName;
                    var prefix = string.Empty;
                    for (int p = 0; p < data.blockClassPrefix.Count; p++)
                    {
                        genPrfx += data.blockClassPrefix[p] + Strings.Separator.Space.S();
                    }
                    genData += code;
                    codeGenerated += genPrfx.Apply(genExtn.Apply(genData.Apply(genNSpc.Apply(genCode))));
                }

                return BuildResult.ValueType.Success;
            }
        }
    }
}
