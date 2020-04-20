namespace Assets.Prateek.CodeGenerator.Code.PrateekScript.ScriptAnalysis.SyntaxSymbols
{
    using System.Text.RegularExpressions;

    public class Value : Symbol<Keyword>
    {
        #region Static and Constants
        private static readonly Regex START = new Regex("([0-9]+)");
        #endregion

        #region Properties
        public override Regex Start
        {
            get { return START; }
        }

        public override Regex End
        {
            get { return null; }
        }
        #endregion
    }
}
