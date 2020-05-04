// -BEGIN_PRATEEK_COPYRIGHT-
// -END_PRATEEK_COPYRIGHT-

// -BEGIN_PRATEEK_CSHARP_IFDEF-
// -END_PRATEEK_CSHARP_IFDEF-

namespace Prateek.CodeGeneration.Code.PrateekScript.ScriptActions
{
    using Prateek.CodeGeneration.Code.PrateekScript.CodeGeneration;
    using Prateek.CodeGeneration.CodeBuilder.Editor.ScriptTemplates;
    using Prateek.Core.Code.Helpers;
    using UnityEditor;

    [InitializeOnLoad]
    internal class NotepadPlusSyntaxTemplateLoader
    {
        #region Constructors
        static NotepadPlusSyntaxTemplateLoader()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                return;
            }

            ScriptFileTemplate.Create(Glossary.importExtension.Extension("xml"), "xml")
                .SetAutorun(false)
                .SetEndsWith("SyntaxAutoComplete")
                .SetTemplateFile(string.Empty)
                .Load("InternalContent_PrateekCodegenSyntaxAutoComplete.xml.txt")
                .Commit();

            ScriptFileTemplate.Create(Glossary.importExtension.Extension("xml"), "xml")
                .SetAutorun(false)
                .SetTemplateFile(string.Empty)
                .SetEndsWith("SyntaxColor")
                .Load("InternalContent_PrateekCodegenSyntaxColor.xml.txt")
                .Commit();
        }
        #endregion
    }
}
