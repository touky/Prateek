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
    using System.Collections.Generic;
    using System.Linq;
    using Assets.Prateek.CodeGenerator.Code.PrateekScriptBuilder.CodeAnalyzer;
    using Prateek.Core.Code.Helpers;
    using Prateek.Helpers;

    //-------------------------------------------------------------------------
    public partial class PrateekScriptBuilder
    {
        //---------------------------------------------------------------------
        public class CodeFile
        {
            //-----------------------------------------------------------------
            public struct ClassInfos
            {
                //-------------------------------------------------------------
                public string className;
                public List<string> names;
                public List<string> variables;

                //-------------------------------------------------------------
                public int NameCount { get { return names == null ? 0 : names.Count; } }
                public int VarCount { get { return variables == null ? 0 : variables.Count; } }
            }

            //-----------------------------------------------------------------
            public struct FuncInfos
            {
                public string funcName;
                public string data;
            }

            //-----------------------------------------------------------------
            public class ContentInfos
            {
                public ScriptAction activeRule;
                public string blockNamespace;
                public string blockClassName;
                public List<string> blockClassPrefix = new List<string>();

                public List<ClassInfos> classInfos = new List<ClassInfos>();
                public List<FuncInfos> funcInfos = new List<FuncInfos>();
                public string classDefaultType;
                public string classDefaultValue;
                public bool classDefaultExportOnly;

                public string codePrefix;
                public string codeMain;
                public string codePostfix;

                public string codeGenerated;

                //-------------------------------------------------------------
                public bool SetClassNames(List<string> args)
                {
                    if (classInfos.Count == 0)
                        return false;
                    var infos = classInfos.Last();
                    if (infos.names == null)
                        infos.names = new List<string>();
                    infos.names.AddRange(args);
                    classInfos[classInfos.Count - 1] = infos;
                    return true;
                }

                public bool SetClassNames(List<Keyword> arguments)
                {
                    if (classInfos.Count == 0)
                    {
                        return false;
                    }

                    var infos = classInfos.Last();
                    if (infos.names == null)
                    {
                        infos.names = new List<string>();
                    }
                    
                    foreach (var argument in arguments)
                    {
                        infos.names.Add(argument.Content);
                    }

                    classInfos[classInfos.Count - 1] = infos;
                    
                    return true;
                }

                //-------------------------------------------------------------
                public bool SetClassVars(List<string> args)
                {
                    if (classInfos.Count == 0)
                        return false;
                    var infos = classInfos.Last();
                    if (infos.variables == null)
                        infos.variables = new List<string>();
                    infos.variables.AddRange(args);
                    classInfos[classInfos.Count - 1] = infos;
                    return true;
                }
                public bool SetClassVars(List<Keyword> arguments)
                {
                    if (classInfos.Count == 0)
                        return false;
                    var infos = classInfos.Last();
                    if (infos.variables == null)
                        infos.variables = new List<string>();
                    foreach (var argument in arguments)
                    {
                        infos.variables.Add(argument.Content);
                    }
                    classInfos[classInfos.Count - 1] = infos;
                    return true;
                }

                //-------------------------------------------------------------
                public bool SetFuncData(string data)
                {
                    if (funcInfos.Count == 0)
                        return false;
                    var infos = funcInfos.Last();
                    infos.data = data;
                    funcInfos[funcInfos.Count - 1] = infos;
                    return true;
                }
            }

            //-----------------------------------------------------------------
            public string fileName;
            public string fileExtension;
            public string fileNamespace;

            //-----------------------------------------------------------------
            private ContentInfos codeInfos;
            private string codeGenerated;
            private List<ContentInfos> datas = new List<ContentInfos>();

            //-----------------------------------------------------------------
            public string CodeGenerated { get { return codeGenerated; } }
            public ContentInfos CodeInfos { get { return codeInfos; } }
            public int DataCount { get { return datas.Count; } }
            public ContentInfos this[int index] { get { return datas[index]; } }

            //-----------------------------------------------------------------
            public bool AllowRule(ScriptAction rule)
            {
                if (codeInfos == null)
                    return true;

                if (codeInfos.activeRule == null)
                    return true;

                return codeInfos.activeRule == rule;
            }

            //-----------------------------------------------------------------
            public ContentInfos NewData(ScriptAction codeSettings)
            {
                if (codeInfos != null)
                    return null;
                codeInfos = new ContentInfos() { activeRule = codeSettings };
                return codeInfos;
            }

            //-----------------------------------------------------------------
            public bool Commit()
            {
                var hasSubmitted = codeInfos != null;
                if (codeInfos != null)
                    datas.Add(codeInfos);
                codeInfos = null;

                return hasSubmitted;
            }

            //-----------------------------------------------------------------
            public CodeGenerator.CodeBuilder.BuildResult Generate(string genHeader, string genCode)
            {
                var genNSpc = (CodeBuilder.Utils.SwapInfo)Tag.Macro.codeGenNSpc.Keyword();
                var genExtn = (CodeBuilder.Utils.SwapInfo)Tag.Macro.codeGenExtn.Keyword();
                var genPrfx = (CodeBuilder.Utils.SwapInfo)Tag.Macro.codeGenPrfx.Keyword();
                var genData = (CodeBuilder.Utils.SwapInfo)Tag.Macro.codeGenData.Keyword();
                var genTabs = (CodeBuilder.Utils.SwapInfo)Tag.Macro.codeGenTabs.Keyword();

                var i = genCode.IndexOf(genData.Original);
                if (i < 0)
                    return CodeGenerator.CodeBuilder.BuildResult.ValueType.PrateekScriptSourceDataTagInvalid;

                var r = genCode.LastIndexOf(Strings.Separator.LineFeed.C(), i);
                if (r >= 0)
                    genTabs = genTabs + genCode.Substring(r + 1, i - (r + 1));

                codeGenerated = genHeader;
                for (int d = 0; d < datas.Count; d++)
                {
                    var data = datas[d];

                    var result = data.activeRule.Generate(data);
                    if (!result)
                    {
                        return result;
                    }

                    var code = genTabs.Apply(data.codeGenerated);
                    genNSpc += data.blockNamespace;
                    genExtn += data.blockClassName;
                    var prefix = string.Empty;
                    for (int p = 0; p < data.blockClassPrefix.Count; p++)
                    {
                        genPrfx += data.blockClassPrefix[p] + Strings.Separator.Space.S();
                    }
                    genData += code;
                    codeGenerated += genPrfx.Apply(genExtn.Apply(genData.Apply(genNSpc.Apply(genCode))));
                }

                return CodeGenerator.CodeBuilder.BuildResult.ValueType.Success;
            }
        }
    }
}
