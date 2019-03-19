// -BEGIN_PRATEEK_COPYRIGHT-
//
//  Prateek, a library that is "bien pratique"
//  Header last update date: 18/03/19
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
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.Serialization;

#if UNITY_PROFILING
using UnityEngine.Profiling;
#endif //UNITY_PROFILING

using Unity.Jobs;
using Unity.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif //UNITY_EDITOR
#endregion Unity

#region Prateek
using Prateek;
using Prateek.Base;
using Prateek.Extensions;
using Prateek.Helpers;
using Prateek.Attributes;

#if UNITY_EDITOR
using Prateek.ScriptTemplating;
#endif //UNITY_EDITOR

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

    //---------------------------------------------------------------------
    public static partial class Code
    {
        //---------------------------------------------------------------------
        public class File
        {
            //-----------------------------------------------------------------
            public class Data
            {
                public struct ClassInfo
                {
                    public string name;
                    public List<string> variables;
                }

                public CodeSettings settings;
                public string blockName;

                public List<ClassInfo> classInfos = new List<ClassInfo>();
                public string classContentType;
                public string classContentValue;

                public string codePrefix;
                public string codeMain;
                public string codePostfix;

                public string codeGenerated;
            }

            //-----------------------------------------------------------------
            public string fileName;
            public string fileExtension;

            //-----------------------------------------------------------------
            private Data activeData;
            private string codeGenerated;
            private List<Data> datas = new List<Data>();

            //-----------------------------------------------------------------
            public string CodeGenerated { get { return codeGenerated; } }
            public Data ActiveData { get { return activeData; } }
            public int DataCount { get { return datas.Count; } }
            public Data this[int index] { get { return datas[index]; } }

            //-----------------------------------------------------------------
            public Data NewData(CodeSettings codeSettings)
            {
                if (activeData != null)
                    return null;
                activeData = new Data() { settings = codeSettings };
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
            public bool Generate()
            {
                var genHeader = Code.Tag.fileHeader.SimplifyNewLines().TabToSpaces();
                var genCode = Code.Tag.fileCode.SimplifyNewLines().TabToSpaces();
                var genExtn = (Code.Tag.SwapInfo)Code.Tag.Macro.codeGenExtn.Keyword();
                var genData = (Code.Tag.SwapInfo)Code.Tag.Macro.codeGenData.Keyword();
                var genTabs = (Code.Tag.SwapInfo)Code.Tag.Macro.codeGenTabs.Keyword();

                var i = genCode.IndexOf(genData.Original);
                if (i < 0)
                    return false;

                var r = genCode.LastIndexOf(Strings.Separator.NewLine.C()[0], i);
                if (r >= 0)
                    genTabs = genTabs + genCode.Substring(r + 1, i - (r + 1));

                codeGenerated = genHeader;
                for (int d = 0; d < datas.Count; d++)
                {
                    var data = datas[d];
                    data.settings.Generate(data);

                    var code = genTabs.Apply(data.codeGenerated);
                    genExtn += data.blockName;
                    genData += code;
                    codeGenerated += genExtn.Apply(genData.Apply(genCode));
                }

                TemplateHelpers.ApplyKeywords(ref codeGenerated, fileExtension);

                return true;
            }
        }
    }
}
