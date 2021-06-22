namespace Prateek.Editor.AtomicFileFramework
{
    using System.Reflection;
    using Prateek.Runtime.Core.AssemblyForager;

#if USE_SCENE_SPLITTER
    internal class FileFormatForagerWorker : AssemblyForagerWorker
    {
        #region Class Methods
        public override void PrepareSearch()
        {
            Search<AtomicFileFormat>(SearchFlag.Abstract);
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
