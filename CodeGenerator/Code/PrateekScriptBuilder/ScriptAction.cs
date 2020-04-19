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
    using System;
    using System.Collections.Generic;
    using Assets.Prateek.CodeGenerator.Code.PrateekScriptBuilder.CodeAnalyzer;
    using Assets.Prateek.CodeGenerator.Code.PrateekScriptBuilder.CodeAnalyzer.Utils;
    using Assets.Prateek.CodeGenerator.Code.PrateekScriptBuilder.CodeCommands;
    using Assets.Prateek.CodeGenerator.Code.PrateekScriptBuilder.CodeGeneration;
    using Assets.Prateek.CodeGenerator.Code.Utils;
    using Prateek.Helpers;
    using Prateek.CodeGenerator;
    using Prateek.Core.Code.Helpers;

    //-------------------------------------------------------------------------
#if UNITY_EDITOR
#endif //UNITY_EDITOR

    public abstract class ScriptAction : ScriptTemplates.ScriptTemplate.BaseTemplate
    {
        protected List<KeywordUsage> keywordUsages = new List<KeywordUsage>();

        //-----------------------------------------------------------------

        //-----------------------------------------------------------------
        public enum GenerationMode
        {
            ForeachSrc,
            ForeachSrcXDest,

            MAX
        }

        //-----------------------------------------------------------------
        public abstract string ScopeTag { get; }
        public abstract GenerationMode GenMode { get; }
        public virtual bool GenerateDefault { get { return false; } }

        //-----------------------------------------------------------------
        private string codeBlock = String.Empty;

        //-----------------------------------------------------------------
        public string CodeBlock { get { return codeBlock; } }

        //-----------------------------------------------------------------
        public StringSwap ClassDst { get { return PrateekScriptBuilder.Tag.Macro.dstClass; } }
        public StringSwap ClassSrc { get { return PrateekScriptBuilder.Tag.Macro.srcClass; } }
        public PrateekScriptBuilder.Tag.NumberedVars Names { get { return PrateekScriptBuilder.Tag.Macro.Names; } }
        public PrateekScriptBuilder.Tag.NumberedVars Funcs { get { return PrateekScriptBuilder.Tag.Macro.Funcs; } }
        public PrateekScriptBuilder.Tag.NumberedVars Vars { get { return PrateekScriptBuilder.Tag.Macro.Vars; } }

        //-----------------------------------------------------------------
        protected ScriptAction(string extension) : base(extension)
        {
            Init();
        }

        //-----------------------------------------------------------------
        public override void Commit()
        {
            ScriptActionDatabase.Add(this);
        }

        //-----------------------------------------------------------------
        protected virtual void Init()
        {
            PrateekScriptBuilder.Tag.Macro.Init();

            codeBlock = String.Format("{0}_{1}_{2}", PrateekScriptBuilder.Tag.Macro.prefix, PrateekScriptBuilder.Tag.Macro.To(PrateekScriptBuilder.Tag.Macro.FuncName.BLOCK), ScopeTag);

            keywordUsages.Add(new KeywordUsage(codeBlock, PrateekScriptBuilder.Tag.Macro.FileInfo)
            {
                arguments = ArgumentRange.AtLeast(2), needOpenScope = true,
                onFeedCodeFile = (codeInfos, arguments, data) =>
                {
                    codeInfos.blockNamespace = arguments[0].Content;
                    codeInfos.blockClassName = arguments[1].Content;
                    if (arguments.Count > 2)
                    {
                        var additionalArguments = arguments.GetRange(2, arguments.Count - 2);
                        foreach (var argument in additionalArguments)
                        {
                            codeInfos.blockClassPrefix.Add(argument.Content);
                        }
                    }

                    return true;
                },
                onCloseScope = (codeFile, scope) =>
                {
                    codeFile.Commit();
                    return scope == codeBlock;
                }
            });

            keywordUsages.Add(new KeywordUsage(PrateekScriptBuilder.Tag.Macro.ClassInfo, codeBlock)
            {
                arguments = 1,
                needOpenScope = true,
                onFeedCodeFile = (codeInfos, arguments, data) =>
                {
                    codeInfos.classInfos.Add(new PrateekScriptBuilder.CodeFile.ClassInfos() {className = arguments[0].Content});
                    return true;
                }
            });

            keywordUsages.Add(new KeywordUsage(PrateekScriptBuilder.Tag.Macro.ClassNames, PrateekScriptBuilder.Tag.Macro.ClassInfo)
            {
                arguments = ArgumentRange.AtLeast(1),
                onFeedCodeFile = (codeInfos, arguments, data) =>
                {
                    return codeInfos.SetClassNames(arguments);
                }
            });

            keywordUsages.Add(new KeywordUsage(PrateekScriptBuilder.Tag.Macro.ClassVars, PrateekScriptBuilder.Tag.Macro.ClassInfo)
            {
                arguments = ArgumentRange.AtLeast(1),
                onFeedCodeFile = (codeInfos, arguments, data) =>
                {
                    return codeInfos.SetClassVars(arguments);
                }
            });

            keywordUsages.Add(new KeywordUsage(PrateekScriptBuilder.Tag.Macro.DefaultInfo, codeBlock)
            {
                arguments = ArgumentRange.Between(2, 3),
                onFeedCodeFile = (codeInfos, arguments, data) =>
                {
                    codeInfos.classDefaultType = arguments[0].Content;
                    codeInfos.classDefaultValue = arguments[1].Content;
                    codeInfos.classDefaultExportOnly = (arguments.Count == 2 || arguments[2].Content == "false") ? false : true;
                    return true;
                }
            });

            keywordUsages.Add(new KeywordUsage(PrateekScriptBuilder.Tag.Macro.Func, codeBlock)
            {
                needOpenScope = true,
                needScopeData = true,
                onFeedCodeFile = (codeInfos, arguments, data) =>
                {
                    codeInfos.funcInfos.Add(new PrateekScriptBuilder.CodeFile.FuncInfos());
                    codeInfos.SetFuncData(data);
                    return true;
                },
                onCloseScope = (codeFile, scope) => { return true; }
            });

            keywordUsages.Add(new KeywordUsage(PrateekScriptBuilder.Tag.Macro.CodePartPrefix, codeBlock)
            {
                arguments = 0, needOpenScope = true, needScopeData = true,
                onFeedCodeFile = (codeInfos, arguments, data) =>
                {
                    codeInfos.codePrefix = data;
                    return true;
                },
                onCloseScope = (codeFile, scope) => { return true; }
            });

            keywordUsages.Add(new KeywordUsage(PrateekScriptBuilder.Tag.Macro.CodePartMain, codeBlock)
            {
                arguments = 0, needOpenScope = true, needScopeData = true,
                onFeedCodeFile = (codeInfos, arguments, data) =>
                {
                    codeInfos.codeMain = data;
                    return true;
                },
                onCloseScope = (codeFile, scope) => { return true; }
            });

            keywordUsages.Add(new KeywordUsage(PrateekScriptBuilder.Tag.Macro.CodePartSuffix, codeBlock)
            {
                arguments = 0, needOpenScope = true, needScopeData = true,
                onFeedCodeFile = (codeInfos, arguments, data) =>
                {
                    codeInfos.codePostfix = data;
                    return true;
                },
                onCloseScope = (codeFile, scope) => { return true; }
            });

            keywordUsages.Add(new KeywordUsage() {usage = KeywordUsage.Usage.Ignore});
        }

        public bool FeedCodeFile(PrateekScriptBuilder.CodeFile codeFile, KeywordUsage keywordUsage, CodeKeyword rootKeyword)
        {
            var codeInfos = codeFile.CodeInfos;
            if (codeInfos == null)
            {
                codeInfos = codeFile.NewData(this);
            }

            if (codeInfos.activeRule == null || codeInfos.activeRule != this)
            {
                return false;
            }

            var data = string.Empty;
            if (keywordUsage.needScopeData)
            {
                foreach (var codeCommand in rootKeyword.scopeContent.commands)
                {
                    if (!(codeCommand is CodeLiteral codeLiteral))
                    {
                        return false;
                    }

                    foreach (var literalValue in codeLiteral.literals)
                    {
                        data += literalValue.Content;
                    }
                }
            }

            return keywordUsage.onFeedCodeFile.Invoke(codeInfos, rootKeyword.arguments, data);
        }

        //-----------------------------------------------------------------
        #region Utils
        protected void AddCode(string code, PrateekScriptBuilder.CodeFile.ContentInfos data, StringSwap stringSwapSrc)
        {
            AddCode(code, data, stringSwapSrc, new StringSwap());
        }

        //-----------------------------------------------------------------
        protected void AddCode(string code, PrateekScriptBuilder.CodeFile.ContentInfos data, StringSwap stringSwapSrc, StringSwap stringSwapDst)
        {
            code = stringSwapSrc.Apply(code);
            code = stringSwapDst.Apply(code);
            data.codeGenerated += code;
        }
        #endregion Utils

        //-----------------------------------------------------------------
        #region CodeRule overridable
        public CodeGenerator.CodeBuilder.BuildResult Generate(PrateekScriptBuilder.CodeFile.ContentInfos data)
        {
            var variants = new List<FunctionVariant>();
            //If needed, Add the default value as a possible class
            var maxSrc = data.classInfos.Count + (GenerateDefault ? 1 : 0);
            //Only loop through the dst classes if required
            var maxDst  = GenMode == GenerationMode.ForeachSrcXDest ? data.classInfos.Count : 1;
            var infoSrc = new PrateekScriptBuilder.CodeFile.ClassInfos();
            var infoDst = new PrateekScriptBuilder.CodeFile.ClassInfos();
            var infoDef = new PrateekScriptBuilder.CodeFile.ClassInfos();
            if (GenerateDefault)
            {
                infoDef.className = data.classDefaultType;
            }

            //Find the amount of FUNC_RESULT & NAMES that are requested by the main code
            var funcCount = Funcs.GetCount(data.codeMain);

            //Loop throught the source classes
            for (int iSrc = 0; iSrc < maxSrc; iSrc++)
            {
                //Add the default value as a possible class
                infoSrc = (GenerateDefault && iSrc == 0)
                    ? infoDef
                    : data.classInfos[iSrc + (GenerateDefault ? -1 : 0)];

                //one pass or as many as the dst classes
                for (int iSDst = 0; iSDst < maxDst; iSDst++)
                {
                    if (GenMode == GenerationMode.ForeachSrcXDest)
                        infoDst = data.classInfos[iSDst];

                    //Gather code variants
                    GatherVariants(variants, data, infoSrc, infoDst);

                    var swapSrc = ClassSrc + infoSrc.className;
                    var swapDst = ClassDst;
                    //Add the prefix from the code file
                    if (GenMode == GenerationMode.ForeachSrcXDest)
                    {
                        swapDst = swapDst + infoDst.className;
                        AddCode(data.codePrefix, data, swapSrc, swapDst);
                    }
                    else
                    {
                        AddCode(data.codePrefix, data, swapSrc);
                    }

                    //Go through all variants and apply them to the code
                    for (int v = 0; v < variants.Count; v++)
                    {
                        var variant = variants[v];
                        var code    = data.codeMain;

                        //Error out if the requested funcs result are not available
                        if (funcCount > variant.Count)
                        {
                            return (CodeGenerator.CodeBuilder.BuildResult)CodeGenerator.CodeBuilder.BuildResult.ValueType.PrateekScriptInsufficientNames + GetType().Name + infoSrc.className;
                        }

                        //Apply variants
                        for (int r = 0; r < variant.Count; r++)
                        {
                            code = (Funcs[r] + variant[r]).Apply(code);
                        }

                        //Error out if the requested Names are not available
                        var nameCount = Names.GetCount(code);
                        if (nameCount > infoSrc.NameCount)
                        {
                            return (CodeGenerator.CodeBuilder.BuildResult)CodeGenerator.CodeBuilder.BuildResult.ValueType.PrateekScriptInsufficientNames + infoSrc.className;
                        }

                        //Apply names
                        for (int r = 0; r < Core.Code.CSharp.min(nameCount, infoSrc.NameCount); r++)
                        {
                            code = (Names[r] + infoSrc.names[r]).Apply(code);
                        }

                        if (GenMode == GenerationMode.ForeachSrcXDest)
                        {
                            AddCode(code, data, swapSrc, swapDst);
                        }
                        else
                        {
                            AddCode(code, data, swapSrc);
                        }
                    }

                    //Add the Postfix from the code file
                    if (GenMode == GenerationMode.ForeachSrcXDest)
                    {
                        AddCode(data.codePostfix, data, swapSrc, swapDst);
                    }
                    else
                    {
                        AddCode(data.codePostfix, data, swapSrc);
                    }
                }
            }

            data.codeGenerated = data.codeGenerated.Replace(Strings.NewLine(String.Empty), Strings.NewLine(String.Empty) + PrateekScriptBuilder.Tag.Macro.codeGenTabs.Keyword());

            return CodeGenerator.CodeBuilder.BuildResult.ValueType.Success;
        }

        //-----------------------------------------------------------------
        public KeywordUsage GetKeywordRule(CodeKeyword keyword, string activeScope)
        {
            foreach (var keywordRule in keywordUsages)
            {
                if (!keywordRule.ValidateRule(keyword, activeScope))
                {
                    continue;
                }

                return keywordRule;
            }

            return default;
        }

        //-----------------------------------------------------------------
        public virtual bool CloseScope(PrateekScriptBuilder.CodeFile codeFile, string scope)
        {
            if (scope == CodeBlock)
            {
                codeFile.Commit();
                return true;
            }
            else if (scope == PrateekScriptBuilder.Tag.Macro.Func
                  || scope == PrateekScriptBuilder.Tag.Macro.CodePartMain
                  || scope == PrateekScriptBuilder.Tag.Macro.CodePartPrefix
                  || scope == PrateekScriptBuilder.Tag.Macro.CodePartSuffix)
            {
                return true;
            }
            return false;
        }
        #endregion CodeRule override

        //-----------------------------------------------------------------
        #region CodeRule abstract
        protected abstract void GatherVariants(List<FunctionVariant> variants, PrateekScriptBuilder.CodeFile.ContentInfos data, PrateekScriptBuilder.CodeFile.ClassInfos infoSrc, PrateekScriptBuilder.CodeFile.ClassInfos infoDst);
        #endregion CodeRule abstract
    }
}