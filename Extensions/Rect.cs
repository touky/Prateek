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
namespace Prateek.Extensions
{
    using UnityEngine;
    using static Prateek.ShaderTo.CSharp;

    //-------------------------------------------------------------------------
    public static partial class RectExt
    {
        //---------------------------------------------------------------------
        #region Declarations
        public static Rect Inflate(this Rect rect, float value)
        {
            return Inflate(rect, Vector2.one * value);
        }

        //---------------------------------------------------------------------
        public static Rect Inflate(this Rect rect, Vector2 value)
        {
            rect.position -= value * sign(rect.size);
            rect.size += value * sign(rect.size) * 2;
            return rect;
        }

        //---------------------------------------------------------------------
        public static Rect TruncateX(this Rect rect, float size)
        {
            if (size > 0)
            {
                rect.x += size;
            }
            rect.width -= abs(size);
            return rect;
        }

        //---------------------------------------------------------------------
        public static Rect TruncateY(this Rect rect, float size)
        {
            if (size > 0)
            {
                rect.y += size;
            }
            rect.height -= abs(size);
            return rect;
        }

        //---------------------------------------------------------------------
        public static Rect NextLine(this Rect rect)
        {
            rect.y += rect.height;
            return rect;
        }

        //---------------------------------------------------------------------
        public static Rect NextColumn(this Rect rect)
        {
            rect.x += rect.width;
            return rect;
        }
        #endregion Declarations
    }
}