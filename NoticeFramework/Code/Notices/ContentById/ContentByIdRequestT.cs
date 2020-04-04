namespace Prateek.NoticeFramework.Notices
{
    using System.Diagnostics;

    [DebuggerDisplay("{GetType().Name}, Sender: {transmitter.Owner.Name}")]
    public abstract class ContentByIdRequest<TNoticeId> : ContentByIdRequest
    {
        #region Properties
        public override long NoticeID
        {
            get { return ConvertToId(typeof(TNoticeId)); }
        }
        #endregion
    }
}
