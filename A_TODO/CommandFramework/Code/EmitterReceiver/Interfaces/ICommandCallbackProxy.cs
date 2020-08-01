namespace Prateek.CommandFramework.TransmitterReceiver
{
    using Commands.Core;

    public interface ICommandCallbackProxy
    {
        #region Class Methods
        void Invoke(Command command);
        #endregion
    }
}
