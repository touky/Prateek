// -BEGIN_PRATEEK_COPYRIGHT-
// -END_PRATEEK_COPYRIGHT-

// -BEGIN_PRATEEK_CSHARP_IFDEF-
// -END_PRATEEK_CSHARP_IFDEF-

namespace Prateek.CodeGeneration.PrateekScript.Editor.ScriptActions
{
    using System.Collections.Generic;
    using Prateek.CodeGeneration.Code.PrateekScript.CodeGeneration;
    using Prateek.CodeGeneration.Code.PrateekScript.ScriptAnalysis.Utils;
    using Prateek.Core.Code.Helpers;

    /// <summary>
    /// Generates methods using NAMES() for each of the  CLASS_INFOS() to create overloads mixing the main type and the DEFAULT() given type
    /// </summary>
    public class MixedOverloadScriptAction : ScriptAction
    {
        #region Properties
        ///----------------------------------------------------------------
        public override string ScopeTag
        {
            get { return "MIXED_OVERLOAD"; }
        }

        public override GenerationRule GenerationMode
        {
            get { return GenerationRule.ForeachSrc; }
        }

        public override bool GenerateDefault
        {
            get { return true; }
        }
        #endregion

        #region Constructors
        ///-----------------------------------------------------------------
        public MixedOverloadScriptAction(string extension) : base(extension) { }
        #endregion

        #region Class Methods
        public static MixedOverloadScriptAction Create(string extension)
        {
            return new MixedOverloadScriptAction(extension);
        }

        ///-----------------------------------------------------------------

        #region CodeRule override
        protected override void Init()
        {
            base.Init();

            var rule = keywordUsages.Find(x =>
            {
                return x.keyword == Glossary.Macros[FunctionKeyword.FUNC] && x.scope == CodeBlock;
            });

            if (rule.keywordUsageType != KeywordUsageType.Ignore)
            {
                keywordUsages.Add(new KeywordUsage(Glossary.Macros[FunctionKeyword.FUNC], CodeBlock)
                {
                    arguments = 1, needOpenScope = true, needScopeData = true,
                    onFeedCodeFile = (FileData, codeFile, codeInfos, arguments, data) =>
                    {
                        codeInfos.functionContents.Add(new FunctionContent {funcName = arguments[0].Content, body = data});
                        return true;
                    }
                });
            }
        }
        #endregion CodeRule override

        #region Rule internal
        ///-----------------------------------------------------------------
        protected override void GatherVariants(List<FunctionVariant> variants, ScriptContent scriptContent, ClassContent contentSrc, ClassContent contentDst)
        {
            variants.Clear();

            var isDefault = contentSrc.VarCount == 0;
            for (var d = 0; d < scriptContent.functionContents.Count; d++)
            {
                for (var p = 0; p < (isDefault ? 1 : 2); p++)
                {
                    if (scriptContent.classDefaultExportOnly && (isDefault || p == 0))
                    {
                        continue;
                    }

                    var funcInfo = scriptContent.functionContents[d];
                    var variant  = new FunctionVariant(funcInfo.funcName, 2);

                    var varsCount = Variables.FindCount(funcInfo.body);
                    if (p == 1 && varsCount == 1)
                    {
                        continue;
                    }

                    var vars = funcInfo.body;
                    for (var a = 0; a < varsCount; a++)
                    {
                        if (isDefault)
                        {
                            variant[1] = string.Format(Glossary.Code.argsN, scriptContent.classDefaultType, a);
                            vars = (Variables[a] + string.Format(Glossary.Code.varsN, a)).Apply(vars);
                        }
                        else
                        {
                            variant[1] = p == 1 && a != 0
                                ? string.Format(Glossary.Code.argsN, scriptContent.classDefaultType, a)
                                : string.Format(Glossary.Code.argsV_, contentSrc.className, a);
                        }
                    }

                    if (isDefault)
                    {
                        variant[2] = vars;
                    }
                    else
                    {
                        for (var v = 0; v < contentSrc.VarCount; v++)
                        {
                            var varsA = vars;
                            for (var a = 0; a < varsCount; a++)
                            {
                                varsA = p == 1 && a != 0
                                    ? (Variables[a] + string.Format(Glossary.Code.varsN, a)).Apply(varsA)
                                    : (Variables[a] + string.Format(Glossary.Code.varsV_, a, contentSrc.variables[v])).Apply(varsA);
                            }

                            variant[2] = varsA;
                        }

                        var v2 = new FunctionVariant(variant.Call, 2);
                        v2[1] = variant[1];
                        v2[2] = Glossary.Code.varNew + contentSrc.className + Strings.Separator.ParenthesisOpen.C() + variant[2] + Strings.Separator.ParenthesisClose.C();
                        variant = v2;
                    }

                    variants.Add(variant);
                }
            }
        }
        #endregion Rule internal
        #endregion
    }
}
