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
using Prateek.IO;
using System.Text.RegularExpressions;
#endregion File namespaces

namespace ScriptTemplating
{
    //-------------------------------------------------------------------------
    internal sealed class ScriptKeywordProcessor : UnityEditor.AssetModificationProcessor
    {
        //---------------------------------------------------------------------
        private static List<Replacement.Script> script = new List<Replacement.Script>();
        private static List<Replacement.Keyword> keywords = new List<Replacement.Keyword>();
        private static List<Replacement.Ignorable> gnorables = new List<Replacement.Ignorable>();

        //---------------------------------------------------------------------
        public static void Add(Replacement.Script data) { script.Add(data); }
        public static void Add(Replacement.Keyword data) { keywords.Add(data); }
        public static void Add(Replacement.Ignorable data) { gnorables.Add(data); }

        //---------------------------------------------------------------------
        public static void OnWillCreateAsset(string path)
        {
            path = path.Replace(".meta", "");
            int index = path.LastIndexOf(".");
            if (index < 0)
                return;

            var fileExtension = path.Substring(index + 1);
            var slashIdx = Mathf.Max(0, path.LastIndexOf("/"));
            var fileName = path.Substring(slashIdx + 1, index - (slashIdx + 1));

            index = Application.dataPath.LastIndexOf("Assets");
            path = Application.dataPath.Substring(0, index) + path;
            if (!System.IO.File.Exists(path))
                return;

            var originalContent = FileHelpers.ReadAllTextCleaned(path);
            if (originalContent == string.Empty)
                return;

            var fileContent = string.Empty;
            //Look for the correct script remplacement
            for (int r = 0; r < script.Count; r++)
            {
                var replacement = script[r];
                if (replacement.Match(fileExtension, originalContent))
                {
                    fileContent = replacement.Content.CleanText();
                    break;
                }

                if (fileContent != string.Empty)
                    break;
            }

            if (fileContent == string.Empty)
                return;

            fileContent = fileContent.Replace("#SCRIPTNAME#", fileName);
            ApplyKeywords(ref fileContent, fileExtension);

            System.IO.File.WriteAllText(path, fileContent.ApplyCRLF());
            AssetDatabase.Refresh();
        }

        //---------------------------------------------------------------------
        private static Replacement.Ignorable.BuildResult GatherValidIgnorables(string fileContent, string fileExtension)
        {
            var results = new Replacement.Ignorable.BuildResult();
            for (int i = 0; i < gnorables.Count; i++)
            {
                var ignorable = gnorables[i];
                if (!ignorable.Match(fileExtension, fileContent))
                    continue;

                var result = ignorable.Build(fileContent);
                if (!result.IsValid)
                    continue;

                if (!results.Merge(result))
                    return new Replacement.Ignorable.BuildResult();
            }

            return results;
        }

        //---------------------------------------------------------------------
        public static void ApplyKeywords(ref string fileContent, string fileExtension)
        {
            var ignorers = GatherValidIgnorables(fileContent, fileExtension);
            for (int r = 0; r < keywords.Count; r++)
            {
                var replacement = keywords[r];
                var keyword = replacement.Tag;
                if (!replacement.Match(fileExtension, fileContent))
                    continue;

                var start = 0;
                while ((start = fileContent.IndexOf(keyword, start)) >= 0)
                {
                    var safety = ignorers.AdvanceToSafety(start, Replacement.Ignorable.Extent.StyleType.Text);
                    if (safety != start)
                    {
                        start = safety;
                        continue;
                    }

                    fileContent = fileContent.Substring(0, start)
                                + replacement.Content.CleanText()
                                + fileContent.Substring(start + keyword.Length);
                    r = -1;

                    start += keyword.Length;
                }
            }
        }

        //---------------------------------------------------------------------
#if PRATEEK_ALLOW_INTERNAL_TOOLS
        [MenuItem("Prateek/Internal/Update prateek templates")]
        private static void UpdateTemplate()
        {
            var path = Application.dataPath;
            if (!Directory.Exists(path))
                return;

            var files = new List<string>();
            FileHelpers.GatherFilesAt(path, files, FileHelpers.BuildExtensionMatch(keywords), true);

            for (int f = 0; f < files.Count; f++)
            {
                var file = files[f];
                var fileExtension = file.Substring(file.LastIndexOf(".") + 1);
                var fileContent = FileHelpers.ReadAllTextCleaned(file);
                var ignorers = GatherValidIgnorables(fileContent, fileExtension);
                for (int r = 0; r < keywords.Count; r++)
                {
                    var replacement = keywords[r];
                    if (!replacement.Match(fileExtension, fileContent))
                        continue;

                    var start = 0;
                    while ((start = fileContent.IndexOf(replacement.TagBegin, start)) >= 0)
                    {
                        var safety = ignorers.AdvanceToSafety(start, Replacement.Ignorable.Extent.StyleType.Text);
                        if (safety != start)
                        {
                            start = safety;
                            continue;
                        }

                        var tagEnd = replacement.TagEnd;
                        var end = fileContent.IndexOf(tagEnd, start);
                        if (end < 0)
                            break;

                        end += tagEnd.Length;
                        fileContent = fileContent.Substring(0, start)
                                    + replacement.Content.CleanText()
                                    + fileContent.Substring(end);

                        start = end;
                    }
                }

                ApplyKeywords(ref fileContent, fileExtension);

                File.WriteAllText(file, fileContent);
            }

            AssetDatabase.Refresh();
        }
#endif //PRATEEK_ALLOW_INTERNAL_TOOLS
    }
}
