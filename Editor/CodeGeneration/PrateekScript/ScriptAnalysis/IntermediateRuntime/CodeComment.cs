namespace Prateek.Editor.CodeGeneration.PrateekScript.ScriptAnalysis.IntermediateRuntime
{
    using Prateek.Editor.CodeGeneration.PrateekScript.ScriptAnalysis.SyntaxSymbols;

    public class CodeComment
        : CodeCommand
        , IComment
    {
        #region Class Methods
        public override bool CanAbsorb(Symbol symbol)
        {
            return symbol is IComment;
        }

        public override void Add(Symbol symbol)
        {
            if (symbol is IComment comment)
            {
                comments.Add(comment);
            }
        }
        #endregion
    }
}
