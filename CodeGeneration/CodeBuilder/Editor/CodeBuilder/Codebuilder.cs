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
    using System.IO;
    using Assets.Prateek.CodeGenerator.Code.CodeBuilder;
    using Prateek.CodeGenerator.ScriptTemplates;
    using Prateek.Core.Code.Helpers;
    using Prateek.Core.Code.Helpers.Files;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.Profiling;

    ///-------------------------------------------------------------------------
    public partial class CodeBuilder
    {
        #region Static and Constants
        private const string TEST_PREFIX = "TEST_";
        private const string TEST_EXTENSION = "txt";
        private const string GENERATED_EXTENSION = "g";
        #endregion

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
        #endregion

        #region Fields
        ///---------------------------------------------------------------------
        private OperationApplied operations = OperationApplied.ALL;
        private List<string> dataDirectories = new List<string>();
        private List<FileSources> dataFiles = new List<FileSources>();

        private List<string> workDirectories = new List<string>();
        private List<FileData> workFiles = new List<FileData>();

        private bool isAutorun = false;
        private bool isWorking = false;
        private int currentWorkFile = -1;
        private BuildResult buildRresult = BuildResult.ValueType.Success;

        private List<string> commentSplits = new List<string>();

        public bool IsWorking
        {
            get { return isWorking; }
        }
        #endregion

        #region Properties
        public string DestinationDirectory
        {
            get { return destinationDirectory; }
            set { destinationDirectory = value; }
        }

        public OperationApplied Operations
        {
            get { return operations; }
            set { operations = value; }
        }

        public virtual string SearchPattern
        {
            get { return FileHelpers.BuildExtensionMatch(TemplateRegistry.Keywords); }
            private set { }
        }

        public bool RunInTestMode
        {
            get { return runInTestMode; }
            set { runInTestMode = value; }
        }

        public int WorkFileCount
        {
            get { return workFiles.Count; }
        }

        public FileData this[int index]
        {
            get
            {
                if (index < 0 || index >= WorkFileCount)
                {
                    return new FileData();
                }

                return workFiles[index];
            }
        }
        #endregion

        #region Behaviour
        ///---------------------------------------------------------------------
        public void AddDirectory(string path)
        {
            if (destinationDirectory == string.Empty)
            {
                destinationDirectory = path;
            }

            dataDirectories.Add(path);
        }

        public void AddDirectories(params string[] paths)
        {
            dataDirectories.AddRange(paths);
        }

        public void AddDirectories(List<string> paths)
        {
            dataDirectories.AddRange(paths);
        }

        ///---------------------------------------------------------------------
        public void AddFiles(string sourceDir, params string[] files)
        {
            AddFiles(sourceDir, new List<string>(files));
        }

        public void AddFiles(string sourceDir, List<string> files)
        {
            dataFiles.Add(new FileSources {sourceDir = sourceDir, files = files});
        }

        public void AddFile(FileData fileData)
        {
            dataFiles.Add(new FileSources {data = fileData});
        }

        ///---------------------------------------------------------------------
        protected void AddWorkFile(FileData fileData)
        {
            workFiles.Add(fileData);
        }

        ///---------------------------------------------------------------------
        protected void AddWorkFiles(FileSources source)
        {
            if (source.files == null)
            {
                workFiles.Add(source.data);
                return;
            }

            for (var f = 0; f < source.files.Count; f++)
            {
                var file = source.files[f];
                if (workFiles.FindIndex(x => { return x.source.fileInfo.FullName == file; }) != -1)
                {
                    continue;
                }

                var rootPath = (operations & OperationApplied.RelativeDestination) != 0 ? source.sourceDir : string.Empty;
                AddWorkFile(new FileData(file, rootPath));
            }
        }

        ///---------------------------------------------------------------------
        public void Init()
        {
            workDirectories.Clear();
            workFiles.Clear();

            workDirectories.AddRange(dataDirectories);
            workDirectories.AddRange(sourceDirectories);

            var files = new List<string>();
            for (var d = 0; d < workDirectories.Count; d++)
            {
                var dir = FileHelpers.GetValidDirectory(workDirectories[d]);
                if (dir == string.Empty)
                {
                    continue;
                }

                if (!FileHelpers.GatherFilesAt(dir, files, SearchPattern, true, "(External)|((InternalContent_).*(cs)$)"))
                {
                    continue;
                }

                AddWorkFiles(new FileSources {sourceDir = dir, files = files});

                files.Clear();
            }

            for (var d = 0; d < dataFiles.Count; d++)
            {
                AddWorkFiles(dataFiles[d]);
            }

            AddWorkFiles(new FileSources {sourceDir = string.Empty, files = sourceFiles});
        }

        ///---------------------------------------------------------------------
        public void StartWork(bool isAutorun = false)
        {
            this.isAutorun = isAutorun;
            this.isWorking = true;
            currentWorkFile = 0;

            AssetDatabase.DisallowAutoRefresh();
        }


        ///---------------------------------------------------------------------
        public void Update()
        {
            var f      = currentWorkFile;
            var file   = workFiles[f];
            Profiler.BeginSample($"CodeBuilder.Update()");
            Profiler.BeginSample($"CodeBuilder.Update({file.source.name})");
            try
            {
                var result = (BuildResult) BuildResult.ValueType.Success;

                result = LoadData(ref file);
                if (result)
                {
                    var nextFile = false;
                    for (var p = 0; p >= 0; p++)
                    {
                        switch (p)
                        {
                            case 0:
                            {
                                result = ApplyValidTemplate(ref file);
                                break;
                            }
                            case 1:
                            {
                                result = ApplyZonedScript(ref file);
                                break;
                            }
                            case 2:
                            {
                                result = ApplyKeyword(ref file);
                                break;
                            }
                            case 3:
                            {
                                result = ApplyFixups(ref file);
                                break;
                            }
                            case 4:
                            {
                                result = WriteData(ref file);
                                break;
                            }
                            default:
                            {
                                p = -100;
                                result = BuildResult.ValueType.Success;
                                nextFile = true;
                                break;
                            }
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

                    if (!nextFile)
                    {
                        currentWorkFile = -1;
                        isWorking = false;
                        buildRresult = result;

                        AssetDatabase.AllowAutoRefresh();
                        AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
                    }
                    else
                    {
                        currentWorkFile++;
                    }
                }
                else if (!result.Is(BuildResult.ValueType.Ignored))
                {
                    result.Log();

                    currentWorkFile = -1;
                    isWorking = false;
                    buildRresult = result;

                    AssetDatabase.AllowAutoRefresh();
                    AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
                }
                else
                {
                    currentWorkFile++;
                }

                if (currentWorkFile >= workFiles.Count)
                {
                    currentWorkFile = -1;
                    isWorking = false;

                    AssetDatabase.AllowAutoRefresh();
                    AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
                }

                buildRresult = BuildResult.ValueType.Success;
            }
            catch
            {
                currentWorkFile = -1;
                isWorking = false;

                AssetDatabase.AllowAutoRefresh();
                AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
            }

            Profiler.EndSample();
            Profiler.EndSample();
        }

        ///---------------------------------------------------------------------

        #region Local calls
        private BuildResult LoadData(ref FileData fileData)
        {
            if ((operations & OperationApplied.LoadData) == 0)
            {
                return BuildResult.ValueType.Success | BuildResult.ValueType.Ignored;
            }

            return DoLoadData(ref fileData);
        }

        ///---------------------------------------------------------------------
        private BuildResult ApplyValidTemplate(ref FileData fileData)
        {
            Profiler.BeginSample($"ApplyValidTemplate()");
            
            if ((operations & OperationApplied.ApplyScriptTemplate) == 0)
            {
                Profiler.EndSample();
                return BuildResult.ValueType.Success | BuildResult.ValueType.Ignored;
            }

            var result = DoApplyValidTemplate(ref fileData);

            Profiler.EndSample();
            return result;
        }

        ///---------------------------------------------------------------------
        private BuildResult ApplyZonedScript(ref FileData fileData)
        {
            Profiler.BeginSample($"ApplyZonedScript()");
            
            if ((operations & OperationApplied.ApplyZonedScript) == 0)
            {
                Profiler.EndSample();
                return BuildResult.ValueType.Success | BuildResult.ValueType.Ignored;
            }

            var result = DoApplyZonedScript(ref fileData);

            Profiler.EndSample();
            return result;
        }

        ///---------------------------------------------------------------------
        private BuildResult ApplyKeyword(ref FileData fileData)
        {
            Profiler.BeginSample($"ApplyKeyword()");
            
            if ((operations & OperationApplied.ApplyKeyword) == 0)
            {
                Profiler.EndSample();
                return BuildResult.ValueType.Success | BuildResult.ValueType.Ignored;
            }

            var result = DoApplyKeyword(ref fileData);

            Profiler.EndSample();
            return result;
        }

        ///---------------------------------------------------------------------
        private BuildResult ApplyFixups(ref FileData fileData)
        {
            Profiler.BeginSample($"ApplyFixups()");
            
            if ((operations & OperationApplied.ApplyFixUp) == 0)
            {
                Profiler.EndSample();
                return BuildResult.ValueType.Success | BuildResult.ValueType.Ignored;
            }

            var result = DoApplyFixUps(ref fileData);

            Profiler.EndSample();
            return result;
        }

        ///---------------------------------------------------------------------
        private BuildResult WriteData(ref FileData fileData)
        {
            Profiler.BeginSample($"WriteData()");
            
            if ((operations & OperationApplied.WriteData) == 0)
            {
                Profiler.EndSample();
                return BuildResult.ValueType.Success | BuildResult.ValueType.Ignored;
            }

            var result = DoWriteData(ref fileData);

            Profiler.EndSample();
            return result;
        }

        ///---------------------------------------------------------------------
        protected virtual BuildResult DoLoadData(ref FileData fileData)
        {
            if (fileData.Load())
            {
                return BuildResult.ValueType.Success;
            }

            return BuildResult.ValueType.LoadingFailed;
        }
        #endregion Local calls

        ///---------------------------------------------------------------------

        #region Inheritable calls
        protected virtual BuildResult DoApplyValidTemplate(ref FileData fileData)
        {
            var content   = string.Empty;
            var extension = string.Empty;

            Profiler.BeginSample($"DoApplyValidTemplate()");
            
            //Look for the correct script remplacement
            var scripts = TemplateRegistry.Scripts;
            for (var r = 0; r < scripts.Count; r++)
            {
                var script = scripts[r];
                if (isAutorun && !script.AllowAutorun)
                {
                    continue;
                }

                if (script.Match(fileData.source.name, fileData.source.extension, fileData.source.content))
                {
                    content = script.Content.CleanText();
                    extension = script.ExportExtension;
                    break;
                }

                if (content != string.Empty)
                {
                    break;
                }
            }

            if (content == string.Empty)
            {
                Profiler.EndSample();
                return BuildResult.ValueType.Success | BuildResult.ValueType.NoMatchingTemplate;
            }

            content = content.Replace("#SCRIPTNAME#", fileData.source.name);

            fileData.destination.extension = extension;
            fileData.destination.SetupFileInfo();

            fileData.destination.content = content;

            Profiler.EndSample();
            return BuildResult.ValueType.Success;
        }

        ///---------------------------------------------------------------------
        protected virtual BuildResult DoApplyZonedScript(ref FileData fileData)
        {
            var keywords = TemplateRegistry.Keywords;
            var ignorers = TemplateHelpers.GatherValidIgnorables(fileData.destination.content, fileData.destination.extension);
            var stack    = new KeywordTemplateStack(KeywordTemplateMode.UsedAsScope, fileData.destination.content);

            for (var r = 0; r < keywords.Count; r++)
            {
                var keyword = keywords[r];
                if (!keyword.Match(fileData.destination.name, fileData.destination.extension, fileData.destination.content))
                {
                    continue;
                }

                if (keyword.TemplateMode == KeywordTemplateMode.UsedAsSwap)
                {
                    continue;
                }

                var start = 0;
                while ((start = fileData.destination.content.IndexOf(keyword.TagBegin, start)) >= 0)
                {
                    var safety = ignorers.AdvanceToSafety(start, IgnorableStyle.Text);
                    if (safety != start)
                    {
                        start = safety;
                        continue;
                    }

                    var tagEnd = keyword.TagEnd;
                    var end    = fileData.destination.content.IndexOf(tagEnd, start);
                    if (end < 0)
                    {
                        break;
                    }

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

        ///---------------------------------------------------------------------
        protected virtual BuildResult DoApplyKeyword(ref FileData fileData)
        {
            TemplateHelpers.ApplyKeywords(ref fileData.destination.content, fileData.destination.extension);

            return BuildResult.ValueType.Success;
        }

        ///---------------------------------------------------------------------
        protected virtual BuildResult DoApplyFixUps(ref FileData fileData)
        {
            Profiler.BeginSample($"DoApplyFixUps()");

            InitCommentSplits();

            var commentSplitRoot  = Strings.CommentSplitRoot;
            var commentSplitLength = Strings.CommentSplitLength - 1;
            var ignorers = TemplateHelpers.GatherValidIgnorables(fileData.destination.content, fileData.destination.extension);
            var stack    = new KeywordTemplateStack(KeywordTemplateMode.UsedAsScope, fileData.destination.content);

            var position = 0;
            while ((position = fileData.destination.content.IndexOf(commentSplitRoot, position)) >= 0)
            {
                Profiler.BeginSample($"while(position)");

                var safety = ignorers.AdvanceToSafety(position, IgnorableStyle.Text);
                if (safety != position)
                {
                    position = safety;
                    
                    Profiler.EndSample();
                    continue;
                }

                var lineStart = fileData.destination.content.LastIndexOf(Strings.Separator.LineFeed.C(), position);
                var lineEnd   = fileData.destination.content.IndexOf(Strings.Separator.LineFeed.C(), position);

                if (lineStart < 0 || lineEnd < 0)
                {
                    position++;

                    Profiler.EndSample();
                    continue;
                }

                lineStart++;
                var line = fileData.destination.content.Substring(lineStart, lineEnd - lineStart);
                if (line.Length != commentSplitLength)
                {
                    var diff = commentSplitLength - line.Length;
                    if (diff < 0)
                    {
                        line = line.Substring(0, commentSplitLength);
                    }
                    else
                    {
                        line += commentSplits[diff];
                    }

                    stack.Add(line, lineStart, lineEnd);
                }

                position = lineEnd;

                Profiler.EndSample();
            }

            if (stack.CanApply)
            {
                fileData.destination.content = stack.Apply();
            }

            Profiler.EndSample();

            return BuildResult.ValueType.Success;
        }

        ///---------------------------------------------------------------------
        private void InitCommentSplits()
        {
            if (commentSplits.Count != 0)
            {
                return;
            }

            Profiler.BeginSample($"Init()");
            {
                var commentSplit = string.Empty;
                for (int c = 0; c < Strings.CommentSplitLength; c++)
                {
                    commentSplits.Add(commentSplit);
                    commentSplit += Strings.Separator.OpMinus.S();
                }
            }
            Profiler.EndSample();
        }

        ///---------------------------------------------------------------------
        protected virtual BuildResult DoWriteData(ref FileData fileData)
        {
            if (runInTestMode)
            {
                fileData.destination.name = TEST_PREFIX + fileData.destination.name;
                fileData.destination.extension = fileData.destination.extension.Extension(TEST_EXTENSION);
            }

            fileData.source.SetupFileInfo();
            fileData.destination.SetupFileInfo();

            if (!fileData.destination.directoryInfo.Exists)
            {
                return (BuildResult) BuildResult.ValueType.WritingFailedDirDoesntExist + fileData.destination.directoryInfo.Name;
            }

            File.WriteAllText(fileData.destination.fileInfo.FullName, fileData.destination.content.ApplyCRLF());

            return BuildResult.ValueType.Success;
        }
        #endregion Inheritable calls
        #endregion Behaviour
    }
}
