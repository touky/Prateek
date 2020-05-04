namespace Mayfair.Core.Code.DebugMenu.Fields
{
    using UnityEngine;

    public class ProgressBarField : StringField
    {
        #region Fields
        private float progressMargin = 0.2f;
        protected Color unloadedColor = Color.red;
        protected Color loadingColor = Color.yellow;
        protected Color loadedColor = Color.green;//todo ColorHelper.green.pastel;

        private float progress;
        #endregion

        #region Properties
        protected float ProgressMargin
        {
            get { return this.progressMargin; }
            set { this.progressMargin = Mathf.Clamp01(value); }
        }
        #endregion

        #region Constructors
        public ProgressBarField() { }

        public ProgressBarField(string text) : base(text) { }
        #endregion

        #region Unity EditorOnly Methods
        public override void OnGUI(DebugMenuContext context)
        {
            GUILayout.Box(GUIContent.none, context.ProgressBar[0]);

            Rect rect = GUILayoutUtility.GetLastRect();
            Rect progressRect = rect;
            progressRect.width *= this.progress;
            //todo using (ColorScope scope = new ColorScope(GetProgressColor(this.progress)))
            //todo {
            //todo     GUI.Box(progressRect, GUIContent.none, context.ProgressBar[1]);
            //todo }
            //todo 
            //todo using (new ColorScope(Color.black))
            //todo {
            //todo     GUI.Label(rect, this.text, context.LabelCenter);
            //todo }
        }
        #endregion

        #region Class Methods
        public void Draw(DebugMenuContext context, string text, float progress)
        {
            this.text = text;

            Draw(context, progress);
        }

        public void Draw(DebugMenuContext context, float progress)
        {
            this.progress = Mathf.Clamp01(progress);

            Draw(context);
        }

        protected void ResetColors()
        {
            this.unloadedColor = Color.red;
            this.loadingColor = Color.yellow;
            this.loadedColor = Color.green;//todo ColorHelper.green.pastel;
        }

        private Color GetProgressColor(float progress)
        {
            Color result = Color.Lerp(this.unloadedColor, this.loadedColor, Mathf.Clamp01(progress / this.progressMargin));
            result = Color.Lerp(result, this.loadedColor, Mathf.Clamp01((progress - (1f - this.progressMargin)) / this.progressMargin));
            return result;
        }
        #endregion
    }
}
