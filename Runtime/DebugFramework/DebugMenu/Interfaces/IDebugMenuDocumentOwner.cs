namespace Prateek.Runtime.DebugFramework.DebugMenu.Interfaces
{
    using Prateek.Runtime.GadgetFramework.Interfaces;

    public interface IDebugMenuDocumentOwner
        : IGadgetOwner
        , IDebugMenuOwner
    {
        #region Class Methods
        void SetupDebugDocument(DebugMenuDocument document, out string title);
        #endregion
    }
}
