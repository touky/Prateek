namespace Prateek.CodeGeneration.CodeBuilder.Editor.ScriptTemplates {
    using System;
    using Prateek.CodeGeneration.CodeBuilder.Editor.CodeBuilder;
    using Prateek.Core.Code.Helpers;

    public class KeywordTemplate : BaseTemplate
    {
        public static KeywordTemplate Create(string extension)
        {
            return new KeywordTemplate(extension);
        }
        ///-----------------------------------------------------------------
        protected string tag;
        protected KeywordTemplateMode templateMode;

        ///-----------------------------------------------------------------
        public KeywordTemplateMode TemplateMode { get { return templateMode; } }
        public string Tag { get { return tag.Keyword(); } }
        public string TagBegin { get { return tag.KeywordBegin(); } }
        public string TagEnd { get { return tag.KeywordEnd(); } }

        ///-----------------------------------------------------------------
        public KeywordTemplate(string extension) : base(extension) { }

        ///-----------------------------------------------------------------
        public KeywordTemplate SetTag(string tag, KeywordTemplateMode tagStyle = KeywordTemplateMode.UsedAsSwap)
        {
            this.tag = tag;
            this.templateMode = tagStyle;
            return this;
        }

        ///-----------------------------------------------------------------
        public override BaseTemplate SetContent(string content)
        {
            base.SetContent(content);

            if (String.IsNullOrEmpty(content))
            {
                return this;
            }

            if (templateMode == KeywordTemplateMode.UsedAsScope)
            {
                this.Content = TagBegin + content + TagEnd;
            }
            return this;
        }

        ///-----------------------------------------------------------------
        public override void Commit()
        {
            TemplateRegistry.Add(this);
        }
    }
}