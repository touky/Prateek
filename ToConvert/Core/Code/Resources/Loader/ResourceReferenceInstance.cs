namespace Mayfair.Core.Code.Resources.Loader
{
    using System;
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

        public void Init(IInstanceCount resourceReference)
        {
            this.resourceReference = resourceReference;

            //Immediatly increment the instance count
            resourceReference.IncrementInstanceCount();
        }

        #region Unity Methods
        private void OnDestroy()
        {
            resourceReference.DecrementInstanceCount();
        }
        #endregion
    }
}
