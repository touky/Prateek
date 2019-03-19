// -BEGIN_PRATEEK_COPYRIGHT-
//
//  Prateek, a library that is "bien pratique"
//  Header last update date: 18/03/19
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
    public abstract class CodeSettings
    {
        //-----------------------------------------------------------------
        public abstract string ScopeTag { get; }

        //-----------------------------------------------------------------
        private List<string> data = new List<string>();

        //-----------------------------------------------------------------
        public string CodeBlock { get { return data[0]; } }
        private string DataCall { get { return data[1]; } }
        private string DataArgs { get { return data[2]; } }
        private string DataVars { get { return data[3]; } }

        //-----------------------------------------------------------------
        public Code.Tag.SwapInfo ClassDst { get { return Code.Tag.Macro.dstClass; } }
        public Code.Tag.SwapInfo ClassSrc { get { return Code.Tag.Macro.srcClass; } }
        public Code.Tag.SwapInfo CodeCall { get { return DataCall; } }
        public Code.Tag.SwapInfo CodeArgs { get { return DataArgs; } }
        public Code.Tag.SwapInfo CodeVars { get { return DataVars; } }

        //-----------------------------------------------------------------
        protected CodeSettings()
        {
            Init();
        }

        //-----------------------------------------------------------------
        private void Init()
        {
            data.Add(string.Format("{0}_{1}_{2}", Code.Tag.Macro.prefix, Code.Tag.Macro.To(Code.Tag.Macro.Content.BLOCK), ScopeTag));
            data.Add(string.Format("{0}_{1}", ScopeTag, Code.Tag.Macro.To(Code.Tag.Macro.Code.CALL)));
            data.Add(string.Format("{0}_{1}", ScopeTag, Code.Tag.Macro.To(Code.Tag.Macro.Code.ARGS)));
            data.Add(string.Format("{0}_{1}", ScopeTag, Code.Tag.Macro.To(Code.Tag.Macro.Code.VARS)));
        }

        //-----------------------------------------------------------------
        protected void Submit()
        {
            CodeGenerator.Add(this);
        }

        //-----------------------------------------------------------------
        public virtual Code.Tag.Keyword GetSetup(string keyword, int codeDepth)
        {
            if (keyword == CodeBlock)
            {
                return new Code.Tag.Keyword(keyword, codeDepth == 1) { minArgCount = 1, maxArgCount = 1, needOpenScope = true };
            }
            else if (keyword == Code.Tag.Macro.OperationClass)
            {
                return new Code.Tag.Keyword(keyword, codeDepth == 2) { minArgCount = 1 };
            }
            else if (keyword == Code.Tag.Macro.TypeInfo)
            {
                return new Code.Tag.Keyword(keyword, codeDepth == 2) { minArgCount = 2, maxArgCount = 2 };
            }
            else if (keyword == Code.Tag.Macro.CodePartPrefix || keyword == Code.Tag.Macro.CodePartMain || keyword == Code.Tag.Macro.CodePartSuffix)
            {
                return new Code.Tag.Keyword(keyword, codeDepth == 2) { minArgCount = 0, maxArgCount = 0, needOpenScope = true, needScopeData = true };
            }

            return new Code.Tag.Keyword() { usage = Code.Tag.Keyword.Usage.Ignore };
        }

        //-----------------------------------------------------------------
        public abstract bool TreatData(Code.File codeFile, Code.Tag.Keyword keyword, List<string> args, string data);

        //-----------------------------------------------------------------
        public virtual bool CloseScope(Code.File codeFile, string scope)
        {
            if (scope == CodeBlock)
            {
                codeFile.Submit();
                return true;
            }
            else if (scope == Code.Tag.Macro.CodePartMain || scope == Code.Tag.Macro.CodePartPrefix || scope == Code.Tag.Macro.CodePartSuffix)
            {
                return true;
            }
            return false;
        }

        //-----------------------------------------------------------------
        public abstract void Generate(Code.File.Data data);



        //-----------------------------------------------------------------
        public class CodeClasses
        {
            public string className;
            public string[] callComponents;
        }

        //-----------------------------------------------------------------
        public List<CodeClasses> codeClasses = new List<CodeClasses>();

        public string codePrefix;
        public string codeSource;
        public string codePostfix;

        public string codeCall;
        public string codeArgs;
        public string codeType;
        public string codeDefault;
        public string codeVars;

        //-----------------------------------------------------------------
        public virtual bool Load(string line)
        {
            return false;
        }

        //-----------------------------------------------------------------
        public virtual string ReplaceAdditional(string code)
        {
            return code;
        }

        ////-----------------------------------------------------------------
        //public virtual void GetSlottedCode(CodeData srcCode, CodeClasses srcDef)
        //{
        //}
    }
}
