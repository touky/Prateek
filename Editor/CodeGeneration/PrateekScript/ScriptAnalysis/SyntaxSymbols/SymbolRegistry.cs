namespace Prateek.Editor.CodeGeneration.PrateekScript.ScriptAnalysis.SyntaxSymbols
{
    using System.Collections.Generic;

    internal static class SymbolRegistry
    {
        #region Static and Constants
        private static List<Symbol> symbols = null;
        #endregion

        #region Properties
        public static IReadOnlyList<Symbol> Symbols
        {
            get
            {
                if (symbols == null)
                {
                    Init();
                }

                return symbols;
            }
        }
        #endregion

        #region Class Methods
        public static void Init()
        {
            symbols = new List<Symbol>();
            symbols.Add(new CommentMultiline());
            symbols.Add(new CommentSingleLine());
            symbols.Add(new LiteralValue());
            symbols.Add(new ScopeCodeBegin());
            symbols.Add(new ScopeCodeEnd());
            symbols.Add(new ScopeInvokeBegin());
            symbols.Add(new ScopeInvokeEnd());
            symbols.Add(new VariableSeparator());
            symbols.Add(new Keyword());
            symbols.Add(new Value());
        }
        #endregion
    }
}
