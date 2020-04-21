namespace Prateek.CodeGenerator.ScriptTemplates {
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Prateek.Core.Code.Helpers.Files;
    using UnityEngine;

    public abstract class BaseTemplate : FileHelpers.IExtensionMatcher
    {
        //-----------------------------------------------------------------
        protected string extension;
        private string contentPath = String.Empty;
        private string content = String.Empty;
        protected long lastWriteTime = 0;

        //-----------------------------------------------------------------
        public string Extension { get { return extension; } }
        public string Content
        {
            get
            {
                return content;
            }
            protected set { content = value; }
        }

        //-----------------------------------------------------------------
        protected BaseTemplate(string extension)
        {
            this.extension = extension;
        }

        //-----------------------------------------------------------------
        public virtual BaseTemplate SetContent(string content)
        {
            this.content = content;
            return this;
        }

        //-----------------------------------------------------------------
        public TemplateLoadJob Load(string filePath)
        {
            FileHelpers.InitIO();

            return new TemplateLoadJob()
            {
                contentPath = filePath,
                template = this
            };
        }

        //-----------------------------------------------------------------
        public virtual BaseTemplate LoadFile(string filePath)
        {
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

            lastWriteTime = fileInfo.LastWriteTime.ToFileTime();
            contentPath = filePath;
            return SetContent(FileHelpers.ReadAllTextCleaned(contentPath));
        }

        //-----------------------------------------------------------------
        public virtual bool Match(string fileName, string extension, string content)
        {
            if (this.extension == String.Empty || this.extension == extension)
                return true;
            return false;
        }

        //-----------------------------------------------------------------
        public abstract void Commit();
    }
}
