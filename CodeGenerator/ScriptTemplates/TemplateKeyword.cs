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
namespace Prateek.CodeGeneration
{
    using System.Collections.Generic;
    using Prateek.Helpers;

    //-------------------------------------------------------------------------
    public partial class ScriptTemplate
    {
        //---------------------------------------------------------------------
        protected static Keyword NewKeyword(string extension)
        {
            return new Keyword(extension);
        }

        //---------------------------------------------------------------------
        public enum KeywordMode
        {
            KeywordOnly,
            ZoneDelimiter,

            MAX
        }

        //---------------------------------------------------------------------
        public class Keyword : BaseTemplate
        {
            //-----------------------------------------------------------------
            protected string tag;
            protected KeywordMode mode;

            //-----------------------------------------------------------------
            public KeywordMode Mode { get { return mode; } }
            public string Tag { get { return tag.Keyword(); } }
            public string TagBegin { get { return tag.KeywordBegin(); } }
            public string TagEnd { get { return tag.KeywordEnd(); } }

            //-----------------------------------------------------------------
            public Keyword(string extension) : base(extension) { }

            //-----------------------------------------------------------------
            public Keyword SetTag(string tag, KeywordMode tagStyle = KeywordMode.KeywordOnly)
            {
                this.tag = tag;
                this.mode = tagStyle;
                return this;
            }

            //-----------------------------------------------------------------
            public override BaseTemplate SetContent(string content)
            {
                base.SetContent(content);

                if (string.IsNullOrEmpty(content))
                {
                    return this;
                }

                if (mode == KeywordMode.ZoneDelimiter)
                {
                    this.Content = TagBegin + content + TagEnd;
                }
                return this;
            }

            //-----------------------------------------------------------------
            public override void Commit()
            {
                ScriptTemplate.Add(this);
            }
        }

        //---------------------------------------------------------------------
        public struct KeywordStack
        {
            //-----------------------------------------------------------------
            public struct SwapInfo
            {
                public Keyword operation;
                public string data;
                public int start;
                public int end;
            }

            //-----------------------------------------------------------------
            private KeywordMode tagType;
            private string content;
            private List<SwapInfo> stack;

            //-----------------------------------------------------------------
            public bool CanApply { get { return stack.Count > 0; } }

            //-----------------------------------------------------------------
            public KeywordStack(KeywordMode tagType, string content)
            {
                this.tagType = tagType;
                this.content = content;
                this.stack = new List<SwapInfo>();
            }

            //-----------------------------------------------------------------
            public void Add(Keyword operation, int start, int end)
            {
                stack.Add(new SwapInfo() { operation = operation, start = start, end = end });
            }

            //-----------------------------------------------------------------
            public void Add(string data, int start, int end)
            {
                stack.Add(new SwapInfo() { data = data, start = start, end = end });
            }

            //-----------------------------------------------------------------
            public void Reset()
            {
                content = string.Empty;
                stack.Clear();
            }

            //-----------------------------------------------------------------
            public string Apply()
            {
                stack.Sort((a, b) => { return a.start - b.start; });

                var result = content;
                for (int s = stack.Count - 1; s >= 0; s--)
                {
                    var data = stack[s];
                    result = result.Substring(0, data.start)
                           + (data.operation != null
                            ? data.operation.Content.CleanText()
                            : data.data)
                           + result.Substring(data.end);
                }
                return result;
            }
        }
    }
}
