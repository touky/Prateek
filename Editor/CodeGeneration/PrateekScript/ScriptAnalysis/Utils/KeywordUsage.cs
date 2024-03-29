namespace Prateek.Editor.CodeGeneration.PrateekScript.ScriptAnalysis.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Prateek.Editor.CodeGeneration.CodeBuilder.RuntimeBuilder;
    using Prateek.Editor.CodeGeneration.CodeBuilder.Utils;
    using Prateek.Editor.CodeGeneration.PrateekScript.RuntimeGeneration;
    using Prateek.Editor.CodeGeneration.PrateekScript.ScriptAnalysis.IntermediateRuntime;
    using Prateek.Editor.CodeGeneration.PrateekScript.ScriptAnalysis.SyntaxSymbols;

    [DebuggerDisplay("{scope}/{keyword}")]
    public struct KeywordUsage
    {
        ///-------------------------------------------------------------
        public string keyword;
        public string scope;
        public KeywordUsageType keywordUsageType;
        public ArgumentRange arguments;
        public bool needOpenScope;
        public bool needScopeData;
        public bool createNewScriptContent;
        public Func<FileData, CodeFile, ScriptContent, List<IKeyword>, string, bool> onFeedCodeFile;
        public Func<CodeFile, string, bool> onCloseScope;

        ///-------------------------------------------------------------
        public KeywordUsage(string keyword, string scope)
        {
            this.keyword = keyword;
            this.scope = scope;
            keywordUsageType = KeywordUsageType.Match;
            arguments = 0;
            needOpenScope = false;
            needScopeData = false;
            createNewScriptContent = false;
            onFeedCodeFile = null;
            onCloseScope = null;
        }

        ///-------------------------------------------------------------
        public bool Match(string key, string scope)
        {
            return key == keyword && scope == this.scope;
        }

        public bool ValidateRule(CodeKeyword codeKeyword, string scope)
        {
            if (keyword != codeKeyword.keyword.Content || this.scope != scope)
            {
                return false;
            }

            if (!arguments.Check(codeKeyword.arguments.Count))
            {
                return false;
            }

            if (needOpenScope)
            {
                if (codeKeyword.scopeContent == null)
                {
                    return false;
                }
            }

            if (needScopeData)
            {
                if (codeKeyword.scopeContent == null)
                {
                    return false;
                }

                if (codeKeyword.scopeContent.innerCommands.Count == 0)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
