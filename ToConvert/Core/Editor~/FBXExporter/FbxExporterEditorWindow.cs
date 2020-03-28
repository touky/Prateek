namespace Mayfair.Core.Editor.FBXExporter
{
    using System;
    using System.Collections.Generic;
    using Mayfair.Core.Code.Utils;
    using Mayfair.Core.Code.Utils.Types.Priority;
    using Mayfair.Core.Editor.EditorWindows;
    using Mayfair.Core.Editor.FBXExporter.Jobs;
    using UnityEditor;

    public class FbxExporterEditorWindow : BaseProcessEditorWindow
    {
        #region Static and Constants
        public const string PASS_PREFIX = "PASS[";
        public const string PASS_SUFFIX = "]";
        public const int PASS_INGAME = 0;
        public const int PASS_REFERENCE = 1;
        public const int PASS_UNIQUE = 2;
        public const int PASS_LOD = 3;

        public const int PASS_ANIMATIONS = 100;
        public const int PASS_ANIM_LIBRARY = 101;

        private const int PREFAB_EXPORT_PRIORITY = 40;
        private const string WINDOW_TITLE = "FBX Exporter Editor";
        private const FieldSlot ROOT_FOLDER = FieldSlot.ProcessFolder;
        private const FieldSlot EXPORT_FOLDER = FieldSlot.ProcessFile;
        public static readonly string PASS_FORMAT = $"{PASS_PREFIX}{{0}}{PASS_SUFFIX}{{1}}";
        #endregion

        #region Fields
        private List<FbxExporterJob> jobs = new List<FbxExporterJob>();
        #endregion

        #region Properties
        protected override double AutoExecOpenDuration
        {
            get { return .5; }
        }

        protected override double AutoExecCloseDuration
        {
            get { return 5f; }
        }

        protected override double WaitForCompilingDuration
        {
            get { return .1; }
        }
        #endregion

        #region Class Methods
        [MenuItem(EditorMenuOrderHelper.WINDOW_MENU + WINDOW_TITLE, priority = EditorMenuOrderHelper.BAR_MENU_GROUP1_0)]
        public static void Open()
        {
            DoOpen<FbxExporterEditorWindow>(WINDOW_TITLE);
        }

        protected override void InitFieldSlotDrawOrder(List<FieldSlot> drawOrder)
        {
            if (drawOrder.Count != 0)
            {
                return;
            }

            drawOrder.AddRange(
                new[]
                {
                    ROOT_FOLDER,
                    EXPORT_FOLDER
                });
        }

        protected override FieldSlotInfos GetFieldSlotInfos(FieldSlot slot)
        {
            FieldSlotInfos infos = base.GetFieldSlotInfos(slot);
            switch (slot)
            {
                case ROOT_FOLDER:
                {
                    infos.title = "Art root directory";
                    infos.status = FieldSlotTag.Enabled | FieldSlotTag.NeedExistenceCheck | FieldSlotTag.IsDirectory | FieldSlotTag.RelativeToAssetPath;
                    infos.defaultValue = ConstsFolders.ART_ROOT;
                    break;
                }
                case EXPORT_FOLDER:
                {
                    infos.title = "Export directory";
                    infos.status = FieldSlotTag.Enabled | FieldSlotTag.NeedExistenceCheck | FieldSlotTag.IsDirectory | FieldSlotTag.RelativeToOtherPath;
                    infos.defaultValue = ConstsFolders.ART_EXPORT_ROOT;
                    break;
                }
            }

            return infos;
        }

        protected override string GetRelativePath(FieldSlot slot, FieldSlotInfos infos)
        {
            switch (slot)
            {
                case EXPORT_FOLDER:
                {
                    string srcDir = string.Empty;
                    GetFieldSlotValues(ROOT_FOLDER, ref srcDir);
                    return srcDir;
                }
            }

            return string.Empty;
        }

        public static void ExportLODs(string path)
        {
            DoOpenWithAutoExecute<FbxExporterEditorWindow>(path);
        }

        protected override void PreExecuteProcess()
        {
            base.PreExecuteProcess();

            foreach (string autoFile in autoExecuteFiles)
            {
                string file = autoFile;
                int passStarted = Consts.INDEX_NONE;
                if (file.StartsWith(PASS_PREFIX))
                {
                    file = file.Remove(0, PASS_PREFIX.Length);
                    int suffix = file.IndexOf(PASS_SUFFIX);
                    passStarted = Convert.ToInt16(file.Substring(0, suffix));
                    file = file.Substring(suffix + 1, file.Length - (suffix + 1));
                }

                if (passStarted == Consts.INDEX_NONE || passStarted >= PASS_LOD)
                {
                    FbxExporterReimportJob reimportJob = new FbxExporterReimportJob(file);
                    FbxExporterFileExportJob exportJob = new FbxExporterFileExportJob(file, reimportJob);
                    LodFbxCreationJob creationJob = new LodFbxCreationJob(file, exportJob);
                    jobs.Add(creationJob);
                    jobs.Add(exportJob);
                    jobs.Add(reimportJob);
                }

                if (passStarted == Consts.INDEX_NONE || passStarted == PASS_UNIQUE)
                {
                    FbxExporterReimportJob reimportJob = new FbxExporterReimportJob(file);
                    FbxExporterFileExportJob exportJob = new FbxExporterFileExportJob(file, reimportJob);
                    UniqueFbxCreationJob creationJob = new UniqueFbxCreationJob(file, exportJob);
                    jobs.Add(creationJob);
                    jobs.Add(exportJob);
                    jobs.Add(reimportJob);
                }

                if (passStarted == Consts.INDEX_NONE || passStarted == PASS_REFERENCE)
                {
                    FbxExporterReimportJob reimportJob = new FbxExporterReimportJob(file, PrefabGenerationJob.DEFAULT_PRIORITY + Consts.THIRD_PASS);
                    FbxExporterFileExportJob exportJob = new FbxExporterFileExportJob(file, reimportJob, PrefabGenerationJob.DEFAULT_PRIORITY + Consts.SECOND_PASS);
                    PrefabGenerationJob creationJob = new PrefabGenerationJob(file, exportJob);
                    jobs.Add(creationJob);
                    jobs.Add(exportJob);
                    jobs.Add(reimportJob);
                }
            }

            jobs.SortWithPriorities();

            foreach (FbxExporterJob job in jobs)
            {
                logger.Log(LOG_TASK, $"> {job.GetType().Name} for path \"{job.SourcePath}\"");
                job.PreExecute();
            }

            processPassCount = jobs.Count;
        }

        protected override bool ExecuteProcess(int pass)
        {
            FbxExporterJob job = jobs[pass];
            logger.Log(LOG_TASK, $"> {job.GetType().Name} for path \"{job.SourcePath}\"");
            job.Logger = logger;
            job.Execute();
            return job.JobDone;
        }

        protected override void PostExecuteProcess()
        {
            base.PostExecuteProcess();

            foreach (FbxExporterJob job in jobs)
            {
                logger.Log(LOG_TASK, $"> {job.GetType().Name} for path \"{job.SourcePath}\"");
                job.PostExecute();
            }

            jobs.Clear();
        }
        #endregion
    }
}
