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
        public override bool CanAbsorb(Symbol symbol)
        {
            return symbol is LiteralValue;
        }

        public override void Add(Symbol symbol)
        {
            if (symbol is LiteralValue literalValue)
            {
                literals.Add(literalValue);
            }
        }
        #endregion
    }
}
