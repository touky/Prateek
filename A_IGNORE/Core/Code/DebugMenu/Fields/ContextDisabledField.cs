namespace Mayfair.Core.Code.DebugMenu.Fields
{
    using UnityEngine;

    public class ContextDisabledField : ScopeableField
    {
        #region Fields
        private bool disabled;
        private ContextDisabledField closeScope;
        #endregion

        #region Properties
        public bool Disabled
        {
            get { return disabled; }
            set { disabled = value; }
        }

        public ContextDisabledField CloseScope
        {
            get { return closeScope; }
            set { closeScope = value; }
        }
        #endregion

        #region Class Methods
        protected override void SetupOpen(DebugMenuContext context) { }

        protected override void SetupClose(DebugMenuContext context) { }

        protected override void DrawOpen(DebugMenuContext context)
        {
            context.Draw(this);
        }

        protected override void DrawClose(DebugMenuContext context)
        {
            context.Draw(this);
        }

        protected override void OnGUIOpen(DebugMenuContext context)
        {
            closeScope.disabled = !GUI.enabled;
            GUI.enabled = !disabled;
        }

        protected override void OnGUIClose(DebugMenuContext context)
        {
            GUI.enabled = !disabled;
        }
        #endregion
    }
}
