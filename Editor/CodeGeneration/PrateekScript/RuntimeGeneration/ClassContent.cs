namespace Prateek.Editor.CodeGeneration.PrateekScript.RuntimeGeneration
{
    using System.Collections.Generic;
    using System.Diagnostics;

    [DebuggerDisplay("{className}: {names} / {variables}")]
    public struct ClassContent
    {
        ///-------------------------------------------------------------
        public string className;
        public List<string> names;
        public List<string> variables;

        ///-------------------------------------------------------------
        public int NameCount
        {
            get { return names == null ? 0 : names.Count; }
        }

        public int VarCount
        {
            get { return variables == null ? 0 : variables.Count; }
        }
    }
}
