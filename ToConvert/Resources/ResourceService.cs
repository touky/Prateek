namespace Assets.Prateek.ToConvert.Resources
{
    using System.Collections.Generic;
    using Assets.Prateek.ToConvert.Helpers.Regexp;
    using Assets.Prateek.ToConvert.Resources.Enums;
    using Assets.Prateek.ToConvert.Resources.Loader;
    using Assets.Prateek.ToConvert.Resources.Messages;
    using Assets.Prateek.ToConvert.Resources.ResourceTree;
    using Assets.Prateek.ToConvert.Service;
    using Assets.Prateek.ToConvert.StateMachines;
    using Assets.Prateek.ToConvert.StateMachines.Interfaces;

    public sealed class ResourceService : ServiceCommunicatorBehaviour<ResourceService, ResourceServiceProvider>,
                                          ISequentialStateMachineOwner<ServiceState>
    {
        #region Fields
        private SequentialStateMachine<ServiceState> stateMachine;
        private ResourceTree<ResourceLoader> resourceTree = new ResourceTree<ResourceLoader>(RegexHelper.FolderSplit);
        private List<RequestCallbackOnChange> resourceUpdateCallbacks = new List<RequestCallbackOnChange>();
        #endregion

        #region Unity Methods
        protected override void Awake()
        {
            base.Awake();

            stateMachine = new SequentialStateMachine<ServiceState>(this);
        }

        protected override void Update()
        {
            base.Update();

            stateMachine.Advance();
        }
        #endregion

        #region Class Methods
        public void Store(ResourceLoader loader)
        {
            resourceTree.Store(loader);
        }

        public void Trigger(SequentialTriggerType trigger)
        {
            stateMachine.Trigger(SequentialTriggerType.PreventStateChange);
        }

        public void StateChange(ServiceState previousState, ServiceState nextState) { }

        public void ExecuteState(ServiceState state)
        {
            switch (state)
            {
                case ServiceState.SendCallback:
                {
                    foreach (var callback in resourceUpdateCallbacks)
                    {
                        var message = callback.GetResponse() as ResourcesHaveChangedResponse;

                        resourceTree.RetrieveResources(callback, message);

                        Communicator.Send(message);
                    }

                    break;
                }
                default:
                {
                    var provider = GetFirstValidProvider();
                    if (provider == null)
                    {
                        break;
                    }

                    provider.ExecuteState(this, state);
                    break;
                }
            }
        }

        public void OnTrigger(SequentialTriggerType trigger, bool hasTriggered) { }

        public bool Compare(ServiceState state0, ServiceState state1)
        {
            return state0 == state1;
        }

        public bool Compare(SequentialTriggerType trigger0, SequentialTriggerType trigger1)
        {
            return trigger0 == trigger1;
        }
        #endregion

        #region Communicator
        public override void MessageReceived()
        {
            //Empty
        }

        protected override void SetupCommunicatorCallback()
        {
            Communicator.AddCallback<RequestCallbackOnChange>(OnResourceUpdateCallback);
        }

        private void OnResourceUpdateCallback(RequestCallbackOnChange message)
        {
            var foundIndex = resourceUpdateCallbacks.FindIndex(x =>
            {
                return x.GetType() == message.GetType();
            });

            if (foundIndex < 0)
            {
                resourceUpdateCallbacks.Add(message);
            }
        }

        protected override void OnAwake() { }
        #endregion
    }
}
