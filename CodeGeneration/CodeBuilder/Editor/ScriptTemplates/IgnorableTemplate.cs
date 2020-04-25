namespace Prateek.CodeGenerator.ScriptTemplates {
    public abstract class IgnorableTemplate : BaseTemplate
    {
        ///-----------------------------------------------------------------
        public IgnorableTemplate(string extension) : base(extension) { }

        ///-----------------------------------------------------------------
        public override void Commit()
        {
            CodeGenerator.TemplateRegistry.Add(this);
        }

        ///-----------------------------------------------------------------
        public abstract IgnorableContent Build(string content);
    }
}