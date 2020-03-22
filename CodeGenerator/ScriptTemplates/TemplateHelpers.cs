// -BEGIN_PRATEEK_COPYRIGHT-// -BEGIN_PRATEEK_COPYRIGHT-
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
// -END_PRATEEK_COPYRIGHT-// -END_PRATEEK_COPYRIGHT-// -END_PRATEEK_COPYRIGHT-

// -BEGIN_PRATEEK_CSHARP_NAMESPACE-// -BEGIN_PRATEEK_CSHARP_NAMESPACE-
//
//-----------------------------------------------------------------------------
#region C# Prateek Namespaces

//Auto activate some of the prateek defines
#if UNITY_EDITOR

#if !PRATEEK_DEBUG
#define PRATEEK_DEBUG
#endif //!PRATEEK_DEBUG

#endif //UNITY_EDITOR && !PRATEEK_DEBUG

#endregion C# Prateek Namespaces
//
// -END_PRATEEK_CSHARP_NAMESPACE-// -END_PRATEEK_CSHARP_NAMESPACE-// -END_PRATEEK_CSHARP_NAMESPACE-

//-----------------------------------------------------------------------------
#region File namespaces
using System.IO;
using Prateek.IO;
using System.Text.RegularExpressions;
#endregion File namespaces

//-----------------------------------------------------------------------------
namespace Prateek.CodeGeneration
{
    using System;

    //-------------------------------------------------------------------------
    public static class TemplateHelpers
    {
        //---------------------------------------------------------------------
        public static ScriptTemplate.Ignorable.BuildResult GatherValidIgnorables(string fileContent, string fileExtension)
        { return GatherValidIgnorables(String.Empty, fileContent, fileExtension); }
        //---------------------------------------------------------------------
        public static ScriptTemplate.Ignorable.BuildResult GatherValidIgnorables(string fileName, string fileContent, string fileExtension)
        {
            var results = default(ScriptTemplate.Ignorable.BuildResult);
            var ignorables = ScriptTemplate.Ignorables;
            for (int i = 0; i < ignorables.Count; i++)
            {
                var ignorable = ignorables[i];
                if (!ignorable.Match(fileExtension, fileExtension, fileContent))
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
        { ApplyKeywords(ref fileContent, string.Empty, fileExtension); }
        //---------------------------------------------------------------------
        public static void ApplyKeywords(ref string fileContent, string fileName, string fileExtension)
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
                    if (!keyword.Match(fileName, fileExtension, fileContent))
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
