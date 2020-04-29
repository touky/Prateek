namespace Assets.Prateek.CodeGenerator.Code.PrateekScript.CodeGeneration
{
    using System.Collections.Generic;
    using Assets.Prateek.CodeGenerator.Code.PrateekScript.ScriptActions;
    using Assets.Prateek.CodeGenerator.Code.Utils;
    using global::Prateek;
    using global::Prateek.CodeGenerator;
    using global::Prateek.Core.Code.Extensions;
    using global::Prateek.Core.Code.Helpers;

    public class CodeFile
    {
        #region Fields
        ///-----------------------------------------------------------------
        public string fileName;
        public string fileExtension;
        public string fileNamespace;

        ///-----------------------------------------------------------------
        private ScriptContent scriptContent;
        private List<ScriptContent.GeneratedCode> codeGenerated = new List<ScriptContent.GeneratedCode>();
        private List<ScriptContent> scriptContents = new List<ScriptContent>();
        private List<string> usingNamespaces = new List<string>();
        #endregion

        #region Properties
        ///-----------------------------------------------------------------
        public List<ScriptContent.GeneratedCode> CodeGenerated
        {
            get { return codeGenerated; }
        }

        public ScriptContent ScriptContent
        {
            get { return scriptContent; }
        }

        public int DataCount
        {
            get { return scriptContents.Count; }
        }

        public ScriptContent this[int index]
        {
            get { return scriptContents[index]; }
        }
        #endregion

        #region Class Methods
        ///-----------------------------------------------------------------
        public bool AllowRule(ScriptAction rule)
        {
            if (scriptContent == null)
            {
                return true;
            }

            if (scriptContent.scriptAction == null)
            {
                return true;
            }

            return scriptContent.scriptAction == rule;
        }

        ///-----------------------------------------------------------------
        public ScriptContent NewData(ScriptAction codeSettings)
        {
            if (scriptContent != null)
            {
                return null;
            }

            scriptContent = new ScriptContent {scriptAction = codeSettings};
            return scriptContent;
        }

        ///-----------------------------------------------------------------
        public void AddNamespace(string usingNamespace)
        {
            usingNamespaces.Add(usingNamespace);
        }

        ///-----------------------------------------------------------------
        public bool Commit()
        {
            var hasSubmitted = scriptContent != null;
            if (scriptContent != null)
            {
                scriptContents.Add(scriptContent);
            }

            scriptContent = null;

            return hasSubmitted;
        }

        ///-----------------------------------------------------------------
        public global::Assets.Prateek.CodeGenerator.Code.CodeBuilder.BuildResult Generate(string sourceHeader, string sourceCode)
        {
            var swapNamespace = (StringSwap) Glossary.Macros.namepaceTag;
            var swapClass = (StringSwap) Glossary.Macros.extensionClassTag;
            var swapPrefix = (StringSwap) Glossary.Macros.extensionPrefixTag;
            var swapUsing = (StringSwap) Glossary.Macros.codeUsingTag;
            var swapCode = (StringSwap) Glossary.Macros.codeDataTag;
            var swapCodeTabs = (StringSwap) Glossary.Macros.codeDataTabsTag;
            var swapTabs = ((StringSwap) Glossary.Macros.codeTabsTag) + Glossary.Macros.codeTabs;

            var usingIndex = sourceCode.IndexOf(swapUsing.Original);
            var codeIndex = sourceCode.IndexOf(swapCode.Original);
            if (codeIndex < Const.INDEX_NONE || usingIndex <= Const.INDEX_NONE)
            {
                return global::Assets.Prateek.CodeGenerator.Code.CodeBuilder.BuildResult.ValueType.PrateekScriptSourceDataTagInvalid;
            }

            //Retrieve tab offset
            var usingTabs = sourceCode.GetTabulation(usingIndex);
            swapCodeTabs = swapCodeTabs + sourceCode.GetTabulation(codeIndex);

            for (var d = 0; d < scriptContents.Count; d++)
            {
                var scriptContent = scriptContents[d];

                var result = scriptContent.scriptAction.Generate(scriptContent);
                if (!result)
                {
                    return result;
                }

                scriptContent.codeGenerated.Sort((a, b) =>
                {
                    return string.Compare(a.className, b.className);
                });

                var namespaces = string.Empty;
                foreach (var usingNamespace in usingNamespaces)
                {
                    if (!string.IsNullOrEmpty(namespaces))
                    {
                        namespaces += $"{usingTabs}using {usingNamespace};".NewLine();
                    }
                    else
                    {
                        namespaces += $"using {usingNamespace};".NewLine();
                    }
                }

                swapUsing += namespaces;

                for (int c = 0; c < scriptContent.codeGenerated.Count; c++)
                {
                    var codeData = scriptContent.codeGenerated[c];
                    var destinationCode = sourceCode;

                    var exportCode = new ScriptContent.GeneratedCode() {className = codeData.className, code = string.Empty};
                    var generatedContent = codeData.code;

                    swapNamespace += scriptContent.blockNamespace;
                    swapClass += scriptContent.blockClassName;
                    
                    for (var p = 0; p < scriptContent.blockClassPrefix.Count; p++)
                    {
                        swapPrefix += scriptContent.blockClassPrefix[p] + Strings.Separator.Space.S();
                    }

                    swapCode += swapCodeTabs.Apply(generatedContent);
                    destinationCode = swapNamespace.Apply(destinationCode);
                    destinationCode = swapCode.Apply(destinationCode);
                    destinationCode = swapClass.Apply(destinationCode);
                    destinationCode = swapPrefix.Apply(destinationCode);
                    destinationCode = swapTabs.Apply(destinationCode);
                    destinationCode = swapUsing.Apply(destinationCode);
                    exportCode.code = destinationCode;

                    var index = codeGenerated.FindIndex((x) => { return x.className == exportCode.className; });
                    if (index != Const.INDEX_NONE)
                    {
                        var oldCode = codeGenerated[index];
                        oldCode.code += exportCode.code;
                        codeGenerated[index] = oldCode;
                    }
                    else
                    {
                        exportCode.code = sourceHeader + exportCode.code;
                        codeGenerated.Add(exportCode);
                    }
                }
            }

            return global::Assets.Prateek.CodeGenerator.Code.CodeBuilder.BuildResult.ValueType.Success;
        }
        #endregion
    }
}