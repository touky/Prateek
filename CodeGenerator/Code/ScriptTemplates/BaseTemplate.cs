namespace Prateek.CodeGenerator.ScriptTemplates {
    using System;
    using System.Collections.Generic;
    using Prateek.Core.Code.Helpers.Files;
    using UnityEngine;

    public abstract class BaseTemplate : FileHelpers.IExtensionMatcher
    {
        //-----------------------------------------------------------------
        protected string extension;
        private string contentPath = String.Empty;
        private string content = String.Empty;

        //-----------------------------------------------------------------
        public string Extension { get { return extension; } }
        public string Content
        {
            get
            {
                if (content == String.Empty && contentPath != String.Empty)
                {
                    DoLoadContent();
                }
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
        public virtual BaseTemplate SetFileContent(string filePath)
        {
            contentPath = filePath;
            return SetContent(String.Empty);
        }

        //-----------------------------------------------------------------
        public virtual bool Match(string fileName, string extension, string content)
        {
            if (this.extension == String.Empty || this.extension == extension)
                return true;
            return false;
        }

        //-----------------------------------------------------------------
        protected void DoLoadContent()
        {
            var files = new List<string>();
            if (!FileHelpers.GatherFilesAt(Application.dataPath, files, "(" + contentPath + ")$", true))
                return;
            SetContent(FileHelpers.ReadAllTextCleaned(files[0]));
        }

        //-----------------------------------------------------------------
        public abstract void Commit();
    }
}