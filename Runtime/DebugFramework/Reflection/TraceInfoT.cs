namespace Prateek.Runtime.DebugFramework.Reflection
{
    using System;
    using System.Collections.Generic;

    public struct TraceInfo<TOwnerClass> : ITraceInfo
    {
        #region Fields
        private KeyValuePair<Type, string> info;
        #endregion

        #region Properties
        public KeyValuePair<Type, string> Info { get { return info; } }
        #endregion

        #region Class Methods
        public static implicit operator TraceInfo<TOwnerClass>(string methodName)
        {
            return new TraceInfo<TOwnerClass>
            {
                info = new KeyValuePair<Type, string>(typeof(TOwnerClass), methodName)
            };
        }
        #endregion
    }
}
