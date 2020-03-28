namespace Mayfair.Core.Editor.Addressables
{
    using System;
    using System.IO;
    using BuildTools.Preprocessing.Interfaces;
    using UnityEditor;
    using UnityEditor.AddressableAssets;
    using UnityEditor.AddressableAssets.Settings;
    using UnityEditor.Build.Reporting;
    using UnityEngine;
    using UnityEngine.AddressableAssets;

    /// <summary>
    /// This class will automatically run code pre and post a <see cref="BuildTools.AutoBuilder"/> build.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This class is referenced via reflection.
    /// </para>
    /// <para>
    /// Be aware that the copy/deletion of the Addressable Asset Content file(s) is duplication of functionality provided in the
    /// com.unity.addressables package. Unity's <see cref="AddressablesPlayerBuildProcessor"/> uses the unity provided build pre/post processing.
    /// </para>
    /// <para>
    /// We have opted not to rely on Unity's build processing pipeline at this time due to the feature being unreliable.
    /// This decision was made on December 9th 2019 against unity 2019.2.8f1 and Addressables 1.4.0.
    /// </para>
    /// </remarks>
    public class AddressablesBuildProcessor : IPreprocessBuild, IPostprocessBuild
    {
        public int CallbackOrder
        {
            get => 0;
        }

        private string GetContentStatePath(BuildTarget buildTarget)
        {
            return $"{Application.dataPath}/AddressableAssetsData/{buildTarget}/addressables_content_state.bin";
        }

        private void BuildAddressableAssetContent(BuildTarget buildTarget)
        {
            string contentStatePath = GetContentStatePath(buildTarget);
            if (File.Exists(contentStatePath))
            {
                Debug.Log("[AddressablesBuildProcessor] Making Addressable Asset Content editable.");
                File.SetAttributes(GetContentStatePath(buildTarget), FileAttributes.Normal);
            }
            else
            {
                Debug.Log("[AddressablesBuildProcessor] Addressable Asset Content could not be found.");
            }

            Debug.Log("[AddressablesBuildProcessor] Building Addressable Asset Content.");
            AddressableAssetSettings.CleanPlayerContent(AddressableAssetSettingsDefaultObject.Settings.ActivePlayerDataBuilder);

            AddressableAssetSettings.BuildPlayerContent();
        }

        private void CopyAddressablesData()
        {
            if (Directory.Exists(Addressables.BuildPath))
            {
                Debug.Log($"[AddressablesBuildProcessor] Copying Addressables data from {Addressables.BuildPath} to {Addressables.PlayerBuildDataPath}.  These copies will be deleted at the end of the build.");
                CopyDirectory(Addressables.BuildPath, Addressables.PlayerBuildDataPath);
            }
        }

        private void CopyDirectory(string sourceDirectoryName, string destDirectoryName)
        {
            DirectoryInfo sourceDirectory = new DirectoryInfo(sourceDirectoryName);

            if (!sourceDirectory.Exists)
            {
                throw new DirectoryNotFoundException($"[AddressablesBuildProcessor] Source directory does not exist or could not be found: {sourceDirectoryName}");
            }

            CreateDestinationDirectoryIfMissing(destDirectoryName);

            CopyFiles(sourceDirectory, destDirectoryName);
            CopySubdirectories(sourceDirectory, destDirectoryName);
        }

        private void CreateDestinationDirectoryIfMissing(string destDirectoryName)
        {
            if (!Directory.Exists(destDirectoryName))
            {
                Directory.CreateDirectory(destDirectoryName);
            }
        }

        private void CopyFiles(DirectoryInfo sourceDirectory, string destDirectoryName)
        {
            FileInfo[] files = sourceDirectory.GetFiles();
            foreach (FileInfo file in files)
            {
                string copyDestination = Path.Combine(destDirectoryName, file.Name);
                file.CopyTo(copyDestination, true);
            }
        }

        private void CopySubdirectories(DirectoryInfo sourceDirectory, string parentDirectoryName)
        {
            DirectoryInfo[] directories = sourceDirectory.GetDirectories();
            foreach (DirectoryInfo directoryInfo in directories)
            {
                string subdirectoryName = Path.Combine(parentDirectoryName, directoryInfo.Name);
                CopyDirectory(directoryInfo.FullName, subdirectoryName);
            }
        }

        private void DeletePreprocessCopiedAddressablesData()
        {
            string addressablesStreamingAssets = $"{Application.streamingAssetsPath}/{Addressables.StreamingAssetsSubFolder}";
            if (Directory.Exists(Addressables.PlayerBuildDataPath))
            {
                Debug.Log($"[AddressablesBuildProcessor] Deleting Addressables data from {Addressables.PlayerBuildDataPath} now that the build is complete.");
                Directory.Delete(Addressables.PlayerBuildDataPath, true);
            }

            // This ensures we do not leave an empty directory floating in our project
            DeleteDirectoryIfEmpty(addressablesStreamingAssets);
        }

        private void DeleteDirectoryIfEmpty(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                return;
            }

            if (Directory.GetFiles(directoryPath).Length == 0 && Directory.GetDirectories(directoryPath).Length == 0)
            {
                Directory.Delete(directoryPath, true);
            }
        }

        #region  IPreprocessBuild

        public void OnPreprocessBuild(BuildTarget buildTarget)
        {
            BuildAddressableAssetContent(buildTarget);
            CopyAddressablesData();
        }

        #endregion

        #region  IPostprocessBuild

        public void OnPostprocessBuild(BuildSummary buildSummary)
        {
            DeletePreprocessCopiedAddressablesData();
        }

        #endregion
    }
}