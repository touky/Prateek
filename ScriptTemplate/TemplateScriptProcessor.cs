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
using System.Text.RegularExpressions;

using Prateek.IO;
using Prateek.ScriptTemplating;
#endregion File namespaces

namespace Prateek.ScriptTemplating
{
    //-------------------------------------------------------------------------
    internal sealed class ScriptKeywordProcessor : UnityEditor.AssetModificationProcessor
    {
        //---------------------------------------------------------------------
#if true || WELL_FUCK
        public static void OnWillCreateAsset(string path)
        {
            path = path.Replace(".meta", "");
            int index = path.LastIndexOf(".");
            if (index < 0)
                return;

            var fileExtension = path.Substring(index + 1);
            var slashIdx = Mathf.Max(0, path.LastIndexOf("/"));
            var fileName = path.Substring(slashIdx + 1, index - (slashIdx + 1));

            index = Application.dataPath.LastIndexOf("Assets");
            path = Application.dataPath.Substring(0, index) + path;
            if (!System.IO.File.Exists(path))
                return;

            var originalContent = FileHelpers.ReadAllTextCleaned(path);
            if (originalContent == string.Empty)
                return;

            var fileContent = string.Empty;
            var scripts = TemplateReplacement.Scripts;
            //Look for the correct script remplacement
            for (int r = 0; r < scripts.Count; r++)
            {
                var replacement = scripts[r];
                if (replacement.Match(fileExtension, originalContent))
                {
                    fileContent = replacement.Content.CleanText();
                    break;
                }

                if (fileContent != string.Empty)
                    break;
            }

            if (fileContent == string.Empty)
                return;

            fileContent = fileContent.Replace("#SCRIPTNAME#", fileName);

            TemplateHelpers.ApplyKeywords(ref fileContent, fileExtension);

            System.IO.File.WriteAllText(path, fileContent.ApplyCRLF());
            AssetDatabase.Refresh();
        }
#endif
    }
}
