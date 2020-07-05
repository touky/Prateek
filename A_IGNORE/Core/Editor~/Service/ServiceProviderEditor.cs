namespace Mayfair.Core.Editor.Service
{
    using Mayfair.Core.Code.Service;
    using UnityEditor;

    [CustomEditor(typeof(ServiceProviderBehaviour), true)]
    public class ServiceProviderEditor : Editor
    {
        #region Static and Constants
        private const string CLASS_NAME = "ServiceProvider";
        private const string READABLE_NAME = "ProviderFor";
        #endregion

        #region Class Methods
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            ServiceProviderBehaviour provider = target as ServiceProviderBehaviour;
            ServiceProviderBehaviour[] providers = provider.GetComponents<ServiceProviderBehaviour>();

            if (providers.Length > 1 || PrefabUtility.IsPartOfPrefabAsset(target))
            {
                return;
            }

            RefreshCustomName();
        }

        protected virtual string BuildCustomName()
        {
            string name = target.GetType().Name;
            name = name.Replace(CLASS_NAME, string.Empty);

            string targetName = $@"{READABLE_NAME}_{name}";

            return targetName;
        }

        private void RefreshCustomName()
        {
            string targetName = BuildCustomName();

            if (target.name != targetName)
            {
                target.name = targetName;

                EditorUtility.SetDirty(target);
            }
        }
        #endregion
    }
}
