namespace Assets.Prateek.CodeGenerator.Code.PrateekScript.CodeGeneration
{
    using UnityEditor;

    [InitializeOnLoad]
    internal class GlossaryLoader
    {
        #region Constructors
        static GlossaryLoader()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                return;
            }

            Glossary.Macros.Init();
        }
        #endregion
    }
}
