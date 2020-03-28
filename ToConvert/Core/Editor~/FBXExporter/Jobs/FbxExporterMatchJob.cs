namespace Mayfair.Core.Editor.FBXExporter.Jobs
{
    using System.Collections.Generic;
    using Mayfair.Core.Editor.ObjectCategorizing;
    using UnityEditor;
    using UnityEngine;

    public abstract class FbxExporterMatchJob : FbxExporterJob
    {
        #region Fields
        protected List<CategorizedInstance> instances = new List<CategorizedInstance>();
        #endregion

        #region Constructors
        protected FbxExporterMatchJob(string path) : base(path) { }
        #endregion

        #region Class Methods
        public override void PreExecute()
        {
            GameObject source = AssetDatabase.LoadAssetAtPath<GameObject>(this.sourcePath);

            GameObjectCategorizer.Gather(source, this.instances);
            GameObjectCategorizer.Identify(this.instances);
        }
        #endregion
    }
}
