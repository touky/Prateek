namespace Assets.Prateek.ToConvert.Resources.Loader
{
    using System;
    using System.Diagnostics;
    using UnityEngine;

    [DebuggerDisplay("{typeof(TResourceType).Name}, {loader.resource.ToString()}, Location: {loader.location}")]
    public abstract class UnityObjectResourceReference<TResourceType, TLoadActionType> : UnityResourceReference<TResourceType, TLoadActionType>
        where TResourceType : UnityEngine.Object
        where TLoadActionType : AbstractResourceReference<TResourceType, TLoadActionType>
    {
        #region Constructors
        protected UnityObjectResourceReference(ResourceLoader loader) : base(loader) { }
        #endregion

        #region Class Methods
        protected TResourceType MarkObject(TResourceType resourceInstance)
        {
            var gameObject = GetGameObject(resourceInstance);
            if (gameObject == null)
            {
                throw new Exception($"Wrong Reference type used for {typeof(TResourceType).Name}");
            }

            var referenceInstance = gameObject.AddComponent<ResourceReferenceInstance>();
            referenceInstance.Init(this);

            return resourceInstance;
        }

        protected abstract GameObject GetGameObject(TResourceType resourceType);
        #endregion

        #region UnityEngine.Object
        /// <summary>
        ///     <para>Clones the original resource and returns the clone.</para>
        /// </summary>
        /// <param name="position">Position for the new object.</param>
        /// <param name="rotation">Orientation of the new object.</param>
        /// <returns>
        ///     <para>The instantiated clone.</para>
        /// </returns>
        public TResourceType Instantiate(Vector3 position, Quaternion rotation)
        {
            return MarkObject(UnityEngine.Object.Instantiate(TypedResource, position, rotation));
        }

        /// <summary>
        ///     <para>Clones the original resource and returns the clone.</para>
        /// </summary>
        /// <param name="position">Position for the new object.</param>
        /// <param name="rotation">Orientation of the new object.</param>
        /// <param name="parent">Parent that will be assigned to the new object.</param>
        /// <returns>
        ///     <para>The instantiated clone.</para>
        /// </returns>
        public TResourceType Instantiate(Vector3 position, Quaternion rotation, Transform parent)
        {
            return MarkObject(UnityEngine.Object.Instantiate(TypedResource, position, rotation, parent));
        }

        /// <summary>
        ///     <para>Clones the original resource and returns the clone.</para>
        /// </summary>
        /// <returns>
        ///     <para>The instantiated clone.</para>
        /// </returns>
        public TResourceType Instantiate()
        {
            return MarkObject(UnityEngine.Object.Instantiate(TypedResource));
        }

        /// <summary>
        ///     <para>Clones the original resource and returns the clone.</para>
        /// </summary>
        /// <param name="parent">Parent that will be assigned to the new object.</param>
        /// <returns>
        ///     <para>The instantiated clone.</para>
        /// </returns>
        public TResourceType Instantiate(Transform parent)
        {
            return MarkObject(UnityEngine.Object.Instantiate(TypedResource, parent));
        }

        /// <summary>
        ///     <para>Clones the original resource and returns the clone.</para>
        /// </summary>
        /// <param name="parent">Parent that will be assigned to the new object.</param>
        /// <param name="worldPositionStays">
        ///     Pass true when assigning a parent Object to maintain the world position of the
        ///     Object, instead of setting its position relative to the new parent. Pass false to set the Object's position
        ///     relative to its new parent.
        /// </param>
        /// <returns>
        ///     <para>The instantiated clone.</para>
        /// </returns>
        public TResourceType Instantiate(Transform parent, bool worldPositionStays)
        {
            return MarkObject(UnityEngine.Object.Instantiate(TypedResource, parent, worldPositionStays));
        }
        #endregion
    }
}
