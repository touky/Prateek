// -BEGIN_PRATEEK_COPYRIGHT-
//
//  Prateek, a library that is "bien pratique"
//  Header last update date: 20/03/2019
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

#if UNITY_EDITOR
using Prateek.ScriptTemplating;
#endif //UNITY_EDITOR

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

//-----------------------------------------------------------------------------
namespace Prateek.ScriptTemplating
{
    //---------------------------------------------------------------------
    public static partial class Code
    {
        public class Analyzer
        {
            //-----------------------------------------------------------------
            public enum SymbolType
            {
                WhiteSpace,
                LineFeed,
                Numeric,
                Letter,
                Literal,
                ScopeStart,
                ScopeEnd,
                CallStart,
                CallEnd,
                ArgSplit,
                Comment,

                MAX
            }

            //-----------------------------------------------------------------
            public string content;
            public bool allowKeywordStartWithAlpha = false;

            //-----------------------------------------------------------------
            private int position = 0;
            private int trailingWhiteSpaces = 0;
            private List<string> scopes = new List<string>();

            //-----------------------------------------------------------------
            private char[] charAllow = new char[] { '_', '.' };
            private char[] charLiteral = new char[1] { '@' };
            private char[] charIgnore = new char[2] { ' ', '\n' };
            private char[] charCalls = new char[2] { '(', ')' };
            private char[] charArgs = new char[1] { ',' };
            private char[] charScope = new char[2] { '{', '}' };
            private char[] charComment = new char[2] { '/', '*' };

            //-----------------------------------------------------------------
            public bool ShouldContinue { get { return position < content.Length; } }

            //-----------------------------------------------------------------
            public void Init(string content)
            {
                position = 0;
                trailingWhiteSpaces = 0;
                scopes.Clear();

                this.content = content;

                RemoveComments();
            }

            //-----------------------------------------------------------------
            public bool FindKeyword(ref string keyword)
            {
                keyword = String.Empty;
                while (position < content.Length)
                {
                    var type = GetSymbol(content[position]);
                    switch (type)
                    {
                        case SymbolType.Numeric:
                        case SymbolType.Letter:
                        {
                            if (type == SymbolType.Numeric && keyword.Length == 0)
                                return false;
                            keyword += content[position];
                            break;
                        }
                        case SymbolType.Comment:
                        case SymbolType.ScopeStart:
                        case SymbolType.WhiteSpace:
                        case SymbolType.LineFeed:
                        {
                            if (keyword.Length > 0)
                                return true;

                            if (type == SymbolType.ScopeStart)
                                scopes.Add(string.Empty);
                            break;
                        }
                        default:
                        {
                            if (keyword.Length > 0)
                                return true;
                            return false;
                        }
                    }

                    position++;
                }
                return keyword.Length > 0;
            }

            //-----------------------------------------------------------------
            public bool FindArgs(List<string> args, Code.Tag.KeyRule setup)
            {
                args.Clear();

                var argSplit = SymbolType.MAX;
                var argScope = SymbolType.MAX;
                var foundScope = false;
                var keyword = string.Empty;
                bool allowContinue = true;
                while (allowContinue && position < content.Length)
                {
                    var type = GetSymbol(content[position]);
                    switch (type)
                    {
                        case SymbolType.CallStart:
                        {
                            if (setup.NoArgNeeded)
                                return false;

                            if (argScope != SymbolType.MAX)
                                return false;

                            argSplit = SymbolType.ArgSplit;
                            argScope = SymbolType.CallStart;
                            break;
                        }
                        case SymbolType.CallEnd:
                        {
                            if (setup.NoArgNeeded)
                                return false;

                            if (argScope != SymbolType.CallStart)
                                return false;

                            argSplit = SymbolType.ArgSplit;
                            argScope = SymbolType.CallEnd;

                            allowContinue = setup.needOpenScope;
                            if (keyword.Length > 0)
                            {
                                args.Add(keyword);
                                keyword = string.Empty;
                            }
                            break;
                        }
                        case SymbolType.Numeric:
                        case SymbolType.Letter:
                        {
                            if (setup.NoArgNeeded)
                                return false;

                            keyword += content[position];
                            argSplit = SymbolType.Letter;
                            break;
                        }
                        case SymbolType.WhiteSpace:
                        case SymbolType.LineFeed:
                        case SymbolType.ArgSplit:
                        {
                            if (type == SymbolType.ArgSplit)
                            {
                                if (setup.NoArgNeeded)
                                    return false;

                                if (argSplit == SymbolType.ArgSplit)
                                    return false;
                            }

                            argSplit = type;
                            if (keyword.Length > 0)
                            {
                                args.Add(keyword);
                                keyword = string.Empty;
                            }
                            break;
                        }
                        case SymbolType.ScopeStart:
                        {
                            scopes.Add(setup.needOpenScope ? setup.key : string.Empty);

                            foundScope = true;
                            allowContinue = false;
                            if (!setup.needOpenScope)
                                break;

                            if (argSplit == SymbolType.ArgSplit)
                                return false;

                            if (argScope != SymbolType.CallEnd && argScope != SymbolType.MAX)
                                return false;
                            break;
                        }
                        default:
                        {
                            return false;
                        }
                    }

                    position++;
                }

                if (setup.needOpenScope && !foundScope)
                    return false;
                return setup.CheckArgCount(args.Count);
            }

            //-----------------------------------------------------------------
            public bool FindData(ref string data, Code.Tag.KeyRule setup)
            {
                if (!setup.needScopeData)
                    return true;
                data = string.Empty;

                var allowContinue = true;
                var storeData = false;
                var scopeCount = 1;
                while (allowContinue && position < content.Length)
                {
                    var type = GetSymbol(content[position]);
                    switch (type)
                    {
                        case SymbolType.ScopeStart:
                        {
                            if (storeData)
                            {
                                data += content[position];
                                break;
                            }

                            scopeCount++;
                            break;
                        }
                        case SymbolType.ScopeEnd:
                        {
                            if (storeData)
                            {
                                data += content[position];
                                break;
                            }

                            if (--scopeCount == 0)
                            {
                                return true;
                            }
                            break;
                        }
                        case SymbolType.Literal:
                        {
                            storeData = true;
                            break;
                        }
                        case SymbolType.LineFeed:
                        {
                            if (storeData)
                            {
                                data += content[position];
                                storeData = false;
                            }
                            break;
                        }
                        default:
                        {
                            if (storeData)
                            {
                                data += content[position];
                            }
                            break;
                        }
                    }

                    position++;
                }
                return true;
            }

            //-----------------------------------------------------------------
            public bool FindScopeEnd(ref string scopeName)
            {
                while (position < content.Length)
                {
                    var type = GetSymbol(content[position]);
                    switch (type)
                    {
                        case SymbolType.ScopeEnd:
                        {
                            if (scopes.Count == 0)
                                return false;

                            scopeName = scopes.Last();
                            scopes.RemoveLast();
                            position++;
                            return true;
                        }
                        case SymbolType.WhiteSpace:
                        {
                            trailingWhiteSpaces++;
                            break;
                        }
                        case SymbolType.LineFeed:
                        {
                            trailingWhiteSpaces = 0;
                            break;
                        }
                        default:
                        {
                            return false;
                        }
                    }

                    position++;
                }
                return false;
            }

            //-----------------------------------------------------------------
            private void RemoveComments()
            {
                int commentStart = -1;
                int commentEnd = -1;
                bool ignoreAllToLineEnd = false;
                bool lookForLineEnd = false;
                bool lookForContEnd = false;
                for (int i = 0; i < content.Length - 1; i++)
                {
                    var value0 = content[i];
                    var value1 = content[i + 1];
                    if (ignoreAllToLineEnd)
                    {
                        if (value0 == charIgnore[1])
                        {
                            ignoreAllToLineEnd = false;
                        }
                        continue;
                    }

                    if (lookForLineEnd || lookForContEnd)
                    {
                        if (lookForLineEnd && value0 == charIgnore[1])
                            commentEnd = i;
                        if (lookForContEnd && value0 == charComment[1] && value1 == charComment[0])
                            commentEnd = ++i;

                        if (commentEnd >= 0)
                        {
                            content = content.Remove(commentStart, (commentEnd + 1) - commentStart);
                            i = commentStart - 1;
                            lookForLineEnd = false;
                            lookForContEnd = false;
                            commentStart = -1;
                            commentEnd = -1;
                        }

                        continue;
                    }

                    if (value0 == charComment[0])
                    {
                        lookForLineEnd = value1 == charComment[0];
                        lookForContEnd = value1 == charComment[1];

                        if (lookForLineEnd || lookForContEnd)
                            commentStart = i;
                    }
                    else if (value0 == charLiteral[0])
                    {
                        ignoreAllToLineEnd = true;
                    }
                }
            }

            //-----------------------------------------------------------------
            private SymbolType GetSymbol(char value)
            {
                if (value == charIgnore[1])
                {
                    trailingWhiteSpaces = 0;
                    return SymbolType.LineFeed;
                }

                if (value == charIgnore[0])
                {
                    trailingWhiteSpaces++;
                    return SymbolType.WhiteSpace;
                }

                if (value == charCalls[0])
                    return SymbolType.CallStart;

                if (value == charCalls[1])
                    return SymbolType.CallEnd;

                if (value == charScope[0])
                {
                    return SymbolType.ScopeStart;
                }

                if (value == charScope[1])
                {
                    return SymbolType.ScopeEnd;
                }

                if (value == charLiteral[0])
                {
                    return SymbolType.Literal;
                }

                for (int c = 0; c < charArgs.Length; c++)
                {
                    if (charArgs[c] == value)
                        return SymbolType.ArgSplit;
                }

                if (Char.IsNumber(value))
                    return SymbolType.Numeric;

                if (Char.IsLetter(value))
                    return SymbolType.Letter;

                for (int c = 0; c < charAllow.Length; c++)
                {
                    if (value == charAllow[c])
                        return SymbolType.Letter;
                }

                return SymbolType.MAX;
            }
        }
    }
}
