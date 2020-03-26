namespace Assets.Prateek.ToConvert.DebugMenu.Fields
{
    using UnityEngine;

    public class ProgressBarField : StringField
    {
        #region Fields
        private float progressMargin = 0.2f;
        protected Color unloadedColor = Color.red;
        protected Color loadingColor = Color.yellow;
        protected Color loadedColor = ColorHelper.green.pastel;

        private float progress;
        #endregion

        #region Properties
        protected float ProgressMargin
        {
            get { return progressMargin; }
            set { progressMargin = Mathf.Clamp01(value); }
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

            var rect         = GUILayoutUtility.GetLastRect();
            var progressRect = rect;
            progressRect.width *= progress;
            using (ColorScope scope = new ColorScope(GetProgressColor(progress)))
            {
                GUI.Box(progressRect, GUIContent.none, context.ProgressBar[1]);
            }

            using (new ColorScope(Color.black))
            {
                GUI.Label(rect, text, context.LabelCenter);
            }
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
            unloadedColor = Color.red;
            loadingColor = Color.yellow;
            loadedColor = ColorHelper.green.pastel;
        }

        private Color GetProgressColor(float progress)
        {
            var result = Color.Lerp(unloadedColor, loadedColor, Mathf.Clamp01(progress / progressMargin));
            result = Color.Lerp(result, loadedColor, Mathf.Clamp01((progress - (1f - progressMargin)) / progressMargin));
            return result;
        }
        #endregion
    }
}
