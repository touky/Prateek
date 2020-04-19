namespace Assets.Prateek.CodeGenerator.Code.PrateekScript.CodeGeneration {
    using System.Collections.Generic;
    using System.Linq;
    using Assets.Prateek.CodeGenerator.Code.PrateekScriptBuilder.CodeAnalyzer.Symbols;
    using global::Prateek.CodeGenerator.PrateekScriptBuilder;

    public class ContentInfos
    {
        public ScriptAction activeRule;
        public string blockNamespace;
        public string blockClassName;
        public List<string> blockClassPrefix = new List<string>();

        public List<ClassInfos> classInfos = new List<ClassInfos>();
        public List<FuncInfos> funcInfos = new List<FuncInfos>();
        public string classDefaultType;
        public string classDefaultValue;
        public bool classDefaultExportOnly;

        public string codePrefix;
        public string codeMain;
        public string codePostfix;

        public string codeGenerated;

        //-------------------------------------------------------------
        public bool SetClassNames(List<string> args)
        {
            if (classInfos.Count == 0)
                return false;
            var infos = classInfos.Last();
            if (infos.names == null)
                infos.names = new List<string>();
            infos.names.AddRange(args);
            classInfos[classInfos.Count - 1] = infos;
            return true;
        }

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

        //-------------------------------------------------------------
        public bool SetClassVars(List<string> args)
        {
            if (classInfos.Count == 0)
                return false;
            var infos = classInfos.Last();
            if (infos.variables == null)
                infos.variables = new List<string>();
            infos.variables.AddRange(args);
            classInfos[classInfos.Count - 1] = infos;
            return true;
        }
        public bool SetClassVars(List<Keyword> arguments)
        {
            if (classInfos.Count == 0)
                return false;
            var infos = classInfos.Last();
            if (infos.variables == null)
                infos.variables = new List<string>();
            foreach (var argument in arguments)
            {
                infos.variables.Add(argument.Content);
            }
            classInfos[classInfos.Count - 1] = infos;
            return true;
        }

        //-------------------------------------------------------------
        public bool SetFuncData(string data)
        {
            if (funcInfos.Count == 0)
                return false;
            var infos = funcInfos.Last();
            infos.data = data;
            funcInfos[funcInfos.Count - 1] = infos;
            return true;
        }
    }
}