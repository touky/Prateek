namespace Mayfair.Core.Code.Database.Messages
{
    using Mayfair.Core.Code.Resources.Messages;
    using UnityEngine;

    public class DatabaseCallbackOnChange<TResourceType>
        : RequestCallbackOnScriptableResourceChange<DatabaseHasChanged<TResourceType>, TResourceType>
        where TResourceType : ScriptableObject
    {
    }
}

