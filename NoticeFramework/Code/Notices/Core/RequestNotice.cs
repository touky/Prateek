namespace Prateek.NoticeFramework.Notices.Core
{
    using System.Diagnostics;

    [DebuggerDisplay("{GetType().Name}, Sender: {transmitter.Owner.Name}")]
    public abstract class RequestNotice : TargetedNotice
    {
        #region Class Methods
        public ResponseNotice GetResponse()
        {
            ResponseNotice response = CreateNewResponse();
            response.Init(this);
            return response;
        }

        protected abstract ResponseNotice CreateNewResponse();
        #endregion
    }
}
