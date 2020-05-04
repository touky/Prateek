// -BEGIN_PRATEEK_COPYRIGHT-
//
// -END_PRATEEK_COPYRIGHT-

// -BEGIN_PRATEEK_CSHARP_IFDEF-
// -END_PRATEEK_CSHARP_IFDEF-

namespace Prateek.EditorTools.Editor
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using UnityEditor;
    using UnityEngine;

    public static class ProjectCleaning
    {

        private static string ASSET = "Assets";

        #region Member Methods

        [MenuItem("Prateek/Tools/Project Cleaning/Clean project files and launch IDE")]
        public static void CleanProjectAndLaunchIDE()
        {
            Debug.Log(string.Format("Beginning deletion of project files for {0}", Application.productName));
            
            string path = Directory.GetCurrentDirectory();
            DeleteAllFilesWithExtension(path, ".csproj");
            DeleteAllFilesWithExtension(path, ".sln");

            EditorApplication.ExecuteMenuItem("Assets/Open C# Project");
        }

        [MenuItem("Prateek/Tools/Project Cleaning/Remove Empty Directories", false, 800)]
        public static void CleanEmptyDirectories()
        {
            RemoveEmptyDirectories(ASSET + Path.DirectorySeparatorChar);
        }

        private static void DeleteAllFilesWithExtension(string path, string extension)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            extension = extension.Replace(".", string.Empty);

            FileInfo[] allFiles = directoryInfo.GetFiles($"*.{extension}");

            foreach (FileInfo fileInfo in allFiles)
            {
                File.Delete(fileInfo.FullName);
            }
        }

        private static void RemoveEmptyDirectories(string directoryPath)
        {
            EditorApplication.LockReloadAssemblies();

            IList<string> deleteList = GetDirectoriesToDelete(directoryPath);

            EditorUtility.ClearProgressBar();

            foreach (string directoryName in deleteList.Reverse())
            {
                if (Directory.Exists(directoryName))
                {
                    DeleteDirectory(directoryName);
                }
            }

            EditorApplication.UnlockReloadAssemblies();
            AssetDatabase.Refresh();
        }

        private static IList<string> GetDirectoriesToDelete(string directoryPath)
        {
            IList<string> deleteList = new List<string>();
            List<string> directories = new List<string>(Directory.GetDirectories(directoryPath, "*", SearchOption.AllDirectories));

            for (int i = 0; i < directories.Count; i++)
            {
                float progress = (float)i / directories.Count;
                EditorUtility.DisplayProgressBar("Busy", "Removing empty directories", progress);

                List<string> files = new List<string>(Directory.GetFiles(directories[i], "*", SearchOption.AllDirectories));

                if (ContainsNonMetaFiles(files))
                {
                    continue;
                }

                deleteList.Add(directories[i]);
            }

            return deleteList;
        }

        private static bool ContainsNonMetaFiles(List<string> files)
        {
            foreach (string filename in files)
            {
                FileInfo file = new FileInfo(filename);

                if (file.Extension.ToLowerInvariant() != ".meta")
                {
                    return true;
                }
            }

            return false;
        }

        private static void DeleteDirectory(string directoryName)
        {
            DirectoryInfo parent = Directory.GetParent(directoryName);
            if (parent != null)
            {
                FileInfo directoryMeta = new FileInfo(directoryName + ".meta");
                if (directoryMeta.Exists)
                {
                    directoryMeta.Attributes &= ~FileAttributes.ReadOnly;
                    directoryMeta.Delete();
                }
            }

            DirectoryInfo info = new DirectoryInfo(directoryName);
            info.Attributes &= ~FileAttributes.ReadOnly;
            info.Delete();

            Debug.Log("Removed empty path " + directoryName);
        }

        #endregion
    }
}