// -BEGIN_PRATEEK_COPYRIGHT-
// -END_PRATEEK_COPYRIGHT-

// -BEGIN_PRATEEK_CSHARP_IFDEF-
// -END_PRATEEK_CSHARP_IFDEF-

namespace Prateek.CodeGenerator.PrateekScriptBuilder
{
    using System.Collections.Generic;
    using Prateek.Core.Code.Helpers;

    public partial class MixedFuncScriptAction : ScriptAction
    {
        #region Properties
        ///----------------------------------------------------------------
        public override string ScopeTag
        {
            get { return "FUNC_MIXED"; }
        }

        public override GenerationMode GenMode
        {
            get { return GenerationMode.ForeachSrc; }
        }

        public override bool GenerateDefault
        {
            get { return true; }
        }
        #endregion

        #region Constructors
        //-----------------------------------------------------------------
        public MixedFuncScriptAction(string extension) : base(extension) { }
        #endregion

        #region Class Methods
        public static MixedFuncScriptAction Create(string extension)
        {
            return new MixedFuncScriptAction(extension);
        }

        //-----------------------------------------------------------------

        #region CodeRule override
        protected override void Init()
        {
            base.Init();

            var rule = keywordRules.Find(x =>
            {
                return x.keyword == PrateekScriptBuilder.Tag.Macro.Func && x.scope == CodeBlock;
            });

            if (rule.usage != CodeBuilder.Utils.KeywordRule.Usage.Ignore)
            {
                keywordRules.Add(new CodeBuilder.Utils.KeywordRule(PrateekScriptBuilder.Tag.Macro.Func, CodeBlock)
                {
                    arguments = 1, needOpenScope = true, needScopeData = true,
                    onFeedCodeFile = (codeInfos, arguments, data) =>
                    {
                        codeInfos.funcInfos.Add(new PrateekScriptBuilder.CodeFile.FuncInfos {funcName = arguments[0].Content, data = data});
                        return true;
                    }
                });
            }
        }
        #endregion CodeRule override

        //-----------------------------------------------------------------

        #region Rule internal
        protected override void GatherVariants(List<FuncVariant> variants, PrateekScriptBuilder.CodeFile.ContentInfos data, PrateekScriptBuilder.CodeFile.ClassInfos infoSrc, PrateekScriptBuilder.CodeFile.ClassInfos infoDst)
        {
            variants.Clear();

            var isDefault = infoSrc.VarCount == 0;
            for (var d = 0; d < data.funcInfos.Count; d++)
            {
                for (var p = 0; p < (isDefault ? 1 : 2); p++)
                {
                    if (data.classDefaultExportOnly && (isDefault || p == 0))
                    {
                        continue;
                    }

                    var funcInfo = data.funcInfos[d];
                    var variant  = new FuncVariant(funcInfo.funcName, 2);

                    var varsCount = Vars.GetCount(funcInfo.data);
                    if (p == 1 && varsCount == 1)
                    {
                        continue;
                    }

                    var vars = funcInfo.data;
                    for (var a = 0; a < varsCount; a++)
                    {
                        if (isDefault)
                        {
                            variant[1] = string.Format(PrateekScriptBuilder.Tag.Code.argsN, data.classDefaultType, a);
                            vars = (Vars[a] + string.Format(PrateekScriptBuilder.Tag.Code.varsN, a)).Apply(vars);
                        }
                        else
                        {
                            variant[1] = p == 1 && a != 0
                                ? string.Format(PrateekScriptBuilder.Tag.Code.argsN, data.classDefaultType, a)
                                : string.Format(PrateekScriptBuilder.Tag.Code.argsV_, infoSrc.className, a);
                        }
                    }

                    if (isDefault)
                    {
                        variant[2] = vars;
                    }
                    else
                    {
                        for (var v = 0; v < infoSrc.VarCount; v++)
                        {
                            var varsA = vars;
                            for (var a = 0; a < varsCount; a++)
                            {
                                varsA = p == 1 && a != 0
                                    ? (Vars[a] + string.Format(PrateekScriptBuilder.Tag.Code.varsN, a)).Apply(varsA)
                                    : (Vars[a] + string.Format(PrateekScriptBuilder.Tag.Code.varsV_, a, infoSrc.variables[v])).Apply(varsA);
                            }

                            variant[2] = varsA;
                        }

                        var v2 = new FuncVariant(variant.Call, 2);
                        v2[1] = variant[1];
                        v2[2] = PrateekScriptBuilder.Tag.Code.varNew + infoSrc.className + Strings.Separator.ParenthesisOpen.C() + variant[2] + Strings.Separator.ParenthesisClose.C();
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
