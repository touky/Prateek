namespace Mayfair.Core.Editor.BuildTools.Preprocessing.Interfaces
{
    using UnityEditor;

    public interface IPreprocessBuild : IOrderedCallback
    {
        void OnPreprocessBuild(BuildTarget buildTarget);
    }
}