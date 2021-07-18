namespace Prateek.Runtime.AppContentFramework.Daemons
{
    using Prateek.Runtime.AppContentFramework.ContentLoaders;
    using Prateek.Runtime.Core;
    using Prateek.Runtime.Core.Helpers;
    using Prateek.Runtime.Core.Interfaces.IPriority;
    using Prateek.Runtime.DaemonFramework.Servants;
    using Prateek.Runtime.DebugFramework.DebugMenu;
    using Prateek.Runtime.DebugFramework.DebugMenu.Documents;
    using Prateek.Runtime.DebugFramework.DebugMenu.Gadgets;
    using Prateek.Runtime.GadgetFramework;
    using Prateek.Runtime.GadgetFramework.Interfaces;
    using Prateek.Runtime.StateMachineFramework;
    using Prateek.Runtime.StateMachineFramework.DelegateStateMachines;
    using Prateek.Runtime.StateMachineFramework.Interfaces;
    using Prateek.Runtime.TickableFramework.Interfaces;
    using UnityEngine.Assertions;

    public abstract class ContentRegistryServant
        : TickableServant<ContentRegistryDaemon, ContentRegistryServant>
        , StateMachine.IDelegateOwner<ContentRegistryServant.InternalMachine, ContentRegistryServant.Trigger>
        , IEarlyUpdateTickable
        , DebugMenu.IDocumentServant
    {
        public enum WorkStatus
        {
            Nothing,
            Pending,
            Working,
            Done,
        }

        #region Fields
        protected WorkStatus workStatus = WorkStatus.Nothing;
        private DiffList<string> paths;
        #endregion

        #region Class Methods
        public override void Startup()
        {
            base.Startup();

            //todo fix
            (this as DebugMenu.IDocumentServant).SetupDebugDocument();
        }

        protected abstract ContentLoader GetNewContentLoader(string path);

        protected void WorkIsReady()
        {
            Assert.IsTrue(StateMachine.ActiveState == ((InternalMachine.StateDelegate) Idle).Method);

            StateMachine.Trigger(Trigger.NextStep);
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

            StateMachine.Trigger(Trigger.NextStep);
        }
        #endregion

        #region IDebugMenuDocumentServant Members
        void DebugMenu.IDocumentServant.SetupDebugDocument()
        {
            SetupDebugDocument(Overseer.Get<DebugMenuDocument>());
        }

        public virtual void SetupDebugDocument(DebugMenuDocument document) { }
        #endregion

        #region IEarlyUpdateTickable Members
        public virtual void EarlyUpdate()
        {
            StateMachine.Step();
        }

        public virtual int Priority(IPriority<IEarlyUpdateTickable> type)
        {
            return DefaultPriority;
        }
        #endregion

        #region IEnumStepMachineOwner<State> Members
        public IGadgetPouch GadgetPouch { get; private set; }
        public InternalMachine StateMachine { get; private set; }

        StateMachine.IMachine StateMachine.IOwner.CreateStateMachine()
        {
            return new InternalMachine();
        }

        void StateMachine.IOwner.Setup(StateMachine.IMachine stateMachine)
        {
            StateMachine
                .Connect(Startup, Trigger.NextStep, Idle)
                .Connect(Idle, Trigger.NextStep, StartWork)
                .Connect(StartWork, Trigger.NextStep, Working)
                .Connect(Working, Trigger.NextStep, ContentTriage)
                .Connect(ContentTriage, Trigger.NextStep, Idle)
                .Init(Startup);
        }

        protected virtual void Startup(StateStatus stateStatus)
        {
            if (stateStatus != StateStatus.Execute)
            {
                return;
            }

            StateMachine.Trigger(Trigger.NextStep);
        }

        protected void Idle(StateStatus stateStatus)
        {
            if (stateStatus != StateStatus.Execute)
            {
                return;
            }

            if (workStatus == WorkStatus.Pending)
            {
                WorkIsReady();
            }
        }

        protected virtual void StartWork(StateStatus stateStatus)
        {
            if (stateStatus != StateStatus.Execute)
            {
                return;
            }

            InvalidateAllPaths();

            workStatus = WorkStatus.Working;
            StateMachine.Trigger(Trigger.NextStep);
        }

        protected virtual void Working(StateStatus stateStatus)
        {
            if (stateStatus != StateStatus.Execute)
            {
                return;
            }

            if (workStatus == WorkStatus.Done)
            {
                StateMachine.Trigger(Trigger.NextStep);
            }
        }

        protected void ContentTriage(StateStatus stateStatus)
        {
            if (stateStatus != StateStatus.Execute)
            {
                return;
            }

            DoContentTriage();

            StateMachine.Trigger(Trigger.NextStep);
        }
        #endregion

        #region Nested type: EnumComparer
        public class EnumComparer : IEnumComparer<Trigger>
        {
            #region IEnumComparer<State,Trigger> Members
            public EnumComparer() { }

            public bool Compare(Trigger enum0, Trigger enum1)
            {
                return enum0 == enum1;
            }
            #endregion
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

        #region Trigger enum
        public enum Trigger
        {
            NextStep,
        }
        #endregion
    }
}
