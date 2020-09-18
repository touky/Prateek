namespace Prateek.Runtime.CommandFramework.EmitterReceiver
{
    using Prateek.Runtime.CommandFramework.Commands.Core;

    public delegate void CommandAction<T>(T notice)
        where T : Command;
}
