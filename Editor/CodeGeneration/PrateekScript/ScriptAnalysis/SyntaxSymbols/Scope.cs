namespace Prateek.Editor.CodeGeneration.PrateekScript.ScriptAnalysis.SyntaxSymbols
{
    using System;
    using System.Text.RegularExpressions;
    using Mono.CompilerServices.SymbolWriter;

    public abstract class Scope<TSymbol>
        : Symbol<TSymbol>
        , IScope
        where TSymbol : Symbol<TSymbol>, new()
    {
        public Symbol GetOpenSymbol()
        {
            return new Open(Start);
        }

        public Symbol GetCloseSymbol()
        {
            return new Close(End);
        }

        public virtual Symbol GetSeparator()
        {
            return null;
        }

        public class Open
            : Symbol<Open>
            , IScopeOpen
        {
            private Regex regex;
            
            public Open()
            {
            }

            public Open(Regex regex)
            {
                this.regex = regex;
            }

            public bool Match(IScopeClose other)
            {
                return typeof(Close) == other.GetType();
            }

            #region Properties
            public override Regex Start
            {
                get { return regex; }
            }

            public override Regex End
            {
                get { return null; }
            }
            #endregion
        }

        public class Close
            : Symbol<Close>
            , IScopeClose
        {
            private Regex regex;

            public Close()
            {
            }

            public Close(Regex regex)
            {
                this.regex = regex;
            }

            #region Properties
            public override Regex Start
            {
                get { return regex; }
            }

            public override Regex End
            {
                get { return null; }
            }
            #endregion
        }
    }
}
