namespace Assets.Prateek.ToConvert.DebugMenu
{
    using Assets.Prateek.ToConvert.DebugMenu.Fields;
    using UnityEngine;

    public abstract class ScopeField<T> : GUI.Scope
        where T : ScopeableField, new()
    {
        #region Fields
        protected T beginScope = new T();
        protected T endScope = new T();
        private DebugMenuContext context;
        #endregion

        #region Class Methods
        protected void OpenScope(DebugMenuContext context)
        {
            this.context = context;

            beginScope.Setup(this.context, true);
            endScope.Setup(this.context, false);

            beginScope.Draw(context);
        }

        protected override void CloseScope()
        {
            endScope.Draw(context);
        }
        #endregion
    }
}
