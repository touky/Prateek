// @formatter:off Deactivating the formatter for protocol
namespace Mayfair.Core.Code.Protocol
    // @formatter:on
{
    using System;
    using System.Collections.Generic;
    using Google.Protobuf;
    using Mayfair.Core.Code.Database.Interfaces;
    using Mayfair.Core.Code.Utils.Debug;

    public class DatabaseHelper
    {
        #region Static and Constants
        protected static List<Unpacker> unpackers = new List<Unpacker>();
        #endregion

        #region Properties
        public static Type UnpackerType
        {
            get { return typeof(Unpacker); }
        }
        #endregion

        #region Class Methods
        public static IDatabaseTable TryUnpack(ProtocolDatabaseContainer container)
        {
            for (int u = 0; u < unpackers.Count; u++)
            {
                Unpacker unpacker = unpackers[u];
                if (unpacker == null)
                {
                    continue;
                }

                return unpacker.TryUnpack(container);
            }

            return null;
        }

        public static bool Transfer<T>(IList<T> source, List<IDatabaseEntry> destination) where T : IDatabaseEntry
        {
            for (int l = 0; l < source.Count; l++)
            {
                destination.Add(source[l]);
            }

            return destination.Count > 0;
        }
        #endregion

        #region Nested type: Unpacker
        protected abstract class Unpacker
        {
            #region Constructors
            protected Unpacker()
            {
                unpackers.Add(this);
            }
            #endregion

            #region Class Methods
            protected static bool TryUnpack<T>(ProtocolDatabaseEnvelope envelope, out IDatabaseTable entry) where T : IMessage, IDatabaseTable, new()
            {
                entry = null;

                T unpacked = default;
                if (envelope.InnerMessage.TryUnpack(out unpacked))
                {
                    entry = unpacked;
                }

                return entry != null;
            }

            public abstract IDatabaseTable TryUnpack(ProtocolDatabaseContainer container);
            #endregion
        }
        #endregion
    }
}
