namespace Prateek.A_TODO.Runtime.CommandFramework.Commands.Core
{
    using System;
    using System.Diagnostics;
    using System.Reflection;
    using System.Text;
    using Prateek.A_TODO.Runtime.CommandFramework.EmitterReceiver.Interfaces;
    using Prateek.A_TODO.Runtime.CommandFramework.Servants;
    using Prateek.Runtime.Core.Extensions;
    using UnityEngine.Assertions;

    /// <summary>
    /// Base class for all the commands
    /// </summary>
    [DebuggerDisplay("{GetType().Name}, Sender: {emitter.Owner.Name}")]
    public abstract class Command
    {
        #region Fields
        private ICommandEmitter emitter;
        #endregion

        #region Properties
        public ICommandEmitter Emitter
        {
            get { return emitter; }
            set { emitter = value; }
        }

        //We allow notice type spoofing for Children commands ids
        public virtual CommandId CommandId
        {
            get { return GetType(); }
        }
        #endregion

        #region Constructors
        protected Command()
        {
            //todo: TraceHelper.EnsureTrace<Notice>("Create");
        }
        #endregion

        internal static T Create<T>() where T : Command, new()
        {
            return new T();
        }

        #region Class Methods
        public override string ToString()
        {
            return GetType().ToDebugString();
        }
        #endregion
    }

    public static class CommandHelper
    {
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
    }
}
