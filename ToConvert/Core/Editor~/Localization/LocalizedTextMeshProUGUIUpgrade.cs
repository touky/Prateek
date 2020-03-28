using UnityEditor.VersionControl;

namespace Mayfair.Core.Editor.Localization
{
    using Mayfair.UIFrontEnd.Code.Localization;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using TMPro;
    using UnityEditor.Experimental.SceneManagement;
    using UnityEditor;
    using UnityEngine;
    using System.IO;

    public static class LocalizedTextMeshProUGUIUpgrade
    {
        #region Methods

        // Validate Function
        [MenuItem("Tools/LocalizedTextMeshProUGUI Upgrade", true)]
        private static bool IsInPrefabView()
        {
            return PrefabStageUtility.GetCurrentPrefabStage() != null;
        }

        [MenuItem("Tools/LocalizedTextMeshProUGUI Upgrade")]
        private static void DoUpgrade()
        {
            PrefabStage stage = PrefabStageUtility.GetCurrentPrefabStage();
            if (stage == null)
            {
                return;
            }

            if (!GetLocalizedTextMeshProUGUIGUID(out string localizedTextMeshProUGUIGUID))
            {
                return;
            }

            if (!GetTextMeshProUGUIGUID(out string textMeshProUGUIGUID))
            {
                return;
            }

            if (!CheckoutPrefab(stage.prefabAssetPath))
            {
                return;
            }

            UpdatePrefab(stage.prefabAssetPath, textMeshProUGUIGUID, localizedTextMeshProUGUIGUID);
        }

        private static bool GetLocalizedTextMeshProUGUIGUID(out string guid)
        {
            guid = AssetDatabase.FindAssets("t:monoscript", new[] {"Assets/Scripts/UIFrontEnd/Code/Localization"})[0];
            return guid != null;
        }

        private static bool GetTextMeshProUGUIGUID(out string guid)
        {
            guid = null;
            string[] textMeshProUGUIGUIDs = AssetDatabase.FindAssets("t:monoscript", new[] {"Packages/com.unity.textmeshpro/Scripts/Runtime"});
            foreach (string assetGUID in textMeshProUGUIGUIDs)
            {
                MonoScript monoScript = AssetDatabase.LoadAssetAtPath<MonoScript>(AssetDatabase.GUIDToAssetPath(assetGUID));
                if (monoScript == null)
                {
                    continue;
                }

                Type type = monoScript.GetClass();
                if (type == typeof(TextMeshProUGUI))
                {
                    guid = assetGUID;
                    break;
                }
            }

            return guid != null;
        }

        private static bool CheckoutPrefab(string prefabAssetPath)
        {
            Task task = Provider.Checkout(prefabAssetPath, CheckoutMode.Asset);
            task.Wait();
            return task.success;
        }

        private static void UpdatePrefab(string prefabAssetPath, string textMeshProUGUIGUID, string localizedTextMeshProUGUIGUID)
        {
            string prefabPath = Application.dataPath.Replace("Assets", "") + prefabAssetPath;
            string prefabContent = File.ReadAllText(prefabPath).Replace(textMeshProUGUIGUID, localizedTextMeshProUGUIGUID);
            File.WriteAllText(prefabPath, prefabContent);
            AssetDatabase.Refresh();
        }

        #endregion
    }
}