namespace Mayfair.Core.Editor.DebugMenu
{
    using System.Collections;
    using System.Collections.Generic;
    using Mayfair.Core.Code.DebugMenu;
    using Mayfair.Core.Editor;
    using Mayfair.Core.Editor.EditorWindows;
    using UnityEditor;
    using UnityEngine;

    public static class DebugMenuOpen
    {
        private const string TOGGLE_TITLE = "Toggle debug menu";

        [MenuItem(EditorMenuOrderHelper.WINDOW_MENU + TOGGLE_TITLE, priority = EditorMenuOrderHelper.BAR_MENU_GROUP2_0)]
        public static void DoOpen()
        {
            DebugMenuService.Toggle();
        }

        [MenuItem(EditorMenuOrderHelper.WINDOW_MENU + TOGGLE_TITLE, true)]
        public static bool DoOpenValidate()
        {
            return EditorApplication.isPlaying;
        }
    }
}