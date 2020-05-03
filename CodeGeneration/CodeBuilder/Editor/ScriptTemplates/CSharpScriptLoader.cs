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
    using UnityEditor;

    ///-------------------------------------------------------------------------
    [InitializeOnLoad]
    class CSharpScriptLoader
    {
        static CSharpScriptLoader()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
                return;

            ScriptFileTemplate.Create("cs")
                              .SetEndsWith("Manager")
                              .SetTemplateFile("81-C# Script-NewBehaviourScript.cs.txt")
                              .Load("InternalContent_Script-NewGlobalManager.cs.txt")
                              .Commit();

            ScriptFileTemplate.Create("cs")
                              .SetEndsWith("EditorWindow")
                              .SetTemplateFile("81-C# Script-NewBehaviourScript.cs.txt")
                              .Load("InternalContent_Script-NewEditorWindowScript.cs.txt")
                              .Commit();

            ScriptFileTemplate.Create("cs")
                              .SetTemplateFile("81-C# Script-NewBehaviourScript.cs.txt")
                              .Load("InternalContent_Script-NewBehaviourScript.cs.txt")
                              .Commit();

            KeywordTemplate.Create("cs")
                           .SetTag("PRATEEK_CSHARP_NAMESPACE_CODE", KeywordTemplateMode.UsedAsScope)
                           .Load("InternalContent_PRATEEK_CSHARP_NAMESPACE_CODE.cs")
                           .Commit();

            KeywordTemplate.Create("cs")
                           .SetTag("PRATEEK_CSHARP_NAMESPACE_EDITOR", KeywordTemplateMode.UsedAsScope)
                           .Load("InternalContent_PRATEEK_CSHARP_NAMESPACE_EDITOR.cs")
                           .Commit();

            KeywordTemplate.Create("cs")
                           .SetTag("PRATEEK_CSHARP_IFDEF", KeywordTemplateMode.UsedAsScope)
                           .Load("InternalContent_PRATEEK_CSHARP_IFDEF.cs")
                           .Commit();
        }
    }
}
