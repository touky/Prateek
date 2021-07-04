namespace Prateek.Editor.CodeGeneration.PrateekScript.ScriptAnalysis.IntermediateRuntime
{
    using Prateek.Editor.CodeGeneration.PrateekScript.ScriptAnalysis.SyntaxSymbols;

    public class InvokeScope<TSeparator> : CodeScope
        where TSeparator : Symbol, ISeparator, new()
    {
        private readonly TSeparator separator = new TSeparator();

        public override ISeparator Separator
        {
            get { return separator; }
        }

        public override bool CanAbsorb(Symbol symbol)
        {
            return symbol is TSeparator;
        }
    }
}