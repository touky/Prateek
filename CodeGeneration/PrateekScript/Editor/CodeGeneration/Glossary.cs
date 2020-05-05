namespace Prateek.CodeGeneration.Code.PrateekScript.CodeGeneration
{
    using System;
    using System.Collections.Generic;
    using Prateek.CodeGeneration.Code.PrateekScript.ScriptActions;
    using Prateek.Core.Code.Helpers;

    public static class Glossary
    {
        #region Static and Constants
        //Default datas ---------------------------------------------------
        public const string importExtension = "prtk";
        public const string exportExtension = "cs";
        #endregion

        #region Nested type: Code
        //Code generation data -------------------------------------
        public static class Code
        {
            #region Static and Constants
            public const string argVarSeparator = ", ";
            public const string callN = "n";
            public const string argsV = "v";
            public const string argsV_ = "{0} v_{1}";
            public const string argsN = "{0} n_{1}";
            public const string argsNOpt = "{0} n_{1} = {2}";
            public const string varsN = "n_{0}";
            public const string varsV = "v.{0}";
            public const string varsV_ = "v_{0}.{1}";
            public const string varNew = "new ";
            #endregion
        }
        #endregion

        #region Nested type: Macro
        ///-----------------------------------------------------------------

        ///-----------------------------------------------------------------
        public static Macro Macros = new Macro();
        public class Macro
        {
            #region Static and Constants
            ///-------------------------------------------------------------
            public string scriptStartTag;
            public string namepaceTag;
            public string extensionClassTag;
            public string extensionPrefixTag;
            public string codeUsingTag;
            public string codeDataTag;
            public string codeDataTabsTag;
            public string codeTabsTag;
            public string codeTabs;
            public string codeBlockFormat;

            public string prefix = "PRATEEK";
            public string codeData = "CODE";

            ///-------------------------------------------------------------
            private NumberedSymbol names;
            private NumberedSymbol variables;
            private NumberedSymbol functions;

            ///-------------------------------------------------------------
            public static string srcClass;
            public static string dstClass;

            ///-------------------------------------------------------------
            private Dictionary<string, string> datas = new Dictionary<string, string>();
            #endregion

            #region Properties
            ///-------------------------------------------------------------
            public string this[FunctionKeyword functionKeyword]
            {
                get
                {
                    if (datas.TryGetValue(functionKeyword.ToString(), out string result))
                    {
                        return result;
                    }

                    return string.Empty;
                }
            }

            ///-------------------------------------------------------------
            public string this[VariableKeyword funcName]
            {
                get
                {
                    if (datas.TryGetValue(funcName.ToString(), out string result))
                    {
                        return result;
                    }

                    return string.Empty;
                }
            }

            ///-------------------------------------------------------------
            public NumberedSymbol Names
            {
                get { return names; }
            }

            public NumberedSymbol Functions
            {
                get { return functions; }
            }

            public NumberedSymbol Variables
            {
                get { return variables; }
            }
            #endregion

            #region Class Methods
            ///-------------------------------------------------------------
            public void Init()
            {
                if (datas.Count != 0)
                {
                    return;
                }

                codeTabsTag         = "TABS";
                codeTabs            = "    ";
                scriptStartTag      = $"{prefix}_SCRIPT_STARTS_HERE".Keyword();
                namepaceTag         = $"{prefix}_EXTENSION_NAMESPACE".Keyword();
                extensionClassTag   = $"{prefix}_EXTENSION_CLASS".Keyword();
                extensionPrefixTag  = $"{prefix}_EXTENSION_PREFIX".Keyword();
                codeUsingTag        = $"{prefix}_USING_NAMESPACE".Keyword();
                codeDataTag         = $"{prefix}_CODEGEN_DATA".Keyword();
                codeDataTabsTag     = $"{prefix}_CODEGEN_{codeTabsTag}".Keyword();
                codeTabsTag         = codeTabsTag.Keyword();
                codeBlockFormat     = $"{Glossary.Macros.prefix}_{FunctionKeyword.BLOCK.S()}_";

                AddData(FunctionKeyword.FILE_INFO);
                AddData(FunctionKeyword.USING);
                AddData(FunctionKeyword.PREFIX, codeData);
                AddData(FunctionKeyword.MAIN, codeData);
                AddData(FunctionKeyword.SUFFIX, codeData);
                AddData(FunctionKeyword.CLASS_INFO);
                AddData(FunctionKeyword.DEFAULT);
                AddData(FunctionKeyword.FUNC);
                AddData(VariableKeyword.NAMES);
                AddData(VariableKeyword.VARS);

                srcClass = ClassKeyword.SRC_CLASS.ToString().Keyword();
                dstClass = ClassKeyword.DST_CLASS.ToString().Keyword();

                names = new NumberedSymbol(VariableKeyword.NAMES);
                variables = new NumberedSymbol(VariableKeyword.VARS);
                functions = new NumberedSymbol(VariableKeyword.FUNC_RESULT);
            }

            ///-------------------------------------------------------------
            private void AddData(VariableKeyword variableKeyword)
            {
                datas.Add(variableKeyword.ToString(), $"{prefix}_{variableKeyword.S()}");
            }

            ///-------------------------------------------------------------
            private void AddData(FunctionKeyword functionKeyword)
            {
                datas.Add(functionKeyword.ToString(), $"{prefix}_{functionKeyword.S()}");
            }

            ///-------------------------------------------------------------
            private void AddData(FunctionKeyword functionKeyword, string additionalText)
            {
                datas.Add(functionKeyword.ToString(), $"{prefix}_{additionalText}_{functionKeyword.S()}");
            }

            ///-------------------------------------------------------------
            public void GetTags(SyntaxScriptAction syntaxer)
            {
                foreach (var name in Enum.GetNames(typeof(FunctionKeyword)))
                {
                    if (name == FunctionKeyword.FILE_INFO.ToString())
                    {
                        syntaxer.AddKeyword(this[FunctionKeyword.FILE_INFO]);
                    }
                    syntaxer.AddIdentifier(datas[name]);
                }

                syntaxer.AddIdentifier(srcClass.Keyword(false));
                syntaxer.AddIdentifier(dstClass.Keyword(false));

                var rules = ScriptActionRegistry.Actions;
                for (var r = 0; r < rules.Count; r++)
                {
                    var rule = rules[r];
                    syntaxer.AddKeyword(rule.CodeBlock);
                }

                for (var p = 0; p < 3; p++)
                {
                    var list = default(NumberedSymbol);
                    switch (p)
                    {
                        case 0:
                        {
                            list = names;
                            break;
                        }
                        case 1:
                        {
                            list = variables;
                            break;
                        }
                        case 2:
                        {
                            list = functions;
                            break;
                        }
                    }

                    for (var l = 0; l < list.Count; l++)
                    {
                        syntaxer.AddIdentifier(list[l].Original.Keyword(false));
                    }
                }
            }
            #endregion
        }
        #endregion
    }
}

