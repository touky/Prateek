// Tips from https://forum.unity3d.com/threads/c-script-template-how-to-make-custom-changes.273191/
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

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
                        data.content += "\n";
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
            public Vector2Int refUpdate;
            public Vector2Int bakUpdate;
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
                            result += "\n";
                        result += lines[c];
                    }
                    return result;
                }
            }

            //-----------------------------------------------------------------
            public Keyword(Keyword other)
            {
                extension = other.extension;
                refUpdate = other.refUpdate;
                bakUpdate = other.bakUpdate;
                keyword = other.keyword;
                lines = other.lines;
            }

            //-----------------------------------------------------------------
            public Keyword SetKeyword(string keyword)
            {
                return new Keyword(this) { keyword = keyword };
            }

            //-----------------------------------------------------------------
            public Keyword SetUpdateIndex(int refStart, int refEnd, int bakStart, int bakEnd)
            {
                return new Keyword(this) { refUpdate = new Vector2Int(refStart, refEnd),
                                           bakUpdate = new Vector2Int(bakStart, bakEnd) };
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
            for (int r = 0; r < keywordReplacements.Count; r++)
            {
                var replacement = keywordReplacements[r];
                var keyword = string.Format("#{0}#", replacement.keyword);
                if ((replacement.extension == string.Empty || replacement.extension == fileExtension)
                    && fileContent.Contains(keyword))
                {
                    fileContent = fileContent.Replace(keyword, replacement.Content);
                    r = -1;
                }
            }

            System.IO.File.WriteAllText(path, fileContent);
            AssetDatabase.Refresh();
        }

        //---------------------------------------------------------------------
        [MenuItem("Prateek/Update templates")]
        private static void UpdateTemplate()
        {
            var path = Application.dataPath + "/Scripts";
            if (!System.IO.Directory.Exists(path))
                return;
        }
    }

    //-------------------------------------------------------------------------
    [InitializeOnLoad]
    class PrateekDefaultTemplate : ScriptTemplateReplacement
    {
        static PrateekDefaultTemplate()
        {
            NewKeyword(".cs")
            .SetKeyword("PRATEEK_COPYRIGHT")
            .SetUpdateIndex(0, 0, 2, -2)
            .SetContent(
"// -BEGIN_PRATEEK_COPYRIGHT-",
"//",
"//  Prateek, a library that is \"bien pratique\"",
"//  #PRATEEK_DATE_UPDATE#",
"//",
"//  Copyright © 2017—#PRATEEK_DATE_YEAR# Benjamin \"Touky\" Huet <huet.benjamin@gmail.com>",
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
            .SetUpdateIndex(-1, 0, -1, 0)
            .SetContent(System.DateTime.Now.ToString("yyyy"))
            .Submit();

            NewKeyword(string.Empty)
            .SetKeyword("PRATEEK_DATE_UPDATE")
            .SetUpdateIndex(-1, 0, -1, 0)
            .SetContent(string.Format("Header last update date: {0}", System.DateTime.Now.ToString("dd/MM/yy")))
            .Submit();
        }
    }
}
