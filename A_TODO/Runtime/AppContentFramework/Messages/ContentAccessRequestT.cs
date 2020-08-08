namespace Prateek.A_TODO.Runtime.AppContentFramework.Messages
{
    using System;

    public abstract class ContentAccessRequest<TResourceType>
        : ContentAccessRequest
    {
        #region Properties
        protected override Type ResourceType
        {
            get { return typeof(TResourceType); }
        }
        #endregion
    }
}
