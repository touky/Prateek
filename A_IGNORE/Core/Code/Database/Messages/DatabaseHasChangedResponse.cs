namespace Mayfair.Core.Code.Database.Messages
{
    using Prateek.Runtime.AppContentFramework.Unity.Commands;
    using UnityEngine;

    public class DatabaseHasChangedResponse<TContentType> : ScriptableContentAccessChangedResponse<TContentType>
        where TContentType : ScriptableObject { }
}
