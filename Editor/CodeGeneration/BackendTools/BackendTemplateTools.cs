#define PRATEEK_ALLOW_INTERNAL_TOOLS
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
namespace Prateek.Editor.CodeGeneration.BackendTools
{
    using System.IO;
    using System.Text.RegularExpressions;
    using Prateek.Editor.CodeGeneration.CodeBuilder.RuntimeBuilder;
    using Prateek.Editor.CodeGeneration.PrateekScript;
    using Prateek.Runtime.Core.Helpers;
    using UnityEngine;

    ///-------------------------------------------------------------------------
    public static class Tools
    {
        ///---------------------------------------------------------------------
        internal sealed class ScriptKeywordProcessor : UnityEditor.AssetModificationProcessor
        {
            ///-----------------------------------------------------------------
            public static void OnWillCreateAsset(string path)
            {
                path = path.RemoveExtension(".meta");
                int index = path.LastIndexOf(Strings.Separator.FileExtension.C());
                if (index < 0)
                    return;

                var builder = new CodeBuilder();
                if (!Regex.Match(path, builder.SearchPattern).Success)
                    return;

                builder.AddFile(new FileData(path, string.Empty));

                builder.Init();
                builder.StartWork(true);
            }
        }

        ///---------------------------------------------------------------------
        public static CodeBuilder GetScriptTemplateUpdater(string sourceDir = "/Scripts")
        {
            var path = Application.dataPath + sourceDir;
            if (!Directory.Exists(path))
                return null;

            var builder = new CodeBuilder();

            builder.AddDirectory(path);

            builder.Operations = OperationApplied.ALL & ~OperationApplied.ApplyScriptTemplate;

            return builder;
        }

        ///---------------------------------------------------------------------
#if PRATEEK_ALLOW_INTERNAL_TOOLS
        public static CodeBuilder GetPrateekScriptGenerator()
        {
            var builder = new PrateekScriptBuilder();
            //builder.AddFile();
            //builder.AddDirectories(sourceDirectories);
            //builder.DestinationDirectory = destinationDirectory;

            return builder;
        }
#endif //PRATEEK_ALLOW_INTERNAL_TOOLS
    }
}
