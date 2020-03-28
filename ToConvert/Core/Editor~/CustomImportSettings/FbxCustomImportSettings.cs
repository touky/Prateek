namespace Mayfair.Core.Editor.CustomImportSettings
{
    using UnityEditor;
    using UnityEngine;

    public class FbxCustomImportSettings : ScriptableObject
    {
        #region Fields
        [Header("Geometry")]
        public ModelImporterTangents importTangents = ModelImporterTangents.None;
        #endregion
    }
}
