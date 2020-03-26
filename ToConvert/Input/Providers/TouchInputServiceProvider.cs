namespace Assets.Prateek.ToConvert.Input.Providers
{
    using System.Collections.Generic;
    using UnityEngine;

    public class TouchInputServiceProvider : InputServiceProvider
    {
        #region Properties
        public override bool IsProviderValid
        {
            get
            {
#if UNITY_EDITOR
                return false;
#else
                return true;
#endif
            }
        }

        public override int Priority
        {
            get { return Consts.FIRST_ITEM; }
        }
        #endregion

        #region Class Methods
        protected override void OnIdentificationRequested()
        {
            SendIdentificationFor<InputService, InputServiceProvider>(this);
        }

        public override void GatherInput(List<Touch> touches)
        {
            if (Input.touchCount > 0)
            {
                touches.AddRange(Input.touches);
            }
        }
        #endregion
    }
}
