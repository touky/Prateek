// -BEGIN_PRATEEK_COPYRIGHT-
//
//  Prateek, a library that is "bien pratique"
//  Header last update date: 16/04/2019
//
//  Copyright � 2017-2019 "Touky" <touky@prateek.top>
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
//-----------------------------------------------------------------------------
#region C# Prateek Namespaces

//Auto activate some of the prateek defines
#if UNITY_EDITOR

#if !PRATEEK_DEBUG
#define PRATEEK_DEBUG
#endif //!PRATEEK_DEBUG

#endif //UNITY_EDITOR && !PRATEEK_DEBUG

//-----------------------------------------------------------------------------
#region System
using System;
using System.Collections;
using System.Collections.Generic;
#endregion System

//-----------------------------------------------------------------------------
#region Unity
using Unity.Jobs;
using Unity.Collections;

#region Engine
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.Serialization;

//-----------------------------------------------------------------------------
#if UNITY_PROFILING
using UnityEngine.Profiling;
#endif //UNITY_PROFILING

#endregion Engine

//-----------------------------------------------------------------------------
#if UNITY_EDITOR
using UnityEditor;
#endif //UNITY_EDITOR

#endregion Unity

//-----------------------------------------------------------------------------
#region Prateek
using Prateek;
using Prateek.Base;
using Prateek.Extensions;
using Prateek.Helpers;
using Prateek.Attributes;
using Prateek.Manager;

//-----------------------------------------------------------------------------
#region Using static
using static Prateek.ShaderTo.CSharp;
#endregion Using static

//-----------------------------------------------------------------------------
#if UNITY_EDITOR
using Prateek.CodeGeneration;
#endif //UNITY_EDITOR

//-----------------------------------------------------------------------------
#if PRATEEK_DEBUG
using Prateek.Debug;
using static Prateek.Debug.DebugDraw.DebugStyle.QuickCTor;
using DebugDraw = Prateek.Debug.DebugDraw;
using DebugPlace = Prateek.Debug.DebugDraw.DebugPlace;
using DebugStyle = Prateek.Debug.DebugDraw.DebugStyle;
#endif //PRATEEK_DEBUG

#endregion Prateek

#endregion C# Prateek Namespaces
//
// -END_PRATEEK_CSHARP_NAMESPACE-

//-----------------------------------------------------------------------------
#region File namespaces
using System.IO;
using System.Text.RegularExpressions;

using Prateek.IO;
using Prateek.CodeGeneration;
#endregion File namespaces

namespace Prateek.CodeGeneration.Editors
{
    //-------------------------------------------------------------------------
    public static class Tools
    {
        //---------------------------------------------------------------------
        internal sealed class ScriptKeywordProcessor : UnityEditor.AssetModificationProcessor
        {
            //-----------------------------------------------------------------
            public static void OnWillCreateAsset(string path)
            {
                path = path.Replace(".meta", string.Empty);
                int index = path.LastIndexOf(Strings.Separator.FileExtension.C());
                if (index < 0)
                    return;

                var builder = new CodeBuilder();
                if (!Regex.Match(path, builder.SearchPattern).Success)
                    return;

                builder.AddFile(new CodeBuilder.FileData(path, string.Empty));

                builder.Init();
                builder.StartWork(true);
            }
        }

        //---------------------------------------------------------------------
        public static CodeBuilder GetScriptTemplateUpdater(string sourceDir = "/Scripts")
        {
            var path = Application.dataPath + sourceDir;
            if (!Directory.Exists(path))
                return null;

            var builder = new CodeBuilder();

            builder.AddDirectory(path);

            builder.Operations = CodeBuilder.OperationApplied.ALL & ~CodeBuilder.OperationApplied.ApplyScriptTemplate;

            return builder;
        }

        //---------------------------------------------------------------------
#if PRATEEK_ALLOW_INTERNAL_TOOLS
        public static CodeBuilder GetPrateekScriptGenerator(string destinationDirectory, List<string> sourceDirectories)
        {
            var builder = new PrateekScriptBuilder();

            builder.AddDirectories(sourceDirectories);
            builder.DestinationDirectory = destinationDirectory;

            return builder;
        }
#endif //PRATEEK_ALLOW_INTERNAL_TOOLS
    }
}
