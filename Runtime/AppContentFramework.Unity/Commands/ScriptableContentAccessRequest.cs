namespace Prateek.Runtime.AppContentFramework.Unity.Commands
{
    using Prateek.Runtime.AppContentFramework.Messages;
    using UnityEngine;

    public class ScriptableContentAccessRequest<TContentType>
        : ContentAccessRequest<TContentType>
        where TContentType : ScriptableObject
    {
        #region Class Methods
        protected override bool ValidateResponse()
        {
            return holder.Validate<ScriptableContentAccessChangedResponse<TContentType>>();
        }
        #endregion
    }
}
