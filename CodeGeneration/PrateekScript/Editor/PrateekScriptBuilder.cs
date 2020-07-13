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
namespace Prateek.CodeGeneration.Code.PrateekScript
{
    using Prateek.CodeGeneration.CodeBuilder.Editor.CodeBuilder;
    using System.Collections.Generic;
    using Prateek.CodeGeneration.Code.PrateekScript.CodeGeneration;
    using Prateek.CodeGeneration.Code.PrateekScript.ScriptAnalysis;
    using Prateek.CodeGeneration.Code.PrateekScript.ScriptAnalysis.IntermediateCode;
    using Prateek.CodeGeneration.Code.PrateekScript.ScriptAnalysis.Utils;
    using Prateek.CodeGeneration.PrateekScript.Editor.ScriptActions;
    using UnityEngine.Profiling;
    using Prateek.Core.Code.Helpers.Files;
    using Prateek.Core.Code.Helpers;
    using Prateek.Core.Code.Consts;

    ///-------------------------------------------------------------------------
    public class PrateekScriptBuilder : CodeBuilder
    {
        #region Properties
        ///---------------------------------------------------------------------
        private static PrateekScriptBuilder editorInstance = null;

        ///---------------------------------------------------------------------
        public static PrateekScriptBuilder GetInstance()
        {
            if (editorInstance == null)
            {
                editorInstance = new PrateekScriptBuilder();
            }

            return editorInstance;
        }

        ///---------------------------------------------------------------------
        public override string SearchPattern
        {
            get { return FileHelpers.BuildExtensionMatch(Glossary.importExtension); }
        }
        #endregion

        #region Class Methods
        ///---------------------------------------------------------------------
        protected BuildResult Error(BuildResult result, ref FileData fileData)
        {
            return result + "in File: " + fileData.source.name.Extension(fileData.source.extension);
        }

        ///---------------------------------------------------------------------
        protected override BuildResult DoApplyValidTemplate(ref FileData fileData)
        {
            Profiler.BeginSample($"DoApplyValidTemplate()");
            
            if (fileData.source.extension != Glossary.importExtension)
            {
                Profiler.EndSample();
                return BuildResult.ValueType.Success | BuildResult.ValueType.Ignored;
            }

            var analyzer       = new ScriptAnalyzer();
            var activeCodeFile = (CodeFile) null;
            var activeScope    = string.Empty;
            var codeFiles      = new List<CodeFile>();
            var codeDepth      = 0;
            var args           = new List<string>();

            analyzer.Init(fileData.source.content);
            {
                analyzer.FindAllSymbols();
                analyzer.BuildCodeCommands();

                var rootScope = analyzer.ContentRootScope;
                foreach (var codeCommand in rootScope.commands)
                {
                    Profiler.BeginSample($"foreach (var codeCommand in rootScope.commands)");

                    if (!(codeCommand is CodeKeyword codeKeyword))
                    {
                        Profiler.EndSample();
                        continue;
                    }

                    var codeFile = RetrieveCodeFile(codeKeyword, codeFiles);
                    if (codeFile == null)
                    {
                        Profiler.EndSample();
                        continue;
                    }

                    var result = FeedCodeFile(ref fileData, codeFile, codeKeyword, string.Empty);

                    Profiler.EndSample();
                    if (!result)
                    {
                        return Error(result, ref fileData);
                    }
                }

                //return BuildResult.ValueType.LoadingFailed;
            }

            //code files have been filled
            for (var f = 0; f < codeFiles.Count; f++)
            {
                var codeFile = codeFiles[f];

                Profiler.BeginSample($"for (var f = 0; f < codeFiles.Count; f++)");

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

                newData.destination.name = codeFile.fileName.Extension(newData.source.extension);
                newData.destination.extension = codeFile.fileExtension;
                newData.source.extension = newData.source.extension.Extension(codeFile.fileExtension);

                var applyResult = base.DoApplyValidTemplate(ref newData);
                if (applyResult.Is(BuildResult.ValueType.NoMatchingTemplate))
                {
                    Profiler.EndSample();
                    Profiler.EndSample();
                    return Error(applyResult, ref newData);
                }

                var genStart   = Glossary.Macros.scriptStartTag;
                var startIndex = newData.destination.content.IndexOf(genStart);
                if (startIndex < 0)
                {
                    Profiler.EndSample();
                    Profiler.EndSample();
                    return Error(BuildResult.ValueType.PrateekScriptSourceStartTagInvalid, ref newData);
                }


                var genHeader = newData.destination.content.Substring(0, startIndex);
                var genCode   = newData.destination.content.Substring(startIndex + genStart.Length);

                // Build the actual code
                var result = codeFile.Generate(genHeader, genCode);
                if (!result)
                {
                    Profiler.EndSample();
                    Profiler.EndSample();
                    return Error(result, ref newData);
                }

                foreach (var codeGenerated in codeFile.CodeGenerated)
                {
                    Profiler.BeginSample($"foreach (var codeGenerated in codeFile.CodeGenerated)");

                    var workData = newData;
                    if (!string.IsNullOrEmpty(codeGenerated.className))
                    {
                        workData.destination.name = codeGenerated.className + workData.destination.name;
                    }

                    workData.destination.content = codeGenerated.code;
                    workData.destination.SetupFileInfo();

                    AddWorkFile(workData);

                    Profiler.EndSample();
                }

                //{ // Log shit
                //    for (int i = 0; i < codeFile.DataCount; i++)
                //    {
                //        var code = codeFile[i];
                //        UnityEngine.Debug.Log(code.codeGenerated);
                //    }

                //    UnityEngine.Debug.Log(codeFile.CodeGenerated);
                //}

                Profiler.EndSample();
            }

            Profiler.EndSample();

            return BuildResult.ValueType.Ignored;
        }

        private BuildResult FeedCodeFile(ref FileData fileData, CodeFile codeFile, CodeKeyword rootKeyword, string rootScope)
        {
            var scriptActions = ScriptActionRegistry.Actions;
            bool foundOne = false;
            for (var s = 0; s < scriptActions.Count; s++)
            {
                var action = scriptActions[s];
                if (action.ExistInVocabulary(rootKeyword))
                {
                    foundOne = true;
                    break;
                }
            }

            if (!foundOne)
            {
                return Error((BuildResult) BuildResult.ValueType.PrateekScriptInvalidKeyword + rootKeyword.keyword.Content, ref fileData);
            }

            var scopeRule     = (KeywordUsage) default;
            for (var s = 0; s < scriptActions.Count; s++)
            {
                var action = scriptActions[s];
                if (!codeFile.AllowRule(action))
                {
                    continue;
                }

                var keywordRule = action.GetKeywordUsage(rootKeyword, rootScope);
                if (keywordRule.keywordUsageType == KeywordUsageType.None
                 || keywordRule.keywordUsageType == KeywordUsageType.Ignore)
                {
                    continue;
                }

                if (!action.FeedCodeFile(codeFile, keywordRule, rootKeyword))
                {
                    return Error((BuildResult) BuildResult.ValueType.PrateekScriptDataNotTreated + rootKeyword.keyword.Content, ref fileData);
                }

                scopeRule = keywordRule;
                break;
            }

            var scopes = rootKeyword.scopeContent != null ? new List<CodeScope> {rootKeyword.scopeContent} : null;
            for (var s = 0; scopes != null && s < scopes.Count; s++)
            {
                var scope = scopes[s];
                foreach (var codeCommand in scope.commands)
                {
                    if (!(codeCommand is CodeKeyword innerKeyword))
                    {
                        continue;
                    }

                    var buildResult = FeedCodeFile(ref fileData, codeFile, innerKeyword, rootKeyword.keyword.Content);
                    if (buildResult.Value != BuildResult.ValueType.Success)
                    {
                        return buildResult;
                    }
                }

                if (scope.scopeContent.Count == 0)
                {
                    continue;
                }

                scopes.InsertRange(s + 1, scope.scopeContent);
            }

            if (scopeRule.keywordUsageType != KeywordUsageType.None && scopeRule.onCloseScope != null)
            {
                scopeRule.onCloseScope(codeFile, rootKeyword.keyword.Content);
            }

            return BuildResult.ValueType.Success;
        }

        ///---------------------------------------------------------------------
        private CodeFile RetrieveCodeFile(CodeKeyword codeKeyword, List<CodeFile> codeFiles)
        {
            var fileInfoRule = KeywordCreator.GetFileInfos();
            if (!fileInfoRule.ValidateRule(codeKeyword, string.Empty))
            {
                return null;
            }

            var arg0     = codeKeyword.arguments[0].Content;
            var arg1     = codeKeyword.arguments[1].Content;
            var codeFile = codeFiles.Find(x => { return x.fileName == arg0 && x.fileExtension == arg1; });
            if (codeFile == null)
            {
                codeFile = new CodeFile {fileName = arg0, fileExtension = arg1};
                codeFiles.Add(codeFile);
            }

            return codeFile;
        }
        #endregion
    }
}
