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
namespace Prateek.CodeGenerator.PrateekScriptBuilder
{
    using System.Collections.Generic;
    using Assets.Prateek.CodeGenerator.Code.PrateekScriptBuilder.CodeAnalyzer;
    using Prateek.Core.Code.Helpers;
    using Prateek.Core.Code.Helpers.Files;
    using static CodeBuilder;

    //-------------------------------------------------------------------------
    public partial class PrateekScriptBuilder : CodeGenerator.CodeBuilder
    {
        #region Properties
        //---------------------------------------------------------------------
        public override string SearchPattern
        {
            get { return FileHelpers.BuildExtensionMatch(Tag.importExtension); }
        }
        #endregion

        #region Class Methods
        //---------------------------------------------------------------------
        protected BuildResult Error(BuildResult result, ref FileData fileData)
        {
            return result + "in File: " + fileData.source.name.Extension(fileData.source.extension);
        }

        //---------------------------------------------------------------------
        protected override BuildResult DoApplyValidTemplate(ref FileData fileData)
        {
            if (fileData.source.extension != Tag.importExtension)
            {
                return BuildResult.ValueType.Success | BuildResult.ValueType.Ignored;
            }

            var analyzer       = new Analyzer();
            var activeCodeFile = (CodeFile) null;
            var activeScope    = string.Empty;
            var codeFiles      = new List<CodeFile>();
            var codeDepth      = 0;
            var args           = new List<string>();

            analyzer.Init(fileData.source.content);
#if true
            {
                analyzer.FindAllSymbols();
                analyzer.BuildCodeCommands();

                var rootScope = analyzer.ContentRootScope;
                foreach (var codeCommand in rootScope.commands)
                {
                    if (!(codeCommand is CodeKeyword codeKeyword))
                    {
                        continue;
                    }

                    var codeFile = RetrieveCodeFile(analyzer, codeKeyword, codeFiles);
                    if (codeFile == null)
                    {
                        continue;
                    }

                    FeedCodeFile(ref fileData, codeFile, codeKeyword, string.Empty);
                }

                //return BuildResult.ValueType.LoadingFailed;
            }
#else
            {
                var keyword = string.Empty;
                var data    = string.Empty;
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
                        var dataResult = RetrieveCodeFile(activeScope, keyword, analyzer, ref activeCodeFile, codeFiles, args);
                        if (dataResult < 1)
                        {
                            if (dataResult < 0)
                            {
                                break;
                            }
                        }
                        else
                        {
                            var foundMatch = false;
                            var rules      = ScriptActionDatabase.Actions;
                            for (var s = 0; s < rules.Count; s++)
                            {
                                var rule = rules[s];
                                if (!activeCodeFile.AllowRule(rule))
                                {
                                    continue;
                                }

                                var keyRule = rule.GetKeywordRule(keyword, activeScope);
                                if (keyRule.usage != Utils.KeywordRule.Usage.Match)
                                {
                                    continue;
                                }

                                if (!analyzer.FindArgs(args, keyRule))
                                {
                                    return Error((BuildResult) BuildResult.ValueType.PrateekScriptArgNotFound + keyword, ref fileData);
                                }

                                if (!analyzer.FindData(ref data, keyRule))
                                {
                                    return Error((BuildResult) BuildResult.ValueType.PrateekScriptDataNotFound + keyword, ref fileData);
                                }

                                if (!rule.RetrieveRuleContent(activeCodeFile, keyRule, args, data))
                                {
                                    return Error((BuildResult) BuildResult.ValueType.PrateekScriptDataNotTreated + keyword, ref fileData);
                                }

                                foundMatch = true;
                                break;
                            }

                            if (!foundMatch)
                            {
                                return Error((BuildResult) BuildResult.ValueType.PrateekScriptInvalidKeyword + keyword, ref fileData);
                            }
                        }
                    }
                    else
                    {
                        var scopeName = string.Empty;
                        if (analyzer.FindScopeEnd(ref scopeName))
                        {
                            if (activeCodeFile != null)
                            {
                                if (activeCodeFile.CodeInfos != null)
                                {
                                    if (activeCodeFile.CodeInfos.activeRule == null)
                                    {
                                        break;
                                    }

                                    if (activeCodeFile.CodeInfos.activeRule.CloseScope(activeCodeFile, scopeName))
                                    {
                                        codeDepth--;
                                    }
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
            }
#endif
            //code files have been filled
            for (var f = 0; f < codeFiles.Count; f++)
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

                newData.destination.name = codeFile.fileName.Extension(newData.source.extension);
                newData.destination.extension = codeFile.fileExtension;
                newData.source.extension = newData.source.extension.Extension(codeFile.fileExtension);
                
                var applyResult = base.DoApplyValidTemplate(ref newData);
                if (applyResult.Is(BuildResult.ValueType.NoMatchingTemplate))
                {
                    return Error(applyResult, ref newData);
                }

                var genStart   = Tag.Macro.codeGenStart.Keyword();
                var startIndex = newData.destination.content.IndexOf(genStart);
                if (startIndex < 0)
                {
                    return Error(BuildResult.ValueType.PrateekScriptSourceStartTagInvalid, ref newData);
                }

                var genHeader = newData.destination.content.Substring(0, startIndex);
                var genCode   = newData.destination.content.Substring(startIndex + genStart.Length);

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

        private BuildResult FeedCodeFile(ref FileData fileData, CodeFile codeFile, CodeKeyword rootKeyword, string rootScope)
        {
            var scriptActions = ScriptActionDatabase.Actions;
            var scopeRule = (Utils.KeywordRule)default;
            for (var s = 0; s < scriptActions.Count; s++)
            {
                var action = scriptActions[s];
                if (!codeFile.AllowRule(action))
                {
                    continue;
                }

                var keywordRule = action.GetKeywordRule(rootKeyword, rootScope);
                if (keywordRule.usage == Utils.KeywordRule.Usage.None
                 || keywordRule.usage == Utils.KeywordRule.Usage.Ignore)
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

            var scopes = rootKeyword.scopeContent != null ? new List<CodeScope>() {rootKeyword.scopeContent} : null;
            for (int s = 0; scopes != null && s < scopes.Count; s++)
            {
                var scope = scopes[s];
                foreach (var codeCommand in scope.commands)
                {
                    if (!(codeCommand is CodeKeyword innerKeyword))
                    {
                        continue;
                    }

                    FeedCodeFile(ref fileData, codeFile, innerKeyword, rootKeyword.keyword.Content);
                }

                if (scope.scopeContent.Count == 0)
                {
                    continue;
                }

                scopes.InsertRange(s + 1, scope.scopeContent);
            }

            if (scopeRule.usage != Utils.KeywordRule.Usage.None && scopeRule.onCloseScope != null)
            {
                scopeRule.onCloseScope(codeFile, rootKeyword.keyword.Content);
            }

            return BuildResult.ValueType.Success;
        }

        //---------------------------------------------------------------------
        private CodeFile RetrieveCodeFile(Analyzer analyzer, CodeKeyword codeKeyword, List<CodeFile> codeFiles)
        {
            var fileInfoRule = new Utils.KeywordRule(Tag.Macro.FileInfo, string.Empty) {arguments = 2, needOpenScope = true};
            if (!fileInfoRule.ValidateRule(codeKeyword, string.Empty))
            {
                return null;
            }

            var arg0 = codeKeyword.arguments[0].Content;
            var arg1 = codeKeyword.arguments[1].Content;
            var codeFile = codeFiles.Find(x => { return x.fileName == arg0 && x.fileExtension == arg1; });
            if (codeFile == null)
            {
                codeFile = new CodeFile {fileName = arg0, fileExtension = arg1};
                codeFiles.Add(codeFile);
            }

            return codeFile;
        }

        private int RetrieveCodeFile(string scope, string keyword, Analyzer analyzer, ref CodeFile activeCodeFile, List<CodeFile> codeFiles, List<string> args)
        {
            if (keyword == Tag.Macro.FileInfo)
            {
                if (scope != string.Empty)
                {
                    return -1;
                }

                var keyRule = new Utils.KeywordRule(keyword, scope) {arguments = 2, needOpenScope = true};
                if (!analyzer.FindArgs(args, keyRule))
                {
                    return -1;
                }

                activeCodeFile = codeFiles.Find(x => { return x.fileName == args[0] && x.fileExtension == args[1]; });
                if (activeCodeFile == null)
                {
                    activeCodeFile = new CodeFile {fileName = args[0], fileExtension = args[1]};
                    codeFiles.Add(activeCodeFile);
                }

                return 0;
            }

            return 1;
        }
        #endregion
    }
}
