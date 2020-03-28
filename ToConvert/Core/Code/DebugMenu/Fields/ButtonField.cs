namespace Mayfair.Core.Code.DebugMenu.Fields
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
            if (GUILayout.Button(this.text, context.Button))
            {
                this.trigger = true;
            }
        }
        #endregion

        #region Class Methods
        public new bool Draw(DebugMenuContext context, string text)
        {
            this.text = text;

            Draw(context);

            if (this.trigger)
            {
                this.trigger = false;
                return true;
            }

            return false;
        }
        #endregion
    }
}
