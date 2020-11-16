namespace Prateek.Runtime.AppContentFramework.Unity.Handles
{
    using System.Diagnostics;
    using Prateek.Runtime.AppContentFramework.ContentLoaders;
    using UnityEngine;
    using UnityEngine.Assertions;

    [DebuggerDisplay("{typeof(TContentType).Name}, {loader.content.ToString()}, Location: {loader.path}")]
    public class ComponentHandle<TContentType>
        : GameObjectHandle
        where TContentType : Component
    {
        #region Fields
        private TContentType component;
        #endregion

        #region Properties
        public TContentType Component { get { return component; } }
        #endregion

        #region Constructors
        public ComponentHandle() : base() { }
        #endregion

        #region Class Methods
        protected override void RetrieveContent()
        {
            base.RetrieveContent();

            component = content.GetComponent<TContentType>();

            Assert.IsNotNull(component);
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
        public new TContentType Instantiate(Vector3 position, Quaternion rotation)
        {
            return base.Instantiate(position, rotation).GetComponent<TContentType>();
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
        public new TContentType Instantiate(Vector3 position, Quaternion rotation, Transform parent)
        {
            return base.Instantiate(position, rotation, parent).GetComponent<TContentType>();
        }

        /// <summary>
        ///     <para>Clones the original resource and returns the clone.</para>
        /// </summary>
        /// <returns>
        ///     <para>The instantiated clone.</para>
        /// </returns>
        public new TContentType Instantiate()
        {
            return base.Instantiate().GetComponent<TContentType>();
        }

        /// <summary>
        ///     <para>Clones the original resource and returns the clone.</para>
        /// </summary>
        /// <param name="parent">Parent that will be assigned to the new object.</param>
        /// <returns>
        ///     <para>The instantiated clone.</para>
        /// </returns>
        public new TContentType Instantiate(Transform parent)
        {
            return base.Instantiate(parent).GetComponent<TContentType>();
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
        public new TContentType Instantiate(Transform parent, bool worldPositionStays)
        {
            return base.Instantiate(parent, worldPositionStays).GetComponent<TContentType>();
        }
        #endregion
    }
}
