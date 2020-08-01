namespace Prateek.CodeGeneration.CodeBuilder.Editor.ScriptTemplates {
    using Prateek.Core.Code.Helpers.Files;
    using System.IO;
    using UnityEditor;

    [InitializeOnLoad]
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
