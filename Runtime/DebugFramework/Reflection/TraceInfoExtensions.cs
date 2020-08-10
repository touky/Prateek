namespace Prateek.Runtime.DebugFramework.Reflection
{
    using System;
    using System.Reflection;

    public static class TraceInfoExtensions
    {
        #region Class Methods
        public static bool Validate(this ITraceInfo traceInfo, Type ownerType, MethodBase methodBase)
        {
            return ownerType == traceInfo.Info.Key && methodBase.Name == traceInfo.Info.Value;
        }
        #endregion
    }
}
