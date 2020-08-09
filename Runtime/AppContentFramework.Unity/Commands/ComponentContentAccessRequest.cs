namespace Prateek.Runtime.AppContentFramework.Unity.Commands
{
    using Prateek.Runtime.AppContentFramework.Messages;
    using UnityEngine;

    public class ComponentContentAccessRequest<TContentType>
        : ContentAccessRequest<TContentType>
        where TContentType : Component
    {
        #region Class Methods
        protected override bool ValidateResponse()
        {
            return holder.Validate<ComponentContentAccessResponse<TContentType>>();
        }
        #endregion
    }
}
