namespace Mayfair.Core.Editor.Utils
{
    using System.IO;
    using Mayfair.Core.Code.Utils;
    using UnityEngine;

    public static class PathHelper
    {
        #region Static and Constants
        private static readonly string PATH_UPDIR = $"..{Path.AltDirectorySeparatorChar}";
        private const int BEFORE_DIR_SEPARATOR = -2;
        #endregion

        #region Class Methods
        public static string Simplify(string path)
        {
            path = path.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

            while (path.Contains(PATH_UPDIR))
            {
                int index = path.IndexOf(PATH_UPDIR);
                if (index > Consts.INDEX_NONE)
                {
                    path = path.Remove(index, PATH_UPDIR.Length);

                    if (index > 0)
                    {
                        int minIndex = Mathf.Max(0, path.LastIndexOf(Path.AltDirectorySeparatorChar, index + BEFORE_DIR_SEPARATOR));
                        if (minIndex > Consts.INDEX_NONE)
                        {
                            path = path.Remove(minIndex, index - 1 - minIndex);
                        }
                    }
                }
                else
                {
                    break;
                }
            }

            return path;
        }
        #endregion
    }
}
