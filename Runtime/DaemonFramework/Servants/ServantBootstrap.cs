namespace Prateek.Runtime.DaemonFramework.Servants
{
    using Prateek.Runtime.DaemonFramework.Interfaces;
    using UnityEngine;

    public abstract class ServantBootstrap<TServant>
        : MonoBehaviour
        where TServant : class, IServant, new()
    {
        protected TServant servant;

        #region Unity Methods
        private void Awake()
        {
            servant = new TServant();
            if (servant is IServantInternal internalServant)
            {
                internalServant.Name = name;
            }
            servant.Startup();
        }

        private void OnDestroy()
        {
            servant.Shutdown();
        }
        #endregion
    }
}
