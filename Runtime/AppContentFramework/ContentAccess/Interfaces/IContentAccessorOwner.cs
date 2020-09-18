namespace Prateek.Runtime.AppContentFramework.Daemons
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
