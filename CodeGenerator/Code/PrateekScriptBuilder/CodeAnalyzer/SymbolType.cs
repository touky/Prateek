namespace Assets.Prateek.CodeGenerator.Code.PrateekScriptBuilder.CodeAnalyzer
{
    using System.Diagnostics;
    using System.Text.RegularExpressions;
    using UnityEngine.PlayerLoop;

    public enum SymbolType
    {
        WhiteSpace,
        LineFeed,
        Numeric,
        Letter,
        Literal,
        LiteralEnd,
        ScopeStart,
        ScopeEnd,
        CallStart,
        CallEnd,
        ArgSplit,
        Comment,

        MAX
    }

    [DebuggerDisplay("{Start?.ToString()}<{Content}>{End?.ToString()}")]
    public abstract class Symbol
    {
        protected string content;
        public string Content
        {
            get { return content; }
        }
        public abstract Regex Start { get; }
        public abstract Regex End { get; }

        public abstract Symbol Clone(string content);

        protected virtual void Init(string content)
        {
            this.content = content;
        }

        public override string ToString()
        {
            return $@"{GetType().Name}<{content}>";
    }
    }

    public abstract class Symbol<TSymbol> : Symbol
        where TSymbol : Symbol<TSymbol>, new()
    {
        public override Symbol Clone(string content)
        {
            var clone = new TSymbol();
            clone.Init(content);
            return clone;
        }
    }

    public interface IComment { }

    public class MultilineComment : Symbol<MultilineComment>, IComment
    {
        private static readonly Regex START = new Regex($@"/\*");
        private static readonly Regex END = new Regex($@"\*/");

        public override Regex Start
        {
            get { return START; }
        }

        public override Regex End
        {
            get { return END; }
        }
    }

    public class SingleLineComment : Symbol<SingleLineComment>, IComment
    {
        private static readonly Regex START = new Regex($@"//");
        private static readonly Regex END = new Regex($@"([\r\n$]+)");

        public override Regex Start
        {
            get { return START; }
        }

        public override Regex End
        {
            get { return END; }
        }
    }

    public class LiteralValue : Symbol<LiteralValue>
    {
        private static readonly Regex START = new Regex($@"\@");
        private static readonly Regex END = new Regex($@"([\r\n\$])");

        public override Regex Start
        {
            get { return START; }
        }

        public override Regex End
        {
            get { return END; }
        }
    }

    public class VariableSeparator : Symbol<VariableSeparator>
    {
        private static readonly Regex START = new Regex(",");

        public override Regex Start
        {
            get { return START; }
        }

        public override Regex End
        {
            get { return null; }
        }
    }

    public abstract class Scope<TSymbol> : Symbol<TSymbol>
        where TSymbol : Symbol<TSymbol>, new()
    {
        public override Regex End
        {
            get { return null; }
        }
    }

    public class CodeBeginScope : Scope<CodeBeginScope>
    {
        private static readonly Regex START = new Regex("{");

        public override Regex Start
        {
            get { return START; }
        }
    }

    public class CodeEndScope : Scope<CodeEndScope>
    {
        private static readonly Regex START = new Regex("}");

        public override Regex Start
        {
            get { return START; }
        }
    }

    public class InvokeBeginScope : Scope<InvokeBeginScope>
    {
        private static readonly Regex START = new Regex($"\\(");

        public override Regex Start
        {
            get { return START; }
        }
    }

    public class InvokeEndScope : Scope<InvokeEndScope>
    {
        private static readonly Regex START = new Regex($"\\)");

        public override Regex Start
        {
            get { return START; }
        }
    }

    public class Keyword : Symbol<Keyword>
    {
        private static readonly Regex START = new Regex($"([a-zA-Z]+[a-zA-Z0-9_.]*)");

        public override Regex Start
        {
            get { return START; }
        }
        public override Regex End
        {
            get { return null; }
        }
    }

    public class Value : Symbol<Keyword>
    {
        private static readonly Regex START = new Regex($"([0-9]+)");

        public override Regex Start
        {
            get { return START; }
        }
        public override Regex End
        {
            get { return null; }
        }
    }
}