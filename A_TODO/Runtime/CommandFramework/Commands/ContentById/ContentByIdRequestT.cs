namespace Prateek.A_TODO.Runtime.CommandFramework.Commands.ContentById
{
    using System.Diagnostics;

    [DebuggerDisplay("{GetType().Name}, Sender: {emitter.Owner.Name}")]
    public abstract class ContentByIdRequest<TNoticeId> : ContentByIdRequest
    {
        #region Properties
        public override long CommandID
        {
            get { return ConvertToId(typeof(TNoticeId)); }
        }
        #endregion
    }
}
