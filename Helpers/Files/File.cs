// -BEGIN_PRATEEK_COPYRIGHT-
//
//  Prateek, a library that is bien "pratique"
//  Header last update date: 05/03/19
//
//  Copyright © 2017—2019 Benjamin "Touky" Huet <huet.benjamin@gmail.com>
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

namespace Prateek.IO
{
    //-------------------------------------------------------------------------
    public static class FileHelpers
    {
        //---------------------------------------------------------------------
        public interface IExtensionMatcher { string Extension { get; } }
        public static string BuildExtensionMatch(string extension) { return string.Format("\\.({0})$", extension); }
        public static string BuildExtensionMatch<T>(List<T> list) where T : IExtensionMatcher
        {
            if (list == null)
                return string.Empty;

            string match = "\\.({0})$";
            string extensions = string.Empty;
            for (int r = 0; r < list.Count; r++)
            {
                var extension = list[r].Extension;
                if (extension == string.Empty)
                    continue;

                if (extensions != string.Empty)
                    extensions += "|";
                extensions += extension;
            }
            return string.Format(match, extensions);
        }

        //---------------------------------------------------------------------
        public static bool GatherFilesAt(string path, List<string> files, string matchPattern, bool recursive = false)
        {
            if (!Directory.Exists(path))
                return false;

            var directories = new List<string>();
            directories.Add(path);
            while (directories.Count > 0)
            {
                var directory = directories[0];
                {
                    if (recursive)
                    {
                        directories.AddRange(Directory.GetDirectories(directory));
                    }

                    var foundFiles = Directory.GetFiles(directory);
                    for (int f = 0; f < foundFiles.Length; f++)
                    {
                        if (Regex.Match(foundFiles[f], matchPattern).Success)
                        {
                            files.Add(foundFiles[f]);
                        }
                    }
                }
                directories.RemoveAt(0);
            }

            return files.Count > 0;
        }

        //---------------------------------------------------------------------
        public static string ReadAllTextCleaned(string path)
        {
            if (!File.Exists(path))
                return string.Empty;
            return File.ReadAllText(path).CleanText();
        }

        //---------------------------------------------------------------------
        public static void WriteTextAdjusted(string path, string content)
        {
            File.WriteAllText(path, content.ApplyCRLF());
        }
    }
}
