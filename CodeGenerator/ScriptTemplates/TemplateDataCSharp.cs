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
//
//-----------------------------------------------------------------------------
#region C# Prateek Namespaces

//Auto activate some of the prateek defines
#if UNITY_EDITOR

#if !PRATEEK_DEBUG
#define PRATEEK_DEBUG
#endif //!PRATEEK_DEBUG

#endif //UNITY_EDITOR && !PRATEEK_DEBUG

#endregion C# Prateek Namespaces
//
// -END_PRATEEK_CSHARP_IFDEF-

//-----------------------------------------------------------------------------
#region File namespaces
using System.IO;
using Prateek.IO;
using System.Text.RegularExpressions;
#endregion File namespaces

//-----------------------------------------------------------------------------
namespace Prateek.CodeGeneration
{
    using UnityEditor;

    //-------------------------------------------------------------------------
#if UNITY_EDITOR
    //todo: fix that [InitializeOnLoad]
    class CSharpScriptLoader : ScriptTemplate
    {
        static CSharpScriptLoader()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
                return;

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

            NewKeyword("cs")
                .SetTag("PRATEEK_CSHARP_IFDEF", KeywordMode.ZoneDelimiter)
                .SetFileContent("InternalContent_PRATEEK_CSHARP_IFDEF.cs")
                .Commit();
        }
    }
#endif //UNITY_EDITOR
}
