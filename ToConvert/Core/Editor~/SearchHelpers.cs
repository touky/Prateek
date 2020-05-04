namespace Mayfair.Core.Editor
{
    using System.Reflection;
    using UnityEditor;
    using UnityEngine;

    public static class SearchHelpers
    {
#if UNITY_EDITOR
        public enum SearchFilter
        {
            FILTERMODE_ALL = 0,
            FILTERMODE_NAME = 1,
            FILTERMODE_TYPE = 2,

            MAX
        }

        public static void SetSearchFilter(string filter, SearchFilter filterMode)
        {
            SearchableEditorWindow hierarchy = null;
            SearchableEditorWindow[] windows = (SearchableEditorWindow[]) Resources.FindObjectsOfTypeAll(typeof(SearchableEditorWindow));
            foreach (SearchableEditorWindow window in windows)
            {
                if (window.GetType().ToString() == "UnityEditor.SceneHierarchyWindow")
                {
                    hierarchy = window;
                    break;
                }
            }

            if (hierarchy == null)
            {
                return;
            }

            MethodInfo setSearchType = typeof(SearchableEditorWindow).GetMethod("SetSearchFilter", BindingFlags.NonPublic | BindingFlags.Instance);
            object[] parameters = {filter, (int) filterMode, false};

            setSearchType.Invoke(hierarchy, parameters);
        }
#endif //UNITY_EDITOR
    }
}
