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
        public struct BuildResult
        {
            //---------------------------------------------------------------------
            [Flags]
            public enum ValueType
            {
                Success = 1 << 0,
                Ignored = 1 << 1,

                LoadingFailed = 1 << 2,
                NoMatchingTemplate = 1 << 3,
                WritingFailedDirDoesntExist = 1 << 4,
                PrateekScriptSourceStartTagInvalid = 1 << 5,
                PrateekScriptArgNotFound = 1 << 6,
                PrateekScriptDataNotFound = 1 << 7,
                PrateekScriptDataNotTreated = 1 << 8,
                PrateekScriptInvalidKeyword = 1 << 9,
                PrateekScriptInsufficientNames = 1 << 10,
                PrateekScriptSourceDataTagInvalid = 1 << 11,
                PrateekScriptInsufficientFuncResults = 1 << 12,
                PrateekScriptKeywordCannotStartWithNumeric = 1 << 13,
                PrateekScriptWrongKeywordChar = 1 << 14,


                MAX = 32
            }

            //---------------------------------------------------------------------
            private ValueType value;
            private string text;

            //---------------------------------------------------------------------
            public ValueType Value { get { return value; } }
            public static implicit operator bool(BuildResult result)
            {
                return (result.value & ValueType.Success) != 0;
            }

            //---------------------------------------------------------------------
            public static implicit operator BuildResult(ValueType value)
            {
                return new BuildResult() { value = value, text = string.Empty };
            }

            //---------------------------------------------------------------------
            public static BuildResult operator +(BuildResult other, string text)
            {
                return new BuildResult() { value = other.value, text = other.text + (other.text != string.Empty ? ", " : "") + text };
            }

            //---------------------------------------------------------------------
            public static BuildResult operator +(BuildResult one, BuildResult other)
            {
                return new BuildResult() { value = one.value | other.value, text = one.text + (one.text != string.Empty ? ", " : "") + other.text };
            }

            //---------------------------------------------------------------------
            public bool Is(ValueType value)
            {
                return (this.value & value) != 0;
            }

            //---------------------------------------------------------------------
            public void Log()
            {
                var log = string.Format("Error with: {0}\n", text);
                for (int i = 0; i < (int)ValueType.MAX; i++)
                {
                    var testValue = (ValueType)(1 << i);
                    if (!Is(testValue))
                        continue;
                    log += string.Format(" - {0}\n", testValue.ToString());
                }
                UnityEngine.Debug.LogError(log);
            }
        }

        //---------------------------------------------------------------------
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

        private bool isAutorun = false;
        #endregion Fields

        //---------------------------------------------------------------------
        #region Properties
        public string DestinationDirectory { get { return destinationDirectory; } set { destinationDirectory = value; } }
        public OperationApplied Operations { get { return operations; } set { operations = value; } }
        public virtual string SearchPattern { get { return FileHelpers.BuildExtensionMatch(ScriptTemplate.Keywords.List); } private set { } }
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
        protected void AddWorkFile(FileData fileData)
        {
            workFiles.Add(fileData);
        }

        //---------------------------------------------------------------------
        protected void AddWorkFiles(FileSources source)
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
                AddWorkFile(new FileData(file, rootPath));
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
        public BuildResult StartWork(bool isAutorun = false)
        {
            this.isAutorun = isAutorun;

            for (int f = 0; f < workFiles.Count; f++)
            {
                var file = workFiles[f];
                var result = (BuildResult)BuildResult.ValueType.Success;

                result = LoadData(ref file);
                if (result)
                {
                    bool nextFile = false;
                    for (int p = 0; p >= 0; p++)
                    {
                        switch (p)
                        {
                            case 0: { result = ApplyValidTemplate(ref file); break; }
                            case 1: { result = ApplyZonedScript(ref file); break; }
                            case 2: { result = ApplyKeyword(ref file); break; }
                            case 3: { result = ApplyFixups(ref file); break; }
                            case 4: { result = WriteData(ref file); break; }
                            default: { p = -100; result = BuildResult.ValueType.Success; break; }
                        }

                        if (!result)
                        {
                            if (!result.Is(BuildResult.ValueType.Ignored))
                            {
                                result.Log();
                            }
                            nextFile = true;
                            break;
                        }
                    }

                    if (nextFile)
                        continue;
                }
                else if (!result.Is(BuildResult.ValueType.Ignored))
                {
                    result.Log();
                }
            }

            AssetDatabase.Refresh();

            return BuildResult.ValueType.Success;
        }

        //---------------------------------------------------------------------
        #region Local calls
        private BuildResult LoadData(ref FileData fileData)
        {
            if ((operations & OperationApplied.LoadData) == 0)
                return BuildResult.ValueType.Success | BuildResult.ValueType.Ignored;
            return DoLoadData(ref fileData);
        }

        //---------------------------------------------------------------------
        private BuildResult ApplyValidTemplate(ref FileData fileData)
        {
            if ((operations & OperationApplied.ApplyScriptTemplate) == 0)
                return BuildResult.ValueType.Success | BuildResult.ValueType.Ignored;
            return DoApplyValidTemplate(ref fileData);
        }

        //---------------------------------------------------------------------
        private BuildResult ApplyZonedScript(ref FileData fileData)
        {
            if ((operations & OperationApplied.ApplyZonedScript) == 0)
                return BuildResult.ValueType.Success | BuildResult.ValueType.Ignored;
            return DoApplyZonedScript(ref fileData);
        }

        //---------------------------------------------------------------------
        private BuildResult ApplyKeyword(ref FileData fileData)
        {
            if ((operations & OperationApplied.ApplyKeyword) == 0)
                return BuildResult.ValueType.Success | BuildResult.ValueType.Ignored;
            return DoApplyKeyword(ref fileData);
        }

        //---------------------------------------------------------------------
        private BuildResult ApplyFixups(ref FileData fileData)
        {
            if ((operations & OperationApplied.ApplyFixUp) == 0)
                return BuildResult.ValueType.Success | BuildResult.ValueType.Ignored;
            return DoApplyFixUps(ref fileData);
        }

        //---------------------------------------------------------------------
        private BuildResult WriteData(ref FileData fileData)
        {
            if ((operations & OperationApplied.WriteData) == 0)
                return BuildResult.ValueType.Success | BuildResult.ValueType.Ignored;
            return DoWriteData(ref fileData);
        }

        //---------------------------------------------------------------------
        protected virtual BuildResult DoLoadData(ref FileData fileData)
        {
            if (fileData.Load())
                return BuildResult.ValueType.Success;
            return BuildResult.ValueType.LoadingFailed;
        }
        #endregion Local calls

        //---------------------------------------------------------------------
        #region Inheritable calls
        protected virtual BuildResult DoApplyValidTemplate(ref FileData fileData)
        {
            var content = string.Empty;
            var extension = string.Empty;

            //Look for the correct script remplacement
            var scripts = ScriptTemplate.Scripts;
            for (int r = 0; r < scripts.Count; r++)
            {
                var script = scripts[r];
                if (isAutorun && !script.AllowAutorun)
                    continue;

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
                return BuildResult.ValueType.Success | BuildResult.ValueType.NoMatchingTemplate;

            content = content.Replace("#SCRIPTNAME#", fileData.source.name);

            fileData.destination.extension = extension;
            fileData.destination.relPath = fileData.destination.relPath
                        .Replace(fileData.source.name.Extension(fileData.source.extension),
                                 fileData.destination.name.Extension(fileData.destination.extension));
            fileData.destination.absPath = fileData.destination.absPath
                        .Replace(fileData.source.name.Extension(fileData.source.extension),
                                 fileData.destination.name.Extension(fileData.destination.extension));

            fileData.destination.content = content;

            return BuildResult.ValueType.Success;
        }

        //---------------------------------------------------------------------
        protected virtual BuildResult DoApplyZonedScript(ref FileData fileData)
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

            return BuildResult.ValueType.Success;
        }

        //---------------------------------------------------------------------
        protected virtual BuildResult DoApplyKeyword(ref FileData fileData)
        {
            TemplateHelpers.ApplyKeywords(ref fileData.destination.content, fileData.destination.extension);

            return BuildResult.ValueType.Success;
        }

        //---------------------------------------------------------------------
        protected virtual BuildResult DoApplyFixUps(ref FileData fileData)
        {
            var comment = Strings.Comment;
            var ignorers = TemplateHelpers.GatherValidIgnorables(fileData.destination.content, fileData.destination.extension);
            var stack = new ScriptTemplate.KeywordStack(ScriptTemplate.KeywordMode.ZoneDelimiter, fileData.destination.content);

            var position = 0;
            while ((position = fileData.destination.content.IndexOf(comment, position)) >= 0)
            {
                var safety = ignorers.AdvanceToSafety(position, ScriptTemplate.Ignorable.Style.Comment);
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

            return BuildResult.ValueType.Success;
        }

        //---------------------------------------------------------------------
        protected virtual BuildResult DoWriteData(ref FileData fileData)
        {
            var dst = fileData.destination;
            var path = destinationDirectory + dst.relPath;
            var dir = path.Replace(dst.name.Extension(dst.extension), string.Empty);
            var newDir = FileHelpers.GetValidDirectory(dir);
            if (newDir == string.Empty)
                return (BuildResult)BuildResult.ValueType.WritingFailedDirDoesntExist + dir;
            dir = newDir;

            path = dir;
            if (runInTestMode)
            {
                path += "TEST_";
            }
            path += dst.name.Extension(dst.extension);
            if (runInTestMode)
            {
                path = path.Extension("txt");
            }

            File.WriteAllText(path, dst.content.ApplyCRLF());

            return BuildResult.ValueType.Success;
        }
        #endregion Inheritable calls
        #endregion Behaviour
    }
}
