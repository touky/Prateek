namespace Mayfair.Core.Code.Resources.Messages
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
