namespace Prateek.Runtime.Core.AssemblyForager
{
    public abstract class AssemblyForagerWorker<TWorker>
        : AssemblyForagerWorker
        where TWorker : AssemblyForagerWorker
    {
        #region Static and Constants
        private static AssemblyForagerWorker<TWorker> instance = null;
        #endregion

        #region Properties
        public static TWorker Instance { get { return instance as TWorker; } }
        #endregion

        #region Class Methods
        internal override void Init()
        {
            instance = this;

            base.Init();
        }
        #endregion
    }
}
