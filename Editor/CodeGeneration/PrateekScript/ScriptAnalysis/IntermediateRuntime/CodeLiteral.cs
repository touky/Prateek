namespace Prateek.Editor.CodeGeneration.PrateekScript.ScriptAnalysis.IntermediateRuntime
{
    using System.Collections.Generic;
    using Prateek.Editor.CodeGeneration.PrateekScript.ScriptAnalysis.SyntaxSymbols;

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
