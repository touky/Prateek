namespace Prateek.A_TODO.Runtime.AppContentFramework.Loader
{
    using Prateek.A_TODO.Runtime.AppContentFramework.Loader.Interfaces;
    using UnityEngine;

    /// <summary>
    ///     This class allows for GameObject.Destroy() to behave properly and decrement the instance count of the
    ///     ResourceLoader
    /// </summary>
    public class ContentInstanceRef : MonoBehaviour
    {
        #region Fields
        private IInstanceRef instanceRef;
        #endregion

        public void Init(IInstanceRef resourceReference)
        {
            this.instanceRef = resourceReference;

            //Immediatly increment the instance count
            resourceReference.IncrementInstanceRef();
        }

        #region Unity Methods
        private void OnDestroy()
        {
            instanceRef.DecrementInstanceRef();
        }
        #endregion
    }
}
