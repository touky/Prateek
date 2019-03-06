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

namespace ScriptTemplating
{
    //-------------------------------------------------------------------------
    public class ScriptTemplateReplacement
    {
        //---------------------------------------------------------------------
        public struct Script
        {
            public string extension;
            public List<string> matchingContents;
            public string content;

            //-----------------------------------------------------------------
            public Script SetMatching(params string[] matchingContents)
            {
                return new Script() { extension = this.extension, matchingContents = new List<string>(matchingContents) };
            }

            //-----------------------------------------------------------------
            public Script SetContent(params string[] content)
            {
                var data = new Script() { extension = this.extension, matchingContents = this.matchingContents };
                for (int c = 0; c < content.Length; c++)
                {
                    if (c > 0)
                        data.content = data.content.NewLine(true);
                    data.content += content[c];
                }
                return data;
            }

            //-----------------------------------------------------------------
            public void Submit()
            {
                ScriptKeywordProcessor.Add(this);
            }
        }

        //---------------------------------------------------------------------
        public struct Keyword
        {
            public string extension;
            public string keyword;
            public List<string> lines;

            //-----------------------------------------------------------------
            public string Content
            {
                get
                {
                    var result = string.Empty;
                    for (int c = 0; c < lines.Count; c++)
                    {
                        if (c > 0)
                            result = result.NewLine(true);
                        result += lines[c];
                    }
                    return result;
                }
            }

            //-----------------------------------------------------------------
            public Keyword(Keyword other)
            {
                extension = other.extension;
                keyword = other.keyword;
                lines = other.lines;
            }

            //-----------------------------------------------------------------
            public int IndexOf(string content, ref int end)
            {
                end = -1;
                if (lines.Count == 0)
                    return -1;

                var start = content.IndexOf(lines[0]);
                var startb = content.IndexOf("\"" + lines[0] + "\"");
                if (start >= 0 && (start != startb + 1 || startb < 0))
                {
                    end = content.IndexOf(lines[lines.Count - 1]);
                    var endb = content.IndexOf("\"" + lines[lines.Count - 1] + "\"");
                    if (end < 0 || (end == endb + 1 && endb >= 0))
                        return -1;

                    end += lines[lines.Count - 1].Length;
                    return start;
                }

                return -1;
            }

            //-----------------------------------------------------------------
            public Keyword SetKeyword(string keyword)
            {
                return new Keyword(this) { keyword = keyword };
            }

            //-----------------------------------------------------------------
            public Keyword SetContent(params string[] lines)
            {
                return new Keyword(this) { lines = new List<string>(lines) };
            }

            //-----------------------------------------------------------------
            public void Submit()
            {
                ScriptKeywordProcessor.Add(this);
            }
        }

        //---------------------------------------------------------------------
        protected static Script NewData(string extension)
        {
            return new Script() { extension = extension };
        }

        //---------------------------------------------------------------------
        protected static Keyword NewKeyword(string extension)
        {
            return new Keyword() { extension = extension };
        }
    }

    //-------------------------------------------------------------------------
    internal sealed class ScriptKeywordProcessor : UnityEditor.AssetModificationProcessor
    {
        //---------------------------------------------------------------------
        private static List<ScriptTemplateReplacement.Script> scriptReplacements = new List<ScriptTemplateReplacement.Script>();
        private static List<ScriptTemplateReplacement.Keyword> keywordReplacements = new List<ScriptTemplateReplacement.Keyword>();

        //---------------------------------------------------------------------
        public static void Add(ScriptTemplateReplacement.Script data)
        {
            scriptReplacements.Add(data);
        }

        //---------------------------------------------------------------------
        public static void Add(ScriptTemplateReplacement.Keyword data)
        {
            keywordReplacements.Add(data);
        }

        //---------------------------------------------------------------------
        public static void OnWillCreateAsset(string path)
        {
            path = path.Replace(".meta", "");
            int index = path.LastIndexOf(".");
            if (index < 0)
                return;

            var fileExtension = path.Substring(index);
            var slashIdx = Mathf.Max(0, path.LastIndexOf("/"));
            var fileName = path.Substring(slashIdx + 1, index - (slashIdx + 1));

            index = Application.dataPath.LastIndexOf("Assets");
            path = Application.dataPath.Substring(0, index) + path;
            if (!System.IO.File.Exists(path))
                return;

            string originalContent = System.IO.File.ReadAllText(path);

            var fileContent = string.Empty;
            for (int r = 0; r < scriptReplacements.Count; r++)
            {
                var replacement = scriptReplacements[r];
                if (replacement.extension == fileExtension)
                {
                    bool isMatching = true;
                    for (int m = 0; m < replacement.matchingContents.Count; m++)
                    {
                        var matching = replacement.matchingContents[m];
                        if (!originalContent.Contains(matching))
                        {
                            isMatching = false;
                            break;
                        }
                    }

                    if (isMatching)
                    {
                        fileContent = replacement.content;
                        break;
                    }
                }

                if (fileContent != string.Empty)
                    break;
            }

            if (fileContent == string.Empty)
                return;

            fileContent = fileContent.Replace("#SCRIPTNAME#", fileName);
            ApplyKeywords(ref fileContent, fileExtension);

            System.IO.File.WriteAllText(path, fileContent);
            AssetDatabase.Refresh();
        }

        //---------------------------------------------------------------------
        private static void ApplyKeywords(ref string fileContent, string fileExtension)
        {
            for (int r = 0; r < keywordReplacements.Count; r++)
            {
                var replacement = keywordReplacements[r];
                var keyword = string.Format("#{0}#", replacement.keyword);
                if (replacement.extension == string.Empty || replacement.extension == fileExtension)
                {
                    var keywordb = "\"" + keyword + "\"";
                    var start = fileContent.IndexOf(keyword);
                    var startb = fileContent.IndexOf(keywordb);
                    while (start >= 0)
                    {
                        if (start != startb + 1 || startb < 0)
                        {
                            fileContent = fileContent.Substring(0, start)
                                        + replacement.Content
                                        + fileContent.Substring(start + keyword.Length);
                            r = -1;
                        }

                        start += keyword.Length;
                        startb = fileContent.IndexOf(keywordb, start);
                        start = fileContent.IndexOf(keyword, start);
                    }
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
            var directories = new List<string>(Directory.GetDirectories(path));
            while (directories.Count > 0)
            {
                var directory = directories[0];
                {
                    directories.AddRange(Directory.GetDirectories(directory));
                    files.AddRange(Directory.GetFiles(directory));
                }
                directories.RemoveAt(0);
            }

            for (int f = 0; f < files.Count; f++)
            {
                var file = files[f];
                if (file.EndsWith(".meta"))
                    continue;

                int index = file.LastIndexOf(".");
                if (index < 0)
                    continue;

                bool isValid = false;
                var fileExtension = file.Substring(index);
                for (int r = 0; r < keywordReplacements.Count; r++)
                {
                    var replacement = keywordReplacements[r];
                    if (replacement.extension != fileExtension)
                        continue;

                    isValid = true;
                    break;
                }

                if (!isValid)
                    continue;

                var fileContent = File.ReadAllText(file);
                for (int r = 0; r < keywordReplacements.Count; r++)
                {
                    var replacement = keywordReplacements[r];
                    if (replacement.extension != fileExtension)
                        continue;

                    var end = -1;
                    var start = replacement.IndexOf(fileContent, ref end);
                    if (start < 0)
                        continue;

                    fileContent = fileContent.Substring(0, start)
                                + replacement.Content
                                + fileContent.Substring(end);
                }

                ApplyKeywords(ref fileContent, fileExtension);

                File.WriteAllText(file, fileContent);
            }

            AssetDatabase.Refresh();
        }
#endif //PRATEEK_ALLOW_INTERNAL_TOOLS
    }

    //-------------------------------------------------------------------------
    [InitializeOnLoad]
    class PrateekDefaultTemplate : ScriptTemplateReplacement
    {
        static PrateekDefaultTemplate()
        {
            NewKeyword(".cs")
            .SetKeyword("PRATEEK_COPYRIGHT")
            .SetContent(
"// -BEGIN_PRATEEK_COPYRIGHT-",
"//",
"//  Prateek, a library that is \"bien pratique\"",
"//  Header last update date: 05/03/19",
"//",
"//  Copyright © 2017—2019 Benjamin \"Touky\" Huet <huet.benjamin@gmail.com>",
"//",
"//  Prateek is free software. It comes without any warranty, to",
"//  the extent permitted by applicable law. You can redistribute it",
"//  and/or modify it under the terms of the Do What the Fuck You Want",
"//  to Public License, Version 2, as published by the WTFPL Task Force.",
"//  See http://www.wtfpl.net/ for more details.",
"//",
"// -END_PRATEEK_COPYRIGHT-")
            .Submit();

            NewKeyword(string.Empty)
            .SetKeyword("PRATEEK_DATE_YEAR")
            .SetContent(System.DateTime.Now.ToString("yyyy"))
            .Submit();

            NewKeyword(string.Empty)
            .SetKeyword("PRATEEK_DATE_UPDATE")
            .SetContent(string.Format("Header last update date: {0}", System.DateTime.Now.ToString("dd/MM/yy")))
            .Submit();
        }
    }
}
