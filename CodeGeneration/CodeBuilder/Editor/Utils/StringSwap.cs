namespace Prateek.CodeGeneration.CodeBuilder.Editor.Utils
{
    using System;
    using Prateek.Core.Code.Helpers;

    public struct StringSwap
    {
        ///-------------------------------------------------------------
        private string original;

        private string replacement;

        ///-------------------------------------------------------------
        public string Original
        {
            get { return original; }
        }

        public string Replacement
        {
            get { return replacement; }
        }

        ///-------------------------------------------------------------
        public StringSwap(string original)
        {
            this.original = original;
            this.replacement = String.Empty;
        }

        ///-------------------------------------------------------------
        public static implicit operator StringSwap(string original)
        {
            return new StringSwap(original);
        }

        ///-------------------------------------------------------------
        public static StringSwap operator +(StringSwap info, string other)
        {
            return new StringSwap() {original = info.original, replacement = other};
        }

        ///-------------------------------------------------------------
        public bool CanSwap(string text)
        {
            return text.Contains(original);
        }

        ///-------------------------------------------------------------
        public string Apply(string text)
        {
            if (text == null || original == null || replacement == null)
                return text;
            return text.Replace(original, !replacement.EndsWith(Strings.Separator.LineFeed.S()) ? replacement : replacement.Substring(0, replacement.Length - 1));
        }
    }
}
