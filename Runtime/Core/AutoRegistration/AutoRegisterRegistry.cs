namespace Prateek.Runtime.Core.AutoRegistration
{
    using System.Collections.Generic;
    using Prateek.Runtime.Core.Singleton;

    public class AutoRegisterRegistry
        : Registry<AutoRegisterRegistry>
    {
        #region Fields
        internal List<AutoRegisterForagerWorker> workers = new List<AutoRegisterForagerWorker>();
        #endregion

        #region Properties
        internal static AutoRegisterRegistry Singleton { get { return Instance; } }
        #endregion

        #region Register/Unregister
        protected override void OnAwake() { }

        internal static void Register(IAutoRegister instance)
        {
            foreach (var worker in Instance.workers)
            {
                worker.Register(instance);
            }
        }
        #endregion

        #region Class Methods
        internal static void Unregister(IAutoRegister instance)
        {
            if (Instance == null)
            {
                return;
            }

            foreach (var worker in Instance.workers)
            {
                worker.Unregister(instance);
            }
        }
        #endregion
    }
}
