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

    public sealed class ContentRegistryDaemonCore
        : NoticeReceiverDaemonCore<ContentRegistryDaemonCore, ContentRegistryDaemonBranch>
        , ISimpleStepMachineOwner<ServiceState>
    {
        #region Fields
        private SimpleStepMachine<ServiceState> stateMachine;
        private ResourceTree<ContentLoader> resourceTree = new ResourceTree<ContentLoader>(RegexHelper.FolderSplit);
        private HashSet<RequestAccessToContent> resourceUpdateCallbacks = new HashSet<RequestAccessToContent>();
        private HashSet<RequestAccessToContent> pendingCallbacks = new HashSet<RequestAccessToContent>();
        #endregion

        #region Unity Methods
        protected override void Awake()
        {
            base.Awake();

            this.stateMachine = new SimpleStepMachine<ServiceState>(this);
        }

        protected override void Update()
        {
            base.Update();

            this.stateMachine.Advance();
        }
        #endregion

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

                        NoticeReceiver.Send(message);
                    }

                    pendingCallbacks.Clear();


                    break;
                }
                default:
                {
                    ContentRegistryDaemonBranch branch = GetFirstAliveBranch();
                    if (branch == null)
                    {
                        break;
                    }

                    branch.ExecuteState(this, state);
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

        protected override void SetupNoticeReceiverCallback()
        {
            NoticeReceiver.AddCallback<RequestAccessToContent>(OnResourceUpdateCallback);
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
