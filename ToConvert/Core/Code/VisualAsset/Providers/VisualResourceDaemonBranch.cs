namespace Mayfair.Core.Code.VisualAsset.Providers
{
    using System.Diagnostics;
    using Mayfair.Core.Code.DebugMenu.Content;
    using Mayfair.Core.Code.DebugMenu.Pages;
    using Mayfair.Core.Code.Resources;
    using Mayfair.Core.Code.VisualAsset.Messages;

    public abstract class VisualResourceDaemonBranch : ResourceDependentDaemonBranch<VisualResourceDaemonCore, VisualResourceDaemonBranch>, IDebugMenuNotebookOwner
    {
        #region Class Methods
        [Conditional("NVIZZIO_DEV")]
        public virtual void SetupDebugContent(DebugMenuNotebook debugNotebook, DebugMenuPage parent)
        {
            debugNotebook.AddPagesWithParent(parent, new VisualResourceMenuPage(this, name));
        }

        public abstract void OnVisualResourceMessage(VisualResourceDirectMessage message);
        #endregion
    }
}
