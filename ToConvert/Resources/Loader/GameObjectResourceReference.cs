namespace Assets.Prateek.ToConvert.Resources.Loader
{
    using System.Diagnostics;
    using UnityEngine;

    [DebuggerDisplay("{loader.resource.ToString()}, Location: {loader.location}")]
    public class GameObjectResourceReference : UnityObjectResourceReference<GameObject, GameObjectResourceReference>
    {
        #region Constructors
        public GameObjectResourceReference(ResourceLoader loader) : base(loader) { }
        #endregion

        #region Class Methods
        protected override GameObject GetGameObject(GameObject resourceType)
        {
            return resourceType;
        }
        #endregion
    }
}
