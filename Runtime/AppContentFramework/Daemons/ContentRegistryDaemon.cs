namespace Prateek.Runtime.AppContentFramework.Daemons
{
    using System.Collections.Generic;
    using Prateek.Runtime.AppContentFramework.ContentLoaders;
    using Prateek.Runtime.AppContentFramework.Daemons.Debug;
    using Prateek.Runtime.AppContentFramework.Messages;
    using Prateek.Runtime.CommandFramework.Gadgets;
    using Prateek.Runtime.Core.Consts;
    using Prateek.Runtime.Core.HierarchicalTree;
    using Prateek.Runtime.Core.HierarchicalTree.Interfaces;
    using Prateek.Runtime.Core.Interfaces.IPriority;
    using Prateek.Runtime.DaemonFramework;
    using Prateek.Runtime.DebugFramework.DebugMenu;
    using Prateek.Runtime.DebugFramework.DebugMenu.Documents;
    using Prateek.Runtime.DebugFramework.DebugMenu.Gadgets;
    using Prateek.Runtime.GadgetFramework;
    using Prateek.Runtime.StateMachineFramework;
    using Prateek.Runtime.StateMachineFramework.DelegateStateMachines;
    using Prateek.Runtime.StateMachineFramework.Interfaces;
    using Prateek.Runtime.TickableFramework.Interfaces;
    using UnityEngine;

    public sealed class ContentRegistryDaemon
        : DaemonOverseer<ContentRegistryDaemon, ContentRegistryServant>
        , StateMachine.IDelegateOwner<ContentRegistryDaemon.InternalMachine, ContentRegistryDaemon.Trigger>
        , CommandTools.IReceiverOwner
        , IPreUpdateTickable
        , DebugMenu.IDocumentOwner
    {
        #region Trigger enum
        public enum Trigger
        {
            NextStep
        }
        #endregion

        #region Static and Constants
        private const float TIMEOUT = 1;
        #endregion

        #region Fields
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
        }
        #endregion

        #region Register/Unregister
        protected override void OnAwake()
        {
            base.OnAwake();
        }
        #endregion

        #region Class Methods
        internal void Store(ContentLoader loader)
        {
            hierarchicalTree.Store(loader);
        }

        internal void Remove(string path)
        {
            hierarchicalTree.Remove(new RemovalLeaf {Path = path});
        }

        private void OnContentAccessRequest(ContentAccessRequest request)
        {
            requestReceived = true;
            timeOut = TIMEOUT;
            contentAccessRequests.Add(request);
        }
        #endregion

        #region ICommandReceiverOwner Members
        public CommandTools.IReceiver Receiver { get; private set; }

        public void DefineReceptionActions(CommandTools.IReceiver receiver)
        {
            receiver.SetActionFor<ContentAccessRequest>(OnContentAccessRequest);
        }
        #endregion

        #region IDebugMenuDocumentOwner Members
        public void SetupDebugDocument(DebugMenuDocument document, out string title)
        {
            title = "Content Registry";

            var servants = new DaemonOverseerSection<ContentRegistryDaemon, ContentRegistryServant>();
            var machine = new DelegateTriggerMachineSection<ContentRegistryDaemon, InternalMachine, Trigger, EnumComparer>();
            var content = new ContentRegistrySection();

            document.AddSections(servants);
            document.AddSections(machine);
            document.AddSections(content);
        }
        #endregion

        #region IEnumStepMachineOwner<State> Members
        public InternalMachine StateMachine { get; private set; }

        StateMachine.IMachine StateMachine.IOwner.CreateStateMachine()
        {
            return new InternalMachine();
        }

        void StateMachine.IOwner.Setup(StateMachine.IMachine stateMachine)
        {
            StateMachine
                .Connect(Startup, Trigger.NextStep, Idle)
                .Connect(Idle, Trigger.NextStep, WaitForTimeout)
                .Connect(WaitForTimeout, Trigger.NextStep, SendAccessResponses)
                .Connect(SendAccessResponses, Trigger.NextStep, Idle)
                .Init(Startup);
        }

        private void Startup(StateStatus stateStatus)
        {
            if (stateStatus != StateStatus.Execute)
            {
                return;
            }

            StateMachine.Trigger(Trigger.NextStep);
        }

        private void Idle(StateStatus stateStatus)
        {
            if (stateStatus != StateStatus.Execute)
            {
                return;
            }

            if (requestReceived)
            {
                requestReceived = false;
                StateMachine.Trigger(Trigger.NextStep);
            }

        }

        private void WaitForTimeout(StateStatus stateStatus)
        {
            switch (stateStatus)
            {
                case StateStatus.Begin:
                {
                    timeOut = TIMEOUT;
                    break;
                }
                case StateStatus.Execute:
                {
                    timeOut -= Time.unscaledDeltaTime;
                    if (timeOut < 0)
                    {
                        StateMachine.Trigger(Trigger.NextStep);
                    }
                    break;
                }
            }
        }

        private void SendAccessResponses(StateStatus stateStatus)
        {
            if (stateStatus != StateStatus.Execute)
            {
                return;
            }

            foreach (var accessRequest in contentAccessRequests)
            {
                var response = accessRequest.GetResponse<ContentAccessChangedResponse>();
                hierarchicalTree.SearchTree(accessRequest, response);
                this.Get<CommandTools.IReceiver>().Send(response);
            }

            StateMachine.Trigger(Trigger.NextStep);
        }
        #endregion

        #region IPreUpdateTickable Members
        public void PreUpdate()
        {
            Receiver.ProcessReceivedCommands();

            StateMachine.Step();
        }

        public int Priority(IPriority<IPreUpdateTickable> type)
        {
            return DefaultPriority;
        }
        #endregion

        #region Nested type: EnumComparer
        public class EnumComparer
            : IEnumComparer<Trigger>
        {
            #region IEnumComparer<State,Trigger> Members
            public bool Compare(Trigger enum0, Trigger enum1)
            {
                return enum0 == enum1;
            }
            #endregion
        }
        #endregion

        #region Nested type: RemovalLeaf
        private struct RemovalLeaf
            : IHierarchicalTreeLeaf
        {
            public string Path { get; set; }
        }
        #endregion

        #region Nested type: StateMachine
        public class InternalMachine
            : DelegateTriggerMachine<Trigger, EnumComparer>
        {
            #region Constructors
            public InternalMachine()
                : base() { }
            #endregion
        }
        #endregion
    }
}
