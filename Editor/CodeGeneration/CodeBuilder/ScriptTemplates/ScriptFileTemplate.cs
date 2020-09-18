namespace Prateek.Editor.CodeGeneration.CodeBuilder.ScriptTemplates {
    using System;
    using Prateek.Editor.CodeGeneration.CodeBuilder.RuntimeBuilder;

    public class ScriptFileTemplate : BaseTemplate
    {
        public static ScriptFileTemplate Create(string extension) { return Create(extension, extension); }
        public static ScriptFileTemplate Create(string extension, string exportExtension)
        {
            return new ScriptFileTemplate(extension, exportExtension);
        }

        ///-----------------------------------------------------------------
        private bool allowAutorun = true;
        private string nameEndsWith = String.Empty;
        private string exportExtension = String.Empty;
        private string templateFile = String.Empty;

        ///-----------------------------------------------------------------
        public string ExportExtension { get { return exportExtension; } }
        public bool AllowAutorun { get { return allowAutorun; } }

        ///-----------------------------------------------------------------
        public ScriptFileTemplate(string extension, string exportExtension) : base(extension)
        {
            this.exportExtension = exportExtension;
        }

        ///-----------------------------------------------------------------
        public ScriptFileTemplate SetAutorun(bool enable)
        {
            allowAutorun = enable;
            return this;
        }

        ///-----------------------------------------------------------------
        public ScriptFileTemplate SetEndsWith(string endsWith)
        {
            this.nameEndsWith = endsWith;
            return this;
        }

        ///-----------------------------------------------------------------
        public ScriptFileTemplate SetTemplateFile(string file)
        {
            this.templateFile = file;
            return this;
        }

        ///-----------------------------------------------------------------
        public override void Commit()
        {
            TemplateRegistry.Add(this);
        }

        ///-----------------------------------------------------------------
        public override bool Match(string fileName, string extension, string content)
        {
            if (!base.Match(fileName, extension, content))
                return false;

#if UNITY_EDITOR
            if (nameEndsWith != String.Empty && !fileName.EndsWith(nameEndsWith))
                return false;

            if (templateFile != String.Empty)
                return TemplateRegistry.MatchTemplate(templateFile, extension, content);
            return true;
#else //!UNITY_EDITOR
                return false;
#endif //UNITY_EDITOR
        }
    }
}