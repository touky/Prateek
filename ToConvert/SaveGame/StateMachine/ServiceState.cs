namespace Assets.Prateek.ToConvert.SaveGame.StateMachine
{
    using Assets.Prateek.ToConvert.SaveGame.Enums;
    using Assets.Prateek.ToConvert.StateMachines.FSM.Common;

    internal abstract class ServiceState : ServiceState<SaveState, SaveService>
    {
        #region Constructors
        protected ServiceState(SaveService service) : base(service) { }
        #endregion
    }
}
