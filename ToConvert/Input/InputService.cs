namespace Assets.Prateek.ToConvert.Input
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Assets.Prateek.ToConvert.DebugMenu.Content;
    using Assets.Prateek.ToConvert.DebugMenu.Pages;
    using Assets.Prateek.ToConvert.Input.Enums;
    using Assets.Prateek.ToConvert.Input.InputLayers;
    using Assets.Prateek.ToConvert.Input.Providers;
    using Assets.Prateek.ToConvert.Input.Reports;
    using Assets.Prateek.ToConvert.Priority;
    using Assets.Prateek.ToConvert.Service;
    using UnityEngine;

    public class InputService : ServiceSingletonBehaviour<InputService, InputServiceProvider>, IDebugMenuNotebookOwner
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
            get { return Instance.inputRaycastHits; }
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
            layers = new List<InputLayer> {new UIInputLayer()};
            layers.SortWithPriorities();

            SetupRaycasts();

            SetupDebugContent();
        }

        protected void Register(InputReceiver receiver)
        {
            var layerType = receiver.LayerType;
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
        public static void RegisterLayer<T>(Func<InputLayer> createFunc)
            where T : InputLayer
        {
            Instance.RegisterLayer(typeof(InputLayer), createFunc);
        }

        private void RegisterLayer(Type layerType, Func<InputLayer> createFunc)
        {
            if (layers.FindIndex(x => { return x.GetType() == layerType; }) == Consts.INDEX_NONE)
            {
                layers.Add(createFunc());
                layers.SortWithPriorities();
            }
        }

        public void ProcessInput()
        {
            var inputGathered = GatherInput();

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
            var provider = GetFirstValidProvider();
            if (provider == null)
            {
                return false;
            }

            //unity will automatically update touches, we just need to update mouse input
            touches.Clear();
            provider.GatherInput(touches);

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
            foreach (var cursor in cursors)
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
            var                 layerType = receiver.LayerType;
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
                foreach (var receiver in receivers)
                {
                    receiver.ProcessInput(inputReport);
                }
            }
        }

        public void SetupRaycasts()
        {
            inputRaycasts = (InputRaycast[]) Enum.GetValues(typeof(InputRaycast));

            layersNames.Clear();

            //First add all of them
            for (var m = 0; m < 32; m++)
            {
                var layerName = LayerMask.LayerToName(m);
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
            for (var m = 0; m < layersNames.Count; m++)
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
            for (var r = 0; r < inputRaycasts.Length; r++)
            {
                var inputRaycast = inputRaycasts[r];
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

            var screenPoint = Vector2.zero;
            if (!TryRaycastPosition(inputRaycast, out screenPoint))
            {
                return;
            }

            var raycastInput = inputRaycastHits[inputRaycast];

            //TODO: Camera
            raycastInput.Ray = camera.ScreenPointToRay(screenPoint);
            var hitCount = Physics.RaycastNonAlloc(raycastInput.Ray, raycastHits, Mathf.Infinity, availableLayers);
            if (hitCount > 0)
            {
                for (var h = 0; h < hitCount; h++)
                {
                    var newHit = raycastHits[h];
                    var layer  = newHit.collider.gameObject.layer;

                    var oldHit = raycastInput[layer];
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
            foreach (var layer in layers)
            {
                if (!layer.IsActive)
                {
                    continue;
                }

                layer.RefreshLayerLocking(touchType, cursors, inputRaycastHits);
            }

            activeLayer = null;
            for (var l = layers.Count - 1; l >= 0; l--)
            {
                var layer = layers[l];
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
                foreach (var receiver in receivers)
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
            var layerIndex = layers.FindIndex(x => x == activeLayer) - 1;
            if (layerIndex <= Consts.INDEX_NONE)
            {
                return;
            }

            activeLayer = layers[layerIndex];
        }

        #region Debug
        [Conditional("NVIZZIO_DEV")]
        private void SetupDebugContent()
        {
            DebugMenuNotebook debugNotebook = new InputDebugMenuNotebook(this, "INPT", "Input Service");

            var main = new EmptyMenuPage("MAIN");
            debugNotebook.AddPagesWithParent(main, new InputServiceMenuPage(this, "Input debug"));
            debugNotebook.Register();
        }
        #endregion Debug
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
    }
}
