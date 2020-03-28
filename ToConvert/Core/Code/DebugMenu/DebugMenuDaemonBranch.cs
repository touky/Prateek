namespace Mayfair.Core.Code.DebugMenu
{
    using Mayfair.Core.Code.DebugMenu.Content;
    using Mayfair.Core.Code.DebugMenu.Pages;
    using Mayfair.Core.Code.Service;
    using Prateek.DaemonCore.Code.Branches;

    public abstract class DebugMenuDaemonBranch : DaemonBranchBehaviour<DebugMenuDaemonCore, DebugMenuDaemonBranch>
    {
        #region Properties
        protected override bool IsAliveInternal
        {
            get { return true; }
        }

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
