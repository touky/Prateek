namespace Prateek.Editor.CodeGeneration.PrateekScript
{
    using Prateek.Editor.CodeGeneration.PrateekScript.ScriptAnalysis.IntermediateRuntime;
    using Prateek.Editor.CodeGeneration.PrateekScript.ScriptAnalysis.SyntaxSymbols;

    public class PrateekScriptSymbolRegistry
        : SymbolRegistry
    {
        #region Class Methods
        public override void Init()
        {
            base.Init();

            Bind<CommentMultiline, CodeComment>();
            Bind<CommentSingleLine, CodeComment>();
            Bind<LiteralValue, CodeLiteral>();
            Bind<ScopeCode, CodeScope>();
            Bind<ScopeInvoke, InvokeScope<VariableSeparator>, VariableSeparator>();
            Bind<Keyword, CodeKeyword>();
            Bind<Value, CodeKeyword>();
        }
        #endregion
    }
}