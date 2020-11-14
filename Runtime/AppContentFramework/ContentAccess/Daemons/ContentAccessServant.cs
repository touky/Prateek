namespace Prateek.Runtime.AppContentFramework.ContentAccess.Daemons
{
    using Prateek.Runtime.Core.HierarchicalTree;
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
