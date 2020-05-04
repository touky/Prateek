namespace Mayfair.Core.Editor.Utils
{
    using System.Diagnostics;
    using UnityEditor;
    using UnityEngine;

    public class OpenDirectoryShortcuts
    {
        [MenuItem("Tools/Directory Shortcuts/Open Persistent Data Path")]
        public static void OpenPersistentDataPath()
        {
            Process.Start(Application.persistentDataPath);
        }

        [MenuItem("Tools/Directory Shortcuts/Open Databases Path")]
        public static void OpenDatabasesPath()
        {
            string databasesPath = $"{Application.dataPath}/../Database";
            Process.Start(databasesPath);
        }
    }
}