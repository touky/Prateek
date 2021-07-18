namespace Prateek.Runtime.StateMachineFramework.EnumStateMachines
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using ImGuiNET;
    using Prateek.Runtime.DebugFramework.DebugMenu;
    using Prateek.Runtime.DebugFramework.DebugMenu.Documents;
    using Prateek.Runtime.DebugFramework.DebugMenu.Gadgets;
    using Prateek.Runtime.DebugFramework.DebugMenu.Sections;
    using Prateek.Runtime.DebugFramework.Reflection;
    using Prateek.Runtime.StateMachineFramework.Interfaces;
    using UnityEngine;

    public class DelegateStateMachineSection<TOwner, TStateMachine, TTrigger>
        : DebugMenuSection<TOwner>
        where TOwner : class, DebugMenu.IDebugMenuOwner, DelegateStateMachine.IOwner
        where TStateMachine : IStateMachine<MethodInfo, TTrigger>
        where TTrigger : struct, IConvertible
    {
        #region Static and Constants
        protected const string STATEMACHINE_FIELD = "StateMachine";
        #endregion

        #region Fields
        protected DebugField<TStateMachine> stateMachine = string.Empty;
        protected DebugField<List<MethodInfo>> states = string.Empty;
        #endregion

        #region Constructors
        public DelegateStateMachineSection(string stateMachineFieldName = STATEMACHINE_FIELD)
            : base($"State machine <{typeof(TTrigger).Name}>")
        {
            stateMachine = stateMachineFieldName;
        }
        #endregion

        #region Class Methods
        protected override void ManualInit()
        {
            states = "states";
        }

        protected override void OnDraw(DebugMenuContext context)
        {
            DrawStates();
        }

        protected void DrawStates()
        {
            if (stateMachine.AssertDrawable())
            {
                ImGui.TextColored(Color.green, $"Active state: {stateMachine.Value.ActiveState.ToString()}");
                ImGui.TextColored(Color.cyan, $"-> Incoming state: {stateMachine.Value.IncomingState.ToString()}");
                ImGui.Separator();
            }

            if (!states.AssertDrawable())
            {
                states.SetOwner(stateMachine.Value);
                return;
            }

            if (ImGui.CollapsingHeader("States:"))
            {
                using (new ScopeIndent())
                {
                    foreach (var state in states.Value)
                    {
                        ImGui.Text($"- {state.ToString()}");
                    }
                }
            }
        }
        #endregion
    }
}
