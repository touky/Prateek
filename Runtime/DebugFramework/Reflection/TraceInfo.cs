namespace Prateek.Runtime.DebugFramework.Reflection
{
    using System;
    using System.Collections.Generic;

    public struct TraceInfo : ITraceInfo
    {
        #region Fields
        private KeyValuePair<Type, string> info;
        #endregion

        #region Properties
        public KeyValuePair<Type, string> Info { get { return info; } }
        #endregion

        #region Class Methods
        public static implicit operator TraceInfo(Type ownerType)
        {
            return new TraceInfo
            {
                info = new KeyValuePair<Type, string>(ownerType, string.Empty)
            };
        }

        public static implicit operator TraceInfo(string methodName)
        {
            return new TraceInfo
            {
                info = new KeyValuePair<Type, string>(null, methodName)
            };
        }

        public static TraceInfo operator +(TraceInfo trace, Type ownerType)
        {
            return new TraceInfo
            {
                info = new KeyValuePair<Type, string>(ownerType, trace.info.Value)
            };
        }

        public static TraceInfo operator +(Type ownerType, TraceInfo trace)
        {
            return new TraceInfo
            {
                info = new KeyValuePair<Type, string>(ownerType, trace.info.Value)
            };
        }

        public static TraceInfo operator +(TraceInfo trace, string methodName)
        {
            return new TraceInfo
            {
                info = new KeyValuePair<Type, string>(trace.info.Key, methodName)
            };
        }

        public static TraceInfo operator +(string methodName, TraceInfo trace)
        {
            return new TraceInfo
            {
                info = new KeyValuePair<Type, string>(trace.info.Key, methodName)
            };
        }
        #endregion
    }
}
