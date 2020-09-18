namespace Prateek.Runtime.CommandFramework.EmitterReceiver.Interfaces
{
    using Prateek.Runtime.CommandFramework.Commands.Core;

    public interface ICommandActionProxy
    {
        #region Class Methods
        void Invoke(Command command);
        #endregion
    }
}
