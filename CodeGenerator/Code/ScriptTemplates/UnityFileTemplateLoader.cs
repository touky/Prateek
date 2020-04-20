namespace Prateek.CodeGenerator.ScriptTemplates {
    using System.IO;
    using Prateek.Core.Code.Helpers.Files;
    using UnityEditor;

    ///todo: fix that
    ///todo [InitializeOnLoad]
    class UnityFileTemplateLoader
    {
        static UnityFileTemplateLoader()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
                return;

            var path = FileHelpers.GetScriptTemplateFolder();
            if (path == string.Empty)
                return;

            var files = Directory.GetFiles(path);
            for (int f = 0; f < files.Length; f++)
            {
                if (!files[f].EndsWith(".txt"))
                    continue;

                UnityFileTemplate.Create("txt").Load(files[f]).Commit();
            }
        }
    }
}