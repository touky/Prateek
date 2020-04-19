namespace Assets.Prateek.CodeGenerator.Code.PrateekScriptBuilder.CodeAnalyzer.Symbols
{
    using System.Text.RegularExpressions;

    public class CodeBeginScope : Scope<CodeBeginScope>
    {
        #region Static and Constants
        private static readonly Regex START = new Regex("{");
        #endregion

        #region Properties
        public override Regex Start
        {
            get { return START; }
        }
        #endregion
    }
}
