namespace Assets.Prateek.CodeGenerator.Code.PrateekScript.CodeGeneration {
    using System.Collections.Generic;

    public struct ClassInfos
    {
        //-------------------------------------------------------------
        public string className;
        public List<string> names;
        public List<string> variables;

        //-------------------------------------------------------------
        public int NameCount { get { return names == null ? 0 : names.Count; } }
        public int VarCount { get { return variables == null ? 0 : variables.Count; } }
    }
}