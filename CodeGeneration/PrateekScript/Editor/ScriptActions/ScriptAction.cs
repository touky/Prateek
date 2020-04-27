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
namespace Assets.Prateek.CodeGenerator.Code.PrateekScript.ScriptActions
{
    using System.Collections.Generic;
    using Assets.Prateek.CodeGenerator.Code.CodeBuilder;
    using Assets.Prateek.CodeGenerator.Code.PrateekScript.CodeGeneration;
    using Assets.Prateek.CodeGenerator.Code.PrateekScript.ScriptAnalysis.IntermediateCode;
    using Assets.Prateek.CodeGenerator.Code.PrateekScript.ScriptAnalysis.Utils;
    using Assets.Prateek.CodeGenerator.Code.Utils;
    using global::Prateek.CodeGenerator;
    using global::Prateek.CodeGenerator.ScriptTemplates;
    using global::Prateek.Core.Code.Extensions;
    using global::Prateek.Core.Code.Helpers;

    ///-------------------------------------------------------------------------
#if UNITY_EDITOR
#endif //UNITY_EDITOR

    public abstract class ScriptAction : BaseTemplate
    {
        #region GenerationMode enum
        ///-----------------------------------------------------------------

        ///-----------------------------------------------------------------
        public enum GenerationRule
        {
            ForeachSrc,
            ForeachSrcCrossDest,

            MAX
        }
        #endregion

        #region Fields
        protected List<KeywordUsage> keywordUsages = new List<KeywordUsage>();

        ///-----------------------------------------------------------------
        private string codeBlock = string.Empty;
        #endregion

        #region Properties
        ///-----------------------------------------------------------------
        public abstract string ScopeTag { get; }
        public abstract GenerationRule GenerationMode { get; }

        public virtual bool UseOneClassPerSource
        {
            get { return false; }
        }

        public virtual bool GenerateDefault
        {
            get { return false; }
        }

        ///-----------------------------------------------------------------
        public string CodeBlock
        {
            get { return codeBlock; }
        }

        ///-----------------------------------------------------------------
        public StringSwap ClassDst
        {
            get { return Glossary.Macro.dstClass; }
        }

        public StringSwap ClassSrc
        {
            get { return Glossary.Macro.srcClass; }
        }

        public NumberedSymbol Names
        {
            get { return Glossary.Macro.Names; }
        }

        public NumberedSymbol Functions
        {
            get { return Glossary.Macro.Functions; }
        }

        public NumberedSymbol Variables
        {
            get { return Glossary.Macro.Variables; }
        }
        #endregion

        #region Constructors
        ///-----------------------------------------------------------------
        protected ScriptAction(string extension) : base(extension)
        {
            Init();
        }
        #endregion

        #region Class Methods
        ///-----------------------------------------------------------------
        public override void Commit()
        {
            ScriptActionRegistry.Add(this);
        }

        ///-----------------------------------------------------------------
        protected virtual void Init()
        {
            Glossary.Macro.Init();

            codeBlock = string.Format("{0}_{1}_{2}", Glossary.Macro.prefix, Glossary.Macro.To(Glossary.Macro.FuncName.BLOCK), ScopeTag);

            keywordUsages.Add(new KeywordUsage(codeBlock, Glossary.Macro.FileInfo)
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

            keywordUsages.Add(new KeywordUsage(Glossary.Macro.ClassInfo, codeBlock)
            {
                arguments = 1,
                needOpenScope = true,
                onFeedCodeFile = (codeInfos, arguments, data) =>
                {
                    codeInfos.classInfos.Add(new ClassContent {className = arguments[0].Content});
                    return true;
                }
            });

            keywordUsages.Add(new KeywordUsage(Glossary.Macro.ClassNames, Glossary.Macro.ClassInfo)
            {
                arguments = ArgumentRange.AtLeast(1),
                onFeedCodeFile = (codeInfos, arguments, data) =>
                {
                    return codeInfos.SetClassNames(arguments);
                }
            });

            keywordUsages.Add(new KeywordUsage(Glossary.Macro.ClassVars, Glossary.Macro.ClassInfo)
            {
                arguments = ArgumentRange.AtLeast(1),
                onFeedCodeFile = (codeInfos, arguments, data) =>
                {
                    return codeInfos.SetClassVars(arguments);
                }
            });

            keywordUsages.Add(new KeywordUsage(Glossary.Macro.DefaultInfo, codeBlock)
            {
                arguments = ArgumentRange.Between(2, 3),
                onFeedCodeFile = (codeInfos, arguments, data) =>
                {
                    codeInfos.classDefaultType = arguments[0].Content;
                    codeInfos.classDefaultValue = arguments[1].Content;
                    codeInfos.classDefaultExportOnly = arguments.Count == 2 || arguments[2].Content == "false" ? false : true;
                    return true;
                }
            });

            keywordUsages.Add(new KeywordUsage(Glossary.Macro.Func, codeBlock)
            {
                needOpenScope = true,
                needScopeData = true,
                onFeedCodeFile = (codeInfos, arguments, data) =>
                {
                    codeInfos.functionContents.Add(new FunctionContent());
                    codeInfos.SetFuncData(data);
                    return true;
                },
                onCloseScope = (codeFile, scope) => { return true; }
            });

            keywordUsages.Add(new KeywordUsage(Glossary.Macro.CodePartPrefix, codeBlock)
            {
                arguments = 0, needOpenScope = true, needScopeData = true,
                onFeedCodeFile = (codeInfos, arguments, data) =>
                {
                    codeInfos.codePrefix = data;
                    return true;
                },
                onCloseScope = (codeFile, scope) => { return true; }
            });

            keywordUsages.Add(new KeywordUsage(Glossary.Macro.CodePartMain, codeBlock)
            {
                arguments = 0, needOpenScope = true, needScopeData = true,
                onFeedCodeFile = (codeInfos, arguments, data) =>
                {
                    codeInfos.codeMain = data;
                    return true;
                },
                onCloseScope = (codeFile, scope) => { return true; }
            });

            keywordUsages.Add(new KeywordUsage(Glossary.Macro.CodePartSuffix, codeBlock)
            {
                arguments = 0, needOpenScope = true, needScopeData = true,
                onFeedCodeFile = (codeInfos, arguments, data) =>
                {
                    codeInfos.codePostfix = data;
                    return true;
                },
                onCloseScope = (codeFile, scope) => { return true; }
            });

            keywordUsages.Add(new KeywordUsage {keywordUsageType = KeywordUsageType.Ignore});
        }

        public bool FeedCodeFile(CodeFile codeFile, KeywordUsage keywordUsage, CodeKeyword rootKeyword)
        {
            var codeInfos = codeFile.ScriptContent;
            if (codeInfos == null)
            {
                codeInfos = codeFile.NewData(this);
            }

            if (codeInfos.scriptAction == null || codeInfos.scriptAction != this)
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

        ///-----------------------------------------------------------------

        #region CodeRule abstract
        protected abstract void GatherVariants(List<FunctionVariant> variants, ScriptContent data, ClassContent contentSrc, ClassContent contentDst);
        #endregion CodeRule abstract
        #endregion

        ///-----------------------------------------------------------------

        #region Utils
        protected void AddCode(string code, ScriptContent data, StringSwap stringSwapSrc)
        {
            AddCode(code, data, stringSwapSrc, new StringSwap());
        }

        ///-----------------------------------------------------------------
        protected void AddCode(string code, ScriptContent data, StringSwap stringSwapSrc, StringSwap stringSwapDst)
        {
            code = stringSwapSrc.Apply(code);
            code = stringSwapDst.Apply(code);
            var generatedCode = data.codeGenerated.Last();
            generatedCode.code += code;
            data.codeGenerated.Last(generatedCode);
        }
        #endregion Utils

        ///-----------------------------------------------------------------

        #region CodeRule overridable
        public global::Assets.Prateek.CodeGenerator.Code.CodeBuilder.BuildResult Generate(ScriptContent data)
        {
            var variants = new List<FunctionVariant>();

            //If needed, Add the default value as a possible class
            var maxSrc = data.classInfos.Count + (GenerateDefault ? 1 : 0);

            //Only loop through the dst classes if required
            var maxDst  = GenerationMode == GenerationRule.ForeachSrcCrossDest ? data.classInfos.Count : 1;
            var infoSrc = new ClassContent();
            var infoDst = new ClassContent();
            var infoDef = new ClassContent();
            if (GenerateDefault)
            {
                infoDef.className = data.classDefaultType;
            }

            //Loop throught the source classes
            for (var iSrc = 0; iSrc < maxSrc; iSrc++)
            {
                //Add the default value as a possible class
                infoSrc = GenerateDefault && iSrc == 0
                    ? infoDef
                    : data.classInfos[iSrc + (GenerateDefault ? -1 : 0)];

                if (UseOneClassPerSource)
                {
                    data.codeGenerated.Add(new ScriptContent.GeneratedCode() { className = infoSrc.className, code = string.Empty });
                }
                else if (data.codeGenerated.Count == 0)
                {
                    data.codeGenerated.Add(new ScriptContent.GeneratedCode() { className = string.Empty, code = string.Empty });
                }

                //one pass or as many as the dst classes
                for (var iSDst = 0; iSDst < maxDst; iSDst++)
                {
                    if (GenerationMode == GenerationRule.ForeachSrcCrossDest)
                    {
                        infoDst = data.classInfos[iSDst];
                    }

                    //Gather code variants
                    GatherVariants(variants, data, infoSrc, infoDst);

                    var swapSrc = ClassSrc + infoSrc.className;
                    var swapDst = ClassDst;

                    //Add the prefix from the code file
                    if (GenerationMode == GenerationRule.ForeachSrcCrossDest)
                    {
                        swapDst = swapDst + infoDst.className;
                        AddCode(data.codePrefix, data, swapSrc, swapDst);
                    }
                    else
                    {
                        AddCode(data.codePrefix, data, swapSrc);
                    }

                    //Go through all variants and apply them to the code
                    for (var v = 0; v < variants.Count; v++)
                    {
                        var variant = variants[v];
                        var code    = data.codeMain;

                        //Error out if the requested funcs result are not available
                        if (!SwapCodeContent(ref code, Functions, variant.Count, variant.Variants))
                        {
                            return (global::Assets.Prateek.CodeGenerator.Code.CodeBuilder.BuildResult) global::Assets.Prateek.CodeGenerator.Code.CodeBuilder.BuildResult.ValueType.PrateekScriptInsufficientNames + GetType().Name + infoSrc.className;
                        }

                        //Error out if the requested Names are not available
                        if (!SwapCodeContent(ref code, Names, infoSrc.NameCount, infoSrc.names))
                        {
                            return (global::Assets.Prateek.CodeGenerator.Code.CodeBuilder.BuildResult) global::Assets.Prateek.CodeGenerator.Code.CodeBuilder.BuildResult.ValueType.PrateekScriptInsufficientNames + infoSrc.className;
                        }

                        if (GenerationMode == GenerationRule.ForeachSrcCrossDest)
                        {
                            AddCode(code, data, swapSrc, swapDst);
                        }
                        else
                        {
                            AddCode(code, data, swapSrc);
                        }
                    }

                    //Add the Postfix from the code file
                    if (GenerationMode == GenerationRule.ForeachSrcCrossDest)
                    {
                        AddCode(data.codePostfix, data, swapSrc, swapDst);
                    }
                    else
                    {
                        AddCode(data.codePostfix, data, swapSrc);
                    }
                }
            }

            for (int c = 0; c < data.codeGenerated.Count; c++)
            {
                var codeGenerated = data.codeGenerated[c];
                codeGenerated.code = codeGenerated.code.Replace(string.Empty.NewLine(), string.Empty.NewLine() + Glossary.Macro.codeGenTabs.Keyword());
                data.codeGenerated[c] = codeGenerated;
            }

            return global::Assets.Prateek.CodeGenerator.Code.CodeBuilder.BuildResult.ValueType.Success;
        }

        ///-----------------------------------------------------------------
        private static bool SwapCodeContent(ref string code, NumberedSymbol symbol, int replacementCount, List<string> replacement)
        {
            for (var r = 0; r < replacementCount; r++)
            {
                if (!symbol[r].CanSwap(code))
                {
                    continue;
                }

                code = (symbol[r] + replacement[r]).Apply(code);
            }

            return !symbol.HasAny(code);
        }

        ///-----------------------------------------------------------------
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

        ///-----------------------------------------------------------------
        public virtual bool CloseScope(CodeFile codeFile, string scope)
        {
            if (scope == CodeBlock)
            {
                codeFile.Commit();
                return true;
            }
            else if (scope == Glossary.Macro.Func
                  || scope == Glossary.Macro.CodePartMain
                  || scope == Glossary.Macro.CodePartPrefix
                  || scope == Glossary.Macro.CodePartSuffix)
            {
                return true;
            }

            return false;
        }
        #endregion CodeRule override
    }
}
