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
    public partial class PrateekScriptBuilder : CodeBuilder
    {
        //---------------------------------------------------------------------
        public class Database : ScriptTemplate
        {
            #region Code rules
            private static List<CodeRule> rules = new List<PrateekScriptBuilder.CodeRule>();
            public static Group<CodeRule> CodeRules { get { return new Group<PrateekScriptBuilder.CodeRule>(rules); } }
            public static void Add(CodeRule data) { rules.Add(data); }
            #endregion Code rules
        }

        //---------------------------------------------------------------------
        protected override string SearchPattern { get { return FileHelpers.BuildExtensionMatch(Tag.importExtension); } }

        //---------------------------------------------------------------------
        protected override bool DoApplyValidTemplate(ref FileData fileData)
        {
            if (fileData.source.extension != Tag.importExtension)
                return true;

            base.DoApplyValidTemplate(ref fileData);

            var genStart = Tag.Macro.codeGenStart.Keyword();
            var startIndex = fileData.destination.content.IndexOf(genStart);
            if (startIndex < 0)
                return false;
            var genHeader = fileData.destination.content.Substring(0, startIndex);
            var genCode = fileData.destination.content.Substring(startIndex + genStart.Length);

            var analyzer = new Analyzer();
            var activeCodeFile = (CodeFile)null;
            var codeFiles = new List<CodeFile>();
            var codeDepth = 0;

            analyzer.Init(fileData.source.content);

            var args = new List<string>();
            var keyword = string.Empty;
            var data = String.Empty;
            while (analyzer.ShouldContinue)
            {
                if (analyzer.FindKeyword(ref keyword))
                {
                    var result = CheckGenericData(ref codeDepth, keyword, analyzer, ref activeCodeFile, codeFiles, args);
                    if (result < 1)
                    {
                        if (result < 0)
                            break;
                    }
                    else
                    {
                        var foundMatch = false;
                        var rules = Database.CodeRules;
                        for (int s = 0; s < rules.Count; s++)
                        {
                            var rule = rules[s];
                            if (!activeCodeFile.AllowRule(rule))
                                continue;

                            var setup = rule.GetKeyRule(keyword, codeDepth);
                            if (setup.usage != Utils.KeyRule.Usage.Match)
                                continue;

                            if (!analyzer.FindArgs(args, setup))
                                return false;

                            if (!analyzer.FindData(ref data, setup))
                                return false;

                            if (!rule.TreatData(activeCodeFile, setup, args, data))
                                return false;

                            foundMatch = true;
                            if (setup.needOpenScope)
                                codeDepth++;
                            break;
                        }

                        if (!foundMatch)
                            return false;
                    }
                }
                else
                {
                    var scopeName = string.Empty;
                    if (analyzer.FindScopeEnd(ref scopeName))
                    {
                        if (activeCodeFile != null)
                        {
                            if (activeCodeFile.ActiveData != null)
                            {
                                if (activeCodeFile.ActiveData.activeRule == null)
                                    break;

                                if (activeCodeFile.ActiveData.activeRule.CloseScope(activeCodeFile, scopeName))
                                    codeDepth--;
                            }
                            else if (codeDepth == 1 && scopeName == Tag.Macro.FileInfo)
                            {
                                activeCodeFile = null;
                                codeDepth--;
                            }
                        }
                    }
                }
            }

            //code files have been filled
            for (int f = 0; f < codeFiles.Count; f++)
            {
                var codeFile = codeFiles[f];
                //{ // Log shit
                //    var log = String.Format("FOUND: {0}.{1}\n", codeFile.fileName, codeFile.fileExtension);
                //    for (int d = 0; d < codeFile.DataCount; d++)
                //    {
                //        var codeData = codeFile[d];
                //        for (int i = 0; i < codeData.classInfos.Count; i++)
                //        {
                //            var info = codeData.classInfos[i];
                //            log += String.Format("  - CLASS: {0} ", info.name);
                //            for (int v = 0; v < info.variables.Count; v++)
                //            {
                //                log += " " + info.variables[v];
                //            }
                //            log += "\n";
                //        }

                //        log += String.Format("  - TYPE: {0} = {1}\n", codeData.classContentType, codeData.classContentValue);
                //        log += String.Format("  - CODE PREFIX:\n > {0}\n", codeData.codePrefix.Replace("\n", "\n> "));
                //        log += String.Format("  - CODE MAIN:\n > {0}\n", codeData.codeMain.Replace("\n", "\n> "));
                //        log += String.Format("  - CODE POSTFIX:\n > {0}\n", codeData.codePostfix.Replace("\n", "\n> "));
                //    }
                //    UnityEngine.Debug.Log(log);
                //}

                // Build the actual code
                codeFile.Generate(genHeader, genCode);

                var newData = fileData;
                var swap = new Utils.SwapInfo(newData.destination.name.Extension(newData.destination.extension)) + codeFile.fileName.Extension(codeFile.fileExtension);
                newData.destination.name = codeFile.fileName;
                newData.destination.extension = codeFile.fileExtension;
                newData.destination.absPath = swap.Apply(newData.destination.absPath);
                newData.destination.relPath = swap.Apply(newData.destination.relPath);
                newData.destination.content = codeFile.CodeGenerated;
                newData.source = newData.destination;
                AddWorkFile(newData);

                //{ // Log shit
                //    for (int i = 0; i < codeFile.DataCount; i++)
                //    {
                //        var code = codeFile[i];
                //        UnityEngine.Debug.Log(code.codeGenerated);
                //    }

                //    UnityEngine.Debug.Log(codeFile.CodeGenerated);
                //}
            }

            return false;
        }

        //---------------------------------------------------------------------
        private int CheckGenericData(ref int codeDepth, string keyword, Analyzer analyzer, ref CodeFile activeCodeFile, List<CodeFile> codeFiles, List<string> args)
        {
            if (keyword == Tag.Macro.FileInfo)
            {
                if (codeDepth != 0)
                    return -1;

                var keyRule = new Utils.KeyRule(keyword, true) { args = 2, needOpenScope = true };
                if (!analyzer.FindArgs(args, keyRule))
                    return -1;

                activeCodeFile = codeFiles.Find((x) => { return x.fileName == args[0] && x.fileExtension == args[1]; });
                if (activeCodeFile == null)
                {
                    activeCodeFile = new CodeFile() { fileName = args[0], fileExtension = args[1] };
                    codeFiles.Add(activeCodeFile);
                }

                codeDepth++;

                return 0;
            }

            return 1;
        }
    }
}
