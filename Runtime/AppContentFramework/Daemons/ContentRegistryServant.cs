namespace Prateek.Runtime.AppContentFramework.Daemons
{
    using Prateek.Runtime.AppContentFramework.ContentLoaders;
    using Prateek.Runtime.Core;
    using Prateek.Runtime.Core.Interfaces.IPriority;
    using Prateek.Runtime.DaemonFramework.Servants;
    using Prateek.Runtime.DebugFramework.DebugMenu;
    using Prateek.Runtime.DebugFramework.DebugMenu.Interfaces;
    using Prateek.Runtime.GadgetFramework;
    using Prateek.Runtime.StateMachineFramework.EnumStateMachines;
    using Prateek.Runtime.StateMachineFramework.Interfaces;
    using Prateek.Runtime.TickableFramework.Interfaces;
    using UnityEngine.Assertions;

    public abstract class ContentRegistryServant
        : TickableServant<ContentRegistryDaemon, ContentRegistryServant>
        , IEnumStepMachineOwner<ContentRegistryServant.State>
        , IEarlyUpdateTickable
        , IDebugMenuDocumentServant
    {
        #region State enum
        public enum State
        {
            Startup,
            Idle,
            StartWork,
            Working,
            ContentTriage,
        }
        #endregion

        public enum WorkStatus
        {
            Nothing,
            Pending,
            Working,
            Done,
        }

        #region Trigger enum
        protected enum Trigger
        {
            NextStep,
        }
        #endregion

        #region Fields
        private StateMachine stateMachine;

        protected WorkStatus workStatus = WorkStatus.Nothing;
        private DiffList<string> paths;
        #endregion

        #region Class Methods
        public override void Startup()
        {
            base.Startup();

            InitStateMachine();

            (this as IDebugMenuDocumentServant).SetupDebugDocument();
        }

        private void InitStateMachine()
        {
            stateMachine = new StateMachine(this, State.Startup);
            stateMachine
                .Connect(State.Startup, Trigger.NextStep, State.Idle)
                .Connect(State.Idle, Trigger.NextStep, State.StartWork)
                .Connect(State.StartWork, Trigger.NextStep, State.Working)
                .Connect(State.Working, Trigger.NextStep, State.ContentTriage)
                .Connect(State.ContentTriage, Trigger.NextStep, State.Idle);
            stateMachine.Reboot();
        }

        protected abstract ContentLoader GetNewContentLoader(string path);

        protected void WorkIsReady()
        {
            Assert.IsTrue(stateMachine.ActiveState == State.Idle);

            stateMachine.Trigger(Trigger.NextStep);
        }

        protected void InvalidatePath(string path)
        {
            paths.Remove(path);
        }

        protected void InvalidateAllPaths()
        {
            paths.Clear();
        }

        protected void ValidatePath(string path)
        {
            paths.Add(path);
        }

        private void DoContentTriage()
        {
            if (paths.Removed != null)
            {
                foreach (var removed in paths.Removed)
                {
                    Overseer.Remove(removed);
                }
            }

            if (paths.Added != null)
            {
                foreach (var added in paths.Added)
                {
                    var contentLoader = GetNewContentLoader(added);
                    if (contentLoader == null)
                    {
                        UnityEngine.Debug.LogError($"Path '{added}' has a null ContentLoader.");
                        continue;
                    }

                    Overseer.Store(contentLoader);
                }
            }

            paths.FlushDiff();

            stateMachine.Trigger(Trigger.NextStep);
        }
        #endregion

        #region IDebugMenuDocumentServant Members
        void IDebugMenuDocumentServant.SetupDebugDocument()
        {
            SetupDebugDocument(Overseer.Get<DebugMenuDocument>());
        }

        public virtual void SetupDebugDocument(DebugMenuDocument document) { }
        #endregion

        #region IEarlyUpdateTickable Members
        public void EarlyUpdate()
        {
            stateMachine.Step();
        }

        public int Priority(IPriority<IEarlyUpdateTickable> type)
        {
            return DefaultPriority;
        }
        #endregion

        #region IEnumStepMachineOwner<State> Members
        public virtual void ChangingState(State endingState, State beginningState) { }

        public virtual void ExecutingState(State state)
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
                    if (workStatus == WorkStatus.Pending)
                    {
                        WorkIsReady();
                    }

                    break;
                }
                case State.StartWork:
                {
                    InvalidateAllPaths();

                    workStatus = WorkStatus.Working;
                    stateMachine.Trigger(Trigger.NextStep);
                    break;
                }
                case State.Working:
                {
                    if (workStatus == WorkStatus.Done)
                    {
                        stateMachine.Trigger(Trigger.NextStep);
                    }
                    break;
                }
                case State.ContentTriage:
                {
                    DoContentTriage();

                    stateMachine.Trigger(Trigger.NextStep);
                    break;
                }
                default:
                {
                    stateMachine.Trigger(Trigger.NextStep);
                    break;
                }
            }
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
