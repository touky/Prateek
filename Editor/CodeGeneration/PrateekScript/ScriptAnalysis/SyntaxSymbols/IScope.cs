namespace Prateek.Editor.CodeGeneration.PrateekScript.ScriptAnalysis.SyntaxSymbols
{
    public interface IScope
    {
        Symbol GetOpenSymbol();
        Symbol GetCloseSymbol();

        Symbol GetSeparator();
    }
}