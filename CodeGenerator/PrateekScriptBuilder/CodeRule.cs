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
namespace Prateek.CodeGeneration
{
    using System;
    using System.Collections.Generic;
    using Prateek.Helpers;
    using UnityEditor;
    using static Prateek.ShaderTo.CSharp;

    //-------------------------------------------------------------------------
#if UNITY_EDITOR
    //todo: fix that [InitializeOnLoad]
    class PrateekScriptLoader : ScriptTemplate
    {
        static PrateekScriptLoader()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
                return;

            NewScript(PrateekScriptBuilder.Tag.importExtension.Extension(PrateekScriptBuilder.Tag.exportExtension), PrateekScriptBuilder.Tag.exportExtension)
            .SetAutorun(false)
            .SetTemplateFile(String.Empty)
            .SetFileContent("InternalContent_Prateek_script.txt")
            .Commit();
        }
    }
#endif //UNITY_EDITOR

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
                public int Count { get { return results == null ? 0 : results.Count; } }
                public string this[int i]
                {
                    get
                    {
                        if (i < 0 || i >= results.Count)
                            return string.Empty;
                        return results[i];
                    }
                    set
                    {
                        if (i < 0 || i >= results.Count)
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
                public FuncVariant(FuncVariant other)
                {
                    results = new List<string>(other.results);
                }

                //-------------------------------------------------------------
                public void Add(string value)
                {
                    results.Add(value);
                }

                //-------------------------------------------------------------
                private void Set(ref string dst, string value)
                {
                    if (dst != null && dst.Length > 0 && !dst.EndsWith(Strings.Separator.LineFeed.S()))
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
            private string codeBlock = string.Empty;

            //-----------------------------------------------------------------
            public string CodeBlock { get { return codeBlock; } }

            //-----------------------------------------------------------------
            public Utils.SwapInfo ClassDst { get { return Tag.Macro.dstClass; } }
            public Utils.SwapInfo ClassSrc { get { return Tag.Macro.srcClass; } }
            public Tag.NumberedVars Names { get { return Tag.Macro.Names; } }
            public Tag.NumberedVars Funcs { get { return Tag.Macro.Funcs; } }
            public Tag.NumberedVars Vars { get { return Tag.Macro.Vars; } }

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
                codeBlock = string.Format("{0}_{1}_{2}", Tag.Macro.prefix, Tag.Macro.To(Tag.Macro.FuncName.BLOCK), ScopeTag);
            }

            //-----------------------------------------------------------------
            public bool RetrieveRuleContent(CodeFile codeFile, Utils.KeyRule keyRule, List<string> args, string data)
            {
                var activeData = codeFile.ActiveData;
                if (activeData == null)
                {
                    activeData = codeFile.NewData(this);
                }

                if (activeData.activeRule == null || activeData.activeRule != this)
                    return false;

                return DoRetrieveRuleContent(activeData, keyRule, args, data);
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
            public BuildResult Generate(CodeFile.ContentInfos data)
            {
                var variants = new List<FuncVariant>();
                //If needed, Add the default value as a possible class
                var maxSrc = data.classInfos.Count + (GenerateDefault ? 1 : 0);
                //Only loop through the dst classes if required
                var maxDst = GenMode == GenerationMode.ForeachSrcXDest ? data.classInfos.Count : 1;
                var infoSrc = new CodeFile.ClassInfos();
                var infoDst = new CodeFile.ClassInfos();
                var infoDef = new CodeFile.ClassInfos();
                if (GenerateDefault)
                {
                    infoDef.className = data.classDefaultType;
                }

                //Find the amount of FUNC_RESULT & NAMES that are requested by the main code
                var funcCount = Funcs.GetCount(data.codeMain);

                //Loop throught the source classes
                for (int iSrc = 0; iSrc < maxSrc; iSrc++)
                {
                    //Add the default value as a possible class
                    infoSrc = (GenerateDefault && iSrc == 0)
                        ? infoDef
                        : data.classInfos[iSrc + (GenerateDefault ? -1 : 0)];

                    //one pass or as many as the dst classes
                    for (int iSDst = 0; iSDst < maxDst; iSDst++)
                    {
                        if (GenMode == GenerationMode.ForeachSrcXDest)
                            infoDst = data.classInfos[iSDst];

                        //Gather code variants
                        GatherVariants(variants, data, infoSrc, infoDst);

                        var swapSrc = ClassSrc + infoSrc.className;
                        var swapDst = ClassDst;
                        //Add the prefix from the code file
                        if (GenMode == GenerationMode.ForeachSrcXDest)
                        {
                            swapDst = swapDst + infoDst.className;
                            AddCode(data.codePrefix, data, swapSrc, swapDst);
                        }
                        else
                        {
                            AddCode(data.codePrefix, data, swapSrc);
                        }

                        //Go through all variants and apply them to the code
                        for (int v = 0; v < variants.Count; v++)
                        {
                            var variant = variants[v];
                            var code = data.codeMain;

                            //Error out if the requested funcs result are not available
                            if (funcCount > variant.Count)
                            {
                                return (BuildResult)BuildResult.ValueType.PrateekScriptInsufficientNames + GetType().Name + infoSrc.className;
                            }

                            //Apply variants
                            for (int r = 0; r < variant.Count; r++)
                            {
                                code = (Funcs[r] + variant[r]).Apply(code);
                            }

                            //Error out if the requested Names are not available
                            var nameCount = Names.GetCount(code);
                            if (nameCount > infoSrc.NameCount)
                            {
                                return (BuildResult)BuildResult.ValueType.PrateekScriptInsufficientNames + infoSrc.className;
                            }

                            //Apply names
                            for (int r = 0; r < min(nameCount, infoSrc.NameCount); r++)
                            {
                                code = (Names[r] + infoSrc.names[r]).Apply(code);
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

                        //Add the Postfix from the code file
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

                return BuildResult.ValueType.Success;
            }

            //-----------------------------------------------------------------
            public virtual Utils.KeyRule GetKeyRule(string keyword, string activeScope)
            {
                var keyRule = new Utils.KeyRule(keyword, activeScope);
                if (keyRule.Match(CodeBlock, Tag.Macro.FileInfo))
                {
                    { keyRule.args = new Utils.KeyRule.ArgRange(2, -1); keyRule.needOpenScope = true; }
                }
                else if (keyRule.Match(Tag.Macro.ClassInfo, codeBlock))
                {
                    { keyRule.args = 1; keyRule.needOpenScope = true; }
                }
                else if (keyRule.Match(Tag.Macro.ClassNames, Tag.Macro.ClassInfo))
                {
                    { keyRule.args = new Utils.KeyRule.ArgRange(1, -1); }
                }
                else if (keyRule.Match(Tag.Macro.ClassVars, Tag.Macro.ClassInfo))
                {
                    { keyRule.args = new Utils.KeyRule.ArgRange(1, -1); }
                }

                else if (keyRule.Match(Tag.Macro.DefaultInfo, codeBlock))
                {
                    { keyRule.args = new Utils.KeyRule.ArgRange(2, 3); }
                }
                else if (keyRule.Match(Tag.Macro.Func, CodeBlock))
                {
                    { keyRule.needOpenScope = true; keyRule.needScopeData = true; }
                }
                else if (keyRule.Match(Tag.Macro.CodePartPrefix, codeBlock)
                      || keyRule.Match(Tag.Macro.CodePartMain, codeBlock)
                      || keyRule.Match(Tag.Macro.CodePartSuffix, codeBlock))
                {
                    { keyRule.args = 0; keyRule.needOpenScope = true; keyRule.needScopeData = true; }
                }
                else
                {
                    return new Utils.KeyRule() { usage = Utils.KeyRule.Usage.Ignore };
                }
                return keyRule;
            }

            //-----------------------------------------------------------------
            protected virtual bool DoRetrieveRuleContent(CodeFile.ContentInfos activeData, Utils.KeyRule keyRule, List<string> args, string data)
            {
                if (keyRule.Match(CodeBlock, Tag.Macro.FileInfo))
                {
                    activeData.blockNamespace = args[0];
                    activeData.blockClassName = args[1];
                    if (args.Count > 2)
                    {
                        activeData.blockClassPrefix.AddRange(args.GetRange(2, args.Count - 2));
                    }
                }
                else if (keyRule.Match(Tag.Macro.ClassInfo, codeBlock))
                {
                    activeData.classInfos.Add(new CodeFile.ClassInfos() { className = args[0] });
                }
                else if (keyRule.Match(Tag.Macro.ClassNames, Tag.Macro.ClassInfo))
                {
                    if (!activeData.SetClassNames(args))
                        return false;
                }
                else if (keyRule.Match(Tag.Macro.ClassVars, Tag.Macro.ClassInfo))
                {
                    if (!activeData.SetClassVars(args))
                        return false;
                }
                else if (keyRule.Match(Tag.Macro.DefaultInfo, codeBlock))
                {
                    activeData.classDefaultType = args[0];
                    activeData.classDefaultValue = args[1];
                    activeData.classDefaultExportOnly = (args.Count == 2 || args[2] == "false") ? false : true;
                }
                else if (keyRule.Match(Tag.Macro.Func, CodeBlock))
                {
                    activeData.funcInfos.Add(new CodeFile.FuncInfos());
                    activeData.SetFuncData(data);
                }
                else if (keyRule.Match(Tag.Macro.CodePartPrefix, codeBlock))
                {
                    activeData.codePrefix = data;
                }
                else if (keyRule.Match(Tag.Macro.CodePartMain, codeBlock))
                {
                    activeData.codeMain = data;
                }
                else if (keyRule.Match(Tag.Macro.CodePartSuffix, codeBlock))
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
                    codeFile.Commit();
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