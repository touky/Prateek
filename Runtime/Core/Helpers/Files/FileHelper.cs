namespace Prateek.Runtime.Core.Helpers.Files
{
    using System.IO;
    using Prateek.Runtime.Core.Consts;
    using UnityEngine;
    using UnityEngine.UIElements;

    ///-------------------------------------------------------------------------
    public static class FileHelper
    {
        ///-------------------------------------------------------------------------
        /// <summary>
        /// Read the content at the path into a new class of the given type
        /// </summary>
        public static TClass Read<TClass>(string path)
            where TClass : class
        {
            return Read<TClass>(new FileInfo(path));
        }

        ///-------------------------------------------------------------------------
        /// <summary>
        /// Read the content at the path into a new class of the given type
        /// </summary>
        public static TClass Read<TClass>(FileInfo fileInfo)
            where TClass : class
        {
            if (!fileInfo.Exists)
            {
                Debug.LogError($"Trying to read from inexistant file '{fileInfo.FullName}'");
                return null;
            }

            if (fileInfo.HasExtension(ConstExtension.JSON))
            {
                return ReadJson<TClass>(fileInfo);
            }

            return null;
        }

        ///-------------------------------------------------------------------------
        private static TClass ReadJson<TClass>(FileInfo fileInfo)
            where TClass : class
        {
            var json   = File.ReadAllText(fileInfo.FullName);
            var result = JsonUtility.FromJson<TClass>(json);
            return result;
        }

        ///-------------------------------------------------------------------------
        /// <summary>
        /// Write the given object to the given path using the extension as hint about the format
        /// </summary>
        public static bool Write<TClass>(TClass data, string path)
            where TClass : class
        {
            return Write(data, new FileInfo(path));
        }

        ///-------------------------------------------------------------------------
        /// <summary>
        /// Write the given object to the given path using the extension as hint about the format
        /// </summary>
        public static bool Write<TClass>(TClass data, FileInfo fileInfo)
            where TClass : class
        {
            var directoryInfo = fileInfo.Directory;
            if (!directoryInfo.Exists)
            {
                directoryInfo = DirectoryHelper.DirectoryMustExist(directoryInfo.FullName);
                if (!directoryInfo.Exists)
                {
                    return false;
                }
            }

            if (fileInfo.HasExtension(ConstExtension.JSON))
            {
                return WriteJson(data, fileInfo);
            }

            return false;
        }

        ///-------------------------------------------------------------------------
        private static bool WriteJson<TClass>(TClass data, FileInfo fileInfo)
            where TClass : class
        {
            try
            {
                var result = JsonUtility.ToJson(data, true);
                File.WriteAllText(fileInfo.FullName, result);
            }
            catch
            {
                Debug.LogError($"{nameof(FileHelper)}: Failed writing {typeof(TClass).Name} into '{fileInfo.FullName}'");
                return false;
            }

            return true;
        }
    }
}
