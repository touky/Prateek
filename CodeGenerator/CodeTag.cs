// -BEGIN_PRATEEK_COPYRIGHT-
//
//  Prateek, a library that is "bien pratique"
//  Header last update date: 20/03/2019
//
//  Copyright © 2017-2019 "Touky" <touky@prateek.top>
//
//  Prateek is free software. It comes without any warranty, to
//  the extent permitted by applicable law. You can redistribute it
//  and/or modify it under the terms of the Do What the Fuck You Want
//  to Public License, Version 2, as published by the WTFPL Task Force.
//  See http://www.wtfpl.net/ for more details.
//
// -END_PRATEEK_COPYRIGHT-

// -BEGIN_PRATEEK_CSHARP_NAMESPACE-
//
#region C# Prateek Namespaces
#if UNITY_EDITOR && !PRATEEK_DEBUG
#define PRATEEK_DEBUG
#endif //UNITY_EDITOR && !PRATEEK_DEBUG

#region System
using System;
using System.Collections;
using System.Collections.Generic;
#endregion System

#region Unity
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.Serialization;

#if UNITY_PROFILING
using UnityEngine.Profiling;
#endif //UNITY_PROFILING

using Unity.Jobs;
using Unity.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif //UNITY_EDITOR
#endregion Unity

#region Prateek
using Prateek;
using Prateek.Base;
using Prateek.Extensions;
using Prateek.Helpers;
using Prateek.Attributes;

#if UNITY_EDITOR
using Prateek.ScriptTemplating;
#endif //UNITY_EDITOR

#if PRATEEK_DEBUGS
using Prateek.Debug;
#endif //PRATEEK_DEBUG
#endregion Prateek

#endregion C# Prateek Namespaces
//
// -END_PRATEEK_CSHARP_NAMESPACE-

//-----------------------------------------------------------------------------
#region File namespaces
using System.IO;
using Prateek.IO;
using System.Text.RegularExpressions;
#endregion File namespaces

//-----------------------------------------------------------------------------
namespace Prateek.ScriptTemplating
{
    //---------------------------------------------------------------------
    public static partial class Code
    {
        //---------------------------------------------------------------------
        public static class Tag
        {
            //Default datas ---------------------------------------------------
            public const string sourceExtension = "prtk";
            public const string destinationExtension = "cs";

            public const string fileHeader = @"#PRATEEK_COPYRIGHT#

#PRATEEK_CSHARP_NAMESPACE#
";
            public const string fileCode = @"
//-----------------------------------------------------------------------------
namespace PRATEEK_EXTENSION_NAMESPACE
{
    //-------------------------------------------------------------------------
    public static partial class #PRATEEK_EXTENSION_STATIC_CLASS#
    {
        #PRATEEK_CODEGEN_DATA#
    }
}
";

            //-----------------------------------------------------------------
            public struct SwapInfo
            {
                //-------------------------------------------------------------
                private string original;
                private string replacement;

                //-------------------------------------------------------------
                public string Original { get { return original; } }
                public string Replacement { get { return replacement; } }

                //-------------------------------------------------------------
                public SwapInfo(string original)
                {
                    this.original = original;
                    this.replacement = string.Empty;
                }

                //-------------------------------------------------------------
                public static implicit operator SwapInfo(string original)
                {
                    return new SwapInfo(original);
                }

                //-------------------------------------------------------------
                public static SwapInfo operator +(SwapInfo info, string other)
                {
                    return new SwapInfo() { original = info.original, replacement = other };
                }

                //-------------------------------------------------------------
                public string Apply(string text)
                {
                    return text.Replace(original, replacement);
                }
            }

            //-----------------------------------------------------------------
            public struct Keyword
            {
                public enum Usage
                {
                    Match,
                    Forbidden,
                    Ignore,

                    MAX
                }

                public string keyword;
                public Usage usage;
                public int minArgCount;
                public int maxArgCount;
                public bool needOpenScope;
                public bool needScopeData;

                public Keyword(string keyword, bool doesMatch)
                {
                    this.keyword = keyword;
                    usage = doesMatch ? Usage.Match : Usage.Forbidden;
                    minArgCount = -1;
                    maxArgCount = -1;
                    needOpenScope = false;
                    needScopeData = false;
                }

                public bool NoArgNeeded { get { return minArgCount <= 0 && maxArgCount <= 0; } }
                public bool CheckArgCount(int count)
                {
                    if (minArgCount < 0)
                        return true;

                    if (count < minArgCount)
                        return false;

                    if (maxArgCount >= 0)
                        return count <= maxArgCount;
                    return true;
                }
            }

            //-----------------------------------------------------------------
            public static class Macro
            {
                public static string codeGenNSpc = "PRATEEK_EXTENSION_NAMESPACE";
                public static string codeGenExtn = "PRATEEK_EXTENSION_STATIC_CLASS";
                public static string codeGenData = "PRATEEK_CODEGEN_DATA";
                public static string codeGenTabs = "PRATEEK_CODEGEN_TABS";
                public static string extractFuncRegex = @"\b[^()]+\((.*)\)$";
                public static string extractArgsRegex = @"([^,]+\(.+?\))|([^,]+)";

                public static string macroMatch0 =
                //@"(^\s*\w+\s*[^\(])";
                //@"([^\(]+\s*\w+\s*[^,]+)";
                //@"(\b\w+\b)+";
                @"([a-zA-Z0-9]+)\((([a-zA-Z0-9]+)(,)(([a-zA-Z0-9])+)*?)*?\)";

                public static string macroMatch = @"^\s*({0})\(.*\)\s*$";
                public static string argsCountMatch = "[^(]*\\(([^)]*)\\)";
                public static string prefix = "PRATEEK_CODEGEN";
                public static string codeData = "CODE";

                //-------------------------------------------------------------
                public enum Content
                {
                    FILE_INFO,  //PRATEEK_CODEGEN_FILE_INFO(MyFile, Extension)
                    BLOCK,      //PRATEEK_CODEGEN_BLOCK_[OPERATION](StaticClass)
                    PREFIX,     //PRATEEK_CODEGEN_CODE_PREFIX
                    MAIN,       //PRATEEK_CODEGEN_CODE_MAIN
                    SUFFIX,     //PRATEEK_CODEGEN_CODE_SUFFIX
                    CLASS,      //PRATEEK_CODEGEN_CLASS(*****)
                    TYPE,       //PRATEEK_CODEGEN_TYPE(*****)

                    MAX
                }

                //-------------------------------------------------------------
                public static string srcClass = "SRC_CLASS";
                public static string dstClass = "DST_CLASS";

                //-------------------------------------------------------------
                public enum Code
                {
                    CALL,    //[OPERATION]_CALL
                    ARGS,    //[OPERATION]_ARGS
                    VARS,    //[OPERATION]_VARS

                    MAX
                }

                //-------------------------------------------------------------
                private static List<string> data = new List<string>();

                //-------------------------------------------------------------
                public static string FileInfo /***/ { get { return data[0]; } }
                public static string CodePartPrefix { get { return data[1]; } }
                public static string CodePartMain { get { return data[2]; } }
                public static string CodePartSuffix { get { return data[3]; } }
                public static string OperationClass { get { return data[4]; } }
                public static string TypeInfo /***/ { get { return data[5]; } }

                //-------------------------------------------------------------
                public static string To(Content value) { return Enum.GetNames(typeof(Content))[(int)value]; }
                public static string To(Code value) { return Enum.GetNames(typeof(Code))[(int)value]; }

                //-------------------------------------------------------------
                public static void Init()
                {
                    data.Add(string.Format("{0}_{1}", prefix, To(Content.FILE_INFO)));
                    data.Add(string.Format("{0}_{1}_{2}", prefix, codeData, To(Content.PREFIX)));
                    data.Add(string.Format("{0}_{1}_{2}", prefix, codeData, To(Content.MAIN)));
                    data.Add(string.Format("{0}_{1}_{2}", prefix, codeData, To(Content.SUFFIX)));
                    data.Add(string.Format("{0}_{1}", prefix, To(Content.CLASS)));
                    data.Add(string.Format("{0}_{1}", prefix, To(Content.TYPE)));
                }
            }

            //Code generation data
            public static class Code
            {
                public const string argVarSeparator = ", ";
                public const string callN = "n";
                public const string argsV = "v";
                public const string argsN = ", {0} n_{1} = {2}";
                public const string varsN = "n_{0}";
                public const string varsV = "v.{0}";
            }
        }
    }
}