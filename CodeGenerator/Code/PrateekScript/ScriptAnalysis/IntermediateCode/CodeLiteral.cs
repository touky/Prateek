namespace Assets.Prateek.CodeGenerator.Code.PrateekScript.ScriptAnalysis.IntermediateCode
{
    using System.Collections.Generic;
    using Assets.Prateek.CodeGenerator.Code.PrateekScript.ScriptAnalysis.SyntaxSymbols;

    public class CodeLiteral : CodeCommand
    {
        #region Fields
        public List<LiteralValue> literals = new List<LiteralValue>();
        #endregion

        #region Class Methods
        public void Add(LiteralValue literalSymbol)
        {
            literals.Add(literalSymbol);
        }
        #endregion
    }
}
