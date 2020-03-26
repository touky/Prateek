namespace Assets.Prateek.ToConvert.DebugMenu.Fields
{
    using UnityEngine;

    public class TextAreaField : DebugField
    {
        #region Properties
        public string Value { get; set; }
        #endregion

        #region Constructors
        public TextAreaField()
        {
            Value = "";
        }

        public TextAreaField(string defaultValue)
        {
            Value = defaultValue;
        }
        #endregion

        #region Unity EditorOnly Methods
        public override void OnGUI(DebugMenuContext context)
        {
            GUILayout.TextArea(Value, context.LabelLeft);
        }
        #endregion

        #region Class Methods
        public void Draw(DebugMenuContext context, string text)
        {
            Value = text;
            Draw(context);
        }
        #endregion
    }
}
