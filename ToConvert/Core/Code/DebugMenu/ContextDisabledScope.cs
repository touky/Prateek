namespace Mayfair.Core.Code.DebugMenu
{
    using Mayfair.Core.Code.DebugMenu.Fields;

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
