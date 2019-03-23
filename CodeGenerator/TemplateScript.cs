// -BEGIN_PRATEEK_COPYRIGHT-
//
//  Prateek, a library that is "bien pratique"
//  Header last update date: 20/03/2019
//
//  Copyright © 2017-2019 "Touky" <touky@prateek.top>
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
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.Serialization;

#if UNITY_PROFILING
using UnityEngine.Profiling;
#endif //UNITY_PROFILING

using Unity.Jobs;
using Unity.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif //UNITY_EDITOR
#endregion Unity

#region Prateek
using Prateek;
using Prateek.Base;
using Prateek.Extensions;
using Prateek.Helpers;
using Prateek.Attributes;

#if UNITY_EDITOR
using Prateek.ScriptTemplating;
#endif //UNITY_EDITOR

#if PRATEEK_DEBUGS
using Prateek.Debug;
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
namespace Prateek.ScriptTemplating
{
    //-------------------------------------------------------------------------
    public partial class TemplateReplacement
    {
        //---------------------------------------------------------------------
        public class Script : TemplateBase
        {
            //-----------------------------------------------------------------
            private string exportExtension = string.Empty;
            private string templateFile = string.Empty;

            //-----------------------------------------------------------------
            public string ExportExtension { get { return exportExtension; } }

            //-----------------------------------------------------------------
            public Script(string extension, string exportExtension) : base(extension)
            {
                this.exportExtension = exportExtension;
            }

            //-----------------------------------------------------------------
            public Script SetTemplateFile(string file)
            {
                this.templateFile = file;
                return this;
            }

            //-----------------------------------------------------------------
            public override void Commit()
            {
                TemplateReplacement.Add(this);
            }

            //-----------------------------------------------------------------
            public override bool Match(string extension, string content)
            {
                if (!base.Match(extension, content))
                    return false;

#if UNITY_EDITOR
                if (templateFile != string.Empty)
                    return MatchTemplate(templateFile, extension, content);
                return true;
#else //!UNITY_EDITOR
                return false;
#endif //UNITY_EDITOR
            }
        }

        //---------------------------------------------------------------------
        protected static Script NewScript(string extension) { return NewScript(extension, extension); }
        protected static Script NewScript(string extension, string exportExtension)
        {
            return new Script(extension, exportExtension);
        }
    }
}