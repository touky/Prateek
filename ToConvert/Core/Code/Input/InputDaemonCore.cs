namespace Mayfair.Core.Code.Input
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Mayfair.Core.Code.DebugMenu;
    using Mayfair.Core.Code.DebugMenu.Content;
    using Mayfair.Core.Code.DebugMenu.Pages;
    using Mayfair.Core.Code.Input.Enums;
    using Mayfair.Core.Code.Input.InputLayers;
    using Mayfair.Core.Code.Input.Providers;
    using Mayfair.Core.Code.Input.Reports;
    using Mayfair.Core.Code.Service;
    using Mayfair.Core.Code.Temporary;
    using Mayfair.Core.Code.Utils;
    using Mayfair.Core.Code.Utils.Debug.Reflection;
    using Mayfair.Core.Code.Utils.Extensions;
    using Mayfair.Core.Code.Utils.Types.Priority;
    using Prateek.DaemonCore.Code;
    using UnityEngine;

    public class InputDaemonCore : DaemonCore<InputDaemonCore, InputDaemonBranch>, IDebugMenuNotebookOwner
    {
        #region Static and Constants
        public const float HOLD_THRESHOLD = 10f;
        public const float HOLD_SHORT_TAP = 0.15f;
        public const float HOLD_LONG_TAP = 0.8f;

        //This value controls the distance margin of error between the two fingers before a zoom is detected
        //Tweaking this will change how granular / sensitive your inputs will be on some motions. Alexandres
        //believes this current value is the optimal sweetspot
        public const float PINCH_MIN_THRESHOLD = 0.002f; //Do not change this value
        private const int MAX_RAYCAST_ALL = 32;
        private static Touch cursor0;
        private static Touch cursor1;
        #endregion

        #region Fields
        private Enums.TouchType previousTouchType;
        private Enums.TouchType currentTouchType;

        private TouchState previousTouchState = TouchState.Inactive;
        private TouchState currentTouchState = TouchState.Inactive;

        private InputReport inputReport = new InputReport();

        private List<Touch> touches = new List<Touch>();
        private Touch[] cursors = new Touch[2] {new Touch(), new Touch()};

        private RaycastHit[] raycastHits = new RaycastHit[MAX_RAYCAST_ALL];

        private InputRaycast[] inputRaycasts;
        private InputRaycastHits inputRaycastHits;
        private int availableLayers = 0;
        private List<string> layersNames = new List<string>();

        private Dictionary<Type, List<InputReceiver>> receiverLayers = new Dictionary<Type, List<InputReceiver>>();

        private InputLayer activeLayer = null;
        private List<InputLayer> layers = null;
        #endregion

        #region Properties
        private Enums.TouchType CurrentTouchType
        {
            get { return currentTouchType; }
            set
            {
                previousTouchType = currentTouchType;
                currentTouchType = value;
            }
        }

        public static InputRaycastHits InputRaycastHits
        {
            get
            {
                return Instance.inputRaycastHits;
            }
        }
        #endregion

        #region Unity Methods
        private void Update()
        {
            ProcessInput();
        }
        #endregion

        #region Service
        protected override void OnAwake()
        {
            layers = new List<InputLayer> { new UIInputLayer() };
            layers.SortWithPriorities();

            SetupRaycasts();

            SetupDebugContent();
        }

        public static void RegisterLayer<T>(Func<InputLayer> createFunc)
            where T : InputLayer
        {
            Instance.RegisterLayer(typeof(InputLayer), createFunc);
        }

        private void RegisterLayer(Type layerType, Func<InputLayer> createFunc)
        {
            if (layers.FindIndex((x) => { return x.GetType() == layerType; }) == Consts.INDEX_NONE)
            {
                layers.Add(createFunc());
                layers.SortWithPriorities();
            }
        }

        protected void Register(InputReceiver receiver)
        {
            Type layerType = receiver.LayerType;
            RegisterLayer(layerType, receiver.GetNewLayerInstance);

            List<InputReceiver> receivers = null;
            if (!receiverLayers.TryGetValue(layerType, out receivers))
            {
                receivers = new List<InputReceiver>();
                receiverLayers.Add(layerType, receivers);
            }

            if (receivers.Contains(receiver))
            {
                throw new Exception("Receiver already registerd");
            }

            receivers.Add(receiver);
        }
        #endregion

        #region Class Methods
        public void ProcessInput()
        {
            bool inputGathered = GatherInput();

            inputRaycastHits.Reset();
            RefreshRaycast(InputRaycast.ScreenCenter);
            RefreshRaycast(InputRaycast.MultiTouchPrimaryCursor);
            RefreshRaycast(InputRaycast.MultiTouchSecondaryCursor);

            if (!inputGathered)
            {
                return;
            }

            if (CurrentTouchType == Enums.TouchType.NoTouch)
            {
                return;
            }

            cursors[0] = touches[0];
            cursors[1] = CurrentTouchType == Enums.TouchType.MultiTouch ? touches[1] : new Touch();

            switch (currentTouchState)
            {
                case TouchState.Inactive:
                {
                    currentTouchState = TouchState.Active;
                    BuildInputReport(TouchStatus.Begin);

                    RefreshRaycast(InputRaycast.MainCursor);

                    SelectInputLayer(currentTouchType);
                    break;
                }
                case TouchState.Active:
                {
                    if (IsTouchEnding(cursors))
                    {
                        currentTouchState = TouchState.Inactive;

                        BuildInputReport(TouchStatus.End);

                        RefreshRaycast(InputRaycast.MainCursor);
                    }
                    else
                    {
                        BuildInputReport(TouchStatus.Active);

                        RefreshRaycast(InputRaycast.MainCursor);

                        RefreshInputLayerValidity();
                    }

                    break;
                }
            }

            UpdateReceivers(activeLayer);

            if (inputReport.Status == TouchStatus.End)
            {
                inputReport.Reset(cursors[0].position);
                activeLayer = null;
            }
        }

        private bool GatherInput()
        {
            InputDaemonBranch branch = GetFirstAliveBranch();
            if (branch == null)
            {
                return false;
            }

            //unity will automatically update touches, we just need to update mouse input
            touches.Clear();
            branch.GatherInput(touches);

            if (touches.Count == 0)
            {
                CurrentTouchType = Enums.TouchType.NoTouch;
                return false;
            }

            CurrentTouchType = touches.Count == 1 ? Enums.TouchType.SingleTouch : Enums.TouchType.MultiTouch;

            return true;
        }

        private static bool IsTouchEnding(Touch[] cursors)
        {
            foreach (Touch cursor in cursors)
            {
                if (cursor.phase == TouchPhase.Ended || cursor.phase == TouchPhase.Canceled)
                {
                    return true;
                }
            }

            return false;
        }

        private void BuildInputReport(TouchStatus touchStatus)
        {
            inputReport.Status = touchStatus;
            inputReport.CurrentTouchType = currentTouchType;
            inputReport.InputRaycastHits = inputRaycastHits;

            inputReport.Update(cursors[0].position, cursors[1].position);
        }

        protected void Unregister(InputReceiver receiver)
        {
            List<InputReceiver> receivers = null;
            Type layerType = receiver.LayerType;
            if (!receiverLayers.TryGetValue(layerType, out receivers))
            {
                receivers = new List<InputReceiver>();
                receiverLayers.Add(layerType, receivers);
            }

            receivers.Remove(receiver);
        }

        private void UpdateReceivers(InputLayer layer)
        {
            List<InputReceiver> receivers = null;
            if (layer != null && receiverLayers.TryGetValue(layer.GetType(), out receivers))
            {
                foreach (InputReceiver receiver in receivers)
                {
                    receiver.ProcessInput(inputReport);
                }
            }
        }

        public void SetupRaycasts()
        {
            inputRaycasts = (InputRaycast[]) System.Enum.GetValues(typeof(InputRaycast));

            layersNames.Clear();
            //First add all of them
            for (int m = 0; m < 32; m++)
            {
                string layerName = LayerMask.LayerToName(m);
                layersNames.Add(layerName);
            }

            //Remove the last ones that are empty
            while (layersNames.Count > 0 && layersNames.Last() == string.Empty)
            {
                layersNames.RemoveAt(layersNames.Count - 1);
                if (layersNames.Last() != string.Empty)
                {
                    break;
                }
            }

            //Only keep the mask that are named
            for (int m = 0; m < layersNames.Count; m++)
            {
                if (layersNames[m] == string.Empty)
                {
                    continue;
                }

                availableLayers |= 1 << m;
            }

            inputRaycastHits = new InputRaycastHits(inputRaycasts.Length, layersNames.Count);
        }

        public bool TryRaycastPosition(InputRaycast inputRaycast, out Vector2 screenPoint)
        {
            screenPoint = Vector2.zero;
            switch (inputRaycast)
            {
                case InputRaycast.ScreenCenter:
                {
                    screenPoint = new Vector2(Screen.width, Screen.height) * 0.5f;
                    break;
                }
                case InputRaycast.MainCursor:
                {
                    if (CurrentTouchType != Enums.TouchType.SingleTouch)
                    {
                        return false;
                    }

                    screenPoint = inputReport.Cursor.Current;
                    break;
                }
                case InputRaycast.MultiTouchPrimaryCursor:
                {
                    if (CurrentTouchType != Enums.TouchType.MultiTouch)
                    {
                        return false;
                    }

                    screenPoint = touches[0].position;
                    break;
                }
                case InputRaycast.MultiTouchSecondaryCursor:
                {
                    if (CurrentTouchType != Enums.TouchType.MultiTouch)
                    {
                        return false;
                    }

                    screenPoint = touches[1].position;
                    break;
                }
            }

            return true;
        }

        public void RefreshRaycast()
        {
            for (int r = 0; r < inputRaycasts.Length; r++)
            {
                InputRaycast inputRaycast = inputRaycasts[r];
                RefreshRaycast(inputRaycast);
            }
        }

        public void RefreshRaycast(InputRaycast inputRaycast)
        {
            Camera camera = CameraUtilities.GetCamera();
            if (camera == null)
            {
                return;
            }

            Vector2 screenPoint = Vector2.zero;
            if (!TryRaycastPosition(inputRaycast, out screenPoint))
            {
                return;
            }

            RaycastInfo raycastInput = inputRaycastHits[inputRaycast];

            //TODO: Camera
            raycastInput.Ray = camera.ScreenPointToRay(screenPoint);
            int hitCount = Physics.RaycastNonAlloc(raycastInput.Ray, raycastHits, Mathf.Infinity, availableLayers);
            if (hitCount > 0)
            {
                for (int h = 0; h < hitCount; h++)
                {
                    RaycastHit newHit = raycastHits[h];
                    int layer = newHit.collider.gameObject.layer;

                    RaycastHit oldHit = raycastInput[layer];
                    if (oldHit.collider == null || newHit.distance < oldHit.distance)
                    {
                        oldHit = newHit;
                    }
                    raycastInput[layer] = oldHit;
                }
            }

            inputRaycastHits[inputRaycast] = raycastInput;
        }

        private void RefreshInputLayerValidity()
        {
            if (activeLayer == null || !activeLayer.CanUnlockIfActive)
            {
                return;
            }

            if (CheckIfLayerHasActiveReceiver(activeLayer))
            {
                return;
            }

            BuildInputReport(TouchStatus.End);

            UpdateReceivers(activeLayer);

            SelectedActiveLayerBelow();

            BuildInputReport(TouchStatus.Begin);
        }

        private void SelectInputLayer(Enums.TouchType touchType)
        {
            foreach (InputLayer layer in layers)
            {
                if (!layer.IsActive)
                {
                    continue;
                }

                layer.RefreshLayerLocking(touchType, cursors, inputRaycastHits);
            }

            activeLayer = null;
            for (int l = layers.Count - 1; l >= 0; l--)
            {
                InputLayer layer = layers[l];
                if (layer.IsLocked && (!layer.NeedActiveReceiverToLock || CheckIfLayerHasActiveReceiver(layer)))
                {
                    activeLayer = layer;
                    break;
                }
            }
        }

        private bool CheckIfLayerHasActiveReceiver(InputLayer layer)
        {
            List<InputReceiver> receivers = null;
            if (receiverLayers.TryGetValue(layer.GetType(), out receivers))
            {
                foreach (InputReceiver receiver in receivers)
                {
                    if (receiver.IsActive)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private void SelectedActiveLayerBelow()
        {
            int layerIndex = layers.FindIndex((x) => x == activeLayer) - 1;
            if (layerIndex <= Consts.INDEX_NONE)
            {
                return;
            }

            activeLayer = layers[layerIndex];
        }
        #endregion

        #region Nested type: InputReceiver
        public abstract class InputReceiver
        {
            #region Fields
            private bool enabled = false;
            protected bool active = true;
            #endregion

            #region Properties
            public bool Enabled
            {
                get { return enabled; }
                set
                {
                    if (enabled == value)
                    {
                        return;
                    }

                    if (enabled)
                    {
                        OnDisable();
                    }
                    else
                    {
                        OnEnable();
                    }

                    enabled = value;
                }
            }

            public abstract Type LayerType { get; }

            public virtual bool IsActive
            {
                get { return active; }
            }
            #endregion

            #region Unity Methods
            protected virtual void OnEnable()
            {
                Instance.Register(this);
            }

            protected virtual void OnDisable()
            {
                Instance.Unregister(this);
            }
            #endregion

            #region Class Methods
            public abstract InputLayer GetNewLayerInstance();
            public virtual void ProcessInput(InputReport inputReport) { }
            #endregion
        }
        #endregion

        #region Debug
        [Conditional("NVIZZIO_DEV")]
        private void SetupDebugContent()
        {
            DebugMenuNotebook debugNotebook = new InputDebugMenuNotebook(this, "INPT", "Input Service");

            EmptyMenuPage main = new EmptyMenuPage("MAIN");
            debugNotebook.AddPagesWithParent(main, new InputServiceMenuPage(this, "Input debug"));
            debugNotebook.Register();
        }
        #endregion Debug
    }
}
