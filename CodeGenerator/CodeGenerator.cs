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
    //-------------------------------------------------------------------------
    [CreateAssetMenu(menuName = "Prateek/New CodeGenerator", fileName = "NewCodeGenerator")]
    public class CodeGenerator : ScriptableObject
    {
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
            Code.Tag.Macro.Init(); //TODO Auto load

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

                FileHelpers.GatherFilesAt(sourceDirectories[d], files, FileHelpers.BuildExtensionMatch(Code.Tag.sourceExtension), true);
            }
            return files.Count > 0;
        }

        //---------------------------------------------------------------------
        private bool GenerateFile(string path)
        {
            var sources = FileHelpers.ReadAllTextCleaned(path);
            if (sources == string.Empty)
                return false;

            var analyzer = new Code.Analyzer();
            var activeCodeFile = (Code.File)null;
            var codeFiles = new List<Code.File>();
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
                        if (keyword == Code.Tag.Macro.FileInfo)
                        {
                            var setup = new Code.Tag.Keyword(keyword, true) { minArgCount = 2, maxArgCount = 2, needOpenScope = true };
                            if (!analyzer.FindArgs(args, setup))
                                break;

                            activeCodeFile = codeFiles.Find((x) => { return x.fileName == args[0] && x.fileExtension == args[1]; });
                            if (activeCodeFile == null)
                            {
                                activeCodeFile = new Code.File() { fileName = args[0], fileExtension = args[1] };
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
                            if (setup.usage != Code.Tag.Keyword.Usage.Match)
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
                            else if (codeDepth == 1 && scopeName == Code.Tag.Macro.FileInfo)
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

                    File.WriteAllText(destinationDirectory + codeFile.fileName + ".txt", codeFile.CodeGenerated.ApplyCRLF());

                    for (int i = 0; i < codeFile.DataCount; i++)
                    {
                        var code = codeFile[i];
                        UnityEngine.Debug.Log(code.codeGenerated);
                    }

                    UnityEngine.Debug.Log(codeFile.CodeGenerated);
                }
            }

            return true;
        }
#endregion Unity Defaults
    }
}
