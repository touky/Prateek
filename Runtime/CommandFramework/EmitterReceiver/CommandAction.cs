namespace Prateek.A_TODO.Runtime.CommandFramework.EmitterReceiver
{
    using Prateek.A_TODO.Runtime.CommandFramework.Commands.Core;

    public delegate void CommandAction<T>(T notice)
        where T : Command;
}
