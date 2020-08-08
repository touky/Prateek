namespace Mayfair.Core.Code.Database.Messages
{
    using Prateek.A_TODO.Runtime.AppContentUnityIntegration.Messages;
    using UnityEngine;

    public class DatabaseHasChangedResponse<TResourceType> : ScriptableContentAccessChangedResponse<TResourceType>
        where TResourceType : ScriptableObject { }
}
