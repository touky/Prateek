namespace Prateek.Runtime.DebugFramework.DebugMenu.Interfaces
{
    using Prateek.Runtime.GadgetFramework.Interfaces;

    public interface IDebugMenuDocumentOwner
        : IGadgetOwner
    {
        #region Class Methods
        void SetupDebugDocument(DebugMenuDocument document, out string title);
        #endregion
    }
}
