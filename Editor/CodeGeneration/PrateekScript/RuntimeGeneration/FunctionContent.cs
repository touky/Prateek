namespace Prateek.Editor.CodeGeneration.PrateekScript.RuntimeGeneration
{
    using System.Diagnostics;

    [DebuggerDisplay("{funcName}: {body}")]
    public struct FunctionContent
    {
        public string funcName;
        public string body;
    }
}
