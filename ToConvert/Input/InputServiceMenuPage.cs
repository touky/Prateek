namespace Assets.Prateek.ToConvert.Input
{
    using System;
    using System.Collections.Generic;
    using Assets.Prateek.ToConvert.DebugMenu;
    using Assets.Prateek.ToConvert.DebugMenu.Fields;
    using Assets.Prateek.ToConvert.DebugMenu.Pages;
    using Assets.Prateek.ToConvert.Input.Enums;
    using Assets.Prateek.ToConvert.Input.InputLayers;
    using Assets.Prateek.ToConvert.Input.Reports;
    using Assets.Prateek.ToConvert.Reflection;
    using UnityEngine;

    internal class InputServiceMenuPage : DebugMenuPage<InputService>
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
        private ReflectedField<Dictionary<Type, List<InputService.InputReceiver>>> receiverLayers = "receiverLayers";
        private ReflectedField<InputRaycast[]> inputRaycasts = "inputRaycasts";
        private ReflectedField<List<string>> layersNames = "layersNames";
        private ReflectedField<InputRaycastHits> inputRaycastHits = "inputRaycastHits";

        private ReflectedField<Enums.TouchType> previousTouchType = "previousTouchType";
        private ReflectedField<Enums.TouchType> currentTouchType = "currentTouchType";
        private ReflectedField<TouchState> previousTouchState = "previousTouchState";
        private ReflectedField<TouchState> currentTouchState = "currentTouchState";
        #endregion

        #region Constructors
        public InputServiceMenuPage(InputService owner, string title)
            : base(owner, title) { }
        #endregion

        #region Class Methods
        protected override void Draw(InputService owner, DebugMenuContext context)
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
                NewLabel.Draw(context, $"Progress     : {(int) (inputReport.Value.hold.Progression * 100)}");
                NewLabel.Draw(context, $"Has triggered: {inputReport.Value.hold.HasLongTapped}");
            }

            inputLayers.Draw(context);
            if (inputLayers.ShowContent)
            {
                using (new ContextIndentScope(context, 1))
                {
                    var activeLayerPassed = false;
                    foreach (var layer in layers.Value)
                    {
                        if (layer == null)
                        {
                            continue;
                        }

                        var color = layer == activeLayer.Value ? Color.red : Color.white;
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
                    foreach (var pair in receiverLayers.Value)
                    {
                        GetField<CategoryField>().Draw(context, pair.Key.Name);
                        if (title.ShowContent)
                        {
                            using (new ContextIndentScope(context, 1))
                            {
                                foreach (var receiver in pair.Value)
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
                    foreach (var inputRaycast in inputRaycasts.Value)
                    {
                        var raycastHits = inputRaycastHits.Value[inputRaycast].Hits;

                        var category = GetField<CategoryField>();
                        category.Draw(context, $"{inputRaycast}");
                        if (category.ShowContent)
                        {
                            using (new ContextIndentScope(context, 1))
                            {
                                var atLeastOneHit = false;
                                for (var r = 0; r < raycastHits.Length; r++)
                                {
                                    var hit = raycastHits[r];
                                    if (hit.collider == null)
                                    {
                                        continue;
                                    }

                                    atLeastOneHit = true;

                                    var hitLabel = GetField<LabelField>();
                                    var layer    = hit.collider.gameObject.layer;
                                    hitLabel.Draw(context, string.Format("{0:D2}: {1} / {2}", layer, layersNames.Value[layer], hit.collider.gameObject.name));
                                }

                                if (!atLeastOneHit)
                                {
                                    var hitLabel = GetField<LabelField>();
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
