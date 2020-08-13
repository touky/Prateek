namespace Prateek.Runtime.AppContentFramework.Daemons
{
    using System.Collections.Generic;
    using Prateek.Runtime.AppContentFramework.Loader;
    using Prateek.Runtime.AppContentFramework.Messages;
    using Prateek.Runtime.CommandFramework;
    using Prateek.Runtime.CommandFramework.Debug;
    using Prateek.Runtime.CommandFramework.EmitterReceiver.Interfaces;
    using Prateek.Runtime.Core.Consts;
    using Prateek.Runtime.Core.HierarchicalTree;
    using Prateek.Runtime.Core.HierarchicalTree.Interfaces;
    using Prateek.Runtime.DebugFramework.DebugMenu;
    using Prateek.Runtime.DebugFramework.DebugMenu.Interfaces;
    using Prateek.Runtime.GadgetFramework;
    using Prateek.Runtime.StateMachineFramework.EnumStateMachines;
    using Prateek.Runtime.StateMachineFramework.Interfaces;
    using Prateek.Runtime.TickableFramework.Interfaces;
    using UnityEngine;

    public sealed class ContentRegistryDaemon
        : ReceiverDaemonOverseer<ContentRegistryDaemon, ContentRegistryServant>
        , IEnumStepMachineOwner<ContentRegistryDaemon.State>
        , IPreUpdateTickable
        , IDebugMenuDocumentOwner
    {
        #region State enum
        public enum State
        {
            Startup,
            Idle,
            WaitForTimeout,
            SendAccessResponses
        }
        #endregion

        #region Trigger enum
        private enum Trigger
        {
            NextStep
        }
        #endregion

        #region Static and Constants
        private const float TIMEOUT = 1;
        #endregion

        #region Fields
        private StateMachine stateMachine;
        private HierarchicalTree<ContentLoader> hierarchicalTree = new HierarchicalTree<ContentLoader>();

        private HashSet<ContentAccessRequest> contentAccessRequests = new HashSet<ContentAccessRequest>();

        private bool requestReceived = false;
        private float timeOut = Const.TIMEOUT_RESET;
        #endregion

        #region Unity Methods
        protected override void Awake()
        {
            base.Awake();

            hierarchicalTree.Setup();

            InitStateMachine();
        }
        #endregion

        #region Register/Unregister
        protected override void OnAwake()
        {
            base.OnAwake();
        }
        #endregion

        #region Class Methods
        private void InitStateMachine()
        {
            stateMachine = new StateMachine(this, State.Startup);
            stateMachine
                .Connect(State.Startup, Trigger.NextStep, State.Idle)
                .Connect(State.Idle, Trigger.NextStep, State.WaitForTimeout)
                .Connect(State.WaitForTimeout, Trigger.NextStep, State.SendAccessResponses)
                .Connect(State.SendAccessResponses, Trigger.NextStep, State.Idle);
        }

        internal void Store(ContentLoader loader)
        {
            hierarchicalTree.Store(loader);
        }

        internal void Remove(string path)
        {
            hierarchicalTree.Remove(new RemovalLeaf {Path = path});
        }

        public override void DefineReceptionActions(ICommandReceiver receiver)
        {
            receiver.SetActionFor<ContentAccessRequest>(OnContentAccessRequest);
        }

        private void OnContentAccessRequest(ContentAccessRequest request)
        {
            requestReceived = true;
            timeOut = TIMEOUT;
            contentAccessRequests.Add(request);
        }
        #endregion

        #region IEnumStepMachineOwner<State> Members
        public void ChangingState(State endingState, State beginningState)
        {
            switch (beginningState)
            {
                case State.WaitForTimeout:
                {
                    timeOut = TIMEOUT;
                    break;
                }
            }
        }

        public void ExecutingState(State state)
        {
            switch (state)
            {
                case State.Startup:
                {
                    stateMachine.Trigger(Trigger.NextStep);
                    break;
                }
                case State.Idle:
                {
                    if (requestReceived)
                    {
                        requestReceived = false;
                        stateMachine.Trigger(Trigger.NextStep);
                    }

                    break;
                }
                case State.WaitForTimeout:
                {
                    timeOut -= Time.unscaledDeltaTime;
                    if (timeOut < 0)
                    {
                        stateMachine.Trigger(Trigger.NextStep);
                    }

                    break;
                }
                case State.SendAccessResponses:
                {
                    foreach (var accessRequest in contentAccessRequests)
                    {
                        var response = accessRequest.GetResponse<ContentAccessChangedResponse>();
                        hierarchicalTree.SearchTree(accessRequest, response);
                        this.Get<ICommandReceiver>().Send(response);
                    }

                    stateMachine.Trigger(Trigger.NextStep);
                    break;
                }
            }
        }
        #endregion

        #region IPreUpdateTickable Members
        public void PreUpdate()
        {
            stateMachine.Step();
        }

        public void SetupDebugDocument(DebugMenuDocument document, out string title)
        {
            title = "Content Registry";

            var section = new ContentRegistrySection("Content in registry");

            document.AddSections(section);
        }
        #endregion

        #region Nested type: EnumComparer
        private class EnumComparer : IEnumComparer<State, Trigger>
        {
            #region IEnumComparer<State,Trigger> Members
            public bool Compare(State state0, State state1)
            {
                return state0 == state1;
            }

            public bool Compare(Trigger trigger0, Trigger trigger1)
            {
                return trigger0 == trigger1;
            }
            #endregion
        }
        #endregion

        #region Nested type: RemovalLeaf
        private struct RemovalLeaf : IHierarchicalTreeLeaf
        {
            public string Path { get; set; }
        }
        #endregion

        #region Nested type: StateMachine
        private class StateMachine : EnumTriggerMachine<State, Trigger, EnumComparer>
        {
            #region Constructors
            public StateMachine(IEnumStateMachineOwner<State> owner, State startState) : base(owner, startState) { }
            #endregion
        }
        #endregion
    }
}
