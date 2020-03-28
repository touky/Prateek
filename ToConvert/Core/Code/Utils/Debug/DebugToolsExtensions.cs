namespace Mayfair.Core.Code.Utils.Debug
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Text;
    using Mayfair.Core.Code.Resources.Loader;
    using Mayfair.Core.Code.Resources.Messages;
    using Mayfair.Core.Code.Service;
    using Mayfair.Core.Code.Utils.Debug.Reflection;
    using Prateek.DaemonCore.Code.Branches;
    using Prateek.DaemonCore.Code.Interfaces;

    public static partial class DebugTools
    {
        #region Class Methods
        [Conditional("NVIZZIO_DEV")]
        public static void Log(IDaemonCore daemonCore, string message, LogLevel logPriority = LogLevel.Normal)
        {
            StringBuilder builder = new StringBuilder();
            builder.AddLogHeader(daemonCore, message);
            Log(builder.ToString(), daemonCore, logPriority);
        }

        [Conditional("NVIZZIO_DEV")]
        public static void Log(DaemonBranch branch, string message, LogLevel logPriority = LogLevel.Normal)
        {
            StringBuilder builder = new StringBuilder();
            builder.AddLogHeader(branch, message);
            Log(builder.ToString(), branch, logPriority);
        }

        [Conditional("NVIZZIO_DEV")]
        public static void Log(DaemonBranchBehaviour branch, string message, LogLevel logPriority = LogLevel.Normal)
        {
            StringBuilder builder = new StringBuilder();
            builder.AddLogHeader(branch, message);
            Log(builder.ToString(), branch, logPriority);
        }

        [Conditional("NVIZZIO_DEV")]
        public static void LogWarning(IDaemonCore daemonCore, string message, LogLevel logPriority = LogLevel.Normal)
        {
            StringBuilder builder = new StringBuilder();
            builder.AddLogHeader(daemonCore, message);
            LogWarning(builder.ToString(), daemonCore, logPriority);
        }

        [Conditional("NVIZZIO_DEV")]
        public static void LogWarning(DaemonBranch branch, string message, LogLevel logPriority = LogLevel.Normal)
        {
            StringBuilder builder = new StringBuilder();
            builder.AddLogHeader(branch, message);
            LogWarning(builder.ToString(), branch, logPriority);
        }

        [Conditional("NVIZZIO_DEV")]
        public static void LogWarning(DaemonBranchBehaviour branch, string message, LogLevel logPriority = LogLevel.Normal)
        {
            StringBuilder builder = new StringBuilder();
            builder.AddLogHeader(branch, message);
            LogWarning(builder.ToString(), branch, logPriority);
        }

        [Conditional("NVIZZIO_DEV")]
        public static void LogError(IDaemonCore daemonCore, string message, LogLevel logPriority = LogLevel.Normal)
        {
            StringBuilder builder = new StringBuilder();
            builder.AddLogHeader(daemonCore, message);
            LogWarning(builder.ToString(), daemonCore, logPriority);
        }

        [Conditional("NVIZZIO_DEV")]
        public static void LogError(DaemonBranch branch, string message, LogLevel logPriority = LogLevel.Normal)
        {
            StringBuilder builder = new StringBuilder();
            builder.AddLogHeader(branch, message);
            LogWarning(builder.ToString(), branch, logPriority);
        }

        [Conditional("NVIZZIO_DEV")]
        public static void LogError(DaemonBranchBehaviour branch, string message, LogLevel logPriority = LogLevel.Normal)
        {
            StringBuilder builder = new StringBuilder();
            builder.AddLogHeader(branch, message);
            LogWarning(builder.ToString(), branch, logPriority);
        }

        [Conditional("NVIZZIO_DEV")]
        public static void Log<TResourceType, TResourceRef>(DaemonBranchBehaviour branch, ResourcesHaveChangedResponse<TResourceRef, TResourceType> message)
            where TResourceRef : AbstractResourceReference<TResourceType, TResourceRef>
        {
            StringBuilder builder = new StringBuilder();
            builder.AddLogHeader(branch, ", data to load are:");

            ReflectedField<List<TResourceRef>> references = "references";
            references.Init(message);

            foreach (TResourceRef resource in references.Value)
            {
                builder.AppendLine($" - {resource.Loader.Location}");
            }

            Log(builder.ToString(), LogLevel.LowPriority);
        }
        #endregion
    }
}
