namespace Prateek.Editor.AtomicFileFramework
{
    using Prateek.Runtime.Core.AssemblyForager;

#if USE_SCENE_SPLITTER
    internal class FileFormatForagerWorker : AssemblyForagerWorker
    {
        #region Properties
        public override bool IgnoreAbstract { get { return true; } }
        #endregion

        #region Class Methods
        public override void Init()
        {
            Search(typeof(AtomicFileFormat));
        }

        public override void WorkDone()
        {
            base.WorkDone();

            AtomicFileFormatter.Instance.RegisterAll(FoundTypes);
        }
        #endregion
    }
#endif
}
