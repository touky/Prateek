namespace Prateek.Runtime.StateMachineFramework.EnumStateMachines
{
    using System;
    using System.Collections.Generic;
    using ImGuiNET;
    using Prateek.Runtime.DebugFramework.DebugMenu;
    using Prateek.Runtime.DebugFramework.DebugMenu.Documents;
    using Prateek.Runtime.DebugFramework.DebugMenu.Gadgets;
    using Prateek.Runtime.DebugFramework.Reflection;
    using Prateek.Runtime.StateMachineFramework.Interfaces;

    public class EnumTriggerMachineSection<TOwner, TStateMachine, TState, TTrigger>
        : EnumStateMachineSection<TOwner, TStateMachine, TState, TTrigger>
        where TOwner : class, DebugMenu.IDebugMenuOwner, IEnumStepMachineOwner<TState>
        where TStateMachine : IStateMachine<TState, TTrigger>
        where TState : struct, IConvertible
        where TTrigger : struct, IConvertible
    {
        #region Fields
        private DebugField<Dictionary<TState, Dictionary<TTrigger, TState>>> connections = string.Empty;
        #endregion

        #region Constructors
        public EnumTriggerMachineSection(string stateMachineFieldName = STATEMACHINE_FIELD)
            : base(stateMachineFieldName) { }
        #endregion

        #region Class Methods
        protected override void ManualInit()
        {
            base.ManualInit();

            connections = "connections";
        }

        protected override void OnDraw(DebugMenuContext context)
        {
            base.OnDraw(context);

            DrawConnections();
        }

        protected void DrawConnections()
        {
            if (!connections.AssertDrawable())
            {
                connections.SetOwner(stateMachine.Value);
                return;
            }

            if (ImGui.CollapsingHeader("Connections:"))
            {
                using (new ScopeIndent())
                {
                    foreach (var connectionPair in connections.Value)
                    {
                        ImGui.Text($"- {connectionPair.Key.ToString()}");
                        using (new ScopeIndent())
                        {
                            foreach (var pair in connectionPair.Value)
                            {
                                ImGui.Text($"-> {pair.Key.ToString()} > {pair.Value.ToString()}");
                            }
                        }
                    }
                }
            }
        }
        #endregion
    }
}
