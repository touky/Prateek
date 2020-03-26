namespace Assets.Prateek.ToConvert.DebugMenu.Fields
{
    using UnityEngine;

    /// <summary>
    ///     A float slider field, draws a slider with the given limits
    /// </summary>
    public class FloatSliderField : DebugField
    {
        #region Static and Constants
        private const int Rects_Value = 0;
        private const int Rects_Min = 1;
        private const int Rects_Slider = 2;
        private const int Rects_Max = 3;
        private const int Rects_COUNT = 4;
        #endregion

        #region Fields
        private string title;
        protected string format;
        protected Vector2 minMax;
        protected GUIContent[] minMaxContent;
        protected float[] baseWidths = new float[Rects_COUNT];
        protected float[] widths = new float[Rects_COUNT];
        #endregion

        #region Properties
        public float Value { get; set; }
        #endregion

        #region Constructors
        public FloatSliderField(string title, Vector2 minMax, float initialValue = 0f)
        {
            this.title = title;
            this.minMax = minMax;
            Value = Mathf.Clamp(initialValue, this.minMax.x, this.minMax.y);
            format = "F2";

            RefreshMinMaxContent();

            //Default empiric values
            baseWidths[Rects_Value] = -100;
            baseWidths[Rects_Min] = -60;
            baseWidths[Rects_Slider] = 200;
            baseWidths[Rects_Max] = -60;
        }
        #endregion

        #region Unity EditorOnly Methods
        public override void OnGUI(DebugMenuContext context)
        {
            GUIContent content = null;

            UpdateContentRectWidths(ref content, context);

            DrawSliderLabels(content, context);
        }
        #endregion

        #region Class Methods
        protected void RefreshMinMaxContent()
        {
            minMaxContent = new[]
            {
                new GUIContent($" [{minMax.x.ToString(format)} "),
                new GUIContent($" {minMax.y.ToString(format)}] ")
            };
        }

        private void UpdateContentRectWidths(ref GUIContent content, DebugMenuContext context)
        {
            content = new GUIContent($"{title}: {Value.ToString(format)}");
            for (var w = 0; w < baseWidths.Length; w++)
            {
                if (w == 0)
                {
                    baseWidths[w] = Mathf.Min(baseWidths[w], -context.LabelLeft.CalcSize(content).x);
                }

                widths[w] = baseWidths[w];

                if (w % 2 == 1)
                {
                    widths[w] = -context.LabelLeft.CalcSize(minMaxContent[w / 2]).x;
                }
            }
        }

        private void DrawSliderLabels(GUIContent content, DebugMenuContext context)
        {
            var sliderRect = GUILayoutUtility.GetRect(content, context.LabelLeft);

            float posX = 0;
            RectHelper.ConvertWeightsIntoRealSizes(sliderRect.width, ref widths);
            for (var w = 0; w < widths.Length; w++)
            {
                var rect = sliderRect;
                rect.x += posX;
                rect.width = widths[w];

                switch (w)
                {
                    case Rects_Value:
                    {
                        GUI.Label(rect, content, context.LabelLeft);
                        break;
                    }
                    case Rects_Min:
                    {
                        GUI.Label(rect, minMaxContent[0], context.LabelLeft);
                        break;
                    }
                    case Rects_Slider:
                    {
                        Value = GUI.HorizontalSlider(rect, Value, minMax.x, minMax.y, context.Sliders[DebugMenuStyle.SLIDER_BG], context.Sliders[DebugMenuStyle.SLIDER_THUMB]);
                        break;
                    }
                    case Rects_Max:
                    {
                        GUI.Label(rect, minMaxContent[1], context.LabelRight);
                        break;
                    }
                }

                posX += rect.width;
            }
        }

        public void Draw(DebugMenuContext context, float value)
        {
            Value = value;

            Draw(context);
        }
        #endregion
    }
}
