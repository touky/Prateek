namespace Prateek.Runtime.Core.FrameworkSettings
{
    using System;
    using UnityEngine;

    /// <summary>
    ///     Base class for framework settings.
    ///     Use templated version
    /// </summary>
    public abstract class FrameworkSettings
    {
        #region Properties
        public abstract bool IsAvailable { get; }
        public abstract Type ResourceType { get; }
        public abstract string DefaultPath { get; }
        #endregion

        #region Class Methods
        internal void InternalInit()
        {
            Init();
        }

        protected abstract void Init();
        #endregion
    }
}