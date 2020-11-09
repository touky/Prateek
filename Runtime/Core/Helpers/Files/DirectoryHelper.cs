namespace Prateek.Runtime.Core.Helpers.Files
{
    using System.IO;
    using UnityEngine;

    ///-------------------------------------------------------------------------
    public static class DirectoryHelper
    {
        ///-------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        public static DirectoryInfo DirectoryMustExist(string path)
        {
            if (!Directory.Exists(path))
            {
                try
                {
                    return Directory.CreateDirectory(path);
                }
                catch
                {
                    Debug.LogError($"{nameof(DirectoryHelper)}: Can't create folder: {path}");
                }
            }

            return new DirectoryInfo(path);
        }
    }
}
