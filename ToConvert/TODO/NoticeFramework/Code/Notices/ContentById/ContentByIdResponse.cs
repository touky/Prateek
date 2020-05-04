namespace Prateek.NoticeFramework.Notices
{
    using System.Collections.Generic;
    using Prateek.NoticeFramework.Notices.Core;

    public abstract class ContentByIdResponse<TContent> : ResponseNotice
    {
        #region Properties
        public List<TContent> Content { get; } = new List<TContent>();
        #endregion
    }
}
