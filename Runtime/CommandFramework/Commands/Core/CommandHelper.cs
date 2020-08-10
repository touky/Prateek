namespace Prateek.Runtime.CommandFramework.Commands.Core
{
    using UnityEngine.Assertions;

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
