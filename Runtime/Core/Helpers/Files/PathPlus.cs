namespace Prateek.Runtime.Core.Helpers.Files
{
    using System.IO;

    ///-------------------------------------------------------------------------
    public static class PathPlus
    {
        public static string GetExtension(string file)
        {
            return Path.GetExtension(file).TrimStart(Strings.Separator.FileExtension.C());
        }

        public static string GetFilenameOnly(string file)
        {
            return Path.GetFileNameWithoutExtension(file);
        }
    }
}
