namespace Prateek.CodeGeneration.Code.PrateekScript.ScriptAnalysis.SyntaxSymbols
{
    public enum SymbolType
    {
        WhiteSpace,
        LineFeed,
        Numeric,
        Letter,
        Literal,
        LiteralEnd,
        ScopeStart,
        ScopeEnd,
        CallStart,
        CallEnd,
        ArgSplit,
        Comment,

        MAX
    }
}
