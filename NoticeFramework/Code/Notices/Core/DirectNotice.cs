namespace Prateek.NoticeFramework.Notices.Core
{
    using System.Diagnostics;

    [DebuggerDisplay("{GetType().Name}, Sender: {transmitter.Owner.Name}")]
    public abstract class DirectNotice : TargetedNotice { }
}
