namespace Mayfair.Core.Code.Input
{
    using System;
    using System.Collections.Generic;
    using Mayfair.Core.Code.DebugMenu;
    using Mayfair.Core.Code.DebugMenu.Fields;
    using Mayfair.Core.Code.DebugMenu.Pages;
    using Mayfair.Core.Code.Input.Enums;
    using Mayfair.Core.Code.Input.InputLayers;
    using Mayfair.Core.Code.Input.Reports;
    using Mayfair.Core.Code.Utils.Debug.Reflection;
    using UnityEngine;

    internal class InputServiceMenuPage : DebugMenuPage<InputDaemon>
    {
        #region Fields
        private CategoryField defaultDebug = new CategoryField("Default debug");
        private CategoryField inputLayers = new CategoryField("Input layers");
        private CategoryField inputReports = new CategoryField("Input Report");
        private CategoryField inputReceivers = new CategoryField("Input receivers");
        private CategoryField DefaultRaycasts = new CategoryField("Default raycasts");

        private ReflectedField<List<InputLayer>> layers = "layers";
        private ReflectedField<InputLayer> activeLayer = "activeLayer";
        private ReflectedField<InputReport> inputReport = "inputReport";
        private ReflectedField<Dictionary<Type, List<InputDaemon.InputReceiver>>> receiverLayers = "receiverLayers";
        private ReflectedField<InputRaycast[]> inputRaycasts = "inputRaycasts";
        private ReflectedField<List<string>> layersNames = "layersNames";
        private ReflectedField<InputRaycastHits> inputRaycastHits = "inputRaycastHits";

        private ReflectedField<Enums.TouchType> previousTouchType = "previousTouchType";
        private ReflectedField<Enums.TouchType> currentTouchType = "currentTouchType";
        private ReflectedField<TouchState> previousTouchState = "previousTouchState";
        private ReflectedField<TouchState> currentTouchState = "currentTouchState";
        #endregion

        #region Constructors
        public InputServiceMenuPage(InputDaemon owner, string title)
            : base(owner, title) { }
        #endregion

        #region Class Methods
        protected override void Draw(InputDaemon owner, DebugMenuContext context)
        {
            defaultDebug.Draw(context);
            if (defaultDebug.ShowContent)
            {
                GetField<LabelField>().Draw(context, $"Touch Type: {currentTouchType.Value}");
                GetField<LabelField>().Draw(context, $"      Was : {previousTouchType.Value}");
                GetField<LabelField>().Draw(context, $"Touch State: {currentTouchState.Value}");
                GetField<LabelField>().Draw(context, $"       Was : {previousTouchState.Value}");
            }

            if (inputReports.Draw(context))
            {
                NewLabel.Draw(context, inputReport.Value.Status.ToString());
                NewLabel.Draw(context, $"Is holding   : {inputReport.Value.hold.IsHolding}");
                NewLabel.Draw(context, $"Progress     : {(int)(inputReport.Value.hold.Progression * 100)}");
                NewLabel.Draw(context, $"Has triggered: {inputReport.Value.hold.HasLongTapped}");
            }

            inputLayers.Draw(context);
            if (inputLayers.ShowContent)
            {
                using (new ContextIndentScope(context, 1))
                {
                    bool activeLayerPassed = false;
                    foreach (InputLayer layer in layers.Value)
                    {
                        if (layer == null)
                        {
                            continue;
                        }

                        Color color = layer == activeLayer.Value ? Color.red : Color.white;
                        color = activeLayerPassed ? Color.grey : color;
                        activeLayerPassed = activeLayerPassed || layer == activeLayer.Value;

                        using (new ContextColorScope(context, color))
                        {
                            GetField<LabelField>().Draw(context, layer.GetType().Name);
                        }
                    }
                }
            }

            inputReceivers.Draw(context);
            if (inputReceivers.ShowContent)
            {
                using (new ContextIndentScope(context, 1))
                {
                    foreach (KeyValuePair<Type, List<InputDaemon.InputReceiver>> pair in receiverLayers.Value)
                    {
                        GetField<CategoryField>().Draw(context, pair.Key.Name);
                        if (title.ShowContent)
                        {
                            using (new ContextIndentScope(context, 1))
                            {
                                foreach (InputDaemon.InputReceiver receiver in pair.Value)
                                {
                                    using (new ContextColorScope(context, receiver.IsActive ? Color.white : Color.red))
                                    {
                                        GetField<LabelField>().Draw(context, receiver.GetType().Name);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            DefaultRaycasts.Draw(context);
            if (DefaultRaycasts.ShowContent)
            {
                using (new ContextIndentScope(context, 1))
                {
                    foreach (InputRaycast inputRaycast in inputRaycasts.Value)
                    {
                        RaycastHit[] raycastHits = inputRaycastHits.Value[inputRaycast].Hits;

                        CategoryField category = GetField<CategoryField>();
                        category.Draw(context, $"{inputRaycast}");
                        if (category.ShowContent)
                        {
                            using (new ContextIndentScope(context, 1))
                            {
                                bool atLeastOneHit = false;
                                for (int r = 0; r < raycastHits.Length; r++)
                                {
                                    RaycastHit hit = raycastHits[r];
                                    if (hit.collider == null)
                                    {
                                        continue;
                                    }

                                    atLeastOneHit = true;

                                    LabelField hitLabel = GetField<LabelField>();
                                    int layer = hit.collider.gameObject.layer;
                                    hitLabel.Draw(context, string.Format("{0:D2}: {1} / {2}", layer, layersNames.Value[layer], hit.collider.gameObject.name));
                                }

                                if (!atLeastOneHit)
                                {
                                    LabelField hitLabel = GetField<LabelField>();
                                    hitLabel.Draw(context, "No hit found");
                                }
                            }
                        }
                    }
                }
            }
        }
        #endregion
    }
}
