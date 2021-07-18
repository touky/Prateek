namespace Prateek.Runtime.StateMachineFramework.EnumStateMachines
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using ImGuiNET;
    using Prateek.Runtime.DebugFramework.DebugMenu;
    using Prateek.Runtime.DebugFramework.DebugMenu.Documents;
    using Prateek.Runtime.DebugFramework.DebugMenu.Gadgets;
    using Prateek.Runtime.DebugFramework.Reflection;
    using Prateek.Runtime.StateMachineFramework.Interfaces;

    public class DelegateTriggerMachineSection<TOwner, TStateMachine, TTrigger>
        : DelegateStateMachineSection<TOwner, TStateMachine, TTrigger>
        where TOwner : class, DebugMenu.IDebugMenuOwner, DelegateStateMachine.IOwner
        where TStateMachine : IStateMachine<MethodInfo, TTrigger>
        where TTrigger : struct, IConvertible
    {
        #region Fields
        private DebugField<Dictionary<MethodInfo, Dictionary<TTrigger, MethodInfo>>> connections = string.Empty;
        #endregion

        #region Constructors
        public DelegateTriggerMachineSection(string stateMachineFieldName = STATEMACHINE_FIELD)
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
