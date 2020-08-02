namespace Prateek.A_TODO.Runtime.CommandFramework.EmitterReceiver.Interfaces
{
    using Prateek.A_TODO.Runtime.CommandFramework.Commands.Core;

    public interface ICommandCallbackProxy
    {
        #region Class Methods
        void Invoke(Command command);
        #endregion
    }
}
