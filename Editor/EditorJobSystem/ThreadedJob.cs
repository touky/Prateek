namespace Prateek.EditorJobSystem.Code
{
    public abstract class ThreadedJob
    {
        #region Properties
        public abstract bool DispatchInOrder { get; }
        #endregion

        #region Class Methods
        public abstract bool Execute();
        #endregion
    }
}
