namespace Mayfair.Core.Code.DebugMenu.Fields
{
    using UnityEngine;

    public class TextAreaField : DebugField
    {
        public string Value { get; set; }

        public TextAreaField()
        {
            Value = "";
        }

        public TextAreaField(string defaultValue)
        {
            Value = defaultValue;
        }

        #region  Unity Methods

        public override void OnGUI(DebugMenuContext context)
        {
            GUILayout.TextArea(Value, context.LabelLeft);
        }

        #endregion

        public void Draw(DebugMenuContext context, string text)
        {
            Value = text;
            Draw(context);
        }
    }
}