namespace Prateek.CodeGeneration.CodeBuilder.Editor.ScriptTemplates {
    using Prateek.CodeGeneration.CodeBuilder.Editor.CodeBuilder;

    public abstract class IgnorableTemplate : BaseTemplate
    {
        ///-----------------------------------------------------------------
        public IgnorableTemplate(string extension) : base(extension) { }

        ///-----------------------------------------------------------------
        public override void Commit()
        {
            TemplateRegistry.Add(this);
        }

        ///-----------------------------------------------------------------
        public abstract IgnorableContent Build(string content);
    }
}