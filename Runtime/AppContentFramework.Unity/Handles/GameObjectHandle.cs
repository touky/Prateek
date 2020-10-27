namespace Prateek.Runtime.AppContentFramework.Unity.Handles
{
    using System;
    using System.Diagnostics;
    using Prateek.Runtime.AppContentFramework.ContentLoaders;
    using Prateek.Runtime.AppContentFramework.Unity.Addressables;
    using UnityEngine;

    [DebuggerDisplay("{typeof(TContentType).Name}, {loader.content.ToString()}, Location: {loader.path}")]
    public class GameObjectHandle
        : UnityContentHandle<GameObject, GameObjectHandle>
    {
        #region Constructors
        public GameObjectHandle(ContentLoader loader) : base(loader) { }
        #endregion

        #region Class Methods
        protected GameObject MarkObject(GameObject resourceInstance)
        {
            if (resourceInstance == null)
            {
                throw new Exception($"Wrong Reference type used for {nameof(GameObject)}");
            }

            var referenceInstance = resourceInstance.AddComponent<GameObjectContentReference>();
            referenceInstance.Init(this);

            return resourceInstance;
        }
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
        public GameObject Instantiate(Vector3 position, Quaternion rotation)
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
        public GameObject Instantiate(Vector3 position, Quaternion rotation, Transform parent)
        {
            return MarkObject(UnityEngine.Object.Instantiate(TypedContent, position, rotation, parent));
        }

        /// <summary>
        ///     <para>Clones the original resource and returns the clone.</para>
        /// </summary>
        /// <returns>
        ///     <para>The instantiated clone.</para>
        /// </returns>
        public GameObject Instantiate()
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
        public GameObject Instantiate(Transform parent)
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
        public GameObject Instantiate(Transform parent, bool worldPositionStays)
        {
            return MarkObject(UnityEngine.Object.Instantiate(TypedContent, parent, worldPositionStays));
        }
        #endregion
    }
}
