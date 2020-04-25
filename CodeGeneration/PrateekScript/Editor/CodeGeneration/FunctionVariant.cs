namespace Assets.Prateek.CodeGenerator.Code.PrateekScript.CodeGeneration
{
    using System.Collections.Generic;
    using global::Prateek.Core.Code.Helpers;

    public struct FunctionVariant
    {
        ///-------------------------------------------------------------
        private List<string> results;

        ///-------------------------------------------------------------
        public string Call
        {
            get { return results[0]; }
            set { results[0] += value; }
        }

        public int Count
        {
            get { return results == null ? 0 : results.Count; }
        }

        public string this[int i]
        {
            get
            {
                if (i < 0 || i >= results.Count)
                {
                    return string.Empty;
                }

                return results[i];
            }
            set
            {
                if (i < 0 || i >= results.Count)
                {
                    return;
                }

                var result = results[i];
                Set(ref result, value);
                results[i] = result;
            }
        }

        ///-------------------------------------------------------------
        public FunctionVariant(string value) : this(value, 0) { }

        public FunctionVariant(string value, int emptySlot)
        {
            results = new List<string>();
            results.Add(value);
            while (emptySlot-- > 0)
            {
                results.Add(string.Empty);
            }
        }

        ///-------------------------------------------------------------
        public FunctionVariant(FunctionVariant other)
        {
            results = new List<string>(other.results);
        }

        ///-------------------------------------------------------------
        public void Add(string value)
        {
            results.Add(value);
        }

        ///-------------------------------------------------------------
        private void Set(ref string dst, string value)
        {
            if (!string.IsNullOrEmpty(dst) && !dst.EndsWith(Strings.Separator.LineFeed.S()))
            {
                dst += Glossary.Code.argVarSeparator;
            }

            dst += value;
        }
    }
}
