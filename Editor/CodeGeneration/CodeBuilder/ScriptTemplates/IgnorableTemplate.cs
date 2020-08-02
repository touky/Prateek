namespace Prateek.Editor.CodeGeneration.CodeBuilder.ScriptTemplates {
    using Prateek.Editor.CodeGeneration.CodeBuilder.RuntimeBuilder;

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