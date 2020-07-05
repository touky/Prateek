
namespace Mayfair.Core.Editor.Localization
{
    using System;
    using Mayfair.Core.Editor.EditorWindows;
    using Mayfair.Core.Code.Localization;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using DG.DemiEditor;
    using Mayfair.Core.Editor.Utils;
    using NPOI.SS.UserModel;
    using NPOI.XSSF.UserModel;
    using UnityEditor;
    using UnityEngine;

    public class LocalizationExportEditorWindow : BaseProcessEditorWindow
    {
        #region Classes
        [Serializable]
        private class SupportedLanguage
        {
            public SystemLanguage language;
            public string column;
        }

        [Serializable]
        private class SerializedSupportedLanguageWrapper
        {
            public SupportedLanguage[] array;
        }
        #endregion

        #region Enums
        private enum LocalizationLogOptions
        {
            Nothing = 0,
            ShowMissingTranslationValueWarnings = 1
        }
        #endregion

        #region Statics and Constants
        private const FieldSlot EXCEL_IMPORT_MINIMAL_FILE = FieldSlot.CustomSlot + NEXT_SLOT * 1;
        private const FieldSlot EXCEL_IMPORT_FULL_FILE = FieldSlot.CustomSlot + NEXT_SLOT * 2;
        private const FieldSlot JSON_EXPORT_MINIMAL_DIR = FieldSlot.CustomSlot + NEXT_SLOT * 3;
        private const FieldSlot JSON_EXPORT_FULL_DIR = FieldSlot.CustomSlot + NEXT_SLOT * 4;

        private const GUIDrawAction GUI_LANGUAGES = GUIDrawAction.DrawCustom + NEXT_DRAW * 1;

        private const string EXPORT_EXTENSION = ".json";

        private const string WINDOW_TITLE = "Localization JSON Exporter";
        private const string KEY_COLUMN_PREF = "keyColumn";
        private const string SUPPORTED_LANGUAGES_PREF = "supportedLanguages";
        #endregion
        
        #region Fields
        private LocalizationLogOptions localizationLogOptions = LocalizationLogOptions.Nothing;

        private EditorPrefsWrapper editorPrefsWrapper;

        private bool showEmptyTranslatedValueWarnings;
        private string keyColumn;
        private List<SupportedLanguage> supportedLanguages;
        #endregion

        #region Methods
        [MenuItem(EditorMenuOrderHelper.WINDOW_MENU + WINDOW_TITLE, priority = EditorMenuOrderHelper.BAR_MENU_GROUP0_3)]
        public static void Open()
        {
            DoOpen<LocalizationExportEditorWindow>(WINDOW_TITLE);
        }

        protected override void InitPrefHelper()
        {
            base.InitPrefHelper();

            if (editorPrefsWrapper == null)
            {
                editorPrefsWrapper = new EditorPrefsWrapper(typeof(LocalizationExportEditorWindow).ToString());
            }
        }

        protected override void RefreshDatas()
        {
            base.RefreshDatas();
            editorPrefsWrapper.Get(KEY_COLUMN_PREF, ref keyColumn, "B");

            string supportedLanguagesEditorPref = string.Empty;
            editorPrefsWrapper.Get(SUPPORTED_LANGUAGES_PREF, ref supportedLanguagesEditorPref, string.Empty);

            SerializedSupportedLanguageWrapper wrapper = JsonUtility.FromJson<SerializedSupportedLanguageWrapper>(supportedLanguagesEditorPref);
            if (wrapper.array == null)
            {
                wrapper.array = new SupportedLanguage[0];
            }

            supportedLanguages = new List<SupportedLanguage>(wrapper.array.Length);
            supportedLanguages.AddRange(wrapper.array);
        }

        private void SavePrefs()
        {
            editorPrefsWrapper.Set("keyColumn", keyColumn);

            SerializedSupportedLanguageWrapper wrapper = new SerializedSupportedLanguageWrapper
            {
                array = supportedLanguages.ToArray()
            };
            string json = JsonUtility.ToJson(wrapper);
            editorPrefsWrapper.Set("supportedLanguages", json);
        }

        protected override FieldSlotInfos GetFieldSlotInfos(FieldSlot slot)
        {
            FieldSlotInfos infos = new FieldSlotInfos();
            switch (slot)
            {
                case EXCEL_IMPORT_MINIMAL_FILE:
                {
                    infos.title = "Minimal Excel File";
                    infos.defaultValue = "../Localization/Localization_Minimal.xlsx";
                    infos.status = FieldSlotTag.Enabled | FieldSlotTag.IsFile | FieldSlotTag.NeedExistenceCheck |
                                   FieldSlotTag.RelativeToAssetPath;
                    break;
                }
                case EXCEL_IMPORT_FULL_FILE:
                {
                    infos.title = "Full Excel File";
                    infos.defaultValue = "../Localization/Localization_Full.xlsx";
                    infos.status = FieldSlotTag.Enabled | FieldSlotTag.IsFile | FieldSlotTag.NeedExistenceCheck | FieldSlotTag.RelativeToAssetPath;
                    break;
                }
                case JSON_EXPORT_FULL_DIR:
                {
                    infos.title = "Full JSON Export directory";
                    infos.defaultValue = "/Localization/Full";
                    infos.status = FieldSlotTag.Enabled | FieldSlotTag.IsDirectory | FieldSlotTag.NeedExistenceCheck | FieldSlotTag.RelativeToAssetPath;
                    break;
                }
                case JSON_EXPORT_MINIMAL_DIR:
                {
                    infos.title = "Minimal JSON Export directory";
                    infos.defaultValue = "/Localization/Minimal";
                    infos.status = FieldSlotTag.Enabled | FieldSlotTag.IsDirectory | FieldSlotTag.NeedExistenceCheck | FieldSlotTag.RelativeToAssetPath;
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

        protected override void DrawLogOptions()
        {
            DrawLogOptions(ref localizationLogOptions, ((a, b) => a | b));
        }

        protected void RefreshImport()
        {
            string minimalFile = string.Empty;
            string fullFile = string.Empty;

            GetFieldSlotValues(EXCEL_IMPORT_MINIMAL_FILE, ref minimalFile);
            GetFieldSlotValues(EXCEL_IMPORT_MINIMAL_FILE, ref fullFile);
        }

        #region GUI Logic

        protected override void InitGUIDrawOrder(List<GUIDrawAction> drawOrder)
        {
            if (drawOrder.Count != 0)
            {
                return;
            }

            drawOrder.AddRange(new[]
            {
                GUIDrawAction.DrawTags,
                GUI_LANGUAGES,
                GUIDrawAction.DrawProcess,
                GUIDrawAction.DrawLogger
            });
        }

        protected override void OnInspectorUpdate()
        {
            base.OnInspectorUpdate();
            RefreshImport();
        }

        protected override bool DrawGUIAction(GUIDrawAction drawAction)
        {
            if (base.DrawGUIAction(drawAction))
            {
                return true;
            }

            switch (drawAction)
            {
                case GUI_LANGUAGES:
                {
                    DrawLanguages();
                    return true;
                }
                default:
                {
                    return false;
                }
            }
        }

        protected void DrawLanguages()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                using (EditorGUI.ChangeCheckScope check = new EditorGUI.ChangeCheckScope())
                {
                    GUIContent keyColumnContent = new GUIContent("Key Column");
                    EditorStyles.label.CalcMinMaxWidth(keyColumnContent, out float keyColumnWidth, out _);
                    using (new DeGUI.LabelFieldWidthScope(keyColumnWidth + 20f, 25f))
                    {
                        keyColumn = EditorGUILayout.TextField(keyColumnContent, keyColumn);
                    }

                    GUILayout.FlexibleSpace();

                    if (check.changed)
                    {
                        SavePrefs();
                    }
                }
            }

            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("Supported Languages:");
                if (GUILayout.Button("Add New"))
                {
                    supportedLanguages.Add(new SupportedLanguage());
                    SavePrefs();
                }

                GUILayout.FlexibleSpace();
            }

            using (new EditorGUI.IndentLevelScope(ConstsEditor.ONE_INDENT))
            {
                using (new EditorGUILayout.VerticalScope())
                {
                    using (EditorGUI.ChangeCheckScope check = new EditorGUI.ChangeCheckScope())
                    {
                        int removeLanguageIndex = -1;

                        for (int i = 0; i < supportedLanguages.Count; i++)
                        {
                            SupportedLanguage supportedLanguage = supportedLanguages[i];

                            using (new EditorGUILayout.HorizontalScope())
                            {
                                GUIContent languageGuiContent = new GUIContent("Language");
                                EditorStyles.label.CalcMinMaxWidth(languageGuiContent, out float languageWidth, out _);
                                using (new DeGUI.LabelFieldWidthScope(languageWidth + 20f, 120f))
                                {
                                    supportedLanguage.language = (SystemLanguage) EditorGUILayout.EnumPopup(languageGuiContent, supportedLanguage.language);
                                }

                                GUIContent columnGuiContent = new GUIContent("Column");
                                EditorStyles.label.CalcMinMaxWidth(columnGuiContent, out float columnWidth, out _);
                                using (new DeGUI.LabelFieldWidthScope(columnWidth + 20f, 25f))
                                {
                                    supportedLanguage.column = EditorGUILayout.TextField(columnGuiContent, supportedLanguage.column);
                                }

                                if (GUILayout.Button("Delete"))
                                {
                                    removeLanguageIndex = i;
                                    break;
                                }

                                GUILayout.FlexibleSpace();
                            }
                        }

                        if (removeLanguageIndex >= 0 && removeLanguageIndex < supportedLanguages.Count)
                        {
                            supportedLanguages.RemoveAt(removeLanguageIndex);
                            SavePrefs();
                        }

                        if (check.changed)
                        {
                            SavePrefs();
                        }
                    }
                }
            }
        }

        #endregion

        #region Process Execution

        protected override bool ExecuteProcess(int pass)
        {
            if (!Parse(EXCEL_IMPORT_MINIMAL_FILE, out Dictionary<SupportedLanguage, LocalizationJSON> minLookup))
            {
                return true;
            }

            if (!Parse(EXCEL_IMPORT_FULL_FILE, out Dictionary<SupportedLanguage, LocalizationJSON> fullLookup))
            {
                return true;
            }

            Export(minLookup, JSON_EXPORT_MINIMAL_DIR);
            Export(fullLookup, JSON_EXPORT_FULL_DIR);

            AssetDatabase.Refresh();

            return true;
        }

        private bool Parse(FieldSlot type, out Dictionary<SupportedLanguage, LocalizationJSON> languageJSONLookup)
        {
            bool showMissingTranslationValueWarnings = Check(LocalizationLogOptions.ShowMissingTranslationValueWarnings);

            #region Init lookups

            languageJSONLookup = new Dictionary<SupportedLanguage, LocalizationJSON>();
            Dictionary<int, SupportedLanguage> cellColumnLanguageLookup = new Dictionary<int, SupportedLanguage>();

            foreach (SupportedLanguage supportedLanguage in supportedLanguages)
            {
                languageJSONLookup.Add(supportedLanguage, new LocalizationJSON());
                cellColumnLanguageLookup.Add(ExcelColumnToInt(supportedLanguage.column), supportedLanguage);
            }

            #endregion

            string importFile = string.Empty;
            GetFieldSlotValues(type, ref importFile);

            #region Open & Rad Excel

            XSSFWorkbook workbook = null;
            try
            {
                using (FileStream fileStream = new FileStream(importFile, FileMode.Open, FileAccess.Read))
                {
                    workbook = new XSSFWorkbook(fileStream);
                }
            }
            catch
            {
                logger.Log(LOG_ERROR, $"ERROR: Could not open {importFile}. Is Excel open?");
                return false;
            }

            #endregion

            #region Parse Excel

            {
                int keyColumnIndex = ExcelColumnToInt(keyColumn);
                ISheet sheet = workbook.GetSheetAt(0);
                IEnumerator rowEnumerator = sheet.GetRowEnumerator();
                while (rowEnumerator.MoveNext())
                {
                    IRow row = rowEnumerator.Current as IRow;
                    if (row == null)
                    {
                        continue;
                    }

                    bool leaveRow = false;
                    string currentKey = string.Empty;

                    using (IEnumerator<ICell> cellEnumerator = row.GetEnumerator())
                    {
                        while (!leaveRow && cellEnumerator.MoveNext())
                        {
                            ICell cell = cellEnumerator.Current;

                            if (cell.ColumnIndex == 0)
                            {
                                if (cell.StringCellValue.Contains("ignore"))
                                {
                                    leaveRow = true;
                                    break;
                                }
                            }
                            else if (cell.ColumnIndex == keyColumnIndex)
                            {
                                string content = cell.StringCellValue.Trim();
                                if (content.Length <= 0)
                                {
                                    leaveRow = true;
                                    break;
                                }

                                if (!HasAllValidCharacters(content))
                                {
                                    logger.Log(LOG_ERROR, $"ERROR: Key \"{content}\" has (an) unexpected character(s). Only ALL CAPS 'A-Z', numbers '0-9', and underscores are allowed");
                                    return false;
                                }

                                currentKey = content;
                            }
                            else if (cellColumnLanguageLookup.TryGetValue(cell.ColumnIndex, out SupportedLanguage supportedLanguage))
                            {
                                string translationValue = cell.StringCellValue;
                                if (translationValue.Length <= 0 && showMissingTranslationValueWarnings)
                                {
                                    logger.Log(LOG_WARNING, $"{currentKey} is missing {supportedLanguage.language} translation");
                                }

                                languageJSONLookup[supportedLanguage].Add(currentKey, translationValue);
                            }
                        }
                    }
                }
            }

            #endregion

            #region Sort each language by key

            foreach (LocalizationJSON localizationJson in languageJSONLookup.Values)
            {
                localizationJson.kvps.Sort((kvp1, kvp2) => EditorUtility.NaturalCompare(kvp1.k, kvp2.k));
            }

            #endregion

            return true;
        }

        private void Export(Dictionary<SupportedLanguage, LocalizationJSON> languageJSONLookup, FieldSlot exportDir)
        {
            foreach (KeyValuePair<SupportedLanguage, LocalizationJSON> kvp in languageJSONLookup)
            {
                string language = kvp.Key.language.ToString();

                string exportFile = string.Empty;
                GetFieldSlotValues(exportDir, ref exportFile);
                exportFile += language + EXPORT_EXTENSION;

                string json = JsonUtility.ToJson(kvp.Value, false);

                File.WriteAllText(exportFile, json);
            }
        }

        #endregion

        #region Helper Methods
        private static int ExcelColumnToInt(string column)
        {
            int value = 0;

            for (int i = column.Length - 1; i >= 0; i--)
            {
                int digit = column[i] - 'A';
                value += digit * (int) Mathf.Pow(26, i);
            }

            return value;
        }

        private bool Check(LocalizationLogOptions option)
        {
            return (localizationLogOptions & option) != LocalizationLogOptions.Nothing;
        }

        private bool HasAllValidCharacters(string key)
        {
            foreach (char c in key)
            {
                if (c >= 'A' && c <= 'Z')
                {
                    continue;
                }

                if (c >= '0' && c <= '9')
                {
                    continue;
                }

                if (c == '_')
                {
                    continue;
                }

                return false;
            }

            return true;
        }

        #endregion
        #endregion
    }
}