namespace Mayfair.Core.Code.DebugMenu
{
    using Mayfair.Core.Code.DebugMenu.Fields;

    public class ContextIndentScope : ScopeField<ContextIndentField>
    {
        #region Constructors
        public ContextIndentScope(DebugMenuContext context, int indent = 1)
        {
            beginScope.IndentLevel = indent;

            OpenScope(context);
        }
        #endregion
    }
}
