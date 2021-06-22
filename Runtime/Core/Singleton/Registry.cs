namespace Prateek.Runtime.Core.Singleton
{
    using System;
    using UnityEngine;

    public abstract class Registry<TInstance>
        : SingletonBehaviour<TInstance>
        where TInstance : MonoBehaviour
    {
        #region Properties
        protected override string ParentName { get { return "Registry"; } }
        #endregion
    }
}