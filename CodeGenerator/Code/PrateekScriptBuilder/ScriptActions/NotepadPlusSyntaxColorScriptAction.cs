// -BEGIN_PRATEEK_COPYRIGHT-
// -END_PRATEEK_COPYRIGHT-

// -BEGIN_PRATEEK_CSHARP_IFDEF-
// -END_PRATEEK_CSHARP_IFDEF-

namespace Prateek.CodeGenerator.PrateekScriptBuilder {
    using System;
    using System.Collections.Generic;
    using Assets.Prateek.CodeGenerator.Code.PrateekScriptBuilder.CodeGeneration;

    public class NotepadPlusSyntaxColorScriptAction : SyntaxScriptAction
    {
        public static NotepadPlusSyntaxColorScriptAction Create(string extension)
        {
            return new NotepadPlusSyntaxColorScriptAction(extension);
        }

        //-----------------------------------------------------------------
        private List<string> keywords = new List<string>();
        private List<string> identifiers = new List<string>();

        //-----------------------------------------------------------------
        public override string ScopeTag { get { return "SYNTAX_NPP_COLOR"; } }
        public override GenerationMode GenMode { get { return GenerationMode.ForeachSrc; } }
        public override bool GenerateDefault { get { return false; } }

        //-----------------------------------------------------------------
        public NotepadPlusSyntaxColorScriptAction(string extension) : base(extension) { }

        //-----------------------------------------------------------------
        #region CodeRule override
        public override void AddKeyword(string content)
        {
            keywords.Add(content);
        }

        //-----------------------------------------------------------------
        public override void AddIdentifier(string content)
        {
            identifiers.Add(content);
        }
        #endregion CodeRule override

        //-----------------------------------------------------------------
        #region Rule internal
        protected override void GatherVariants(List<FunctionVariant> variants, PrateekScriptBuilder.CodeFile.ContentInfos data, PrateekScriptBuilder.CodeFile.ClassInfos infoSrc, PrateekScriptBuilder.CodeFile.ClassInfos infoDst)
        {
            keywords.Clear();
            identifiers.Clear();
            PrateekScriptBuilder.Tag.Macro.GetTags(this);

            variants.Clear();
            if (data.funcInfos.Count == 0)
                return;

            var result = String.Empty;
            for (int k = 0; k < keywords.Count; k++)
            {
                result += data.funcInfos[0].data;
                result = (Vars[0] + keywords[k]).Apply(result);
            }

            var variant = new FunctionVariant(result, 1);
            result = String.Empty;
            for (int i = 0; i < identifiers.Count; i++)
            {
                result += data.funcInfos[0].data;
                result = (Vars[0] + identifiers[i]).Apply(result);
            }
            variant[1] = result;
            variants.Add(variant);
        }
        #endregion Rule internal
    }
}