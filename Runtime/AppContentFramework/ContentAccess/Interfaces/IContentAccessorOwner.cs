namespace Prateek.Runtime.AppContentFramework.ContentAccess.Interfaces
{
    using Prateek.Runtime.CommandFramework.EmitterReceiver.Interfaces;

    public interface IContentAccessorOwner
        : ICommandReceiverOwner
    {
        #region Class Methods
        void SetupContentAccess(ContentAccessor contentAccessor);
        #endregion
    }
}
