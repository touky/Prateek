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
namespace Prateek.CodeGenerator
{
    using System.Collections.Generic;

    //-------------------------------------------------------------------------
    public partial class ScriptTemplate
    {
        //---------------------------------------------------------------------
        public struct Group<T> where T: ScriptTemplates.ScriptTemplate.BaseTemplate
        {
            //-----------------------------------------------------------------
            private List<T> list;

            //-----------------------------------------------------------------
            public Group(List<T> list)
            {
                this.list = list;
            }

            //-----------------------------------------------------------------
            public int Count { get { return list != null ? list.Count : 0; } }
            public T this[int index] { get { return list != null ? list[index] : default(T); } }
            public List<T> List { get { return list != null ? list : null; } }
        }

        //---------------------------------------------------------------------
        #region Scripts
        private static List<ScriptTemplates.ScriptTemplate.ScriptFile> scripts = new List<ScriptTemplates.ScriptTemplate.ScriptFile>();
        public static Group<ScriptTemplates.ScriptTemplate.ScriptFile> Scripts { get { return new Group<ScriptTemplates.ScriptTemplate.ScriptFile>(scripts); } }
        public static void Add(ScriptTemplates.ScriptTemplate.ScriptFile data) { scripts.Add(data); }
        #endregion Scripts

        //---------------------------------------------------------------------
        #region Keywords
        private static List<ScriptTemplates.ScriptTemplate.Keyword> keywords = new List<ScriptTemplates.ScriptTemplate.Keyword>();
        public static Group<ScriptTemplates.ScriptTemplate.Keyword> Keywords { get { return new Group<ScriptTemplates.ScriptTemplate.Keyword>(keywords); } }
        public static void Add(ScriptTemplates.ScriptTemplate.Keyword data) { keywords.Add(data); }
        #endregion Keywords

        //---------------------------------------------------------------------
        #region Ignorables
        private static List<ScriptTemplates.ScriptTemplate.Ignorable> ignorables = new List<ScriptTemplates.ScriptTemplate.Ignorable>();
        public static Group<ScriptTemplates.ScriptTemplate.Ignorable> Ignorables { get { return new Group<ScriptTemplates.ScriptTemplate.Ignorable>(ignorables); } }
        public static void Add(ScriptTemplates.ScriptTemplate.Ignorable data) { ignorables.Add(data); }
        #endregion Ignorables

        //---------------------------------------------------------------------
        #region Unity templates
        private static List<ScriptTemplates.ScriptTemplate.UnityFile> templates = new List<ScriptTemplates.ScriptTemplate.UnityFile>();
        public static void Add(ScriptTemplates.ScriptTemplate.UnityFile data) { templates.Add(data); }
        public static bool MatchTemplate(string filePath, string extension, string content)
        {
            for (int t = 0; t < templates.Count; t++)
            {
                var template = templates[t];
                if (template.FullName != filePath)
                    continue;

                return template.Match(template.FileName, extension, content);
            }
            return false;
        }
        #endregion Unity templates
    }
}
