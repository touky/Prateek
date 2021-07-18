namespace Prateek.Editor.EditorJobSystem
{
    using Prateek.Runtime.JobFramework;

    public abstract class EditorJob
        : RuntimeJob
    {
        #region Properties
        public abstract bool DispatchInOrder { get; }
        #endregion
    }
}