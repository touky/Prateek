namespace Mayfair.Core.Code.Input.InputLayers
{
    using Prateek.Runtime.Core.Interfaces.IPriority;
    using UnityEngine;
    using UnityEngine.Assertions;

    public abstract class InputLayer : IPriority
    {
        #region Fields
        private bool isLocked = false;
        #endregion

        #region Properties
        public abstract bool IsActive { get; }

        public virtual bool IsLocked
        {
            get { return isLocked; }
        }

        public abstract bool SupportsMultiInput { get; }
        public abstract bool NeedActiveReceiverToLock { get; }
        public abstract bool CanUnlockIfActive { get; }
        #endregion

        #region Class Methods
        internal void RefreshLayerLocking(Enums.TouchType touchType, Touch[] cursors, InputRaycastHits inputRaycastHits)
        {
            isLocked = false;
            if (touchType == Enums.TouchType.NoTouch)
            {
                return;
            }

            if (touchType == Enums.TouchType.MultiTouch && !SupportsMultiInput)
            {
                return;
            }

            switch (touchType)
            {
                case Enums.TouchType.SingleTouch:
                {
                    isLocked = RefreshLayerLocking(cursors[0].position, inputRaycastHits[InputRaycast.MainCursor].Hits);
                    break;
                }
                case Enums.TouchType.MultiTouch:
                {
                    isLocked = RefreshLayerLocking(cursors[0].position, inputRaycastHits[InputRaycast.MultiTouchPrimaryCursor].Hits,
                                                      cursors[1].position, inputRaycastHits[InputRaycast.MultiTouchSecondaryCursor].Hits);
                    break;
                }
            }
        }

        public abstract bool RefreshLayerLocking(Vector2 cursorPosition, RaycastHit[] hits);

        public virtual bool RefreshLayerLocking(Vector2 cursorPosition0, RaycastHit[] hits0, Vector2 cursorPosition1, RaycastHit[] hits1)
        {
            Assert.IsFalse(SupportsMultiInput, $"{GetType().Name} says it supports Multitouch input but does no implement the multitouch version of RefreshLayerLocking()");
            return false;
        }
        #endregion

        #region IPriority Members
        public abstract int DefaultPriority { get; }
        #endregion
    }
}
