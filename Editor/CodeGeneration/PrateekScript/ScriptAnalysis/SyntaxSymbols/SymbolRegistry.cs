namespace Prateek.Editor.CodeGeneration.PrateekScript.ScriptAnalysis.SyntaxSymbols
{
    using System;
    using System.Collections.Generic;
    using Prateek.Editor.CodeGeneration.PrateekScript.ScriptAnalysis.IntermediateRuntime;

    public abstract class SymbolRegistry
    {
        #region Static and Constants
        private List<Symbol> symbols = null;
        private Dictionary<Type, Func<CodeCommand>> symbolToCommand = null;
        #endregion

        #region Properties
        public IReadOnlyList<Symbol> Symbols
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

        public IReadOnlyDictionary<Type, Func<CodeCommand>> SymbolToCommand
        {
            get
            {
                if (symbolToCommand == null)
                {
                    Init();
                }

                return symbolToCommand;
            }
        }
        #endregion

        #region Class Methods
        public virtual void Init()
        {
            symbols = new List<Symbol>();
            symbolToCommand = new Dictionary<Type, Func<CodeCommand>>();
        }

        public void Bind<TSymbol, TCodeCommand, TSeparator>()
            where TSymbol : Symbol, new()
            where TCodeCommand : InvokeScope<TSeparator>, new()
            where TSeparator : Symbol, ISeparator, new()
        {
            Bind<TSymbol, TCodeCommand>();
            symbols.Add(new TSeparator());
        }

        public void Bind<TSymbol, TCodeCommand>()
            where TSymbol : Symbol, new()
            where TCodeCommand : CodeCommand, new()
        {
            var symbol = new TSymbol();
            if (symbol is IScope scope)
            {
                var openSymbol = scope.GetOpenSymbol();
                symbolToCommand.Add(openSymbol.GetType(), () => new TCodeCommand());
                symbols.Add(openSymbol);
                symbols.Add(scope.GetCloseSymbol());
            }
            else
            {
                symbols.Add(symbol);
                symbolToCommand.Add(typeof(TSymbol), () => new TCodeCommand());
            }
        }
        #endregion
    }
}
