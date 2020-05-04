namespace Mayfair.Core.Editor.Utils
{
    using System.IO;
    using Mayfair.Core.Code.Utils;
    using UnityEngine;

    public static class IOHelper
    {
        #region Class Methods
        public static bool MoveOrReplaceFile(string tempPath, string destPath)
        {
            FileInfo tempFile = new FileInfo(tempPath);
            FileInfo destFile = new FileInfo(destPath);

            Debug.Assert(tempFile.Exists);

            if (destFile.Exists && destFile.IsReadOnly)
            {
                Debug.LogError($"Can't Move Or Replace File, {destFile.Name} is read-only");
                return false;
            }

            if (!destFile.Exists)
            {
                Directory.CreateDirectory(destFile.Directory.FullName);
                File.Move(tempFile.FullName, destFile.FullName);
            }
            else
            {
                string replacingPath = $"{destFile.FullName}{ConstsFolders.TEMP}";
                File.Replace(tempFile.FullName, destFile.FullName, replacingPath);
                File.Delete(replacingPath);
            }

            return true;
        }
        #endregion
    }
}
