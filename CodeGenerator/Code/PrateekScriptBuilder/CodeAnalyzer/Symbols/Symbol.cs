namespace Assets.Prateek.CodeGenerator.Code.PrateekScriptBuilder.CodeAnalyzer.Symbols
{
    using System.Diagnostics;
    using System.Text.RegularExpressions;

    [DebuggerDisplay("{Start?.ToString()}<{Content}>{End?.ToString()}")]
    public abstract class Symbol
    {
        #region Fields
        protected string content;
        #endregion

        #region Properties
        public string Content
        {
            get { return content; }
        }

        public virtual bool CanStopWithEndOfLine
        {
            get { return false; }
        }

        public abstract Regex Start { get; }

        public abstract Regex End { get; }
        #endregion

        #region Class Methods
        public abstract Symbol Clone(string content);

        protected virtual void Init(string content)
        {
            this.content = content;
        }

        public override string ToString()
        {
            return $@"{GetType().Name}<{content}>";
        }
        #endregion
    }
}
