namespace Assets.Prateek.CodeGenerator.Code.PrateekScriptBuilder.CodeCommands {
    using System.Collections.Generic;
    using Assets.Prateek.CodeGenerator.Code.PrateekScriptBuilder.CodeAnalyzer.Symbols;

    public class CodeLiteral : CodeCommand
    {
        public List<LiteralValue> literals = new List<LiteralValue>();

        public void Add(LiteralValue literalSymbol)
        {
            literals.Add(literalSymbol);
        }
    }
}