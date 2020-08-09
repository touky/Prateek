namespace Prateek.Runtime.AppContentFramework.Unity.Addressables
{
    using Prateek.Runtime.AppContentFramework.Loader.Interfaces;
    using UnityEngine;

    /// <summary>
    ///     This component exists to automatically decrement its content reference on death
    /// </summary>
    public class GameObjectContentReference : MonoBehaviour
    {
        #region Fields
        private IContentReference contentReference;
        #endregion

        #region Unity Methods
        private void OnDestroy()
        {
            contentReference.DecrementReferences();
        }
        #endregion

        #region Class Methods
        public void Init(IContentReference contentReference)
        {
            this.contentReference = contentReference;

            //Immediatly increment the instance count
            contentReference.IncrementReferences();
        }
        #endregion
    }
}
