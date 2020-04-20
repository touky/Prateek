namespace Assets.Prateek.CodeGenerator.Code.PrateekScript.ScriptAnalysis.SyntaxSymbols
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
