namespace Assets.Prateek.ToConvert.StateMachines
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using UnityEngine.Profiling;

    public struct StateMachineDebug<TState, TTrigger>
    {
        #region Static and Constants
        private static string FAILED_FOUND = "-UNKNOWN-";
        private static readonly string DEFAULT_STATE_FORMAT = $"State {typeof(TState).Name} {{0}}";
        private static readonly string DEFAULT_TRIGGER_FORMAT = $"Trigger {typeof(TTrigger).Name} {{0}}";
        #endregion

#if ENABLE_PROFILER
        private Dictionary<TState, string> stateDebugTexts;
        private Dictionary<TTrigger, string> triggerDebugTexts;
#endif

        #region Profiling
        [Conditional("ENABLE_PROFILER")]
        public void InitProfiling(string stateProfilerFormat, string triggerProfilerFormat, Action<Dictionary<TState, string>, string, Dictionary<TTrigger, string>, string> DebugCaching)
        {
#if ENABLE_PROFILER
            stateDebugTexts = new Dictionary<TState, string>();
            triggerDebugTexts = new Dictionary<TTrigger, string>();

#if ENABLE_PROFILER
            stateProfilerFormat = stateProfilerFormat == null ? DEFAULT_STATE_FORMAT : stateProfilerFormat;
            triggerProfilerFormat = triggerProfilerFormat == null ? DEFAULT_TRIGGER_FORMAT : triggerProfilerFormat;
#else
            stateProfilerFormat = null;
            triggerProfilerFormat = null;
#endif

            DebugCaching.Invoke(stateDebugTexts, stateProfilerFormat, triggerDebugTexts, triggerProfilerFormat);
#endif //ENABLE_PROFILER
        }

        [Conditional("ENABLE_PROFILER")]
        public void ProfilerBeginFrame(TState currentState)
        {
#if ENABLE_PROFILER
            var text = string.Empty;
            if (!stateDebugTexts.TryGetValue(currentState, out text))
            {
                text = FAILED_FOUND;
            }

            Profiler.BeginSample(text);
#endif //ENABLE_PROFILER
        }

        [Conditional("ENABLE_PROFILER")]
        public void ProfilerEndFrame()
        {
#if ENABLE_PROFILER
            Profiler.EndSample();
#endif //ENABLE_PROFILER
        }
        #endregion
    }
}
