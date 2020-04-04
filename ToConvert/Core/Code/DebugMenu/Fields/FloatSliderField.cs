namespace Mayfair.Core.Code.DebugMenu.Fields
{
    using UnityEngine;

    /// <summary>
    ///     A float slider field, draws a slider with the given limits
    /// </summary>
    public class FloatSliderField : DebugField
    {
        private const int Rects_Value = 0;
        private const int Rects_Min = 1;
        private const int Rects_Slider = 2;
        private const int Rects_Max = 3;
        private const int Rects_COUNT = 4;

        #region Fields
        private string title;
        protected string format;
        protected Vector2 minMax;
        protected GUIContent[] minMaxContent;
        protected float[] baseWidths = new float[Rects_COUNT];
        protected float[] widths = new float[Rects_COUNT];
        #endregion

        public float Value { get; set; }

        #region Constructors
        public FloatSliderField(string title, Vector2 minMax, float initialValue = 0f)
        {
            this.title = title;
            this.minMax = minMax;
            this.Value = Mathf.Clamp(initialValue, this.minMax.x, this.minMax.y);
            this.format = "F2";

            RefreshMinMaxContent();

            //Default empiric values
            this.baseWidths[Rects_Value] = -100;
            this.baseWidths[Rects_Min] = -60;
            this.baseWidths[Rects_Slider] = 200;
            this.baseWidths[Rects_Max] = -60;
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
            this.minMaxContent = new[]
            {
                new GUIContent($" [{this.minMax.x.ToString(this.format)} "),
                new GUIContent($" {this.minMax.y.ToString(this.format)}] ")
            };
        }

        private void UpdateContentRectWidths(ref GUIContent content, DebugMenuContext context)
        {
            content = new GUIContent($"{this.title}: {this.Value.ToString(this.format)}");
            for (int w = 0; w < this.baseWidths.Length; w++)
            {
                if (w == 0)
                {
                    this.baseWidths[w] = Mathf.Min(this.baseWidths[w], -context.LabelLeft.CalcSize(content).x);
                }

                this.widths[w] = this.baseWidths[w];

                if (w % 2 == 1)
                {
                    this.widths[w] = -context.LabelLeft.CalcSize(this.minMaxContent[w / 2]).x;
                }
            }
        }

        private void DrawSliderLabels(GUIContent content, DebugMenuContext context)
        {
            //todo Rect sliderRect = GUILayoutUtility.GetRect(content, context.LabelLeft);
            //todo 
            //todo float posX = 0;
            //todo RectHelper.ConvertWeightsIntoRealSizes(sliderRect.width, ref this.widths);
            //todo for (int w = 0; w < this.widths.Length; w++)
            //todo {
            //todo     Rect rect = sliderRect;
            //todo     rect.x += posX;
            //todo     rect.width = this.widths[w];
            //todo 
            //todo     switch (w)
            //todo     {
            //todo         case Rects_Value:
            //todo         {
            //todo             GUI.Label(rect, content, context.LabelLeft);
            //todo             break;
            //todo         }
            //todo         case Rects_Min:
            //todo         {
            //todo             GUI.Label(rect, this.minMaxContent[0], context.LabelLeft);
            //todo             break;
            //todo         }
            //todo         case Rects_Slider:
            //todo         {
            //todo             this.Value = GUI.HorizontalSlider(rect, this.Value, this.minMax.x, this.minMax.y, context.Sliders[DebugMenuStyle.SLIDER_BG], context.Sliders[DebugMenuStyle.SLIDER_THUMB]);
            //todo             break;
            //todo         }
            //todo         case Rects_Max:
            //todo         {
            //todo             GUI.Label(rect, this.minMaxContent[1], context.LabelRight);
            //todo             break;
            //todo         }
            //todo     }
            //todo 
            //todo     posX += rect.width;
            //todo }
        }

        public void Draw(DebugMenuContext context, float value)
        {
            this.Value = value;

            Draw(context);
        }
        #endregion
    }
}
