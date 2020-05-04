namespace Mayfair.Core.Code.DebugMenu.Fields
{
    using UnityEngine;

    public class ContextIndentField : ScopeableField
    {
        #region Fields
        private int indentLevel;
        #endregion

        #region Properties
        public int IndentLevel
        {
            get { return indentLevel; }
            set { indentLevel = value; }
        }
        #endregion

        #region Class Methods
        protected override void SetupOpen(DebugMenuContext context) { }

        protected override void SetupClose(DebugMenuContext context)
        {
            indentLevel = context.Indent;
        }

        protected override void DrawOpen(DebugMenuContext context)
        {
            context.Indent += indentLevel;
            context.Draw(this);
        }

        protected override void DrawClose(DebugMenuContext context)
        {
            context.Indent = indentLevel;
            context.Draw(this);
        }

        protected override void OnGUIOpen(DebugMenuContext context)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(indentLevel * 10);
            GUILayout.BeginVertical();
        }

        protected override void OnGUIClose(DebugMenuContext context)
        {
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }
        #endregion
    }
}
