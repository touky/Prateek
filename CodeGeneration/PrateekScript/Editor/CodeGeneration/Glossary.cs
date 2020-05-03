namespace Assets.Prateek.CodeGenerator.Code.PrateekScript.CodeGeneration
{
    using System;
    using System.Collections.Generic;
    using Assets.Prateek.CodeGenerator.Code.PrateekScript.ScriptActions;
    using global::Prateek.Core.Code.Helpers;

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
            #region ClassName enum
            ///-------------------------------------------------------------
            public enum ClassName
            {
                SRC_CLASS,
                DST_CLASS,

                MAX
            }
            #endregion

            #region FuncName enum
            #endregion

            #region VarName enum
            #endregion

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
            public string this[FuncName funcName]
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
            public string this[VarName funcName]
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
                codeBlockFormat     = $"{Glossary.Macros.prefix}_{Glossary.FuncName.BLOCK.To()}_";

                AddData(FuncName.FILE_INFO);
                AddData(FuncName.USING);
                AddData(FuncName.PREFIX, codeData);
                AddData(FuncName.MAIN, codeData);
                AddData(FuncName.SUFFIX, codeData);
                AddData(FuncName.CLASS_INFO);
                AddData(FuncName.DEFAULT);
                AddData(FuncName.FUNC);
                AddData(VarName.NAMES);
                AddData(VarName.VARS);

                srcClass = ClassName.SRC_CLASS.ToString().Keyword();
                dstClass = ClassName.DST_CLASS.ToString().Keyword();

                names = new NumberedSymbol(VarName.NAMES);
                variables = new NumberedSymbol(VarName.VARS);
                functions = new NumberedSymbol(VarName.FUNC_RESULT);
            }

            ///-------------------------------------------------------------
            private void AddData(VarName varName)
            {
                datas.Add(varName.ToString(), $"{prefix}_{varName.To()}");
            }

            ///-------------------------------------------------------------
            private void AddData(FuncName funcName)
            {
                datas.Add(funcName.ToString(), $"{prefix}_{funcName.To()}");
            }

            ///-------------------------------------------------------------
            private void AddData(FuncName funcName, string additionalText)
            {
                datas.Add(funcName.ToString(), $"{prefix}_{additionalText}_{funcName.To()}");
            }

            ///-------------------------------------------------------------
            public void GetTags(SyntaxScriptAction syntaxer)
            {
                foreach (var name in Enum.GetNames(typeof(FuncName)))
                {
                    if (name == FuncName.FILE_INFO.ToString())
                    {
                        syntaxer.AddKeyword(this[FuncName.FILE_INFO]);
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

        ///-------------------------------------------------------------
        public enum FuncName
        {
            FILE_INFO,
            USING,
            BLOCK,
            PREFIX,
            MAIN,
            SUFFIX,
            CLASS_INFO,
            DEFAULT,
            FUNC,

            MAX
        }

        ///-------------------------------------------------------------
        public enum VarName
        {
            NAMES, //NAMES_[n]
            VARS, //VARS_[n]
            FUNC_RESULT, //FUNC_RESULT_[n]

            MAX
        }
    }

    public static class FuncNameExtensions
    {
        public static string To(this Glossary.FuncName value)
        {
            return Enum.GetNames(typeof(Glossary.FuncName))[(int) value];
        }
    }

    public static class VarNameExtensions
    {
        public static string To(this Glossary.VarName value)
        {
            return Enum.GetNames(typeof(Glossary.VarName))[(int) value];
        }
    }
}

