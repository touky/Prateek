// -BEGIN_PRATEEK_COPYRIGHT-
//
//  Prateek, a library that is "bien pratique"
//  Header last update date: 24/03/2019
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
using Unity.Jobs;
using Unity.Collections;

#region Engine
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.Serialization;

#if UNITY_PROFILING
using UnityEngine.Profiling;
#endif //UNITY_PROFILING
#endregion Engine

#region Editor
#if UNITY_EDITOR
using UnityEditor;
#endif //UNITY_EDITOR
#endregion Editor
#endregion Unity

#region Prateek
using Prateek;
using Prateek.Base;
using Prateek.Extensions;
using Prateek.Helpers;
using Prateek.Attributes;

#region Using static
using static Prateek.ShaderTo.CSharp;
#endregion Using static

#region Editor
#if UNITY_EDITOR
using Prateek.CodeGeneration;
#endif //UNITY_EDITOR
#endregion Editor

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
namespace Prateek.CodeGeneration
{
    //-------------------------------------------------------------------------
    public partial class CodeBuilder
    {
        //-------------------------------------------------------------------------
        [InitializeOnLoad]
        class TagLoader
        {
            static TagLoader()
            {
                CodeBuilder.Tag.Macro.Init();
            }
        }

        //---------------------------------------------------------------------
        public static class Tag
        {
            //Default datas ---------------------------------------------------
            public const string importExtension = "prtk";
            public const string exportExtension = "cs";

            //-----------------------------------------------------------------
            public static class Macro
            {
                //-------------------------------------------------------------
                public static string codeGenStart = "PRATEEK_SCRIPT_STARTS_HERE";
                public static string codeGenNSpc = "PRATEEK_EXTENSION_NAMESPACE";
                public static string codeGenExtn = "PRATEEK_EXTENSION_STATIC_CLASS";
                public static string codeGenData = "PRATEEK_CODEGEN_DATA";
                public static string codeGenTabs = "PRATEEK_CODEGEN_TABS";

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
                    DEFAULT,    //PRATEEK_CODEGEN_DEFAULT(*****)
                    FUNC,       //PRATEEK_CODEGEN_FUNC(*****) { }

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
                public static string FileInfo       { get { return data[0]; } }
                public static string CodePartPrefix { get { return data[1]; } }
                public static string CodePartMain   { get { return data[2]; } }
                public static string CodePartSuffix { get { return data[3]; } }
                public static string OperationClass { get { return data[4]; } }
                public static string DefaultInfo    { get { return data[5]; } }
                public static string Func           { get { return data[6]; } }

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
                    data.Add(string.Format("{0}_{1}", prefix, To(Content.DEFAULT)));
                    data.Add(string.Format("{0}_{1}", prefix, To(Content.FUNC)));
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