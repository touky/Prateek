namespace Mayfair.Core.Code.Utils.Debug
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Text;
    using Mayfair.Core.Code.Utils.Debug.Reflection;
    using Prateek.DaemonCore.Code.Branches;
    using Prateek.DaemonCore.Code.Interfaces;

    public static partial class DebugTools
    {
        #region Class Methods
        [Conditional("NVIZZIO_DEV")]
        public static void Log(IDaemonCore daemonCore, string notice, LogLevel logPriority = LogLevel.Normal)
        {
            StringBuilder builder = new StringBuilder();
            builder.AddLogHeader(daemonCore, notice);
            Log(builder.ToString(), daemonCore, logPriority);
        }

        [Conditional("NVIZZIO_DEV")]
        public static void Log(IDaemonBranch branch, string notice, LogLevel logPriority = LogLevel.Normal)
        {
            StringBuilder builder = new StringBuilder();
            builder.AddLogHeader(branch, notice);
            Log(builder.ToString(), branch, logPriority);
        }

        [Conditional("NVIZZIO_DEV")]
        public static void LogWarning(IDaemonCore daemonCore, string notice, LogLevel logPriority = LogLevel.Normal)
        {
            StringBuilder builder = new StringBuilder();
            builder.AddLogHeader(daemonCore, notice);
            LogWarning(builder.ToString(), daemonCore, logPriority);
        }

        [Conditional("NVIZZIO_DEV")]
        public static void LogWarning(IDaemonBranch branch, string notice, LogLevel logPriority = LogLevel.Normal)
        {
            StringBuilder builder = new StringBuilder();
            builder.AddLogHeader(branch, notice);
            LogWarning(builder.ToString(), branch, logPriority);
        }

        [Conditional("NVIZZIO_DEV")]
        public static void LogError(IDaemonCore daemonCore, string notice, LogLevel logPriority = LogLevel.Normal)
        {
            StringBuilder builder = new StringBuilder();
            builder.AddLogHeader(daemonCore, notice);
            LogWarning(builder.ToString(), daemonCore, logPriority);
        }

        [Conditional("NVIZZIO_DEV")]
        public static void LogError(IDaemonBranch branch, string notice, LogLevel logPriority = LogLevel.Normal)
        {
            StringBuilder builder = new StringBuilder();
            builder.AddLogHeader(branch, notice);
            LogWarning(builder.ToString(), branch, logPriority);
        }

        //todo [Conditional("NVIZZIO_DEV")]
        //todo public static void Log<TResourceType, TResourceRef>(IDaemonBranch branch, ResourcesHaveChangedResponse<TResourceRef, TResourceType> notice)
        //todo     where TResourceRef : AbstractResourceReference<TResourceType, TResourceRef>
        //todo {
        //todo     StringBuilder builder = new StringBuilder();
        //todo     builder.AddLogHeader(branch, ", data to load are:");
        //todo 
        //todo     ReflectedField<List<TResourceRef>> references = "references";
        //todo     references.Init(notice);
        //todo 
        //todo     foreach (TResourceRef resource in references.Value)
        //todo     {
        //todo         builder.AppendLine($" - {resource.Loader.Location}");
        //todo     }
        //todo 
        //todo     Log(builder.ToString(), LogLevel.LowPriority);
        //todo }
        #endregion
    }
}
