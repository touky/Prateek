namespace Assets.Prateek.ToConvert.DebugMenu.Fields
{
    using UnityEngine;

    public class InputField : DebugField
    {
        #region Properties
        public string Value { get; set; }
        #endregion

        #region Constructors
        public InputField()
        {
            Value = "";
        }

        public InputField(string defaultValue)
        {
            Value = defaultValue;
        }
        #endregion

        #region Unity EditorOnly Methods
        public override void OnGUI(DebugMenuContext context)
        {
            Value = GUILayout.TextField(Value, context.InputField);
        }
        #endregion

        #region Class Methods
        public void Draw(DebugMenuContext context, out string text)
        {
            text = Value;
            Draw(context);
        }
        #endregion
    }
}
