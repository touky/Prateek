namespace Assets.Prateek.CodeGenerator.Code.PrateekScriptBuilder.CodeAnalyzer.Symbols
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
