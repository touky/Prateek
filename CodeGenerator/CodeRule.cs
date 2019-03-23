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
    //-------------------------------------------------------------------------
    public partial class TemplateReplacement
    {
        //---------------------------------------------------------------------
        public abstract class CodeRule : TemplateBase
        {
            //-----------------------------------------------------------------
            public struct Variant
            {
                //-----------------------------------------------------------------
                public string call;
                public string args;
                public string vars;

                //-----------------------------------------------------------------
                public string Call { get { return call; } set { call += value; } }
                public string Args { get { return args; } set { Set(ref args, value); } }
                public string Vars { get { return vars; } set { Set(ref vars, value); } }

                //-----------------------------------------------------------------
                public Variant(string value)
                {
                    call = value;
                    args = value;
                    vars = value;
                }

                //-----------------------------------------------------------------
                private void Set(ref string dst, string value)
                {
                    if (dst != null && dst.Length > 0)
                        dst += Code.Tag.Code.argVarSeparator;
                    dst += value;
                }
            }

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
            protected CodeRule(string extension) : base(extension)
            {
                Init();
            }

            //-----------------------------------------------------------------
            public override void Commit()
            {
                TemplateReplacement.Add(this);
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
            public bool TreatData(Code.File codeFile, Code.Tag.KeyRule keyRule, List<string> args, string data)
            {
                var activeData = codeFile.ActiveData;
                if (activeData == null)
                {
                    activeData = codeFile.NewData(this);
                }

                if (activeData.activeRule == null || activeData.activeRule != this)
                    return false;

                return DoTreatData(activeData, keyRule, args, data);
            }

            //-----------------------------------------------------------------
            #region Utils
            protected void AddCode(string code, Code.File.Data data, Code.Tag.SwapInfo swapSrc)
            {
                AddCode(code, data, swapSrc, new Code.Tag.SwapInfo());
            }

            //-----------------------------------------------------------------
            protected void AddCode(string code, Code.File.Data data, Code.Tag.SwapInfo swapSrc, Code.Tag.SwapInfo swapDst)
            {
                code = swapSrc.Apply(code);
                code = swapDst.Apply(code);
                data.codeGenerated += code;
            }
            #endregion Utils

            //-----------------------------------------------------------------
            #region CodeRule overridable
            public virtual Code.Tag.KeyRule GetKeyRule(string keyword, int codeDepth)
            {
                if (keyword == CodeBlock)
                {
                    return new Code.Tag.KeyRule(keyword, codeDepth == 1) { minArgCount = 2, maxArgCount = 2, needOpenScope = true };
                }
                else if (keyword == Code.Tag.Macro.OperationClass)
                {
                    return new Code.Tag.KeyRule(keyword, codeDepth == 2) { minArgCount = 1 };
                }
                else if (keyword == Code.Tag.Macro.DefaultInfo)
                {
                    return new Code.Tag.KeyRule(keyword, codeDepth == 2) { minArgCount = 2, maxArgCount = 2 };
                }
                else if (keyword == Code.Tag.Macro.CodePartPrefix || keyword == Code.Tag.Macro.CodePartMain || keyword == Code.Tag.Macro.CodePartSuffix)
                {
                    return new Code.Tag.KeyRule(keyword, codeDepth == 2) { minArgCount = 0, maxArgCount = 0, needOpenScope = true, needScopeData = true };
                }

                return new Code.Tag.KeyRule() { usage = Code.Tag.KeyRule.Usage.Ignore };
            }

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
            protected virtual bool DoTreatData(Code.File.Data activeData, Code.Tag.KeyRule keyRule, List<string> args, string data)
            {
                if (keyRule.key == CodeBlock)
                {
                    activeData.blockNamespace = args[0];
                    activeData.blockClassName = args[1];
                }
                else if (keyRule.key == Code.Tag.Macro.OperationClass)
                {
                    activeData.classInfos.Add(new Code.File.Data.ClassInfo()
                    {
                        names = args.GetRange(0, 1),
                        variables = args.GetRange(1, args.Count - 1)
                    });
                }
                else if (keyRule.key == Code.Tag.Macro.DefaultInfo)
                {
                    activeData.classDefaultType = args[0];
                    activeData.classDefaultValue = args[1];
                }
                else if (keyRule.key == Code.Tag.Macro.CodePartPrefix)
                {
                    activeData.codePrefix = data;
                }
                else if (keyRule.key == Code.Tag.Macro.CodePartMain)
                {
                    activeData.codeMain = data;
                }
                else if (keyRule.key == Code.Tag.Macro.CodePartSuffix)
                {
                    activeData.codePostfix = data;
                }
                return true;
            }
            #endregion CodeRule override

            //-----------------------------------------------------------------
            #region CodeRule abstract
            public abstract void Generate(Code.File.Data data);
            #endregion CodeRule abstract
        }
    }

    //-------------------------------------------------------------------------
    [InitializeOnLoad]
    class PrateekScriptTemplate : TemplateReplacement
    {
        static PrateekScriptTemplate()
        {
            NewScript(Code.Tag.importExtension, Code.Tag.exportExtension)
                .SetTemplateFile(String.Empty)
                .SetContent(@"#PRATEEK_COPYRIGHT#

#PRATEEK_CSHARP_NAMESPACE#
#PRATEEK_SCRIPT_STARTS_HERE#
//-----------------------------------------------------------------------------
namespace #PRATEEK_EXTENSION_NAMESPACE#
{
    //-------------------------------------------------------------------------
    public static partial class #PRATEEK_EXTENSION_STATIC_CLASS#
    {
        #PRATEEK_CODEGEN_DATA#
    }
}
").Commit();
        }
    }
}