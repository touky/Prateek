namespace Prateek.A_TODO.Runtime.AppContentFramework.Daemons
{
    using System.Collections.Generic;
    using Prateek.A_TODO.Runtime.AppContentFramework.Enums;
    using Prateek.A_TODO.Runtime.AppContentFramework.Loader;
    using Prateek.A_TODO.Runtime.AppContentFramework.Messages;
    using Prateek.A_TODO.Runtime.CommandFramework.Tools;
    using Prateek.A_TODO.Runtime.StateMachines.Interfaces;
    using Prateek.A_TODO.Runtime.StateMachines.SimpleStateMachine;
    using Prateek.Runtime.Core.HierarchicalTree;
    using Prateek.Runtime.TickableFramework.Enums;

    public sealed class ContentRegistryDaemon
        : ReceiverDaemonOverseer<ContentRegistryDaemon, ContentRegistryServant>
        , ISimpleStepMachineOwner<ServiceState>
    {
        #region Fields
        private SimpleStepMachine<ServiceState> stateMachine;
        private HierarchicalTree<ContentLoader> hierarchicalTree = new HierarchicalTree<ContentLoader>();
        //todo private HashSet<RequestAccessToContent> resourceUpdateCallbacks = new HashSet<RequestAccessToContent>();
        //todo private HashSet<RequestAccessToContent> pendingCallbacks = new HashSet<RequestAccessToContent>();
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
            this.hierarchicalTree.Store(loader);
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
                    //todo foreach (RequestAccessToContent callback in pendingCallbacks)
                    //todo {
                    //todo     ResourcesHaveChangedResponse message = callback.GetResponse() as ResourcesHaveChangedResponse;
                    //todo 
                    //todo     resourceTree.RetrieveResources(callback, message);
                    //todo 
                    //todo     CommandReceiver.Send(message);
                    //todo }
                    //todo 
                    //todo pendingCallbacks.Clear();


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

        public override void DefineCommandReceiverActions()
        {
            //todo CommandReceiver.SetActionFor<RequestAccessToContent>(OnResourceUpdateCallback);
        }

        //todo private void OnResourceUpdateCallback(RequestAccessToContent notice)
        //todo {
        //todo     if (!resourceUpdateCallbacks.Contains(notice))
        //todo     {
        //todo         resourceUpdateCallbacks.Add(notice);
        //todo     }
        //todo 
        //todo     if (!pendingCallbacks.Contains(notice))
        //todo     {
        //todo         if (!stateMachine.IsRunning)
        //todo         {
        //todo             stateMachine.Trigger(SimpleStepTrigger.ForceNextState, ServiceState.SendCallback);
        //todo         }
        //todo 
        //todo         pendingCallbacks.Add(notice);
        //todo     }
        //todo }

        protected override void OnAwake() { }
    }
}
