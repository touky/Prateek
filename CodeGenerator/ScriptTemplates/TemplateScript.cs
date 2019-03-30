// -BEGIN_PRATEEK_COPYRIGHT-
//
//  Prateek, a library that is "bien pratique"
//  Header last update date: 30/03/2019
//
//  Copyright � 2017-2019 "Touky" <touky@prateek.top>
//
//  Prateek is free software. It comes without any warranty, to
//  the extent permitted by applicable law. You can redistribute it
//  and/or modify it under the terms of the Do What the Fuck You Want
//  to Public License, Version 2, as published by the WTFPL Task Force.
//  See http://www.wtfpl.net/ for more details.
//
// -END_PRATEEK_COPYRIGHT-

// -BEGIN_PRATEEK_CSHARP_NAMESPACE-
//
#region C# Prateek Namespaces
#if UNITY_EDITOR && !PRATEEK_DEBUG
#define PRATEEK_DEBUG
#endif //UNITY_EDITOR && !PRATEEK_DEBUG

#region System
using System;
using System.Collections;
using System.Collections.Generic;
#endregion System

#region Unity
using Unity.Jobs;
using Unity.Collections;

#region Engine
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.Serialization;

#if UNITY_PROFILING
using UnityEngine.Profiling;
#endif //UNITY_PROFILING
#endregion Engine

#region Editor
#if UNITY_EDITOR
using UnityEditor;
#endif //UNITY_EDITOR
#endregion Editor
#endregion Unity

#region Prateek
using Prateek;
using Prateek.Base;
using Prateek.Extensions;
using Prateek.Helpers;
using Prateek.Attributes;
using Prateek.Manager;

#region Using static
using static Prateek.ShaderTo.CSharp;
#endregion Using static

#region Editor
#if UNITY_EDITOR
using Prateek.CodeGeneration;
#endif //UNITY_EDITOR
#endregion Editor

#if PRATEEK_DEBUGS
using Prateek.Debug;
using static Prateek.Debug.Draw.Setup.QuickCTor;
#endif //PRATEEK_DEBUG
#endregion Prateek

#endregion C# Prateek Namespaces
//
// -END_PRATEEK_CSHARP_NAMESPACE-

//-----------------------------------------------------------------------------
#region File namespaces
using System.IO;
using Prateek.IO;
using System.Text.RegularExpressions;
#endregion File namespaces

//-----------------------------------------------------------------------------
namespace Prateek.CodeGeneration
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
                ScriptTemplate.Add(this);
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
                    return MatchTemplate(templateFile, extension, content);
                return true;
#else //!UNITY_EDITOR
                return false;
#endif //UNITY_EDITOR
            }
        }
    }
}
