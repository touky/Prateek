namespace Prateek.A_TODO.Runtime.CommandFramework.Commands.Core
{
    using Prateek.A_TODO.Runtime.CommandFramework.EmitterReceiver.Interfaces;

    public interface IRequestCommand
    {
        ICommandEmitter Emitter { get; }
        ResponseCommand GetResponse(bool requestFailed = false);
    }
}
