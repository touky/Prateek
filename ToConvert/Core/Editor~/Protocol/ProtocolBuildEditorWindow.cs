namespace Mayfair.Core.Editor.Protocol
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using Mayfair.Core.Code.StateMachines;
    using Mayfair.Core.Code.Utils;
    using Mayfair.Core.Editor.EditorWindows;
    using Mayfair.Core.Editor.Utils;
    using Mayfair.Plugins.AtDbMayfair.Editor;
    using UnityEditor;

    public class ProtocolBuildEditorWindow : CommandLineEditorWindow
    {
        #region FileChange enum
        private enum FileChange
        {
            TempSave,
            TempDelete,
            TempRollback,
            SourceControl
        }
        #endregion

        #region ProtocolLogOptions enum
        private enum ProtocolLogOptions
        {
            Nothing = 0,

            LogFiles = 1 << 0,
            LogCodeGen = 1 << 1
        }
        #endregion

        #region Static and Constants
        private const string DEFAULT = "GameSimulation";

        private const EditorWorkState PROCESS_PROTO_COMPILE = EditorWorkState.WorkTaskCustom + NEXT_WORK;
        private const EditorWorkState PROCESS_PROTO_COMPILE_WAIT = PROCESS_PROTO_COMPILE + WORK_WAIT_FOR;
        private const EditorWorkState PROCESS_DB_INTEGRATION = PROCESS_PROTO_COMPILE_WAIT + NEXT_WORK;

        private const FieldSlot PROTO_APIVERSION = FieldSlot.CustomSlot + NEXT_SLOT;

        private const FieldSlot SCRIPT_ROOT_DIR = PROTO_ROOT_DIR + NEXT_SLOT;
        private const FieldSlot PROTO_ROOT_DIR = PROTO_CSHARP_DB_CLASS + NEXT_SLOT;
        private const FieldSlot PROTO_SOURCES_DIR = FieldSlot.ArgumentSrcDir;
        private const FieldSlot PROTO_EXPORT_DIR = SCRIPT_ROOT_DIR + NEXT_SLOT;

        private const FieldSlot PROTO_CSHARP_TAG = FieldSlot.ArgumentDstTag;
        private const FieldSlot PROTO_CSHARP_DIR = FieldSlot.ArgumentDstDir;
        private const FieldSlot PROTO_CSHARP_INTEGRATION = PROTO_JSCRIPTOPTION + NEXT_SLOT;
        private const FieldSlot PROTO_CSHARP_DB_CLASS = PROTO_CSHARP_DATAENTRY + NEXT_SLOT;
        private const FieldSlot PROTO_CSHARP_CONTAINERENTRY = PROTO_CSHARP_INTEGRATION + NEXT_SLOT;
        private const FieldSlot PROTO_CSHARP_DATAENTRY = PROTO_CSHARP_CONTAINERENTRY + NEXT_SLOT;
        private const FieldSlot PROTO_CSHARP_OPTION = PROTO_APIVERSION + NEXT_SLOT;
        private const FieldSlot PROTO_CSHARP_OPTION_NMSPC_DIR = PROTO_EXPORT_DIR + NEXT_SLOT;
        private const FieldSlot PROTO_CSHARP_OPTION_NMSPC_ROOT = PROTO_CSHARP_OPTION_NMSPC_DIR + NEXT_SLOT;
        private const FieldSlot PROTO_CSHARP_OPTION_EXTENSION = PROTO_CSHARP_OPTION_NMSPC_ROOT + NEXT_SLOT;

        private const FieldSlot PROTO_JSCRIPTTAG = PROTO_CSHARP_OPTION + NEXT_SLOT;
        private const FieldSlot PROTO_JSCRIPTBIN = PROTO_JSCRIPTTAG + NEXT_SLOT;
        private const FieldSlot PROTO_JSCRIPTDIR = PROTO_JSCRIPTBIN + NEXT_SLOT;
        private const FieldSlot PROTO_JSCRIPTOPTION = PROTO_JSCRIPTDIR + NEXT_SLOT;

        private const string WINDOW_TITLE = "Protocol Builder";
        private static readonly string IMPORT_EXTENSION = "*.proto";
        private static readonly string INTEGRATION_EXTENSION = ".cs";
        private static readonly FieldSlot[] EXPORT_DIRECTORIES = {PROTO_CSHARP_DIR, PROTO_JSCRIPTDIR, PROTO_EXPORT_DIR};
        private static readonly string[] EXPORT_EXTENSIONS = {"*.cs", "*.js", "*.cs"};
        #endregion

        #region Fields
        private ProtocolLogOptions logOptions = ProtocolLogOptions.Nothing;
        private string assemblyName = DEFAULT;
        private string[] directories = null;
        #endregion

        #region Class Methods
        [MenuItem(EditorMenuOrderHelper.WINDOW_MENU + WINDOW_TITLE, priority = EditorMenuOrderHelper.BAR_MENU_GROUP0_0)]
        public static void Open()
        {
            DoOpen<ProtocolBuildEditorWindow>(WINDOW_TITLE);
        }

        protected override bool AcceptAutoExecute(string path)
        {
            //Refuse auto-execution if the file is read-only, this is to prevent first-time import triggering a rebuild.
            // i.e.: You need to have modified the file to get the compile process.
            FileAttributes attributes = File.GetAttributes(path);
            if ((attributes & FileAttributes.ReadOnly) != 0)
            {
                return false;
            }

            return true;
        }

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
                    FieldSlot.ProcessFolder,
                    PROTO_APIVERSION,
                    FieldSlot.ProcessFile,
                    SCRIPT_ROOT_DIR,
                    PROTO_ROOT_DIR,
                    FieldSlot.ArgumentSrcTag,
                    PROTO_SOURCES_DIR,
                    PROTO_EXPORT_DIR,
                    PROTO_CSHARP_TAG,
                    PROTO_CSHARP_DIR,
                    PROTO_CSHARP_OPTION,
                    PROTO_CSHARP_OPTION_NMSPC_DIR,
                    PROTO_CSHARP_OPTION_NMSPC_ROOT,
                    PROTO_CSHARP_OPTION_EXTENSION,
                    PROTO_CSHARP_DB_CLASS,
                    PROTO_CSHARP_INTEGRATION,
                    PROTO_CSHARP_CONTAINERENTRY,
                    PROTO_CSHARP_DATAENTRY,
                    PROTO_JSCRIPTTAG,
                    PROTO_JSCRIPTDIR,
                    PROTO_JSCRIPTBIN,
                    PROTO_JSCRIPTOPTION
                });
        }
        #endregion GUI Logic

        private bool Check(ProtocolLogOptions option)
        {
            return (logOptions & option) != ProtocolLogOptions.Nothing;
        }

        protected override void DrawLogOptions()
        {
            DrawLogOptions(ref logOptions, (a, b) => { return a | b; });
        }

        public override void ExecuteState(EditorWorkState state)
        {
            base.ExecuteState(state);

            switch (state)
            {
                case PROCESS_PROTO_COMPILE:
                {
                    CheckForCompiling();

                    EditorApplication.UnlockReloadAssemblies();
                    AssetDatabase.Refresh();

                    saveState = stateMachine.NextState;
                    break;
                }
                case PROCESS_PROTO_COMPILE_WAIT:
                {
                    if (!IsCompilingDone)
                    {
                        stateMachine.Trigger(SequentialTriggerType.PreventStateChange);
                    }

                    break;
                }
                case PROCESS_DB_INTEGRATION:
                {
                    FinishDatabaseIntegration();
                    break;
                }
            }
        }

        public override string GetStateName(EditorWorkState state)
        {
            switch (state)
            {
                case PROCESS_PROTO_COMPILE:
                {
                    return "Check protocol compiling";
                }
                case PROCESS_PROTO_COMPILE_WAIT:
                {
                    return "Wait for protocol compiling";
                }
                case PROCESS_DB_INTEGRATION:
                {
                    return "Finishing integration";
                }
                default:
                {
                    return base.GetStateName(state);
                }
            }
        }
        #endregion

        #region Code generation
        private bool GenerateDatabaseIntegration(bool reset = false)
        {
            logger.Log(LOG_STORY, $"Generating integration code: Reset [{reset}]");

            //Generate database code then
            string csharpFile = string.Empty;
            FieldSlotInfos csharpFileInfos = GetFieldSlotInfos(PROTO_CSHARP_INTEGRATION);
            GetFieldSlotValues(PROTO_CSHARP_INTEGRATION, csharpFileInfos, ref csharpFile);

            string csharpOptionExtension = string.Empty;
            bool csharpToggleExtension = true;
            GetFieldSlotValues(PROTO_CSHARP_OPTION_EXTENSION, ref csharpOptionExtension);
            prefsWrapper.Get(PROTO_CSHARP_OPTION_EXTENSION, ref csharpToggleExtension);

            {
                FieldSlotInfos protoExportInfos = GetFieldSlotInfos(PROTO_EXPORT_DIR);

                string protoDbClass = string.Empty;
                GetFieldSlotValues(PROTO_CSHARP_DB_CLASS, ref protoDbClass);

                string containerEntry = string.Empty;
                GetFieldSlotValues(PROTO_CSHARP_CONTAINERENTRY, ref containerEntry);

                string dataEntry = string.Empty;
                GetFieldSlotValues(PROTO_CSHARP_DATAENTRY, ref dataEntry);

                List<string> searchEntries = new List<string>(dataEntry.Split(new[] {';'}, StringSplitOptions.RemoveEmptyEntries));
                List<string> dataEntries = new List<string>();
                List<List<string>> patchEntries = new List<List<string>>();

                List<Assembly> assemblies = Code.Utils.Helpers.AssemblyHelper.GetAssembliesMatching(ProtocolHelper.ASSEMBLY_MATCH[0], ProtocolHelper.ASSEMBLY_MATCH[1]);
                Type dbClassType = Plugins.AtDbMayfair.Editor.AssemblyHelper.GetTypeFromAnyAssembly(protoDbClass);
                string namespaceName = dbClassType.Namespace;
                FormatExporterConfiguration configuration = new FormatExporterConfiguration(namespaceName, assemblies, string.Empty, ProtocolHelper.KEYWORD_IGNORE, ProtocolHelper.TYPE_IGNORE);
                IFormatExportData exportData = DatabaseTools.GatherExportData(configuration);
                bool success = exportData.HasContentToExport;
                if (success)
                {
                    ProtocolBuildCodeGenerator generator = new ProtocolBuildCodeGenerator();
                    generator.Init(assemblyName, namespaceName, containerEntry);

                    string srcDir = string.Empty;
                    GetFieldSlotValues(PROTO_SOURCES_DIR, ref srcDir);

                    //Gather proto files comments first
                    generator.BuildComments(srcDir, reset, list =>
                    {
                        return GatherFiles(srcDir, list, IMPORT_EXTENSION, AtDbEditorConstants.EXCEL_TEMP_PREFIX);
                    });

                    //Sort by namespace to help adding inner namespaces
                    exportData.ExportedTypes.Sort((a, b) => { return string.Compare(a.Namespace, b.Namespace); });

                    if (Check(ProtocolLogOptions.LogCodeGen))
                    {
                        logger.Log(LOG_STORY, $"Analyzing {protoDbClass}");
                    }

                    //Export All the types
                    {
                        for (int t = 0; t < exportData.ExportedTypes.Count; t++)
                        {
                            Type type = exportData.ExportedTypes[t];
                            if (Check(ProtocolLogOptions.LogCodeGen))
                            {
                                logger.Log(LOG_STORY, $"Analyzing {type.FullName}");
                            }

                            if (type.IsEnum)
                            {
                                if (Check(ProtocolLogOptions.LogCodeGen))
                                {
                                    logger.Log(LOG_ERROR, $" Rejected {type.FullName}: Is Enum");
                                }

                                continue;
                            }

                            //Gather all the Properties with set;
                            PropertyInfo[] properties = type.GetProperties(AtDbConstants.PROPERTY_SET_FLAG);
                            if (properties.Length == 0)
                            {
                                if (Check(ProtocolLogOptions.LogCodeGen))
                                {
                                    logger.Log(LOG_ERROR, $" Rejected {type.FullName}: No SET Properties");
                                }

                                continue;
                            }

                            PropertyInfo listProp = null;
                            TypeInfo typeInfo = null;

                            //Retrieve the first IList
                            for (int p = 0; p < properties.Length; p++)
                            {
                                PropertyInfo property = properties[p];
                                if (!typeof(ICollection).IsAssignableFrom(property.PropertyType))
                                {
                                    if (Check(ProtocolLogOptions.LogCodeGen))
                                    {
                                        logger.Log(LOG_ERROR, $" Rejected {type.FullName}.{property.Name}: Not IList ");
                                    }

                                    continue;
                                }

                                typeInfo = property.PropertyType.GetTypeInfo();
                                if (typeInfo.GenericTypeArguments.Length != 1)
                                {
                                    if (Check(ProtocolLogOptions.LogCodeGen))
                                    {
                                        logger.Log(LOG_ERROR, $" Rejected {type.FullName}.{property.Name}: Wrong type");
                                    }

                                    continue;
                                }

                                if (property.Name != containerEntry)
                                {
                                    if (Check(ProtocolLogOptions.LogCodeGen))
                                    {
                                        logger.Log(LOG_ERROR, $" Rejected {type.FullName}.{property.Name}: Not {containerEntry}");
                                    }

                                    continue;
                                }

                                listProp = property;
                                break;
                            }

                            if (listProp == null)
                            {
                                if (Check(ProtocolLogOptions.LogCodeGen))
                                {
                                    logger.Log(LOG_ERROR, $" Rejected {type.FullName}: No list property found");
                                }

                                continue;
                            }

                            //We're exposing the content of the IList first generic
                            Type genericType = typeInfo.GenericTypeArguments[0];
                            properties = genericType.GetProperties(AtDbConstants.PROPERTY_SET_FLAG);
                            if (properties.Length == 0)
                            {
                                if (Check(ProtocolLogOptions.LogCodeGen))
                                {
                                    logger.Log(LOG_ERROR, $" Rejected {type.FullName}");
                                }

                                continue;
                            }

                            if (Check(ProtocolLogOptions.LogCodeGen))
                            {
                                logger.Log(LOG_STORY, $"Analyzing Container {type.Name}/{genericType.Name}");
                            }

                            //Find contained type
                            Type entryType = null;
                            for (int p = 0; p < properties.Length; p++)
                            {
                                PropertyInfo property = properties[p];
                                bool foundEntry = false;
                                foreach (string searchEntry in searchEntries)
                                {
                                    if (property.PropertyType != typeof(string) || property.Name != searchEntry && !property.Name.EndsWith(searchEntry))
                                    {
                                        if (Check(ProtocolLogOptions.LogCodeGen))
                                        {
                                            logger.Log(LOG_ERROR, $" Rejected {type.FullName}.{property.Name}: Not {searchEntry} ");
                                        }

                                        continue;
                                    }

                                    dataEntries.Add(property.Name);
                                    foundEntry = true;
                                    break;
                                }

                                if (!foundEntry)
                                {
                                    continue;
                                }

                                entryType = genericType;
                            }

                            if (entryType == null)
                            {
                                if (Check(ProtocolLogOptions.LogCodeGen))
                                {
                                    logger.Log(LOG_ERROR, $" Rejected {type.FullName}: No inner type found");
                                }

                                continue;
                            }

                            if (Check(ProtocolLogOptions.LogCodeGen))
                            {
                                logger.Log($"-Found valid: {type.FullName}/{entryType.Name}");
                            }

                            generator.AddCodeElement(type, entryType, dataEntries, reset);
                            dataEntries.Insert(0, entryType.Name);
                            patchEntries.Add(new List<string>(dataEntries));
                            dataEntries.Clear();
                        }
                    }

                    string extension = csharpToggleExtension ? csharpOptionExtension : INTEGRATION_EXTENSION;

                    //Generate the actual code for integration
                    if (generator.BuildCode(extension, INTEGRATION_EXTENSION))
                    {
                        string directory = Path.GetDirectoryName(csharpFile);
                        if (reset)
                        {
                            SourceControlHelper.CheckoutOrAdd(directory);
                        }

                        for (int c = 0; c < generator.Code.Length; c++)
                        {
                            string file = generator.Files[c];
                            string code = generator.Code[c];

                            file = csharpFile.Replace(csharpFileInfos.defaultValue, file);

                            File.WriteAllText(file, code);
                        }

                        if (!reset)
                        {
                            PatchProtocolScripts(generator, directory, extension, patchEntries);

                            SourceControlHelper.RevertUnchanged(directory);
                        }

                        return true;
                    }
                }

                return false;
            }
        }

        private void PatchProtocolScripts(ProtocolBuildCodeGenerator generator, string workDirectory, string extension, List<List<string>> patchEntries)
        {
            List<string> files = new List<string>(Directory.GetFiles(workDirectory, $"*{extension}", SearchOption.AllDirectories));
            foreach (List<string> patchEntry in patchEntries)
            {
                bool foundFile = false;
                string filename = $"{patchEntry[Consts.FIRST_ITEM]}{extension}";
                for (int f = 0; f < files.Count; f++)
                {
                    string file = files[f];
                    if (file.EndsWith(filename))
                    {
                        foundFile = true;
                        filename = file;
                        files.RemoveAt(f--);
                        break;
                    }
                }

                if (!foundFile)
                {
                    continue;
                }

                string content = File.ReadAllText(filename);
                patchEntry.RemoveAt(0);
                if (generator.PatchCodeWithObsolete(ref content, patchEntry))
                {
                    File.WriteAllText(filename, content);
                }
            }
        }
        #endregion

        #region Behaviour
        protected override EditorWorkState[] InjectCustomStates()
        {
            List<EditorWorkState> states = new List<EditorWorkState>
            {
                PROCESS_PROTO_COMPILE,
                PROCESS_PROTO_COMPILE_WAIT,
                PROCESS_DB_INTEGRATION
            };

            return states.ToArray();
        }

        protected override FieldSlotInfos GetFieldSlotInfos(FieldSlot slot)
        {
            FieldSlotInfos infos = base.GetFieldSlotInfos(slot);
            switch (slot)
            {
                case FieldSlot.ProcessFolder:
                {
                    infos.title = "Compiler directory";
                    infos.defaultValue = "../External/GoogleProtobuf/protoc-";
                    break;
                }
                case PROTO_APIVERSION:
                {
                    infos.title = "API version";
                    infos.status = FieldSlotTag.Enabled;
                    infos.defaultValue = "3.9.0";
                    break;
                }
                case FieldSlot.ProcessFile:
                {
                    infos.title = "Compiler binary";
                    infos.defaultValue = "bin/protoc.exe";
                    break;
                }

                case SCRIPT_ROOT_DIR:
                {
                    infos.title = "Scripts root folder";
                    infos.status = FieldSlotTag.AddSpaceBefore | FieldSlotTag.Enabled | FieldSlotTag.NeedExistenceCheck | FieldSlotTag.IsDirectory | FieldSlotTag.RelativeToAssetPath;
                    infos.defaultValue = "Scripts";
                    break;
                }
                case PROTO_ROOT_DIR:
                {
                    infos.title = "Procotol root folder";
                    infos.status = FieldSlotTag.Enabled | FieldSlotTag.NeedExistenceCheck | FieldSlotTag.IsDirectory | FieldSlotTag.RelativeToOtherPath;
                    infos.defaultValue = "Code/Protocol";
                    break;
                }
                case FieldSlot.ArgumentSrcTag:
                {
                    infos.defaultValue = "--proto_path=";
                    break;
                }
                case PROTO_SOURCES_DIR:
                {
                    infos.defaultValue = "Sources";
                    infos.status |= FieldSlotTag.RelativeToOtherPath;
                    infos.status &= ~FieldSlotTag.RelativeToAssetPath;
                    break;
                }
                case PROTO_EXPORT_DIR:
                {
                    infos.title = "Export Dir";
                    infos.defaultValue = ConstsFolders.EXPORT;
                    infos.status = FieldSlotTag.Enabled | FieldSlotTag.NeedExistenceCheck | FieldSlotTag.IsDirectory | FieldSlotTag.RelativeToOtherPath;
                    break;
                }

                //CSharp options
                case PROTO_CSHARP_TAG:
                {
                    infos.title = "CSharp destination tag";
                    infos.status |= FieldSlotTag.GroupStart;
                    infos.defaultValue = "--csharp_out=";
                    break;
                }
                case PROTO_CSHARP_DIR:
                {
                    infos.title = "CSharp destination directory";
                    infos.status |= FieldSlotTag.Group | FieldSlotTag.RelativeToOtherPath;
                    infos.status &= ~FieldSlotTag.RelativeToAssetPath;
                    infos.defaultValue = "CSharp";
                    break;
                }
                case PROTO_CSHARP_OPTION:
                {
                    infos.title = "CSharp option";
                    infos.status = FieldSlotTag.Enabled | FieldSlotTag.Toggleable | FieldSlotTag.ToggleDefaultOn | FieldSlotTag.Group | FieldSlotTag.AddSpaceBefore;
                    infos.defaultValue = "--csharp_opt=";
                    break;
                }
                case PROTO_CSHARP_OPTION_NMSPC_DIR:
                {
                    infos.title = "Use Namespace as directory";
                    infos.status = FieldSlotTag.Enabled | FieldSlotTag.Toggleable | FieldSlotTag.ToggleDefaultOn | FieldSlotTag.Group;
                    infos.defaultValue = "base_namespace";
                    break;
                }
                case PROTO_CSHARP_OPTION_NMSPC_ROOT:
                {
                    infos.title = "Protocol root namespace";
                    infos.defaultValue = ProtocolHelper.PROTOCOL_ROOT_SUFFIX;
                    infos.status = FieldSlotTag.Enabled | FieldSlotTag.Toggleable | FieldSlotTag.ToggleDefaultOn | FieldSlotTag.Group | FieldSlotTag.IsType | FieldSlotTag.NeedExistenceCheck;
                    break;
                }
                case PROTO_CSHARP_OPTION_EXTENSION:
                {
                    infos.title = "Generated file extension";
                    infos.status = FieldSlotTag.Enabled | FieldSlotTag.Toggleable | FieldSlotTag.ToggleDefaultOn | FieldSlotTag.Group;
                    infos.defaultValue = ".g.cs";
                    break;
                }
                case PROTO_CSHARP_DB_CLASS:
                {
                    infos.title = "Database root class";
                    infos.defaultValue = ProtocolHelper.PROTOCOL_DATABASE_SUFFIX;
                    infos.status = FieldSlotTag.Enabled | FieldSlotTag.Group | FieldSlotTag.AddSpaceBefore | FieldSlotTag.IsType | FieldSlotTag.NeedExistenceCheck;
                    break;
                }
                case PROTO_CSHARP_INTEGRATION:
                {
                    infos.title = "DB integration";
                    infos.status = FieldSlotTag.Enabled | FieldSlotTag.Group | FieldSlotTag.IsFile | FieldSlotTag.RelativeToOtherPath;
                    infos.defaultValue = $"{DEFAULT}DatabaseIntegration.cs";
                    break;
                }
                case PROTO_CSHARP_CONTAINERENTRY:
                {
                    infos.title = "DB container member";
                    infos.status = FieldSlotTag.Enabled | FieldSlotTag.Group;
                    infos.defaultValue = "List";
                    break;
                }
                case PROTO_CSHARP_DATAENTRY:
                {
                    infos.title = "DB entry member";
                    infos.status = FieldSlotTag.Enabled | FieldSlotTag.Group;
                    infos.defaultValue = "UniqueId;TemplateId";
                    break;
                }

                //CSharp options
                case PROTO_JSCRIPTTAG:
                {
                    infos.title = "JScript destination tag";
                    infos.status = FieldSlotTag.Enabled | FieldSlotTag.AddSpaceBefore | FieldSlotTag.GroupStart;
                    infos.defaultValue = "--js_out=";
                    break;
                }
                case PROTO_JSCRIPTBIN:
                {
                    infos.title = "JScript binary tag";
                    infos.status = FieldSlotTag.Enabled | FieldSlotTag.Group;
                    infos.defaultValue = "binary:";
                    break;
                }
                case PROTO_JSCRIPTDIR:
                {
                    infos.title = "JScript destination directory";
                    infos.status = FieldSlotTag.Enabled | FieldSlotTag.Group | FieldSlotTag.NeedExistenceCheck | FieldSlotTag.IsDirectory | FieldSlotTag.RelativeToOtherPath;
                    infos.defaultValue = "JScript";
                    break;
                }
                case PROTO_JSCRIPTOPTION:
                {
                    infos.title = "JScript option";
                    infos.status = FieldSlotTag.Enabled | FieldSlotTag.Group | FieldSlotTag.Toggleable | FieldSlotTag.ToggleDefaultOn;
                    infos.defaultValue = "library=ProtoLib";
                    break;
                }
            }

            return infos;
        }

        protected override string GetRelativePath(FieldSlot slot, FieldSlotInfos infos)
        {
            switch (slot)
            {
                case PROTO_SOURCES_DIR:
                {
                    string srcDir = string.Empty;
                    GetFieldSlotValues(PROTO_ROOT_DIR, ref srcDir);
                    return srcDir;
                }
                case PROTO_EXPORT_DIR:
                {
                    string srcDir = string.Empty;
                    GetFieldSlotValues(PROTO_ROOT_DIR, ref srcDir);
                    return srcDir;
                }
                case PROTO_ROOT_DIR:
                {
                    string srcDir = string.Empty;
                    GetFieldSlotValues(SCRIPT_ROOT_DIR, ref srcDir);
                    string result = Path.Combine(srcDir, assemblyName);
                    return result;
                }
                case PROTO_CSHARP_DIR:
                {
                    string protoRootDir = string.Empty;
                    GetFieldSlotValues(PROTO_EXPORT_DIR, ref protoRootDir);
                    return protoRootDir;
                }
                case PROTO_JSCRIPTDIR:
                {
                    string protoRootDir = string.Empty;
                    GetFieldSlotValues(PROTO_EXPORT_DIR, ref protoRootDir);
                    return protoRootDir;
                }
                case PROTO_CSHARP_INTEGRATION:
                {
                    string folder = string.Empty;
                    GetFieldSlotValues(PROTO_EXPORT_DIR, ref folder);
                    return folder;
                }
            }

            return GetRelativePath(slot, infos);
        }

        protected override void GetFieldSlotValues(FieldSlot slot, FieldSlotInfos infos, ref string currentValue, ref string validationValue)
        {
            currentValue = GetFieldSlot(slot);
            validationValue = currentValue;

            ApplyDirectoryRules(slot, infos, ref validationValue);

            switch (slot)
            {
                case FieldSlot.ProcessFolder:
                {
                    string version = string.Empty;
                    GetFieldSlotValues(PROTO_APIVERSION, ref version);
                    validationValue = currentValue + version;
                    ApplyDirectoryRules(slot, infos, ref validationValue);
                    break;
                }
                case FieldSlot.ProcessFile:
                {
                    string folder = string.Empty;
                    GetFieldSlotValues(FieldSlot.ProcessFolder, ref folder);
                    validationValue = Path.Combine(folder, currentValue);
                    break;
                }
                case PROTO_CSHARP_OPTION_NMSPC_ROOT:
                case PROTO_CSHARP_DB_CLASS:
                {
                    validationValue = assemblyName + currentValue;
                    break;
                }
            }

            ApplyIORules(infos, ref validationValue);
        }

        protected override void BuildCommandArguments()
        {
            base.BuildCommandArguments();

            string srcDir = string.Empty;
            GetFieldSlotValues(PROTO_SOURCES_DIR, ref srcDir);

            //CSharp
            {
                string csharpOption = string.Empty;
                GetFieldSlotValues(PROTO_CSHARP_OPTION, ref csharpOption);

                string csharpOptionNamespaceDir = string.Empty;
                bool csharpToggleNamespaceDir = true;
                GetFieldSlotValues(PROTO_CSHARP_OPTION_NMSPC_DIR, ref csharpOptionNamespaceDir);
                prefsWrapper.Get(PROTO_CSHARP_OPTION_NMSPC_DIR, ref csharpToggleNamespaceDir);

                string csharpOptionNamespaceRoot = string.Empty;
                bool csharpToggleNamespaceRoot = true;
                GetFieldSlotValues(PROTO_CSHARP_OPTION_NMSPC_ROOT, ref csharpOptionNamespaceRoot);
                prefsWrapper.Get(PROTO_CSHARP_OPTION_NMSPC_ROOT, ref csharpToggleNamespaceRoot);

                string csharpOptionExtension = string.Empty;
                bool csharpToggleExtension = true;
                GetFieldSlotValues(PROTO_CSHARP_OPTION_EXTENSION, ref csharpOptionExtension);
                prefsWrapper.Get(PROTO_CSHARP_OPTION_EXTENSION, ref csharpToggleExtension);

                bool csharpToggle = false;
                prefsWrapper.Get(PROTO_CSHARP_OPTION, ref csharpToggle);

                if (csharpToggle && (csharpToggleNamespaceDir || csharpToggleExtension))
                {
                    string cmdLine = csharpOption;
                    if (csharpToggleExtension)
                    {
                        cmdLine += "file_extension=" + csharpOptionExtension;
                    }

                    if (csharpToggleNamespaceDir)
                    {
                        if (csharpToggleExtension)
                        {
                            cmdLine += ",";
                        }

                        cmdLine += csharpOptionNamespaceDir;

                        if (csharpToggleNamespaceRoot)
                        {
                            Type type = AssemblyHelper.GetTypeFromAnyAssembly(csharpOptionNamespaceRoot);
                            cmdLine += "=" + (type != null ? type.Namespace : Consts.UNDEFINED);
                        }
                    }

                    cmdLineArguments = string.Format("{0}\n{1}", cmdLineArguments, cmdLine);
                }
            }

            //JScript
            {
                string jscriptTag = string.Empty;
                string jscripDir = string.Empty;
                string jscripBin = string.Empty;
                GetFieldSlotValues(PROTO_JSCRIPTTAG, ref jscriptTag);
                GetFieldSlotValues(PROTO_JSCRIPTDIR, ref jscripDir);
                GetFieldSlotValues(PROTO_JSCRIPTBIN, ref jscripBin);

                string jscriptOption = string.Empty;
                GetFieldSlotValues(PROTO_JSCRIPTOPTION, ref jscriptOption);

                bool jscriptToggle = false;
                prefsWrapper.Get(PROTO_JSCRIPTOPTION, ref jscriptToggle);

                cmdLineArguments = string.Format("{0}\n{1}", cmdLineArguments, jscriptTag);

                if (jscriptToggle)
                {
                    cmdLineArguments = string.Format("{0}{1},", cmdLineArguments, jscriptOption);
                }

                cmdLineArguments = string.Format("{0}{1}{2}", cmdLineArguments, jscripBin, jscripDir);
            }

            //Add all files to be built
            List<string> importFiles = new List<string>();
            if (GatherFiles(srcDir, importFiles, IMPORT_EXTENSION, AtDbEditorConstants.EXCEL_TEMP_PREFIX))
            {
                for (int f = 0; f < importFiles.Count; f++)
                {
                    cmdLineArguments = string.Format("{0}\n{1}", cmdLineArguments, importFiles[f]);
                }
            }
        }

        private static bool GatherFiles(string srcDir, List<string> files, string extension, string exclusion)
        {
            if (Directory.Exists(srcDir))
            {
                string[] foundFiles = Directory.GetFiles(srcDir, extension, SearchOption.AllDirectories);
                for (int f = 0; f < foundFiles.Length; f++)
                {
                    string file = foundFiles[f];
                    if (file.Contains(exclusion))
                    {
                        continue;
                    }

                    files.Add(foundFiles[f]);
                }
            }

            return files.Count > 0;
        }
        #endregion Behaviour

        #region Process Execution
        private void ThrowException(Exception e)
        {
            EditorApplication.UnlockReloadAssemblies();
            Console.WriteLine(e);
        }

        private bool CheckRootDir(FieldSlot slot)
        {
            string folder = string.Empty;
            GetFieldSlotValues(PROTO_ROOT_DIR, ref folder);
            return Directory.Exists(folder);
        }

        protected override void PreExecuteProcess()
        {
            base.PreExecuteProcess();

            EditorApplication.LockReloadAssemblies();

            //Forcing unlock reload because it can destroy assembly loading/compiling
            try
            {
                if (RunTestMode)
                {
                    return;
                }

                string folder = string.Empty;
                GetFieldSlotValues(SCRIPT_ROOT_DIR, ref folder);

                directories = Directory.GetDirectories(folder);
                for (int d = 0; d < directories.Length; d++)
                {
                    directories[d] = Code.Utils.Helpers.PathHelper.RemoveLeadingAndTrailingSlashes(PathHelper.Simplify(directories[d]).Replace(folder, string.Empty));
                }

                processPassCount = directories.Length;
            }
            catch (Exception e)
            {
                ThrowException(e);
                throw;
            }
        }

        protected override bool ExecuteProcess(int pass)
        {
            try
            {
                assemblyName = directories[pass];

                string folder = string.Empty;
                GetFieldSlotValues(PROTO_ROOT_DIR, ref folder);
                if (!Directory.Exists(folder))
                {
                    logger.Log(LOG_IGNORE, $"No processing in {assemblyName}");
                    return true;
                }

                logger.Log(LOG_TASK, $"Executing process in: {assemblyName}");

                ChangeExportFiles(FileChange.TempSave);

                if (!GenerateDatabaseIntegration(true))
                {
                    logger.Log(LOG_WARNING, "Code generation failed");
                }

                BuildCommandArguments();

                base.ExecuteProcess(pass);
            }
            catch (Exception e)
            {
                ThrowException(e);
                throw;
            }

            return true;
        }

        protected override void OnProcessEnded()
        {
            try
            {
                base.OnProcessEnded();

                for (int d = 0; d < directories.Length; d++)
                {
                    assemblyName = directories[d];
                    if (!CheckRootDir(PROTO_ROOT_DIR))
                    {
                        continue;
                    }

                    ChangeExportFiles(exitCode == 0 ? FileChange.TempDelete : FileChange.TempRollback);
                    if (exitCode == 0)
                    {
                        ChangeExportFiles(FileChange.SourceControl);
                    }
                }
            }
            catch (Exception e)
            {
                ThrowException(e);
                throw;
            }
        }

        private void FinishDatabaseIntegration()
        {
            EditorApplication.LockReloadAssemblies();
            try
            {
                for (int d = 0; d < directories.Length; d++)
                {
                    assemblyName = directories[d];
                    if (!CheckRootDir(PROTO_ROOT_DIR))
                    {
                        logger.Log(LOG_IGNORE, $"No processing in {assemblyName}");
                        continue;
                    }

                    logger.Log(LOG_TASK, $"Executing process in: {assemblyName}");

                    if (!GenerateDatabaseIntegration())
                    {
                        logger.Log(LOG_WARNING, "Code generation failed");
                    }
                }
            }
            catch (Exception e)
            {
                ThrowException(e);
                throw;
            }

            directories = null;
            assemblyName = DEFAULT;

            EditorApplication.UnlockReloadAssemblies();
            AssetDatabase.Refresh();

            if (exitCode == 0)
            {
                DoOpenWithAutoExecute<ProtocolToExcelExportEditorWindow>(titleContent.text);
            }
        }

        private void ChangeExportFiles(FileChange operation)
        {
            string dstDir = string.Empty;

            //Go through all export extension files and rename them to "whatever~"
            for (int d = 0; d < EXPORT_DIRECTORIES.Length; d++)
            {
                string extension = EXPORT_EXTENSIONS[d];
                GetFieldSlotValues(EXPORT_DIRECTORIES[d], ref dstDir);

                switch (operation)
                {
                    case FileChange.TempSave:
                    {
                        if (Check(ProtocolLogOptions.LogFiles))
                        {
                            logger.Log("{0} Saving File with extension: {1} to {2}", LOG_STORY, extension, extension + ConstsFolders.TEMP);
                        }

                        break;
                    }
                    case FileChange.TempDelete:
                    {
                        extension += ConstsFolders.TEMP;
                        if (Check(ProtocolLogOptions.LogFiles))
                        {
                            logger.Log("{0} Deleting Files with extension: {1}", LOG_STORY, extension);
                        }

                        break;
                    }
                    case FileChange.TempRollback:
                    {
                        if (Check(ProtocolLogOptions.LogFiles))
                        {
                            logger.Log("{0} Rollbacking File with extension: {1} to {2}", LOG_STORY, extension + ConstsFolders.TEMP, extension);
                        }

                        extension += ConstsFolders.TEMP;
                        break;
                    }
                    case FileChange.SourceControl:
                    {
                        if (Check(ProtocolLogOptions.LogFiles))
                        {
                            logger.Log("{0} Marking Files with extension: {1} in P4", LOG_STORY, extension);
                        }

                        break;
                    }
                }

                if (!Directory.Exists(dstDir))
                {
                    continue;
                }

                if (operation == FileChange.SourceControl)
                {
                    SourceControlHelper.CheckoutOrAddAndRevertUnchanged(dstDir);
                    continue;
                }

                string[] files = Directory.GetFiles(dstDir, extension, SearchOption.AllDirectories);
                for (int f = 0; f < files.Length; f++)
                {
                    string file = files[f];
                    if (Check(ProtocolLogOptions.LogFiles))
                    {
                        logger.Log("    {0}", file);
                    }

                    File.SetAttributes(file, File.GetAttributes(file) & ~FileAttributes.ReadOnly);

                    switch (operation)
                    {
                        case FileChange.TempSave:
                        {
                            File.Delete(file + ConstsFolders.TEMP);
                            File.Move(file, file + ConstsFolders.TEMP);
                            break;
                        }
                        case FileChange.TempDelete:
                        {
                            File.Delete(file);
                            break;
                        }
                        case FileChange.TempRollback:
                        {
                            File.Move(file, file.TrimEnd('~'));
                            break;
                        }
                    }
                }
            }
        }
        #endregion Process Execution
    }
}
