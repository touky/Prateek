namespace Assets.Prateek.ToConvert.Helpers
{
    using System.IO;

    public static class PathHelper
    {
        #region Class Methods
        public static string RemoveLeadingAndTrailingSlashes(string path)
        {
            if (path[0] == Path.DirectorySeparatorChar || path[0] == Path.AltDirectorySeparatorChar)
            {
                path = path.Substring(1);
            }

            if (path[path.Length - 1] == Path.DirectorySeparatorChar || path[path.Length - 1] == Path.AltDirectorySeparatorChar)
            {
                path = path.Substring(0, path.Length - 1);
            }

            return path;
        }
        #endregion
    }
}
