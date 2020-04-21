namespace Prateek.CodeGenerator.ScriptTemplates
{
    using UnityEditor;

    [InitializeOnLoad]
    class CSharpIgnorableLoader
    {
        #region Constructors
        static CSharpIgnorableLoader()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                return;
            }

            CSharpIgnorableTemplate.Create("cs").Commit();
        }
        #endregion
    }
}
