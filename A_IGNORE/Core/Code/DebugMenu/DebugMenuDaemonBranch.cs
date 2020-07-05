namespace Mayfair.Core.Code.DebugMenu
{
    using Mayfair.Core.Code.DebugMenu.Content;
    using Mayfair.Core.Code.DebugMenu.Pages;
    using Prateek.DaemonFramework.Code.Branches;

    public abstract class DebugMenuDaemonBranch : DaemonBranchTickableBehaviour<DebugMenuDaemonCore, DebugMenuDaemonBranch>
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
