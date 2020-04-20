namespace Prateek.CodeGenerator.ScriptTemplates {
    using System;
    using System.Collections.Generic;
    using Prateek.Core.Code.Helpers;

    public struct KeywordTemplateStack
    {
        //-----------------------------------------------------------------
        public struct SwapInfo
        {
            public KeywordTemplate operation;
            public string data;
            public int start;
            public int end;
        }

        //-----------------------------------------------------------------
        private KeywordTemplateMode tagType;
        private string content;
        private List<SwapInfo> stack;

        //-----------------------------------------------------------------
        public bool CanApply { get { return stack.Count > 0; } }

        //-----------------------------------------------------------------
        public KeywordTemplateStack(KeywordTemplateMode tagType, string content)
        {
            this.tagType = tagType;
            this.content = content;
            this.stack = new List<SwapInfo>();
        }

        //-----------------------------------------------------------------
        public void Add(KeywordTemplate operation, int start, int end)
        {
            stack.Add(new SwapInfo() { operation = operation, start = start, end = end });
        }

        //-----------------------------------------------------------------
        public void Add(string data, int start, int end)
        {
            stack.Add(new SwapInfo() { data = data, start = start, end = end });
        }

        //-----------------------------------------------------------------
        public void Reset()
        {
            content = String.Empty;
            stack.Clear();
        }

        //-----------------------------------------------------------------
        public string Apply()
        {
            stack.Sort((a, b) => { return a.start - b.start; });

            var result = content;
            for (int s = stack.Count - 1; s >= 0; s--)
            {
                var data = stack[s];
                result = result.Substring(0, data.start)
                       + (data.operation != null
                             ? data.operation.Content.CleanText()
                             : data.data)
                       + result.Substring(data.end);
            }
            return result;
        }
    }
}