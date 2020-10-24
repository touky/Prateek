namespace Prateek.Editor.AtomicFileFramework.Yaml
{
    using System.IO;
    using System.Text.RegularExpressions;
    using UnityEditor.SceneManagement;
    using UnityEngine.SceneManagement;

    public class YamlSceneAtomicFormat : AtomicFileFormat
    {
        #region Static and Constants
        private const int NON_SCENE_UID_LENGTH = 4;
        private const int TYPE_ID = -2;
        private const int GUID = -1;
        private static Regex yamlStartRegex = new Regex("--- !u!([0-9]*)\\s*&([0-9]*)");
        #endregion

        #region Properties
        public override string OriginalExtension { get { return ".unity"; } }

        public override Regex PartStartRegex { get { return yamlStartRegex; } }
        #endregion

        #region Class Methods
        public override void Init()
        {
            EditorSceneManager.sceneSaved += OnSceneSaved;
        }

        public override string FormatFilename(string originalName, Match match)
        {
            var guid   = match.Groups[match.Groups.Count + GUID].Value;
            var typeid = match.Groups[match.Groups.Count + TYPE_ID].Value;
            if (YamlClassReference.references.ContainsKey(typeid))
            {
                typeid = YamlClassReference.references[typeid];
            }

            var extension = guid.Length > NON_SCENE_UID_LENGTH ? PartExtension : HeaderExtension;
            var filename  = $"{Path.GetFileNameWithoutExtension(originalName)}_{guid}_{typeid}{extension}";
            return filename;
        }

        private static void OnSceneSaved(Scene scene)
        {
            ExportFile(scene.path);
        }
        #endregion
    }
}
