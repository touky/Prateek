namespace Prateek.Editor.CodeGeneration.PrateekScript.RuntimeGeneration
{
    using System;
    using System.Collections.Generic;
    using Prateek.Editor.CodeGeneration.CodeBuilder.RuntimeBuilder;
    using Prateek.Editor.CodeGeneration.CodeBuilder.Utils;
    using Prateek.Editor.CodeGeneration.PrateekScript.ScriptActions;
    using Prateek.Runtime.Core.Consts;
    using Prateek.Runtime.Core.Helpers;

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
        public BuildResult Generate(string defaultfileHeader, string defaultFileBody)
        {
            var swapNamespace = (StringSwap) Glossary.Macros.namepaceTag;
            var swapClass     = (StringSwap) Glossary.Macros.extensionClassTag;
            var swapPrefix    = (StringSwap) Glossary.Macros.extensionPrefixTag;
            var swapDefines   = (StringSwap) Glossary.Macros.codeDefineTag;
            var swapUsing     = (StringSwap) Glossary.Macros.codeUsingTag;
            var swapCode      = (StringSwap) Glossary.Macros.codeDataTag;
            var swapCodeTabs  = (StringSwap) Glossary.Macros.codeDataTabsTag;
            var swapTabs      = (StringSwap) Glossary.Macros.codeTabsTag + Glossary.Macros.codeTabs;

            var usingDirectiveIndex = defaultFileBody.IndexOf(swapUsing.Original, StringComparison.InvariantCulture);
            var defaultCodeInjectIndex = defaultFileBody.IndexOf(swapCode.Original, StringComparison.InvariantCulture);
            if (defaultCodeInjectIndex < Const.INDEX_NONE)
            {
                return BuildResult.ValueType.PrateekScriptSourceDataTagInvalid;
            }

            //Retrieve tab offset
            var usingDirectiveTabs = defaultFileBody.GetTabulation(usingDirectiveIndex);

            for (var d = 0; d < scriptContents.Count; d++)
            {
                var fileHeader = defaultfileHeader;
                var fileBody   = defaultFileBody;
                swapCodeTabs.Replacement = defaultFileBody.GetTabulation(defaultCodeInjectIndex);

                var scriptContent = scriptContents[d];
                var result = scriptContent.scriptAction.Generate(scriptContent);
                if (!result)
                {
                    return result;
                }

                if (!string.IsNullOrEmpty(scriptContent.fileBody))
                {
                    var newCodeInjectIndex = scriptContent.fileBody.IndexOf(swapCode.Original, StringComparison.InvariantCulture);
                    if (newCodeInjectIndex <= Const.INDEX_NONE)
                    {
                        fileBody = Glossary.Macros.codeDataTag;
                        swapCodeTabs.Replacement = string.Empty;
                    }
                    else
                    {
                        fileBody = scriptContent.fileBody;
                        swapCodeTabs.Replacement = defaultFileBody.GetTabulation(newCodeInjectIndex);
                    }
                }

                scriptContent.codeGenerated.Sort((a, b) =>
                {
                    return string.Compare(a.className, b.className, StringComparison.InvariantCulture);
                });

                var defineSection = BuildDefineDirective(defineDirectives, scriptContent.defineDirectives);
                swapDefines.Replacement = defineSection;

                var usingSection = BuildUsingDirective(usingDirectiveTabs, usingDirectives, scriptContent.usingDirectives);
                swapUsing.Replacement = usingSection;

                for (var c = 0; c < scriptContent.codeGenerated.Count; c++)
                {
                    var codeData        = scriptContent.codeGenerated[c];
                    var destinationCode = fileBody;

                    var exportCode       = new ScriptContent.GeneratedCode {className = codeData.className, code = string.Empty};
                    var generatedContent = codeData.code;

                    swapNamespace.Replacement = scriptContent.blockNamespace;
                    swapClass.Replacement = codeData.className + scriptContent.blockClassName;

                    swapPrefix = swapPrefix.Original;
                    for (var p = 0; p < scriptContent.blockClassPrefix.Count; p++)
                    {
                        swapPrefix += scriptContent.blockClassPrefix[p] + Strings.Separator.Space.S();
                    }

                    swapCode.Replacement = swapCodeTabs.Apply(generatedContent);
                    destinationCode = swapNamespace.Apply(destinationCode);
                    destinationCode = swapCode.Apply(destinationCode);
                    destinationCode = swapClass.Apply(destinationCode);
                    destinationCode = swapPrefix.Apply(destinationCode);
                    destinationCode = swapTabs.Apply(destinationCode);
                    destinationCode = swapDefines.Apply(destinationCode);
                    destinationCode = swapUsing.Apply(destinationCode);
                    exportCode.code = destinationCode;

                    var index = codeGenerated.FindIndex(x => { return x.className == exportCode.className; });
                    if (index != Const.INDEX_NONE)
                    {
                        var oldCode = codeGenerated[index];
                        oldCode.code += exportCode.code;
                        codeGenerated[index] = oldCode;
                    }
                    else
                    {
                        exportCode.code = fileHeader + exportCode.code;
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

        ///-----------------------------------------------------------------
        public static string CleanPrateekComments(string fileBody)
        {
            var swapComments = new[]
            {
                (StringSwap) Glossary.Macros.csharpCommentLine + string.Empty,
                (StringSwap) Glossary.Macros.csharpCommentOpen + string.Empty,
                (StringSwap) Glossary.Macros.csharpCommentClose + string.Empty
            };

            foreach (var swapComment in swapComments)
            {
                fileBody = swapComment.Apply(fileBody);
            }

            return fileBody;
        }
        #endregion
    }
}
