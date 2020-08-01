namespace Commands.Core
{
    using System;
    using System.Diagnostics;
    using System.Reflection;
    using System.Text;
    using Prateek.CommandFramework.TransmitterReceiver;

    [DebuggerDisplay("{GetType().Name}, Sender: {emitter.Owner.Name}")]
    public abstract class Command
    {
        #region Static and Constants
        public const long ID_COMMAND_MASK = 0xFFFFFFFF;
        public const long ID_TYPE_MASK = ~ID_COMMAND_MASK;
        #endregion

        #region Fields
        private ICommandEmitter emitter;
        #endregion

        #region Properties
        public ICommandEmitter Emitter
        {
            get { return emitter; }
            set { emitter = value; }
        }

        //We allow notice type spoofing for Children notices
        public virtual long CommandID
        {
            get { return ConvertToId(GetType()); }
        }
        #endregion

        #region Constructors
        protected Command()
        {
            //todo: TraceHelper.EnsureTrace<Notice>("Create");
        }
        #endregion

        #region Class Methods
        public static T Create<T>() where T : Command, new()
        {
            return new T();
        }

        /// <summary>
        ///     The ID is generated from the hash code of the type and the noticeReceiver.
        ///     Since both are int 32, a long 64 is a perfect fit:
        ///     32 bits for the type, 32 bits for the recipient, 64 bits to bring them all, and in the darkness bind them
        /// </summary>
        /// <param name="type">The type of the notice</param>
        /// <param name="transmitter">Optional: the noticeReceiver used for the notice</param>
        /// <returns></returns>
        public static long ConvertToId(Type type, ICommandEmitter transmitter = null)
        {
            long id = 0;
            if (type != null)
            {
                id = ((long) type.GetHashCode() << 32) & ID_TYPE_MASK;
            }

            if (transmitter != null)
            {
                id |= transmitter.GetHashCode() & ID_COMMAND_MASK;
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
