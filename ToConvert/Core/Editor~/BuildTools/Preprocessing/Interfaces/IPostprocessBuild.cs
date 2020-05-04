namespace Mayfair.Core.Editor.BuildTools.Preprocessing.Interfaces
{
    using UnityEditor.Build.Reporting;

    public interface IPostprocessBuild : IOrderedCallback
    {
        void OnPostprocessBuild(BuildSummary buildSummary);
    }
}