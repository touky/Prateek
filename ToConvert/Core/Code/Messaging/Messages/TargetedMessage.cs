namespace Mayfair.Core.Code.Messaging.Messages
{
    using System.Diagnostics;

    [DebuggerDisplay("{GetType().Name}, Sender: {sender.Owner.Name}")]
    public abstract class TargetedMessage : Message { }
}
