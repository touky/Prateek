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
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Assets.Prateek.CodeGenerator.Code.PrateekScriptBuilder.CodeAnalyzer;
    using Prateek.Core.Code.Helpers;
    using Prateek.Helpers;

    public abstract class CodeCommand
    {
        protected List<IComment> comments = new List<IComment>();

        public virtual void Add(CodeCommand other)
        {
            if (other.comments.Count > 0)
            {
                comments.AddRange(other.comments);
                other.comments.Clear();
            }
        }

        public void Set(IComment comment)
        {
            comments.Add(comment);
        }

        public virtual bool AllowInternalScope
        {
            get { return false; }
        }
    }

    public class CodeComment : CodeCommand, IComment
    {

    }

    public class CodeKeyword : CodeCommand
    {
        public override bool AllowInternalScope { get { return true; } }

        public Keyword keyword;
        public List<Keyword> arguments = new List<Keyword>();
        public CodeScope scopeContent;

        public override void Add(CodeCommand other)
        {
            base.Add(other);

            if (other is CodeScope scope)
            {
                scopeContent = scope;
            }
        }

        public void Add(Keyword argument)
        {
            arguments.Add(argument);
        }
    }
    
    public class CodeLiteral : CodeCommand
    {
        public List<LiteralValue> literals = new List<LiteralValue>();

        public void Add(LiteralValue literalSymbol)
        {
            literals.Add(literalSymbol);
        }
    }

    public class CodeScope : CodeCommand
    {
        public override bool AllowInternalScope { get { return true; } }

        public List<CodeCommand> commands = new List<CodeCommand>();
        public List<CodeScope> scopeContent = new List<CodeScope>();

        public override void Add(CodeCommand other)
        {
            base.Add(other);

            if (other is CodeScope scope)
            {
                scopeContent.Add(scope);
            }
            else if (!(other is CodeComment))
            {
                commands.Add(other);
            }
        }
    }

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
            [DebuggerDisplay("{scope}/{keyword}")]
            public struct KeywordRule
            {
                //-------------------------------------------------------------
                public enum Usage
                {
                    None,

                    Match,
                    Forbidden,
                    Ignore,

                    MAX
                }

                //-------------------------------------------------------------

                //-------------------------------------------------------------
                public string keyword;
                public string scope;
                public Usage usage;
                public ArgumentRange arguments;
                public bool needOpenScope;
                public bool needScopeData;
                public Func<PrateekScriptBuilder.CodeFile.ContentInfos, List<Keyword>, string, bool> onFeedCodeFile;
                public Func<PrateekScriptBuilder.CodeFile, string, bool> onCloseScope;

                //-------------------------------------------------------------
                public KeywordRule(string keyword, string scope)
                {
                    this.keyword = keyword;
                    this.scope = scope;
                    usage = Usage.Match;
                    arguments = 0;
                    needOpenScope = false;
                    needScopeData = false;
                    onFeedCodeFile = null;
                    onCloseScope = null;
                }

                //-------------------------------------------------------------
                public bool Match(string key, string scope)
                {
                    return key == this.keyword && scope == this.scope;
                }

                public bool ValidateRule(CodeKeyword codeKeyword, string scope)
                {
                    if (keyword != codeKeyword.keyword.Content || this.scope != scope)
                    {
                        return false;
                    }

                    if (!arguments.Check(codeKeyword.arguments.Count))
                    {
                        return false;
                    }

                    if (needOpenScope)
                    {
                        if (codeKeyword.scopeContent == null)
                        {
                            return false;
                        }
                    }

                    if (needScopeData)
                    {
                        if (codeKeyword.scopeContent == null)
                        {
                            return false;
                        }

                        if (codeKeyword.scopeContent.commands.Count == 0)
                        {
                            return false;
                        }
                    }

                    return true;
                }
            }
        }
    }

    public struct ArgumentRange
    {
        //---------------------------------------------------------
        private int min;
        private int max;

        //---------------------------------------------------------
        public bool NoneNeeded { get { return min <= 0 && max <= 0; } }

        //---------------------------------------------------------
        public static ArgumentRange AtLeast(int value)
        {
            return new ArgumentRange(value, -1);
        }

        public static ArgumentRange Between(int min, int max)
        {
            return new ArgumentRange(min, max);
        }

        public static implicit operator ArgumentRange(int value)
        {
            return new ArgumentRange(value, value);
        }

        //---------------------------------------------------------
        private ArgumentRange(int min, int max)
        {
            this.min = min;
            this.max = max;
        }

        //---------------------------------------------------------
        public bool Check(int count)
        {
            if (NoneNeeded && count > 0)
            {
                return false;
            }

            if (min < 0)
                return true;

            if (count < min)
                return false;

            if (max >= 0)
                return count <= max;
            return true;
        }
    }
}