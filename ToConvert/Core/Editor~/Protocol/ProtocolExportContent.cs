namespace Mayfair.Core.Editor.Protocol
{
    using System.IO;
    using Code.Protocol;
    using FBXExporter;
    using Google.Protobuf;

    public class ProtocolExportContent : ExportContent
    {
        public override ExporterType Type => ExporterType.PROTOCOL;
        public ProtocolDatabaseEnvelope Content { get; }
        public override int ContentCount => 1;

        public ProtocolExportContent(ProtocolDatabaseEnvelope content, string filenamePath)
        {
            Content = content;
            ExportPath = filenamePath;
        }
        
        public override string GetExportPath()
        {
            return ExportPath;
        }

        public override object ContentAtIndex(int index)
        {
            return Content;
        }
        
        public override string ExportData(string path)
        {
            string directoryPath = Path.GetDirectoryName(path);

            if (string.IsNullOrEmpty(directoryPath))
            {
                return "";
            }
            Directory.CreateDirectory(directoryPath);
            using (FileStream output = File.Open(path, FileMode.OpenOrCreate, FileAccess.Write))
            {
                Content.WriteTo(output);
            }

            return path;
        }
    }
}