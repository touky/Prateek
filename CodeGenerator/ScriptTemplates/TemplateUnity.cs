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
namespace Prateek.CodeGenerator.ScriptTemplates
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Prateek.Helpers;
    using Prateek.Helpers.Files;
    using UnityEditor;

    //-------------------------------------------------------------------------
#if UNITY_EDITOR
    //todo: fix that
    [InitializeOnLoad]
    class UnityFileLoader : CodeGenerator.ScriptTemplates.ScriptTemplate
    {
        static UnityFileLoader()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
                return;

            var path = FileHelpers.GetScriptTemplateFolder();
            if (path == string.Empty)
                return;

            var files = Directory.GetFiles(path);
            for (int f = 0; f < files.Length; f++)
            {
                if (!files[f].EndsWith(".txt"))
                    continue;

                ScriptTemplate.NewUnityTemplate("txt").Load(files[f]).Commit();
            }
        }
    }
#endif //UNITY_EDITOR

    //-------------------------------------------------------------------------
    public partial class ScriptTemplate
    {
        //---------------------------------------------------------------------
        protected static UnityFile NewUnityTemplate(string extension)
        {
            return new UnityFile(extension);
        }

        //---------------------------------------------------------------------
        public class UnityFile : BaseTemplate
        {
            //-----------------------------------------------------------------
            protected static string[] tags = new string[2] { "#SCRIPTNAME#", "#NOTRIM#" };
            protected string fullName;
            protected string fileName;
            protected List<string> parts;

            //-----------------------------------------------------------------
            public string FullName { get { return fullName; } }
            public string FileName { get { return fileName; } }

            //-----------------------------------------------------------------
            public UnityFile(string extension) : base(extension) { }

            //-----------------------------------------------------------------
            public override void Commit()
            {
                CodeGenerator.ScriptTemplate.Add(this);
            }

            //-----------------------------------------------------------------
            public override bool Match(string fileName, string extension, string content)
            {
                if (!base.Match(fileName, extension, content))
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
            public UnityFile Load(string path)
            {
                if (!File.Exists(path))
                    return this;

                var lastSlash = path.LastIndexOf(Strings.Separator.DirSlash.C());
                if (lastSlash < 0)
                    return this;

                var ext0 = path.LastIndexOf(Strings.Separator.FileExtension.C());
                if (ext0 < 0)
                    return this;

                var ext1 = path.LastIndexOf(Strings.Separator.FileExtension.C(), ext0 - 1);
                if (ext1 < 0)
                    return this;

                this.fullName = path.Substring(lastSlash + 1, path.Length - (lastSlash + 1));
                this.fileName = path.Substring(0, ext1);
                this.extension = path.Substring(ext1 + 1, (ext0 - ext1) - 1);

                SetContent(FileHelpers.ReadAllTextCleaned(path));
                parts = new List<string>(Content.Split(tags, StringSplitOptions.RemoveEmptyEntries));
                return this;
            }
        }
    }
}

