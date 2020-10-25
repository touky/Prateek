namespace Prateek.Runtime.AppContentFramework.Local.ContentFormat
{
    using System.Collections.Generic;
    using System.IO;
    using Prateek.Runtime.Core.Interfaces.IPriority;

    public abstract class ContentFormat
        : IPriority
    {
        #region Properties
        public abstract string Extension { get; }
        #endregion

        #region Class Methods
        public abstract bool ExtractPath(string relativePath, FileInfo fileInfo, List<ContentPath> foundPaths);
        #endregion

        #region IPriority Members
        public abstract int DefaultPriority { get; }
        #endregion
    }
}
