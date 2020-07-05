// @formatter:off Deactivating the formatter for protocol
namespace Mayfair.Core.Code.Protocol
    // @formatter:on
{
    using UnityEngine;

    public class ProtocolDatabaseContainer : ScriptableObject
    {
        #region Settings
        [SerializeField]
        private byte[] datas;

        [SerializeField]
        private string source;

        [SerializeField]
        private string className;
        #endregion

        #region Properties
        public string Source
        {
            get { return source; }
        }

        public string ClassName
        {
            get { return className; }
        }
        #endregion

        #region Class Methods
        public ProtocolDatabaseEnvelope GetEnvelope()
        {
            ProtocolDatabaseEnvelope envelope = ProtocolDatabaseEnvelope.Parser.ParseFrom(datas);
            if (envelope == null)
            {
                return null;
            }

            return envelope;
        }

        public static ProtocolDatabaseContainer CreateInstance(byte[] bytes)
        {
            ProtocolDatabaseContainer database = CreateInstance<ProtocolDatabaseContainer>();
            database.datas = bytes;

            ProtocolDatabaseEnvelope envelope = database.GetEnvelope();
            if (envelope == null)
            {
                Destroy(database);
                return null;
            }

            database.source = envelope.ExportSource;
            database.className = envelope.InnerMessage.TypeUrl;
            database.datas = bytes;
            return database;
        }
        #endregion
    }
}
