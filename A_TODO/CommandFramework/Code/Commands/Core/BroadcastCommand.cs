namespace Commands.Core
{
    using System.Diagnostics;

    [DebuggerDisplay("{GetType().Name}, Sender: {emitter.Owner.Name}")]
    public abstract class BroadcastCommand : Command { }
}
