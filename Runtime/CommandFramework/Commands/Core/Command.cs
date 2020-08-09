namespace Prateek.Runtime.CommandFramework.Commands.Core
{
    using System.Diagnostics;
    using Prateek.Runtime.CommandFramework.EmitterReceiver.Interfaces;
    using Prateek.Runtime.Core.Extensions;
    using UnityEngine.Assertions;

    /// <summary>
    ///     Base class for all the commands
    /// </summary>
    [DebuggerDisplay("{GetType().Name}, Sender: {emitter.Owner.Name}")]
    public abstract class Command
    {
        #region Fields
        private ICommandEmitter emitter;
        #endregion

        #region Properties
        public ICommandEmitter Emitter { get { return emitter; } set { emitter = value; } }

        //We allow notice type spoofing for Children commands ids
        public virtual CommandId CommandId { get { return GetType(); } }
        #endregion

        #region Constructors
        #endregion

        #region Class Methods
        internal static T Create<T>() where T : Command, new()
        {
            return new T();
        }

        public override string ToString()
        {
            return GetType().ToDebugString();
        }
        #endregion
    }

    public static class CommandHelper
    {
        #region Class Methods
        public static T Create<T>()
            where T : Command, new()
        {
            Assert.IsFalse(typeof(T).IsSubclassOf(typeof(RequestCommand)));

            return Command.Create<T>();
        }

        public static TRequest Create<TRequest, TResponse>()
            where TRequest : RequestCommand, new()
            where TResponse : ResponseCommand, new()
        {
            return RequestCommand.Create<TRequest, TResponse>();
        }
        #endregion
    }
}
