// -BEGIN_PRATEEK_COPYRIGHT-
//
//  Prateek, a library that is "bien pratique"
//  Header last update date: 18/03/19
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

#if PRATEEK_DEBUGS
using Prateek.Debug;
#endif //PRATEEK_DEBUG
#endregion Prateek

#endregion C# Prateek Namespaces
//
// -END_PRATEEK_CSHARP_NAMESPACE-

//-----------------------------------------------------------------------------
#region File namespaces
#endregion File namespaces

//-----------------------------------------------------------------------------
namespace Prateek.Extensions
{
    //-------------------------------------------------------------------------
    public static partial class VectorExt
    {
        //---------------------------------------------------------------------
        #region Vector2
        #region Extensions
        public static Vector2 Max(this Vector2 v1, float v2) { return Max(v1, Vector2.one * v2); }
        public static Vector2 Max(this Vector2 v1, Vector2 v2)
        {
            return new Vector2(Mathf.Max(v1.x, v2.x), Mathf.Max(v1.y, v2.y));
        }

        //---------------------------------------------------------------------
        public static Vector2 Min(this Vector2 v1, float v2) { return Min(v1, Vector2.one * v2); }
        public static Vector2 Min(this Vector2 v1, Vector2 v2)
        {
            return new Vector2(Mathf.Min(v1.x, v2.x), Mathf.Min(v1.y, v2.y));
        }

        //---------------------------------------------------------------------
        public static float Area(this Vector2 v)
        {
            return v.x * v.y;
        }

        //---------------------------------------------------------------------
        public static int ToIndex(this Vector2 v, Vector2 dimensions)
        {
            return (int)v.x + (int)v.y * (int)dimensions.x;
        }

        //---------------------------------------------------------------------
        public static Vector2 Circle(this Vector2 v1, Vector2 size)
        {
            return new Vector2((v1.x + size.x) % size.x, (v1.y + size.y) % size.y);
        }

        //---------------------------------------------------------------------
        public static Vector2 Mul(this Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.x * v2.x, v1.y * v2.y);
        }

        //---------------------------------------------------------------------
        public static Vector2 Div(this Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.x / v2.x, v1.y / v2.y);
        }

        //---------------------------------------------------------------------
        public static Vector2 Abs(this Vector2 v)
        {
            return new Vector2(Mathf.Abs(v.x), Mathf.Abs(v.y));
        }

        //---------------------------------------------------------------------
        public static Vector2 Sign(this Vector2 v)
        {
            return new Vector2(Mathf.Sign(v.x), Mathf.Sign(v.y));
        }
        #endregion Extensions
        #endregion //Vector2

        //---------------------------------------------------------------------
        #region Vector3
        #region Extensions
        public static Vector3 Max(this Vector3 v1, float v2) { return Max(v1, Vector3.one * v2); }
        public static Vector3 Max(this Vector3 v1, Vector3 v2)
        {
            return new Vector3(Mathf.Max(v1.x, v2.x), Mathf.Max(v1.y, v2.y), Mathf.Max(v1.z, v2.z));
        }

        //---------------------------------------------------------------------
        public static Vector3 Min(this Vector3 v1, float v2) { return Min(v1, Vector3.one * v2); }
        public static Vector3 Min(this Vector3 v1, Vector3 v2)
        {
            return new Vector3(Mathf.Min(v1.x, v2.x), Mathf.Min(v1.y, v2.y), Mathf.Min(v1.z, v2.z));
        }

        //---------------------------------------------------------------------
        public static float Area(this Vector3 v)
        {
            return v.x * v.y * v.z;
        }

        //---------------------------------------------------------------------
        public static int ToIndex(this Vector3 v, Vector3 dimensions)
        {
            return (int)v.x + (int)v.y * (int)dimensions.x + (int)v.z * (int)dimensions.xy().Area();
        }

        //---------------------------------------------------------------------
        public static Vector3 Circle(this Vector3 v1, Vector3 size)
        {
            return new Vector3((v1.x + size.x) % size.x, (v1.y + size.y) % size.y, (v1.z + size.z) % size.z);
        }

        //---------------------------------------------------------------------
        public static Vector3 Mul(this Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
        }

        //---------------------------------------------------------------------
        public static Vector3 Div(this Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.x / v2.x, v1.y / v2.y, v1.z / v2.z);
        }

        //---------------------------------------------------------------------
        public static Vector3 Abs(this Vector3 v)
        {
            return new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
        }

        //---------------------------------------------------------------------
        public static Vector3 Sign(this Vector3 v)
        {
            return new Vector3(Mathf.Sign(v.x), Mathf.Sign(v.y), Mathf.Sign(v.z));
        }
        #endregion Extensions
        #endregion //Vector3

        //---------------------------------------------------------------------
        #region Vector4
        #region Extensions
        public static Vector4 Max(this Vector4 v1, float v2) { return Max(v1, Vector4.one * v2); }
        public static Vector4 Max(this Vector4 v1, Vector4 v2)
        {
            return new Vector4(Mathf.Max(v1.x, v2.x), Mathf.Max(v1.y, v2.y), Mathf.Max(v1.z, v2.z), Mathf.Max(v1.w, v2.w));
        }

        //---------------------------------------------------------------------
        public static Vector4 Min(this Vector4 v1, float v2) { return Min(v1, Vector4.one * v2); }
        public static Vector4 Min(this Vector4 v1, Vector4 v2)
        {
            return new Vector4(Mathf.Min(v1.x, v2.x), Mathf.Min(v1.y, v2.y), Mathf.Min(v1.z, v2.z), Mathf.Min(v1.w, v2.w));
        }

        //---------------------------------------------------------------------
        public static float Area(this Vector4 v)
        {
            return v.x * v.y * v.z * v.w;
        }

        //---------------------------------------------------------------------
        public static int ToIndex(this Vector4 v, Vector4 dimensions)
        {
            return (int)v.x + (int)v.y * (int)dimensions.x + (int)v.z * (int)dimensions.xy().Area() + (int)v.w * (int)dimensions.xyz().Area();
        }

        //---------------------------------------------------------------------
        public static Vector4 Circle(this Vector4 v1, Vector4 size)
        {
            return new Vector4((v1.x + size.x) % size.x, (v1.y + size.y) % size.y, (v1.z + size.z) % size.z, (v1.w + size.w) % size.w);
        }

        //---------------------------------------------------------------------
        public static Vector4 Mul(this Vector4 v1, Vector4 v2)
        {
            return new Vector4(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z, v1.w * v2.w);
        }

        //---------------------------------------------------------------------
        public static Vector4 Div(this Vector4 v1, Vector4 v2)
        {
            return new Vector4(v1.x / v2.x, v1.y / v2.y, v1.z / v2.z, v1.w / v2.w);
        }

        //---------------------------------------------------------------------
        public static Vector4 Abs(this Vector4 v)
        {
            return new Vector4(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z), Mathf.Abs(v.w));
        }

        //---------------------------------------------------------------------
        public static Vector4 Sign(this Vector4 v)
        {
            return new Vector4(Mathf.Sign(v.x), Mathf.Sign(v.y), Mathf.Sign(v.z), Mathf.Sign(v.w));
        }
        #endregion Extensions
        #endregion //Vector4
    }
}