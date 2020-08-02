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
namespace Prateek.Editor.CodeGeneration.PrateekScript.ScriptActions
{
    using System.Collections.Generic;
    using Prateek.Editor.CodeGeneration.CodeBuilder.RuntimeBuilder;
    using Prateek.Editor.CodeGeneration.CodeBuilder.ScriptTemplates;
    using Prateek.Editor.CodeGeneration.CodeBuilder.Utils;
    using Prateek.Editor.CodeGeneration.PrateekScript.RuntimeGeneration;
    using Prateek.Editor.CodeGeneration.PrateekScript.ScriptAnalysis.IntermediateRuntime;
    using Prateek.Editor.CodeGeneration.PrateekScript.ScriptAnalysis.Utils;
    using Prateek.Runtime.Core.Extensions;
    using Prateek.Runtime.Core.Helpers;

    public abstract class ScriptAction : BaseTemplate
    {
        #region GenerationRule enum
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

        public IReadOnlyList<KeywordUsage> KeywordUsages
        {
            get { return keywordUsages; }
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

        public NumberedSymbol Defaults
        {
            get { return Glossary.Macros.Defaults; }
        }

        public NumberedSymbol Names
        {
            get { return Glossary.Macros.Names; }
        }

        public NumberedSymbol Functions
        {
            get { return Glossary.Macros.Functions; }
        }

        public NumberedSymbol Variables
        {
            get { return Glossary.Macros.Variables; }
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
            Glossary.Macros.Init();

            codeBlock = $"{Glossary.Macros.codeBlockFormat}{ScopeTag}";

            keywordUsages.Add(KeywordCreator.GetFileInfos());

            KeywordCreator.AddDefine(keywordUsages, codeBlock);
            KeywordCreator.AddUsing(keywordUsages, codeBlock);
            KeywordCreator.AddCodeBlock(keywordUsages, codeBlock);
            KeywordCreator.AddClassInfo(keywordUsages, codeBlock);

            KeywordCreator.AddCodeImport(keywordUsages, codeBlock);

            KeywordCreator.AddNames(keywordUsages);
            KeywordCreator.AddVars(keywordUsages);

            KeywordCreator.AddDefault(keywordUsages, codeBlock);
            KeywordCreator.AddFunc(keywordUsages, codeBlock);

            KeywordCreator.AddCodeHeader(keywordUsages, codeBlock);
            KeywordCreator.AddCodeBody(keywordUsages, codeBlock);
            KeywordCreator.AddCodeFooter(keywordUsages, codeBlock);

            keywordUsages.Add(new KeywordUsage {keywordUsageType = KeywordUsageType.Forbidden});
        }

        public bool FeedCodeFile(FileData fileData, CodeFile codeFile, KeywordUsage keywordUsage, CodeKeyword rootKeyword)
        {
            var scriptContent = codeFile.ScriptContent;
            if (keywordUsage.createNewScriptContent)
            {
                if (scriptContent == null)
                {
                    scriptContent = codeFile.NewData(this);
                }

                if (scriptContent.scriptAction == null || scriptContent.scriptAction != this)
                {
                    return false;
                }
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

            return keywordUsage.onFeedCodeFile.Invoke(fileData, codeFile, scriptContent, rootKeyword.arguments, data);
        }

        ///-----------------------------------------------------------------

        #region CodeRule abstract
        protected abstract void GatherVariants(List<FunctionVariant> variants, ScriptContent scriptContent, ClassContent contentSrc, ClassContent contentDst);
        #endregion CodeRule abstract
        #endregion

        ///-----------------------------------------------------------------

        #region Utils
        protected void AddCodeTo(ScriptContent data, string code, StringSwap stringSwapSrc)
        {
            AddCodeTo(data, code, stringSwapSrc, new StringSwap());
        }

        ///-----------------------------------------------------------------
        protected void AddCodeTo(ScriptContent data, string code, StringSwap stringSwapSrc, StringSwap stringSwapDst)
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
        public virtual BuildResult Generate(ScriptContent data)
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
                    data.codeGenerated.Add(new ScriptContent.GeneratedCode {className = infoSrc.className, code = string.Empty});
                }
                else if (data.codeGenerated.Count == 0)
                {
                    data.codeGenerated.Add(new ScriptContent.GeneratedCode {className = string.Empty, code = string.Empty});
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

                    //Add the Header from the code file
                    if (GenerationMode == GenerationRule.ForeachSrcCrossDest)
                    {
                        swapDst = swapDst + infoDst.className;
                        AddCodeTo(data, data.codeHeader, swapSrc, swapDst);
                    }
                    else
                    {
                        AddCodeTo(data, data.codeHeader, swapSrc);
                    }

                    //Go through all variants and apply them to the code
                    for (var v = 0; v < variants.Count; v++)
                    {
                        var variant = variants[v];
                        var codeBody    = data.codeBody;

                        //Error out if the requested funcs result are not available
                        if (!SwapCodeContent(ref codeBody, Functions, variant.Count, variant.Variants))
                        {
                            return (BuildResult) BuildResult.ValueType.PrateekScriptInsufficientNames + GetType().Name + infoSrc.className;
                        }

                        //Error out if the requested Names are not available
                        if (!SwapCodeContent(ref codeBody, Names, infoSrc.NameCount, infoSrc.names))
                        {
                            return (BuildResult) BuildResult.ValueType.PrateekScriptInsufficientNames + infoSrc.className;
                        }

                        if (GenerationMode == GenerationRule.ForeachSrcCrossDest)
                        {
                            AddCodeTo(data, codeBody, swapSrc, swapDst);
                        }
                        else
                        {
                            AddCodeTo(data, codeBody, swapSrc);
                        }
                    }

                    //Add the Footer from the code file
                    if (GenerationMode == GenerationRule.ForeachSrcCrossDest)
                    {
                        AddCodeTo(data, data.codeFooter, swapSrc, swapDst);
                    }
                    else
                    {
                        AddCodeTo(data, data.codeFooter, swapSrc);
                    }
                }
            }

            for (var c = 0; c < data.codeGenerated.Count; c++)
            {
                var codeGenerated = data.codeGenerated[c];
                codeGenerated.code = codeGenerated.code.Replace(string.Empty.NewLine(), string.Empty.NewLine() + Glossary.Macros.codeDataTabsTag);
                data.codeGenerated[c] = codeGenerated;
            }

            return BuildResult.ValueType.Success;
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
        public bool ExistInVocabulary(CodeKeyword rootKeyword)
        {
            foreach (var keywordRule in keywordUsages)
            {
                if (keywordRule.keyword == rootKeyword.keyword.Content)
                {
                    return true;
                }
            }

            return false;
        }

        ///-----------------------------------------------------------------
        public KeywordUsage GetKeywordUsage(CodeKeyword keyword, string activeScope)
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
        #endregion CodeRule override
    }
}
