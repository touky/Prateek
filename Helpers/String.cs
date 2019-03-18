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
#endregion File namespaces

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
            Keyword = 1 << 9,

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
        private static readonly char[] keyword = { '#' };

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
                case Separator.Keyword: { return keyword; }
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
            var result = left;
            if (addBoth)
                result += newLine[1];
            result += newLine[0];
            return result;
        }

        //---------------------------------------------------------------------
        public static string CleanText(this string left)
        {
            return left.TabToSpaces().SimplifyNewLines();
        }

        //---------------------------------------------------------------------
        public static string SimplifyNewLines(this string left)
        {
            var cr = Separator.NewLine.C()[1].ToString();
            if (!left.Contains(cr))
                return left;

            var lf = Separator.NewLine.C()[0].ToString();
            var lfcr = lf + cr;
            var crlf = cr + lf;

            var result = left;
            result = result.Replace(lfcr, lf);
            result = result.Replace(crlf, lf);
            result = result.Replace(cr, lf);
            return result;
        }

        //---------------------------------------------------------------------
        public static string ApplyCRLF(this string left)
        {
            return left.Replace(Separator.NewLine.C()[0].ToString(), string.Empty + Separator.NewLine.C()[1] + Separator.NewLine.C()[0]);
        }

        //---------------------------------------------------------------------
        public static string[] SplitLines(this string left)
        {
            return left.SimplifyNewLines().Split(Separator.NewLine.C());
        }

        //---------------------------------------------------------------------
        public static string TabToSpaces(this string left, int spaceCount = 4)
        {
            if (!left.Contains(Separator.Tabs.C()[0].ToString()))
                return left;

            var s = string.Empty;
            for (int c = 0; c < spaceCount; c++)
                s += ' ';
            var tab = Separator.Tabs.C()[0].ToString();
            return left.Replace(tab, s);
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

        //---------------------------------------------------------------------
        public static string Keyword(this string left)
        {
            return string.Format("{0}{1}{2}", keyword[0], left, keyword[0]);
        }

        //---------------------------------------------------------------------
        public static string KeywordBegin(this string left)
        {
            return string.Format("// -BEGIN_{0}-", left);
        }

        //---------------------------------------------------------------------
        public static string KeywordEnd(this string left)
        {
            return string.Format("// -END_{0}-", left);
        }
    }
}

