namespace Mayfair.Core.Editor.EditorWindows.ShaderUsageFinder.ShaderFinderParameter
{
    using UnityEngine;

    public class InstancingSetAllParameter : InstancingParameter
    {
        #region Fields
        private bool value;
        #endregion

        #region Constructors
        public InstancingSetAllParameter(GUIStyle[] buttonStyle)
            : base(string.Empty, buttonStyle) { }
        #endregion

        #region Class Methods
        public void Set(bool value)
        {
            this.value = value;
        }

        protected override void InternalDoAction(SearchResult result)
        {
            Material material = result.instance;
            material.enableInstancing = value;
        }
        #endregion
    }
}
