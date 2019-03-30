// -BEGIN_PRATEEK_COPYRIGHT-
//
//  Prateek, a library that is "bien pratique"
//  Header last update date: 30/03/2019
//
//  Copyright � 2017-2019 "Touky" <touky@prateek.top>
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
using Unity.Jobs;
using Unity.Collections;

#region Engine
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.Serialization;

#if UNITY_PROFILING
using UnityEngine.Profiling;
#endif //UNITY_PROFILING
#endregion Engine

#region Editor
#if UNITY_EDITOR
using UnityEditor;
#endif //UNITY_EDITOR
#endregion Editor
#endregion Unity

#region Prateek
using Prateek;
using Prateek.Base;
using Prateek.Extensions;
using Prateek.Helpers;
using Prateek.Attributes;
using Prateek.Manager;

#region Using static
using static Prateek.ShaderTo.CSharp;
#endregion Using static

#region Editor
#if UNITY_EDITOR
using Prateek.CodeGeneration;
#endif //UNITY_EDITOR
#endregion Editor

#if PRATEEK_DEBUGS
using Prateek.Debug;
using static Prateek.Debug.Draw.Setup.QuickCTor;
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
namespace Prateek.CodeGeneration
{
    //-------------------------------------------------------------------------
    [InitializeOnLoad]
    class CSharpIgnorableLoader : ScriptTemplate
    {
        static CSharpIgnorableLoader()
        {
            NewIgnorableCSharp("cs").Commit();
        }
    }

    //-------------------------------------------------------------------------
    public partial class ScriptTemplate
    {
        //---------------------------------------------------------------------
        protected static IgnorableCSharp NewIgnorableCSharp(string extension)
        {
            return new IgnorableCSharp(extension);
        }

        //---------------------------------------------------------------------
        public class IgnorableCSharp : Ignorable
        {
            //-----------------------------------------------------------------
            private struct MatchData
            {
                //-------------------------------------------------------------
                public Style type;
                public bool isLine;
                public string start;
                public string ignore;
                public string end;

                //-------------------------------------------------------------
                public MatchData(Style type, string start, string end, bool isLine = false) : this(type, start, string.Empty, end, isLine) { }
                public MatchData(Style type, string start, string ignore, string end, bool isLine = false)
                {
                    this.type = type;
                    this.isLine = isLine;
                    this.start = start;
                    this.ignore = ignore;
                    this.end = end;
                }
            }

            //-----------------------------------------------------------------
            private MatchData[] datas = new MatchData[]
            {
                new MatchData(Style.Comment, "//", "\n", true),
                new MatchData(Style.Comment, "/*", "*/"),
                new MatchData(Style.Text, "@\"", "\"\"", "\""),
                new MatchData(Style.Text, "\"", "\\\"", "\"", true)
            };

            //-----------------------------------------------------------------
            public IgnorableCSharp(string extension) : base(extension) { }

            //-----------------------------------------------------------------
            public override void Commit()
            {
                ScriptTemplate.Add(this);
            }

            //-----------------------------------------------------------------
            public override BuildResult Build(string content)
            {
                var result = default(BuildResult);
                var position = 0;
                while (position < content.Length)
                {
                    var start = int.MaxValue;
                    var foundD = -1;
                    for (int d = 0; d < datas.Length; d++)
                    {
                        var s0 = content.IndexOf(datas[d].start, position);
                        if (s0 >= 0 && s0 < start)
                        {
                            start = s0;
                            foundD = d;
                        }
                    }

                    if (foundD < 0)
                        break;

                    var data = datas[foundD];
                    position = start + data.start.Length;

                    var end = position;
                    while (true)
                    {
                        var ignore = data.ignore == string.Empty ? -1 : content.IndexOf(data.ignore, position);
                        end = content.IndexOf(data.end, position);

                        if (ignore >= 0 && ignore <= end)
                        {
                            position = Mathf.Max(ignore + data.ignore.Length, end + data.end.Length);
                            continue;
                        }

                        break;
                    }

                    position = end + data.end.Length;
                    result.Add(new Extent(data.type, data.isLine, start, end + (data.end.Length - 1)));
                }
                return result;
            }
        }
    }
}

