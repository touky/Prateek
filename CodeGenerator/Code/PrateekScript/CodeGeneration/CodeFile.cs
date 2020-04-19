namespace Assets.Prateek.CodeGenerator.Code.PrateekScript.CodeGeneration {
    using System;
    using System.Collections.Generic;
    using Assets.Prateek.CodeGenerator.Code.Utils;
    using global::Prateek.CodeGenerator.PrateekScriptBuilder;
    using global::Prateek.Core.Code.Helpers;

    public class CodeFile
    {
        //-----------------------------------------------------------------

        //-----------------------------------------------------------------

        //-----------------------------------------------------------------

        //-----------------------------------------------------------------
        public string fileName;
        public string fileExtension;
        public string fileNamespace;

        //-----------------------------------------------------------------
        private ContentInfos codeInfos;
        private string codeGenerated;
        private List<ContentInfos> datas = new List<ContentInfos>();

        //-----------------------------------------------------------------
        public string CodeGenerated { get { return codeGenerated; } }
        public ContentInfos CodeInfos { get { return codeInfos; } }
        public int DataCount { get { return datas.Count; } }
        public ContentInfos this[int index] { get { return datas[index]; } }

        //-----------------------------------------------------------------
        public bool AllowRule(ScriptAction rule)
        {
            if (codeInfos == null)
                return true;

            if (codeInfos.activeRule == null)
                return true;

            return codeInfos.activeRule == rule;
        }

        //-----------------------------------------------------------------
        public ContentInfos NewData(ScriptAction codeSettings)
        {
            if (codeInfos != null)
                return null;
            codeInfos = new ContentInfos() { activeRule = codeSettings };
            return codeInfos;
        }

        //-----------------------------------------------------------------
        public bool Commit()
        {
            var hasSubmitted = codeInfos != null;
            if (codeInfos != null)
                datas.Add(codeInfos);
            codeInfos = null;

            return hasSubmitted;
        }

        //-----------------------------------------------------------------
        public global::Prateek.CodeGenerator.CodeBuilder.BuildResult Generate(string genHeader, string genCode)
        {
            var genNSpc = (StringSwap)global::Assets.Prateek.CodeGenerator.Code.PrateekScript.CodeGeneration.Glossary.Macro.codeGenNSpc.Keyword();
            var genExtn = (StringSwap)global::Assets.Prateek.CodeGenerator.Code.PrateekScript.CodeGeneration.Glossary.Macro.codeGenExtn.Keyword();
            var genPrfx = (StringSwap)global::Assets.Prateek.CodeGenerator.Code.PrateekScript.CodeGeneration.Glossary.Macro.codeGenPrfx.Keyword();
            var genData = (StringSwap)global::Assets.Prateek.CodeGenerator.Code.PrateekScript.CodeGeneration.Glossary.Macro.codeGenData.Keyword();
            var genTabs = (StringSwap)global::Assets.Prateek.CodeGenerator.Code.PrateekScript.CodeGeneration.Glossary.Macro.codeGenTabs.Keyword();

            var i = genCode.IndexOf(genData.Original);
            if (i < 0)
                return global::Prateek.CodeGenerator.CodeBuilder.BuildResult.ValueType.PrateekScriptSourceDataTagInvalid;

            var r = genCode.LastIndexOf(Strings.Separator.LineFeed.C(), i);
            if (r >= 0)
                genTabs = genTabs + genCode.Substring(r + 1, i - (r + 1));

            codeGenerated = genHeader;
            for (int d = 0; d < datas.Count; d++)
            {
                var data = datas[d];

                var result = data.activeRule.Generate(data);
                if (!result)
                {
                    return result;
                }

                var code = genTabs.Apply(data.codeGenerated);
                genNSpc += data.blockNamespace;
                genExtn += data.blockClassName;
                var prefix = String.Empty;
                for (int p = 0; p < data.blockClassPrefix.Count; p++)
                {
                    genPrfx += data.blockClassPrefix[p] + Strings.Separator.Space.S();
                }
                genData += code;
                codeGenerated += genPrfx.Apply(genExtn.Apply(genData.Apply(genNSpc.Apply(genCode))));
            }

            return global::Prateek.CodeGenerator.CodeBuilder.BuildResult.ValueType.Success;
        }
    }
}