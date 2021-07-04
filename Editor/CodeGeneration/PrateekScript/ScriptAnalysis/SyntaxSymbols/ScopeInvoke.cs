namespace Prateek.Editor.CodeGeneration.PrateekScript.ScriptAnalysis.SyntaxSymbols
{
    using System.Text.RegularExpressions;

    public class ScopeInvoke : Scope<ScopeInvoke>
    {
        #region Static and Constants
        private static readonly Regex START = new Regex("\\(");
        private static readonly Regex END = new Regex("\\)");
        #endregion

        #region Properties
        public override Regex Start
        {
            get { return START; }
        }

        public override Regex End
        {
            get { return END; }
        }
        #endregion
    }
}
