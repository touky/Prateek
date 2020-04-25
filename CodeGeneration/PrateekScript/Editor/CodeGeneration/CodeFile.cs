namespace Assets.Prateek.CodeGenerator.Code.PrateekScript.CodeGeneration
{
    using System.Collections.Generic;
    using Assets.Prateek.CodeGenerator.Code.PrateekScript.ScriptActions;
    using Assets.Prateek.CodeGenerator.Code.Utils;
    using global::Prateek.CodeGenerator;
    using global::Prateek.Core.Code.Helpers;

    public class CodeFile
    {
        #region Fields
        ///-----------------------------------------------------------------

        ///-----------------------------------------------------------------

        ///-----------------------------------------------------------------

        ///-----------------------------------------------------------------
        public string fileName;
        public string fileExtension;
        public string fileNamespace;

        ///-----------------------------------------------------------------
        private ScriptContent scriptContent;
        private string codeGenerated;
        private List<ScriptContent> datas = new List<ScriptContent>();
        #endregion

        #region Properties
        ///-----------------------------------------------------------------
        public string CodeGenerated
        {
            get { return codeGenerated; }
        }

        public ScriptContent ScriptContent
        {
            get { return scriptContent; }
        }

        public int DataCount
        {
            get { return datas.Count; }
        }

        public ScriptContent this[int index]
        {
            get { return datas[index]; }
        }
        #endregion

        #region Class Methods
        ///-----------------------------------------------------------------
        public bool AllowRule(ScriptAction rule)
        {
            if (scriptContent == null)
            {
                return true;
            }

            if (scriptContent.scriptAction == null)
            {
                return true;
            }

            return scriptContent.scriptAction == rule;
        }

        ///-----------------------------------------------------------------
        public ScriptContent NewData(ScriptAction codeSettings)
        {
            if (scriptContent != null)
            {
                return null;
            }

            scriptContent = new ScriptContent {scriptAction = codeSettings};
            return scriptContent;
        }

        ///-----------------------------------------------------------------
        public bool Commit()
        {
            var hasSubmitted = scriptContent != null;
            if (scriptContent != null)
            {
                datas.Add(scriptContent);
            }

            scriptContent = null;

            return hasSubmitted;
        }

        ///-----------------------------------------------------------------
        public global::Assets.Prateek.CodeGenerator.Code.CodeBuilder.BuildResult Generate(string genHeader, string genCode)
        {
            var genNSpc = (StringSwap) Glossary.Macro.codeGenNSpc.Keyword();
            var genExtn = (StringSwap) Glossary.Macro.codeGenExtn.Keyword();
            var genPrfx = (StringSwap) Glossary.Macro.codeGenPrfx.Keyword();
            var genData = (StringSwap) Glossary.Macro.codeGenData.Keyword();
            var genTabs = (StringSwap) Glossary.Macro.codeGenTabs.Keyword();

            var i = genCode.IndexOf(genData.Original);
            if (i < 0)
            {
                return global::Assets.Prateek.CodeGenerator.Code.CodeBuilder.BuildResult.ValueType.PrateekScriptSourceDataTagInvalid;
            }

            var r = genCode.LastIndexOf(Strings.Separator.LineFeed.C(), i);
            if (r >= 0)
            {
                genTabs = genTabs + genCode.Substring(r + 1, i - (r + 1));
            }

            codeGenerated = genHeader;
            for (var d = 0; d < datas.Count; d++)
            {
                var data = datas[d];

                var result = data.scriptAction.Generate(data);
                if (!result)
                {
                    return result;
                }

                var code = genTabs.Apply(data.codeGenerated);
                genNSpc += data.blockNamespace;
                genExtn += data.blockClassName;
                var prefix = string.Empty;
                for (var p = 0; p < data.blockClassPrefix.Count; p++)
                {
                    genPrfx += data.blockClassPrefix[p] + Strings.Separator.Space.S();
                }

                genData += code;
                codeGenerated += genPrfx.Apply(genExtn.Apply(genData.Apply(genNSpc.Apply(genCode))));
            }

            return global::Assets.Prateek.CodeGenerator.Code.CodeBuilder.BuildResult.ValueType.Success;
        }
        #endregion
    }
}
