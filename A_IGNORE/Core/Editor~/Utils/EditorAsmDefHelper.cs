namespace Mayfair.Core.Editor.Utils
{
    using UnityEditor;

    public static class EditorAsmDefHelper
    {
        #region Static and Constants
        private const string ASMDEF_PATH = "Assemblies/Reload";
        private const string ASMDEF_FILTER = "Mayfair t:asmdef";
        #endregion

        #region Class Methods
        [MenuItem(EditorMenuOrderHelper.WINDOW_MENU + "Assemblies/FAILSAFE Unlock Assemblies", priority = EditorMenuOrderHelper.IMPORTANT_0)]
        private static void FAILSAFE_UnlockAssemblies()
        {
            EditorApplication.UnlockReloadAssemblies();
        }

        [MenuItem(EditorMenuOrderHelper.WINDOW_MENU + ASMDEF_PATH + " All", priority = EditorMenuOrderHelper.BAR_MENU_GROUP1_0)]
        private static void ReloadAllAssemblies()
        {
            ReloadAssemblies(string.Empty);
        }

        [MenuItem(EditorMenuOrderHelper.WINDOW_MENU + ASMDEF_PATH + " All Code", priority = EditorMenuOrderHelper.BAR_MENU_GROUP1_1)]
        private static void ReloadAllCodeAssemblies()
        {
            ReloadAssemblies("Code");
        }

        [MenuItem(EditorMenuOrderHelper.WINDOW_MENU + ASMDEF_PATH + " All Editor", priority = EditorMenuOrderHelper.BAR_MENU_GROUP1_2)]
        private static void ReloadAllEditorAssemblies()
        {
            ReloadAssemblies("Editor");
        }

        private static void ReloadAssemblies(string asmName)
        {
            string[] assets = AssetDatabase.FindAssets($"{asmName} {ASMDEF_FILTER}");
            foreach (string asset in assets)
            {
                string path = AssetDatabase.GUIDToAssetPath(asset);
                AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
            }
        }
        #endregion
    }
}
