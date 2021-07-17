namespace Prateek.Runtime.CommandFramework.Commands.Core.Interfaces
{
    using Prateek.Runtime.CommandFramework.Commands.Core.Commands;
    using Prateek.Runtime.CommandFramework.Gadgets;

    public interface IRequestCommand
    {
        #region Properties
        CommandTools.IEmitter Emitter { get; }
        #endregion

        #region Class Methods
        TResponse GetResponse<TResponse>(bool requestFailed = false)
            where TResponse : ResponseCommand;
        #endregion
    }
}
