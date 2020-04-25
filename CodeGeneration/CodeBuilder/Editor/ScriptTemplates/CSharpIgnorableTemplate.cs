namespace Prateek.CodeGenerator.ScriptTemplates {
    using System;
    using UnityEngine;

    public class CSharpIgnorableTemplate : IgnorableTemplate
    {
        internal static CSharpIgnorableTemplate Create(string extension)
        {
            return new CSharpIgnorableTemplate(extension);
        }

        ///-----------------------------------------------------------------
        private CsharpIgnorableMatch[] datas = new CsharpIgnorableMatch[]
        {
            new CsharpIgnorableMatch(IgnorableStyle.Comment, "//", "\n", true),
            new CsharpIgnorableMatch(IgnorableStyle.Comment, "/*", "*/"),
            new CsharpIgnorableMatch(IgnorableStyle.Text, "@\"", "\"\"", "\""),
            new CsharpIgnorableMatch(IgnorableStyle.Text, "\"", "\\\"", "\"", true)
        };

        ///-----------------------------------------------------------------
        public CSharpIgnorableTemplate(string extension) : base(extension) { }

        ///-----------------------------------------------------------------
        public override void Commit()
        {
            CodeGenerator.TemplateRegistry.Add(this);
        }

        ///-----------------------------------------------------------------
        public override IgnorableContent Build(string content)
        {
            var result   = default(IgnorableContent);
            var position = 0;
            while (position < content.Length)
            {
                var start  = Int32.MaxValue;
                var foundD = -1;
                for (int d = 0; d < datas.Length; d++)
                {
                    var s0 = content.IndexOf(datas[d].start, position);
                    if (s0 >= 0 && s0 < start)
                    {
                        start = s0;
                        foundD = d;
                    }
                }

                if (foundD < 0)
                    break;

                var data = datas[foundD];
                position = start + data.start.Length;

                var end = position;
                while (true)
                {
                    var ignore = data.ignore == String.Empty ? -1 : content.IndexOf(data.ignore, position);
                    end = content.IndexOf(data.end, position);

                    if (ignore >= 0 && ignore <= end)
                    {
                        position = Mathf.Max(ignore + data.ignore.Length, end + data.end.Length);
                        continue;
                    }

                    break;
                }

                position = end + data.end.Length;
                result.Add(new IgnorableBlock(data.type, data.isLine, start, end + (data.end.Length - 1)));
            }
            return result;
        }
    }
}