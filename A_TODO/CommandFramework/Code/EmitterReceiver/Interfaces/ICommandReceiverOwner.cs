namespace Prateek.CommandFramework.TransmitterReceiver
{
    using UnityEngine;

    public interface ICommandReceiverOwner
    {
        #region Properties
        ICommandReceiver CommandReceiver { get; }
        string Name { get; }
        #endregion
    }
}
