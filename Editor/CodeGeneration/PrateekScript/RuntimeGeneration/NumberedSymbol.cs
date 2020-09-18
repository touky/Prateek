namespace Prateek.Editor.CodeGeneration.PrateekScript.RuntimeGeneration
{
    using System.Collections.Generic;
    using Prateek.Editor.CodeGeneration.CodeBuilder.Utils;
    using Prateek.Runtime.Core.Consts;
    using Prateek.Runtime.Core.Helpers;

    public struct NumberedSymbol
    {
        ///-------------------------------------------------------------
        private const int generatedCount = 20;
        private const string defaultNumber = "N";
        private const string symbolFormat = "{0}_{1}";

        ///-------------------------------------------------------------
        private string defaultSymbol;
        private List<string> numberedSymbols;

        ///-------------------------------------------------------------
        public int Count
        {
            get { return numberedSymbols.Count; }
        }

        ///-------------------------------------------------------------
        public StringSwap DefaultSymbol
        {
            get { return defaultSymbol; }
        }

        ///-------------------------------------------------------------
        public StringSwap this[int i]
        {
            get
            {
                if (i == Const.INDEX_NONE)
                {
                    return defaultNumber;
                }

                if (i < Const.INDEX_NONE || i >= numberedSymbols.Count)
                {
                    return string.Empty;
                }

                return numberedSymbols[i];
            }
        }

        ///-------------------------------------------------------------
        public NumberedSymbol(VariableKeyword root)
        {
            numberedSymbols = new List<string>();
            defaultSymbol = string.Format(symbolFormat, root, defaultNumber).Keyword();
            for (var i = 0; i < generatedCount; i++)
            {
                numberedSymbols.Add(string.Format(symbolFormat, root, i).Keyword());
            }
        }

        ///-------------------------------------------------------------
        public bool HasAny(string code)
        {
            foreach (var symbol in numberedSymbols)
            {
                if (code.Contains(symbol))
                {
                    return true;
                }
            }

            return false;
        }

        public int FindCount(string code)
        {
            int count = 0;
            foreach (var symbol in numberedSymbols)
            {
                if (!code.Contains(symbol))
                {
                    continue;
                }

                count++;
            }

            return count;
        }
    }
}
