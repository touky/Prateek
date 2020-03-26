namespace Assets.Prateek.ToConvert.Resources.Loader
{
    using UnityEngine;

    /// <summary>
    ///     This class allows for GameObject.Destroy() to behave properly and decrement the instance count of the
    ///     ResourceLoader
    /// </summary>
    internal class ResourceReferenceInstance : MonoBehaviour
    {
        #region Fields
        private IInstanceCount resourceReference;
        #endregion

        #region Unity Methods
        private void OnDestroy()
        {
            resourceReference.DecrementInstanceCount();
        }
        #endregion

        #region Class Methods
        public void Init(IInstanceCount resourceReference)
        {
            this.resourceReference = resourceReference;

            //Immediatly increment the instance count
            resourceReference.IncrementInstanceCount();
        }
        #endregion
    }
}
