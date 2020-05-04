namespace Mayfair.Core.Code.Utils
{
    using System;
    using Mayfair.Core.Code.Rendering;
    using UnityEngine;

    public static class ConstsShader
    {
        #region Static and Constants
        public static readonly ShaderProperty COLOR_PROP = "_Color";

        public static readonly ShaderProperty SHADER_PLACEMENT_COLOR = "_PlacementOverlayColor";
        public static readonly ShaderProperty SHADER_PLACEMENT_ALPHA = "PlacementOverlayAlpha";
        #endregion
    }

    public class DirtyContainer<T> : IDisposable
        where T : class, new()
    {
        private bool isDirty = false;
        private T value;

        /// <summary>
        /// Auto-inits the content if not init
        /// Returns the content and mark itself dirty
        /// </summary>
        public T Content
        {
            get
            {
                isDirty = true;

                Init();

                return value;
            }
        }

        /// <summary>
        /// Auto-inits the content if not init
        /// Returns the content without marking itself dirty
        /// </summary>
        public T ContentWithoutDirtying
        {
            get
            {
                Init();

                return value;
            }
        }

        /// <summary>
        /// Returns if content is dirty
        /// </summary>
        public bool ContentIsDirty
        {
            get { return isDirty; }
        }

        /// <summary>
        /// Cleans the dirty status
        /// </summary>
        public void Cleanup()
        {
            isDirty = false;
        }

        /// <summary>
        /// Disposes of the content, override for custom dispose behaviour
        /// </summary>
        public virtual void Dispose()
        {
            value = null;
        }

        protected virtual void Init()
        {
            if (value == null)
            {
                isDirty = true;
                value = new T();
            }
        }
    }
}
