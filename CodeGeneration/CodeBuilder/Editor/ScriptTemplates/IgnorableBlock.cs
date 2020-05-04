namespace Prateek.CodeGeneration.CodeBuilder.Editor.ScriptTemplates {
    public struct IgnorableBlock
    {
        ///-------------------------------------------------------------
        private IgnorableStyle ignorableStyle;
        private bool isLine;
        private int start;
        private int end;

        ///-------------------------------------------------------------
        public IgnorableStyle IgnorableStyle { get { return ignorableStyle; } }
        public bool IsLine { get { return isLine; } }
        public int Start { get { return start; } }
        public int End { get { return end; } }
        public int OverStart { get { return start - 1; } }
        public int OverEnd { get { return end + 1; } }

        ///-------------------------------------------------------------
        public IgnorableBlock(IgnorableStyle type, bool isLine, int start, int end)
        {
            this.ignorableStyle = type;
            this.isLine = isLine;
            this.start = start;
            this.end = end;
        }

        ///-------------------------------------------------------------
        public bool Contains(int index)
        {
            if (start <= index && index <= end)
                return true;
            return false;
        }
    }
}