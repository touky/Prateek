namespace Prateek.Runtime.AppContentFramework.ContentAccess.Interfaces
{
    using Prateek.Runtime.Core.HierarchicalTree;

    public interface IContentAccessSettings
    {
        string[] ContentPaths { get; }
        string[] ContentExtensions { get; }
        HierarchicalTreeSettingsData Settings { get; }
    }
}