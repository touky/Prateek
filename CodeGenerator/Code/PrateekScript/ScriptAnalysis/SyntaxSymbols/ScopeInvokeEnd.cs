namespace Assets.Prateek.CodeGenerator.Code.PrateekScript.ScriptAnalysis.SyntaxSymbols
{
    using System.Text.RegularExpressions;

    public class ScopeInvokeEnd : Scope<ScopeInvokeEnd>
    {
        #region Static and Constants
        private static readonly Regex START = new Regex("\\)");
        #endregion

        #region Properties
        public override Regex Start
        {
            get { return START; }
        }
        #endregion
    }
}
