namespace Prateek.Runtime.CommandFramework.Commands.Core
{
    using System.Diagnostics;
    using Prateek.Runtime.CommandFramework.Gadgets;
    using Prateek.Runtime.Core.Extensions;

    /// <summary>
    ///     Base class for all the commands
    /// </summary>
    [DebuggerDisplay("{GetType().Name}, Sender: {emitter.Owner.Name}")]
    public abstract class Command
    {
        #region Fields
        private CommandTools.IEmitter emitter;
        #endregion

        #region Properties
        public CommandTools.IEmitter Emitter { get { return emitter; } set { emitter = value; } }

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
}
