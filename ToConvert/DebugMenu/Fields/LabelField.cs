namespace Assets.Prateek.ToConvert.DebugMenu.Fields
{
    using UnityEngine;

    /// <summary>
    ///     A simple label field, draws the text content
    /// </summary>
    public class LabelField : StringField
    {
        #region Constructors
        public LabelField() : base() { }
        public LabelField(string text) : base(text) { }
        #endregion

        #region Unity EditorOnly Methods
        public override void OnGUI(DebugMenuContext context)
        {
            GUILayout.Label(text, context.LabelLeft);
        }
        #endregion
    }
}
