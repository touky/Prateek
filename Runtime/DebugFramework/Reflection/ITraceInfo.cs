namespace Prateek.Runtime.DebugFramework.Reflection
{
    using System;
    using System.Collections.Generic;

    public interface ITraceInfo
    {
        #region Properties
        KeyValuePair<Type, string> Info { get; }
        #endregion
    }
}
