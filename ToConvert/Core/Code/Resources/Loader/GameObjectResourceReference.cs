namespace Mayfair.Core.Code.Resources.Loader
{
    using System.Diagnostics;
    using UnityEngine;

    [DebuggerDisplay("{loader.resource.ToString()}, Location: {loader.location}")]
    public class GameObjectResourceReference : UnityObjectResourceReference<GameObject, GameObjectResourceReference>
    {
        #region Constructors
        public GameObjectResourceReference(ResourceLoader loader) : base(loader) { }

        protected override GameObject GetGameObject(GameObject resourceType)
        {
            return resourceType;
        }
        #endregion
    }
}
