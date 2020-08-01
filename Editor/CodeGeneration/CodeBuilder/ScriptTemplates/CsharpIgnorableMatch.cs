namespace Prateek.CodeGeneration.CodeBuilder.Editor.ScriptTemplates {
    using System;

    internal struct CsharpIgnorableMatch
    {
        ///-------------------------------------------------------------
        public IgnorableStyle type;
        public bool isLine;
        public string start;
        public string ignore;
        public string end;

        ///-------------------------------------------------------------
        public CsharpIgnorableMatch(IgnorableStyle type, string start, string end, bool isLine = false) : this(type, start, String.Empty, end, isLine) { }
        public CsharpIgnorableMatch(IgnorableStyle type, string start, string ignore, string end, bool isLine = false)
        {
            this.type = type;
            this.isLine = isLine;
            this.start = start;
            this.ignore = ignore;
            this.end = end;
        }
    }
}