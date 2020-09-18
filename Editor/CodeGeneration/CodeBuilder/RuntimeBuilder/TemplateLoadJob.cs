namespace Prateek.Editor.CodeGeneration.CodeBuilder.RuntimeBuilder
{
    using Prateek.Editor.CodeGeneration.CodeBuilder.ScriptTemplates;
    using Prateek.Editor.EditorJobSystem;

    public class TemplateLoadJob : ThreadedJob
    {
        #region Fields
        public string contentPath;
        public BaseTemplate template;
        #endregion

        #region Properties
        public override bool DispatchInOrder
        {
            get { return true; }
        }
        #endregion

        #region Class Methods
        public void Commit()
        {
            EditorJobSystem.AddWork(this);
        }

        public override bool Execute()
        {
            template.LoadFile(contentPath).Commit();
            return true;
        }
        #endregion
    }
}
