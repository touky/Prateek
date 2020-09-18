namespace Prateek.Editor.CodeGeneration.CodeBuilder.Utils
{
    using System;
    using System.Diagnostics;
    using Prateek.Runtime.Core.Helpers;

    [DebuggerDisplay("{original} -> {replacement}")]
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
            set{ replacement = value; }
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
            return new StringSwap()
            {
                original = info.original,
                replacement = (string.IsNullOrEmpty(info.replacement) ? string.Empty : info.replacement) + other
            };
        }

        ///-------------------------------------------------------------
        public int IndexOf(string text)
        {
            return text.IndexOf(original, StringComparison.InvariantCulture);
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
            {
                return text;
            }

            return text.Replace(original, !replacement.EndsWith(Strings.Separator.LineFeed.S())
                ? replacement
                : replacement.Substring(0, replacement.Length - 1));
        }
    }
}
