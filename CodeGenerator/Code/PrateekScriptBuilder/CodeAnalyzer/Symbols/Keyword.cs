namespace Assets.Prateek.CodeGenerator.Code.PrateekScriptBuilder.CodeAnalyzer.Symbols
{
    using System.Text.RegularExpressions;

    public class Keyword : Symbol<Keyword>
    {
        #region Static and Constants
        private static readonly Regex START = new Regex("([a-zA-Z]+[a-zA-Z0-9_.]*)");
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
