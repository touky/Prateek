namespace Assets.Prateek.ToConvert.Messaging.Messages
{
    using System.Diagnostics;

    [DebuggerDisplay("{GetType().Name}, Sender: {sender.Owner.Name}")]
    public abstract class BroadcastMessage : Message { }
}
