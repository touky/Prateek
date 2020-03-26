namespace Assets.Prateek.ToConvert.DebugMenu.Fields
{
    using UnityEngine;

    public class ButtonField : StringField
    {
        #region Fields
        private bool trigger;
        #endregion

        #region Unity EditorOnly Methods
        public override void OnGUI(DebugMenuContext context)
        {
            if (GUILayout.Button(text, context.Button))
            {
                trigger = true;
            }
        }
        #endregion

        #region Class Methods
        public new bool Draw(DebugMenuContext context, string text)
        {
            this.text = text;

            Draw(context);

            if (trigger)
            {
                trigger = false;
                return true;
            }

            return false;
        }
        #endregion
    }
}
