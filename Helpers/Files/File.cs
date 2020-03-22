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
namespace Prateek.IO
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text.RegularExpressions;
    using Prateek.Helpers;
    using UnityEditor;
    using UnityEngine;

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
        public static bool GatherFilesAt(string path, List<string> files, string matchPattern)
        { return GatherFilesAt(path, files, matchPattern, false, string.Empty); }
        public static bool GatherFilesAt(string path, List<string> files, string matchPattern, bool recursive)
        { return GatherFilesAt(path, files, matchPattern, recursive, string.Empty); }
        public static bool GatherFilesAt(string path, List<string> files, string matchPattern, bool recursive, string ignorePattern)
        {
            if (!Directory.Exists(path))
                return false;

            var ignoreMatch = "(.git)";
            if (ignorePattern != string.Empty)
                ignoreMatch = String.Format("(({0})|({1}))", ignoreMatch, ignorePattern);

            var directories = new List<string>();
            directories.Add(path);
            while (directories.Count > 0)
            {
                var directory = directories[0];
                {
                    if (recursive)
                    {
                        var dirs = Directory.GetDirectories(directory);
                        for (int d = 0; d < dirs.Length; d++)
                        {
                            if (Regex.Match(dirs[d], ignoreMatch).Success)
                                continue;

                            directories.Add(dirs[d]);
                        }
                    }

                    var foundFiles = Directory.GetFiles(directory);
                    for (int f = 0; f < foundFiles.Length; f++)
                    {
                        if (Regex.Match(foundFiles[f], ignoreMatch).Success)
                            continue;

                        if (!Regex.Match(foundFiles[f], matchPattern).Success)
                            continue;

                        files.Add(foundFiles[f]);
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

        //---------------------------------------------------------------------
        public static string GetAppFolder()
        {
#if UNITY_EDITOR
            var appPath = EditorApplication.applicationPath;
            if (!appPath.EndsWith(".exe"))
                return string.Empty;

            var last = appPath.LastIndexOf(Strings.Separator.DirSlash.C());
            if (last < 0)
                return string.Empty;

            return appPath.Substring(0, last + 1);
#else //!UNITY_EDITOR
            return string.Empty;
#endif //UNITY_EDITOR
        }

        //---------------------------------------------------------------------
        public static string GetScriptTemplateFolder()
        {
#if UNITY_EDITOR
            var appPath = GetAppFolder();
            if (appPath == string.Empty)
                return string.Empty;

            appPath = appPath + "Data/Resources/ScriptTemplates/";
            if (!Directory.Exists(appPath))
                return string.Empty;

            return appPath;
#else //!UNITY_EDITOR
            return string.Empty;
#endif //UNITY_EDITOR
        }

        //---------------------------------------------------------------------
        public static string GetValidDirectory(string path) { return GetValidIO(path, false); }
        public static string GetValidFile(string path) { return GetValidIO(path, true); }

        //---------------------------------------------------------------------
        private static string GetValidIO(string path, bool isFile)
        {
            for (int p = 0; ; p++)
            {
                var tempPath = path;
                switch (p)
                {
                    case 0: { break; }
                    case 1: { tempPath = Application.dataPath.Directory(tempPath); break; }
                    case 2: { tempPath = Application.streamingAssetsPath.Directory(tempPath); break; }
                    case 3: { tempPath = Application.persistentDataPath.Directory(tempPath); break; }
                    case 4: { tempPath = Application.temporaryCachePath.Directory(tempPath); break; }
#if UNITY_EDITOR
                    case 5: { tempPath = EditorApplication.applicationContentsPath.Directory(tempPath); break; }
                    case 6: { tempPath = EditorApplication.applicationPath.Directory(tempPath); break; }
#endif //UNITY_EDITOR
                    default: { return string.Empty; }
                }

                if ((isFile && File.Exists(tempPath))
                || (!isFile && Directory.Exists(tempPath)))
                {
                    return tempPath;
                }
            }
        }
    }
}
