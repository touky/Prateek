namespace Mayfair.Core.Code.GameData
{
    using System.Diagnostics;
    using Google.Protobuf;
    using Mayfair.Core.Code.SaveGame;

    public abstract class ProtocolGameDataServiceProvider : GameDataServiceProvider
    {
        #region Static and Constants
        protected const string extension = "protosave";
        #endregion

        #region Nested type: ProtocolGameDataIdentification
        protected abstract class ProtocolGameDataIdentification<TData> : SaveDataIdentification<TData>
            where TData : class, IMessage<TData>
        {
            #region Properties
            public override string FileName
            {
                get { return $"{typeof(TData).Name}.{extension}"; }
            }

            private MessageParser<TData> ParserTyped
            {
                get
                {
                    Debug.Assert(Parser is MessageParser<TData>);

                    return (MessageParser<TData>) Parser;
                }
            }

            protected abstract MessageParser Parser { get; }
            #endregion

            #region Class Methods
            protected override bool TryLoad(byte[] data)
            {
                this.data = ParserTyped.ParseFrom(data);

                return this.data != null;
            }

            protected override bool TryLoad(string data)
            {
                this.data = ParserTyped.ParseJson(data);

                return this.data != null;
            }

            protected override bool TrySave(out byte[] data)
            {
                Debug.Assert(this.data != null);

                data = this.data.ToByteArray();

                return data != null;
            }

            protected override bool TrySave(out string data)
            {
                Debug.Assert(this.data != null);

                data = this.data.ToString();

                return data != string.Empty;
            }
            #endregion
        }
        #endregion
    }
}
