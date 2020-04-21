namespace Prateek.CodeGenerator.ScriptTemplates {
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Prateek.Core.Code.Helpers;
    using Prateek.Core.Code.Helpers.Files;
    using UnityEngine;

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
            TemplateRegistry.Add(this);
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
        public override BaseTemplate LoadFile(string filePath)
        {
            base.LoadFile(filePath);

            filePath = FileHelpers.GetValidFile(filePath);
            if (string.IsNullOrEmpty(filePath))
            {
                return this;
            }

            var fileInfo = new FileInfo(filePath);
            if (!fileInfo.Exists)
            {
                return this;
            }

            var lastTime = fileInfo.LastWriteTime.ToFileTime();
            if (lastTime <= lastWriteTime)
            {
                return this;
            }

            this.fullName = Path.GetFileName(fileInfo.Name);
            this.fileName = Path.GetFileNameWithoutExtension(fileInfo.Name);
            this.extension = fileInfo.Extension.TrimStart(Strings.Separator.FileExtension.C());

            parts = new List<string>(Content.Split(tags, StringSplitOptions.RemoveEmptyEntries));
            return this;
        }
    }
}