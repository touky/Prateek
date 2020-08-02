namespace Prateek.A_TODO.Runtime.AppContentFramework.Daemons
{
    using System.Collections.Generic;
    using Prateek.A_TODO.Runtime.AppContentFramework.Enums;
    using Prateek.A_TODO.Runtime.AppContentFramework.Loader;
    using Prateek.A_TODO.Runtime.AppContentFramework.Messages;
    using Prateek.A_TODO.Runtime.AppContentFramework.ResourceTree;
    using Prateek.A_TODO.Runtime.CommandFramework.Tools;
    using Prateek.A_TODO.Runtime.StateMachines.Interfaces;
    using Prateek.A_TODO.Runtime.StateMachines.SimpleStateMachine;
    using Prateek.Runtime.TickableFramework.Enums;

    public sealed class ContentRegistryDaemon
        : CommandReceiverDaemon<ContentRegistryDaemon, ContentRegistryServant>
        , ISimpleStepMachineOwner<ServiceState>
    {
        #region Fields
        private SimpleStepMachine<ServiceState> stateMachine;
        private ResourceTree<ContentLoader> resourceTree = new ResourceTree<ContentLoader>(RegexHelper.FolderSplit);
        private HashSet<RequestAccessToContent> resourceUpdateCallbacks = new HashSet<RequestAccessToContent>();
        private HashSet<RequestAccessToContent> pendingCallbacks = new HashSet<RequestAccessToContent>();
        #endregion
        public override TickableSetup TickableSetup
        {
            get { return TickableSetup.UpdateBegin; }
        }


        protected override void Awake()
        {
            base.Awake();

            this.stateMachine = new SimpleStepMachine<ServiceState>(this);
        }

        public override void Tick(TickableFrame tickableFrame, float seconds, float unscaledSeconds)
        {
            base.Tick(tickableFrame, seconds, unscaledSeconds);

            this.stateMachine.Advance();
        }

        #region Class Methods
        public void Store(ContentLoader loader)
        {
            this.resourceTree.Store(loader);
        }

        public void Trigger(SimpleStepTrigger trigger)
        {
            this.stateMachine.Trigger(SimpleStepTrigger.PreventStateChange);
        }
        #endregion

        public void OnStateChange(ServiceState previousState, ServiceState nextState) { }

        public void OnStateExecute(ServiceState state)
        {
            switch (state)
            {
                case ServiceState.SendCallback:
                {
                    foreach (RequestAccessToContent callback in pendingCallbacks)
                    {
                        ResourcesHaveChangedResponse message = callback.GetResponse() as ResourcesHaveChangedResponse;

                        resourceTree.RetrieveResources(callback, message);

                        CommandReceiver.Send(message);
                    }

                    pendingCallbacks.Clear();


                    break;
                }
                default:
                {
                    ContentRegistryServant servant = FirstAliveServant;
                    if (servant == null)
                    {
                        break;
                    }

                    servant.ExecuteState(this, state);
                    break;
                }
            }
        }

        public void OnTrigger(SimpleStepTrigger trigger, bool hasTriggered)
        {
        }

        public bool Compare(ServiceState state0, ServiceState state1)
        {
            return state0 == state1;
        }

        public bool Compare(SimpleStepTrigger trigger0, SimpleStepTrigger trigger1)
        {
            return trigger0 == trigger1;
        }

        protected override void SetupCommandReceiverCallback()
        {
            CommandReceiver.AddCallback<RequestAccessToContent>(OnResourceUpdateCallback);
        }

        private void OnResourceUpdateCallback(RequestAccessToContent notice)
        {
            if (!resourceUpdateCallbacks.Contains(notice))
            {
                resourceUpdateCallbacks.Add(notice);
            }

            if (!pendingCallbacks.Contains(notice))
            {
                if (!stateMachine.IsRunning)
                {
                    stateMachine.Trigger(SimpleStepTrigger.ForceNextState, ServiceState.SendCallback);
                }

                pendingCallbacks.Add(notice);
            }
        }

        protected override void OnAwake() { }
    }
}
