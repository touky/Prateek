// -BEGIN_PRATEEK_COPYRIGHT-// -BEGIN_PRATEEK_COPYRIGHT-
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
// -END_PRATEEK_COPYRIGHT-// -END_PRATEEK_COPYRIGHT-// -END_PRATEEK_COPYRIGHT-

// -BEGIN_PRATEEK_CSHARP_NAMESPACE-// -BEGIN_PRATEEK_CSHARP_NAMESPACE-
//
//-----------------------------------------------------------------------------
#region C# Prateek Namespaces

//Auto activate some of the prateek defines
#if UNITY_EDITOR

#if !PRATEEK_DEBUG
#define PRATEEK_DEBUG
#endif //!PRATEEK_DEBUG

#endif //UNITY_EDITOR && !PRATEEK_DEBUG

#endregion C# Prateek Namespaces
//
// -END_PRATEEK_CSHARP_NAMESPACE-// -END_PRATEEK_CSHARP_NAMESPACE-// -END_PRATEEK_CSHARP_NAMESPACE-

//-----------------------------------------------------------------------------
#region File namespaces
using System.IO;
using Prateek.IO;
using System.Text.RegularExpressions;
#endregion File namespaces

//-----------------------------------------------------------------------------
namespace Prateek.CodeGeneration
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    //-------------------------------------------------------------------------
    public partial class ScriptTemplate
    {
        //---------------------------------------------------------------------
        public abstract class Ignorable : BaseTemplate
        {
            //-----------------------------------------------------------------
            [Flags]
            public enum Style
            {
                Comment = 1,
                Text = 2,

                MAX = ~0
            }

            //-----------------------------------------------------------------
            public struct Extent
            {
                //-------------------------------------------------------------
                private Style style;
                private bool isLine;
                private int start;
                private int end;

                //-------------------------------------------------------------
                public Style Style { get { return style; } }
                public bool IsLine { get { return isLine; } }
                public int Start { get { return start; } }
                public int End { get { return end; } }
                public int OverStart { get { return start - 1; } }
                public int OverEnd { get { return end + 1; } }

                //-------------------------------------------------------------
                public Extent(Style type, bool isLine, int start, int end)
                {
                    this.style = type;
                    this.isLine = isLine;
                    this.start = start;
                    this.end = end;
                }

                //-------------------------------------------------------------
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
                public int AdvanceToSafety(int index, Style typeToAvoid = Style.MAX)
                {
                    if (extends == null)
                        return index;

                    for (int e = 0; e < extends.Count; e++)
                    {
                        var extent = extends[e];
                        if ((extent.Style & typeToAvoid) != 0 && extent.Contains(index))
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
                ScriptTemplate.Add(this);
            }

            //-----------------------------------------------------------------
            public abstract BuildResult Build(string content);
        }
    }
}
