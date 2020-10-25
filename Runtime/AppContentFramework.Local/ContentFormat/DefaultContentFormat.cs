namespace Prateek.Runtime.AppContentFramework.Local.ContentFormat
{
    using System.Collections.Generic;
    using System.IO;

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
        #endregion
    }
}
