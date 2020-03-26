namespace Assets.Prateek.ToConvert.BaseBehaviour
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
            AutoDisableService.Remove(this);
        }
        #endregion

        #region Class Methods
        protected void WakeUp()
        {
            AutoDisableService.WakeUp(this);
        }
        #endregion
    }
}
