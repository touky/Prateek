// -BEGIN_PRATEEK_COPYRIGHT-
//
//  Prateek, a library that is "bien pratique"
//  Header last update date: 19/03/19
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
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.Serialization;

#if UNITY_PROFILING
using UnityEngine.Profiling;
#endif //UNITY_PROFILING

using Unity.Jobs;
using Unity.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif //UNITY_EDITOR
#endregion Unity

#region Prateek
using Prateek;
using Prateek.Base;
using Prateek.Extensions;
using Prateek.Helpers;
using Prateek.Attributes;

#if UNITY_EDITOR
using Prateek.ScriptTemplating;
#endif //UNITY_EDITOR

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

//-----------------------------------------------------------------------------
namespace Prateek.ScriptTemplating
{
    //-------------------------------------------------------------------------
    public class Codebuilder
    {
        //---------------------------------------------------------------------
        #region Declarations
        public struct FileData
        {
            //----
            public struct Infos
            {
                public string absPath;
                public string relPath;
                public string name;
                public string extension;

                public string content;
            }

            //----
            public Infos source;
            public Infos destination;

            //----
            public FileData(string file, string sourceDir) : this(file, sourceDir, null) { }
            public FileData(string file, string sourceDir, string content)
            {
                file = FileHelpers.GetValidFile(file);
                if (file == string.Empty)
                {
                    source = default(Infos);
                    destination = default(Infos);
                    return;
                }

                var iExt = file.LastIndexOf(Strings.Separator.FileExtension.C()[0]);
                var iName = Mathf.Max(file.LastIndexOf(Strings.Separator.Directory.C()[0]), file.LastIndexOf(Strings.Separator.Directory.C()[1]));

                source.absPath = file;
                source.relPath = sourceDir == string.Empty ? file : file.Replace(sourceDir, string.Empty);
                source.extension = file.Substring(iExt + 1);
                source.name = file.Substring(iName + 1, iExt - (iName + 1));
                source.content = content;

                destination = source;
            }

            //----
            public bool Load(bool forceReload = false)
            {
                if (source.content != null)
                {
                    if (!forceReload)
                        return true;
                }

                source.content = string.Empty;
                var path = FileHelpers.GetValidFile(source.absPath);
                if (path != string.Empty)
                {
                    source.content = FileHelpers.ReadAllTextCleaned(path);
                    destination.content = source.content;
                    return true;
                }
                return false;
            }
        }

        //---------------------------------------------------------------------
        [Flags]
        public enum OperationApplied
        {
            RelativeDestination = (1 << 0),

            LoadData = (1 << 1),

            ApplyScriptTemplate = (1 << 2),
            ApplyZonedScript = (1 << 3),
            ApplyKeyword = (1 << 4),

            WriteData = (1 << 5),

            ALL = ~0,

            MAX
        }
        #endregion Declarations

        //---------------------------------------------------------------------
        #region Settings
        [Header("Directories")]
        [SerializeField]
        private List<string> sourceDirectories = new List<string>();

        [SerializeField]
        private string destinationDirectory = string.Empty;

        [SerializeField]
        private List<string> sourceFiles = new List<string>();
        #endregion Settings

        //---------------------------------------------------------------------
        #region Fields
        private OperationApplied operations = OperationApplied.ALL;
        private List<string> workDirectories = new List<string>();
        private List<FileData> workFiles = new List<FileData>();
        #endregion Fields

        //---------------------------------------------------------------------
        #region Properties
        public OperationApplied Operations { get { return operations; } set { operations = value; } }
        protected virtual string SearchPattern { get { return FileHelpers.BuildExtensionMatch(TemplateReplacement.Keywords.List); } private set { } }
        #endregion Properties

        //---------------------------------------------------------------------
        #region Behaviour
        public void AddDirectory(string path) { workDirectories.Add(path); }
        public void AddDirectories(params string[] paths) { workDirectories.AddRange(paths); }
        public void AddDirectories(List<string> paths) { workDirectories.AddRange(paths); }

        //---------------------------------------------------------------------
        public void AddFiles(string sourceDir, params string[] files) { AddFiles(sourceDir, new List<string>(files)); }
        public void AddFiles(string sourceDir, List<string> files)
        {
            for (int f = 0; f < files.Count; f++)
            {
                var file = files[f];
                if (workFiles.FindIndex((x) => { return x.source.absPath == file; }) != -1)
                    continue;

                string rootPath = ((operations & OperationApplied.RelativeDestination) != 0) ? sourceDir : string.Empty;
                AddFile(new FileData(file, rootPath));
            }
        }

        //---------------------------------------------------------------------
        public void AddFile(FileData fileData)
        {
            workFiles.Add(fileData);
        }

        //---------------------------------------------------------------------
        public void Init()
        {
            for (int d = 0; d < sourceDirectories.Count; d++)
            {
                var dir = FileHelpers.GetValidDirectory(sourceDirectories[d]);
                if (dir != string.Empty)
                    continue;

                AddFiles(dir, Directory.GetFiles(dir, SearchPattern));
            }

            AddFiles(string.Empty, sourceFiles);
        }

        //---------------------------------------------------------------------
        public void StartWork()
        {
            for (int f = 0; f < workFiles.Count; f++)
            {
                var file = workFiles[f];

                LoadData(ref file);
                {
                    ApplyValidTemplate(ref file);

                    ApplyZonedScript(ref file);

                    ApplyKeyword(ref file);
                }
                WriteData(ref file);
            }

            AssetDatabase.Refresh();
        }

        //---------------------------------------------------------------------
        #region Local calls
        private bool LoadData(ref FileData fileData)
        {
            if ((operations & OperationApplied.LoadData) == 0)
                return true;
            return DoLoadData(ref fileData);
        }

        //---------------------------------------------------------------------
        private bool ApplyValidTemplate(ref FileData fileData)
        {
            if ((operations & OperationApplied.ApplyScriptTemplate) == 0)
                return true;
            return DoApplyValidTemplate(ref fileData);
        }

        //---------------------------------------------------------------------
        private bool ApplyZonedScript(ref FileData fileData)
        {
            if ((operations & OperationApplied.ApplyZonedScript) == 0)
                return true;
            return DoApplyZonedScript(ref fileData);
        }

        //---------------------------------------------------------------------
        private bool ApplyKeyword(ref FileData fileData)
        {
            if ((operations & OperationApplied.ApplyKeyword) == 0)
                return true;
            return DoApplyKeyword(ref fileData);
        }

        //---------------------------------------------------------------------
        private bool WriteData(ref FileData fileData)
        {
            if ((operations & OperationApplied.WriteData) == 0)
                return true;
            return DoWriteData(ref fileData);
        }

        //---------------------------------------------------------------------
        protected virtual bool DoLoadData(ref FileData fileData)
        {
            return fileData.Load();
        }
        #endregion Local calls

        //---------------------------------------------------------------------
        #region Inheritable calls
        protected virtual bool DoApplyValidTemplate(ref FileData fileData)
        {
            var content = string.Empty;

            //Look for the correct script remplacement
            var scripts = TemplateReplacement.Scripts;
            for (int r = 0; r < scripts.Count; r++)
            {
                var script = scripts[r];
                if (script.Match(fileData.source.extension, fileData.source.content))
                {
                    content = script.Content.CleanText();
                    break;
                }

                if (content != string.Empty)
                    break;
            }

            if (content == string.Empty)
                return false;

            content = content.Replace("#SCRIPTNAME#", fileData.source.name);

            fileData.destination.content = content;

            return true;
        }

        //---------------------------------------------------------------------
        protected virtual bool DoApplyZonedScript(ref FileData fileData)
        {
            var keywords = TemplateReplacement.Keywords;
            var ignorers = TemplateHelpers.GatherValidIgnorables(fileData.destination.content, fileData.destination.extension);
            var stack = new TemplateReplacement.KeywordStack(TemplateReplacement.KeywordMode.ZoneDelimiter, fileData.destination.content);

            for (int r = 0; r < keywords.Count; r++)
            {
                var keyword = keywords[r];
                if (!keyword.Match(fileData.destination.extension, fileData.destination.content))
                    continue;

                if (keyword.Mode == TemplateReplacement.KeywordMode.KeywordOnly)
                    continue;

                var start = 0;
                while ((start = fileData.destination.extension.IndexOf(keyword.TagBegin, start)) >= 0)
                {
                    var safety = ignorers.AdvanceToSafety(start, TemplateReplacement.Ignorable.Style.Text);
                    if (safety != start)
                    {
                        start = safety;
                        continue;
                    }

                    var tagEnd = keyword.TagEnd;
                    var end = fileData.destination.extension.IndexOf(tagEnd, start);
                    if (end < 0)
                        break;

                    end += tagEnd.Length;

                    stack.Add(keyword, start, end);

                    start = end;
                }
            }

            fileData.destination.content = stack.Apply();

            return true;
        }

        //---------------------------------------------------------------------
        protected virtual bool DoApplyKeyword(ref FileData fileData)
        {
            TemplateHelpers.ApplyKeywords(ref fileData.destination.content, fileData.destination.extension);

            return true;
        }

        //---------------------------------------------------------------------
        protected virtual bool DoWriteData(ref FileData fileData)
        {
            var path = (destinationDirectory + fileData.destination.relPath)
                            .Replace(fileData.source.name.Extension(fileData.source.extension),
                                     fileData.destination.name.Extension(fileData.destination.extension));
            path = FileHelpers.GetValidFile(path);
            if (path == string.Empty)
                return false;

            File.WriteAllText(path, fileData.destination.content.ApplyCRLF());

            return true;
        }
        #endregion Inheritable calls
        #endregion Behaviour
    }
}
