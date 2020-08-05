namespace Prateek.Runtime.Core.HierarchicalTree
{
    using System.Text.RegularExpressions;
    using Prateek.Runtime.Core.FrameworkSettings;
    using UnityEngine;

    /// <summary>
    /// Defines the settings used by a hierarchical tree to sort datas
    /// </summary>
    public class HierarchicalTreeSettingsData : FrameworkSettingsData
    {
        [SerializeField]
        internal Regex folderRegex = new Regex("([^\\\\/]+)(?:[\\\\/]*)");
    }
}
