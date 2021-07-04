namespace Prateek.Editor.CodeGeneration.PrateekScript.ScriptAnalysis.SyntaxSymbols
{
    public interface IScopeOpen
    {
        bool Match(IScopeClose other);
    }
}