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
    public partial class CodeBuilder
    {
        //---------------------------------------------------------------------
        public class CodeFile
        {
            //-----------------------------------------------------------------
            public struct ClassInfos
            {
                public List<string> names;
                public List<string> variables;
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
            public bool Submit()
            {
                var hasSubmitted = activeData != null;
                if (activeData != null)
                    datas.Add(activeData);
                activeData = null;

                return hasSubmitted;
            }

            //-----------------------------------------------------------------
            public bool Generate(string genHeader, string genCode)
            {
                var genNSpc = (Code.Tag.SwapInfo)Code.Tag.Macro.codeGenNSpc.Keyword();
                var genExtn = (Code.Tag.SwapInfo)Code.Tag.Macro.codeGenExtn.Keyword();
                var genData = (Code.Tag.SwapInfo)Code.Tag.Macro.codeGenData.Keyword();
                var genTabs = (Code.Tag.SwapInfo)Code.Tag.Macro.codeGenTabs.Keyword();

                var i = genCode.IndexOf(genData.Original);
                if (i < 0)
                    return false;

                var r = genCode.LastIndexOf(Strings.Separator.LineFeed.C(), i);
                if (r >= 0)
                    genTabs = genTabs + genCode.Substring(r + 1, i - (r + 1));

                codeGenerated = genHeader;
                for (int d = 0; d < datas.Count; d++)
                {
                    var data = datas[d];

                    data.activeRule.Generate(data);

                    var code = genTabs.Apply(data.codeGenerated);
                    genNSpc += data.blockNamespace;
                    genExtn += data.blockClassName;
                    genData += code;
                    codeGenerated += genExtn.Apply(genData.Apply(genNSpc.Apply(genCode)));
                }

                return true;
            }
        }
    }
}