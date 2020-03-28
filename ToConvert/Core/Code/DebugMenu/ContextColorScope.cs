namespace Mayfair.Core.Code.DebugMenu
{
    using Mayfair.Core.Code.DebugMenu.Fields;
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
