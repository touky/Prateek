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
namespace Prateek
{
    //-------------------------------------------------------------------------
    [CreateAssetMenu(menuName = "Prateek/New CodeGenerator", fileName = "NewCodeGenerator")]
    public class CodeGenerator : ScriptableObject
    {
        //---------------------------------------------------------------------
        #region Declarations
        public class CodeAnalyzer
        {
            //-----------------------------------------------------------------
            public enum SymbolType
            {
                WhiteSpace,
                LineFeed,
                Numeric,
                Letter,
                Literal,
                ScopeStart,
                ScopeEnd,
                CallStart,
                CallEnd,
                ArgSplit,
                Comment,

                MAX
            }

            //-----------------------------------------------------------------
            public string content;
            public bool allowKeywordStartWithAlpha = false;

            //-----------------------------------------------------------------
            private int position = 0;
            private int trailingWhiteSpaces = 0;
            private List<string> scopes = new List<string>();

            //-----------------------------------------------------------------
            private char[] charUnder = new char[1] { '_' };
            private char[] charLiteral = new char[1] { '@' };
            private char[] charIgnore = new char[2] { ' ', '\n' };
            private char[] charCalls = new char[2] { '(', ')' };
            private char[] charArgs  = new char[1] { ',' };
            private char[] charScope = new char[2] { '{', '}' };
            private char[] charComment = new char[2] { '/', '*' };

            //-----------------------------------------------------------------
            public bool ShouldContinue { get { return position < content.Length; } }

            //-----------------------------------------------------------------
            public void Init(string content)
            {
                position = 0;
                trailingWhiteSpaces = 0;
                scopes.Clear();

                this.content = content;

                RemoveComments();
            }

            //-----------------------------------------------------------------
            public bool FindKeyword(ref string keyword)
            {
                keyword = String.Empty;
                while (position < content.Length)
                {
                    var type = GetSymbol(content[position]);
                    switch (type)
                    {
                        case SymbolType.Numeric:
                        case SymbolType.Letter:
                        {
                            if (type == SymbolType.Numeric && keyword.Length == 0)
                                return false;
                            keyword += content[position];
                            break;
                        }
                        case SymbolType.Comment:
                        case SymbolType.ScopeStart:
                        case SymbolType.WhiteSpace:
                        case SymbolType.LineFeed:
                        {
                            if (keyword.Length > 0)
                                return true;

                            if (type == SymbolType.ScopeStart)
                                scopes.Add(string.Empty);
                            break;
                        }
                        default:
                        {
                            if (keyword.Length > 0)
                                return true;
                            return false;
                        }
                    }

                    position++;
                }
                return keyword.Length > 0;
            }

            //-----------------------------------------------------------------
            public bool FindArgs(List<string> args, Tag.Keyword setup)
            {
                args.Clear();

                var argSplit = SymbolType.MAX;
                var argScope = SymbolType.MAX;
                var foundScope = false;
                var keyword = string.Empty;
                bool allowContinue = true;
                while (allowContinue && position < content.Length)
                {
                    var type = GetSymbol(content[position]);
                    switch (type)
                    {
                        case SymbolType.CallStart:
                        {
                            if (setup.NoArgNeeded)
                                return false;

                            if (argScope != SymbolType.MAX)
                                return false;

                            argSplit = SymbolType.ArgSplit;
                            argScope = SymbolType.CallStart;
                            break;
                        }
                        case SymbolType.CallEnd:
                        {
                            if (setup.NoArgNeeded)
                                return false;

                            if (argScope != SymbolType.CallStart)
                                return false;

                            argSplit = SymbolType.ArgSplit;
                            argScope = SymbolType.CallEnd;

                            allowContinue = setup.needOpenScope;
                            if (keyword.Length > 0)
                            {
                                args.Add(keyword);
                                keyword = string.Empty;
                            }
                            break;
                        }
                        case SymbolType.Numeric:
                        case SymbolType.Letter:
                        {
                            if (setup.NoArgNeeded)
                                return false;

                            keyword += content[position];
                            argSplit = SymbolType.Letter;
                            break;
                        }
                        case SymbolType.WhiteSpace:
                        case SymbolType.LineFeed:
                        case SymbolType.ArgSplit:
                        {
                            if (type == SymbolType.ArgSplit)
                            {
                                if (setup.NoArgNeeded)
                                    return false;

                                if (argSplit == SymbolType.ArgSplit)
                                    return false;
                            }

                            argSplit = type;
                            if (keyword.Length > 0)
                            {
                                args.Add(keyword);
                                keyword = string.Empty;
                            }
                            break;
                        }
                        case SymbolType.ScopeStart:
                        {
                            scopes.Add(setup.needOpenScope ? setup.keyword : string.Empty);

                            foundScope = true;
                            allowContinue = false;
                            if (!setup.needOpenScope)
                                break;

                            if (argSplit == SymbolType.ArgSplit)
                                return false;

                            if (argScope != SymbolType.CallEnd && argScope != SymbolType.MAX)
                                return false;
                            break;
                        }
                        default:
                        {
                            return false;
                        }
                    }

                    position++;
                }

                if (setup.needOpenScope && !foundScope)
                    return false;
                return setup.CheckArgCount(args.Count);
            }

            //-----------------------------------------------------------------
            public bool FindData(ref string data, Tag.Keyword setup)
            {
                if (!setup.needScopeData)
                    return true;
                data = string.Empty;

                var allowContinue = true;
                var storeData = false;
                var scopeCount = 1;
                while (allowContinue && position < content.Length)
                {
                    var type = GetSymbol(content[position]);
                    switch (type)
                    {
                        case SymbolType.ScopeStart:
                        {
                            if (storeData)
                            {
                                data += content[position];
                                break;
                            }

                            scopeCount++;
                            break;
                        }
                        case SymbolType.ScopeEnd:
                        {
                            if (storeData)
                            {
                                data += content[position];
                                break;
                            }

                            if (--scopeCount == 0)
                            {
                                return true;
                            }
                            break;
                        }
                        case SymbolType.Literal:
                        {
                            storeData = true;
                            break;
                        }
                        case SymbolType.LineFeed:
                        {
                            if (storeData)
                            {
                                data += content[position];
                                storeData = false;
                            }
                            break;
                        }
                        default:
                        {
                            if (storeData)
                            {
                                data += content[position];
                            }
                            break;
                        }
                    }

                    position++;
                }
                return true;
            }

            //-----------------------------------------------------------------
            public bool FindScopeEnd(ref string scopeName)
            {
                while (position < content.Length)
                {
                    var type = GetSymbol(content[position]);
                    switch (type)
                    {
                        case SymbolType.ScopeEnd:
                        {
                            if (scopes.Count == 0)
                                return false;

                            scopeName = scopes.Last();
                            scopes.RemoveLast();
                            position++;
                            return true;
                        }
                        case SymbolType.WhiteSpace:
                        {
                            trailingWhiteSpaces++;
                            break;
                        }
                        case SymbolType.LineFeed:
                        {
                            trailingWhiteSpaces = 0;
                            break;
                        }
                        default:
                        {
                            return false;
                        }
                    }

                    position++;
                }
                return false;
            }

            //-----------------------------------------------------------------
            private void RemoveComments()
            {
                int commentStart = -1;
                int commentEnd = -1;
                bool ignoreAllToLineEnd = false;
                bool lookForLineEnd = false;
                bool lookForContEnd = false;
                for (int i = 0; i < content.Length - 1; i++)
                {
                    var value0 = content[i];
                    var value1 = content[i + 1];
                    if (ignoreAllToLineEnd)
                    {
                        if (value0 == charIgnore[1])
                        {
                            ignoreAllToLineEnd = false;
                        }
                        continue;
                    }

                    if (lookForLineEnd || lookForContEnd)
                    {
                        if (lookForLineEnd && value0 == charIgnore[1])
                            commentEnd = i;
                        if (lookForContEnd && value0 == charComment[1] && value1 == charComment[0])
                            commentEnd = ++i;

                        if (commentEnd >= 0)
                        {
                            content = content.Remove(commentStart, (commentEnd + 1) - commentStart);
                            i = commentStart - 1;
                            lookForLineEnd = false;
                            lookForContEnd = false;
                            commentStart = -1;
                            commentEnd = -1;
                        }

                        continue;
                    }

                    if (value0 == charComment[0])
                    {
                        lookForLineEnd = value1 == charComment[0];
                        lookForContEnd = value1 == charComment[1];

                        if (lookForLineEnd || lookForContEnd)
                            commentStart = i;
                    }
                    else if (value0 == charLiteral[0])
                    {
                        ignoreAllToLineEnd = true;
                    }
                }
            }

            //-----------------------------------------------------------------
            private SymbolType GetSymbol(char value)
            {
                if (value == charIgnore[1])
                {
                    trailingWhiteSpaces = 0;
                    return SymbolType.LineFeed;
                }

                if (value == charIgnore[0])
                {
                    trailingWhiteSpaces++;
                    return SymbolType.WhiteSpace;
                }

                if (value == charCalls[0])
                    return SymbolType.CallStart;

                if (value == charCalls[1])
                    return SymbolType.CallEnd;

                if (value == charScope[0])
                {
                    return SymbolType.ScopeStart;
                }

                if (value == charScope[1])
                {
                    return SymbolType.ScopeEnd;
                }

                if (value == charLiteral[0])
                {
                    return SymbolType.Literal;
                }

                for (int c = 0; c < charArgs.Length; c++)
                {
                    if (charArgs[c] == value)
                        return SymbolType.ArgSplit;
                }

                if (Char.IsNumber(value))
                    return SymbolType.Numeric;

                if (Char.IsLetter(value) || value == charUnder[0])
                    return SymbolType.Letter;

                return SymbolType.MAX;
            }
        }

        //---------------------------------------------------------------------
        public static class Tag
        {
            //Default datas ---------------------------------------------------
            public const string sourceExtension = "prtk";
            public const string destinationExtension = "cs";

            public const string fileHeader =
@"#PRATEEK_COPYRIGHT#

#PRATEEK_CSHARP_NAMESPACE#
";
            public const string fileCode =
@"
//-----------------------------------------------------------------------------
namespace Prateek.Extensions
{
    //-------------------------------------------------------------------------
    public static partial class #PRATEEK_STATIC_EXTENSION_CLASS#
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
                public static string codeGenExtn = "PRATEEK_STATIC_EXTENSION_CLASS";
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
                public static string CodePartMain   { get { return data[2]; } }
                public static string CodePartSuffix { get { return data[3]; } }
                public static string OperationClass { get { return data[4]; } }
                public static string TypeInfo /***/ { get { return data[5]; } }

                //-------------------------------------------------------------
                public static string To(Tag.Macro.Content value) { return Enum.GetNames(typeof(Tag.Macro.Content))[(int)value]; }
                public static string To(Tag.Macro.Code value) { return Enum.GetNames(typeof(Tag.Macro.Code))[(int)value]; }

                //-------------------------------------------------------------
                public static void Init()
                {
                    data.Add(string.Format("{0}_{1}", Tag.Macro.prefix, To(Tag.Macro.Content.FILE_INFO)));
                    data.Add(string.Format("{0}_{1}_{2}", Tag.Macro.prefix, Tag.Macro.codeData, To(Tag.Macro.Content.PREFIX)));
                    data.Add(string.Format("{0}_{1}_{2}", Tag.Macro.prefix, Tag.Macro.codeData, To(Tag.Macro.Content.MAIN)));
                    data.Add(string.Format("{0}_{1}_{2}", Tag.Macro.prefix, Tag.Macro.codeData, To(Tag.Macro.Content.SUFFIX)));
                    data.Add(string.Format("{0}_{1}", Tag.Macro.prefix, To(Tag.Macro.Content.CLASS)));
                    data.Add(string.Format("{0}_{1}", Tag.Macro.prefix, To(Tag.Macro.Content.TYPE)));
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

        //---------------------------------------------------------------------
        public abstract class CodeSettings
        {
            //-----------------------------------------------------------------
            public abstract string ScopeTag { get; }

            //-----------------------------------------------------------------
            private List<string> data = new List<string>();

            //-----------------------------------------------------------------
            public string CodeBlock     { get { return data[0]; } }
            private string DataCall      { get { return data[1]; } }
            private string DataArgs      { get { return data[2]; } }
            private string DataVars      { get { return data[3]; } }

            //-----------------------------------------------------------------
            public Tag.SwapInfo ClassDst { get { return Tag.Macro.dstClass; } }
            public Tag.SwapInfo ClassSrc { get { return Tag.Macro.srcClass; } }
            public Tag.SwapInfo CodeCall { get { return DataCall; } }
            public Tag.SwapInfo CodeArgs { get { return DataArgs; } }
            public Tag.SwapInfo CodeVars { get { return DataVars; } }

            //-----------------------------------------------------------------
            protected CodeSettings()
            {
                Init();
            }

            //-----------------------------------------------------------------
            private void Init()
            {
                data.Add(string.Format("{0}_{1}_{2}", Tag.Macro.prefix, Tag.Macro.To(Tag.Macro.Content.BLOCK), ScopeTag));
                data.Add(string.Format("{0}_{1}", ScopeTag, Tag.Macro.To(Tag.Macro.Code.CALL)));
                data.Add(string.Format("{0}_{1}", ScopeTag, Tag.Macro.To(Tag.Macro.Code.ARGS)));
                data.Add(string.Format("{0}_{1}", ScopeTag, Tag.Macro.To(Tag.Macro.Code.VARS)));
            }

            //-----------------------------------------------------------------
            protected void Submit()
            {
                CodeGenerator.Add(this);
            }

            //-----------------------------------------------------------------
            public virtual Tag.Keyword GetSetup(string keyword, int codeDepth)
            {
                if (keyword == CodeBlock)
                {
                    return new Tag.Keyword(keyword, codeDepth == 1) { minArgCount = 1, maxArgCount = 1, needOpenScope = true };
                }
                else if (keyword == Tag.Macro.OperationClass)
                {
                    return new Tag.Keyword(keyword, codeDepth == 2) { minArgCount = 1 };
                }
                else if (keyword == Tag.Macro.TypeInfo)
                {
                    return new Tag.Keyword(keyword, codeDepth == 2) { minArgCount = 2, maxArgCount = 2 };
                }
                else if (keyword == Tag.Macro.CodePartPrefix || keyword == Tag.Macro.CodePartMain || keyword == Tag.Macro.CodePartSuffix)
                {
                    return new Tag.Keyword(keyword, codeDepth == 2) { minArgCount = 0, maxArgCount = 0, needOpenScope = true, needScopeData = true };
                }

                return new Tag.Keyword() { usage = Tag.Keyword.Usage.Ignore };
            }

            //-----------------------------------------------------------------
            public abstract bool TreatData(CodeFile codeFile, Tag.Keyword keyword, List<string> args, string data);

            //-----------------------------------------------------------------
            public virtual bool CloseScope(CodeFile codeFile, string scope)
            {
                if (scope == CodeBlock)
                {
                    codeFile.Submit();
                    return true;
                }
                else if (scope == Tag.Macro.CodePartMain || scope == Tag.Macro.CodePartPrefix || scope == Tag.Macro.CodePartSuffix)
                {
                    return true;
                }
                return false;
            }

            //-----------------------------------------------------------------
            public abstract void Generate(CodeFile.Data data);



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

        //---------------------------------------------------------------------
        public class CodeFile
        {
            //-----------------------------------------------------------------
            public class Data
            {
                public struct ClassInfo
                {
                    public string name;
                    public List<string> variables;
                }

                public CodeSettings settings;
                public string blockName;

                public List<ClassInfo> classInfos = new List<ClassInfo>();
                public string classContentType;
                public string classContentValue;

                public string codePrefix;
                public string codeMain;
                public string codePostfix;

                public string codeGenerated;
            }

            //-----------------------------------------------------------------
            public string fileName;
            public string fileExtension;

            //-----------------------------------------------------------------
            private Data activeData;
            private string codeGenerated;
            private List<Data> datas = new List<Data>();

            //-----------------------------------------------------------------
            public string CodeGenerated {  get { return codeGenerated; } }
            public Data ActiveData { get { return activeData; } }
            public int DataCount { get { return datas.Count; } }
            public Data this[int index] { get { return datas[index]; } }

            //-----------------------------------------------------------------
            public Data NewData(CodeSettings codeSettings)
            {
                if (activeData != null)
                    return null;
                activeData = new Data() { settings = codeSettings };
                return activeData;
            }

            //-----------------------------------------------------------------
            public bool Submit()
            {
                var hasSubmitted = activeData != null;
                if (activeData != null)
                    datas.Add(activeData);
                activeData = null;

                return hasSubmitted;
            }

            //-----------------------------------------------------------------
            public bool Generate()
            {
                var genHeader = Tag.fileHeader.SimplifyNewLines().TabToSpaces();
                var genCode = Tag.fileCode.SimplifyNewLines().TabToSpaces();
                var genExtn = (Tag.SwapInfo)Tag.Macro.codeGenExtn.Keyword();
                var genData = (Tag.SwapInfo)Tag.Macro.codeGenData.Keyword();
                var genTabs = (Tag.SwapInfo)Tag.Macro.codeGenTabs.Keyword();

                var i = genCode.IndexOf(genData.Original);
                if (i < 0)
                    return false;

                var r = genCode.LastIndexOf(Strings.Separator.NewLine.C()[0], i);
                if (r >= 0)
                    genTabs = genTabs + genCode.Substring(r + 1, i - (r + 1));

                codeGenerated = genHeader;
                for (int d = 0; d < datas.Count; d++)
                {
                    var data = datas[d];
                    data.settings.Generate(data);

                    var code = genTabs.Apply(data.codeGenerated);
                    genExtn += data.blockName;
                    genData += code;
                    codeGenerated += genExtn.Apply(genData.Apply(genCode));
                }

                return true;
            }
        }
        #endregion Declarations

        //---------------------------------------------------------------------
        #region Settings
        [SerializeField]
        private List<string> sourceDirectories = new List<string>();
        [SerializeField]
        private string destinationDirectory;
        #endregion Settings

        //---------------------------------------------------------------------
        #region Fields
        private static List<CodeSettings> settings = new List<CodeSettings>();
        #endregion Fields

        //---------------------------------------------------------------------
        #region Unity Defaults
        [ContextMenu("Generate code")]
        public void StartGeneration()
        {
            Tag.Macro.Init(); //TODO Auto load

            var files = new List<string>();
            if (!FindSources(files))
            {
                UnityEngine.Debug.LogError("No files found");
            }

            //Start the generating
            for (int f = 0; f < files.Count; f++)
            {
                var path = files[f];
                if (!File.Exists(path))
                    continue;

                if (!GenerateFile(path))
                {
                    UnityEngine.Debug.LogError("Code generation failed");
                    break;
                }
            }
        }

        //---------------------------------------------------------------------
        public static void Add(CodeSettings setting)
        {
            settings.Add(setting);
        }

        //---------------------------------------------------------------------
        private bool FindSources(List<string> files)
        {
            for (int d = 0; d < sourceDirectories.Count; d++)
            {
                if (!Directory.Exists(sourceDirectories[d]))
                    continue;

                FileHelpers.GatherFilesAt(sourceDirectories[d], files, FileHelpers.BuildExtensionMatch(Tag.sourceExtension), true);
            }
            return files.Count > 0;
        }

        //---------------------------------------------------------------------
        private bool GenerateFile(string path)
        {
            var sources = FileHelpers.ReadAllTextCleaned(path);
            if (sources == string.Empty)
                return false;

            var analyzer = new CodeAnalyzer();
            var activeCodeFile = (CodeFile)null;
            var codeFiles = new List<CodeFile>();
            var codeDepth = 0;

            analyzer.Init(sources);

            var args = new List<string>();
            var keyword = string.Empty;
            var data = String.Empty;
            while (analyzer.ShouldContinue)
            {
                if (analyzer.FindKeyword(ref keyword))
                {
                    if (codeDepth == 0)
                    {
                        if (keyword == Tag.Macro.FileInfo)
                        {
                            var setup = new Tag.Keyword(keyword, true) { minArgCount = 2, maxArgCount = 2, needOpenScope = true };
                            if (!analyzer.FindArgs(args, setup))
                                break;

                            activeCodeFile = codeFiles.Find((x) => { return x.fileName == args[0] && x.fileExtension == args[1]; });
                            if (activeCodeFile == null)
                            {
                                activeCodeFile = new CodeFile() { fileName = args[0], fileExtension = args[1] };
                                codeFiles.Add(activeCodeFile);
                            }

                            codeDepth++;
                        }
                    }
                    else
                    {
                        var foundMatch = false;
                        for (int s = 0; s < settings.Count; s++)
                        {
                            var setting = settings[s];
                            var setup = setting.GetSetup(keyword, codeDepth);
                            if (setup.usage != Tag.Keyword.Usage.Match)
                                continue;

                            if (!analyzer.FindArgs(args, setup))
                                break;

                            if (!analyzer.FindData(ref data, setup))
                                break;

                            if (!setting.TreatData(activeCodeFile, setup, args, data))
                                break;

                            foundMatch = true;
                            if (setup.needOpenScope)
                                codeDepth++;
                            break;
                        }

                        if (!foundMatch)
                            break;
                    }
                }
                else
                {
                    var scopeName = string.Empty;
                    if (analyzer.FindScopeEnd(ref scopeName))
                    {
                        if (activeCodeFile != null)
                        {
                            if (activeCodeFile.ActiveData != null)
                            {
                                if (activeCodeFile.ActiveData.settings == null)
                                    break;

                                if (activeCodeFile.ActiveData.settings.CloseScope(activeCodeFile, scopeName))
                                    codeDepth--;
                            }
                            else if (codeDepth == 1 && scopeName == Tag.Macro.FileInfo)
                            {
                                activeCodeFile = null;
                                codeDepth--;
                            }
                        }
                    }
                }
            }

            //code files have been filled
            for (int f = 0; f < codeFiles.Count; f++)
            {
                var codeFile = codeFiles[f];
                { // Log shit
                    var log = String.Format("FOUND: {0}.{1}\n", codeFile.fileName, codeFile.fileExtension);
                    for (int d = 0; d < codeFile.DataCount; d++)
                    {
                        var fileData = codeFile[d];
                        for (int i = 0; i < fileData.classInfos.Count; i++)
                        {
                            var info = fileData.classInfos[i];
                            log += String.Format("  - CLASS: {0} ", info.name);
                            for (int v = 0; v < info.variables.Count; v++)
                            {
                                log += " " + info.variables[v];
                            }
                            log += "\n";
                        }

                        log += String.Format("  - TYPE: {0} = {1}\n", fileData.classContentType, fileData.classContentValue);
                        log += String.Format("  - CODE PREFIX:\n > {0}\n", fileData.codePrefix.Replace("\n", "\n> "));
                        log += String.Format("  - CODE MAIN:\n > {0}\n", fileData.codeMain.Replace("\n", "\n> "));
                        log += String.Format("  - CODE POSTFIX:\n > {0}\n", fileData.codePostfix.Replace("\n", "\n> "));
                    }
                    UnityEngine.Debug.Log(log);
                }

                { // Build the actual code
                    codeFile.Generate();

                    for (int i = 0; i < codeFile.DataCount; i++)
                    {
                        var code = codeFile[i];
                        UnityEngine.Debug.Log(code.codeGenerated);
                    }

                    UnityEngine.Debug.Log(codeFile.CodeGenerated);
                }
            }

            return true;

#if false
            var sourceLines = new List<string>();
            var classContents = new List<string>();
            var classSettings = (CodeSettings)null;

            var codeFiles = new List<CodeFile>();
            var matchMethod = new Regex(Tag.Macro.extractFuncRegex);
            var matchArgs = new Regex(Tag.Macro.extractArgsRegex);

            //Go through the content and search for any codegen tag
            for (int l = 0; l < sourceLines.Count; l++)
            {
                var line = sourceLines[l];
                var matc0 = Regex.Match(line, Tag.Macro.macroMatch0);
                while (matc0 != null && matc0.Success)
                {
                    matc0 = matc0.NextMatch();
                }
                //for (int g = 0; g < matc0.Groups.Count; g++)
                //{
                //    var test = matc0.Groups[g].Value;
                //    test = test;
                //}
                break;

                var matc = Regex.Match(line, string.Format(Tag.Macro.macroMatch, Tag.Macro.FileInfo));
                var erer = matc.Groups[1].Value;
                var method = matchMethod.Match(line);// 
                if (!method.Success || method.Groups.Count != 2)
                    continue; //TODO ERROR ?

                var call = method.Groups[0].Value;
                var args = matchArgs.Match(method.Groups[1].Value);
                var a0 = args.Groups[1].Value;
                var a1 = args.Groups[2].Value;
                if (args.Groups.Count != 2)
                    continue; //TODO ERROR ?

                //var subStart = match.Value.IndexOf(Strings.C(Strings.Separator.Parenthesis)[0]);
                //var subStop = match.Value.IndexOf(Strings.C(Strings.Separator.Parenthesis)[1]);
                //var args = match.Value.Substring(subStart + 1, subStop - (subStart + 1)).Split(Strings.C(Strings.Separator.TextParse), StringSplitOptions.RemoveEmptyEntries);





                //if (!line.Contains(Tag.codeStart))
                //    continue;

                //var codePrefix = string.Empty;
                //var codeSource = string.Empty;
                //var codePostfix = string.Empty;

                //var reachedCall = false;
                //sourceLines.RemoveAt(l);
                //while (l < sourceLines.Count)
                //{
                //    line = sourceLines[l];
                //    //Retrieve potential code settings and load them
                //    {
                //        if (classSettings == null)
                //        {
                //            classSettings = GetCodeSettings(line);
                //        }

                //        if (classSettings != null && classSettings.Load(line))
                //        {
                //            sourceLines.RemoveAt(l);
                //            continue;
                //        }
                //    }

                //    //OR fill the code call
                //    if (classSettings != null && line.Contains(classSettings.codeCall))
                //    {
                //        reachedCall = true;
                //        codeSource = line;
                //        sourceLines.RemoveAt(l);
                //        continue;
                //    }

                //    //Stop the search if we hit the stop tag
                //    if (line.Contains(Tag.codeStop))
                //    {
                //        sourceLines.RemoveAt(l);
                //        break;
                //    }

                //    //Or if nothing, fill the code prefix/postfix
                //    if (reachedCall)
                //    {
                //        codePostfix += line.NewLine();
                //    }
                //    else
                //    {
                //        codePrefix += line.NewLine();
                //    }

                //    sourceLines.RemoveAt(l);
                //}

                //if (classSettings == null)
                //    continue;

                //classSettings.codePrefix = codePrefix;
                //classSettings.codeSource = codeSource;
                //classSettings.codePostfix = codePostfix;

                ////Prepare class codes
                //classCode = new CodeData[classSettings.codeClasses.Count];
                //for (int cc = 0; cc < classSettings.codeClasses.Count; cc++)
                //{
                //    classCode[cc] = new CodeData();
                //}

                ////Fill all the slots datas
                //var finalCode = string.Empty;
                //for (int sd = 0; sd < classSettings.codeClasses.Count; sd++)
                //{
                //    //Load up the pre/post-fix
                //    var srcDef = classSettings.codeClasses[sd];
                //    var srcCode = classCode[sd];
                //    {
                //        srcCode.codePrefix = classSettings.codePrefix.Replace(Tag.srcClass, srcDef.className);
                //        srcCode.codePostfix = classSettings.codePostfix.Replace(Tag.srcClass, srcDef.className);
                //    }

                //    //Build the slot datas
                //    classSettings.GetSlottedCode(srcCode, srcDef);

                //    var keys = new String[srcCode.codeLines.Keys.Count];
                //    srcCode.codeLines.Keys.CopyTo(keys, 0);
                //    Array.Sort(keys, (a, b) =>
                //    {
                //        int nA = 0;
                //        for (int c = 0; c < a.Length; c++)
                //        {
                //            if (a[c] == 'n')
                //            {
                //                nA++;
                //            }
                //        }

                //        int nB = 0;
                //        for (int c = 0; c < b.Length; c++)
                //        {
                //            if (b[c] == 'n')
                //            {
                //                nB++;
                //            }
                //        }

                //        if (nA != nB)
                //        {
                //            return nA - nB;
                //        }
                //        else if (a.Length != b.Length)
                //        {
                //            return a.Length - b.Length;
                //        }

                //        return string.Compare(a, b);
                //    });

                //    finalCode += srcCode.codePrefix.NewLine();
                //    foreach (var key in keys)
                //    {
                //        var realKeys = key.Split(Strings.Separator.TextParse.C(), StringSplitOptions.RemoveEmptyEntries);
                //        var realKey = string.Empty;
                //        bool hasNonN = false;
                //        for (int k = 0; k < realKeys.Length; k++)
                //        {
                //            var u = realKeys[k].IndexOf(Strings.Separator.Parenthesis.C()[0]);
                //            realKey += u == -1 ? realKeys[k] : realKeys[k].Substring(0, u);

                //            if (realKeys.Length != 1 || realKeys[k] != "n")
                //            {
                //                hasNonN = true;
                //            }
                //        }

                //        if (!hasNonN)
                //            continue;

                //        var value = srcCode.codeLines[key];

                //        var code = classSettings.codeSource;

                //        code = code.Replace(Tag.srcClass, srcDef.className);
                //        code = code.Replace(Tag.dstClass, value);
                //        code = code.Replace(Tag.swizzleCall, realKey);

                //        var vars = "";
                //        var args = Tag.codeArgsV;
                //        var argsCount = 0;
                //        for (int k = 0; k < realKeys.Length; k++)
                //        {
                //            if (k != 0)
                //            {
                //                vars += Tag.codeArgs;
                //            }

                //            var c = realKeys[k];
                //            if (c == "n")
                //            {
                //                args += string.Format(Tag.codeArgs, classSettings.codeType, argsCount, classSettings.codeDefault);
                //                vars += string.Format(Tag.codeVarsN, argsCount);
                //                argsCount++;
                //            }
                //            else
                //            {
                //                vars += string.Format(Tag.codeVarsV, c);
                //            }
                //        }

                //        code = code.Replace(classSettings.codeArgs, args);
                //        code = code.Replace(classSettings.codeVars, vars);
                //        code = classSettings.ReplaceAdditional(code);

                //        finalCode += code.NewLine();
                //    }
                //    finalCode += srcCode.codePostfix.NewLine();
                //}

                //sourceLines.Insert(l, finalCode);
            }

            var fileCode = string.Empty;
            for (int l = 0; l < sourceLines.Count; l++)
            {
                fileCode += sourceLines[l].NewLine();
            }

            var dirPath = string.Empty;
            for (int d = 0; d < sourceDirectories.Count; d++)
            {
                var dir = sourceDirectories[d];
                if (!path.Contains(dir))
                    continue;

                dirPath = path.Replace(dir, "");
            }

            var c0 = Strings.Separator.Directory.C()[0];
            var c1 = Strings.Separator.Directory.C()[1];
            var s0 = Strings.Separator.Directory.C()[0].ToString();
            var s1 = Strings.Separator.Directory.C()[1].ToString();

            var index = dirPath.LastIndexOf(c0);
            if (index == -1)
            {
                index = dirPath.LastIndexOf(c1);
            }
            var filePath = dirPath.Substring(index + 1, dirPath.Length - (index + 1));
            dirPath = dirPath.Substring(0, index + 1);

            var finalPath = destinationDirectory;
            var endSep = finalPath.Contains(s0) ? s0 : s1;
            if (!finalPath.EndsWith(s0) && !finalPath.EndsWith(s1))
            {
                finalPath += endSep;
            }

            var dirs = dirPath.Split(Strings.Separator.Directory.C(), StringSplitOptions.RemoveEmptyEntries);
            for (int d = -1; d < dirs.Length; d++)
            {
                if (d >= 0)
                {
                    finalPath += dirs[d] + endSep;
                }

                if (!Directory.Exists(finalPath))
                {
                    Directory.CreateDirectory(finalPath);
                }
            }

            var extension = Strings.Separator.FileExtension.C()[0] + Tag.sourceExtension;
            finalPath += filePath;
            finalPath = finalPath.Replace(extension, "");
            File.WriteAllText(finalPath, fileCode);
#endif //
            return true;
        }

        //---------------------------------------------------------------------
        [InitializeOnLoad]
        public partial class SwizzleSettings : CodeSettings
        {
            //-----------------------------------------------------------------
            public struct Variant
            {
                public string call;
                public string args;
                public string vars;

                public Variant(string value)
                {
                    call = value;
                    args = value;
                    vars = value;
                }
            }

            //-----------------------------------------------------------------
            public override string ScopeTag { get { return "SWIZZLE"; } }

            //-----------------------------------------------------------------
            public override bool TreatData(CodeFile codeFile, Tag.Keyword setup, List<string> args, string data)
            {
                var activeData = codeFile.ActiveData;
                if (activeData == null)
                {
                    activeData = codeFile.NewData(this);
                }

                if (activeData.settings == null || activeData.settings != this)
                    return false;

                if (setup.keyword == CodeBlock)
                {
                    activeData.blockName = args[0];
                }
                else if (setup.keyword == Tag.Macro.OperationClass)
                {
                    activeData.classInfos.Add(new CodeFile.Data.ClassInfo()
                    {
                        name = args[0],
                        variables = args.GetRange(1, args.Count - 1)
                    });
                }
                else if (setup.keyword == Tag.Macro.TypeInfo)
                {
                    activeData.classContentType = args[0];
                    activeData.classContentValue = args[1];
                }
                else if (setup.keyword == Tag.Macro.CodePartPrefix)
                {
                    activeData.codePrefix = data;
                }
                else if (setup.keyword == Tag.Macro.CodePartMain)
                {
                    activeData.codeMain = data;
                }
                else if (setup.keyword == Tag.Macro.CodePartSuffix)
                {
                    activeData.codePostfix = data;
                }
                return true;
            }

            //-----------------------------------------------------------------
            public override void Generate(CodeFile.Data data)
            {
                var variants = new List<Variant>();
                for (int iSrc = 0; iSrc < data.classInfos.Count; iSrc++)
                {
                    var infoSrc = data.classInfos[iSrc];
                    for (int iSDst = 0; iSDst < data.classInfos.Count; iSDst++)
                    {
                        var infoDst = data.classInfos[iSDst];

                        GatherVariants(variants, data, infoSrc, infoDst);

                        var swapSrc = ClassSrc + infoSrc.name;
                        var swapDst = ClassDst + infoDst.name;
                        AddCode(data.codePrefix, data, swapSrc, swapDst);
                        for (int v = 0; v < variants.Count; v++)
                        {
                            var variant = variants[v];
                            var code = data.codeMain;
                            code = (CodeCall + variant.call).Apply(code);
                            code = (CodeArgs + variant.args).Apply(code);
                            code = (CodeVars + variant.vars).Apply(code);
                            AddCode(code, data, swapSrc, swapDst);
                        }
                        AddCode(data.codePostfix, data, swapSrc, swapDst);
                    }
                }

                data.codeGenerated = data.codeGenerated.Replace(Strings.NewLine(String.Empty), Strings.NewLine(String.Empty) + Tag.Macro.codeGenTabs.Keyword());
            }

            //-----------------------------------------------------------------
            private void AddCode(string code, CodeFile.Data data, Tag.SwapInfo swapSrc, Tag.SwapInfo swapDst)
            {
                code = swapSrc.Apply(code);
                code = swapDst.Apply(code);
                data.codeGenerated += code;
            }

            //-----------------------------------------------------------------
            private void GatherVariants(List<Variant> variants, CodeFile.Data data, CodeFile.Data.ClassInfo infoSrc, CodeFile.Data.ClassInfo infoDst)
            {
                var slots = new int[infoDst.variables.Count];
                for (int s = 0; s < slots.Length; s++)
                {
                    slots[s] = 0;
                }

                variants.Clear();
                GatherVariantsSlots(0, slots, variants, data, infoSrc, infoDst);
            }

            //-----------------------------------------------------------------
            private void GatherVariantsSlots(int s, int[] slots, List<Variant> variants, CodeFile.Data data, CodeFile.Data.ClassInfo infoSrc, CodeFile.Data.ClassInfo infoDst)
            {
                var varCount = infoSrc.variables.Count + 1;
                for (int c = 0; c < varCount; c++)
                {
                    slots[s] = c;
                    if (s + 1 < slots.Length)
                    {
                        GatherVariantsSlots(s + 1, slots, variants, data, infoSrc, infoDst);
                    }
                    else
                    {
                        var sn = 0;
                        var variant = new Variant(string.Empty);
                        variant.args += Tag.Code.argsV;
                        for (int v = 0; v < slots.Length; v++)
                        {
                            var sv = slots[v];
                            if (variant.vars.Length > 0)
                                variant.vars += Tag.Code.argVarSeparator;

                            if (sv < infoSrc.variables.Count)
                            {
                                var variable = infoSrc.variables[sv];
                                variant.call += variable;
                                variant.vars += string.Format(Tag.Code.varsV, variable);
                            }
                            else
                            {
                                variant.call += Tag.Code.callN;
                                variant.args += string.Format(Tag.Code.argsN, data.classContentType, sn, data.classContentValue);
                                variant.vars += string.Format(Tag.Code.varsN, sn);
                                sn++;
                            }
                        }

                        if (sn != slots.Length)
                        {
                            variants.Add(variant);
                        }
                    }
                }
            }



            public string argType;
            public string argDefault;

            //-----------------------------------------------------------------
            static SwizzleSettings()
            {
                CodeGenerator.Add(new SwizzleSettings(true));
                //codeCall = Tag.swizzleCall;
                //codeArgs = Tag.swizzleArgs;
                //codeType = Tag.swizzleType;
                //codeDefault = Tag.swizzleDefault;
                //codeVars = Tag.swizzleVars;
            }

            //-----------------------------------------------------------------
            private SwizzleSettings(bool doAccess) : base()
            {
                //codeCall = Tag.swizzleCall;
                //codeArgs = Tag.swizzleArgs;
                //codeType = Tag.swizzleType;
                //codeDefault = Tag.swizzleDefault;
                //codeVars = Tag.swizzleVars;
            }

            //-----------------------------------------------------------------
            public override string ReplaceAdditional(string code)
            {
                return base.ReplaceAdditional(code.Replace(codeType, argType).Replace(codeDefault, argDefault));
            }
        }
#endregion Unity Defaults
    }
}
