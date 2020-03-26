namespace Assets.Prateek.ToConvert.DebugMenu.Fields
{
    public abstract class ScopeableField : DebugField
    {
        #region Fields
        private bool closeScope;
        #endregion

        #region Unity EditorOnly Methods
        public override void OnGUI(DebugMenuContext context)
        {
            if (!closeScope)
            {
                OnGUIOpen(context);
            }
            else
            {
                OnGUIClose(context);
            }
        }
        #endregion

        #region Class Methods
        public void Setup(DebugMenuContext context, bool openScope)
        {
            closeScope = !openScope;
            if (openScope)
            {
                SetupOpen(context);
            }
            else
            {
                SetupClose(context);
            }
        }

        public override void Draw(DebugMenuContext context)
        {
            if (!closeScope)
            {
                DrawOpen(context);
            }
            else
            {
                DrawClose(context);
            }
        }

        protected abstract void SetupOpen(DebugMenuContext context);
        protected abstract void DrawOpen(DebugMenuContext context);
        protected abstract void OnGUIOpen(DebugMenuContext context);

        protected abstract void SetupClose(DebugMenuContext context);
        protected abstract void DrawClose(DebugMenuContext context);
        protected abstract void OnGUIClose(DebugMenuContext context);
        #endregion
    }
}
