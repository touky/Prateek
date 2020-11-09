namespace Prateek.Runtime.Core.FrameworkSettings
{
    using System;
    using UnityEngine;

    /// <summary>
    ///     Base class to implement Framework settings ScriptableObject
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    [Serializable]
    public abstract class FrameworkSettingsResource<TData> : ScriptableObject
        where TData : FrameworkSettingsData, new()
    {
        #region Fields
        [SerializeField]
        protected TData data;
        #endregion

        #region Properties
        public TData Data
        {
            get { return data; }
        }
        #endregion
    }
}
