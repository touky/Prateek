namespace Mayfair.Core.Code.Resources.Messages
{
    using System;

    public abstract class RequestCallbackOnChange<TResourceType> : RequestCallbackOnChange
    {
        #region Properties
        protected override Type ResourceType
        {
            get { return typeof(TResourceType); }
        }
        #endregion
    }
}
