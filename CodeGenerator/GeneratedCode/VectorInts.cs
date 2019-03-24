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
namespace Prateek.Extensions
{
    //-------------------------------------------------------------------------
    public static partial class VectorExt
    {
        //---------------------------------------------------------------------
        #region Swizzle Vector2Int to Vector2Int
        public static Vector2Int xx(this Vector2Int v) { return new Vector2Int(v.x, v.x); }
        public static Vector2Int xy(this Vector2Int v) { return new Vector2Int(v.x, v.y); }
        public static Vector2Int xn(this Vector2Int v, int n_0 = 0) { return new Vector2Int(v.x, n_0); }
        public static Vector2Int yx(this Vector2Int v) { return new Vector2Int(v.y, v.x); }
        public static Vector2Int yy(this Vector2Int v) { return new Vector2Int(v.y, v.y); }
        public static Vector2Int yn(this Vector2Int v, int n_0 = 0) { return new Vector2Int(v.y, n_0); }
        public static Vector2Int nx(this Vector2Int v, int n_0 = 0) { return new Vector2Int(n_0, v.x); }
        public static Vector2Int ny(this Vector2Int v, int n_0 = 0) { return new Vector2Int(n_0, v.y); }
        #endregion Swizzle Vector2Int to Vector2Int
        
        //---------------------------------------------------------------------
        #region Swizzle Vector2Int to Vector3Int
        public static Vector3Int xxx(this Vector2Int v) { return new Vector3Int(v.x, v.x, v.x); }
        public static Vector3Int xxy(this Vector2Int v) { return new Vector3Int(v.x, v.x, v.y); }
        public static Vector3Int xxn(this Vector2Int v, int n_0 = 0) { return new Vector3Int(v.x, v.x, n_0); }
        public static Vector3Int xyx(this Vector2Int v) { return new Vector3Int(v.x, v.y, v.x); }
        public static Vector3Int xyy(this Vector2Int v) { return new Vector3Int(v.x, v.y, v.y); }
        public static Vector3Int xyn(this Vector2Int v, int n_0 = 0) { return new Vector3Int(v.x, v.y, n_0); }
        public static Vector3Int xnx(this Vector2Int v, int n_0 = 0) { return new Vector3Int(v.x, n_0, v.x); }
        public static Vector3Int xny(this Vector2Int v, int n_0 = 0) { return new Vector3Int(v.x, n_0, v.y); }
        public static Vector3Int xnn(this Vector2Int v, int n_0 = 0, int n_1 = 0) { return new Vector3Int(v.x, n_0, n_1); }
        public static Vector3Int yxx(this Vector2Int v) { return new Vector3Int(v.y, v.x, v.x); }
        public static Vector3Int yxy(this Vector2Int v) { return new Vector3Int(v.y, v.x, v.y); }
        public static Vector3Int yxn(this Vector2Int v, int n_0 = 0) { return new Vector3Int(v.y, v.x, n_0); }
        public static Vector3Int yyx(this Vector2Int v) { return new Vector3Int(v.y, v.y, v.x); }
        public static Vector3Int yyy(this Vector2Int v) { return new Vector3Int(v.y, v.y, v.y); }
        public static Vector3Int yyn(this Vector2Int v, int n_0 = 0) { return new Vector3Int(v.y, v.y, n_0); }
        public static Vector3Int ynx(this Vector2Int v, int n_0 = 0) { return new Vector3Int(v.y, n_0, v.x); }
        public static Vector3Int yny(this Vector2Int v, int n_0 = 0) { return new Vector3Int(v.y, n_0, v.y); }
        public static Vector3Int ynn(this Vector2Int v, int n_0 = 0, int n_1 = 0) { return new Vector3Int(v.y, n_0, n_1); }
        public static Vector3Int nxx(this Vector2Int v, int n_0 = 0) { return new Vector3Int(n_0, v.x, v.x); }
        public static Vector3Int nxy(this Vector2Int v, int n_0 = 0) { return new Vector3Int(n_0, v.x, v.y); }
        public static Vector3Int nxn(this Vector2Int v, int n_0 = 0, int n_1 = 0) { return new Vector3Int(n_0, v.x, n_1); }
        public static Vector3Int nyx(this Vector2Int v, int n_0 = 0) { return new Vector3Int(n_0, v.y, v.x); }
        public static Vector3Int nyy(this Vector2Int v, int n_0 = 0) { return new Vector3Int(n_0, v.y, v.y); }
        public static Vector3Int nyn(this Vector2Int v, int n_0 = 0, int n_1 = 0) { return new Vector3Int(n_0, v.y, n_1); }
        public static Vector3Int nnx(this Vector2Int v, int n_0 = 0, int n_1 = 0) { return new Vector3Int(n_0, n_1, v.x); }
        public static Vector3Int nny(this Vector2Int v, int n_0 = 0, int n_1 = 0) { return new Vector3Int(n_0, n_1, v.y); }
        #endregion Swizzle Vector2Int to Vector3Int
        
        //---------------------------------------------------------------------
        #region Swizzle Vector3Int to Vector2Int
        public static Vector2Int xx(this Vector3Int v) { return new Vector2Int(v.x, v.x); }
        public static Vector2Int xy(this Vector3Int v) { return new Vector2Int(v.x, v.y); }
        public static Vector2Int xz(this Vector3Int v) { return new Vector2Int(v.x, v.z); }
        public static Vector2Int xn(this Vector3Int v, int n_0 = 0) { return new Vector2Int(v.x, n_0); }
        public static Vector2Int yx(this Vector3Int v) { return new Vector2Int(v.y, v.x); }
        public static Vector2Int yy(this Vector3Int v) { return new Vector2Int(v.y, v.y); }
        public static Vector2Int yz(this Vector3Int v) { return new Vector2Int(v.y, v.z); }
        public static Vector2Int yn(this Vector3Int v, int n_0 = 0) { return new Vector2Int(v.y, n_0); }
        public static Vector2Int zx(this Vector3Int v) { return new Vector2Int(v.z, v.x); }
        public static Vector2Int zy(this Vector3Int v) { return new Vector2Int(v.z, v.y); }
        public static Vector2Int zz(this Vector3Int v) { return new Vector2Int(v.z, v.z); }
        public static Vector2Int zn(this Vector3Int v, int n_0 = 0) { return new Vector2Int(v.z, n_0); }
        public static Vector2Int nx(this Vector3Int v, int n_0 = 0) { return new Vector2Int(n_0, v.x); }
        public static Vector2Int ny(this Vector3Int v, int n_0 = 0) { return new Vector2Int(n_0, v.y); }
        public static Vector2Int nz(this Vector3Int v, int n_0 = 0) { return new Vector2Int(n_0, v.z); }
        #endregion Swizzle Vector3Int to Vector2Int
        
        //---------------------------------------------------------------------
        #region Swizzle Vector3Int to Vector3Int
        public static Vector3Int xxx(this Vector3Int v) { return new Vector3Int(v.x, v.x, v.x); }
        public static Vector3Int xxy(this Vector3Int v) { return new Vector3Int(v.x, v.x, v.y); }
        public static Vector3Int xxz(this Vector3Int v) { return new Vector3Int(v.x, v.x, v.z); }
        public static Vector3Int xxn(this Vector3Int v, int n_0 = 0) { return new Vector3Int(v.x, v.x, n_0); }
        public static Vector3Int xyx(this Vector3Int v) { return new Vector3Int(v.x, v.y, v.x); }
        public static Vector3Int xyy(this Vector3Int v) { return new Vector3Int(v.x, v.y, v.y); }
        public static Vector3Int xyz(this Vector3Int v) { return new Vector3Int(v.x, v.y, v.z); }
        public static Vector3Int xyn(this Vector3Int v, int n_0 = 0) { return new Vector3Int(v.x, v.y, n_0); }
        public static Vector3Int xzx(this Vector3Int v) { return new Vector3Int(v.x, v.z, v.x); }
        public static Vector3Int xzy(this Vector3Int v) { return new Vector3Int(v.x, v.z, v.y); }
        public static Vector3Int xzz(this Vector3Int v) { return new Vector3Int(v.x, v.z, v.z); }
        public static Vector3Int xzn(this Vector3Int v, int n_0 = 0) { return new Vector3Int(v.x, v.z, n_0); }
        public static Vector3Int xnx(this Vector3Int v, int n_0 = 0) { return new Vector3Int(v.x, n_0, v.x); }
        public static Vector3Int xny(this Vector3Int v, int n_0 = 0) { return new Vector3Int(v.x, n_0, v.y); }
        public static Vector3Int xnz(this Vector3Int v, int n_0 = 0) { return new Vector3Int(v.x, n_0, v.z); }
        public static Vector3Int xnn(this Vector3Int v, int n_0 = 0, int n_1 = 0) { return new Vector3Int(v.x, n_0, n_1); }
        public static Vector3Int yxx(this Vector3Int v) { return new Vector3Int(v.y, v.x, v.x); }
        public static Vector3Int yxy(this Vector3Int v) { return new Vector3Int(v.y, v.x, v.y); }
        public static Vector3Int yxz(this Vector3Int v) { return new Vector3Int(v.y, v.x, v.z); }
        public static Vector3Int yxn(this Vector3Int v, int n_0 = 0) { return new Vector3Int(v.y, v.x, n_0); }
        public static Vector3Int yyx(this Vector3Int v) { return new Vector3Int(v.y, v.y, v.x); }
        public static Vector3Int yyy(this Vector3Int v) { return new Vector3Int(v.y, v.y, v.y); }
        public static Vector3Int yyz(this Vector3Int v) { return new Vector3Int(v.y, v.y, v.z); }
        public static Vector3Int yyn(this Vector3Int v, int n_0 = 0) { return new Vector3Int(v.y, v.y, n_0); }
        public static Vector3Int yzx(this Vector3Int v) { return new Vector3Int(v.y, v.z, v.x); }
        public static Vector3Int yzy(this Vector3Int v) { return new Vector3Int(v.y, v.z, v.y); }
        public static Vector3Int yzz(this Vector3Int v) { return new Vector3Int(v.y, v.z, v.z); }
        public static Vector3Int yzn(this Vector3Int v, int n_0 = 0) { return new Vector3Int(v.y, v.z, n_0); }
        public static Vector3Int ynx(this Vector3Int v, int n_0 = 0) { return new Vector3Int(v.y, n_0, v.x); }
        public static Vector3Int yny(this Vector3Int v, int n_0 = 0) { return new Vector3Int(v.y, n_0, v.y); }
        public static Vector3Int ynz(this Vector3Int v, int n_0 = 0) { return new Vector3Int(v.y, n_0, v.z); }
        public static Vector3Int ynn(this Vector3Int v, int n_0 = 0, int n_1 = 0) { return new Vector3Int(v.y, n_0, n_1); }
        public static Vector3Int zxx(this Vector3Int v) { return new Vector3Int(v.z, v.x, v.x); }
        public static Vector3Int zxy(this Vector3Int v) { return new Vector3Int(v.z, v.x, v.y); }
        public static Vector3Int zxz(this Vector3Int v) { return new Vector3Int(v.z, v.x, v.z); }
        public static Vector3Int zxn(this Vector3Int v, int n_0 = 0) { return new Vector3Int(v.z, v.x, n_0); }
        public static Vector3Int zyx(this Vector3Int v) { return new Vector3Int(v.z, v.y, v.x); }
        public static Vector3Int zyy(this Vector3Int v) { return new Vector3Int(v.z, v.y, v.y); }
        public static Vector3Int zyz(this Vector3Int v) { return new Vector3Int(v.z, v.y, v.z); }
        public static Vector3Int zyn(this Vector3Int v, int n_0 = 0) { return new Vector3Int(v.z, v.y, n_0); }
        public static Vector3Int zzx(this Vector3Int v) { return new Vector3Int(v.z, v.z, v.x); }
        public static Vector3Int zzy(this Vector3Int v) { return new Vector3Int(v.z, v.z, v.y); }
        public static Vector3Int zzz(this Vector3Int v) { return new Vector3Int(v.z, v.z, v.z); }
        public static Vector3Int zzn(this Vector3Int v, int n_0 = 0) { return new Vector3Int(v.z, v.z, n_0); }
        public static Vector3Int znx(this Vector3Int v, int n_0 = 0) { return new Vector3Int(v.z, n_0, v.x); }
        public static Vector3Int zny(this Vector3Int v, int n_0 = 0) { return new Vector3Int(v.z, n_0, v.y); }
        public static Vector3Int znz(this Vector3Int v, int n_0 = 0) { return new Vector3Int(v.z, n_0, v.z); }
        public static Vector3Int znn(this Vector3Int v, int n_0 = 0, int n_1 = 0) { return new Vector3Int(v.z, n_0, n_1); }
        public static Vector3Int nxx(this Vector3Int v, int n_0 = 0) { return new Vector3Int(n_0, v.x, v.x); }
        public static Vector3Int nxy(this Vector3Int v, int n_0 = 0) { return new Vector3Int(n_0, v.x, v.y); }
        public static Vector3Int nxz(this Vector3Int v, int n_0 = 0) { return new Vector3Int(n_0, v.x, v.z); }
        public static Vector3Int nxn(this Vector3Int v, int n_0 = 0, int n_1 = 0) { return new Vector3Int(n_0, v.x, n_1); }
        public static Vector3Int nyx(this Vector3Int v, int n_0 = 0) { return new Vector3Int(n_0, v.y, v.x); }
        public static Vector3Int nyy(this Vector3Int v, int n_0 = 0) { return new Vector3Int(n_0, v.y, v.y); }
        public static Vector3Int nyz(this Vector3Int v, int n_0 = 0) { return new Vector3Int(n_0, v.y, v.z); }
        public static Vector3Int nyn(this Vector3Int v, int n_0 = 0, int n_1 = 0) { return new Vector3Int(n_0, v.y, n_1); }
        public static Vector3Int nzx(this Vector3Int v, int n_0 = 0) { return new Vector3Int(n_0, v.z, v.x); }
        public static Vector3Int nzy(this Vector3Int v, int n_0 = 0) { return new Vector3Int(n_0, v.z, v.y); }
        public static Vector3Int nzz(this Vector3Int v, int n_0 = 0) { return new Vector3Int(n_0, v.z, v.z); }
        public static Vector3Int nzn(this Vector3Int v, int n_0 = 0, int n_1 = 0) { return new Vector3Int(n_0, v.z, n_1); }
        public static Vector3Int nnx(this Vector3Int v, int n_0 = 0, int n_1 = 0) { return new Vector3Int(n_0, n_1, v.x); }
        public static Vector3Int nny(this Vector3Int v, int n_0 = 0, int n_1 = 0) { return new Vector3Int(n_0, n_1, v.y); }
        public static Vector3Int nnz(this Vector3Int v, int n_0 = 0, int n_1 = 0) { return new Vector3Int(n_0, n_1, v.z); }
        #endregion Swizzle Vector3Int to Vector3Int
        
        
    }
}

//-----------------------------------------------------------------------------
namespace Prateek.ShaderTo
{
    //-------------------------------------------------------------------------
    public static partial class CSharp
    {
        //---------------------------------------------------------------------
        #region Mixed Ctor Vector2Int
        public static Vector2Int vec2i(int n_0, int n_1) { return new Vector2Int(n_0, n_1); }
        public static Vector2Int vec2i(Vector2Int v_0) { return new Vector2Int(v_0.x, v_0.y); }
        #endregion Mixed Ctor Vector2Int
        
        //---------------------------------------------------------------------
        #region Mixed Ctor Vector3Int
        public static Vector3Int vec3i(int n_0, int n_1, int n_2) { return new Vector3Int(n_0, n_1, n_2); }
        public static Vector3Int vec3i(int n_0, Vector2Int v_0) { return new Vector3Int(n_0, v_0.x, v_0.y); }
        public static Vector3Int vec3i(Vector2Int v_0, int n_0) { return new Vector3Int(v_0.x, v_0.y, n_0); }
        public static Vector3Int vec3i(Vector3Int v_0) { return new Vector3Int(v_0.x, v_0.y, v_0.z); }
        #endregion Mixed Ctor Vector3Int
        
        
    }
}

//-----------------------------------------------------------------------------
namespace Prateek.ShaderTo
{
    //-------------------------------------------------------------------------
    public static partial class CSharp
    {
        //---------------------------------------------------------------------
        #region Mixed Func int
        public static int max(int n_0, int n_1) { return Mathf.Max(n_0, n_1); }
        public static int min(int n_0, int n_1) { return Mathf.Min(n_0, n_1); }
        public static int mul(int n_0, int n_1) { return n_0 * n_1; }
        public static int div(int n_0, int n_1) { return n_0 / n_1; }
        public static int abs(int n_0) { return Mathf.Abs(n_0); }
        public static int sign(int n_0) { return System.Math.Sign(n_0); }
        public static int exp(int n_0) { return (int)Mathf.Exp(n_0); }
        public static int mod(int n_0, int n_1) { return (n_0 + n_1) % n_1; }
        #endregion Mixed Func int
        
        //---------------------------------------------------------------------
        #region Mixed Func Vector2Int
        public static Vector2Int max(Vector2Int v_0, Vector2Int v_1) { return new Vector2Int(Mathf.Max(v_0.x, v_1.x), Mathf.Max(v_0.y, v_1.y)); }
        public static Vector2Int min(Vector2Int v_0, Vector2Int v_1) { return new Vector2Int(Mathf.Min(v_0.x, v_1.x), Mathf.Min(v_0.y, v_1.y)); }
        public static Vector2Int mul(Vector2Int v_0, Vector2Int v_1) { return new Vector2Int(v_0.x * v_1.x, v_0.y * v_1.y); }
        public static Vector2Int div(Vector2Int v_0, Vector2Int v_1) { return new Vector2Int(v_0.x / v_1.x, v_0.y / v_1.y); }
        public static Vector2Int abs(Vector2Int v_0) { return new Vector2Int(Mathf.Abs(v_0.x), Mathf.Abs(v_0.y)); }
        public static Vector2Int sign(Vector2Int v_0) { return new Vector2Int(System.Math.Sign(v_0.x), System.Math.Sign(v_0.y)); }
        public static Vector2Int exp(Vector2Int v_0) { return new Vector2Int((int)Mathf.Exp(v_0.x), (int)Mathf.Exp(v_0.y)); }
        public static Vector2Int mod(Vector2Int v_0, Vector2Int v_1) { return new Vector2Int((v_0.x + v_1.x) % v_1.x, (v_0.y + v_1.y) % v_1.y); }
        #endregion Mixed Func Vector2Int
        
        //---------------------------------------------------------------------
        #region Mixed Func Vector3Int
        public static Vector3Int max(Vector3Int v_0, Vector3Int v_1) { return new Vector3Int(Mathf.Max(v_0.x, v_1.x), Mathf.Max(v_0.y, v_1.y), Mathf.Max(v_0.z, v_1.z)); }
        public static Vector3Int min(Vector3Int v_0, Vector3Int v_1) { return new Vector3Int(Mathf.Min(v_0.x, v_1.x), Mathf.Min(v_0.y, v_1.y), Mathf.Min(v_0.z, v_1.z)); }
        public static Vector3Int mul(Vector3Int v_0, Vector3Int v_1) { return new Vector3Int(v_0.x * v_1.x, v_0.y * v_1.y, v_0.z * v_1.z); }
        public static Vector3Int div(Vector3Int v_0, Vector3Int v_1) { return new Vector3Int(v_0.x / v_1.x, v_0.y / v_1.y, v_0.z / v_1.z); }
        public static Vector3Int abs(Vector3Int v_0) { return new Vector3Int(Mathf.Abs(v_0.x), Mathf.Abs(v_0.y), Mathf.Abs(v_0.z)); }
        public static Vector3Int sign(Vector3Int v_0) { return new Vector3Int(System.Math.Sign(v_0.x), System.Math.Sign(v_0.y), System.Math.Sign(v_0.z)); }
        public static Vector3Int exp(Vector3Int v_0) { return new Vector3Int((int)Mathf.Exp(v_0.x), (int)Mathf.Exp(v_0.y), (int)Mathf.Exp(v_0.z)); }
        public static Vector3Int mod(Vector3Int v_0, Vector3Int v_1) { return new Vector3Int((v_0.x + v_1.x) % v_1.x, (v_0.y + v_1.y) % v_1.y, (v_0.z + v_1.z) % v_1.z); }
        #endregion Mixed Func Vector3Int
        
        
    }
}
