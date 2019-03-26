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
#endregion File namespaces

//-----------------------------------------------------------------------------
namespace Prateek.CodeGeneration
{
    //-------------------------------------------------------------------------
    public partial class CodeBuilder
    {
        //---------------------------------------------------------------------
        #region Declarations
        public struct FileData
        {
            //-----------------------------------------------------------------
            public struct Infos
            {
                public string absPath;
                public string relPath;
                public string name;
                public string extension;

                public string content;
            }

            //-----------------------------------------------------------------
            public Infos source;
            public Infos destination;

            //-----------------------------------------------------------------
            public bool IsLoaded { get { return source.content != null && source.content != string.Empty; } }

            //-----------------------------------------------------------------
            public FileData(string file, string sourceDir) : this(file, sourceDir, null) { }
            public FileData(string file, string sourceDir, string content)
            {
                file = FileHelpers.GetValidFile(file);
                if ((content == null || content == string.Empty) && file == string.Empty)
                {
                    source = default(Infos);
                    destination = default(Infos);
                    return;
                }

                var iExt = file.LastIndexOf(Strings.Separator.FileExtension.C());
                var iName = Mathf.Max(file.LastIndexOf(Strings.Separator.DirSlash.C()), file.LastIndexOf(Strings.Separator.DirBackslash.C()));

                source.absPath = file;
                source.relPath = sourceDir == string.Empty ? file : file.Replace(sourceDir, string.Empty);
                source.extension = file.Substring(iExt + 1);
                source.name = file.Substring(iName + 1, iExt - (iName + 1));
                source.content = content;

                destination = source;
            }

            //-----------------------------------------------------------------
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
        public struct FileSources
        {
            public string sourceDir;
            public List<string> files;
            public FileData data;
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
            ApplyFixUp = (1 << 5),

            WriteData = (1 << 6),

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

        [SerializeField]
        private bool runInTestMode = false;
        #endregion Settings

        //---------------------------------------------------------------------
        #region Fields
        private OperationApplied operations = OperationApplied.ALL;
        private List<string> dataDirectories = new List<string>();
        private List<FileSources> dataFiles = new List<FileSources>();

        private List<string> workDirectories = new List<string>();
        private List<FileData> workFiles = new List<FileData>();
        #endregion Fields

        //---------------------------------------------------------------------
        #region Properties
        public string DestinationDirectory { get { return destinationDirectory; } set { destinationDirectory = value; } }
        public OperationApplied Operations { get { return operations; } set { operations = value; } }
        protected virtual string SearchPattern { get { return FileHelpers.BuildExtensionMatch(ScriptTemplate.Keywords.List); } private set { } }
        public bool RunInTestMode { get { return runInTestMode; } set { runInTestMode = value; } }

        public int WorkFileCount { get { return workFiles.Count; } }
        public FileData this[int index]
        {
            get
            {
                if (index < 0 || index >= WorkFileCount)
                    return new FileData();
                return workFiles[index];
            }
        }
        #endregion Properties

        //---------------------------------------------------------------------
        #region Behaviour
        public void AddDirectory(string path)
        {
            if (destinationDirectory == string.Empty)
                destinationDirectory = path;
            dataDirectories.Add(path);
        }
        public void AddDirectories(params string[] paths) { dataDirectories.AddRange(paths); }
        public void AddDirectories(List<string> paths) { dataDirectories.AddRange(paths); }

        //---------------------------------------------------------------------
        public void AddFiles(string sourceDir, params string[] files) { AddFiles(sourceDir, new List<string>(files)); }
        public void AddFiles(string sourceDir, List<string> files) { dataFiles.Add(new FileSources() { sourceDir = sourceDir, files = files }); }
        public void AddFile(FileData fileData) { dataFiles.Add(new FileSources() { data = fileData }); }

        //---------------------------------------------------------------------
        private void AddWorkFiles(FileSources source)
        {
            if (source.files == null)
            {
                workFiles.Add(source.data);
                return;
            }

            for (int f = 0; f < source.files.Count; f++)
            {
                var file = source.files[f];
                if (workFiles.FindIndex((x) => { return x.source.absPath == file; }) != -1)
                    continue;

                string rootPath = ((operations & OperationApplied.RelativeDestination) != 0) ? source.sourceDir : string.Empty;
                workFiles.Add(new FileData(file, rootPath));
            }
        }

        //---------------------------------------------------------------------
        public void Init()
        {
            workDirectories.Clear();
            workFiles.Clear();

            workDirectories.AddRange(dataDirectories);
            workDirectories.AddRange(sourceDirectories);

            var files = new List<string>();
            for (int d = 0; d < workDirectories.Count; d++)
            {
                var dir = FileHelpers.GetValidDirectory(workDirectories[d]);
                if (dir == string.Empty)
                    continue;

                if (!FileHelpers.GatherFilesAt(dir, files, SearchPattern, true, "(External)|((InternalContent_).*(cs)$)"))
                    continue;

                AddWorkFiles(new FileSources() { sourceDir = dir, files = files });

                files.Clear();
            }

            for (int d = 0; d < dataFiles.Count; d++)
            {
                AddWorkFiles(dataFiles[d]);
            }

            AddWorkFiles(new FileSources() { sourceDir = string.Empty, files = sourceFiles });
        }

        //---------------------------------------------------------------------
        public void StartWork()
        {
            for (int f = 0; f < workFiles.Count; f++)
            {
                var file = workFiles[f];

                if (LoadData(ref file))
                {
                    if (!ApplyValidTemplate(ref file))
                        continue;

                    if (!ApplyZonedScript(ref file))
                        continue;

                    if (!ApplyKeyword(ref file))
                        continue;

                    if (!ApplyFixups(ref file))
                        continue;

                    WriteData(ref file);
                }
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
        private bool ApplyFixups(ref FileData fileData)
        {
            if ((operations & OperationApplied.ApplyFixUp) == 0)
                return true;
            return DoApplyFixUps(ref fileData);
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
            var extension = string.Empty;

            //Look for the correct script remplacement
            var scripts = ScriptTemplate.Scripts;
            for (int r = 0; r < scripts.Count; r++)
            {
                var script = scripts[r];
                if (script.Match(fileData.source.name, fileData.source.extension, fileData.source.content))
                {
                    content = script.Content.CleanText();
                    extension = script.ExportExtension;
                    break;
                }

                if (content != string.Empty)
                    break;
            }

            if (content == string.Empty)
                return false;

            content = content.Replace("#SCRIPTNAME#", fileData.source.name);

            fileData.destination.extension = extension;
            fileData.destination.relPath = fileData.destination.relPath
                        .Replace(fileData.source.name.Extension(fileData.source.extension),
                                 fileData.destination.name.Extension(fileData.destination.extension));
            fileData.destination.absPath = fileData.destination.absPath
                        .Replace(fileData.source.name.Extension(fileData.source.extension),
                                 fileData.destination.name.Extension(fileData.destination.extension));

            fileData.destination.content = content;

            return true;
        }

        //---------------------------------------------------------------------
        protected virtual bool DoApplyZonedScript(ref FileData fileData)
        {
            var keywords = ScriptTemplate.Keywords;
            var ignorers = TemplateHelpers.GatherValidIgnorables(fileData.destination.content, fileData.destination.extension);
            var stack = new ScriptTemplate.KeywordStack(ScriptTemplate.KeywordMode.ZoneDelimiter, fileData.destination.content);

            for (int r = 0; r < keywords.Count; r++)
            {
                var keyword = keywords[r];
                if (!keyword.Match(fileData.destination.name, fileData.destination.extension, fileData.destination.content))
                    continue;

                if (keyword.Mode == ScriptTemplate.KeywordMode.KeywordOnly)
                    continue;

                var start = 0;
                while ((start = fileData.destination.content.IndexOf(keyword.TagBegin, start)) >= 0)
                {
                    var safety = ignorers.AdvanceToSafety(start, ScriptTemplate.Ignorable.Style.Text);
                    if (safety != start)
                    {
                        start = safety;
                        continue;
                    }

                    var tagEnd = keyword.TagEnd;
                    var end = fileData.destination.content.IndexOf(tagEnd, start);
                    if (end < 0)
                        break;

                    end += tagEnd.Length;

                    stack.Add(keyword, start, end);

                    start = end;
                }
            }

            if (stack.CanApply)
            {
                fileData.destination.content = stack.Apply();
            }

            return true;
        }

        //---------------------------------------------------------------------
        protected virtual bool DoApplyKeyword(ref FileData fileData)
        {
            TemplateHelpers.ApplyKeywords(ref fileData.destination.content, fileData.destination.extension);

            return true;
        }

        //---------------------------------------------------------------------
        protected virtual bool DoApplyFixUps(ref FileData fileData)
        {
            var comment = Strings.Comment;
            var ignorers = TemplateHelpers.GatherValidIgnorables(fileData.destination.content, fileData.destination.extension);
            var stack = new ScriptTemplate.KeywordStack(ScriptTemplate.KeywordMode.ZoneDelimiter, fileData.destination.content);

            var position = 0;
            while ((position = fileData.destination.content.IndexOf(comment, position)) >= 0)
            {
                var safety = ignorers.AdvanceToSafety(position, ScriptTemplate.Ignorable.Style.Text);
                if (safety != position)
                {
                    position = safety;
                    continue;
                }

                var start = fileData.destination.content.LastIndexOf(Strings.Separator.LineFeed.C(), position);
                var end = fileData.destination.content.IndexOf(Strings.Separator.LineFeed.C(), position);

                if (start < 0 || end < 0)
                {
                    position++;
                    continue;
                }

                start++;
                var line = fileData.destination.content.Substring(start, end - start);
                if (line.Length != 79)
                {
                    while (line.Length > 79)
                    {
                        if (line[line.Length - 1] != Strings.Separator.OpMinus.C())
                            break;
                        line = line.Remove(line.Length - 1);
                    }

                    while (line.Length < 79)
                    {
                        line += Strings.Separator.OpMinus.C();
                    }

                    stack.Add(line, start, end);
                }

                position = end;
            }

            if (stack.CanApply)
            {
                fileData.destination.content = stack.Apply();
            }

            return true;
        }

        //---------------------------------------------------------------------
        protected virtual bool DoWriteData(ref FileData fileData)
        {
            var dst = fileData.destination;
            var path = destinationDirectory + dst.relPath;
            var dir = path.Replace(dst.name.Extension(dst.extension), string.Empty);
            dir = FileHelpers.GetValidDirectory(dir);
            if (dir == string.Empty)
                return false;

            path = dir + dst.name.Extension(dst.extension);
            File.WriteAllText(path + (runInTestMode ? ".txt" : String.Empty), dst.content.ApplyCRLF());

            return true;
        }
        #endregion Inheritable calls
        #endregion Behaviour
    }
}
