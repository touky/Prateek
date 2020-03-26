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

    //-------------------------------------------------------------------------
#if UNITY_EDITOR
    //todo: fix that
    //todo [InitializeOnLoad]
    class PrateekDefaultLoader : CodeGenerator.ScriptTemplates.ScriptTemplate
    {
        static PrateekDefaultLoader()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
                return;

            ScriptTemplate.NewKeyword("cs")
            .SetTag("PRATEEK_COPYRIGHT", ScriptTemplate.KeywordMode.ZoneDelimiter)
            .SetFileContent("InternalContent_PRATEEK_COPYRIGHT.cs")
            .Commit();

            ScriptTemplate.NewKeyword(string.Empty)
            .SetTag("PRATEEK_DATE_YEAR")
            .SetContent(System.DateTime.Now.ToString("yyyy"))
            .Commit();

            ScriptTemplate.NewKeyword(string.Empty)
            .SetTag("PRATEEK_DATE_UPDATE")
            .SetContent(string.Format("{0}", System.DateTime.Now.ToString("dd/MM/yyyy")))
            .Commit();
        }
    }
#endif //UNITY_EDITOR
}
