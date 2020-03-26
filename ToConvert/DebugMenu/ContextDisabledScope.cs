namespace Assets.Prateek.ToConvert.DebugMenu
{
    using Assets.Prateek.ToConvert.DebugMenu.Fields;

    public class ContextDisabledScope : ScopeField<ContextDisabledField>
    {
        #region Constructors
        public ContextDisabledScope(DebugMenuContext context, bool disabled)
        {
            beginScope.Disabled = disabled;
            beginScope.CloseScope = endScope;

            OpenScope(context);
        }
        #endregion
    }
}
