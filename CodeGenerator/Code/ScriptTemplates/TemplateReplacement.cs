// -BEGIN_PRATEEK_COPYRIGHT-
//
//  Prateek, a library that is "bien pratique"
//  Header last update date: 22/03/2020
//
//  Copyright � 2017-2020 "Touky" <touky@prateek.top>
//
//  Prateek is free software. It comes without any warranty, to
//  the extent permitted by applicable law. You can redistribute it
//  and/or modify it under the terms of the Do What the Fuck You Want
//  to Public License, Version 2, as published by the WTFPL Task Force.
//  See http://www.wtfpl.net/ for more details.
//
// -END_PRATEEK_COPYRIGHT-

// -BEGIN_PRATEEK_CSHARP_IFDEF-
//-----------------------------------------------------------------------------
#region Prateek Ifdefs

//Auto activate some of the prateek defines
#if UNITY_EDITOR

//Auto activate debug
#if !PRATEEK_DEBUG
#define PRATEEK_DEBUG
#endif //!PRATEEK_DEBUG

#endif //UNITY_EDITOR && !PRATEEK_DEBUG

#endregion Prateek Ifdefs
// -END_PRATEEK_CSHARP_IFDEF-

//-----------------------------------------------------------------------------
namespace Prateek.CodeGenerator.ScriptTemplates
{
    using System.Collections.Generic;
    using Prateek.Core.Code.Helpers.Files;
    using UnityEngine;

    //-------------------------------------------------------------------------
    public partial class ScriptTemplate
    {
        //---------------------------------------------------------------------
        public abstract class BaseTemplate : FileHelpers.IExtensionMatcher
        {
            //-----------------------------------------------------------------
            protected string extension;
            private string contentPath = string.Empty;
            private string content = string.Empty;

            //-----------------------------------------------------------------
            public string Extension { get { return extension; } }
            public string Content
            {
                get
                {
                    if (content == string.Empty && contentPath != string.Empty)
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
                return SetContent(string.Empty);
            }

            //-----------------------------------------------------------------
            public virtual bool Match(string fileName, string extension, string content)
            {
                if (this.extension == string.Empty || this.extension == extension)
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
}
