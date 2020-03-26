namespace Assets.Prateek.ToConvert.DebugMenu
{
    using Assets.Prateek.ToConvert.DebugMenu.Fields;

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
