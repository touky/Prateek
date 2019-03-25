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
    [InitializeOnLoad]
    class PrateekScriptLoader : ScriptTemplate
    {
        static PrateekScriptLoader()
        {
            NewScript(CodeBuilder.Tag.importExtension, CodeBuilder.Tag.exportExtension)
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

    //-------------------------------------------------------------------------
    public partial class PrateekScriptBuilder
    {
        //---------------------------------------------------------------------
        public abstract class CodeRule : ScriptTemplate.BaseTemplate
    {
            //-----------------------------------------------------------------
            public struct Variant
            {
                //-------------------------------------------------------------
                private string call;
                private string args;
                private string vars;

                //-------------------------------------------------------------
                public string Call { get { return call; } set { call += value; } }
                public string Args { get { return args; } set { Set(ref args, value); } }
                public string Vars { get { return vars; } set { Set(ref vars, value); } }

                //-------------------------------------------------------------
                public Variant(string value)
                {
                    call = value;
                    args = string.Empty;
                    vars = string.Empty;
                }

                //-------------------------------------------------------------
                private void Set(ref string dst, string value)
                {
                    if (dst != null && dst.Length > 0)
                        dst += Tag.Code.argVarSeparator;
                    dst += value;
                }
            }

            //-----------------------------------------------------------------
            public enum GenerationMode
            {
                ForeachSrc,
                ForeachSrcXDest,

                MAX
            }

            //-----------------------------------------------------------------
            public abstract string ScopeTag { get; }
            public abstract GenerationMode GenMode { get; }
            public virtual bool GenerateDefault { get { return false; } }

            //-----------------------------------------------------------------
            private List<string> data = new List<string>();
            private List<string> vars = new List<string>();

            //-----------------------------------------------------------------
            public string CodeBlock { get { return data[0]; } }
            private string DataCall { get { return data[1]; } }
            private string DataArgs { get { return data[2]; } }
            private string DataVars { get { return data[3]; } }

            //-----------------------------------------------------------------
            public Utils.SwapInfo ClassDst { get { return Tag.Macro.dstClass; } }
            public Utils.SwapInfo ClassSrc { get { return Tag.Macro.srcClass; } }
            public Utils.SwapInfo CodeCall { get { return DataCall; } }
            public Utils.SwapInfo CodeArgs { get { return DataArgs; } }
            public Utils.SwapInfo CodeVars { get { return DataVars; } }
            public Utils.SwapInfo this[int i] { get { if (i < 0 || i > 9) return string.Empty; return vars[i]; } }
            public int VarCount { get { return vars.Count; } }

            //-----------------------------------------------------------------
            protected CodeRule(string extension) : base(extension)
            {
                Init();
            }

            //-----------------------------------------------------------------
            public override void Commit()
            {
                Database.Add(this);
            }

            //-----------------------------------------------------------------
            private void Init()
            {
                data.Add(string.Format("{0}_{1}_{2}", Tag.Macro.prefix, Tag.Macro.To(Tag.Macro.Content.BLOCK), ScopeTag));
                data.Add(string.Format("{0}_{1}", ScopeTag, Tag.Macro.To(Tag.Macro.Code.CALL)));
                data.Add(string.Format("{0}_{1}", ScopeTag, Tag.Macro.To(Tag.Macro.Code.ARGS)));
                data.Add(string.Format("{0}_{1}", ScopeTag, Tag.Macro.To(Tag.Macro.Code.VARS)));

                for (int v = 0; v < 10; v++)
                {
                    vars.Add(string.Format("{0}_{1}", Tag.Macro.Code.VARS, v));
                }
            }

            //-----------------------------------------------------------------
            public bool TreatData(CodeFile codeFile, Utils.KeyRule keyRule, List<string> args, string data)
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
            protected void AddCode(string code, CodeFile.ContentInfos data, Utils.SwapInfo swapSrc)
            {
                AddCode(code, data, swapSrc, new Utils.SwapInfo());
            }

            //-----------------------------------------------------------------
            protected void AddCode(string code, CodeFile.ContentInfos data, Utils.SwapInfo swapSrc, Utils.SwapInfo swapDst)
            {
                code = swapSrc.Apply(code);
                code = swapDst.Apply(code);
                data.codeGenerated += code;
            }
            #endregion Utils

            //-----------------------------------------------------------------
            #region CodeRule overridable
            public void Generate(CodeFile.ContentInfos data)
            {
                var variants = new List<Variant>();
                var maxSrc = data.classInfos.Count + (GenerateDefault ? 1 : 0);
                var maxDst = GenMode == GenerationMode.ForeachSrcXDest ? data.classInfos.Count : 1;
                var infoSrc = new CodeFile.ClassInfos();
                var infoDst = new CodeFile.ClassInfos();
                var infoDef = new CodeFile.ClassInfos();
                if (GenerateDefault)
                {
                    infoDef.names = new List<string>();
                    infoDef.names.Add(data.classDefaultType);
                }

                for (int iSrc = 0; iSrc < maxSrc; iSrc++)
                {
                    infoSrc = (GenerateDefault && iSrc == 0)
                        ? infoDef
                        : data.classInfos[iSrc + (GenerateDefault ? -1 : 0)];

                    for (int iSDst = 0; iSDst < maxDst; iSDst++)
                    {
                        if (GenMode == GenerationMode.ForeachSrcXDest)
                            infoDst = data.classInfos[iSDst];

                        GatherVariants(variants, data, infoSrc, infoDst);

                        var swapSrc = ClassSrc + infoSrc.names[0];
                        var swapDst = ClassDst;
                        if (GenMode == GenerationMode.ForeachSrcXDest)
                        {
                            swapDst = swapDst + infoDst.names[0];
                            AddCode(data.codePrefix, data, swapSrc, swapDst);
                        }
                        else
                        {
                            AddCode(data.codePrefix, data, swapSrc);
                        }

                        for (int v = 0; v < variants.Count; v++)
                        {
                            var variant = variants[v];
                            var code = data.codeMain;
                            code = (CodeCall + variant.Call).Apply(code);
                            code = (CodeArgs + variant.Args).Apply(code);
                            code = (CodeVars + variant.Vars).Apply(code);
                            if (GenMode == GenerationMode.ForeachSrcXDest)
                            {
                                AddCode(code, data, swapSrc, swapDst);
                            }
                            else
                            {
                                AddCode(code, data, swapSrc);
                            }
                        }

                        if (GenMode == GenerationMode.ForeachSrcXDest)
                        {
                            AddCode(data.codePostfix, data, swapSrc, swapDst);
                        }
                        else
                        {
                            AddCode(data.codePostfix, data, swapSrc);
                        }
                    }
                }

                data.codeGenerated = data.codeGenerated.Replace(Strings.NewLine(String.Empty), Strings.NewLine(String.Empty) + Tag.Macro.codeGenTabs.Keyword());
            }

            //-----------------------------------------------------------------
            public virtual Utils.KeyRule GetKeyRule(string keyword, int codeDepth)
            {
                if (keyword == CodeBlock)
                {
                    return new Utils.KeyRule(keyword, codeDepth == 1) { args = 2, needOpenScope = true };
                }
                else if (keyword == Tag.Macro.OperationClass)
                {
                    return new Utils.KeyRule(keyword, codeDepth == 2) { args = new Utils.KeyRule.ArgRange(1, -1) };
                }
                else if (keyword == Tag.Macro.DefaultInfo)
                {
                    return new Utils.KeyRule(keyword, codeDepth == 2) { args = 2 };
                }
                else if (keyword == Tag.Macro.CodePartPrefix || keyword == Tag.Macro.CodePartMain || keyword == Tag.Macro.CodePartSuffix)
                {
                    return new Utils.KeyRule(keyword, codeDepth == 2) { args = 0, needOpenScope = true, needScopeData = true };
                }

                return new Utils.KeyRule() { usage = Utils.KeyRule.Usage.Ignore };
            }

            //-----------------------------------------------------------------
            public virtual bool CloseScope(CodeFile codeFile, string scope)
            {
                if (scope == CodeBlock)
                {
                    codeFile.Submit();
                    return true;
                }
                else if (scope == Tag.Macro.Func
                     || scope == Tag.Macro.CodePartMain
                     || scope == Tag.Macro.CodePartPrefix
                     || scope == Tag.Macro.CodePartSuffix)
                {
                    return true;
                }
                return false;
            }

            //-----------------------------------------------------------------
            protected virtual bool DoTreatData(CodeFile.ContentInfos activeData, Utils.KeyRule keyRule, List<string> args, string data)
            {
                if (keyRule.key == CodeBlock)
                {
                    activeData.blockNamespace = args[0];
                    activeData.blockClassName = args[1];
                }
                else if (keyRule.key == Tag.Macro.OperationClass)
                {
                    activeData.classInfos.Add(new CodeFile.ClassInfos()
                    {
                        names = args.GetRange(0, 1),
                        variables = args.GetRange(1, args.Count - 1)
                    });
                }
                else if (keyRule.key == Tag.Macro.DefaultInfo)
                {
                    activeData.classDefaultType = args[0];
                    activeData.classDefaultValue = args[1];
                }
                else if (keyRule.key == Tag.Macro.CodePartPrefix)
                {
                    activeData.codePrefix = data;
                }
                else if (keyRule.key == Tag.Macro.CodePartMain)
                {
                    activeData.codeMain = data;
                }
                else if (keyRule.key == Tag.Macro.CodePartSuffix)
                {
                    activeData.codePostfix = data;
                }
                return true;
            }
            #endregion CodeRule override

            //-----------------------------------------------------------------
            #region CodeRule abstract
            protected abstract void GatherVariants(List<Variant> variants, CodeFile.ContentInfos data, CodeFile.ClassInfos infoSrc, CodeFile.ClassInfos infoDst);
            #endregion CodeRule abstract
        }
    }
}