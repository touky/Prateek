namespace Mayfair.Core.Editor.EditorWindows.ShaderUsageFinder
{
    using Mayfair.Core.Editor.EditorWindows.ShaderUsageFinder.Enums;
    using UnityEngine;

    public struct SearchResult
    {
        public SearchValidation validation;
        public Material instance;
        public string shader;
        public string material;
        public string location;
    }
}
