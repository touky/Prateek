#define PRATEEK_DEBUG

namespace Mayfair.Core.Code.Utils.Types
{
    using UnityEngine;
    using UnityEngine.Profiling;

    //benjaminh: Aggressive ifdef to ensure it's never used in release
#if PRATEEK_DEBUG
    public class ProfilerScope : GUI.Scope
    {
        #region Constructors
        public ProfilerScope(string sample)
        {
            Profiler.BeginSample(sample);
        }
        #endregion

        #region Class Methods
        protected override void CloseScope()
        {
            Profiler.EndSample();
        }
        #endregion
    }
#endif //PRATEEK_DEBUG
}
