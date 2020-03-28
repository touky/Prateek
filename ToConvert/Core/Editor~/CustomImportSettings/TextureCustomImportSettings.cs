namespace Mayfair.Core.Editor.CustomImportSettings
{
    using UnityEngine;

    public class TextureCustomImportSettings : ScriptableObject
    {
        #region Fields
        [Header("Mipmaps")]
        public bool streamingMipmaps = true;
        #endregion
    }
}
