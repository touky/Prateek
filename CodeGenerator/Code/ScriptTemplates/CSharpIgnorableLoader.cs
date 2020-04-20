namespace Prateek.CodeGenerator.ScriptTemplates
{
    using UnityEditor;

    ///todo: fix that
    ///todo [InitializeOnLoad]
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
