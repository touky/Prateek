namespace Prateek.Runtime.AppContentFramework.Unity.Commands
{
    using Prateek.Runtime.AppContentFramework.ContentLoaders;
    using Prateek.Runtime.AppContentFramework.Messages;
    using Prateek.Runtime.AppContentFramework.Unity.Handles;
    using UnityEngine;

    /// <summary>
    ///     Use this class as base to implement your own callback for the ResourcesHaveChanged notice
    /// </summary>
    /// <typeparam name="TContentType">The type of the resource</typeparam>
    public abstract class ScriptableContentAccessChangedResponse<TContentType>
        : ContentAccessChangedResponse<TContentType, ScriptableObjectHandle<TContentType>>
        where TContentType : ScriptableObject
    {
        #region Class Methods
        protected override ScriptableObjectHandle<TContentType> GetHandle()
        {
            return new ScriptableObjectHandle<TContentType>();
        }
        #endregion
    }
}
