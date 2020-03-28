namespace Mayfair.Core.Code.Utils.Debug
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Text;
    using Mayfair.Core.Code.Resources.Loader;
    using Mayfair.Core.Code.Resources.Messages;
    using Mayfair.Core.Code.Service;
    using Mayfair.Core.Code.Service.Interfaces;
    using Mayfair.Core.Code.Utils.Debug.Reflection;

    public static partial class DebugTools
    {
        #region Class Methods
        [Conditional("NVIZZIO_DEV")]
        public static void Log(IService service, string message, LogLevel logPriority = LogLevel.Normal)
        {
            StringBuilder builder = new StringBuilder();
            builder.AddLogHeader(service, message);
            Log(builder.ToString(), service, logPriority);
        }

        [Conditional("NVIZZIO_DEV")]
        public static void Log(ServiceProvider provider, string message, LogLevel logPriority = LogLevel.Normal)
        {
            StringBuilder builder = new StringBuilder();
            builder.AddLogHeader(provider, message);
            Log(builder.ToString(), provider, logPriority);
        }

        [Conditional("NVIZZIO_DEV")]
        public static void Log(ServiceProviderBehaviour provider, string message, LogLevel logPriority = LogLevel.Normal)
        {
            StringBuilder builder = new StringBuilder();
            builder.AddLogHeader(provider, message);
            Log(builder.ToString(), provider, logPriority);
        }

        [Conditional("NVIZZIO_DEV")]
        public static void LogWarning(IService service, string message, LogLevel logPriority = LogLevel.Normal)
        {
            StringBuilder builder = new StringBuilder();
            builder.AddLogHeader(service, message);
            LogWarning(builder.ToString(), service, logPriority);
        }

        [Conditional("NVIZZIO_DEV")]
        public static void LogWarning(ServiceProvider provider, string message, LogLevel logPriority = LogLevel.Normal)
        {
            StringBuilder builder = new StringBuilder();
            builder.AddLogHeader(provider, message);
            LogWarning(builder.ToString(), provider, logPriority);
        }

        [Conditional("NVIZZIO_DEV")]
        public static void LogWarning(ServiceProviderBehaviour provider, string message, LogLevel logPriority = LogLevel.Normal)
        {
            StringBuilder builder = new StringBuilder();
            builder.AddLogHeader(provider, message);
            LogWarning(builder.ToString(), provider, logPriority);
        }

        [Conditional("NVIZZIO_DEV")]
        public static void LogError(IService service, string message, LogLevel logPriority = LogLevel.Normal)
        {
            StringBuilder builder = new StringBuilder();
            builder.AddLogHeader(service, message);
            LogWarning(builder.ToString(), service, logPriority);
        }

        [Conditional("NVIZZIO_DEV")]
        public static void LogError(ServiceProvider provider, string message, LogLevel logPriority = LogLevel.Normal)
        {
            StringBuilder builder = new StringBuilder();
            builder.AddLogHeader(provider, message);
            LogWarning(builder.ToString(), provider, logPriority);
        }

        [Conditional("NVIZZIO_DEV")]
        public static void LogError(ServiceProviderBehaviour provider, string message, LogLevel logPriority = LogLevel.Normal)
        {
            StringBuilder builder = new StringBuilder();
            builder.AddLogHeader(provider, message);
            LogWarning(builder.ToString(), provider, logPriority);
        }

        [Conditional("NVIZZIO_DEV")]
        public static void Log<TResourceType, TResourceRef>(ServiceProviderBehaviour provider, ResourcesHaveChangedResponse<TResourceRef, TResourceType> message)
            where TResourceRef : AbstractResourceReference<TResourceType, TResourceRef>
        {
            StringBuilder builder = new StringBuilder();
            builder.AddLogHeader(provider, ", data to load are:");

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
