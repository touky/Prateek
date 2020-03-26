namespace Assets.Prateek.ToConvert.DebugMenu.Fields
{
    using UnityEngine;

    public class ContextColorField : ScopeableField
    {
        #region Fields
        private Color color;
        private ContextColorField closeScope;
        #endregion

        #region Properties
        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        public ContextColorField CloseScope
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
            closeScope.color = GUI.color;
            GUI.color = color;
        }

        protected override void OnGUIClose(DebugMenuContext context)
        {
            GUI.color = color;
        }
        #endregion
    }
}
