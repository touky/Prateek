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
        public static class Macro
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
            ///-------------------------------------------------------------
            public enum FuncName
            {
                FILE_INFO, //PRATEEK_CODEGEN_FILE_INFO(MyFile, Extension)
                BLOCK, //PRATEEK_CODEGEN_BLOCK_[OPERATION](StaticClass)
                PREFIX, //PRATEEK_CODEGEN_CODE_PREFIX
                MAIN, //PRATEEK_CODEGEN_CODE_MAIN
                SUFFIX, //PRATEEK_CODEGEN_CODE_SUFFIX
                CLASS_INFO, //PRATEEK_CODEGEN_CLASS_INFO(*****)
                DEFAULT, //PRATEEK_CODEGEN_DEFAULT(*****)
                FUNC, //PRATEEK_CODEGEN_FUNC(*****) { }

                MAX
            }
            #endregion

            #region VarName enum
            ///-------------------------------------------------------------
            public enum VarName
            {
                NAMES, //NAMES_[n]
                VARS, //VARS_[n]
                FUNC_RESULT, //FUNC_RESULT_[n]

                MAX
            }
            #endregion

            #region Static and Constants
            ///-------------------------------------------------------------
            public static string codeGenStart = "PRATEEK_SCRIPT_STARTS_HERE";
            public static string codeGenNSpc = "PRATEEK_EXTENSION_NAMESPACE";
            public static string codeGenExtn = "PRATEEK_EXTENSION_CLASS";
            public static string codeGenPrfx = "PRATEEK_EXTENSION_PREFIX";
            public static string codeGenData = "PRATEEK_CODEGEN_DATA";
            public static string codeGenTabs = "PRATEEK_CODEGEN_TABS";

            public static string prefix = "PRATEEK";
            public static string codeData = "CODE";

            ///-------------------------------------------------------------
            private static NumberedSymbol names;
            private static NumberedSymbol variables;
            private static NumberedSymbol functions;

            ///-------------------------------------------------------------
            public static string srcClass = "#SRC_CLASS#";
            public static string dstClass = "#DST_CLASS#";

            ///-------------------------------------------------------------
            private static List<string> data = new List<string>();
            #endregion

            #region Properties
            ///-------------------------------------------------------------
            public static string FileInfo
            {
                get { return data[0]; }
            }

            public static string CodePartPrefix
            {
                get { return data[1]; }
            }

            public static string CodePartMain
            {
                get { return data[2]; }
            }

            public static string CodePartSuffix
            {
                get { return data[3]; }
            }

            public static string ClassInfo
            {
                get { return data[4]; }
            }

            public static string DefaultInfo
            {
                get { return data[5]; }
            }

            public static string Func
            {
                get { return data[6]; }
            }

            public static string ClassNames
            {
                get { return data[7]; }
            }

            public static string ClassVars
            {
                get { return data[8]; }
            }

            ///-------------------------------------------------------------
            public static NumberedSymbol Names
            {
                get { return names; }
            }

            public static NumberedSymbol Functions
            {
                get { return functions; }
            }

            public static NumberedSymbol Variables
            {
                get { return variables; }
            }
            #endregion

            #region Class Methods
            ///-------------------------------------------------------------
            public static string To(FuncName value)
            {
                return Enum.GetNames(typeof(FuncName))[(int) value];
            }

            public static string To(VarName value)
            {
                return Enum.GetNames(typeof(VarName))[(int) value];
            }

            ///-------------------------------------------------------------
            public static void Init()
            {
                if (data.Count != 0)
                {
                    return;
                }

                data.Add(string.Format("{0}_{1}", prefix, To(FuncName.FILE_INFO)));
                data.Add(string.Format("{0}_{1}_{2}", prefix, codeData, To(FuncName.PREFIX)));
                data.Add(string.Format("{0}_{1}_{2}", prefix, codeData, To(FuncName.MAIN)));
                data.Add(string.Format("{0}_{1}_{2}", prefix, codeData, To(FuncName.SUFFIX)));
                data.Add(string.Format("{0}_{1}", prefix, To(FuncName.CLASS_INFO)));
                data.Add(string.Format("{0}_{1}", prefix, To(FuncName.DEFAULT)));
                data.Add(string.Format("{0}_{1}", prefix, To(FuncName.FUNC)));
                data.Add(string.Format("{0}_{1}", prefix, To(VarName.NAMES)));
                data.Add(string.Format("{0}_{1}", prefix, To(VarName.VARS)));

                srcClass = ClassName.SRC_CLASS.ToString().Keyword();
                dstClass = ClassName.DST_CLASS.ToString().Keyword();

                names = new NumberedSymbol(VarName.NAMES);
                variables = new NumberedSymbol(VarName.VARS);
                functions = new NumberedSymbol(VarName.FUNC_RESULT);
            }

            ///-------------------------------------------------------------
            public static void GetTags(SyntaxScriptAction syntaxer)
            {
                syntaxer.AddKeyword(FileInfo);
                for (var d = 1; d < data.Count; d++)
                {
                    syntaxer.AddIdentifier(data[d]);
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
