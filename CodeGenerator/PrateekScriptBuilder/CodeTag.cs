// -BEGIN_PRATEEK_COPYRIGHT-
//
//  Prateek, a library that is "bien pratique"
//  Header last update date: 22/03/2020
//
//  Copyright � 2017-2020 "Touky" <touky@prateek.top>
//
//  Prateek is free software. It comes without any warranty, to
//  the extent permitted by applicable law. You can redistribute it
//  and/or modify it under the terms of the Do What the Fuck You Want
//  to Public License, Version 2, as published by the WTFPL Task Force.
//  See http://www.wtfpl.net/ for more details.
//
// -END_PRATEEK_COPYRIGHT-

// -BEGIN_PRATEEK_CSHARP_IFDEF-
//-----------------------------------------------------------------------------
#region Prateek Ifdefs

//Auto activate some of the prateek defines
#if UNITY_EDITOR

//Auto activate debug
#if !PRATEEK_DEBUG
#define PRATEEK_DEBUG
#endif //!PRATEEK_DEBUG

#endif //UNITY_EDITOR && !PRATEEK_DEBUG

#endregion Prateek Ifdefs
// -END_PRATEEK_CSHARP_IFDEF-

//-----------------------------------------------------------------------------
namespace Prateek.CodeGenerator.PrateekScriptBuilder
{
    using System;
    using System.Collections.Generic;
    using Prateek.Core.Code.Helpers;
    using Prateek.Helpers;
    using UnityEditor;

    //-------------------------------------------------------------------------
    public partial class PrateekScriptBuilder
    {
        //---------------------------------------------------------------------
#if UNITY_EDITOR
        [InitializeOnLoad]
        class TagLoader
        {
            static TagLoader()
            {
                if (EditorApplication.isPlayingOrWillChangePlaymode)
                    return;

                Tag.Macro.Init();
            }
        }
#endif //UNITY_EDITOR

        //---------------------------------------------------------------------
        public static class Tag
        {
            //Default datas ---------------------------------------------------
            public const string importExtension = "prtk";
            public const string exportExtension = "cs";

            //-----------------------------------------------------------------
            public struct NumberedVars
            {
                //-------------------------------------------------------------
                private List<string> datas;

                //-------------------------------------------------------------
                public int Count { get { return datas.Count; } }
                public CodeBuilder.Utils.SwapInfo this[int i]
                {
                    get
                    {
                        if (i < 0 || i >= datas.Count)
                            return string.Empty;
                        return datas[i];
                    }
                }

                //-------------------------------------------------------------
                public NumberedVars(Tag.Macro.VarName root)
                {
                    datas = new List<string>();
                    for (int i = 0; i < 10; i++)
                    {
                        datas.Add(string.Format("{0}_{1}", root, i).Keyword());
                    }
                }

                //-------------------------------------------------------------
                public int GetCount(string content)
                {
                    if (datas == null || content == null)
                        return 0;

                    var count = 0;
                    for (int c = 0; c < Count; c++)
                    {
                        count += content.Contains(datas[c]) ? 1 : 0;
                    }
                    return count;
                }
            }

            //-----------------------------------------------------------------
            public static class Macro
            {
                //-------------------------------------------------------------
                public static string codeGenStart = "PRATEEK_SCRIPT_STARTS_HERE";
                public static string codeGenNSpc = "PRATEEK_EXTENSION_NAMESPACE";
                public static string codeGenExtn = "PRATEEK_EXTENSION_CLASS";
                public static string codeGenPrfx = "PRATEEK_EXTENSION_PREFIX";
                public static string codeGenData = "PRATEEK_CODEGEN_DATA";
                public static string codeGenTabs = "PRATEEK_CODEGEN_TABS";

                public static string prefix = "PRATEEK";
                public static string codeData = "CODE";

                //-------------------------------------------------------------
                public enum FuncName
                {
                    FILE_INFO,  //PRATEEK_CODEGEN_FILE_INFO(MyFile, Extension)
                    BLOCK,      //PRATEEK_CODEGEN_BLOCK_[OPERATION](StaticClass)
                    PREFIX,     //PRATEEK_CODEGEN_CODE_PREFIX
                    MAIN,       //PRATEEK_CODEGEN_CODE_MAIN
                    SUFFIX,     //PRATEEK_CODEGEN_CODE_SUFFIX
                    CLASS_INFO, //PRATEEK_CODEGEN_CLASS_INFO(*****)
                    DEFAULT,    //PRATEEK_CODEGEN_DEFAULT(*****)
                    FUNC,       //PRATEEK_CODEGEN_FUNC(*****) { }

                    MAX
                }

                //-------------------------------------------------------------
                public enum ClassName
                {
                    SRC_CLASS,
                    DST_CLASS,

                    MAX
                }

                //-------------------------------------------------------------
                public enum VarName
                {
                    NAMES,      //NAMES_[n]
                    VARS,       //VARS_[n]
                    FUNC_RESULT,//FUNC_RESULT_[n]

                    MAX
                }

                //-------------------------------------------------------------
                private static NumberedVars names;
                private static NumberedVars vars;
                private static NumberedVars funcs;

                //-------------------------------------------------------------
                public static string srcClass = "#SRC_CLASS#";
                public static string dstClass = "#DST_CLASS#";

                //-------------------------------------------------------------
                private static List<string> data = new List<string>();

                //-------------------------------------------------------------
                public static string FileInfo       { get { return data[0]; } }
                public static string CodePartPrefix { get { return data[1]; } }
                public static string CodePartMain   { get { return data[2]; } }
                public static string CodePartSuffix { get { return data[3]; } }
                public static string ClassInfo      { get { return data[4]; } }
                public static string DefaultInfo    { get { return data[5]; } }
                public static string Func           { get { return data[6]; } }
                public static string ClassNames     { get { return data[7]; } }
                public static string ClassVars      { get { return data[8]; } }

                //-------------------------------------------------------------
                public static NumberedVars Names { get { return names; } }
                public static NumberedVars Funcs { get { return funcs; } }
                public static NumberedVars Vars { get { return vars; } }

                //-------------------------------------------------------------
                public static string To(FuncName value) { return Enum.GetNames(typeof(FuncName))[(int)value]; }
                public static string To(VarName value) { return Enum.GetNames(typeof(VarName))[(int)value]; }

                //-------------------------------------------------------------
                public static void Init()
                {
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

                    names = new NumberedVars(VarName.NAMES);
                    vars = new NumberedVars(VarName.VARS);
                    funcs = new NumberedVars(VarName.FUNC_RESULT);
                }

                //-------------------------------------------------------------
                public static void GetTags(SyntaxCodeRule syntaxer)
                {
                    syntaxer.AddKeyword(FileInfo);
                    for (int d = 1; d < data.Count; d++)
                    {
                        syntaxer.AddIdentifier(data[d]);
                    }

                    syntaxer.AddIdentifier(srcClass.Keyword(false));
                    syntaxer.AddIdentifier(dstClass.Keyword(false));

                    var rules = PrateekScriptBuilder.Database.CodeRules;
                    for (int r = 0; r < rules.Count; r++)
                    {
                        var rule = rules[r];
                        syntaxer.AddKeyword(rule.CodeBlock);
                    }

                    for (int p = 0; p < 3; p++)
                    {
                        var list = default(NumberedVars);
                        switch (p)
                        {
                            case 0: { list = names; break; }
                            case 1: { list = vars; break; }
                            case 2: { list = funcs; break; }
                        }

                        for (int l = 0; l < list.Count; l++)
                        {
                            syntaxer.AddIdentifier(list[l].Original.Keyword(false));
                        }
                    }
                }
            }

            //Code generation data -------------------------------------
            public static class Code
            {
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
            }
        }
    }
}