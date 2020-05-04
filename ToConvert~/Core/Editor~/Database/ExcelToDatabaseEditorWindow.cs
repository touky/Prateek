namespace Mayfair.Core.Editor.Database
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using Mayfair.Core.Code.MathExt;
    using Mayfair.Core.Code.Protocol.Database;
    using Mayfair.Core.Code.StateMachines;
    using Mayfair.Core.Code.Utils;
    using Mayfair.Core.Editor.Database.ExcelToDatabaseContent;
    using Mayfair.Core.Editor.Database.Protocol;
    using Mayfair.Core.Editor.EditorWindows;
    using Mayfair.Core.Editor.GUI;
    using Mayfair.Core.Editor.Protocol;
    using Mayfair.Core.Editor.Utils;
    using Mayfair.Plugins.AtDbMayfair.Editor;
    using Mayfair.Plugins.AtDbMayfair.Editor.ErrorSystem;
    using Mayfair.Plugins.AtDbMayfair.Editor.Tasks;
    using UnityEditor;
    using UnityEditor.VersionControl;
    using UnityEngine;

    public class ExcelToDatabaseEditorWindow : BaseProcessEditorWindow, ISerializationCallbackReceiver
    {
        #region Static and Constants
        private const FieldSlot EXCEL_IMPORT_DIR = FieldSlot.ProcessFolder;
        private const FieldSlot EXCEL_EXPORT_DIR = FieldSlot.ProcessFile;
        private const FieldSlot PROTO_CSHARPASSEMBLY = FieldSlot.CustomSlot + NEXT_SLOT;
        private const GUIDrawAction GUI_FOUND_FILES = GUIDrawAction.DrawCustom + NEXT_DRAW;
        private const GUIDrawAction GUI_FOUND_DB = GUI_FOUND_FILES + NEXT_DRAW;
        private const GUIDrawAction GUI_FOUND_ASSEMBLIES = GUI_FOUND_DB + NEXT_DRAW;

        private const string WINDOW_TITLE = "Database Builder";

        private static readonly string IGNORE_FILE = "IGNORE_FILE_{0}";
        private static readonly string IGNORE_FOLDER = "/{0}/";
        private static readonly string IGNORE_TEMPLATE = "Mayfair_Template";
        private static readonly string IMPORT_EXTENSION = "*.xlsx";
        private static readonly string EXPORT_EXTENSION = "*.protodb";
        #endregion

        #region Settings
        [SerializeField]
        private List<FileInfos> savedFileInfos;
        #endregion

        #region Fields
        private bool workStarted;
        private EditorPrefsWrapper codeExportPrefsWrapper;
        private string codeExportFile;
        private Dictionary<string, FileInfos> fileInfos = new Dictionary<string, FileInfos>();
        private List<string> exportedFiles = new List<string>();

        private ProtocolDatabaseExporterConfiguration exportConfig;
        private AtDbTasker tasker;
        #endregion

        #region Properties
        protected override bool IsWorking
        {
            get { return workStarted; }
        }
        #endregion

        #region Class Methods
        [MenuItem(EditorMenuOrderHelper.WINDOW_MENU + WINDOW_TITLE, priority = EditorMenuOrderHelper.BAR_MENU_GROUP0_2)]
        public static void Open()
        {
            DoOpen<ExcelToDatabaseEditorWindow>(WINDOW_TITLE);
        }

        protected override void OnInspectorUpdate()
        {
            base.OnInspectorUpdate();

            RefreshImportList();
            RefreshExportList();
        }

        public override void ExecuteState(EditorWorkState state)
        {
            base.ExecuteState(state);

            switch (state)
            {
                case EditorWorkState.AutoStop:
                {
                    stateMachine.Trigger(SequentialTriggerType.JumpToEnd);
                    AutoCancel();
                    autoExecuteFiles.Clear();
                    break;
                }
            }
        }

        protected override bool SetProperLogger()
        {
            if (logger is ErrorGUILogger)
            {
                return false;
            }

            logger = new ErrorGUILogger();
            return true;
        }
        #endregion

        #region ISerializationCallbackReceiver
        public void OnBeforeSerialize()
        {
            savedFileInfos = new List<FileInfos>(fileInfos.Values);
        }

        public void OnAfterDeserialize()
        {
            fileInfos.Clear();
            foreach (FileInfos infos in savedFileInfos)
            {
                fileInfos.Add(infos.path, infos);
            }

            savedFileInfos = null;
        }
        #endregion ISerializationCallbackReceiver

        #region Behaviour
        protected override void InitPrefHelper()
        {
            base.InitPrefHelper();

            if (codeExportPrefsWrapper != null)
            {
                return;
            }

            codeExportPrefsWrapper = new EditorPrefsWrapper(typeof(ProtocolToExcelExportEditorWindow).ToString());
        }

        protected override void RefreshDatas()
        {
            base.RefreshDatas();

            codeExportPrefsWrapper.Get((FieldSlot) ProtocolToExcelExportEditorWindow.EXCEL_FILE_EXTERN, ref codeExportFile, ProtocolToExcelExportEditorWindow.CODE_EXPORT_FILE);
        }

        protected override FieldSlotInfos GetFieldSlotInfos(FieldSlot slot)
        {
            FieldSlotInfos infos = new FieldSlotInfos();
            switch (slot)
            {
                case EXCEL_IMPORT_DIR:
                {
                    infos.title = "Excel directory";
                    infos.defaultValue = "../Database";
                    infos.status = FieldSlotTag.Enabled | FieldSlotTag.IsDirectory | FieldSlotTag.NeedExistenceCheck | FieldSlotTag.RelativeToAssetPath | FieldSlotTag.AddSpaceBefore;
                    break;
                }
                case EXCEL_EXPORT_DIR:
                {
                    infos.title = "Export file";
                    infos.defaultValue = "Database/Export";
                    infos.status = FieldSlotTag.Enabled | FieldSlotTag.IsDirectory | FieldSlotTag.NeedExistenceCheck | FieldSlotTag.RelativeToAssetPath;
                    break;
                }
                case PROTO_CSHARPASSEMBLY:
                {
                    infos.title = "Source Assembly";
                    infos.defaultValue = typeof(CoreEmptyDatabaseClass).Assembly.FullName;
                    infos.status = FieldSlotTag.Enabled | FieldSlotTag.AddSpaceBefore;
                    break;
                }
            }

            return infos;
        }

        protected override void GetFieldSlotValues(FieldSlot slot, FieldSlotInfos infos, ref string currentValue, ref string validationValue)
        {
            currentValue = GetFieldSlot(slot);
            validationValue = currentValue;

            ApplyDirectoryRules(slot, infos, ref validationValue);

            ApplyIORules(infos, ref validationValue);
        }

        protected void RefreshImportList()
        {
            if (autoExecuteFiles.Count > 0)
            {
                return;
            }

            string srcDir = string.Empty;
            GetFieldSlotValues(EXCEL_IMPORT_DIR, ref srcDir);

            string[] files = null;
            if (Directory.Exists(srcDir))
            {
                files = Directory.GetFiles(srcDir, IMPORT_EXTENSION, SearchOption.AllDirectories);
            }

            for (int f = 0; files != null && f < files.Length; f++)
            {
                string file = PathHelper.Simplify(files[f]);
                if (file.Contains(codeExportFile)
                 || file.Contains(IGNORE_TEMPLATE)
                 || file.Contains(string.Format(IGNORE_FOLDER, ProtocolToExcelExportEditorWindow.CODE_EXPORT))
                 || file.Contains(AtDbEditorConstants.EXCEL_TEMP_PREFIX))
                {
                    if (fileInfos.ContainsKey(file))
                    {
                        fileInfos.Remove(file);
                    }
                    continue;
                }

                if (!fileInfos.ContainsKey(file))
                {
                    fileInfos.Add(file, new FileInfos(file));
                }

                FileInfos infos = fileInfos[file];
                long writeTime = infos.GetLastWriteTime();

                FileInfosStatus status = writeTime != infos.lastWriteTime
                    ? infos.rebuildStatus | FileInfosStatus.WasWritten
                    : infos.rebuildStatus & ~FileInfosStatus.WasWritten;

                if (infos.rebuildStatus != status)
                {
                    infos.rebuildStatus = status;
                    fileInfos[file] = infos;
                }

                if (status.HasBoth(FileInfosStatus.WasWritten)
                 && !status.HasBoth(FileInfosStatus.ShouldIgnore)
                 && EditorUtilities.IsFocused)
                {
                    DoOpenWithAutoExecute<ExcelToDatabaseEditorWindow>(infos.path);
                }
            }
        }

        protected void RefreshExportList()
        {
            string srcDir = string.Empty;
            GetFieldSlotValues(EXCEL_EXPORT_DIR, ref srcDir);

            string[] files = null;
            if (Directory.Exists(srcDir))
            {
                files = Directory.GetFiles(srcDir, EXPORT_EXTENSION, SearchOption.AllDirectories);
            }

            exportedFiles.Clear();
            for (int f = 0; files != null && f < files.Length; f++)
            {
                string file = files[f];
                if (file.Contains(AtDbEditorConstants.EXCEL_TEMP_PREFIX))
                {
                    continue;
                }

                exportedFiles.Add(files[f]);
            }
        }

        protected void CleanDirtyFiles()
        {
            List<string> keys = new List<string>(fileInfos.Keys);
            foreach (string key in keys)
            {
                FileInfos infos = fileInfos[key];
                if (infos.rebuildStatus.HasBoth(FileInfosStatus.ShouldIgnore))
                {
                    continue;
                }

                infos.MarkWriteTime();
                fileInfos[key] = infos;
            }
        }

        protected override void BuildCommandArguments()
        {
            cmdLineFileName = string.Format("Files to export (Ignoring: \"{0}\", \"{1}\", and \"{2}\")", codeExportFile, IGNORE_TEMPLATE, AtDbEditorConstants.EXCEL_TEMP_PREFIX);
            cmdLineArguments = string.Empty;

            string srcDir = string.Empty;
            GetFieldSlotValues(EXCEL_IMPORT_DIR, ref srcDir);

            Dictionary<string, FileInfos>.KeyCollection keys = fileInfos.Keys;
            foreach (string key in keys)
            {
                FileInfos infos = fileInfos[key];
                if (infos.rebuildStatus == FileInfosStatus.Nothing)
                {
                    continue;
                }

                cmdLineArguments = string.Format("{0}\n{1}", cmdLineArguments, infos.path);
            }
        }
        #endregion Behaviour

        #region GUI Logic
        protected override void InitFieldSlotDrawOrder(List<FieldSlot> drawOrder)
        {
            if (drawOrder.Count != 0)
            {
                return;
            }

            drawOrder.AddRange(
                new[]
                {
                    EXCEL_IMPORT_DIR,
                    EXCEL_EXPORT_DIR,
                    PROTO_CSHARPASSEMBLY
                });
        }

        protected override void InitGUIDrawOrder(List<GUIDrawAction> drawOrder)
        {
            if (drawOrder.Count != 0)
            {
                return;
            }

            drawOrder.AddRange(new[]
            {
                GUIDrawAction.DrawTags,
                GUIDrawAction.DrawAutoStart,
                GUI_FOUND_FILES,
                GUI_FOUND_DB,
                GUI_FOUND_ASSEMBLIES,
                GUIDrawAction.DrawProcess,
                GUIDrawAction.DrawLogger
            });
        }

        protected override bool DrawGUIAction(GUIDrawAction drawAction)
        {
            if (base.DrawGUIAction(drawAction))
            {
                return true;
            }

            switch (drawAction)
            {
                case GUI_FOUND_FILES:
                {
                    DrawFilesToExport();
                    return true;
                }
                case GUI_FOUND_DB:
                {
                    DrawFilesExported();
                    return true;
                }
                case GUI_FOUND_ASSEMBLIES:
                {
                    DrawSourceAssemblies();
                    return true;
                }
            }

            return false;
        }
        #endregion GUI Logic

        #region Main GUI Logic
        private void DrawFilesToExport()
        {
            Rect titleRect = EditorGUILayout.GetControlRect();
            Rect[] titleRects = RectHelper.SplitX(ref titleRect, 1, Split.FixedSize(80));

            EditorGUI.LabelField(titleRects[0], "Found files for export:");
            if (GUI.Button(titleRects[1], "Clean list"))
            {
                fileInfos.Clear();
            }

            using (new EditorGUI.IndentLevelScope(ConstsEditor.ONE_INDENT))
            {
                List<string> keys = new List<string>(fileInfos.Keys);
                for (int k = 0; k < keys.Count; k++)
                {
                    string key = keys[k];
                    FileInfos infos = fileInfos[key];
                    string fileName = Path.GetFileName(infos.path);
                    FileInfosStatus status = infos.rebuildStatus & ~FileInfosStatus.ForceRebuild;

                    Rect rect = EditorGUILayout.GetControlRect();
                    int r = 0;
                    Rect[] rects = RectHelper.SplitX(ref rect, Split.FixedSize(80), Split.FixedSize(120), Split.FixedSize(120), 100, Split.FixedSize(120));

                    string tag = string.Format(IGNORE_FILE, fileName);
                    bool shouldIgnore = false;
                    prefsWrapper.Get(tag, ref shouldIgnore, false);

                    using (EditorGUI.ChangeCheckScope check = new EditorGUI.ChangeCheckScope())
                    {
                        shouldIgnore = EditorGUI.ToggleLeft(rects[r++], "Ignore", shouldIgnore);

                        if (check.changed)
                        {
                            prefsWrapper.Set(tag, shouldIgnore);
                        }
                    }

                    status = shouldIgnore ? status | FileInfosStatus.ShouldIgnore : status & ~FileInfosStatus.ShouldIgnore;

                    using (new EditorGUI.DisabledScope(shouldIgnore))
                    {
                        using (EditorGUI.ChangeCheckScope check = new EditorGUI.ChangeCheckScope())
                        {
                            status |= EditorGUI.ToggleLeft(rects[r++], "Force Rebuild", infos.rebuildStatus.HasBoth(FileInfosStatus.ForceRebuild))
                                ? FileInfosStatus.ForceRebuild
                                : FileInfosStatus.Nothing;

                            if (check.changed)
                            {
                                infos.rebuildStatus = status;
                                fileInfos[key] = infos;
                            }
                        }

                        FileAttributes attributes = File.GetAttributes(infos.path);
                        if ((attributes & FileAttributes.ReadOnly) != 0)
                        {
                            GUIHelper.ShowStatusBox(rects[r++], false, string.Empty, string.Empty, "READ ONLY");
                        }
                        else
                        {
                            GUIHelper.ShowStatusBox(rects[r++], (infos.rebuildStatus & FileInfosStatus.WasWritten) == 0, "", "UP TO DATE", "NEED REBUILD");
                        }

                        EditorGUI.LabelField(rects[r++], fileName);
                    }

                    if (GUI.Button(rects[r++], "Show in Perforce"))
                    {
                        SourceControlHelper.SelectInPerforce(infos.path);
                    }
                }
            }
        }

        private void DrawFilesExported()
        {
            string exportedTag = GetType() + "ExportedFiles";
            bool showExported = false;
            prefsWrapper.Get(exportedTag, ref showExported);
            using (EditorGUI.ChangeCheckScope check = new EditorGUI.ChangeCheckScope())
            {
                showExported = EditorGUILayout.Foldout(showExported, "Found exported files:", true);
                if (check.changed)
                {
                    prefsWrapper.Set(exportedTag, showExported);
                }
            }

            if (showExported)
            {
                using (new EditorGUI.IndentLevelScope(ConstsEditor.ONE_INDENT))
                {
                    for (int k = 0; k < exportedFiles.Count; k++)
                    {
                        string path = exportedFiles[k];
                        string fileName = Path.GetFileNameWithoutExtension(path);

                        Rect rect = EditorGUILayout.GetControlRect();
                        int r = 0;
                        Rect[] rects = RectHelper.SplitX(ref rect, Split.FixedSize(140), 120, Split.FixedSize(6), Split.FixedSize(80));

                        string tag = string.Format(IGNORE_FILE, fileName);
                        bool shouldIgnore = false;
                        prefsWrapper.Get(tag, ref shouldIgnore, false);

                        using (new EditorGUI.DisabledScope(shouldIgnore))
                        {
                            EditorGUI.LabelField(rects[r++], fileName);
                            EditorGUI.LabelField(rects[r++], path);
                            r++;
                        }

                        if (GUI.Button(rects[r++], "Delete"))
                        {
                            Task task = Provider.Delete(path);
                            task.Wait();

                            for (int m = 0; m < task.messages.Length; m++)
                            {
                                task.messages[m].Show();
                                logger.Log(task.messages[m].message);
                            }
                        }
                    }
                }
            }
        }

        private void DrawSourceAssemblies()
        {
            string foldoutTag = GetType() + "SourceAssemblies";
            bool showAssemblies = false;
            prefsWrapper.Get(foldoutTag, ref showAssemblies);
            using (EditorGUI.ChangeCheckScope check = new EditorGUI.ChangeCheckScope())
            {
                showAssemblies = EditorGUILayout.Foldout(showAssemblies, "Found source assemblies:", true);
                if (check.changed)
                {
                    prefsWrapper.Set(foldoutTag, showAssemblies);
                }
            }

            if (showAssemblies)
            {
                List<Assembly> assemblies = Code.Utils.Helpers.AssemblyHelper.GetAssembliesMatching(ProtocolHelper.ASSEMBLY_MATCH[0], ProtocolHelper.ASSEMBLY_MATCH[1]);
                assemblies.Sort((a, b) => { return string.Compare(a.FullName, b.FullName); });
                using (new EditorGUI.IndentLevelScope(ConstsEditor.ONE_INDENT))
                {
                    foreach (Assembly assembly in assemblies)
                    {
                        EditorGUILayout.LabelField(assembly.FullName);
                    }
                }
            }
        }
        #endregion Main GUI Logic

        #region Process Execution
        protected override void PreExecuteProcess()
        {
            base.PreExecuteProcess();

            processPassCount = Consts.THREE_PASS;
        }

        protected override bool ExecuteProcess(int pass)
        {
            base.ExecuteProcess(pass);

            switch (pass)
            {
                case Consts.FIRST_PASS:
                {
                    workStarted = true;

                    string exportDir = string.Empty;
                    GetFieldSlotValues(EXCEL_EXPORT_DIR, ref exportDir);

                    string sourceAssembly = string.Empty;
                    GetFieldSlotValues(PROTO_CSHARPASSEMBLY, ref sourceAssembly);

                    exportConfig = new ProtocolDatabaseExporterConfiguration
                    {
                        exportFullPath = exportDir,
                        exportAssetPath = SourceControlHelper.DirectoryToAsset(exportDir),
                        fileExtension = ".protodb",
                        sourceAssemblies = Code.Utils.Helpers.AssemblyHelper.GetAssembliesMatching(ProtocolHelper.ASSEMBLY_MATCH[0], ProtocolHelper.ASSEMBLY_MATCH[1])
                    };

                    string[] files = cmdLineArguments.Split(new[] {'\n'}, StringSplitOptions.RemoveEmptyEntries);

                    //DatabaseTools.ExportDatabase(exportConfig, files);
                    tasker = files.Length == 0 ? null : DatabaseTools.GetTasker(exportConfig, files);
                    break;
                }
                case Consts.SECOND_PASS:
                {
                    TaskStatusType status = tasker != null ? tasker.PerformTask() : TaskStatusType.Error;

                    LogTasker(false);

                    return status != TaskStatusType.Working;
                }
                case Consts.THIRD_PASS:
                {
                    bool success = !exportConfig.ErrorLogger.Has(NoticeType.Error);

                    LogTasker(true);

                    if (success)
                    {
                        logger.Log("Export to {0} Successful", exportConfig.exportFullPath);
                        exitCode = 0;
                    }
                    else
                    {
                        logger.Log("{0} Export to {1} Had errors", LOG_ERROR, exportConfig.exportFullPath);
                        exitCode = 1;
                    }

                    exportConfig = null;
                    break;
                }
            }

            return true;
        }

        private void LogTasker(bool doCleanUp)
        {
            ErrorGUILogger errorLogger = logger as ErrorGUILogger;
            if (errorLogger != null)
            {
                if (tasker != null)
                {
                    tasker.ErrorLogger.Flush(doCleanUp);
                    errorLogger.LogNotices(exportConfig);
                    errorLogger.PrintNotices(LOG_ERROR, LOG_WARNING, LOG_TASK);
                }
            }
        }

        protected override void OnProcessEnded()
        {
            if (exitCode == 0)
            {
                AutoPause();
                CleanDirtyFiles();
            }

            base.OnProcessEnded();

            AssetDatabase.Refresh();

            workStarted = false;
        }
        #endregion Process Execution
    }
}
