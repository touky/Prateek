﻿namespace Prateek.Runtime.CommandFramework.Commands.Core
{
    using System;
    using System.Diagnostics;
    using Prateek.Runtime.Core.Extensions;

    [DebuggerDisplay("{DebugDisplay,nq}")]
    public struct CommandId
    {
        #region Static and Constants
        public const int TYPE_OFFSET = 32;
        public const long MASK_TARGET = 0xFFFFFFFF;
        public const long MASK_TYPE = ~MASK_TARGET;
        #endregion

        #region Fields
        private Type type;
        private object target;
        private long idValue;
        #endregion

        #region Properties
        private string DebugDisplay { get { return $"{type.Name}/{target}: {Key}/{KeyDebugDisplay}"; } }

        internal string KeyDebugDisplay
        {
            get
            {
                var left = ((Int32) ((Key & MASK_TYPE) >> 32)).ToHex();
                var right = ((Int32) (Key & MASK_TARGET)).ToHex();
                return $"{left}-{right}";
            }
        }

        public Type Type { get { return type; } }

        public long Key
        {
            get
            {
                if (idValue != 0)
                {
                    return idValue;
                }

                idValue = CreateId(type, target);

                return idValue;
            }
        }
        #endregion

        #region Constructors
        public CommandId(Type type, object target)
        {
            this.type = type;
            this.target = target;
            idValue = 0;
        }
        #endregion

        #region Class Methods
        public static implicit operator CommandId(Type type)
        {
            return new CommandId(type, null);
        }

        public static implicit operator CommandId(long idValue)
        {
            return new CommandId {idValue = idValue};
        }

        /// <summary>
        ///     The ID is generated from the hash code of the type of command and the target.
        ///     Since both are int 32, a long 64 is a perfect fit:
        ///     32 bits for the type, 32 bits for the target, 64 bits to bring them all, and in the darkness bind them
        /// </summary>
        /// <param name="type">The type of the command</param>
        /// <param name="target">Optional: the target used for the command</param>
        /// <returns></returns>
        public static long CreateId(Type type, object target = null)
        {
            long id = 0;
            if (type != null)
            {
                id = ((long) type.GetHashCode() << TYPE_OFFSET) & MASK_TYPE;
            }

            if (target != null)
            {
                id |= target.GetHashCode() & MASK_TARGET;
            }

            return id;
        }
        #endregion
    }
}
