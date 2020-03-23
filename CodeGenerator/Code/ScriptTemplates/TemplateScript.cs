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
    //-------------------------------------------------------------------------
    public partial class ScriptTemplate
    {
        //---------------------------------------------------------------------
        protected static ScriptFile NewScript(string extension) { return NewScript(extension, extension); }
        protected static ScriptFile NewScript(string extension, string exportExtension)
        {
            return new ScriptFile(extension, exportExtension);
        }

        //---------------------------------------------------------------------
        public class ScriptFile : BaseTemplate
        {
            //-----------------------------------------------------------------
            private bool allowAutorun = true;
            private string nameEndsWith = string.Empty;
            private string exportExtension = string.Empty;
            private string templateFile = string.Empty;

            //-----------------------------------------------------------------
            public string ExportExtension { get { return exportExtension; } }
            public bool AllowAutorun { get { return allowAutorun; } }

            //-----------------------------------------------------------------
            public ScriptFile(string extension, string exportExtension) : base(extension)
            {
                this.exportExtension = exportExtension;
            }

            //-----------------------------------------------------------------
            public ScriptFile SetAutorun(bool enable)
            {
                allowAutorun = enable;
                return this;
            }

            //-----------------------------------------------------------------
            public ScriptFile SetEndsWith(string endsWith)
            {
                this.nameEndsWith = endsWith;
                return this;
            }

            //-----------------------------------------------------------------
            public ScriptFile SetTemplateFile(string file)
            {
                this.templateFile = file;
                return this;
            }

            //-----------------------------------------------------------------
            public override void Commit()
            {
                CodeGenerator.ScriptTemplate.Add(this);
            }

            //-----------------------------------------------------------------
            public override bool Match(string fileName, string extension, string content)
            {
                if (!base.Match(fileName, extension, content))
                    return false;

#if UNITY_EDITOR
                if (nameEndsWith != string.Empty && !fileName.EndsWith(nameEndsWith))
                    return false;

                if (templateFile != string.Empty)
                    return CodeGenerator.ScriptTemplate.MatchTemplate(templateFile, extension, content);
                return true;
#else //!UNITY_EDITOR
                return false;
#endif //UNITY_EDITOR
            }
        }
    }
}
