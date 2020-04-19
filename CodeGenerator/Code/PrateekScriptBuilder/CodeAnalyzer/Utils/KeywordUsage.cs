namespace Assets.Prateek.CodeGenerator.Code.PrateekScriptBuilder.CodeAnalyzer.Utils {
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Assets.Prateek.CodeGenerator.Code.PrateekScriptBuilder.CodeAnalyzer.Symbols;
    using Assets.Prateek.CodeGenerator.Code.PrateekScriptBuilder.CodeCommands;
    using Assets.Prateek.CodeGenerator.Code.Utils;
    using global::Prateek.CodeGenerator.PrateekScriptBuilder;

    [DebuggerDisplay("{scope}/{keyword}")]
    public struct KeywordUsage
    {
        //-------------------------------------------------------------
        public enum Usage
        {
            None,

            Match,
            Forbidden,
            Ignore,

            MAX
        }

        //-------------------------------------------------------------

        //-------------------------------------------------------------
        public string keyword;
        public string scope;
        public Usage usage;
        public ArgumentRange arguments;
        public bool needOpenScope;
        public bool needScopeData;
        public Func<PrateekScriptBuilder.CodeFile.ContentInfos, List<Keyword>, string, bool> onFeedCodeFile;
        public Func<PrateekScriptBuilder.CodeFile, string, bool> onCloseScope;

        //-------------------------------------------------------------
        public KeywordUsage(string keyword, string scope)
        {
            this.keyword = keyword;
            this.scope = scope;
            usage = Usage.Match;
            arguments = 0;
            needOpenScope = false;
            needScopeData = false;
            onFeedCodeFile = null;
            onCloseScope = null;
        }

        //-------------------------------------------------------------
        public bool Match(string key, string scope)
        {
            return key == this.keyword && scope == this.scope;
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

                if (codeKeyword.scopeContent.commands.Count == 0)
                {
                    return false;
                }
            }

            return true;
        }
    }
}