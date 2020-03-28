namespace Mayfair.Core.Editor.EditorService
{
    using System.Collections.Generic;
    using Mayfair.Core.Code.Utils;
    using UnityEditor;
    using UnityEngine;

    public static class EditorServiceHelpers
    {
        #region Class Methods
        public static void LoadAllAt<T>(string assetPath, ref List<T> results)
            where T : ScriptableObject
        {
            if (results == null)
            {
                results = new List<T>();
            }

            results.Clear();

            string filter = $"t:{typeof(T).Name}";
            string[] foundAssets = AssetDatabase.FindAssets(filter, new[] {$"{ConstsFolders.ASSET}/{assetPath}"});
            foreach (string foundAsset in foundAssets)
            {
                results.Add(AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(foundAsset)));
            }
        }
        #endregion
    }
}
