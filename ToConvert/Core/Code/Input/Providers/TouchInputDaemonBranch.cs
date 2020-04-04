namespace Mayfair.Core.Code.Input.Providers
{
    using System.Collections.Generic;
    using Mayfair.Core.Code.Utils;
    using UnityEngine;

    public class TouchInputDaemonBranch : InputDaemonBranch
    {
        #region Properties
        public override bool IsAlive
        {
            get
            {
#if UNITY_EDITOR
                return false;
#else
                return base.IsAlive;
#endif
            }
        }

        public override int Priority
        {
            get { return Consts.FIRST_ITEM; }
        }
        #endregion

        #region Class Methods
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
