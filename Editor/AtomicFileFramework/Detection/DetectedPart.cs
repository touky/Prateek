namespace Prateek.Editor.AtomicFileFramework.Detection
{
    using System.IO;
    using System.Text.RegularExpressions;

    /// <summary>
    ///     A detected part of a yaml object
    /// </summary>
    internal class DetectedPart
    {
        #region Fields
        public AtomicFileFormat fileFormat;
        public string[] content;
        public Match match;
        public int startLine;
        public string partContent;
        #endregion

        #region Class Methods
        public void LoadContent()
        {
            partContent = string.Empty;
            var line  = string.Empty;
            var match = (Match) null;
            do
            {
                line = content[startLine++];
                partContent += string.Format(AtomicFileFormatter.lineFeed, line);

                if (startLine >= content.Length)
                {
                    break;
                }

                line = content[startLine];
                match = fileFormat.PartStartRegex.Match(line);
            } while (!match.Success);
        }

        public string GetFilename(string originalName)
        {
            var filename = string.Empty;
            if (match == null)
            {
                filename = Path.ChangeExtension(originalName, fileFormat.HeaderExtension);
            }
            else
            {
                filename = fileFormat.FormatFilename(originalName, match);
            }

            return filename;
        }

        public void Save(string destination, string filename)
        {
            var dirInfo = new DirectoryInfo(destination);
            if (!dirInfo.Exists)
            {
                Directory.CreateDirectory(dirInfo.FullName);
            }

            File.WriteAllText(Path.Combine(destination, filename), partContent);
        }
        #endregion
    }
}
