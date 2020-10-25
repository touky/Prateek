namespace Prateek.Editor.AtomicFileFramework
{
    using System.IO;
    using System.Text.RegularExpressions;
    using Prateek.Runtime.Core.Helpers;

    public abstract class AtomicFileFormat
    {
        #region Static and Constants
        internal const string CONTENT_EXTENSION = ".atomcontent";
        internal const string HEADER_EXTENSION = ".atomheader";
        internal const string PART_EXTENSION = ".atompart";
        #endregion

        #region Properties
        public abstract string OriginalExtension { get; }

        public abstract Regex PartStartRegex { get; }

        public virtual string ContentExtension { get { return CONTENT_EXTENSION; } }

        public virtual string HeaderExtension { get { return HEADER_EXTENSION; } }

        public virtual string PartExtension { get { return PART_EXTENSION; } }
        #endregion

        #region Class Methods
        public abstract void Init();
        public abstract string FormatFilename(string originalName, Match match);

        protected static void ExportFile(string path)
        {
            AtomicFileFormatter.Instance.ExportFile(path);
        }
        
        public FileInfo GetAtomicHeader(string atomicPath)
        {
            var atomicInfo = new FileInfo(atomicPath);
            return GetAtomicHeader(atomicInfo);
        }

        public FileInfo GetAtomicHeader(FileInfo atomicInfo)
        {
            return GetAtomicHeader(atomicInfo, CONTENT_EXTENSION,  HEADER_EXTENSION);
        }

        public static FileInfo GetAtomicHeader(string atomicPath, string contentExtension, string headerExtension)
        {
            var atomicInfo = new FileInfo(atomicPath);
            return GetAtomicHeader(atomicInfo, contentExtension, headerExtension);
        }

        public static FileInfo GetAtomicHeader(FileInfo atomicInfo, string contentExtension, string headerExtension)
        {
            var sourceInfo = new FileInfo(atomicInfo.DirectoryName.RemoveExtension(CONTENT_EXTENSION));
            var headerInfo = new FileInfo(Path.Combine(atomicInfo.DirectoryName, Path.ChangeExtension(sourceInfo.Name, HEADER_EXTENSION)));
            return headerInfo;
        }
        #endregion
    }
}
