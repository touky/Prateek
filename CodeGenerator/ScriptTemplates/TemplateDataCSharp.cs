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
    [InitializeOnLoad]
    class CSharpScriptLoader : ScriptTemplate
    {
        static CSharpScriptLoader()
        {
            NewScript("cs")
            .SetEndsWith("Manager")
            .SetTemplateFile("81-C# Script-NewBehaviourScript.cs.txt")
            .SetFileContent("InternalContent_Script-NewGlobalManager.cs.txt")
            .Commit();

            NewScript("cs")
            .SetEndsWith("EditorWindow")
            .SetTemplateFile("81-C# Script-NewBehaviourScript.cs.txt")
            .SetFileContent("InternalContent_Script-NewEditorWindowScript.cs.txt")
            .Commit();

            NewScript("cs")
            .SetTemplateFile("81-C# Script-NewBehaviourScript.cs.txt")
            .SetFileContent("InternalContent_Script-NewBehaviourScript.cs.txt")
            .Commit();

            NewKeyword("cs")
            .SetTag("PRATEEK_CSHARP_NAMESPACE", KeywordMode.ZoneDelimiter)
            .SetFileContent("InternalContent_PRATEEK_CSHARP_NAMESPACE.cs")
            .Commit();
        }
    }
}
