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

    //-------------------------------------------------------------------------
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
        private OperationApplied operations = OperationApplied.ALL;
        private List<string> dataDirectories = new List<string>();
        private List<FileSources> dataFiles = new List<FileSources>();

        private List<string> workDirectories = new List<string>();
        private List<FileData> workFiles = new List<FileData>();

        private bool isAutorun = false;
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
            get { return FileHelpers.BuildExtensionMatch(TemplateRegistry.Keywords.List); }
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

        //---------------------------------------------------------------------

        //---------------------------------------------------------------------

        //---------------------------------------------------------------------

        //---------------------------------------------------------------------

        #region Behaviour
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

        //---------------------------------------------------------------------
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

        //---------------------------------------------------------------------
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

        //---------------------------------------------------------------------
        public BuildResult StartWork(bool isAutorun = false)
        {
            this.isAutorun = isAutorun;

            for (var f = 0; f < workFiles.Count; f++)
            {
                var file   = workFiles[f];
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

                    if (nextFile) { }
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
            {
                return BuildResult.ValueType.Success | BuildResult.ValueType.Ignored;
            }

            return DoLoadData(ref fileData);
        }

        //---------------------------------------------------------------------
        private BuildResult ApplyValidTemplate(ref FileData fileData)
        {
            if ((operations & OperationApplied.ApplyScriptTemplate) == 0)
            {
                return BuildResult.ValueType.Success | BuildResult.ValueType.Ignored;
            }

            return DoApplyValidTemplate(ref fileData);
        }

        //---------------------------------------------------------------------
        private BuildResult ApplyZonedScript(ref FileData fileData)
        {
            if ((operations & OperationApplied.ApplyZonedScript) == 0)
            {
                return BuildResult.ValueType.Success | BuildResult.ValueType.Ignored;
            }

            return DoApplyZonedScript(ref fileData);
        }

        //---------------------------------------------------------------------
        private BuildResult ApplyKeyword(ref FileData fileData)
        {
            if ((operations & OperationApplied.ApplyKeyword) == 0)
            {
                return BuildResult.ValueType.Success | BuildResult.ValueType.Ignored;
            }

            return DoApplyKeyword(ref fileData);
        }

        //---------------------------------------------------------------------
        private BuildResult ApplyFixups(ref FileData fileData)
        {
            if ((operations & OperationApplied.ApplyFixUp) == 0)
            {
                return BuildResult.ValueType.Success | BuildResult.ValueType.Ignored;
            }

            return DoApplyFixUps(ref fileData);
        }

        //---------------------------------------------------------------------
        private BuildResult WriteData(ref FileData fileData)
        {
            if ((operations & OperationApplied.WriteData) == 0)
            {
                return BuildResult.ValueType.Success | BuildResult.ValueType.Ignored;
            }

            return DoWriteData(ref fileData);
        }

        //---------------------------------------------------------------------
        protected virtual BuildResult DoLoadData(ref FileData fileData)
        {
            if (fileData.Load())
            {
                return BuildResult.ValueType.Success;
            }

            return BuildResult.ValueType.LoadingFailed;
        }
        #endregion Local calls

        //---------------------------------------------------------------------

        #region Inheritable calls
        protected virtual BuildResult DoApplyValidTemplate(ref FileData fileData)
        {
            var content   = string.Empty;
            var extension = string.Empty;

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
                return BuildResult.ValueType.Success | BuildResult.ValueType.NoMatchingTemplate;
            }

            content = content.Replace("#SCRIPTNAME#", fileData.source.name);

            fileData.destination.extension = extension;
            fileData.destination.SetupFileInfo();

            fileData.destination.content = content;

            return BuildResult.ValueType.Success;
        }

        //---------------------------------------------------------------------
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

        //---------------------------------------------------------------------
        protected virtual BuildResult DoApplyKeyword(ref FileData fileData)
        {
            TemplateHelpers.ApplyKeywords(ref fileData.destination.content, fileData.destination.extension);

            return BuildResult.ValueType.Success;
        }

        //---------------------------------------------------------------------
        protected virtual BuildResult DoApplyFixUps(ref FileData fileData)
        {
            var comment  = Strings.Comment;
            var ignorers = TemplateHelpers.GatherValidIgnorables(fileData.destination.content, fileData.destination.extension);
            var stack    = new KeywordTemplateStack(KeywordTemplateMode.UsedAsScope, fileData.destination.content);

            var position = 0;
            while ((position = fileData.destination.content.IndexOf(comment, position)) >= 0)
            {
                var safety = ignorers.AdvanceToSafety(position, IgnorableStyle.Text);
                if (safety != position)
                {
                    position = safety;
                    continue;
                }

                var start = fileData.destination.content.LastIndexOf(Strings.Separator.LineFeed.C(), position);
                var end   = fileData.destination.content.IndexOf(Strings.Separator.LineFeed.C(), position);

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
                        {
                            break;
                        }

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
