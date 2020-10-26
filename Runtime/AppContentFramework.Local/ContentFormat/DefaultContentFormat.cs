namespace Prateek.Runtime.AppContentFramework.Local.ContentFormat
{
    using System.Collections.Generic;
    using System.IO;
    using Prateek.Runtime.AppContentFramework.Loader;
    using Prateek.Runtime.AppContentFramework.Local.ContentLoader;

    public class DefaultContentFormat
        : ContentFormat
    {
        #region Properties
        public override int DefaultPriority { get { return int.MaxValue; } }
        public override string Extension { get { return string.Empty; } }
        #endregion

        #region Class Methods
        public override bool ExtractPath(string relativePath, FileInfo fileInfo, List<ContentPath> foundPaths)
        {
            foundPaths.Add(new ContentPath(relativePath, fileInfo, this));
            return true;
        }

        public override ContentLoader GetLoader(ContentPath contentPath)
        {
            return new LocalContentLoader(contentPath.StoragePath, contentPath);
        }
        #endregion
    }
}
