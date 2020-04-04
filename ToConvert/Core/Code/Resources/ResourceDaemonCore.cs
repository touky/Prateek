namespace Mayfair.Core.Code.Resources
{
    using System.Collections.Generic;
    using Mayfair.Core.Code.Resources.Enums;
    using Mayfair.Core.Code.Resources.Loader;
    using Mayfair.Core.Code.Resources.Messages;
    using Mayfair.Core.Code.Resources.ResourceTree;
    using Mayfair.Core.Code.StateMachines;
    using Mayfair.Core.Code.StateMachines.Interfaces;
    using Prateek.NoticeFramework.Tools;

    public sealed class ResourceDaemonCore : NoticeReceiverDaemonCore<ResourceDaemonCore, ResourceDaemonBranch>,
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

            this.stateMachine = new SequentialStateMachine<ServiceState>(this);
        }

        protected override void Update()
        {
            base.Update();

            this.stateMachine.Advance();
        }
        #endregion

        #region Class Methods
        public void Store(ResourceLoader loader)
        {
            this.resourceTree.Store(loader);
        }

        public void Trigger(SequentialTriggerType trigger)
        {
            this.stateMachine.Trigger(SequentialTriggerType.PreventStateChange);
        }
        #endregion

        #region ISequentialStateMachineOwner<ServiceState> Members
        public void StateChange(ServiceState previousState, ServiceState nextState) { }

        public void ExecuteState(ServiceState state)
        {
            switch (state)
            {
                case ServiceState.SendCallback:
                {
                    foreach (RequestCallbackOnChange callback in this.resourceUpdateCallbacks)
                    {
                        ResourcesHaveChangedResponse notice = callback.GetResponse() as ResourcesHaveChangedResponse;

                        this.resourceTree.RetrieveResources(callback, notice);

                        NoticeReceiver.Send(notice);
                    }

                    break;
                }
                default:
                {
                    ResourceDaemonBranch branch = GetFirstAliveBranch();
                    if (branch == null)
                    {
                        break;
                    }

                    branch.ExecuteState(this, state);
                    break;
                }
            }
        }

        public void OnTrigger(SequentialTriggerType trigger, bool hasTriggered)
        {
        }

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
        public override void NoticeReceived()
        {
            //Empty
        }

        protected override void SetupNoticeReceiverCallback()
        {
            NoticeReceiver.AddCallback<RequestCallbackOnChange>(OnResourceUpdateCallback);
        }

        private void OnResourceUpdateCallback(RequestCallbackOnChange notice)
        {
            int foundIndex = this.resourceUpdateCallbacks.FindIndex(x =>
            {
                return x.GetType() == notice.GetType();
            });

            if (foundIndex < 0)
            {
                this.resourceUpdateCallbacks.Add(notice);
            }
        }

        protected override void OnAwake() { }
        #endregion
    }
}
