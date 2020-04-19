// -BEGIN_PRATEEK_COPYRIGHT-
// -END_PRATEEK_COPYRIGHT-

// -BEGIN_PRATEEK_CSHARP_IFDEF-
// -END_PRATEEK_CSHARP_IFDEF-

namespace Prateek.CodeGenerator.PrateekScriptBuilder {
    using System;
    using System.Collections.Generic;
    using Assets.Prateek.CodeGenerator.Code.PrateekScript.CodeGeneration;
    using Assets.Prateek.CodeGenerator.Code.PrateekScriptBuilder.CodeGeneration;

    public partial class NotepadPlusSyntaxAutoCompleteScriptAction : SyntaxScriptAction
    {
        public static NotepadPlusSyntaxAutoCompleteScriptAction Create(string extension)
        {
            return new NotepadPlusSyntaxAutoCompleteScriptAction(extension);
        }

        //-----------------------------------------------------------------
        public struct KeywordInfo
        {
            public string name;
        }

        //-----------------------------------------------------------------
        public List<KeywordInfo> infos = new List<KeywordInfo>();

        //-----------------------------------------------------------------
        public override string ScopeTag { get { return "SYNTAX_NPP_AUTO_COMPLETE"; } }
        public override GenerationMode GenMode { get { return GenerationMode.ForeachSrc; } }
        public override bool GenerateDefault { get { return false; } }

        //-----------------------------------------------------------------
        public NotepadPlusSyntaxAutoCompleteScriptAction(string extension) : base(extension) { }

        //-----------------------------------------------------------------
        #region CodeRule override
        public override void AddKeyword(string content)
        {
            infos.Add(new KeywordInfo() { name = content });
        }

        //-----------------------------------------------------------------
        public override void AddIdentifier(string content)
        {
            infos.Add(new KeywordInfo() { name = content });
        }
        #endregion CodeRule override

        //-----------------------------------------------------------------
        #region Rule internal
        protected override void GatherVariants(List<FunctionVariant> variants, ContentInfos data, ClassInfos infoSrc, ClassInfos infoDst)
        {
            infos.Clear();
            Glossary.Macro.GetTags(this);

            variants.Clear();
            if (data.funcInfos.Count == 0)
                return;

            var result  = String.Empty;
            var variant = new FunctionVariant(result);
            for (int k = 0; k < infos.Count; k++)
            {
                result += data.funcInfos[0].data;
                result = (Vars[0] + infos[k].name).Apply(result);
            }
            variant[0] = result;
            variants.Add(variant);
        }
        #endregion Rule internal
    }
}