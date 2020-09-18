namespace Prateek.Runtime.Core.Profiling
{
    using UnityEngine;
    using UnityEngine.Profiling;

    public class ProfilerScope : GUI.Scope
    {
        #region Constructors
        protected ProfilerScope(string sample)
        {
#if ENABLE_PROFILER
            Profiler.BeginSample(sample);
#endif //ENABLE_PROFILER
        }

        public static ProfilerScope Open(string sample)
        {
#if ENABLE_PROFILER
            return new ProfilerScope(sample);
#else
            return null;
#endif //ENABLE_PROFILER
        }
        #endregion

        #region Class Methods
        protected override void CloseScope()
        {
#if ENABLE_PROFILER
            Profiler.EndSample();
#endif //ENABLE_PROFILER
        }
        #endregion
    }
}
