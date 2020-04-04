namespace Prateek.NoticeFramework.Notices.Core
{
    using System.Diagnostics;

    [DebuggerDisplay("{GetType().Name}, Sender: {transmitter.Owner.Name}")]
    public abstract class RequestNotice<TResponseType> : RequestNotice
        where TResponseType : ResponseNotice, new()
    {
        #region Class Methods
        public new TResponseType GetResponse()
        {
            return base.GetResponse() as TResponseType;
        }

        protected override ResponseNotice CreateNewResponse()
        {
            return Notice.Create<TResponseType>();
        }
        #endregion
    }
}

