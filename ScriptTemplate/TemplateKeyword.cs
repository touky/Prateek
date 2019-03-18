// -BEGIN_PRATEEK_COPYRIGHT-
//
//  Prateek, a library that is "bien pratique"
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
    //-------------------------------------------------------------------------
    public partial class TemplateReplacement
    {
        //---------------------------------------------------------------------
        public enum KeywordMode
        {
            KeywordOnly,
            ZoneDelimiter,

            MAX
        }

        //---------------------------------------------------------------------
        public class Keyword : TemplateBase
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
            public override TemplateBase SetContent(string content)
            {
                base.SetContent(content);

                if (mode == KeywordMode.ZoneDelimiter)
                {
                    this.content = tag.KeywordBegin() + this.content + tag.KeywordEnd();
                }
                return this;
            }

            //-----------------------------------------------------------------
            public override void Commit()
            {
                TemplateReplacement.Add(this);
            }
        }

        //---------------------------------------------------------------------
        protected static Keyword NewKeyword(string extension)
        {
            return new Keyword(extension);
        }

        //---------------------------------------------------------------------
        public struct KeywordStack
        {
            //-----------------------------------------------------------------
            public struct SwapInfo
            {
                public Keyword operation;
                public int start;
                public int end;
            }

            //-----------------------------------------------------------------
            private KeywordMode tagType;
            private string content;
            private List<SwapInfo> stack;

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
                           + data.operation.Content.CleanText()
                           + result.Substring(data.end);
                }
                return result;
            }
        }
    }
}
