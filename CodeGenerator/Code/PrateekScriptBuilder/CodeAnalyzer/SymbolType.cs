namespace Assets.Prateek.CodeGenerator.Code.PrateekScriptBuilder.CodeAnalyzer
{
    using UnityEngine.PlayerLoop;

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