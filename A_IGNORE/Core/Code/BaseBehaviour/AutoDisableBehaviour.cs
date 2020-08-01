namespace Mayfair.Core.Code.BaseBehaviour
{
    using UnityEngine;

    public class AutoDisableBehaviour : MonoBehaviour
    {
        #region Unity Methods
        protected virtual void Awake()
        {
            if (enabled)
            {
                WakeUp();
            }
        }

        protected virtual void OnDestroy()
        {
            AutoDisableDaemon.Remove(this);
        }
        #endregion

        #region Class Methods
        protected void WakeUp()
        {
            AutoDisableDaemon.WakeUp(this);
        }
        #endregion
    }
}
