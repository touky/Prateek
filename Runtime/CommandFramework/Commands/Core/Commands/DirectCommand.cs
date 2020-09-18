namespace Prateek.Runtime.CommandFramework.Commands.Core
{
    using System.Diagnostics;

    [DebuggerDisplay("{GetType().Name}, Sender: {emitter.Owner.Name}")]
    public abstract class DirectCommand : TargetedCommand { }
}
