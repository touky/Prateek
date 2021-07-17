namespace Prateek.Runtime.CommandFramework.Commands.Core.Commands
{
    using System.Diagnostics;

    [DebuggerDisplay("{GetType().Name}, Sender: {emitter.Owner.Name}")]
    public abstract class DirectCommand : TargetedCommand { }
}
