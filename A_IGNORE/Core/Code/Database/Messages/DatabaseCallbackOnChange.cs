namespace Mayfair.Core.Code.Database.Messages
{
    using Prateek.Runtime.AppContentFramework.Unity.Commands;
    using UnityEngine;

    public class DatabaseCallbackOnChange<TContentType>
        : ScriptableContentAccessRequest<TContentType>
        where TContentType : ScriptableObject
    {
        protected override bool ValidateResponse()
        {
            return holder.Validate<DatabaseHasChangedResponse<TContentType>>();
        }
    }
}

