namespace Mayfair.Core.Code.Resources.Loader
{
    using System.Diagnostics;
    using UnityEngine;

    [DebuggerDisplay("{loader.content.ToString()}, Location: {loader.location}")]
    public class GameObjectContentHandle : UnityObjectContentHandle<GameObject, GameObjectContentHandle>
    {
        #region Constructors
        public GameObjectContentHandle(ContentLoader loader) : base(loader) { }

        protected override GameObject GetGameObject(GameObject resourceType)
        {
            return resourceType;
        }
        #endregion
    }
}
