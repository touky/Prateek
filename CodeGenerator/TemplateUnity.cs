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
using Prateek.ScriptTemplating;
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
#endregion File namespaces

namespace Prateek.ScriptTemplating
{
    //-------------------------------------------------------------------------
    public partial class TemplateReplacement
    {
        //---------------------------------------------------------------------
        public class TemplateUnity : TemplateBase
        {
            //-----------------------------------------------------------------
            protected static string[] tags = new string[2] { "#SCRIPTNAME#", "#NOTRIM#" };
            protected string path;
            protected List<string> parts;

            //-----------------------------------------------------------------
            public string Path { get { return path; } }

            //-----------------------------------------------------------------
            public TemplateUnity(string extension) : base(extension) { }

            //-----------------------------------------------------------------
            public override void Commit()
            {
                TemplateReplacement.Add(this);
            }

            //-----------------------------------------------------------------
            public override bool Match(string extension, string content)
            {
                if (!base.Match(extension, content))
                    return false;

                var start = 0;
                for (int p = 0; p < parts.Count; p++)
                {
                    var index = content.IndexOf(parts[p], start);
                    if (index < start)
                        return false;

                    start = index;
                }

                return true;
            }

            //-----------------------------------------------------------------
            public TemplateUnity Load(string path)
            {
                if (!File.Exists(path))
                    return this;

                var index = path.LastIndexOf(Strings.Separator.DirSlash.C());
                if (index < 0)
                    return this;

                var i0 = path.LastIndexOf(Strings.Separator.FileExtension.C());
                if (i0 < 0)
                    return this;

                var i1 = path.LastIndexOf(Strings.Separator.FileExtension.C(), i0 - 1);
                if (i1 < 0)
                    return this;

                this.path = path.Substring(index + 1, path.Length - (index + 1));
                this.extension = path.Substring(i1 + 1, (i0 - i1) - 1);

                SetContent(FileHelpers.ReadAllTextCleaned(path));
                parts = new List<string>(content.Split(tags, StringSplitOptions.RemoveEmptyEntries));
                return this;
            }
        }

        //---------------------------------------------------------------------
        protected static TemplateUnity NewUnityTemplate(string extension)
        {
            return new TemplateUnity(extension);
        }
    }

    //-------------------------------------------------------------------------
    [InitializeOnLoad]
    class UnityTemplateLoader : TemplateReplacement
    {
        static UnityTemplateLoader()
        {
            var path = FileHelpers.GetScriptTemplateFolder();
            if (path == string.Empty)
                return;

            var files = Directory.GetFiles(path);
            for (int f = 0; f < files.Length; f++)
            {
                if (!files[f].EndsWith(".txt"))
                    continue;

                NewUnityTemplate("txt").Load(files[f]).Commit();
            }
        }
    }
}
