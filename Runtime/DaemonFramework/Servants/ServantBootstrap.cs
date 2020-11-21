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
        protected virtual void Awake()
        {
            servant = new TServant();
            if (servant is IServantInternal internalServant)
            {
                internalServant.Name = name;
            }

            BeforeStartup();

            servant.Startup();
        }

        protected virtual void BeforeStartup() { }

        protected virtual void OnDestroy()
        {
            servant.Shutdown();
        }
        #endregion
    }
}
