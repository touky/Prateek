namespace Prateek.CodeGeneration.CodeBuilder.Editor.CodeBuilder
{
    using Prateek.CodeGeneration.CodeBuilder.Editor.ScriptTemplates;
    using Prateek.EditorJobSystem.Code;

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
