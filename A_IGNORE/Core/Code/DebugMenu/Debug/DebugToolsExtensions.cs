namespace Mayfair.Core.Code.Utils.Debug
{
    using System.Diagnostics;
    using System.Text;
    using Prateek.DaemonFramework.Code.Interfaces;

    public static partial class DebugTools
    {
        #region Class Methods
        [Conditional("NVIZZIO_DEV")]
        public static void Log(IDaemon daemonCore, string notice, LogLevel logPriority = LogLevel.Normal)
        {
            StringBuilder builder = new StringBuilder();
            builder.AddLogHeader(daemonCore, notice);
            Log(builder.ToString(), daemonCore, logPriority);
        }

        [Conditional("NVIZZIO_DEV")]
        public static void Log(IServant servant, string notice, LogLevel logPriority = LogLevel.Normal)
        {
            StringBuilder builder = new StringBuilder();
            builder.AddLogHeader(servant, notice);
            Log(builder.ToString(), servant, logPriority);
        }

        [Conditional("NVIZZIO_DEV")]
        public static void LogWarning(IDaemon daemonCore, string notice, LogLevel logPriority = LogLevel.Normal)
        {
            StringBuilder builder = new StringBuilder();
            builder.AddLogHeader(daemonCore, notice);
            LogWarning(builder.ToString(), daemonCore, logPriority);
        }

        [Conditional("NVIZZIO_DEV")]
        public static void LogWarning(IServant servant, string notice, LogLevel logPriority = LogLevel.Normal)
        {
            StringBuilder builder = new StringBuilder();
            builder.AddLogHeader(servant, notice);
            LogWarning(builder.ToString(), servant, logPriority);
        }

        [Conditional("NVIZZIO_DEV")]
        public static void LogError(IDaemon daemonCore, string notice, LogLevel logPriority = LogLevel.Normal)
        {
            StringBuilder builder = new StringBuilder();
            builder.AddLogHeader(daemonCore, notice);
            LogWarning(builder.ToString(), daemonCore, logPriority);
        }

        [Conditional("NVIZZIO_DEV")]
        public static void LogError(IServant servant, string notice, LogLevel logPriority = LogLevel.Normal)
        {
            StringBuilder builder = new StringBuilder();
            builder.AddLogHeader(servant, notice);
            LogWarning(builder.ToString(), servant, logPriority);
        }

        //todo [Conditional("NVIZZIO_DEV")]
        //todo public static void Log<TResourceType, TResourceRef>(IServant servant, ResourcesHaveChangedResponse<TResourceRef, TResourceType> notice)
        //todo     where TResourceRef : AbstractResourceReference<TResourceType, TResourceRef>
        //todo {
        //todo     StringBuilder builder = new StringBuilder();
        //todo     builder.AddLogHeader(servant, ", data to load are:");
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
