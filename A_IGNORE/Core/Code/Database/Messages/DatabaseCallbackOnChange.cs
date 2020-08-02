namespace Mayfair.Core.Code.Database.Messages
{
    using Prateek.A_TODO.Runtime.AppContentUnityIntegration.Messages;
    using UnityEngine;

    public class DatabaseCallbackOnChange<TResourceType>
        : RequestCallbackOnScriptableResourceChange<DatabaseHasChanged<TResourceType>, TResourceType>
        where TResourceType : ScriptableObject
    {
    }
}

