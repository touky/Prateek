// -BEGIN_PRATEEK_COPYRIGHT-
// -END_PRATEEK_COPYRIGHT-

// -BEGIN_PRATEEK_CSHARP_IFDEF-
// -END_PRATEEK_CSHARP_IFDEF-

namespace Prateek.CodeGeneration.Code.PrateekScript.ScriptActions
{
    using System.Collections.Generic;
    using Prateek.CodeGeneration.Code.PrateekScript.CodeGeneration;

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
        protected override void GatherVariants(List<FunctionVariant> variants, ScriptContent data, ClassContent contentSrc, ClassContent contentDst)
        {
            infos.Clear();
            Glossary.Macros.GetTags(this);

            variants.Clear();
            if (data.functionContents.Count == 0)
            {
                return;
            }

            var result  = string.Empty;
            var variant = new FunctionVariant(result);
            for (var k = 0; k < infos.Count; k++)
            {
                result += data.functionContents[0].data;
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
