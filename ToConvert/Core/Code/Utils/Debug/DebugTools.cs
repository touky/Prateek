namespace Mayfair.Core.Code.Utils.Debug
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Text;
    using Attributes;
    using UnityEngine;
    using UnityEngine.Assertions;
    using Debug = UnityEngine.Debug;
    using Object = UnityEngine.Object;

    public static partial class DebugTools
    {
        public const string LOG_LEVEL_PREFS = "DebugTools.logLevel";
        /// <summary>
        ///     The types of logs to allow for output
        /// </summary>
        public static LogLevel logLevel = LogLevel.Normal;

        public static void Log(string message, LogLevel logPriority = LogLevel.Normal)
        {
            if (logLevel <= logPriority)
            {
                Debug.Log(message);
            }
        }

        public static void Log(string message, Object context, LogLevel logPriority = LogLevel.Normal)
        {
            if (logLevel <= logPriority)
            {
                Debug.LogWarning(message, context);
            }
        }

        public static void Log(string message, params object[] args)
        {
            if (logLevel <= LogLevel.Normal)
            {
                message = String.Format(message, args);
                Log(message);
            }
        }

        public static void Log(string message, object[] args, LogLevel logPriority = LogLevel.Normal)
        {
            if (logLevel <= logPriority)
            {
                message = String.Format(message, args);
                Log(message);
            }
        }

        public static void LogWarning(string message, LogLevel logPriority = LogLevel.Normal)
        {
            if (logLevel <= logPriority)
            {
                Debug.LogWarning(message);
            }
        }

        public static void LogWarning(string message, Object context, LogLevel logPriority = LogLevel.Normal)
        {
            if (logLevel <= logPriority)
            {
                Debug.LogWarning(message, context);
            }
        }

        public static void LogWarning(string message, params object[] args)
        {
            if (logLevel <= LogLevel.Normal)
            {
                message = String.Format(message, args);
                LogWarning(message);
            }
        }

        public static void LogWarning(string message, object[] args, LogLevel logPriority = LogLevel.Normal)
        {
            if (logLevel <= logPriority)
            {
                message = String.Format(message, args);
                LogWarning(message);
            }
        }
         
        public static void Log(StringBuilder build, LogLevel logPriority = LogLevel.Normal)
        {
            if (build == null)
            {
                return;
            }

            if (logLevel <= logPriority)
            {
                Debug.Log(build.ToString());
            }
        }

        /// <summary>
        ///     Errors are always logged ot the console regardless of the logLevel
        /// </summary>
        public static void LogError(string message)
        {
            Debug.LogError(message);
        }

        /// <summary>
        ///     Errors are always logged ot the console regardless of the logLevel
        /// </summary>
        public static void LogError(string message, Object context)
        {
            Debug.LogError(message, context);
        }

        /// <summary>
        ///     Errors are always logged ot the console regardless of the logLevel
        /// </summary>
        public static void LogError(string message, params object[] args)
        {
            message = String.Format(message, args);
            LogError(message);
        }

        [Obsolete("Use new MonoBehaviour extension method AssertAssetsAreLinked")]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void AssertAssetLinked(Object obj, string objName)
        {
            if (logLevel == LogLevel.None)
            {
                return;
            }

            // If the object is not null, then we assume that it is linked in the inspector
            if (obj != null)
            {
                return;
            }

            MethodBase callingMethod = new StackFrame(1).GetMethod();
            string callingTypeName = "Unknown";

            if (callingMethod != null)
            {
                callingTypeName = callingMethod.ReflectedType.Name;
            }
            else
            {
                LogError("Failed to retrieve calling Type. Full stack: {0}", new StackTrace());
            }

            LogError("An object named {0} was not linked in the inspector for component type {1}.", objName, callingTypeName);
        }

        [Conditional("NVIZZIO_DEV")]
        public static void AssertAssetsAreLinked(this MonoBehaviour owner)
        {
            if (logLevel == LogLevel.None)
            {
                return;
            }

            Type callingType = owner.GetType();

            IEnumerable<FieldInfo> publicFields = callingType.GetFields(BindingFlags.Instance | BindingFlags.Public);
            IEnumerable<FieldInfo> serializedFields = callingType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                                                                 .Where(field => Attribute.IsDefined(field, typeof(SerializeField)));

            publicFields = RemoveUnlinkedAssetFields(publicFields);
            serializedFields = RemoveUnlinkedAssetFields(serializedFields);

            AssertFieldsAreNotNull(owner, publicFields);
            AssertFieldsAreNotNull(owner, serializedFields);
        }

        public static string ColorCodeMessage(string msg, Color32 color)
        {
            return $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{msg}</color>";
        }

        public static void ColorCodeMessage(this StringBuilder builder, Color32 color)
        {
            builder.Insert(0, $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>");
            builder.Append("</color>");
        }

        private static IEnumerable<FieldInfo> RemoveUnlinkedAssetFields(IEnumerable<FieldInfo> fields)
        {
            return fields.Where(field => !Attribute.IsDefined(field, typeof(UnlinkedAssetAttribute)));
        }

        private static void AssertFieldsAreNotNull(MonoBehaviour owner, IEnumerable<FieldInfo> fieldInfos)
        {
            foreach (FieldInfo fieldInfo in fieldInfos)
            {
                // string is a special case as it is a reference type, but in inspector treated as a value
                if (!fieldInfo.FieldType.IsValueType && fieldInfo.FieldType != typeof(string))
                {
                    Object unityObject = fieldInfo.GetValue(owner) as Object;
                    Assert.IsNotNull(unityObject, $"Field {fieldInfo.Name} of {owner.GetType().Name} on object named '{owner.name}' was not linked in the inspector.");
                }
            }
        }

        public enum LogLevel
        {
            /// <summary>
            ///     The Verbose option will allow output of every log
            /// </summary>
            Verbose,

            /// <summary>
            ///     <para>
            ///         The Low Priority option is for messages that are not verbose, but are also unnecessary in typical workflow.
            ///     </para>
            ///     <example>
            ///         Logs related to the loading flow may typically be too much noise, so we only want them to appear when Low
            ///         Priority logs are enabled.
            ///     </example>
            /// </summary>
            LowPriority,

            /// <summary>
            ///     Typical logging level
            /// </summary>
            Normal,

            // None should always be the last item
            None
        }
    }
}