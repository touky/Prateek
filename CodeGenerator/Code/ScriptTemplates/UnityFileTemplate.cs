namespace Prateek.CodeGenerator.ScriptTemplates {
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Prateek.Core.Code.Helpers;
    using Prateek.Core.Code.Helpers.Files;

    public class UnityFileTemplate : BaseTemplate
    {
        public static UnityFileTemplate Create(string extension)
        {
            return new UnityFileTemplate(extension);
        }

        //-----------------------------------------------------------------
        protected static string[] tags = new string[2] { "#SCRIPTNAME#", "#NOTRIM#" };
        protected string fullName;
        protected string fileName;
        protected List<string> parts;

        //-----------------------------------------------------------------
        public string FullName { get { return fullName; } }
        public string FileName { get { return fileName; } }

        //-----------------------------------------------------------------
        public UnityFileTemplate(string extension) : base(extension) { }

        //-----------------------------------------------------------------
        public override void Commit()
        {
            CodeGenerator.TemplateRegistry.Add(this);
        }

        //-----------------------------------------------------------------
        public override bool Match(string fileName, string extension, string content)
        {
            if (!base.Match(fileName, extension, content))
                return false;

            var start = 0;
            for (int p = 0; p < parts.Count; p++)
            {
                var index = content.IndexOf(parts[p], start);
                if (index < start)
                    return false;

                start = index;
            }

            return true;
        }

        //-----------------------------------------------------------------
        public UnityFileTemplate Load(string path)
        {
            if (!File.Exists(path))
                return this;

            var lastSlash = path.LastIndexOf(Strings.Separator.DirSlash.C());
            if (lastSlash < 0)
                return this;

            var ext0 = path.LastIndexOf(Strings.Separator.FileExtension.C());
            if (ext0 < 0)
                return this;

            var ext1 = path.LastIndexOf(Strings.Separator.FileExtension.C(), ext0 - 1);
            if (ext1 < 0)
                return this;

            this.fullName = path.Substring(lastSlash + 1, path.Length - (lastSlash + 1));
            this.fileName = path.Substring(0, ext1);
            this.extension = path.Substring(ext1 + 1, (ext0 - ext1) - 1);

            SetContent(FileHelpers.ReadAllTextCleaned(path));
            parts = new List<string>(Content.Split(tags, StringSplitOptions.RemoveEmptyEntries));
            return this;
        }
    }
}