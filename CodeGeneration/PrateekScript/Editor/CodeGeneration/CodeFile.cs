namespace Prateek.CodeGeneration.Code.PrateekScript.CodeGeneration
{
    using System.Collections.Generic;
    using Prateek.CodeGeneration.CodeBuilder.Editor.CodeBuilder;
    using Prateek.CodeGeneration.CodeBuilder.Editor.Utils;
    using Prateek.CodeGeneration.PrateekScript.Editor.ScriptActions;
    using Prateek.Core.Code.Helpers;
    using Prateek.Core.Code.Consts;

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
        private List<string> defineDirectives = new List<string>();
        private List<string> usingDirectives = new List<string>();
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
        public void AddDefine(string define)
        {
            defineDirectives.Add(define);
        }

        ///-----------------------------------------------------------------
        public void AddNamespace(string usingNamespace)
        {
            usingDirectives.Add(usingNamespace);
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
        public BuildResult Generate(string sourceHeader, string sourceCode)
        {
            var swapNamespace = (StringSwap) Glossary.Macros.namepaceTag;
            var swapClass = (StringSwap) Glossary.Macros.extensionClassTag;
            var swapPrefix = (StringSwap) Glossary.Macros.extensionPrefixTag;
            var swapDefines = (StringSwap) Glossary.Macros.codeDefineTag;
            var swapUsing = (StringSwap) Glossary.Macros.codeUsingTag;
            var swapCode = (StringSwap) Glossary.Macros.codeDataTag;
            var swapCodeTabs = (StringSwap) Glossary.Macros.codeDataTabsTag;
            var swapTabs = ((StringSwap) Glossary.Macros.codeTabsTag) + Glossary.Macros.codeTabs;

            var usingIndex = sourceCode.IndexOf(swapUsing.Original);
            var codeIndex = sourceCode.IndexOf(swapCode.Original);
            if (codeIndex < Const.INDEX_NONE)
            {
                return Prateek.CodeGeneration.CodeBuilder.Editor.CodeBuilder.BuildResult.ValueType.PrateekScriptSourceDataTagInvalid;
            }

            //Retrieve tab offset
            var usingTabs = usingIndex > Const.INDEX_NONE ? sourceCode.GetTabulation(usingIndex) : string.Empty;
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
                
                var defineSection = BuildDefineDirective(defineDirectives, scriptContent.defineDirectives);
                swapDefines -= defineSection;
                
                var usingSection = BuildUsingDirective(usingTabs, usingDirectives, scriptContent.usingDirectives);
                swapUsing -= usingSection;

                for (int c = 0; c < scriptContent.codeGenerated.Count; c++)
                {
                    var codeData = scriptContent.codeGenerated[c];
                    var destinationCode = sourceCode;

                    var exportCode = new ScriptContent.GeneratedCode() {className = codeData.className, code = string.Empty};
                    var generatedContent = codeData.code;

                    swapNamespace -= scriptContent.blockNamespace;
                    swapClass -= codeData.className + scriptContent.blockClassName;

                    swapPrefix = swapPrefix.Original;
                    for (var p = 0; p < scriptContent.blockClassPrefix.Count; p++)
                    {
                        swapPrefix += scriptContent.blockClassPrefix[p] + Strings.Separator.Space.S();
                    }

                    swapCode -= swapCodeTabs.Apply(generatedContent);
                    destinationCode = swapNamespace.Apply(destinationCode);
                    destinationCode = swapCode.Apply(destinationCode);
                    destinationCode = swapClass.Apply(destinationCode);
                    destinationCode = swapPrefix.Apply(destinationCode);
                    destinationCode = swapTabs.Apply(destinationCode);
                    destinationCode = swapDefines.Apply(destinationCode);
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

            return BuildResult.ValueType.Success;
        }

        ///-----------------------------------------------------------------
        public string BuildDefineDirective(params List<string>[] definesList)
        {
            var resultCode = string.Empty;
            foreach (var defines in definesList)
            {
                foreach (var define in defines)
                {
                    resultCode += $"#define {define}".NewLine();
                }
            }

            return resultCode;
        }

        ///-----------------------------------------------------------------
        public string BuildUsingDirective(string currentTabs, params List<string>[] namespacesList)
        {
            var resultCode = string.Empty;
            foreach (var namespaces in namespacesList)
            {
                foreach (var namespaceName in namespaces)
                {
                    if (!string.IsNullOrEmpty(resultCode))
                    {
                        resultCode += $"{currentTabs}using {namespaceName};".NewLine();
                    }
                    else
                    {
                        resultCode += $"using {namespaceName};".NewLine();
                    }
                }
            }

            return resultCode;
        }
        #endregion
    }
}
