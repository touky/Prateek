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
namespace Prateek.Runtime.Core.Helpers
{
    using System;
    using System.Collections.Generic;
    using Prateek.Runtime.Core.Consts;

    ///-------------------------------------------------------------------------
    public static class TextMessage
    {
        ///---------------------------------------------------------------------
        public static string INVALID_FORMAT = @"/!\ INVALID FORMAT /!\";
        public static string GENERIC_LOAD_ERROR = @"/!\ GENERIC LOAD ERROR /!\";
    }

    ///-------------------------------------------------------------------------
    public static class Strings
    {
        ///---------------------------------------------------------------------
        [Flags]
        public enum Separator : long
        {
            Space = 1,
            LineFeed = 1 << 1,
            CarrRet = 1 << 2,
            DirSlash = 1 << 3,
            DirBackslash = 1 << 4,
            FileExtension = 1 << 5,
            ParsePipe = 1 << 6,
            ParseComma = 1 << 7,
            ParseSemiColon = 1 << 8,
            ParenthesisOpen = 1 << 9,
            ParenthesisClose = 1 << 10,
            BraceOpen = 1 << 11,
            BraceClose = 1 << 12,
            BracketOpen = 1 << 13,
            BracketClose = 1 << 14,
            Tabs = 1 << 15,
            Keyword = 1 << 16,
            OpPlus = 1 << 17,
            OpMinus = 1 << 18,
            OpMul = 1 << 19,
            OpDiv = 1 << 20,

            NewLine = (LineFeed | CarrRet),
            Directory = (DirSlash | DirBackslash),
            TextParse = (ParsePipe | ParseComma | ParseSemiColon),
            Parenthesis = (ParenthesisOpen | ParenthesisClose),
            Brace = (BraceOpen | BraceClose),
            Bracket = (BracketOpen | BracketClose),
            Operators = (OpPlus | OpMinus | OpMul | OpDiv),

            ALL = ~0
        }

        ///---------------------------------------------------------------------
        public const int CommentSplitLength = 80;
        public static readonly string CommentSplitRoot = "///--";

        ///---------------------------------------------------------------------
        private static readonly char[] space = { ' ' };
        private static readonly char[] newLine = { '\n', '\r' };
        private static readonly char[] directory = { '/', '\\' };
        private static readonly char[] fileExtension = { '.' };
        private static readonly char[] textParse = { '|', ',', ';' };
        private static readonly char[] parenthesis = { '(', ')' };
        private static readonly char[] brace = { '{', '}' };
        private static readonly char[] bracket = { '[', ']' };
        private static readonly char[] tabs = { '\t' };
        private static readonly char[] keyword = { '#' };
        private static readonly char[] operators = { '+', '-', '*', '/' };
    
        ///---------------------------------------------------------------------
        private static Dictionary<Separator, char[]> dictionnary = new Dictionary<Separator, char[]>();

        ///---------------------------------------------------------------------
        public static string S(this Separator mask)
        {
            return C(mask).ToString();
        }

        ///---------------------------------------------------------------------
        public static char C(this Separator mask)
        {
            switch (mask)
            {
                case Separator.Space: { return space[0]; }
                case Separator.LineFeed: { return newLine[0]; }
                case Separator.CarrRet: { return newLine[1]; }
                case Separator.DirSlash: { return directory[0]; }
                case Separator.DirBackslash: { return directory[1]; }
                case Separator.FileExtension: { return fileExtension[0]; }
                case Separator.ParsePipe: { return textParse[0]; }
                case Separator.ParseComma: { return textParse[1]; }
                case Separator.ParseSemiColon: { return textParse[0]; }
                case Separator.ParenthesisOpen: { return parenthesis[0]; }
                case Separator.ParenthesisClose: { return parenthesis[1]; }
                case Separator.BraceOpen: { return brace[0]; }
                case Separator.BraceClose: { return brace[1]; }
                case Separator.BracketOpen: { return bracket[0]; }
                case Separator.BracketClose: { return bracket[1]; }
                case Separator.Tabs: { return tabs[0]; }
                case Separator.Keyword: { return keyword[0]; }
                case Separator.OpPlus: { return operators[0]; }
                case Separator.OpMinus: { return operators[1]; }
                case Separator.OpMul: { return operators[2]; }
                case Separator.OpDiv: { return operators[3]; }
            }
            return (char)0;
        }

        ///---------------------------------------------------------------------
        private static char[] GetRaw(Separator mask)
        {
            switch (mask)
            {
                case Separator.Space: { return space; }
                case Separator.NewLine: { return newLine; }
                case Separator.Directory: { return directory; }
                case Separator.FileExtension: { return fileExtension; }
                case Separator.TextParse: { return textParse; }
                case Separator.Parenthesis: { return parenthesis; }
                case Separator.Brace: { return brace; }
                case Separator.Bracket: { return bracket; }
                case Separator.Tabs: { return tabs; }
                case Separator.Keyword: { return keyword; }
                case Separator.Operators: { return operators; }
            }
            return null;
        }

        ///---------------------------------------------------------------------
        public static char[] Get(this Separator mask)
        {
            var result = GetRaw(mask);
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
                        chars.AddRange(GetRaw(value));
                    }
                }
                dictionnary[mask] = chars.ToArray();
                result = dictionnary[mask];
            }

            return result;
        }

        public static char First(this string mask)
        {
            return mask.Length > 0 ? mask[0] : default;
        }

        public static char Last(this string mask)
        {
            return mask.Length > 0 ? mask[mask.Length - 1] : default;
        }

        ///---------------------------------------------------------------------
        public static string NewLine(this string left, bool addBoth = false)
        {
            var result = left;
            if (addBoth)
                result += newLine[1];
            result += newLine[0];
            return result;
        }

        ///---------------------------------------------------------------------
        public static string GetTabulation(this string left, int textStart)
        {
            if (textStart <= Const.INDEX_NONE)
            {
                return string.Empty;
            }

            var i = left.LastIndexOf(Strings.Separator.LineFeed.C(), textStart);
            if (i > Const.INDEX_NONE)
            {
                return left.Substring(i + 1, textStart - (i + 1));
            }

            return string.Empty;
        }

        ///---------------------------------------------------------------------
        public static string CleanText(this string left)
        {
            return left.TabToSpaces().SimplifyNewLines();
        }

        ///---------------------------------------------------------------------
        public static string SimplifyNewLines(this string left)
        {
            var cr = Separator.CarrRet.C().ToString();
            if (!left.Contains(cr))
                return left;

            var lf = Separator.LineFeed.C().ToString();
            var lfcr = lf + cr;
            var crlf = cr + lf;

            var result = left;
            result = result.Replace(lfcr, lf);
            result = result.Replace(crlf, lf);
            result = result.Replace(cr, lf);
            return result;
        }

        ///---------------------------------------------------------------------
        public static string ApplyCRLF(this string left)
        {
            return left.Replace(Separator.LineFeed.C().ToString(), string.Empty + Separator.CarrRet.C() + Separator.LineFeed.C());
        }

        ///---------------------------------------------------------------------
        public static string[] SplitLines(this string left)
        {
            return left.SimplifyNewLines().Split(Separator.NewLine.Get());
        }

        ///---------------------------------------------------------------------
        public static string TabToSpaces(this string left, int spaceCount = 4)
        {
            if (!left.Contains(Separator.Tabs.C().ToString()))
                return left;

            var s = string.Empty;
            for (int c = 0; c < spaceCount; c++)
                s += ' ';
            var tab = Separator.Tabs.C().ToString();
            return left.Replace(tab, s);
        }

        ///---------------------------------------------------------------------
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

        ///---------------------------------------------------------------------
        public static string Extension(this string left, string extension, string extension2)
        {
            return left.Extension(extension).Extension(extension2);
        }

        public static string Extension(this string left, string extension)
        {
            if (string.IsNullOrEmpty(extension))
            {
                return left;
            }

            if (extension[0] == fileExtension[0])
            {
                return $"{left}{fileExtension[0]}{extension.TrimStart(fileExtension[0])}";
            }
            return $"{left}{fileExtension[0]}{extension}";
        }

        public static string Extension(this string left)
        {
            if (left.Last() != fileExtension[0])
            {
                return $"{left}{fileExtension[0]}";
            }
            return left;
        }

        ///---------------------------------------------------------------------
        public static string RemoveExtension(this string left)
        {
            var index = left.LastIndexOf(fileExtension[0]);
            if (index < 0)
            {
                return left;
            }

            return left.Substring(0, index);
        }

        ///---------------------------------------------------------------------
        public static string RemoveExtension(this string left, string extension)
        {
            if (!left.EndsWith(extension))
            {
                return left;
            }

            return left.RemoveExtension();
        }

        ///---------------------------------------------------------------------
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

        ///---------------------------------------------------------------------
        public static string Keyword(this string left, bool enable = true)
        {
            if (enable)
            {
                return string.Format("{0}{1}{2}", keyword[0], left, keyword[0]);
            }
            else
            {
                var result = left;
                if (result.StartsWith(Separator.Keyword.S()))
                    result = result.Remove(0, 1);
                if (result.EndsWith(Separator.Keyword.S()))
                    result = result.Remove(result.Length - 1);
                return result;
            }
        }

        ///---------------------------------------------------------------------
        public static string KeywordBegin(this string left)
        {
            return string.Format("// -BEGIN_{0}-", left);
        }

        ///---------------------------------------------------------------------
        public static string KeywordEnd(this string left)
        {
            return string.Format("// -END_{0}-", left);
        }

        ///---------------------------------------------------------------------
        public static bool Contains(this string value, string[] array)
        {
            foreach (var search in array)
            {
                if (value.Contains(search))
                {
                    return true;
                }
            }

            return false;
        }

        ///---------------------------------------------------------------------
        public static bool Contains(this string value, List<string> list)
        {
            foreach (var search in list)
            {
                if (value.Contains(search))
                {
                    return true;
                }
            }

            return false;
        }
    }
}

