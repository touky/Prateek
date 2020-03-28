namespace Mayfair.Core.Code.Messaging.Messages
{
    using System;
    using System.Diagnostics;
    using System.Reflection;
    using System.Text;
    using Mayfair.Core.Code.Messaging.Communicator;
    using Mayfair.Core.Code.Utils.Debug;

    [DebuggerDisplay("{GetType().Name}, Sender: {sender.Owner.Name}")]
    public abstract class Message
    {
        #region Static and Constants
        public const long ID_COMMUNICATOR_MASK = 0xFFFFFFFF;
        public const long ID_TYPE_MASK = ~ID_COMMUNICATOR_MASK;
        #endregion

        #region Fields
        private ILightMessageCommunicator sender;
        #endregion

        #region Properties
        public ILightMessageCommunicator Sender
        {
            get { return sender; }
            set { sender = value; }
        }

        //We allow message type spoofing for Children messages
        public virtual long MessageID
        {
            get { return ConvertToId(GetType()); }
        }
        #endregion

        #region Constructors
        protected Message()
        {
            TraceHelper.EnsureTrace<Message>("Create");
        }
        #endregion

        #region Class Methods
        public static T Create<T>() where T : Message, new()
        {
            return new T();
        }

        /// <summary>
        ///     The ID is generated from the hash code of the type and the communicator.
        ///     Since both are int 32, a long 64 is a perfect fit:
        ///     32 bits for the type, 32 bits for the recipient, 64 bits to bring them all, and in the darkness bind them
        /// </summary>
        /// <param name="type">The type of the message</param>
        /// <param name="communicator">Optional: the communicator used for the message</param>
        /// <returns></returns>
        public static long ConvertToId(Type type, ILightMessageCommunicator communicator = null)
        {
            long id = 0;
            if (type != null)
            {
                id = ((long) type.GetHashCode() << 32) & ID_TYPE_MASK;
            }

            if (communicator != null)
            {
                id |= communicator.GetHashCode() & ID_COMMUNICATOR_MASK;
            }

            return id;
        }

        public override string ToString()
        {
            Type type = GetType();

            bool hasSeveral = false;
            StringBuilder builder = new StringBuilder();
            builder.Append(type.Name);

            TypeInfo typeInfo = type.GetTypeInfo();
            if (typeInfo.GenericTypeArguments.Length > 0)
            {
                builder.Append("<");
                foreach (Type argument in typeInfo.GenericTypeArguments)
                {
                    if (hasSeveral)
                    {
                        builder.Append("/");
                    }

                    builder.Append(argument.Name);
                    hasSeveral = true;
                }

                builder.Append(">");
            }

            return builder.ToString();
        }
        #endregion
    }
}
