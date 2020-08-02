namespace Mayfair.Core.Code.Database.Messages
{
    using Prateek.A_TODO.Runtime.AppContentUnityIntegration.Messages;
    using UnityEngine;

    public class DatabaseHasChanged<TResourceType> : ScriptableResourcesHaveChanged<TResourceType>
        where TResourceType : ScriptableObject { }
}
