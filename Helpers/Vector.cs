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
namespace Prateek.Helpers
{
    //-------------------------------------------------------------------------
    public static class Vectors
    {
        //---------------------------------------------------------------------
        #region Vector2
        public static Vector2 Random(Vector2 min, Vector2 max)
        {
            var x = UnityEngine.Random.Range(min.x, max.x);
            var y = UnityEngine.Random.Range(min.y, max.y);
            return new Vector2(x, y);
        }

        //-----------------------------------------------------------------------------------------
        public static bool Test(Vector2 v0, Vector2 v1, float epsilon = Vector2.kEpsilon)
        {
            var d = v0 - v1;
            return -epsilon < d.x && d.x < epsilon
                && -epsilon < d.y && d.y < epsilon;
        }

        //-----------------------------------------------------------------------------------------
        public static Vector2 FromIndex(int index2D, Vector2 dimensions)
        {
            return new Vector2(index2D % (int)dimensions.x, index2D / (int)dimensions.x);
        }
        #endregion //Vector2

        //---------------------------------------------------------------------
        #region Vector3
        public static Vector3 Random(Vector3 min, Vector3 max)
        {
            var x = UnityEngine.Random.Range(min.x, max.x);
            var y = UnityEngine.Random.Range(min.y, max.y);
            var z = UnityEngine.Random.Range(min.z, max.z);
            return new Vector3(x, y, z);
        }

        //-----------------------------------------------------------------------------------------
        public static bool Test(Vector3 v0, Vector3 v1, float epsilon = Vector3.kEpsilon)
        {
            var d = v0 - v1;
            return -epsilon < d.x && d.x < epsilon
                && -epsilon < d.y && d.y < epsilon
                && -epsilon < d.z && d.z < epsilon;
        }

        //-----------------------------------------------------------------------------------------
        public static Vector3 FromIndex(int index3D, Vector3 dimensions)
        {
            var area2D = dimensions.xy().Area();
            var index2D = index3D % area2D;
            return new Vector3(index2D % (int)dimensions.x, index2D / (int)dimensions.x, index3D / (int)area2D);
        }
        #endregion //Vector3

        //---------------------------------------------------------------------
        #region Vector4
        public static Vector4 Random(Vector4 min, Vector4 max)
        {
            var x = UnityEngine.Random.Range(min.x, max.x);
            var y = UnityEngine.Random.Range(min.y, max.y);
            var z = UnityEngine.Random.Range(min.z, max.z);
            var w = UnityEngine.Random.Range(min.w, max.w);
            return new Vector4(x, y, z, w);
        }

        //-----------------------------------------------------------------------------------------
        public static bool Test(Vector4 v0, Vector4 v1, float epsilon = Vector4.kEpsilon)
        {
            var d = v0 - v1;
            return -epsilon < d.x && d.x < epsilon
                && -epsilon < d.y && d.y < epsilon
                && -epsilon < d.z && d.z < epsilon
                && -epsilon < d.w && d.w < epsilon;
        }

        //-----------------------------------------------------------------------------------------
        public static Vector4 FromIndex(int index4D, Vector4 dimensions)
        {
            var area3D = dimensions.xyz().Area();
            var area2D = dimensions.xy().Area();
            var index3D = index4D % area3D;
            var index2D = index3D % area2D;
            return new Vector4(index2D % (int)dimensions.x, index2D / (int)dimensions.x, index3D / (int)area2D, index4D / (int)area3D);
        }
        #endregion //Vector4
    }
}