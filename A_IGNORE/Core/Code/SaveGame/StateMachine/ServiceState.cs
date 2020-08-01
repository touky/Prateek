namespace Mayfair.Core.Code.SaveGame.StateMachine
{
    using Mayfair.Core.Code.SaveGame.Enums;
    using Mayfair.Core.Code.StateMachines.FSM.Common;

    internal abstract class ServiceState : ServiceState<SaveState, SaveDaemon>
    {
        #region Constructors
        protected ServiceState(SaveDaemon daemonCore) : base(daemonCore) { }
        #endregion
    }
}
