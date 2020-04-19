namespace Assets.Prateek.CodeGenerator.Code.PrateekScriptBuilder.CodeAnalyzer.Symbols
{
    using System.Text.RegularExpressions;

    public abstract class Scope<TSymbol> : Symbol<TSymbol>
        where TSymbol : Symbol<TSymbol>, new()
    {
        #region Properties
        public override Regex End
        {
            get { return null; }
        }
        #endregion
    }
}
