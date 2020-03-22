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
    class PrateekDefaultLoader : ScriptTemplate
    {
        static PrateekDefaultLoader()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
                return;

            NewKeyword("cs")
            .SetTag("PRATEEK_COPYRIGHT", KeywordMode.ZoneDelimiter)
            .SetFileContent("InternalContent_PRATEEK_COPYRIGHT.cs")
            .Commit();

            NewKeyword(string.Empty)
            .SetTag("PRATEEK_DATE_YEAR")
            .SetContent(System.DateTime.Now.ToString("yyyy"))
            .Commit();

            NewKeyword(string.Empty)
            .SetTag("PRATEEK_DATE_UPDATE")
            .SetContent(string.Format("{0}", System.DateTime.Now.ToString("dd/MM/yyyy")))
            .Commit();
        }
    }
#endif //UNITY_EDITOR
}
