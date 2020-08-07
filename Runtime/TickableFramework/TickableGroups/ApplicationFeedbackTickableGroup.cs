namespace Prateek.Runtime.TickableFramework.TickableGroups
{
    using System;
    using Prateek.Runtime.TickableFramework.Interfaces;

    internal class ApplicationFeedbackTickableGroup : TickableGroup<IApplicationFeedbackTickable>
    {
        #region State enum
        private enum State
        {
            None,
            ApplicationQuit,
            ApplicationFocus,
            ApplicationPause,
        }
        #endregion

        #region Fields
        private State state;
        private bool status;
        #endregion

        #region Properties
        public override bool InjectAtTheEnd
        {
            get { return false; }
        }
        #endregion

        #region Unity Application Methods
        public void OnApplicationPause(bool pauseStatus)
        {
            state = State.ApplicationPause;
            status = pauseStatus;
            Tick();
            state = State.None;
        }
        #endregion

        #region Class Methods
        public override void Tick(IApplicationFeedbackTickable tickable)
        {
            switch (state)
            {
                case State.ApplicationQuit:
                {
                    tickable.ApplicationQuit();
                    break;
                }
                case State.ApplicationFocus:
                {
                    tickable.ApplicationFocus(status);
                    break;
                }
                case State.ApplicationPause:
                {
                    tickable.ApplicationPause(status);
                    break;
                }
                default:
                {
                    throw new NotImplementedException();
                }
            }
        }

        public void ApplicationQuit()
        {
            state = State.ApplicationQuit;
            Tick();
            state = State.None;
        }

        public void ApplicationFocus(bool focusStatus)
        {
            state = State.ApplicationFocus;
            status = focusStatus;
            Tick();
            state = State.None;
        }
        #endregion
    }
}
