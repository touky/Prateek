namespace Mayfair.Core.Code.Resources.Loader
{
    using System;
    using System.Diagnostics;
    using UnityEngine;

    [DebuggerDisplay("{typeof(TResourceType).Name}, {loader.content.ToString()}, Location: {loader.location}")]
    public abstract class UnityObjectContentHandle<TContentType, TLoadActionType>
        : UnityContentHandle<TContentType, TLoadActionType>
        where TContentType : UnityEngine.Object
        where TLoadActionType : ContentHandle<TContentType, TLoadActionType>
    {
        #region Constructors
        protected UnityObjectContentHandle(ContentLoader loader) : base(loader) { }
        #endregion

        #region Class Methods
        protected TContentType MarkObject(TContentType resourceInstance)
        {
            GameObject gameObject = GetGameObject(resourceInstance);
            if (gameObject == null)
            {
                throw new Exception($"Wrong Reference type used for {typeof(TContentType).Name}");
            }

            ContentInstanceRef referenceInstance = gameObject.AddComponent<ContentInstanceRef>();
            referenceInstance.Init(this);

            return resourceInstance;
        }

        protected abstract GameObject GetGameObject(TContentType resourceType);
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
        public TContentType Instantiate(Vector3 position, Quaternion rotation)
        {
            return MarkObject(UnityEngine.Object.Instantiate(TypedContent, position, rotation));
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
        public TContentType Instantiate(Vector3 position, Quaternion rotation, Transform parent)
        {
            return MarkObject(UnityEngine.Object.Instantiate(TypedContent, position, rotation, parent));
        }

        /// <summary>
        ///     <para>Clones the original resource and returns the clone.</para>
        /// </summary>
        /// <returns>
        ///     <para>The instantiated clone.</para>
        /// </returns>
        public TContentType Instantiate()
        {
            return MarkObject(UnityEngine.Object.Instantiate(TypedContent));
        }

        /// <summary>
        ///     <para>Clones the original resource and returns the clone.</para>
        /// </summary>
        /// <param name="parent">Parent that will be assigned to the new object.</param>
        /// <returns>
        ///     <para>The instantiated clone.</para>
        /// </returns>
        public TContentType Instantiate(Transform parent)
        {
            return MarkObject(UnityEngine.Object.Instantiate(TypedContent, parent));
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
        public TContentType Instantiate(Transform parent, bool worldPositionStays)
        {
            return MarkObject(UnityEngine.Object.Instantiate(TypedContent, parent, worldPositionStays));
        }
        #endregion
    }
}
