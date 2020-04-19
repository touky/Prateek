namespace Assets.Prateek.CodeGenerator.Code.PrateekScript.CodeGeneration {
    using UnityEditor;

    [InitializeOnLoad]
    class TagLoader
    {
        static TagLoader()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
                return;

            Glossary.Macro.Init();
        }
    }
}