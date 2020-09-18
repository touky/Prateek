namespace Prateek.Runtime.AppContentFramework.Daemons
{
    using Prateek.Runtime.CommandFramework;
    using Prateek.Runtime.DaemonFramework.Servants;
    using UnityEngine;

    public abstract class ContentAccessServant<TDaemon, TServant>
        : Servant<TDaemon, TServant>
        where TDaemon : ContentAccessDaemonOverseer<TDaemon, TServant>
        where TServant : ContentAccessServant<TDaemon, TServant>
    {
        #region Settings
        [SerializeField]
        protected ContentAccessSettings contentAccessSettings = new ContentAccessSettings();
        #endregion

        #region Class Methods
        public abstract void SetupContentAccess(ContentAccessor contentAccessor);
        #endregion
    }
}
