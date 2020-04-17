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
            if (true)
            {
                var symbols = analyzer.FindAllSymbols();
                for (int s = 0; s < symbols.Count; s++)
                {
                    var symbol = symbols[s];
                    if (symbol is IComment)
                    {
                        continue;
                    }

                    if (!(symbol is Keyword keyword))
                    {
                        return BuildResult.ValueType.LoadingFailed;
                    }

                    if (RetrieveCodeFile(keyword, analyzer, symbols, ref s, ref activeCodeFile, codeFiles, args) < 0)
                    {
                        return BuildResult.ValueType.LoadingFailed;
                    }

                    var rules = ScriptActionDatabase.Actions;
                    for (var r = 0; r < rules.Count; r++)
                    {
                        var rule = rules[r];
                        if (!activeCodeFile.AllowRule(rule))
                        {
                            continue;
                        }

                        var keyRule = rule.GetKeyRule(keyword.Content, activeScope);
                        if (keyRule.usage != Utils.KeyRule.Usage.Match)
                        {
                            continue;
                        }
                    }
                }

                return BuildResult.ValueType.LoadingFailed;
            }
            else
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

                                var keyRule = rule.GetKeyRule(keyword, activeScope);
                                if (keyRule.usage != Utils.KeyRule.Usage.Match)
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
                                if (activeCodeFile.ActiveData != null)
                                {
                                    if (activeCodeFile.ActiveData.activeRule == null)
                                    {
                                        break;
                                    }

                                    if (activeCodeFile.ActiveData.activeRule.CloseScope(activeCodeFile, scopeName))
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
                var ext     = newData.source.extension.Extension(codeFile.fileExtension);
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

        //---------------------------------------------------------------------
        private int RetrieveCodeFile(string scope, string keyword, Analyzer analyzer, ref CodeFile activeCodeFile, List<CodeFile> codeFiles, List<string> args)
        {
            if (keyword == Tag.Macro.FileInfo)
            {
                if (scope != string.Empty)
                {
                    return -1;
                }

                var keyRule = new Utils.KeyRule(keyword, scope) {args = 2, needOpenScope = true};
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

        private int RetrieveCodeFile(Keyword keyword, Analyzer analyzer, List<Symbol> symbols, ref int symbolIndex, ref CodeFile activeCodeFile, List<CodeFile> codeFiles, List<string> args)
        {
            if (keyword.Content == Tag.Macro.FileInfo)
            {
                var keyRule = new Utils.KeyRule(keyword.Content, string.Empty) {args = 2, needOpenScope = true};
                if (!(symbols[++symbolIndex] is InvokeStartScope))
                {
                    return -1;
                }

                for (++symbolIndex; symbolIndex < symbols.Count; symbolIndex++)
                {
                    var symbol = symbols[symbolIndex];
                    if (symbol is VariableSeparator)
                    {
                        continue;
                    }

                    if (symbol is InvokeEndScope)
                    {
                        break;
                    }

                    if (!(symbol is Keyword arg))
                    {
                        return -1;
                    }

                    args.Add(arg.Content);
                }

                if (!(symbols[++symbolIndex] is CodeStartScope))
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
