namespace Assets.Prateek.CodeGenerator.Code.PrateekScriptBuilder.CodeAnalyzer.Symbols
{
    using System.Text.RegularExpressions;

    public class SingleLineComment : Symbol<SingleLineComment>, IComment
    {
        #region Static and Constants
        private static readonly Regex START = new Regex(@"//");
        private static readonly Regex END = null;
        #endregion

        #region Properties
        public override bool CanStopWithEndOfLine
        {
            get { return true; }
        }

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
