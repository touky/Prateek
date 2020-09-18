namespace Prateek.Runtime.StateMachineFramework
{
    using Prateek.Runtime.Core.Profiling;

    public class StateMachineProfilingScope<TState, TTrigger> : ProfilerScope
    {
        #region Constructors
        protected StateMachineProfilingScope(string state) : base(state) { }
        protected StateMachineProfilingScope(TState state) : base($"State {typeof(TState).Name}") { }
        #endregion

        #region Class Methods
        public static ProfilerScope Open(TState state)
        {
#if ENABLE_PROFILER
            return new StateMachineProfilingScope<TState, TTrigger>(state);
#else
            return null;
#endif //ENABLE_PROFILER
        }

        protected override void CloseScope()
        {
            base.CloseScope();
        }
        #endregion
    }
}
