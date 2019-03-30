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
        public abstract class SyntaxCodeRule : CodeRule
        {
            public SyntaxCodeRule(string extension) : base(extension) { }

            public abstract void AddKeyword(string content);
            public abstract void AddIdentifier(string content);
        }

        //---------------------------------------------------------------------
        public override string SearchPattern { get { return FileHelpers.BuildExtensionMatch(Tag.importExtension); } }

        //---------------------------------------------------------------------
        protected BuildResult Error(BuildResult result, ref FileData fileData)
        {
            return result + "in File: " + fileData.source.name.Extension(fileData.source.extension);
        }

        //---------------------------------------------------------------------
        protected override BuildResult DoApplyValidTemplate(ref FileData fileData)
        {
            if (fileData.source.extension != Tag.importExtension)
                return BuildResult.ValueType.Success | BuildResult.ValueType.Ignored;

            var analyzer = new Analyzer();
            var activeCodeFile = (CodeFile)null;
            var activeScope = string.Empty;
            var codeFiles = new List<CodeFile>();
            var codeDepth = 0;

            analyzer.Init(fileData.source.content);

            var args = new List<string>();
            var keyword = string.Empty;
            var data = String.Empty;
            while (analyzer.ShouldContinue)
            {
                activeScope = analyzer.Scope;
                var keywordResult = analyzer.FindKeyword(ref keyword);
                if (!keywordResult && !keywordResult.Is(BuildResult.ValueType.Ignored))
                {
                    return Error(keywordResult, ref fileData);
                }

                if (keywordResult)
                {
                    var dataResult = CheckGenericData(activeScope, keyword, analyzer, ref activeCodeFile, codeFiles, args);
                    if (dataResult < 1)
                    {
                        if (dataResult < 0)
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

                            var keyRule = rule.GetKeyRule(keyword, activeScope);
                            if (keyRule.usage != Utils.KeyRule.Usage.Match)
                                continue;

                            if (!analyzer.FindArgs(args, keyRule))
                                return Error((BuildResult)BuildResult.ValueType.PrateekScriptArgNotFound + keyword, ref fileData);

                            if (!analyzer.FindData(ref data, keyRule))
                                return Error((BuildResult)BuildResult.ValueType.PrateekScriptDataNotFound + keyword, ref fileData);

                            if (!rule.RetrieveRuleContent(activeCodeFile, keyRule, args, data))
                                return Error((BuildResult)BuildResult.ValueType.PrateekScriptDataNotTreated + keyword, ref fileData);

                            foundMatch = true;
                            break;
                        }

                        if (!foundMatch)
                            return Error((BuildResult)BuildResult.ValueType.PrateekScriptInvalidKeyword + keyword, ref fileData);
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

                var newData = fileData;
                var ext = newData.source.extension.Extension(codeFile.fileExtension);
                var swap = new Utils.SwapInfo(newData.source.name.Extension(newData.source.extension))
                                            + codeFile.fileName.Extension(ext);
                newData.source.name = codeFile.fileName;
                newData.source.extension = ext;
                newData.source.absPath = swap.Apply(newData.source.absPath);
                newData.source.relPath = swap.Apply(newData.source.relPath);
                swap = new Utils.SwapInfo(newData.destination.name.Extension(newData.destination.extension)) + codeFile.fileName.Extension(codeFile.fileExtension);
                newData.destination.name = codeFile.fileName;
                newData.destination.extension = codeFile.fileExtension;
                newData.destination.absPath = swap.Apply(newData.destination.absPath);
                newData.destination.relPath = swap.Apply(newData.destination.relPath);

                var applyResult = base.DoApplyValidTemplate(ref newData);
                if (applyResult.Is(BuildResult.ValueType.NoMatchingTemplate))
                    return Error(applyResult, ref newData);

                var genStart = Tag.Macro.codeGenStart.Keyword();
                var startIndex = newData.destination.content.IndexOf(genStart);
                if (startIndex < 0)
                    return Error(BuildResult.ValueType.PrateekScriptSourceStartTagInvalid, ref newData);
                var genHeader = newData.destination.content.Substring(0, startIndex);
                var genCode = newData.destination.content.Substring(startIndex + genStart.Length);

                // Build the actual code
                var result = codeFile.Generate(genHeader, genCode);
                if (!result)
                {
                    return Error(result, ref newData);
                }

                newData.destination.content = codeFile.CodeGenerated;
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

            return BuildResult.ValueType.Ignored;
        }

        //---------------------------------------------------------------------
        private int CheckGenericData(string scope, string keyword, Analyzer analyzer, ref CodeFile activeCodeFile, List<CodeFile> codeFiles, List<string> args)
        {
            if (keyword == Tag.Macro.FileInfo)
            {
                if (scope != string.Empty)
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
                return 0;
            }

            return 1;
        }
    }
}
