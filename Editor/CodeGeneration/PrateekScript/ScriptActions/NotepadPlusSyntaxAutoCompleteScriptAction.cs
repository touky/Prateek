// -BEGIN_PRATEEK_COPYRIGHT-
// -END_PRATEEK_COPYRIGHT-

// -BEGIN_PRATEEK_CSHARP_IFDEF-
// -END_PRATEEK_CSHARP_IFDEF-

namespace Prateek.Editor.CodeGeneration.PrateekScript.ScriptActions
{
    using System.Collections.Generic;
    using Prateek.Editor.CodeGeneration.PrateekScript.RuntimeGeneration;

    public partial class NotepadPlusSyntaxAutoCompleteScriptAction : SyntaxScriptAction
    {
        #region Fields
        ///-----------------------------------------------------------------
        public List<KeywordInfo> infos = new List<KeywordInfo>();
        #endregion

        #region Properties
        ///-----------------------------------------------------------------
        public override string ScopeTag
        {
            get { return "SYNTAX_NPP_AUTO_COMPLETE"; }
        }

        public override GenerationRule GenerationMode
        {
            get { return GenerationRule.ForeachSrc; }
        }

        public override bool GenerateDefault
        {
            get { return false; }
        }
        #endregion

        #region Constructors
        ///-----------------------------------------------------------------
        public NotepadPlusSyntaxAutoCompleteScriptAction(string extension) : base(extension) { }
        #endregion

        #region Class Methods
        public static NotepadPlusSyntaxAutoCompleteScriptAction Create(string extension)
        {
            return new NotepadPlusSyntaxAutoCompleteScriptAction(extension);
        }

        ///-----------------------------------------------------------------

        #region Rule internal
        protected override void GatherVariants(List<FunctionVariant> variants, ScriptContent scriptContent, ClassContent contentSrc, ClassContent contentDst)
        {
            infos.Clear();
            Glossary.Macros.GetTags(this);

            variants.Clear();
            if (scriptContent.functionContents.Count == 0)
            {
                return;
            }

            var result  = string.Empty;
            var variant = new FunctionVariant(result);
            for (var k = 0; k < infos.Count; k++)
            {
                result += scriptContent.functionContents[0].body;
                result = (Variables[0] + infos[k].name).Apply(result);
            }

            variant[0] = result;
            variants.Add(variant);
        }
        #endregion Rule internal
        #endregion

        #region Nested type: KeywordInfo
        ///-----------------------------------------------------------------
        public struct KeywordInfo
        {
            public string name;
        }
        #endregion

        ///-----------------------------------------------------------------

        #region CodeRule override
        public override void AddKeyword(string content)
        {
            infos.Add(new KeywordInfo {name = content});
        }

        ///-----------------------------------------------------------------
        public override void AddIdentifier(string content)
        {
            infos.Add(new KeywordInfo {name = content});
        }
        #endregion CodeRule override
    }
}
