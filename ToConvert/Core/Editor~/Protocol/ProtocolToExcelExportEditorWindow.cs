namespace Mayfair.Core.Editor.Protocol
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Mayfair.Core.Code.GUIExt;
    using Mayfair.Core.Editor.Database.ExcelToDatabaseContent;
    using Mayfair.Core.Editor.EditorWindows;
    using Mayfair.Plugins.AtDbMayfair.Editor;
    using UnityEditor;

    public class ProtocolToExcelExportEditorWindow : BaseProcessEditorWindow
    {
        #region ProtocolLogOptions enum
        private enum ProtocolLogOptions
        {
            Nothing = 0,

            LogFilesToExport = 1 << 0
        }
        #endregion

        #region Static and Constants
        private const FieldSlot PROTO_CSHARP_DB_CLASS = FieldSlot.CustomSlot + NEXT_SLOT;
        private const FieldSlot PROTO_CSHARP_NMSPC_ROOT = PROTO_CSHARP_DB_CLASS + NEXT_SLOT;
        private const FieldSlot EXCEL_DIR = FieldSlot.ProcessFolder;
        private const FieldSlot EXCEL_FILE = FieldSlot.ProcessFile;

        public static int EXCEL_FILE_EXTERN = (int)EXCEL_FILE;
        public const string CODE_EXPORT = "CODE_EXPORT";
        public static readonly string CODE_EXPORT_FILE = $"{CODE_EXPORT}.xlsx";

        private const string WINDOW_TITLE = "Code Exporter";
        #endregion

        #region Fields
        private ProtocolLogOptions logOptions = ProtocolLogOptions.Nothing;
        private bool workStarted;

        private FormatExporterConfiguration exporterConfiguration;
        #endregion

        #region Class Methods
        private bool Check(ProtocolLogOptions option)
        {
            return (this.logOptions & option) != ProtocolLogOptions.Nothing;
        }

        protected override void DrawLogOptions()
        {
            DrawLogOptions(ref this.logOptions, (a, b) => { return a | b; });
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
                    PROTO_CSHARP_NMSPC_ROOT,
                    PROTO_CSHARP_DB_CLASS,
                    EXCEL_DIR,
                    EXCEL_FILE
                });
        }

        [MenuItem(EditorMenuOrderHelper.WINDOW_MENU + WINDOW_TITLE, priority = EditorMenuOrderHelper.BAR_MENU_GROUP0_1)]
        public static void Open()
        {
            DoOpen<ProtocolToExcelExportEditorWindow>(WINDOW_TITLE);
        }

        public static void Export(FormatExporterConfiguration configuration)
        {
            DoOpenWithAutoExecute<ProtocolToExcelExportEditorWindow>(configuration.ExportDestination);
            ProtocolToExcelExportEditorWindow instance = GetWindow<ProtocolToExcelExportEditorWindow>();
            instance.exporterConfiguration = configuration;
        }

        #region Base Behaviour
        protected override bool SetProperLogger()
        {
            if (this.logger is ErrorGUILogger)
            {
                return false;
            }

            this.logger = new ErrorGUILogger();
            return true;
        }
        #endregion
        #endregion

        #region Behaviour
        protected override FieldSlotInfos GetFieldSlotInfos(FieldSlot slot)
        {
            FieldSlotInfos infos = new FieldSlotInfos();
            switch (slot)
            {
                case PROTO_CSHARP_NMSPC_ROOT:
                {
                    infos.title = "Protocol root namespace";
                    infos.defaultValue = ProtocolHelper.GetRootClassForAssembly("Core");
                    infos.status = FieldSlotTag.Enabled | FieldSlotTag.IsType | FieldSlotTag.NeedExistenceCheck | FieldSlotTag.AddSpaceBefore;
                    break;
                }
                case PROTO_CSHARP_DB_CLASS:
                {
                    infos.title = "Source Namespace";
                    infos.defaultValue = ProtocolHelper.GetDatabaseClassForAssembly("Core");
                    infos.status = FieldSlotTag.Enabled | FieldSlotTag.IsType | FieldSlotTag.NeedExistenceCheck;
                    break;
                }
                case EXCEL_DIR:
                {
                    infos.title = "Excel directory";
                    infos.defaultValue = "../Database";
                    infos.status = FieldSlotTag.Enabled | FieldSlotTag.IsDirectory | FieldSlotTag.NeedExistenceCheck | FieldSlotTag.RelativeToAssetPath | FieldSlotTag.AddSpaceBefore;
                    break;
                }
                case EXCEL_FILE:
                {
                    infos.title = "Export file";
                    infos.defaultValue = CODE_EXPORT_FILE;
                    infos.status = FieldSlotTag.Enabled | FieldSlotTag.IsFile | FieldSlotTag.NeedCheckout | FieldSlotTag.NeedExistenceCheck;
                    break;
                }
            }

            return infos;
        }

        protected override void BuildCommandArguments()
        {
            string option = string.Empty;
            GetFieldSlotValues(PROTO_CSHARP_DB_CLASS, ref option);

            this.cmdLineArguments = string.Format("\n{0}", option);
        }
        #endregion Behaviour

        #region Process Execution
        protected override void PreExecuteProcess()
        {
            base.PreExecuteProcess();
        }

        protected override bool ExecuteProcess(int pass)
        {
            base.ExecuteProcess(pass);

            string excelFile = string.Empty;
            if (this.exporterConfiguration == null)
            {
                string rootNamespace = string.Empty;
                GetFieldSlotValues(PROTO_CSHARP_NMSPC_ROOT, ref rootNamespace);

                string protoNamespace = string.Empty;
                GetFieldSlotValues(PROTO_CSHARP_DB_CLASS, ref protoNamespace);

                string excelPath = string.Empty;
                GetFieldSlotValues(EXCEL_FILE, ref excelFile, ref excelPath);

                Type rootType = AssemblyHelper.GetTypeFromAnyAssembly(rootNamespace);
                Type dbType = AssemblyHelper.GetTypeFromAnyAssembly(protoNamespace);
                int index = rootType.Namespace.LastIndexOf(".");
                string name = rootType.Namespace.Substring(index + 1, rootType.Namespace.Length - (index + 1)) + dbType.Namespace.Replace(rootType.Namespace, string.Empty);
                if (name.StartsWith("."))
                {
                    name = name.Substring(1);
                }

                List<Assembly> assemblies = Code.Utils.Helpers.AssemblyHelper.GetAssembliesMatching(ProtocolHelper.ASSEMBLY_MATCH[0], ProtocolHelper.ASSEMBLY_MATCH[1]);
                this.exporterConfiguration = new FormatExporterConfiguration(name, assemblies, excelPath, ProtocolHelper.KEYWORD_IGNORE, ProtocolHelper.TYPE_IGNORE);
            }
            else
            {
                string excelPath = string.Empty;
                GetFieldSlotValues(EXCEL_FILE, ref excelFile, ref excelPath);

                excelPath = excelPath.Replace(excelFile, this.exporterConfiguration.ExportDestination);

                excelFile = this.exporterConfiguration.ExportDestination;
                this.exporterConfiguration.ExportDestination = excelPath;
            }

            IFormatExportData exportData = DatabaseTools.GatherExportData(this.exporterConfiguration);
            bool success = exportData.HasContentToExport;
            if (!success)
            {
                this.logger.Log(LOG_ERROR, "Nothing to export");
            }

            this.logger.Log("Found {0} types to export", exportData.ExportedTypes.Count);
            for (int t = 0; t < exportData.ExportedTypes.Count; t++)
            {
                this.logger.Log(" - {0}", exportData.ExportedTypes[t].FullName);
            }

            success = success && DatabaseTools.ExportNamespaceContent(this.exporterConfiguration, exportData);

            ErrorGUILogger errorLogger = this.logger as ErrorGUILogger;
            if (errorLogger != null)
            {
                errorLogger.LogNotices(this.exporterConfiguration);
                errorLogger.PrintNotices(LOG_ERROR, LOG_WARNING, LOG_TASK);
            }

            if (success)
            {
                this.logger.Log("Export to {0} Successful", excelFile);
                this.exitCode = 0;
            }
            else
            {
                this.logger.Log("{0} Export to {1} FAILED", LOG_ERROR, excelFile);
                this.exitCode = 1;
            }

            return true;
        }

        protected override void OnProcessEnded()
        {
            base.OnProcessEnded();

            this.exporterConfiguration = null;
        }
        #endregion Process Execution
    }
}
