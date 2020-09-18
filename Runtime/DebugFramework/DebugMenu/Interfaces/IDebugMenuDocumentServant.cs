namespace Prateek.Runtime.DebugFramework.DebugMenu.Interfaces
{
    public interface IDebugMenuDocumentServant
        : IDebugMenuOwner
    {
        #region Class Methods
        void SetupDebugDocument();
        void SetupDebugDocument(DebugMenuDocument document);
        #endregion
    }
}
