// -BEGIN_PRATEEK_COPYRIGHT-
// -END_PRATEEK_COPYRIGHT-

// -BEGIN_PRATEEK_CSHARP_IFDEF-
// -END_PRATEEK_CSHARP_IFDEF-

namespace Prateek.CodeGenerator.PrateekScriptBuilder {
    using System;
    using System.Collections.Generic;

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
        protected override void GatherVariants(List<FuncVariant> variants, PrateekScriptBuilder.CodeFile.ContentInfos data, PrateekScriptBuilder.CodeFile.ClassInfos infoSrc, PrateekScriptBuilder.CodeFile.ClassInfos infoDst)
        {
            infos.Clear();
            PrateekScriptBuilder.Tag.Macro.GetTags(this);

            variants.Clear();
            if (data.funcInfos.Count == 0)
                return;

            var result  = String.Empty;
            var variant = new FuncVariant(result);
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