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
using System.Text.RegularExpressions;

using Prateek.IO;
using Prateek.ScriptTemplating;
#endregion File namespaces

namespace Prateek.ScriptTemplating
{
    //-------------------------------------------------------------------------
    public static partial class TemplateTools
    {
        //---------------------------------------------------------------------
#if PRATEEK_ALLOW_INTERNAL_TOOLS
        [MenuItem("Prateek/Internal/Update prateek templates")]
        private static void UpdateTemplate()
        {
            //EditorApplication.applicationPath
            var path = Application.dataPath;
            if (!Directory.Exists(path))
                return;

            var files = new List<string>();
            var keywords = TemplateReplacement.Keywords;
            FileHelpers.GatherFilesAt(path, files, FileHelpers.BuildExtensionMatch(keywords.List), true);

            for (int f = 0; f < files.Count; f++)
            {
                var file = files[f];
                var fileExtension = file.Substring(file.LastIndexOf(".") + 1);
                var fileContent = FileHelpers.ReadAllTextCleaned(file);
                var ignorers = TemplateHelpers.GatherValidIgnorables(fileContent, fileExtension);
                var stack = new TemplateReplacement.KeywordStack(TemplateReplacement.KeywordMode.ZoneDelimiter, fileContent);

                for (int r = 0; r < keywords.Count; r++)
                {
                    var keyword = keywords[r];
                    if (!keyword.Match(fileExtension, fileContent))
                        continue;

                    if (keyword.Mode == TemplateReplacement.KeywordMode.KeywordOnly)
                        continue;

                    var start = 0;
                    while ((start = fileContent.IndexOf(keyword.TagBegin, start)) >= 0)
                    {
                        var safety = ignorers.AdvanceToSafety(start, TemplateReplacement.Ignorable.Style.Text);
                        if (safety != start)
                        {
                            start = safety;
                            continue;
                        }

                        var tagEnd = keyword.TagEnd;
                        var end = fileContent.IndexOf(tagEnd, start);
                        if (end < 0)
                            break;

                        end += tagEnd.Length;

                        stack.Add(keyword, start, end);

                        start = end;
                    }
                }

                fileContent = stack.Apply();

                TemplateHelpers.ApplyKeywords(ref fileContent, fileExtension);

                File.WriteAllText(file + ".txt", fileContent);
            }

            AssetDatabase.Refresh();
        }
#endif //PRATEEK_ALLOW_INTERNAL_TOOLS
    }
}
