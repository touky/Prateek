// -BEGIN_PRATEEK_COPYRIGHT-
// -END_PRATEEK_COPYRIGHT-

// -BEGIN_PRATEEK_CSHARP_IFDEF-
// -END_PRATEEK_CSHARP_IFDEF-

namespace Prateek.CodeGeneration.Code.PrateekScript.ScriptActions
{
    using System.Collections.Generic;
    using Prateek.CodeGeneration.Code.PrateekScript.CodeGeneration;

    public partial class BasicFuncScriptAction : ScriptAction
    {
        #region Properties
        ///-----------------------------------------------------------------
        public override string ScopeTag
        {
            get { return "FUNC_BASIC"; }
        }

        public override GenerationRule GenerationMode
        {
            get { return GenerationRule.ForeachSrc; }
        }

        public override bool GenerateDefault
        {
            get { return false; }
        }
        #endregion

        #region Constructors
        ///-----------------------------------------------------------------
        public BasicFuncScriptAction(string extension) : base(extension) { }
        #endregion

        #region Class Methods
        ///---------------------------------------------------------------------
        internal static BasicFuncScriptAction Create(string extension)
        {
            return new BasicFuncScriptAction(extension);
        }

        ///-----------------------------------------------------------------

        #region Rule internal
        protected override void GatherVariants(List<FunctionVariant> variants, ScriptContent data, ClassContent contentSrc, ClassContent contentDst)
        {
            variants.Clear();
            if (data.functionContents.Count == 0)
            {
                variants.Add(new FunctionVariant());
            }
            else
            {
                var variant = new FunctionVariant(data.functionContents.Count);
                for (var d = 0; d < data.functionContents.Count; d++)
                {
                    var functionContent = data.functionContents[d];
                    for (var v = 0; v < contentSrc.variables.Count; v++)
                    {
                        var functionData = functionContent.data;
                        if (Variables.DefaultSymbol.CanSwap(functionContent.data))
                        {
                            var swap = Variables.DefaultSymbol + contentSrc.variables[v];
                            functionData = swap.Apply(functionData);
                        }

                        var currentVariable = Variables[v];
                        if (currentVariable.CanSwap(functionContent.data))
                        {
                            var swap = currentVariable + contentSrc.variables[v];
                            functionData = swap.Apply(functionData);
                        }

                        variant[d] = functionData;
                    }
                }

                variants.Add(variant);
            }
        }
        #endregion Rule internal
        #endregion
    }
}
