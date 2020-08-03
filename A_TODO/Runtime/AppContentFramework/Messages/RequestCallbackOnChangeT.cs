namespace Prateek.A_TODO.Runtime.AppContentFramework.Messages
{
    using System;

    public abstract class RequestAccessToContent<TResourceType>
        : RequestAccessToContent
    {
        #region Properties
        protected override Type ResourceType
        {
            get { return typeof(TResourceType); }
        }
        #endregion
    }
}