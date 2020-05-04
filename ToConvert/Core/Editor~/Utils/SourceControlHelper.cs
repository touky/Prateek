namespace Mayfair.Core.Editor.Utils
{
    using System.Diagnostics;
    using System.IO;
    using Mayfair.Core.Code.Utils;
    using UnityEditor;
    using UnityEditor.VersionControl;
    using UnityEngine;

    public static class SourceControlHelper
    {
        #region Static and Constants
        private static readonly string P4_USERNAME = "Username";
        private static readonly string P4_WORKSPACE = "Workspace";
        private static readonly string P4_SERVER = "Server";
        #endregion

        #region Class Methods
        public static string DirectoryToAsset(string path)
        {
            path = PathHelper.Simplify(path);

            if (path.Contains(ConstsFolders.ASSET))
            {
                return path.Replace(Application.dataPath, ConstsFolders.ASSET);
            }

            string[] dataPaths = Application.dataPath.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            foreach (string dataPath in dataPaths)
            {
                if (path.StartsWith(dataPath))
                {
                    path = path.Remove(0, dataPath.Length + 1);
                    continue;
                }

                break;
            }

            return path;
        }

        public static void SelectInPerforce(string path)
        {
            string[] configValue = {P4_WORKSPACE, P4_SERVER, P4_USERNAME};
            ConfigField[] fields = Provider.GetActiveConfigFields();
            foreach (ConfigField field in fields)
            {
                for (int v = 0; v < configValue.Length; v++)
                {
                    if (configValue[v] == field.label)
                    {
                        configValue[v] = EditorUserSettings.GetConfigValue(field.name);
                    }
                }
            }

            Process process = ProcessHelper.GetNewProcess(Path.GetDirectoryName(path), "start", $"p4v -c {configValue[0]} -p {configValue[1]} -u {configValue[2]} -s {path}"); //
            if (process.Start()) { }
        }
        
        public static void MarkForDelete(string path)
        {
            if (!File.Exists(path) && !Directory.Exists(path))
            {
                return;
            }

            string assetPath = DirectoryToAsset(path);
            if (assetPath == null)
            {
                return;
            }

            Asset asset = Provider.GetAssetByPath(assetPath);
            if (asset != null)
            {
                Task task = Provider.Delete(asset);
                task.Wait();
            }
        }

        public static void CheckoutOrAdd(string path)
        {
            if (!File.Exists(path) && !Directory.Exists(path))
            {
                return;
            }

            string assetPath = DirectoryToAsset(path);
            if (assetPath == null)
            {
                return;
            }

            Asset asset = Provider.GetAssetByPath(assetPath);
            if (asset != null)
            {
                if (Provider.onlineState == OnlineState.Online)
                {
                    Task task = null;
                    if (Provider.CheckoutIsValid(asset))
                    {
                        task = Provider.Checkout(asset, CheckoutMode.Asset);
                    }
                    else
                    {
                        task = Provider.Add(asset, true);
                    }

                    task.Wait();
                }
            }
        }

        public static void RevertUnchanged(string path)
        {
            if (!File.Exists(path) && !Directory.Exists(path))
            {
                return;
            }

            string assetPath = DirectoryToAsset(path);
            if (assetPath == null)
            {
                return;
            }

            Asset asset = Provider.GetAssetByPath(assetPath);
            if (asset != null)
            {
                if (Provider.onlineState == OnlineState.Online)
                {
                    Task task = Provider.Revert(asset, RevertMode.Unchanged);
                    task.Wait();
                }
            }
        }

        public static void CheckoutOrAddAndRevertUnchanged(string path)
        {
            CheckoutOrAdd(path);
            RevertUnchanged(path);
        }
        #endregion
    }
}
