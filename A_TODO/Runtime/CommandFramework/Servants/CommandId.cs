namespace Prateek.A_TODO.Runtime.CommandFramework.Servants {
    using System;
    using Prateek.A_TODO.Runtime.CommandFramework.Commands.Core;
    using Prateek.A_TODO.Runtime.CommandFramework.EmitterReceiver;

    internal struct CommandId
    {
        private long Id;
        private Type type;
        private CommandReceiver commandReceiver;

        public Type Type
        {
            get { return type; }
        }

        public CommandId(Type type, CommandReceiver commandReceiver)
        {
            this.type = type;
            this.commandReceiver = commandReceiver;
            Id = 0;
        }

        public static implicit operator CommandId(Type type)
        {
            return new CommandId(type, null);
        }

        public static implicit operator CommandId(long Id)
        {
            return new CommandId {Id = Id};
        }

        public long GetValidId()
        {
            if (Id != 0)
            {
                return Id;
            }

            if (type.IsSubclassOf(typeof(ResponseCommand)))
            {
                Id = Command.ConvertToId(type, commandReceiver);
            }
            else
            {
                Id = Command.ConvertToId(type);
            }

            return Id;
        }
    }
}