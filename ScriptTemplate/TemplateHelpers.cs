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

//-----------------------------------------------------------------------------
namespace Prateek.ScriptTemplating
{
    //-------------------------------------------------------------------------
    public static class TemplateHelpers
    {
        //---------------------------------------------------------------------
        public static TemplateReplacement.Ignorable.BuildResult GatherValidIgnorables(string fileContent, string fileExtension)
        {
            var results = default(TemplateReplacement.Ignorable.BuildResult);
            var ignorables = TemplateReplacement.Ignorables;
            for (int i = 0; i < ignorables.Count; i++)
            {
                var ignorable = ignorables[i];
                if (!ignorable.Match(fileExtension, fileContent))
                    continue;

                var result = ignorable.Build(fileContent);
                if (!result.IsValid)
                    continue;

                if (!results.Merge(result))
                    return default;
            }

            return results;
        }

        //---------------------------------------------------------------------
        public static void ApplyKeywords(ref string fileContent, string fileExtension)
        {
            var ignorers = GatherValidIgnorables(fileContent, fileExtension);
            var keywords = TemplateReplacement.Keywords;
            for (int r = 0; r < keywords.Count; r++)
            {
                var container = keywords[r];
                var keyword = container.Tag;
                if (!container.Match(fileExtension, fileContent))
                    continue;

                var start = 0;
                while ((start = fileContent.IndexOf(keyword, start)) >= 0)
                {
                    var safety = ignorers.AdvanceToSafety(start, TemplateReplacement.Ignorable.Style.Text);
                    if (safety != start)
                    {
                        start = safety;
                        continue;
                    }

                    fileContent = fileContent.Substring(0, start)
                                + container.Content.CleanText()
                                + fileContent.Substring(start + keyword.Length);
                    r = -1;

                    start += keyword.Length;
                }
            }
        }
    }
}
