namespace Prateek.Editor.CodeGeneration.PrateekScript.ScriptAnalysis.SyntaxSymbols
{
    public abstract class Symbol<TSymbol> : Symbol
        where TSymbol : Symbol<TSymbol>, new()
    {
        #region Class Methods
        public override Symbol Clone(string content)
        {
            var clone = new TSymbol();
            clone.Init(content);
            return clone;
        }
        #endregion
    }
}
