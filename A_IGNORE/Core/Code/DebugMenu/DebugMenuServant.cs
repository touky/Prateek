namespace Mayfair.Core.Code.DebugMenu
{
    using Mayfair.Core.Code.DebugMenu.Content;
    using Mayfair.Core.Code.DebugMenu.Pages;
    using Prateek.Runtime.DaemonFramework.Servants;

    public abstract class DebugMenuServant : ServantTickable<DebugMenuDaemon, DebugMenuServant>
    {
        #region Properties
        public override int Priority
        {
            get { return 0; }
        }
        #endregion

        #region Class Methods
        public abstract void AddDebugContent(DebugMenuNotebook notebook, DebugMenuPage rootPage);
        #endregion
    }
}