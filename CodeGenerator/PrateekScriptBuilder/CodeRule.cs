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
            .SetFileContent("InternalContent_Prateek_script.txt")
            .Commit();
        }
    }

    //-------------------------------------------------------------------------
    public partial class PrateekScriptBuilder
    {
        //---------------------------------------------------------------------
        public abstract class CodeRule : ScriptTemplate.BaseTemplate
    {
            //-----------------------------------------------------------------
            public struct FuncVariant
            {
                //-------------------------------------------------------------
                private List<string> results;

                //-------------------------------------------------------------
                public string Call { get { return results[0]; } set { results[0] += value; } }
                public int Count { get { return results.Count; } }
                public string this[int i]
                {
                    get
                    {
                        if (i < 0 || i > results.Count)
                            return string.Empty;
                        return results[i];
                    }
                    set
                    {
                        if (i < 0 || i > results.Count)
                            return;
                        var result = results[i];
                        Set(ref result, value);
                        results[i] = result;
                    }
                }

                //-------------------------------------------------------------
                public FuncVariant(string value) : this(value, 0) { }
                public FuncVariant(string value, int emptySlot)
                {
                    results = new List<string>();
                    results.Add(value);
                    while (emptySlot-- > 0)
                    {
                        results.Add(string.Empty);
                    }
                }

                //-------------------------------------------------------------
                public void Add(string value)
                {
                    results.Add(value);
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
            public struct NumberedVars
            {
                //-----------------------------------------------------------------
                private List<string> datas;

                //-----------------------------------------------------------------
                public int Count { get { return datas.Count; } }
                public Utils.SwapInfo this[int i]
                {
                    get
                    {
                        if (i < 0 || i > datas.Count)
                            return string.Empty;
                        return datas[i];
                    }
                }

                //-----------------------------------------------------------------
                public NumberedVars(string root)
                {
                    datas = new List<string>();
                    for (int i = 0; i < 10; i++)
                    {
                        datas.Add(root + i);
                    }
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
            private NumberedVars names;
            private NumberedVars vars;
            private NumberedVars funcs;

            //-----------------------------------------------------------------
            public string CodeBlock { get { return data[0]; } }
            private string DataCall { get { return data[1]; } }
            private string DataArgs { get { return data[2]; } }
            private string DataVars { get { return data[3]; } }

            //-----------------------------------------------------------------
            public Utils.SwapInfo ClassDst { get { return Tag.Macro.dstClass; } }
            public Utils.SwapInfo ClassSrc { get { return Tag.Macro.srcClass; } }
            //public Utils.SwapInfo CodeCall { get { return DataCall; } }
            //public Utils.SwapInfo CodeArgs { get { return DataArgs; } }
            //public Utils.SwapInfo CodeVars { get { return DataVars; } }
            public NumberedVars Names { get { return names; } }
            public NumberedVars Vars { get { return vars; } }
            public NumberedVars Funcs { get { return funcs; } }

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
                data.Add(string.Format("{0}_{1}_{2}", Tag.Macro.prefix, Tag.Macro.To(Tag.Macro.FuncName.BLOCK), ScopeTag));
                //data.Add(string.Format("{0}_{1}", ScopeTag, Tag.Macro.To(Tag.Macro.VarName.CALL)));
                //data.Add(string.Format("{0}_{1}", ScopeTag, Tag.Macro.To(Tag.Macro.VarName.ARGS)));
                //data.Add(string.Format("{0}_{1}", ScopeTag, Tag.Macro.To(Tag.Macro.VarName.VARS)));

                names = new NumberedVars(string.Format("{0}_", Tag.Macro.VarName.NAMES));
                vars = new NumberedVars(string.Format("{0}_", Tag.Macro.VarName.VARS));
                funcs = new NumberedVars(string.Format("{0}_", Tag.Macro.VarName.FUNC_RESULT));
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
                var variants = new List<FuncVariant>();
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
                            for (int r = 0; r < variant.Count; r++)
                            {
                                code = (Funcs[r] + variant[r]).Apply(code);
                            }

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
            #endregion CodeRule override

            //-----------------------------------------------------------------
            #region CodeRule abstract
            protected abstract void GatherVariants(List<FuncVariant> variants, CodeFile.ContentInfos data, CodeFile.ClassInfos infoSrc, CodeFile.ClassInfos infoDst);
            #endregion CodeRule abstract
        }
    }
}