namespace Mayfair.Core.Code.VisualAsset.Providers
{
    using System.Diagnostics;
    using Mayfair.Core.Code.DebugMenu.Content;
    using Mayfair.Core.Code.DebugMenu.Pages;
    using Mayfair.Core.Code.VisualAsset.Messages;
    using Prateek.Runtime.AppContentFramework.Daemons;

    public abstract class VisualResourceServant : ContentAccessServant<VisualResourceDaemonOverseer, VisualResourceServant>, IDebugMenuNotebookOwner
    {
        public abstract void RefreshPending();

        #region Class Methods
        [Conditional("PRATEEK_DEBUG")]
        public virtual void SetupDebugContent(DebugMenuNotebook debugNotebook, DebugMenuPage parent)
        {
            debugNotebook.AddPagesWithParent(parent, new VisualResourceMenuPage(this, Name));
        }

        public abstract void OnVisualResourceMessage(VisualResourceDirectCommand command);
        #endregion
    }
}
