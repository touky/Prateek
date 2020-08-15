namespace Prateek.Runtime.CommandFramework.Commands.Core
{
    using Prateek.Runtime.CommandFramework.EmitterReceiver.Interfaces;

    public interface IRequestCommand
    {
        #region Properties
        ICommandEmitter Emitter { get; }
        #endregion

        #region Class Methods
        TResponse GetResponse<TResponse>(bool requestFailed = false)
            where TResponse : ResponseCommand;
        #endregion
    }
}
