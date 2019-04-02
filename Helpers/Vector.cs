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

#if PRATEEK_DEBUG
using Prateek.Debug;
using static Prateek.Debug.DebugDraw.DebugStyle.QuickCTor;
#endif //PRATEEK_DEBUG
#endregion Prateek

#endregion C# Prateek Namespaces
//
// -END_PRATEEK_CSHARP_NAMESPACE-

//-----------------------------------------------------------------------------
#region File namespaces
#endregion File namespaces

//-----------------------------------------------------------------------------
namespace Prateek.ShaderTo
{
    //-------------------------------------------------------------------------
    public static partial class CSharp
    {
        public static Vector2 normalize(Vector2 v) { return v.normalized; }
        public static Vector3 normalize(Vector3 v) { return v.normalized; }
        public static Vector4 normalize(Vector4 v) { return v.normalized; }
        public static float length(Vector2 v) { return v.magnitude; }
        public static float length(Vector3 v) { return v.magnitude; }
        public static float length(Vector4 v) { return v.magnitude; }
        public static float length(Vector2Int v) { return v.magnitude; }
        public static float length(Vector3Int v) { return v.magnitude; }
        public static float dot(Vector2 v0, Vector2 v1) { return Vector2.Dot(v0, v1); }
        public static float dot(Vector3 v0, Vector3 v1) { return Vector3.Dot(v0, v1); }
        public static float dot(Vector4 v0, Vector4 v1) { return Vector4.Dot(v0, v1); }
        public static float lerp(float v0, float v1, float alpha) { return Mathf.Lerp(v0, v1, alpha); }
        public static Vector2 lerp(Vector2 v0, Vector2 v1, float alpha) { return Vector2.Lerp(v0, v1, alpha); }
        public static Vector3 lerp(Vector3 v0, Vector3 v1, float alpha) { return Vector3.Lerp(v0, v1, alpha); }
        public static Vector4 lerp(Vector4 v0, Vector4 v1, float alpha) { return Vector4.Lerp(v0, v1, alpha); }
        public static Color lerp(Color v0, Color v1, float alpha) { return Color.Lerp(v0, v1, alpha); }
        public static float mix(float v0, float v1, float alpha) { return Mathf.Lerp(v0, v1, alpha); }
        public static Vector2 mix(Vector2 v0, Vector2 v1, float alpha) { return Vector2.Lerp(v0, v1, alpha); }
        public static Vector3 mix(Vector3 v0, Vector3 v1, float alpha) { return Vector3.Lerp(v0, v1, alpha); }
        public static Vector4 mix(Vector4 v0, Vector4 v1, float alpha) { return Vector4.Lerp(v0, v1, alpha); }
        public static Color mix(Color v0, Color v1, float alpha) { return Color.Lerp(v0, v1, alpha); }
        public static Vector3 cross(Vector3 v0, Vector3 v1) { return Vector3.Cross(v0, v1); }
        public static Vector2Int Int(this Vector2 v) { return vec2i((int)v.x, (int)v.y); }
        public static Vector3Int Int(this Vector3 v) { return vec3i((int)v.x, (int)v.y, (int)v.z); }
        public static Vector2 Float(this Vector2Int v) { return vec2(v.x, v.y); }
        public static Vector3 Float(this Vector3Int v) { return vec3(v.x, v.y, v.z); }
    }
}

//-----------------------------------------------------------------------------
namespace Prateek.Helpers
{
    //-------------------------------------------------------------------------
    public static class Vectors
    {
        //---------------------------------------------------------------------
        #region Vector2
        public static bool Test(Vector2 v0, Vector2 v1, float epsilon = Vector2.kEpsilon)
        {
            var d = v0 - v1;
            return -epsilon < d.x && d.x < epsilon
                && -epsilon < d.y && d.y < epsilon;
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
        public static Vector2 FromIndex(int index2D, Vector2 dimensions)
        {
            return new Vector2(index2D % (int)dimensions.x, index2D / (int)dimensions.x);
        }
        #endregion //Vector2

        //---------------------------------------------------------------------
        #region Vector3
        public static bool Test(Vector3 v0, Vector3 v1, float epsilon = Vector3.kEpsilon)
        {
            var d = v0 - v1;
            return -epsilon < d.x && d.x < epsilon
                && -epsilon < d.y && d.y < epsilon
                && -epsilon < d.z && d.z < epsilon;
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
        public static Vector3 FromIndex(int index3D, Vector3 dimensions)
        {
            var area2D = dimensions.xy().Area();
            var index2D = index3D % area2D;
            return new Vector3(index2D % (int)dimensions.x, index2D / (int)dimensions.x, index3D / (int)area2D);
        }
        #endregion //Vector3

        //---------------------------------------------------------------------
        #region Vector4
        public static bool Test(Vector4 v0, Vector4 v1, float epsilon = Vector4.kEpsilon)
        {
            var d = v0 - v1;
            return -epsilon < d.x && d.x < epsilon
                && -epsilon < d.y && d.y < epsilon
                && -epsilon < d.z && d.z < epsilon
                && -epsilon < d.w && d.w < epsilon;
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