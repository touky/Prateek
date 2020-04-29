// -BEGIN_PRATEEK_COPYRIGHT-
// -END_PRATEEK_COPYRIGHT-

// -BEGIN_PRATEEK_CSHARP_IFDEF-
// -END_PRATEEK_CSHARP_IFDEF-

namespace Assets.Prateek.CodeGenerator.Code.PrateekScript.ScriptActions
{
    using System.Collections.Generic;
    using Assets.Prateek.CodeGenerator.Code.PrateekScript.CodeGeneration;

    public class NotepadPlusSyntaxColorScriptAction : SyntaxScriptAction
    {
        #region Fields
        ///-----------------------------------------------------------------
        private List<string> keywords = new List<string>();
        private List<string> identifiers = new List<string>();
        #endregion

        #region Properties
        ///-----------------------------------------------------------------
        public override string ScopeTag
        {
            get { return "SYNTAX_NPP_COLOR"; }
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
        public NotepadPlusSyntaxColorScriptAction(string extension) : base(extension) { }
        #endregion

        #region Class Methods
        public static NotepadPlusSyntaxColorScriptAction Create(string extension)
        {
            return new NotepadPlusSyntaxColorScriptAction(extension);
        }

        ///-----------------------------------------------------------------

        #region Rule internal
        protected override void GatherVariants(List<FunctionVariant> variants, ScriptContent data, ClassContent contentSrc, ClassContent contentDst)
        {
            keywords.Clear();
            identifiers.Clear();
            Glossary.Macros.GetTags(this);

            variants.Clear();
            if (data.functionContents.Count == 0)
            {
                return;
            }

            var result = string.Empty;
            for (var k = 0; k < keywords.Count; k++)
            {
                result += data.functionContents[0].data;
                result = (Variables[0] + keywords[k]).Apply(result);
            }

            var variant = new FunctionVariant(result, 1);
            result = string.Empty;
            for (var i = 0; i < identifiers.Count; i++)
            {
                result += data.functionContents[0].data;
                result = (Variables[0] + identifiers[i]).Apply(result);
            }

            variant[1] = result;
            variants.Add(variant);
        }
        #endregion Rule internal
        #endregion

        ///-----------------------------------------------------------------

        #region CodeRule override
        public override void AddKeyword(string content)
        {
            keywords.Add(content);
        }

        ///-----------------------------------------------------------------
        public override void AddIdentifier(string content)
        {
            identifiers.Add(content);
        }
        #endregion CodeRule override
    }
}
