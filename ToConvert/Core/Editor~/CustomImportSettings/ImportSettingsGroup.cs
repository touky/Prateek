namespace Mayfair.Core.Editor.CustomImportSettings
{
    using System;
    using Mayfair.Core.Editor.Animation;
    using UnityEngine;

    [Serializable]
    public struct ImportSettingsGroup
    {
        [SerializeField]
        public FbxCustomImportSettings fbx;

        [SerializeField]
        public TextureCustomImportSettings texture;

        [SerializeField]
        public AnimationCustomImportSettings animation;
    }
}
