// -BEGIN_PRATEEK_COPYRIGHT-
//
//  Prateek, a library that is "bien pratique"
//  Header last update date: 24/03/2019
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

#region Using static
using static Prateek.ShaderTo.CSharp;
#endregion Using static

#region Editor
#if UNITY_EDITOR
using Prateek.ScriptTemplating;
#endif //UNITY_EDITOR
#endregion Editor

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
    //-------------------------------------------------------------------------
    public partial class TemplateReplacement
    {
        //---------------------------------------------------------------------
        public abstract class Ignorable : TemplateBase
        {
            //-----------------------------------------------------------------
            public enum Style
            {
                Comment,
                Text,

                MAX
            }

            //-----------------------------------------------------------------
            public struct Extent
            {
                //---------------------------------------------------------
                private Style style;
                private bool isLine;
                private int start;
                private int end;

                //---------------------------------------------------------
                public Style Style { get { return style; } }
                public bool IsLine { get { return isLine; } }
                public int Start { get { return start; } }
                public int End { get { return end; } }
                public int OverStart { get { return start - 1; } }
                public int OverEnd { get { return end + 1; } }

                //---------------------------------------------------------
                public Extent(Style type, bool isLine, int start, int end)
                {
                    this.style = type;
                    this.isLine = isLine;
                    this.start = start;
                    this.end = end;
                }

                //---------------------------------------------------------
                public bool Contains(int index)
                {
                    if (start <= index && index <= end)
                        return true;
                    return false;
                }
            }

            //-----------------------------------------------------------------
            public struct BuildResult
            {
                //-------------------------------------------------------------
                private List<Extent> extends;

                //-------------------------------------------------------------
                public bool IsValid { get { return extends != null && extends.Count > 0; } }

                //-------------------------------------------------------------
                public void Add(Extent extent)
                {
                    if (extends == null)
                        extends = new List<Extent>();
                    extends.Add(extent);
                }

                //-------------------------------------------------------------
                public int AdvanceToSafety(int index, Style style = Style.MAX)
                {
                    if (extends == null)
                        return index;

                    for (int e = 0; e < extends.Count; e++)
                    {
                        var extent = extends[e];
                        if (extent.Style == style && extent.Contains(index))
                        {
                            return extent.OverEnd;
                        }
                    }

                    return index;
                }

                //-------------------------------------------------------------
                public bool Merge(BuildResult other)
                {
                    if (other.extends == null)
                        return IsValid;

                    if (extends == null)
                    {
                        extends = new List<Extent>(other.extends);
                        return IsValid;
                    }

                    var i0 = 0;
                    var i1 = 0;
                    var result = new List<Extent>();
                    while (i0 < extends.Count || i1 < other.extends.Count)
                    {
                        var e = new Extent();
                        if (i0 >= extends.Count)
                        {
                            e = other.extends[i1++];
                        }
                        else if (i1 >= other.extends.Count)
                        {
                            e = extends[i0++];
                        }
                        else
                        {
                            e = extends[i0].Start <= other.extends[i1].Start
                                ? extends[i0++]
                                : other.extends[i1++];
                        }

                        var hasMerged = false;
                        for (int r = 0; r < result.Count; r++)
                        {
                            if (result[r].Contains(e.Start) || result[r].Contains(e.End)
                             || e.Contains(result[r].Start) || e.Contains(result[r].End))
                            {
                                e = new Extent(result[r].Style,
                                        result[r].IsLine,
                                        Mathf.Min(e.Start, result[r].Start),
                                        Mathf.Max(e.End, result[r].End));
                                result[r] = e;
                                hasMerged = true;
                                break;
                            }
                        }

                        if (!hasMerged)
                        {
                            result.Add(e);
                        }
                    }

                    extends = result;
                    return IsValid;
                }
            }

            //-----------------------------------------------------------------
            public Ignorable(string extension) : base(extension) { }

            //-----------------------------------------------------------------
            public override void Commit()
            {
                TemplateReplacement.Add(this);
            }

            //-----------------------------------------------------------------
            public abstract BuildResult Build(string content);
        }
    }
}
