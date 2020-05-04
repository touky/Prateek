namespace Mayfair.Core.Code.Database.Messages
{
    using Mayfair.Core.Code.Resources.Messages;
    using UnityEngine;

    public class DatabaseHasChanged<TResourceType> : ScriptableResourcesHaveChanged<TResourceType>
        where TResourceType : ScriptableObject { }
}
