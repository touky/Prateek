// -BEGIN_PRATEEK_COPYRIGHT-
//
//  Prateek, a library that is "bien pratique"
//  Header last update date: 18/03/19
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
        public abstract class TemplateBase : FileHelpers.IExtensionMatcher
        {
            //-----------------------------------------------------------------
            protected string extension;
            protected string content;

            //-----------------------------------------------------------------
            public string Extension { get { return extension; } }
            public string Content { get { return content; } }

            //-----------------------------------------------------------------
            protected TemplateBase(string extension)
            {
                this.extension = extension;
            }

            //-----------------------------------------------------------------
            public virtual TemplateBase SetContent(string content)
            {
                this.content = content;
                return this;
            }

            //-----------------------------------------------------------------
            public virtual bool Match(string extension, string content)
            {
                if (this.extension == string.Empty || this.extension == extension)
                    return true;
                return false;
            }

            //-----------------------------------------------------------------
            public abstract void Commit();
        }
    }
}
