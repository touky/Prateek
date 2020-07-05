namespace Mayfair.Core.Editor.Database.Protocol
{
#if NVIZZIO
    using Nvizzio.Core.Utils;
#endif //NVIZZIO
    using Editor.Protocol;
    using Google.Protobuf;
    using Google.Protobuf.WellKnownTypes;
    using Mayfair.Core.Code.Protocol;
    using Mayfair.Plugins.AtDbMayfair.Editor;
    using Mayfair.Plugins.AtDbMayfair.Editor.Containers;
    using Utils;

    public class ProtocolDatabaseExporterConfiguration : DatabaseExporterConfiguration
    {
        #region Class Methods
        public override object SerializationFunction(object model, bool shouldCompress)
        {
            string result = string.Empty;

            IMessage message = model as IMessage;
            if (message == null)
            {
                return null;
            }

            return message;
        }

        public override string CompressionFunction(string data)
        {
            return data;
        }

        public override bool WriteData(string path, object data, SourceIdentification sourceIdentification)
        {
            IMessage message = data as IMessage;
            ProtocolDatabaseEnvelope envelope = new ProtocolDatabaseEnvelope();
            envelope.ExportSource = sourceIdentification.ToString();
            envelope.InnerMessage = Any.Pack(message);

            ProtocolExportContent content = new ProtocolExportContent(envelope, path);
            ExporterHelper.ExportToFile(content);
            return true;
        }
        #endregion
    }
}
