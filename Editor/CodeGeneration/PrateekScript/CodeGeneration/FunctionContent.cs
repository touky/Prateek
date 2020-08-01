namespace Prateek.CodeGeneration.Code.PrateekScript.CodeGeneration
{
    using System.Diagnostics;

    [DebuggerDisplay("{funcName}: {body}")]
    public struct FunctionContent
    {
        public string funcName;
        public string body;
    }
}
