// -BEGIN_PRATEEK_COPYRIGHT-
//
//  Prateek, a library that is "bien pratique"
//  Header last update date: 24/03/2019
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
using Unity.Jobs;
using Unity.Collections;

#region Engine
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.Serialization;

#if UNITY_PROFILING
using UnityEngine.Profiling;
#endif //UNITY_PROFILING
#endregion Engine

#region Editor
#if UNITY_EDITOR
using UnityEditor;
#endif //UNITY_EDITOR
#endregion Editor
#endregion Unity

#region Prateek
using Prateek;
using Prateek.Base;
using Prateek.Extensions;
using Prateek.Helpers;
using Prateek.Attributes;

#region Using static
using static Prateek.ShaderTo.CSharp;
#endregion Using static

#region Editor
#if UNITY_EDITOR
using Prateek.CodeGeneration;
#endif //UNITY_EDITOR
#endregion Editor

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
namespace Prateek.CodeGeneration
{
    //-------------------------------------------------------------------------
    public static class TemplateHelpers
    {
        //---------------------------------------------------------------------
        public static ScriptTemplate.Ignorable.BuildResult GatherValidIgnorables(string fileContent, string fileExtension)
        {
            var results = default(ScriptTemplate.Ignorable.BuildResult);
            var ignorables = ScriptTemplate.Ignorables;
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
            var keywords = ScriptTemplate.Keywords;
            var doAnotherPass = true;
            while (doAnotherPass)
            {
                doAnotherPass = false;
                var ignorers = GatherValidIgnorables(fileContent, fileExtension);
                var stack = new ScriptTemplate.KeywordStack(ScriptTemplate.KeywordMode.KeywordOnly, fileContent);
                for (int r = 0; r < keywords.Count; r++)
                {
                    var keyword = keywords[r];
                    var tag = keyword.Tag;
                    if (!keyword.Match(fileExtension, fileContent))
                        continue;

                    var start = 0;
                    while ((start = fileContent.IndexOf(tag, start)) >= 0)
                    {
                        var safety = ignorers.AdvanceToSafety(start, ScriptTemplate.Ignorable.Style.Text);
                        if (safety != start)
                        {
                            start = safety;
                            continue;
                        }

                        var end = start + tag.Length;

                        doAnotherPass = true;
                        stack.Add(keyword, start, end);

                        start = end;
                    }
                }

                if (stack.CanApply)
                {
                    fileContent = stack.Apply();
                }
            }
        }
    }
}
