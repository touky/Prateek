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
namespace Prateek.CodeGenerator.PrateekScriptBuilder
{
    using Prateek.Core.Code.Helpers;
    using Prateek.Helpers;

    //-------------------------------------------------------------------------
    public partial class CodeBuilder
    {
        //---------------------------------------------------------------------
        public static class Utils
        {
            //-----------------------------------------------------------------
            public struct SwapInfo
            {
                //-------------------------------------------------------------
                private string original;
                private string replacement;

                //-------------------------------------------------------------
                public string Original { get { return original; } }
                public string Replacement { get { return replacement; } }

                //-------------------------------------------------------------
                public SwapInfo(string original)
                {
                    this.original = original;
                    this.replacement = string.Empty;
                }

                //-------------------------------------------------------------
                public static implicit operator SwapInfo(string original)
                {
                    return new SwapInfo(original);
                }

                //-------------------------------------------------------------
                public static SwapInfo operator +(SwapInfo info, string other)
                {
                    return new SwapInfo() { original = info.original, replacement = other };
                }

                //-------------------------------------------------------------
                public string Apply(string text)
                {
                    if (text == null || original == null || replacement == null)
                        return text;
                    return text.Replace(original, !replacement.EndsWith(Strings.Separator.LineFeed.S()) ? replacement : replacement.Substring(0, replacement.Length - 1));
                }
            }

            //-----------------------------------------------------------------
            public struct KeyRule
            {
                //-------------------------------------------------------------
                public enum Usage
                {
                    Match,
                    Forbidden,
                    Ignore,

                    MAX
                }

                //-------------------------------------------------------------
                public struct ArgRange
                {
                    //---------------------------------------------------------
                    private int min;
                    private int max;

                    //---------------------------------------------------------
                    public bool NoneNeeded { get { return min <= 0 && max <= 0; } }

                    //---------------------------------------------------------
                    public static implicit operator ArgRange(int value)
                    {
                        return new ArgRange(value);
                    }

                    //---------------------------------------------------------
                    public ArgRange(int min) : this(min, min) { }
                    public ArgRange(int min, int max)
                    {
                        this.min = min;
                        this.max = max;
                    }

                    //---------------------------------------------------------
                    public bool Check(int count)
                    {
                        if (min < 0)
                            return true;

                        if (count < min)
                            return false;

                        if (max >= 0)
                            return count <= max;
                        return true;
                    }
                }

                //-------------------------------------------------------------
                public string key;
                public string scope;
                public Usage usage;
                public ArgRange args;
                public bool needOpenScope;
                public bool needScopeData;

                //-------------------------------------------------------------
                public KeyRule(string key, string scope)
                {
                    this.key = key;
                    this.scope = scope;
                    usage = Usage.Match;
                    args = new ArgRange(0);
                    needOpenScope = false;
                    needScopeData = false;
                }

                //-------------------------------------------------------------
                public bool Match(string key, string scope)
                {
                    return key == this.key && scope == this.scope;
                }
            }
        }
    }
}