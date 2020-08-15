namespace Mayfair.Core.Code.Input.InputLayers
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class UIInputLayer : InputLayer
    {
        #region Fields
        private EventSystem eventSystem = null;
        private GraphicRaycaster canvasRaycaster = null;

        private PointerEventData pointerEventData = null;
        private List<RaycastResult> pointerCastResults = null;
        #endregion

        #region Properties
        public override int DefaultPriority
        {
            get { return 1000; }
        }

        public override bool IsActive
        {
            get { return true; }
        }

        public override bool SupportsMultiInput
        {
            get { return true; }
        }

        public override bool NeedActiveReceiverToLock
        {
            get { return false; }
        }
        
        public override bool CanUnlockIfActive
        {
            get { return false; }
        }
        #endregion

        #region Class Methods
        private void RefreshInit()
        {
            if (eventSystem != null && canvasRaycaster != null)
            {
                return;
            }

            eventSystem = Object.FindObjectOfType<EventSystem>();
            canvasRaycaster = Object.FindObjectOfType<GraphicRaycaster>();

            if (eventSystem == null || canvasRaycaster == null)
            {
                return;
            }

            pointerEventData = new PointerEventData(eventSystem);
            pointerCastResults = new List<RaycastResult>();
        }

        public override bool RefreshLayerLocking(Vector2 cursorPosition, RaycastHit[] hits)
        {
            RefreshInit();

            pointerCastResults.Clear();
            pointerEventData.position = cursorPosition;
            EventSystem.current.RaycastAll(pointerEventData, pointerCastResults);

            if (pointerCastResults.Count > 0)
            {
                return pointerCastResults[0].gameObject.transform is RectTransform;
            }

            return false;
        }

        public override bool RefreshLayerLocking(Vector2 cursorPosition0, RaycastHit[] hits0, Vector2 cursorPosition1, RaycastHit[] hits1)
        {
            if (!RefreshLayerLocking(cursorPosition0, hits0))
            {
                return RefreshLayerLocking(cursorPosition1, hits1);
            }

            return false;
        }
        #endregion
    }
}
