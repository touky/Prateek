namespace Prateek.Editor.CodeGeneration.PrateekScript.RuntimeGeneration
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
