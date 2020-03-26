namespace Assets.Prateek.ToConvert.StateMachines.FSM.BoolTrigger
{
    public class BoolTriggerTransition : FiniteTransition<bool>
    {
        #region Constructors
        #endregion

        #region Class Methods
        public override void TryTriggering(bool trigger)
        {
            if (trigger)
            {
                Trigger();
            }
        }
        #endregion
    }
}
