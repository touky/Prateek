namespace Mayfair.Core.Code.VisualAsset.Providers
{
    using System.Diagnostics;
    using Mayfair.Core.Code.DebugMenu.Content;
    using Mayfair.Core.Code.DebugMenu.Pages;
    using Mayfair.Core.Code.VisualAsset.Messages;
    using Prateek.A_TODO.Runtime.AppContentFramework.Daemons;

    public abstract class VisualResourceServant : ContentAccessServant<VisualResourceDaemon, VisualResourceServant>, IDebugMenuNotebookOwner
    {
        public abstract void RefreshPending();

        #region Class Methods
        [Conditional("NVIZZIO_DEV")]
        public virtual void SetupDebugContent(DebugMenuNotebook debugNotebook, DebugMenuPage parent)
        {
            debugNotebook.AddPagesWithParent(parent, new VisualResourceMenuPage(this, Name));
        }

        public abstract void OnVisualResourceMessage(VisualResourceDirectCommand command);
        #endregion
    }
}