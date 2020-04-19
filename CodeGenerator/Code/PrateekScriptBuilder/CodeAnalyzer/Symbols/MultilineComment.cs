namespace Assets.Prateek.CodeGenerator.Code.PrateekScriptBuilder.CodeAnalyzer.Symbols
{
    using System.Text.RegularExpressions;

    public class MultilineComment : Symbol<MultilineComment>, IComment
    {
        #region Static and Constants
        private static readonly Regex START = new Regex(@"/\*");
        private static readonly Regex END = new Regex(@"\*/");
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
