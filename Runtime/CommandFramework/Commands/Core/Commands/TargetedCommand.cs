namespace Prateek.Runtime.CommandFramework.Commands.Core
{
    using System.Diagnostics;

    /// <summary>
    ///     This command can only be received by one recipent, any other trying to catch it will throw an exception
    /// </summary>
    [DebuggerDisplay("{GetType().Name}, Sender: {emitter.Owner.Name}")]
    public abstract class TargetedCommand : Command { }
}
