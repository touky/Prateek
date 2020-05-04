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
namespace Prateek.CodeGeneration.CodeBuilder.Editor.ScriptTemplates
{
    using System;
    using Prateek.CodeGeneration.CodeBuilder.Editor.CodeBuilder;

    ///-------------------------------------------------------------------------
    public static class TemplateHelpers
    {
        ///---------------------------------------------------------------------
        public static IgnorableContent GatherValidIgnorables(string fileContent, string fileExtension)
        { return GatherValidIgnorables(String.Empty, fileContent, fileExtension); }
        ///---------------------------------------------------------------------
        public static IgnorableContent GatherValidIgnorables(string fileName, string fileContent, string fileExtension)
        {
            var results = default(IgnorableContent);
            var ignorables = TemplateRegistry.Ignorables;
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

        ///---------------------------------------------------------------------
        public static void ApplyKeywords(ref string fileContent, string fileExtension)
        { ApplyKeywords(ref fileContent, string.Empty, fileExtension); }
        ///---------------------------------------------------------------------
        public static void ApplyKeywords(ref string fileContent, string fileName, string fileExtension)
        {
            var keywords = TemplateRegistry.Keywords;
            var doAnotherPass = true;
            while (doAnotherPass)
            {
                doAnotherPass = false;
                var ignorers = GatherValidIgnorables(fileContent, fileExtension);
                var stack = new KeywordTemplateStack(KeywordTemplateMode.UsedAsSwap, fileContent);
                for (int r = 0; r < keywords.Count; r++)
                {
                    var keyword = keywords[r];
                    var tag = keyword.Tag;
                    if (!keyword.Match(fileName, fileExtension, fileContent))
                        continue;

                    var start = 0;
                    while ((start = fileContent.IndexOf(tag, start)) >= 0)
                    {
                        var safety = ignorers.AdvanceToSafety(start, IgnorableStyle.Text);
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
