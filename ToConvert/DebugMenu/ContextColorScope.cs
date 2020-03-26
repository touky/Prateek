namespace Assets.Prateek.ToConvert.DebugMenu
{
    using Assets.Prateek.ToConvert.DebugMenu.Fields;
    using UnityEngine;

    public class ContextColorScope : ScopeField<ContextColorField>
    {
        #region Constructors
        public ContextColorScope(DebugMenuContext context, Color color)
        {
            beginScope.Color = color;
            beginScope.CloseScope = endScope;

            OpenScope(context);
        }
        #endregion
    }
}
