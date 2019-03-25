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

namespace Prateek.CodeGeneration
{
    //-------------------------------------------------------------------------
    public partial class ScriptTemplate
    {
        //---------------------------------------------------------------------
        public struct Group<T> where T: ScriptTemplate.BaseTemplate
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
        private static List<ScriptFile> scripts = new List<ScriptFile>();
        public static Group<ScriptFile> Scripts { get { return new Group<ScriptFile>(scripts); } }
        public static void Add(ScriptFile data) { scripts.Add(data); }
        #endregion Scripts

        //---------------------------------------------------------------------
        #region Keywords
        private static List<Keyword> keywords = new List<Keyword>();
        public static Group<Keyword> Keywords { get { return new Group<Keyword>(keywords); } }
        public static void Add(Keyword data) { keywords.Add(data); }
        #endregion Keywords

        //---------------------------------------------------------------------
        #region Ignorables
        private static List<Ignorable> ignorables = new List<Ignorable>();
        public static Group<Ignorable> Ignorables { get { return new Group<Ignorable>(ignorables); } }
        public static void Add(Ignorable data) { ignorables.Add(data); }
        #endregion Ignorables

        //---------------------------------------------------------------------
        #region Unity templates
        private static List<UnityFile> templates = new List<UnityFile>();
        public static void Add(UnityFile data) { templates.Add(data); }
        public static bool MatchTemplate(string filePath, string extension, string content)
        {
            for (int t = 0; t < templates.Count; t++)
            {
                var template = templates[t];
                if (template.Path != filePath)
                    continue;

                return template.Match(extension, content);
            }
            return false;
        }
        #endregion Unity templates

        //---------------------------------------------------------------------
        #region Code rules
        private static List<CodeRule> rules = new List<CodeRule>();
        public static Group<CodeRule> CodeRules { get { return new Group<CodeRule>(rules); } }
        public static void Add(CodeRule data) { rules.Add(data); }
        #endregion Code rules
    }
}
