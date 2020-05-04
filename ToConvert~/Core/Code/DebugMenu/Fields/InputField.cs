namespace Mayfair.Core.Code.DebugMenu.Fields
{
    using UnityEngine;

    public class InputField : DebugField
    {
        public string Value { get; set; }

        public InputField()
        {
            Value = "";
        }

        public InputField(string defaultValue)
        {
            Value = defaultValue;
        }

        #region  Unity Methods

        public override void OnGUI(DebugMenuContext context)
        {
            Value = GUILayout.TextField(Value, context.InputField);
        }

        #endregion

        public void Draw(DebugMenuContext context, out string text)
        {
            text = Value;
            Draw(context);
        }
    }
}