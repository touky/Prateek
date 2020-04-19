namespace Assets.Prateek.CodeGenerator.Code.PrateekScriptBuilder.CodeCommands {
    using System.Collections.Generic;
    using Assets.Prateek.CodeGenerator.Code.PrateekScriptBuilder.CodeAnalyzer.Symbols;

    public abstract class CodeCommand
    {
        protected List<IComment> comments = new List<IComment>();

        public virtual void Add(CodeCommand other)
        {
            if (other.comments.Count > 0)
            {
                comments.AddRange(other.comments);
                other.comments.Clear();
            }
        }

        public void Set(IComment comment)
        {
            comments.Add(comment);
        }

        public virtual bool AllowInternalScope
        {
            get { return false; }
        }
    }
}