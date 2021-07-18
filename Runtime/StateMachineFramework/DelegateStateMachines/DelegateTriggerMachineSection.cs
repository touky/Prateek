namespace Prateek.Runtime.StateMachineFramework.DelegateStateMachines
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

    public class DelegateTriggerMachineSection<TOwner, TStateMachine, TTrigger, TEnumComparer>
        : DelegateStateMachineSection<TOwner, TStateMachine, TTrigger, TEnumComparer>
        where TOwner : class, DebugMenu.IDebugMenuOwner, StateMachine.IOwner
        where TStateMachine : StateMachine.IStateMachine<MethodInfo, TTrigger>
        where TTrigger : struct, IConvertible
        where TEnumComparer : IEnumComparer<TTrigger>, new()
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
                        ImGui.Text($"- {connectionPair.Key.Name}");
                        using (new ScopeIndent())
                        {
                            foreach (var pair in connectionPair.Value)
                            {
                                ImGui.Text($"-> {pair.Key.ToString()} > {pair.Value.Name}");
                            }
                        }
                    }
                }
            }
        }
        #endregion
    }
}