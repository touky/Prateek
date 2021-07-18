namespace Prateek.Runtime.AppContentFramework.Local.ContentFormats
{
    using System.Collections.Generic;
    using System.IO;
    using Prateek.Runtime.AppContentFramework.ContentLoaders;
    using Prateek.Runtime.AppContentFramework.Local.ContentLoaders;

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

        public override LocalContentLoader GetLoader(ContentPath contentPath)
        {
            return new DefaultLocalContentLoader(contentPath.StoragePath, contentPath);
        }
        #endregion
    }
}
