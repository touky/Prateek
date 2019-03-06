// -BEGIN_PRATEEK_COPYRIGHT-
//
//  Prateek, a library that is "bien pratique"
//  Header last update date: 05/03/19
//
//  Copyright © 2017—2019 Benjamin "Touky" Huet <huet.benjamin@gmail.com>
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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine.Jobs;

#if UNITY_EDITOR
using UnityEditor;
#endif //UNITY_EDITOR

#if UNITY_PROFILING
using UnityEngine.Profiling;
#endif //UNITY_PROFILING

using Prateek;
using Prateek.Base;
using Prateek.Extensions;
using Prateek.Helpers;
using Prateek.Attributes;

#if PRATEEK_DEBUGS
using Prateek.Debug;
#endif //PRATEEK_DEBUG
#endregion C# Prateek Namespaces
//
// -END_PRATEEK_CSHARP_NAMESPACE-

//-----------------------------------------------------------------------------
#region File namespaces
using System.IO;
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
        public class CodeSettings
        {
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
            public CodeSettings() { }

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

            //-----------------------------------------------------------------
            public virtual void GetSlottedCode(ClassCode srcCode, CodeClasses srcDef)
            {
            }
        }

        //---------------------------------------------------------------------
        public class ClassCode
        {
            public string codePrefix;
            public Dictionary<string, string> codeLines = new Dictionary<string, string>();
            public string codePostfix;
        }

        //---------------------------------------------------------------------
        public static class Tag
        {
            //Operations main tags
            public enum Operations
            {
                PRTK_SWIZZLE_DEFINITION,

                MAX
            }

            //Default datas
            public const string sourceExtension = "prtk";
            public const string destinationExtension= "cs";

            public const string codeStart = "PRTK_CODEGEN_STARTS_HERE";
            public const string codeStop = "PRTK_CODEGEN_STOPS_HERE";

            public const string srcClass = "PRTK_SRC_CLASS";
            public const string dstClass = "PRTK_DST_CLASS";

            //Code generation data
            public const string codeVarsSeparator = ", ";
            public const string codeArgsV = "v";
            public const string codeArgs = ", {0} n_{1} = {2}";
            public const string codeVarsN = "n_{0}";
            public const string codeVarsV = "v.{0}";

            //Swizzle tags
            public const string swizzleInit = "PRTK_SWIZZLE_INIT";
            public const string swizzleCall = "PRTK_SWIZZLE_CALL";
            public const string swizzleArgs = "PRTK_SWIZZLE_ARGS";
            public const string swizzleType = "PRTK_SWIZZLE_TYPE";
            public const string swizzleDefault = "PRTK_SWIZZLE_DEFAULT";
            public const string swizzleVars = "PRTK_SWIZZLE_VARS";
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
        public ClassCode[] classCode;
        #endregion Fields

        //---------------------------------------------------------------------
        public partial class SwizzleSettings : CodeSettings
        {
            public string argType;
            public string argDefault;

            //-----------------------------------------------------------------
            public SwizzleSettings() : base()
            {
                codeCall = Tag.swizzleCall;
                codeArgs = Tag.swizzleArgs;
                codeType = Tag.swizzleType;
                codeDefault = Tag.swizzleDefault;
                codeVars = Tag.swizzleVars;
            }

            //-----------------------------------------------------------------
            public override bool Load(string line)
            {
                var isDefinition = line.Contains(Tag.Operations.PRTK_SWIZZLE_DEFINITION.ToString());
                var isTyping = line.Contains(Tag.swizzleInit);
                if (isDefinition || isTyping)
                {
                    var open = line.IndexOf(Strings.Separator.Parenthesis.C()[0]);
                    if (open == -1)
                        return false;

                    var close = line.LastIndexOf(Strings.Separator.Parenthesis.C()[1]);
                    if (close == -1)
                        return false;

                    if (open < close)
                    {
                        open += 1;
                        var subString = line.Substring(open, close - open);
                        var separators = (Strings.Separator.TextParse | Strings.Separator.Space).C();
                        var settings = new List<string>(subString.Split(separators, StringSplitOptions.RemoveEmptyEntries));

                        if (isDefinition)
                        {
                            if (settings.Count < 2)
                                return false;

                            codeClasses.Add(new CodeClasses()
                            {
                                className = settings.PopFirst(),
                                callComponents = settings.ToArray()
                            });
                            return true;
                        }
                        else if (isTyping)
                        {
                            if (settings.Count != 2)
                                return false;

                            argType = settings.PopFirst();
                            argDefault = settings.PopFirst();

                            return true;
                        }
                    }
                }
                return false;
            }

            //-----------------------------------------------------------------
            public override string ReplaceAdditional(string code)
            {
                return base.ReplaceAdditional(code.Replace(codeType, argType).Replace(codeDefault, argDefault));
            }

            //-----------------------------------------------------------------
            public override void GetSlottedCode(ClassCode srcCode, CodeClasses srcDef)
            {
                for (int dd = 0; dd < codeClasses.Count; dd++)
                {
                    var dstDef = codeClasses[dd];
                    var components = new String[srcDef.callComponents.Length + 1];
                    Array.Copy(srcDef.callComponents, components, srcDef.callComponents.Length);
                    components[components.Length - 1] = "n";

                    string[] slots = new string[dstDef.callComponents.Length];
                    FillSlot(slots, 0, srcCode, dstDef, components);
                }
            }
        }

        #region Vector to Vector
        //public CodeSettings vector2To4 = new CodeSettings()
        //{
        //    codeStart = "#region Swizzle #SRC_CLASS#\n",
        //    codeSwizzle = "public static #DST_CLASS# #SWIZZLE_CALL#(this #SRC_CLASS# v#SWIZZLE_ARGS#) { return new #DST_CLASS#(#SWIZZLE_VARS#); }\n",
        //    codeEnd = "#endregion Swizzle #SRC_CLASS#\n",
        //    codeVarsV = "v.{0}",

        //    codeClasses = new CodeSettings.CodeClasses[]
        //    {
        //        new CodeSettings.CodeClasses() { className = "Vector2", callComponents = new string[] {"x", "y" } },
        //        new CodeSettings.CodeClasses() { className = "Vector3", callComponents = new string[] {"x", "y", "z" } },
        //        new CodeSettings.CodeClasses() { className = "Vector4", callComponents = new string[] {"x", "y", "z", "w" } }
        //    },
        //};
        //#endregion Vector to vector

        ////---------------------------------------------------------------------
        //#region Color to Color
        //public CodeSettings color = new CodeSettings()
        //{
        //    codeStart = "#region Swizzle #SRC_CLASS#\n",
        //    codeSwizzle = "public static #DST_CLASS# #SWIZZLE_CALL#(this #SRC_CLASS# c#SWIZZLE_ARGS#) { return new #DST_CLASS#(#SWIZZLE_VARS#); }\n",
        //    codeEnd = "#endregion Swizzle #SRC_CLASS#\n",
        //    codeVarsV = "c.{0}",

        //    codeClasses = new CodeSettings.CodeClasses[]
        //    {
        //        new CodeSettings.CodeClasses()
        //        {
        //            className = "Color", callComponents = new string[] {"r", "g", "b", "a" },
        //        },
        //    },
        //};
        #endregion Color to Color

        //---------------------------------------------------------------------
        #region Unity Defaults
        [ContextMenu("Generate code")]
        public void StartGeneration()
        {
            if (!FindSources())
            {
                UnityEngine.Debug.LogError("Code generation failed");
            }
        }

        //---------------------------------------------------------------------
        private bool FindSources()
        {
            var foundFiles = new List<string>();
            var foundDirectories = new List<string>(sourceDirectories);
            for (int d = 0; d < foundDirectories.Count; d++)
            {
                var dirPath = foundDirectories[d];
                if (!Directory.Exists(dirPath))
                {
                    foundDirectories.RemoveAt(d--);
                    continue;
                }

                foundDirectories.AddRange(Directory.GetDirectories(dirPath));
            }

            for (int d = 0; d < foundDirectories.Count; d++)
            {
                var path = foundDirectories[d];
                foundFiles.AddRange(Directory.GetFiles(path, String.Format("*.{0}", Tag.sourceExtension)));
            }

            //Start the generating
            for (int f = 0; f < foundFiles.Count; f++)
            {
                var path = foundFiles[f];
                if (!File.Exists(path))
                    continue;

                if (!GenerateFile(path))
                    return false;
            }

            return true;
        }

        //---------------------------------------------------------------------
        private CodeSettings GetCodeSettings(string line)
        {
            for (int o = 0; o < (int)Tag.Operations.MAX; o++)
            {
                var operation = (Tag.Operations)o;
                if (line.Contains(operation.ToString()))
                {
                    switch (operation)
                    {
                        case Tag.Operations.PRTK_SWIZZLE_DEFINITION: { return new SwizzleSettings(); }
                    }
                }
            }
            return null;
        }

        //---------------------------------------------------------------------
        private bool GenerateFile(string path)
        {
            var sourceLines = new List<string>(File.ReadAllText(path).TabToSpaces().SplitLines());
            var classContents = new List<string>();
            var classSettings = (CodeSettings)null;

            //Go through the content and search for any codegen tag
            for (int l = 0; l < sourceLines.Count; l++)
            {
                var line = sourceLines[l];
                if (!line.Contains(Tag.codeStart))
                    continue;

                var codePrefix = string.Empty;
                var codeSource = string.Empty;
                var codePostfix = string.Empty;

                var reachedCall = false;
                sourceLines.RemoveAt(l);
                while (l < sourceLines.Count)
                {
                    line = sourceLines[l];
                    //Retrieve potential code settings and load them
                    {
                        if (classSettings == null)
                        {
                            classSettings = GetCodeSettings(line);
                        }

                        if (classSettings != null && classSettings.Load(line))
                        {
                            sourceLines.RemoveAt(l);
                            continue;
                        }
                    }

                    //OR fill the code call
                    if (classSettings != null && line.Contains(classSettings.codeCall))
                    {
                        reachedCall = true;
                        codeSource = line;
                        sourceLines.RemoveAt(l);
                        continue;
                    }

                    //Stop the search if we hit the stop tag
                    if (line.Contains(Tag.codeStop))
                    {
                        sourceLines.RemoveAt(l);
                        break;
                    }

                    //Or if nothing, fill the code prefix/postfix
                    if (reachedCall)
                    {
                        codePostfix += line.NewLine();
                    }
                    else
                    {
                        codePrefix += line.NewLine();
                    }

                    sourceLines.RemoveAt(l);
                }

                if (classSettings == null)
                    continue;

                classSettings.codePrefix = codePrefix;
                classSettings.codeSource = codeSource;
                classSettings.codePostfix = codePostfix;

                //Prepare class codes
                classCode = new ClassCode[classSettings.codeClasses.Count];
                for (int cc = 0; cc < classSettings.codeClasses.Count; cc++)
                {
                    classCode[cc] = new ClassCode();
                }

                //Fill all the slots datas
                var finalCode = string.Empty;
                for (int sd = 0; sd < classSettings.codeClasses.Count; sd++)
                {
                    //Load up the pre/post-fix
                    var srcDef = classSettings.codeClasses[sd];
                    var srcCode = classCode[sd];
                    {
                        srcCode.codePrefix = classSettings.codePrefix.Replace(Tag.srcClass, srcDef.className);
                        srcCode.codePostfix = classSettings.codePostfix.Replace(Tag.srcClass, srcDef.className);
                    }

                    //Build the slot datas
                    classSettings.GetSlottedCode(srcCode, srcDef);

                    var keys = new String[srcCode.codeLines.Keys.Count];
                    srcCode.codeLines.Keys.CopyTo(keys, 0);
                    Array.Sort(keys, (a, b) =>
                    {
                        int nA = 0;
                        for (int c = 0; c < a.Length; c++)
                        {
                            if (a[c] == 'n')
                            {
                                nA++;
                            }
                        }

                        int nB = 0;
                        for (int c = 0; c < b.Length; c++)
                        {
                            if (b[c] == 'n')
                            {
                                nB++;
                            }
                        }

                        if (nA != nB)
                        {
                            return nA - nB;
                        }
                        else if (a.Length != b.Length)
                        {
                            return a.Length - b.Length;
                        }

                        return string.Compare(a, b);
                    });

                    finalCode += srcCode.codePrefix.NewLine();
                    foreach (var key in keys)
                    {
                        var realKeys = key.Split(Strings.Separator.TextParse.C(), StringSplitOptions.RemoveEmptyEntries);
                        var realKey = string.Empty;
                        bool hasNonN = false;
                        for (int k = 0; k < realKeys.Length; k++)
                        {
                            var u = realKeys[k].IndexOf(Strings.Separator.Parenthesis.C()[0]);
                            realKey += u == -1 ? realKeys[k] : realKeys[k].Substring(0, u);

                            if (realKeys.Length != 1 || realKeys[k] != "n")
                            {
                                hasNonN = true;
                            }
                        }

                        if (!hasNonN)
                            continue;

                        var value = srcCode.codeLines[key];

                        var code = classSettings.codeSource;

                        code = code.Replace(Tag.srcClass, srcDef.className);
                        code = code.Replace(Tag.dstClass, value);
                        code = code.Replace(Tag.swizzleCall, realKey);

                        var vars = "";
                        var args = Tag.codeArgsV;
                        var argsCount = 0;
                        for (int k = 0; k < realKeys.Length; k++)
                        {
                            if (k != 0)
                            {
                                vars += Tag.codeVarsSeparator;
                            }

                            var c = realKeys[k];
                            if (c == "n")
                            {
                                args += string.Format(Tag.codeArgs, classSettings.codeType, argsCount, classSettings.codeDefault);
                                vars += string.Format(Tag.codeVarsN, argsCount);
                                argsCount++;
                            }
                            else
                            {
                                vars += string.Format(Tag.codeVarsV, c);
                            }
                        }

                        code = code.Replace(classSettings.codeArgs, args);
                        code = code.Replace(classSettings.codeVars, vars);
                        code = classSettings.ReplaceAdditional(code);

                        finalCode += code.NewLine();
                    }
                    finalCode += srcCode.codePostfix.NewLine();
                }

                sourceLines.Insert(l, finalCode);
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
            return true;
        }

        //---------------------------------------------------------------------
        public partial class SwizzleSettings : CodeSettings
        {
            private void FillSlot(string[] slots, int index, ClassCode classCode, CodeSettings.CodeClasses codeDef, string[] components)
            {
                //Go through components
                for (int c = 0; c < components.Length; c++)
                {
                    slots[index] = components[c];
                    if (index + 1 < slots.Length)
                    {
                        FillSlot(slots, index + 1, classCode, codeDef, components);
                    }
                    else
                    {
                        var swizzle = string.Empty;
                        for (int s = 0; s < slots.Length; s++)
                        {
                            swizzle += (s > 0 ? ";" : "") + slots[s];
                        }

                        if (!classCode.codeLines.ContainsKey(swizzle))
                        {
                            classCode.codeLines.Add(swizzle, codeDef.className);
                        }
                    }
                }
            }
        }
        #endregion Unity Defaults
    }
}