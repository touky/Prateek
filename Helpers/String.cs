//
//  Prateek, a library that is "bien pratique"
//
//  Copyright © 2017—2018 Benjamin “Touky” Huet <huet.benjamin@gmail.com>
//
//  Prateek is free software. It comes without any warranty, to
//  the extent permitted by applicable law. You can redistribute it
//  and/or modify it under the terms of the Do What the Fuck You Want
//  to Public License, Version 2, as published by the WTFPL Task Force.
//  See http://www.wtfpl.net/ for more details.
//

#region Namespaces
#if UNITY_EDITOR && !PRATEEK_DEBUG
#define PRATEEK_DEBUG
#endif //UNITY_EDITOR && !PRATEEK_DEBUG

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine.Jobs;

#if UNITY_EDITOR
using UnityEditor;
#endif //UNITY_EDITOR

#if UNITY_PROFILING
using UnityEngine.Profiling;
#endif //UNITY_PROFILING

using Prateek;
using Prateek.Base;
using Prateek.Extensions;
using Prateek.Helpers;
using Prateek.Attributes;

#if PRATEEK_DEBUGS
using Prateek.Debug;
#endif //PRATEEK_DEBUG
#endregion Namespaces

//-----------------------------------------------------------------------------
namespace Prateek.Helpers
{
    //-------------------------------------------------------------------------
    public static class TextMessage
    {
        //---------------------------------------------------------------------
        public static string INVALID_FORMAT = "/!\\ INVALID FORMAT /!\\";
        public static string GENERIC_LOAD_ERROR = "/!\\ GENERIC LOAD ERROR /!\\";
    }

    //-------------------------------------------------------------------------
    public static class Strings
    {
        //---------------------------------------------------------------------
        [Flags]
        public enum Separator : long
        {
            Space = 1,
            NewLine = 1 << 1,
            Directory = 1 << 2,
            FileExtension = 1 << 3,
            TextParse = 1 << 4,
            Parenthesis = 1 << 5,
            Brace = 1 << 6,
            Bracket = 1 << 7,
            Tabs = 1 << 8,

            ALL = ~0
        }

        //---------------------------------------------------------------------
        private static readonly char[] newLine = { '\n', '\r' };
        private static readonly char[] directory = { '/', '\\' };
        private static readonly char[] fileExtension = { '.' };
        private static readonly char[] space = { ' ' };
        private static readonly char[] tabs = { '\t' };
        private static readonly char[] parenthesis = { '(', ')' };
        private static readonly char[] brace = { '{', '}' };
        private static readonly char[] bracket = { '[', ']' };
        private static readonly char[] textParse = { '|', ',', ';' };

        //---------------------------------------------------------------------
        private static Dictionary<Separator, char[]> dictionnary = new Dictionary<Separator, char[]>();

        //---------------------------------------------------------------------
        private static char[] Get(Separator mask)
        {
            switch (mask)
            {
                case Separator.NewLine: { return newLine; }
                case Separator.Directory: { return directory; }
                case Separator.FileExtension: { return fileExtension; }
                case Separator.Space: { return space; }
                case Separator.Tabs: { return tabs; }
                case Separator.Parenthesis: { return parenthesis; }
                case Separator.Brace: { return brace; }
                case Separator.Bracket: { return bracket; }
                case Separator.TextParse: { return textParse; }
            }
            return null;
        }

        //---------------------------------------------------------------------
        public static char[] C(this Separator mask)
        {
            var result = Get(mask);
            if (result != null)
                return result;

            if (dictionnary == null)
            {
                dictionnary = new Dictionary<Separator, char[]>();
            }

            if (dictionnary.ContainsKey(mask))
            {
                result = dictionnary[mask];
            }

            if (result == null)
            {
                List<char> chars = new List<char>();
                var values = Enum.GetValues(typeof(Separator));
                foreach (Separator value in values)
                {
                    if (value == Separator.ALL)
                        break;

                    if ((value & mask) != 0)
                    {
                        chars.AddRange(Get(value));
                    }
                }
                dictionnary[mask] = chars.ToArray();
                result = dictionnary[mask];
            }

            return result;
        }

        //---------------------------------------------------------------------
        public static string NewLine(this string left, bool addBoth = false)
        {
            var result = left + newLine[0];
            if (addBoth)
                result += newLine[1];
            return result;
        }

        //---------------------------------------------------------------------
        public static string[] SplitLines(this string left)
        {
            var newLine = Separator.NewLine.C()[0].ToString();
            var s = string.Empty;
            s = string.Empty + Separator.NewLine.C()[0] + Separator.NewLine.C()[1];
            left = left.Replace(s, newLine);
            s = string.Empty + Separator.NewLine.C()[1] + Separator.NewLine.C()[0];
            left = left.Replace(s, newLine);

            return left.Split(Separator.NewLine.C());
        }

        //---------------------------------------------------------------------
        public static string TabToSpaces(this string left, int spaceCount = 4)
        {
            var s = string.Empty;
            for (int c = 0; c < spaceCount; c++)
                s += ' ';
            return left.Replace(Separator.Tabs.C().ToString(), s);
        }

        //---------------------------------------------------------------------
        public static string Directory(this string left)
        {
            return left + directory[0];
        }

        public static string Directory(this string left, int typeIndex)
        {
            return left + directory[Math.Max(0, Math.Min(typeIndex, directory.Length - 1))];
        }

        public static string Directory(this string left, string right)
        {
            return left + directory[0] + right;
        }

        public static string Directory(this string left, int typeIndex, string right)
        {
            return left + directory[Math.Max(0, Math.Min(typeIndex, directory.Length - 1))] + right;
        }

        //---------------------------------------------------------------------
        public static string Extension(this string left, string right)
        {
            return left + fileExtension[0] + right;
        }

        public static string Extension(this string left)
        {
            return left + fileExtension[0];
        }

        //---------------------------------------------------------------------
        public static string RemoveExtension(this string left)
        {
            var index = left.LastIndexOf(fileExtension[0]);
            if (index < 0)
                return left;
            return left.Substring(0, index);
        }

        //---------------------------------------------------------------------
        public static string TextParse(this string left)
        {
            return left + textParse[0];
        }

        public static string TextParse(this string left, bool addSpace)
        {
            return left + textParse[0] + space[0];
        }

        public static string TextParse(this string left, int typeIndex)
        {
            return left + textParse[Math.Max(0, Math.Min(typeIndex, directory.Length - 1))];
        }

        public static string TextParse(this string left, int typeIndex, bool addSpace)
        {
            return left + textParse[Math.Max(0, Math.Min(typeIndex, directory.Length - 1))] + space[0];
        }

        public static string TextParse(this string left, string right)
        {
            return left + textParse[0] + right;
        }

        public static string TextParse(this string left, bool addSpace, string right)
        {
            return left + textParse[0] + space[0] + right;
        }

        public static string TextParse(this string left, int typeIndex, string right)
        {
            return left + textParse[Math.Max(0, Math.Min(typeIndex, directory.Length - 1))] + right;
        }

        public static string TextParse(this string left, int typeIndex, bool addSpace, string right)
        {
            return left + textParse[Math.Max(0, Math.Min(typeIndex, directory.Length - 1))] + space[0] + right;
        }
    }
}
