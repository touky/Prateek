namespace Assets.Prateek.CodeGenerator.Code.PrateekScript.CodeGeneration
{
    using System.Collections.Generic;
    using System.Linq;
    using Assets.Prateek.CodeGenerator.Code.PrateekScript.ScriptActions;
    using Assets.Prateek.CodeGenerator.Code.PrateekScript.ScriptAnalysis.SyntaxSymbols;

    public class ScriptContent
    {
        #region Fields
        public ScriptAction scriptAction;
        public string blockNamespace;
        public string blockClassName;
        public List<string> blockClassPrefix = new List<string>();

        public List<ClassContent> classInfos = new List<ClassContent>();
        public List<FunctionContent> functionContents = new List<FunctionContent>();
        public string classDefaultType;
        public string classDefaultValue;
        public bool classDefaultExportOnly;

        public string codePrefix;
        public string codeMain;
        public string codePostfix;

        public string codeGenerated;
        #endregion

        #region Class Methods
        ///-------------------------------------------------------------
        public bool SetClassNames(List<Keyword> arguments)
        {
            if (classInfos.Count == 0)
            {
                return false;
            }

            var infos = classInfos.Last();
            if (infos.names == null)
            {
                infos.names = new List<string>();
            }

            foreach (var argument in arguments)
            {
                infos.names.Add(argument.Content);
            }

            classInfos[classInfos.Count - 1] = infos;

            return true;
        }

        ///-------------------------------------------------------------
        public bool SetClassVars(List<Keyword> arguments)
        {
            if (classInfos.Count == 0)
            {
                return false;
            }

            var infos = classInfos.Last();
            if (infos.variables == null)
            {
                infos.variables = new List<string>();
            }

            foreach (var argument in arguments)
            {
                infos.variables.Add(argument.Content);
            }

            classInfos[classInfos.Count - 1] = infos;
            return true;
        }

        ///-------------------------------------------------------------
        public bool SetFuncData(string data)
        {
            if (functionContents.Count == 0)
            {
                return false;
            }

            var infos = functionContents.Last();
            infos.data = data;
            functionContents[functionContents.Count - 1] = infos;
            return true;
        }
        #endregion
    }
}
