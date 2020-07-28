namespace Mayfair.Core.Code.FrameworkSettings
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
        protected TData data = null;
        #endregion

        #region Properties
        public TData Data
        {
            get { return data; }
        }
        #endregion
    }
}
