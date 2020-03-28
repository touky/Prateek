namespace Mayfair.Core.Code.DebugMenu.Fields
{
    using UnityEngine;

    /// <summary>
    ///     A toggle field, with a checkbox and a Toggled property
    /// </summary>
    public class ToggleField : DebugField
    {
        #region Fields
        private string title;
        private bool toggled;
        #endregion

        #region Properties
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public bool Toggled
        {
            get { return toggled; }
        }
        #endregion

        #region Constructors
        public ToggleField() { }

        public ToggleField(string title)
        {
            this.title = title;
        }
        #endregion

        #region Unity EditorOnly Methods
        public override void OnGUI(DebugMenuContext context)
        {
            toggled = GUILayout.Toggle(toggled, title);
        }
        #endregion

        #region Class Methods
        public void Draw(DebugMenuContext context, bool value)
        {
            toggled = value;

            Draw(context);
        }
        #endregion
    }
}
