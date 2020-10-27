namespace Prateek.Runtime.AppContentFramework.Local.ContentFormats
{
    using System.Collections.Generic;
    using System.IO;
    using Prateek.Runtime.AppContentFramework.ContentLoaders;
    using Prateek.Runtime.Core.Interfaces.IPriority;

    public abstract class ContentFormat
        : IPriority
    {
        #region Properties
        public abstract string Extension { get; }
        #endregion

        #region Class Methods
        public abstract bool ExtractPath(string relativePath, FileInfo fileInfo, List<ContentPath> foundPaths);
        public abstract ContentLoader GetLoader(ContentPath contentPath);
        #endregion

        #region IPriority Members
        public abstract int DefaultPriority { get; }
        #endregion
    }
}
