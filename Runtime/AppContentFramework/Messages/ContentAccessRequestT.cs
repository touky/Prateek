namespace Prateek.Runtime.AppContentFramework.Messages
{
    using System;

    public abstract class ContentAccessRequest<TContentType>
        : ContentAccessRequest
    {
        #region Properties
        protected override Type ContentType
        {
            get { return typeof(TContentType); }
        }
        #endregion
    }
}
