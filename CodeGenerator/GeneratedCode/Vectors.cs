// -BEGIN_PRATEEK_COPYRIGHT-
//
//  Prateek, a library that is "bien pratique"
//  Header last update date: 23/03/2019
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

#if UNITY_EDITOR
using Prateek.ScriptTemplating;
#endif //UNITY_EDITOR

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
        #region Swizzle Vector2 to Vector2
        public static Vector2 xx(this Vector2 v) { return new Vector2(v.x, v.x); }
        public static Vector2 xy(this Vector2 v) { return new Vector2(v.x, v.y); }
        public static Vector2 xn(this Vector2 v, float n_0 = 0) { return new Vector2(v.x, n_0); }
        public static Vector2 yx(this Vector2 v) { return new Vector2(v.y, v.x); }
        public static Vector2 yy(this Vector2 v) { return new Vector2(v.y, v.y); }
        public static Vector2 yn(this Vector2 v, float n_0 = 0) { return new Vector2(v.y, n_0); }
        public static Vector2 nx(this Vector2 v, float n_0 = 0) { return new Vector2(n_0, v.x); }
        public static Vector2 ny(this Vector2 v, float n_0 = 0) { return new Vector2(n_0, v.y); }
        #endregion Swizzle Vector2 to Vector2
        
        //---------------------------------------------------------------------
        #region Swizzle Vector2 to Vector3
        public static Vector3 xxx(this Vector2 v) { return new Vector3(v.x, v.x, v.x); }
        public static Vector3 xxy(this Vector2 v) { return new Vector3(v.x, v.x, v.y); }
        public static Vector3 xxn(this Vector2 v, float n_0 = 0) { return new Vector3(v.x, v.x, n_0); }
        public static Vector3 xyx(this Vector2 v) { return new Vector3(v.x, v.y, v.x); }
        public static Vector3 xyy(this Vector2 v) { return new Vector3(v.x, v.y, v.y); }
        public static Vector3 xyn(this Vector2 v, float n_0 = 0) { return new Vector3(v.x, v.y, n_0); }
        public static Vector3 xnx(this Vector2 v, float n_0 = 0) { return new Vector3(v.x, n_0, v.x); }
        public static Vector3 xny(this Vector2 v, float n_0 = 0) { return new Vector3(v.x, n_0, v.y); }
        public static Vector3 xnn(this Vector2 v, float n_0 = 0, float n_1 = 0) { return new Vector3(v.x, n_0, n_1); }
        public static Vector3 yxx(this Vector2 v) { return new Vector3(v.y, v.x, v.x); }
        public static Vector3 yxy(this Vector2 v) { return new Vector3(v.y, v.x, v.y); }
        public static Vector3 yxn(this Vector2 v, float n_0 = 0) { return new Vector3(v.y, v.x, n_0); }
        public static Vector3 yyx(this Vector2 v) { return new Vector3(v.y, v.y, v.x); }
        public static Vector3 yyy(this Vector2 v) { return new Vector3(v.y, v.y, v.y); }
        public static Vector3 yyn(this Vector2 v, float n_0 = 0) { return new Vector3(v.y, v.y, n_0); }
        public static Vector3 ynx(this Vector2 v, float n_0 = 0) { return new Vector3(v.y, n_0, v.x); }
        public static Vector3 yny(this Vector2 v, float n_0 = 0) { return new Vector3(v.y, n_0, v.y); }
        public static Vector3 ynn(this Vector2 v, float n_0 = 0, float n_1 = 0) { return new Vector3(v.y, n_0, n_1); }
        public static Vector3 nxx(this Vector2 v, float n_0 = 0) { return new Vector3(n_0, v.x, v.x); }
        public static Vector3 nxy(this Vector2 v, float n_0 = 0) { return new Vector3(n_0, v.x, v.y); }
        public static Vector3 nxn(this Vector2 v, float n_0 = 0, float n_1 = 0) { return new Vector3(n_0, v.x, n_1); }
        public static Vector3 nyx(this Vector2 v, float n_0 = 0) { return new Vector3(n_0, v.y, v.x); }
        public static Vector3 nyy(this Vector2 v, float n_0 = 0) { return new Vector3(n_0, v.y, v.y); }
        public static Vector3 nyn(this Vector2 v, float n_0 = 0, float n_1 = 0) { return new Vector3(n_0, v.y, n_1); }
        public static Vector3 nnx(this Vector2 v, float n_0 = 0, float n_1 = 0) { return new Vector3(n_0, n_1, v.x); }
        public static Vector3 nny(this Vector2 v, float n_0 = 0, float n_1 = 0) { return new Vector3(n_0, n_1, v.y); }
        #endregion Swizzle Vector2 to Vector3
        
        //---------------------------------------------------------------------
        #region Swizzle Vector2 to Vector4
        public static Vector4 xxxx(this Vector2 v) { return new Vector4(v.x, v.x, v.x, v.x); }
        public static Vector4 xxxy(this Vector2 v) { return new Vector4(v.x, v.x, v.x, v.y); }
        public static Vector4 xxxn(this Vector2 v, float n_0 = 0) { return new Vector4(v.x, v.x, v.x, n_0); }
        public static Vector4 xxyx(this Vector2 v) { return new Vector4(v.x, v.x, v.y, v.x); }
        public static Vector4 xxyy(this Vector2 v) { return new Vector4(v.x, v.x, v.y, v.y); }
        public static Vector4 xxyn(this Vector2 v, float n_0 = 0) { return new Vector4(v.x, v.x, v.y, n_0); }
        public static Vector4 xxnx(this Vector2 v, float n_0 = 0) { return new Vector4(v.x, v.x, n_0, v.x); }
        public static Vector4 xxny(this Vector2 v, float n_0 = 0) { return new Vector4(v.x, v.x, n_0, v.y); }
        public static Vector4 xxnn(this Vector2 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.x, v.x, n_0, n_1); }
        public static Vector4 xyxx(this Vector2 v) { return new Vector4(v.x, v.y, v.x, v.x); }
        public static Vector4 xyxy(this Vector2 v) { return new Vector4(v.x, v.y, v.x, v.y); }
        public static Vector4 xyxn(this Vector2 v, float n_0 = 0) { return new Vector4(v.x, v.y, v.x, n_0); }
        public static Vector4 xyyx(this Vector2 v) { return new Vector4(v.x, v.y, v.y, v.x); }
        public static Vector4 xyyy(this Vector2 v) { return new Vector4(v.x, v.y, v.y, v.y); }
        public static Vector4 xyyn(this Vector2 v, float n_0 = 0) { return new Vector4(v.x, v.y, v.y, n_0); }
        public static Vector4 xynx(this Vector2 v, float n_0 = 0) { return new Vector4(v.x, v.y, n_0, v.x); }
        public static Vector4 xyny(this Vector2 v, float n_0 = 0) { return new Vector4(v.x, v.y, n_0, v.y); }
        public static Vector4 xynn(this Vector2 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.x, v.y, n_0, n_1); }
        public static Vector4 xnxx(this Vector2 v, float n_0 = 0) { return new Vector4(v.x, n_0, v.x, v.x); }
        public static Vector4 xnxy(this Vector2 v, float n_0 = 0) { return new Vector4(v.x, n_0, v.x, v.y); }
        public static Vector4 xnxn(this Vector2 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.x, n_0, v.x, n_1); }
        public static Vector4 xnyx(this Vector2 v, float n_0 = 0) { return new Vector4(v.x, n_0, v.y, v.x); }
        public static Vector4 xnyy(this Vector2 v, float n_0 = 0) { return new Vector4(v.x, n_0, v.y, v.y); }
        public static Vector4 xnyn(this Vector2 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.x, n_0, v.y, n_1); }
        public static Vector4 xnnx(this Vector2 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.x, n_0, n_1, v.x); }
        public static Vector4 xnny(this Vector2 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.x, n_0, n_1, v.y); }
        public static Vector4 xnnn(this Vector2 v, float n_0 = 0, float n_1 = 0, float n_2 = 0) { return new Vector4(v.x, n_0, n_1, n_2); }
        public static Vector4 yxxx(this Vector2 v) { return new Vector4(v.y, v.x, v.x, v.x); }
        public static Vector4 yxxy(this Vector2 v) { return new Vector4(v.y, v.x, v.x, v.y); }
        public static Vector4 yxxn(this Vector2 v, float n_0 = 0) { return new Vector4(v.y, v.x, v.x, n_0); }
        public static Vector4 yxyx(this Vector2 v) { return new Vector4(v.y, v.x, v.y, v.x); }
        public static Vector4 yxyy(this Vector2 v) { return new Vector4(v.y, v.x, v.y, v.y); }
        public static Vector4 yxyn(this Vector2 v, float n_0 = 0) { return new Vector4(v.y, v.x, v.y, n_0); }
        public static Vector4 yxnx(this Vector2 v, float n_0 = 0) { return new Vector4(v.y, v.x, n_0, v.x); }
        public static Vector4 yxny(this Vector2 v, float n_0 = 0) { return new Vector4(v.y, v.x, n_0, v.y); }
        public static Vector4 yxnn(this Vector2 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.y, v.x, n_0, n_1); }
        public static Vector4 yyxx(this Vector2 v) { return new Vector4(v.y, v.y, v.x, v.x); }
        public static Vector4 yyxy(this Vector2 v) { return new Vector4(v.y, v.y, v.x, v.y); }
        public static Vector4 yyxn(this Vector2 v, float n_0 = 0) { return new Vector4(v.y, v.y, v.x, n_0); }
        public static Vector4 yyyx(this Vector2 v) { return new Vector4(v.y, v.y, v.y, v.x); }
        public static Vector4 yyyy(this Vector2 v) { return new Vector4(v.y, v.y, v.y, v.y); }
        public static Vector4 yyyn(this Vector2 v, float n_0 = 0) { return new Vector4(v.y, v.y, v.y, n_0); }
        public static Vector4 yynx(this Vector2 v, float n_0 = 0) { return new Vector4(v.y, v.y, n_0, v.x); }
        public static Vector4 yyny(this Vector2 v, float n_0 = 0) { return new Vector4(v.y, v.y, n_0, v.y); }
        public static Vector4 yynn(this Vector2 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.y, v.y, n_0, n_1); }
        public static Vector4 ynxx(this Vector2 v, float n_0 = 0) { return new Vector4(v.y, n_0, v.x, v.x); }
        public static Vector4 ynxy(this Vector2 v, float n_0 = 0) { return new Vector4(v.y, n_0, v.x, v.y); }
        public static Vector4 ynxn(this Vector2 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.y, n_0, v.x, n_1); }
        public static Vector4 ynyx(this Vector2 v, float n_0 = 0) { return new Vector4(v.y, n_0, v.y, v.x); }
        public static Vector4 ynyy(this Vector2 v, float n_0 = 0) { return new Vector4(v.y, n_0, v.y, v.y); }
        public static Vector4 ynyn(this Vector2 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.y, n_0, v.y, n_1); }
        public static Vector4 ynnx(this Vector2 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.y, n_0, n_1, v.x); }
        public static Vector4 ynny(this Vector2 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.y, n_0, n_1, v.y); }
        public static Vector4 ynnn(this Vector2 v, float n_0 = 0, float n_1 = 0, float n_2 = 0) { return new Vector4(v.y, n_0, n_1, n_2); }
        public static Vector4 nxxx(this Vector2 v, float n_0 = 0) { return new Vector4(n_0, v.x, v.x, v.x); }
        public static Vector4 nxxy(this Vector2 v, float n_0 = 0) { return new Vector4(n_0, v.x, v.x, v.y); }
        public static Vector4 nxxn(this Vector2 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, v.x, v.x, n_1); }
        public static Vector4 nxyx(this Vector2 v, float n_0 = 0) { return new Vector4(n_0, v.x, v.y, v.x); }
        public static Vector4 nxyy(this Vector2 v, float n_0 = 0) { return new Vector4(n_0, v.x, v.y, v.y); }
        public static Vector4 nxyn(this Vector2 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, v.x, v.y, n_1); }
        public static Vector4 nxnx(this Vector2 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, v.x, n_1, v.x); }
        public static Vector4 nxny(this Vector2 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, v.x, n_1, v.y); }
        public static Vector4 nxnn(this Vector2 v, float n_0 = 0, float n_1 = 0, float n_2 = 0) { return new Vector4(n_0, v.x, n_1, n_2); }
        public static Vector4 nyxx(this Vector2 v, float n_0 = 0) { return new Vector4(n_0, v.y, v.x, v.x); }
        public static Vector4 nyxy(this Vector2 v, float n_0 = 0) { return new Vector4(n_0, v.y, v.x, v.y); }
        public static Vector4 nyxn(this Vector2 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, v.y, v.x, n_1); }
        public static Vector4 nyyx(this Vector2 v, float n_0 = 0) { return new Vector4(n_0, v.y, v.y, v.x); }
        public static Vector4 nyyy(this Vector2 v, float n_0 = 0) { return new Vector4(n_0, v.y, v.y, v.y); }
        public static Vector4 nyyn(this Vector2 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, v.y, v.y, n_1); }
        public static Vector4 nynx(this Vector2 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, v.y, n_1, v.x); }
        public static Vector4 nyny(this Vector2 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, v.y, n_1, v.y); }
        public static Vector4 nynn(this Vector2 v, float n_0 = 0, float n_1 = 0, float n_2 = 0) { return new Vector4(n_0, v.y, n_1, n_2); }
        public static Vector4 nnxx(this Vector2 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, n_1, v.x, v.x); }
        public static Vector4 nnxy(this Vector2 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, n_1, v.x, v.y); }
        public static Vector4 nnxn(this Vector2 v, float n_0 = 0, float n_1 = 0, float n_2 = 0) { return new Vector4(n_0, n_1, v.x, n_2); }
        public static Vector4 nnyx(this Vector2 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, n_1, v.y, v.x); }
        public static Vector4 nnyy(this Vector2 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, n_1, v.y, v.y); }
        public static Vector4 nnyn(this Vector2 v, float n_0 = 0, float n_1 = 0, float n_2 = 0) { return new Vector4(n_0, n_1, v.y, n_2); }
        public static Vector4 nnnx(this Vector2 v, float n_0 = 0, float n_1 = 0, float n_2 = 0) { return new Vector4(n_0, n_1, n_2, v.x); }
        public static Vector4 nnny(this Vector2 v, float n_0 = 0, float n_1 = 0, float n_2 = 0) { return new Vector4(n_0, n_1, n_2, v.y); }
        #endregion Swizzle Vector2 to Vector4
        
        //---------------------------------------------------------------------
        #region Swizzle Vector3 to Vector2
        public static Vector2 xx(this Vector3 v) { return new Vector2(v.x, v.x); }
        public static Vector2 xy(this Vector3 v) { return new Vector2(v.x, v.y); }
        public static Vector2 xz(this Vector3 v) { return new Vector2(v.x, v.z); }
        public static Vector2 xn(this Vector3 v, float n_0 = 0) { return new Vector2(v.x, n_0); }
        public static Vector2 yx(this Vector3 v) { return new Vector2(v.y, v.x); }
        public static Vector2 yy(this Vector3 v) { return new Vector2(v.y, v.y); }
        public static Vector2 yz(this Vector3 v) { return new Vector2(v.y, v.z); }
        public static Vector2 yn(this Vector3 v, float n_0 = 0) { return new Vector2(v.y, n_0); }
        public static Vector2 zx(this Vector3 v) { return new Vector2(v.z, v.x); }
        public static Vector2 zy(this Vector3 v) { return new Vector2(v.z, v.y); }
        public static Vector2 zz(this Vector3 v) { return new Vector2(v.z, v.z); }
        public static Vector2 zn(this Vector3 v, float n_0 = 0) { return new Vector2(v.z, n_0); }
        public static Vector2 nx(this Vector3 v, float n_0 = 0) { return new Vector2(n_0, v.x); }
        public static Vector2 ny(this Vector3 v, float n_0 = 0) { return new Vector2(n_0, v.y); }
        public static Vector2 nz(this Vector3 v, float n_0 = 0) { return new Vector2(n_0, v.z); }
        #endregion Swizzle Vector3 to Vector2
        
        //---------------------------------------------------------------------
        #region Swizzle Vector3 to Vector3
        public static Vector3 xxx(this Vector3 v) { return new Vector3(v.x, v.x, v.x); }
        public static Vector3 xxy(this Vector3 v) { return new Vector3(v.x, v.x, v.y); }
        public static Vector3 xxz(this Vector3 v) { return new Vector3(v.x, v.x, v.z); }
        public static Vector3 xxn(this Vector3 v, float n_0 = 0) { return new Vector3(v.x, v.x, n_0); }
        public static Vector3 xyx(this Vector3 v) { return new Vector3(v.x, v.y, v.x); }
        public static Vector3 xyy(this Vector3 v) { return new Vector3(v.x, v.y, v.y); }
        public static Vector3 xyz(this Vector3 v) { return new Vector3(v.x, v.y, v.z); }
        public static Vector3 xyn(this Vector3 v, float n_0 = 0) { return new Vector3(v.x, v.y, n_0); }
        public static Vector3 xzx(this Vector3 v) { return new Vector3(v.x, v.z, v.x); }
        public static Vector3 xzy(this Vector3 v) { return new Vector3(v.x, v.z, v.y); }
        public static Vector3 xzz(this Vector3 v) { return new Vector3(v.x, v.z, v.z); }
        public static Vector3 xzn(this Vector3 v, float n_0 = 0) { return new Vector3(v.x, v.z, n_0); }
        public static Vector3 xnx(this Vector3 v, float n_0 = 0) { return new Vector3(v.x, n_0, v.x); }
        public static Vector3 xny(this Vector3 v, float n_0 = 0) { return new Vector3(v.x, n_0, v.y); }
        public static Vector3 xnz(this Vector3 v, float n_0 = 0) { return new Vector3(v.x, n_0, v.z); }
        public static Vector3 xnn(this Vector3 v, float n_0 = 0, float n_1 = 0) { return new Vector3(v.x, n_0, n_1); }
        public static Vector3 yxx(this Vector3 v) { return new Vector3(v.y, v.x, v.x); }
        public static Vector3 yxy(this Vector3 v) { return new Vector3(v.y, v.x, v.y); }
        public static Vector3 yxz(this Vector3 v) { return new Vector3(v.y, v.x, v.z); }
        public static Vector3 yxn(this Vector3 v, float n_0 = 0) { return new Vector3(v.y, v.x, n_0); }
        public static Vector3 yyx(this Vector3 v) { return new Vector3(v.y, v.y, v.x); }
        public static Vector3 yyy(this Vector3 v) { return new Vector3(v.y, v.y, v.y); }
        public static Vector3 yyz(this Vector3 v) { return new Vector3(v.y, v.y, v.z); }
        public static Vector3 yyn(this Vector3 v, float n_0 = 0) { return new Vector3(v.y, v.y, n_0); }
        public static Vector3 yzx(this Vector3 v) { return new Vector3(v.y, v.z, v.x); }
        public static Vector3 yzy(this Vector3 v) { return new Vector3(v.y, v.z, v.y); }
        public static Vector3 yzz(this Vector3 v) { return new Vector3(v.y, v.z, v.z); }
        public static Vector3 yzn(this Vector3 v, float n_0 = 0) { return new Vector3(v.y, v.z, n_0); }
        public static Vector3 ynx(this Vector3 v, float n_0 = 0) { return new Vector3(v.y, n_0, v.x); }
        public static Vector3 yny(this Vector3 v, float n_0 = 0) { return new Vector3(v.y, n_0, v.y); }
        public static Vector3 ynz(this Vector3 v, float n_0 = 0) { return new Vector3(v.y, n_0, v.z); }
        public static Vector3 ynn(this Vector3 v, float n_0 = 0, float n_1 = 0) { return new Vector3(v.y, n_0, n_1); }
        public static Vector3 zxx(this Vector3 v) { return new Vector3(v.z, v.x, v.x); }
        public static Vector3 zxy(this Vector3 v) { return new Vector3(v.z, v.x, v.y); }
        public static Vector3 zxz(this Vector3 v) { return new Vector3(v.z, v.x, v.z); }
        public static Vector3 zxn(this Vector3 v, float n_0 = 0) { return new Vector3(v.z, v.x, n_0); }
        public static Vector3 zyx(this Vector3 v) { return new Vector3(v.z, v.y, v.x); }
        public static Vector3 zyy(this Vector3 v) { return new Vector3(v.z, v.y, v.y); }
        public static Vector3 zyz(this Vector3 v) { return new Vector3(v.z, v.y, v.z); }
        public static Vector3 zyn(this Vector3 v, float n_0 = 0) { return new Vector3(v.z, v.y, n_0); }
        public static Vector3 zzx(this Vector3 v) { return new Vector3(v.z, v.z, v.x); }
        public static Vector3 zzy(this Vector3 v) { return new Vector3(v.z, v.z, v.y); }
        public static Vector3 zzz(this Vector3 v) { return new Vector3(v.z, v.z, v.z); }
        public static Vector3 zzn(this Vector3 v, float n_0 = 0) { return new Vector3(v.z, v.z, n_0); }
        public static Vector3 znx(this Vector3 v, float n_0 = 0) { return new Vector3(v.z, n_0, v.x); }
        public static Vector3 zny(this Vector3 v, float n_0 = 0) { return new Vector3(v.z, n_0, v.y); }
        public static Vector3 znz(this Vector3 v, float n_0 = 0) { return new Vector3(v.z, n_0, v.z); }
        public static Vector3 znn(this Vector3 v, float n_0 = 0, float n_1 = 0) { return new Vector3(v.z, n_0, n_1); }
        public static Vector3 nxx(this Vector3 v, float n_0 = 0) { return new Vector3(n_0, v.x, v.x); }
        public static Vector3 nxy(this Vector3 v, float n_0 = 0) { return new Vector3(n_0, v.x, v.y); }
        public static Vector3 nxz(this Vector3 v, float n_0 = 0) { return new Vector3(n_0, v.x, v.z); }
        public static Vector3 nxn(this Vector3 v, float n_0 = 0, float n_1 = 0) { return new Vector3(n_0, v.x, n_1); }
        public static Vector3 nyx(this Vector3 v, float n_0 = 0) { return new Vector3(n_0, v.y, v.x); }
        public static Vector3 nyy(this Vector3 v, float n_0 = 0) { return new Vector3(n_0, v.y, v.y); }
        public static Vector3 nyz(this Vector3 v, float n_0 = 0) { return new Vector3(n_0, v.y, v.z); }
        public static Vector3 nyn(this Vector3 v, float n_0 = 0, float n_1 = 0) { return new Vector3(n_0, v.y, n_1); }
        public static Vector3 nzx(this Vector3 v, float n_0 = 0) { return new Vector3(n_0, v.z, v.x); }
        public static Vector3 nzy(this Vector3 v, float n_0 = 0) { return new Vector3(n_0, v.z, v.y); }
        public static Vector3 nzz(this Vector3 v, float n_0 = 0) { return new Vector3(n_0, v.z, v.z); }
        public static Vector3 nzn(this Vector3 v, float n_0 = 0, float n_1 = 0) { return new Vector3(n_0, v.z, n_1); }
        public static Vector3 nnx(this Vector3 v, float n_0 = 0, float n_1 = 0) { return new Vector3(n_0, n_1, v.x); }
        public static Vector3 nny(this Vector3 v, float n_0 = 0, float n_1 = 0) { return new Vector3(n_0, n_1, v.y); }
        public static Vector3 nnz(this Vector3 v, float n_0 = 0, float n_1 = 0) { return new Vector3(n_0, n_1, v.z); }
        #endregion Swizzle Vector3 to Vector3
        
        //---------------------------------------------------------------------
        #region Swizzle Vector3 to Vector4
        public static Vector4 xxxx(this Vector3 v) { return new Vector4(v.x, v.x, v.x, v.x); }
        public static Vector4 xxxy(this Vector3 v) { return new Vector4(v.x, v.x, v.x, v.y); }
        public static Vector4 xxxz(this Vector3 v) { return new Vector4(v.x, v.x, v.x, v.z); }
        public static Vector4 xxxn(this Vector3 v, float n_0 = 0) { return new Vector4(v.x, v.x, v.x, n_0); }
        public static Vector4 xxyx(this Vector3 v) { return new Vector4(v.x, v.x, v.y, v.x); }
        public static Vector4 xxyy(this Vector3 v) { return new Vector4(v.x, v.x, v.y, v.y); }
        public static Vector4 xxyz(this Vector3 v) { return new Vector4(v.x, v.x, v.y, v.z); }
        public static Vector4 xxyn(this Vector3 v, float n_0 = 0) { return new Vector4(v.x, v.x, v.y, n_0); }
        public static Vector4 xxzx(this Vector3 v) { return new Vector4(v.x, v.x, v.z, v.x); }
        public static Vector4 xxzy(this Vector3 v) { return new Vector4(v.x, v.x, v.z, v.y); }
        public static Vector4 xxzz(this Vector3 v) { return new Vector4(v.x, v.x, v.z, v.z); }
        public static Vector4 xxzn(this Vector3 v, float n_0 = 0) { return new Vector4(v.x, v.x, v.z, n_0); }
        public static Vector4 xxnx(this Vector3 v, float n_0 = 0) { return new Vector4(v.x, v.x, n_0, v.x); }
        public static Vector4 xxny(this Vector3 v, float n_0 = 0) { return new Vector4(v.x, v.x, n_0, v.y); }
        public static Vector4 xxnz(this Vector3 v, float n_0 = 0) { return new Vector4(v.x, v.x, n_0, v.z); }
        public static Vector4 xxnn(this Vector3 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.x, v.x, n_0, n_1); }
        public static Vector4 xyxx(this Vector3 v) { return new Vector4(v.x, v.y, v.x, v.x); }
        public static Vector4 xyxy(this Vector3 v) { return new Vector4(v.x, v.y, v.x, v.y); }
        public static Vector4 xyxz(this Vector3 v) { return new Vector4(v.x, v.y, v.x, v.z); }
        public static Vector4 xyxn(this Vector3 v, float n_0 = 0) { return new Vector4(v.x, v.y, v.x, n_0); }
        public static Vector4 xyyx(this Vector3 v) { return new Vector4(v.x, v.y, v.y, v.x); }
        public static Vector4 xyyy(this Vector3 v) { return new Vector4(v.x, v.y, v.y, v.y); }
        public static Vector4 xyyz(this Vector3 v) { return new Vector4(v.x, v.y, v.y, v.z); }
        public static Vector4 xyyn(this Vector3 v, float n_0 = 0) { return new Vector4(v.x, v.y, v.y, n_0); }
        public static Vector4 xyzx(this Vector3 v) { return new Vector4(v.x, v.y, v.z, v.x); }
        public static Vector4 xyzy(this Vector3 v) { return new Vector4(v.x, v.y, v.z, v.y); }
        public static Vector4 xyzz(this Vector3 v) { return new Vector4(v.x, v.y, v.z, v.z); }
        public static Vector4 xyzn(this Vector3 v, float n_0 = 0) { return new Vector4(v.x, v.y, v.z, n_0); }
        public static Vector4 xynx(this Vector3 v, float n_0 = 0) { return new Vector4(v.x, v.y, n_0, v.x); }
        public static Vector4 xyny(this Vector3 v, float n_0 = 0) { return new Vector4(v.x, v.y, n_0, v.y); }
        public static Vector4 xynz(this Vector3 v, float n_0 = 0) { return new Vector4(v.x, v.y, n_0, v.z); }
        public static Vector4 xynn(this Vector3 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.x, v.y, n_0, n_1); }
        public static Vector4 xzxx(this Vector3 v) { return new Vector4(v.x, v.z, v.x, v.x); }
        public static Vector4 xzxy(this Vector3 v) { return new Vector4(v.x, v.z, v.x, v.y); }
        public static Vector4 xzxz(this Vector3 v) { return new Vector4(v.x, v.z, v.x, v.z); }
        public static Vector4 xzxn(this Vector3 v, float n_0 = 0) { return new Vector4(v.x, v.z, v.x, n_0); }
        public static Vector4 xzyx(this Vector3 v) { return new Vector4(v.x, v.z, v.y, v.x); }
        public static Vector4 xzyy(this Vector3 v) { return new Vector4(v.x, v.z, v.y, v.y); }
        public static Vector4 xzyz(this Vector3 v) { return new Vector4(v.x, v.z, v.y, v.z); }
        public static Vector4 xzyn(this Vector3 v, float n_0 = 0) { return new Vector4(v.x, v.z, v.y, n_0); }
        public static Vector4 xzzx(this Vector3 v) { return new Vector4(v.x, v.z, v.z, v.x); }
        public static Vector4 xzzy(this Vector3 v) { return new Vector4(v.x, v.z, v.z, v.y); }
        public static Vector4 xzzz(this Vector3 v) { return new Vector4(v.x, v.z, v.z, v.z); }
        public static Vector4 xzzn(this Vector3 v, float n_0 = 0) { return new Vector4(v.x, v.z, v.z, n_0); }
        public static Vector4 xznx(this Vector3 v, float n_0 = 0) { return new Vector4(v.x, v.z, n_0, v.x); }
        public static Vector4 xzny(this Vector3 v, float n_0 = 0) { return new Vector4(v.x, v.z, n_0, v.y); }
        public static Vector4 xznz(this Vector3 v, float n_0 = 0) { return new Vector4(v.x, v.z, n_0, v.z); }
        public static Vector4 xznn(this Vector3 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.x, v.z, n_0, n_1); }
        public static Vector4 xnxx(this Vector3 v, float n_0 = 0) { return new Vector4(v.x, n_0, v.x, v.x); }
        public static Vector4 xnxy(this Vector3 v, float n_0 = 0) { return new Vector4(v.x, n_0, v.x, v.y); }
        public static Vector4 xnxz(this Vector3 v, float n_0 = 0) { return new Vector4(v.x, n_0, v.x, v.z); }
        public static Vector4 xnxn(this Vector3 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.x, n_0, v.x, n_1); }
        public static Vector4 xnyx(this Vector3 v, float n_0 = 0) { return new Vector4(v.x, n_0, v.y, v.x); }
        public static Vector4 xnyy(this Vector3 v, float n_0 = 0) { return new Vector4(v.x, n_0, v.y, v.y); }
        public static Vector4 xnyz(this Vector3 v, float n_0 = 0) { return new Vector4(v.x, n_0, v.y, v.z); }
        public static Vector4 xnyn(this Vector3 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.x, n_0, v.y, n_1); }
        public static Vector4 xnzx(this Vector3 v, float n_0 = 0) { return new Vector4(v.x, n_0, v.z, v.x); }
        public static Vector4 xnzy(this Vector3 v, float n_0 = 0) { return new Vector4(v.x, n_0, v.z, v.y); }
        public static Vector4 xnzz(this Vector3 v, float n_0 = 0) { return new Vector4(v.x, n_0, v.z, v.z); }
        public static Vector4 xnzn(this Vector3 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.x, n_0, v.z, n_1); }
        public static Vector4 xnnx(this Vector3 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.x, n_0, n_1, v.x); }
        public static Vector4 xnny(this Vector3 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.x, n_0, n_1, v.y); }
        public static Vector4 xnnz(this Vector3 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.x, n_0, n_1, v.z); }
        public static Vector4 xnnn(this Vector3 v, float n_0 = 0, float n_1 = 0, float n_2 = 0) { return new Vector4(v.x, n_0, n_1, n_2); }
        public static Vector4 yxxx(this Vector3 v) { return new Vector4(v.y, v.x, v.x, v.x); }
        public static Vector4 yxxy(this Vector3 v) { return new Vector4(v.y, v.x, v.x, v.y); }
        public static Vector4 yxxz(this Vector3 v) { return new Vector4(v.y, v.x, v.x, v.z); }
        public static Vector4 yxxn(this Vector3 v, float n_0 = 0) { return new Vector4(v.y, v.x, v.x, n_0); }
        public static Vector4 yxyx(this Vector3 v) { return new Vector4(v.y, v.x, v.y, v.x); }
        public static Vector4 yxyy(this Vector3 v) { return new Vector4(v.y, v.x, v.y, v.y); }
        public static Vector4 yxyz(this Vector3 v) { return new Vector4(v.y, v.x, v.y, v.z); }
        public static Vector4 yxyn(this Vector3 v, float n_0 = 0) { return new Vector4(v.y, v.x, v.y, n_0); }
        public static Vector4 yxzx(this Vector3 v) { return new Vector4(v.y, v.x, v.z, v.x); }
        public static Vector4 yxzy(this Vector3 v) { return new Vector4(v.y, v.x, v.z, v.y); }
        public static Vector4 yxzz(this Vector3 v) { return new Vector4(v.y, v.x, v.z, v.z); }
        public static Vector4 yxzn(this Vector3 v, float n_0 = 0) { return new Vector4(v.y, v.x, v.z, n_0); }
        public static Vector4 yxnx(this Vector3 v, float n_0 = 0) { return new Vector4(v.y, v.x, n_0, v.x); }
        public static Vector4 yxny(this Vector3 v, float n_0 = 0) { return new Vector4(v.y, v.x, n_0, v.y); }
        public static Vector4 yxnz(this Vector3 v, float n_0 = 0) { return new Vector4(v.y, v.x, n_0, v.z); }
        public static Vector4 yxnn(this Vector3 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.y, v.x, n_0, n_1); }
        public static Vector4 yyxx(this Vector3 v) { return new Vector4(v.y, v.y, v.x, v.x); }
        public static Vector4 yyxy(this Vector3 v) { return new Vector4(v.y, v.y, v.x, v.y); }
        public static Vector4 yyxz(this Vector3 v) { return new Vector4(v.y, v.y, v.x, v.z); }
        public static Vector4 yyxn(this Vector3 v, float n_0 = 0) { return new Vector4(v.y, v.y, v.x, n_0); }
        public static Vector4 yyyx(this Vector3 v) { return new Vector4(v.y, v.y, v.y, v.x); }
        public static Vector4 yyyy(this Vector3 v) { return new Vector4(v.y, v.y, v.y, v.y); }
        public static Vector4 yyyz(this Vector3 v) { return new Vector4(v.y, v.y, v.y, v.z); }
        public static Vector4 yyyn(this Vector3 v, float n_0 = 0) { return new Vector4(v.y, v.y, v.y, n_0); }
        public static Vector4 yyzx(this Vector3 v) { return new Vector4(v.y, v.y, v.z, v.x); }
        public static Vector4 yyzy(this Vector3 v) { return new Vector4(v.y, v.y, v.z, v.y); }
        public static Vector4 yyzz(this Vector3 v) { return new Vector4(v.y, v.y, v.z, v.z); }
        public static Vector4 yyzn(this Vector3 v, float n_0 = 0) { return new Vector4(v.y, v.y, v.z, n_0); }
        public static Vector4 yynx(this Vector3 v, float n_0 = 0) { return new Vector4(v.y, v.y, n_0, v.x); }
        public static Vector4 yyny(this Vector3 v, float n_0 = 0) { return new Vector4(v.y, v.y, n_0, v.y); }
        public static Vector4 yynz(this Vector3 v, float n_0 = 0) { return new Vector4(v.y, v.y, n_0, v.z); }
        public static Vector4 yynn(this Vector3 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.y, v.y, n_0, n_1); }
        public static Vector4 yzxx(this Vector3 v) { return new Vector4(v.y, v.z, v.x, v.x); }
        public static Vector4 yzxy(this Vector3 v) { return new Vector4(v.y, v.z, v.x, v.y); }
        public static Vector4 yzxz(this Vector3 v) { return new Vector4(v.y, v.z, v.x, v.z); }
        public static Vector4 yzxn(this Vector3 v, float n_0 = 0) { return new Vector4(v.y, v.z, v.x, n_0); }
        public static Vector4 yzyx(this Vector3 v) { return new Vector4(v.y, v.z, v.y, v.x); }
        public static Vector4 yzyy(this Vector3 v) { return new Vector4(v.y, v.z, v.y, v.y); }
        public static Vector4 yzyz(this Vector3 v) { return new Vector4(v.y, v.z, v.y, v.z); }
        public static Vector4 yzyn(this Vector3 v, float n_0 = 0) { return new Vector4(v.y, v.z, v.y, n_0); }
        public static Vector4 yzzx(this Vector3 v) { return new Vector4(v.y, v.z, v.z, v.x); }
        public static Vector4 yzzy(this Vector3 v) { return new Vector4(v.y, v.z, v.z, v.y); }
        public static Vector4 yzzz(this Vector3 v) { return new Vector4(v.y, v.z, v.z, v.z); }
        public static Vector4 yzzn(this Vector3 v, float n_0 = 0) { return new Vector4(v.y, v.z, v.z, n_0); }
        public static Vector4 yznx(this Vector3 v, float n_0 = 0) { return new Vector4(v.y, v.z, n_0, v.x); }
        public static Vector4 yzny(this Vector3 v, float n_0 = 0) { return new Vector4(v.y, v.z, n_0, v.y); }
        public static Vector4 yznz(this Vector3 v, float n_0 = 0) { return new Vector4(v.y, v.z, n_0, v.z); }
        public static Vector4 yznn(this Vector3 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.y, v.z, n_0, n_1); }
        public static Vector4 ynxx(this Vector3 v, float n_0 = 0) { return new Vector4(v.y, n_0, v.x, v.x); }
        public static Vector4 ynxy(this Vector3 v, float n_0 = 0) { return new Vector4(v.y, n_0, v.x, v.y); }
        public static Vector4 ynxz(this Vector3 v, float n_0 = 0) { return new Vector4(v.y, n_0, v.x, v.z); }
        public static Vector4 ynxn(this Vector3 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.y, n_0, v.x, n_1); }
        public static Vector4 ynyx(this Vector3 v, float n_0 = 0) { return new Vector4(v.y, n_0, v.y, v.x); }
        public static Vector4 ynyy(this Vector3 v, float n_0 = 0) { return new Vector4(v.y, n_0, v.y, v.y); }
        public static Vector4 ynyz(this Vector3 v, float n_0 = 0) { return new Vector4(v.y, n_0, v.y, v.z); }
        public static Vector4 ynyn(this Vector3 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.y, n_0, v.y, n_1); }
        public static Vector4 ynzx(this Vector3 v, float n_0 = 0) { return new Vector4(v.y, n_0, v.z, v.x); }
        public static Vector4 ynzy(this Vector3 v, float n_0 = 0) { return new Vector4(v.y, n_0, v.z, v.y); }
        public static Vector4 ynzz(this Vector3 v, float n_0 = 0) { return new Vector4(v.y, n_0, v.z, v.z); }
        public static Vector4 ynzn(this Vector3 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.y, n_0, v.z, n_1); }
        public static Vector4 ynnx(this Vector3 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.y, n_0, n_1, v.x); }
        public static Vector4 ynny(this Vector3 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.y, n_0, n_1, v.y); }
        public static Vector4 ynnz(this Vector3 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.y, n_0, n_1, v.z); }
        public static Vector4 ynnn(this Vector3 v, float n_0 = 0, float n_1 = 0, float n_2 = 0) { return new Vector4(v.y, n_0, n_1, n_2); }
        public static Vector4 zxxx(this Vector3 v) { return new Vector4(v.z, v.x, v.x, v.x); }
        public static Vector4 zxxy(this Vector3 v) { return new Vector4(v.z, v.x, v.x, v.y); }
        public static Vector4 zxxz(this Vector3 v) { return new Vector4(v.z, v.x, v.x, v.z); }
        public static Vector4 zxxn(this Vector3 v, float n_0 = 0) { return new Vector4(v.z, v.x, v.x, n_0); }
        public static Vector4 zxyx(this Vector3 v) { return new Vector4(v.z, v.x, v.y, v.x); }
        public static Vector4 zxyy(this Vector3 v) { return new Vector4(v.z, v.x, v.y, v.y); }
        public static Vector4 zxyz(this Vector3 v) { return new Vector4(v.z, v.x, v.y, v.z); }
        public static Vector4 zxyn(this Vector3 v, float n_0 = 0) { return new Vector4(v.z, v.x, v.y, n_0); }
        public static Vector4 zxzx(this Vector3 v) { return new Vector4(v.z, v.x, v.z, v.x); }
        public static Vector4 zxzy(this Vector3 v) { return new Vector4(v.z, v.x, v.z, v.y); }
        public static Vector4 zxzz(this Vector3 v) { return new Vector4(v.z, v.x, v.z, v.z); }
        public static Vector4 zxzn(this Vector3 v, float n_0 = 0) { return new Vector4(v.z, v.x, v.z, n_0); }
        public static Vector4 zxnx(this Vector3 v, float n_0 = 0) { return new Vector4(v.z, v.x, n_0, v.x); }
        public static Vector4 zxny(this Vector3 v, float n_0 = 0) { return new Vector4(v.z, v.x, n_0, v.y); }
        public static Vector4 zxnz(this Vector3 v, float n_0 = 0) { return new Vector4(v.z, v.x, n_0, v.z); }
        public static Vector4 zxnn(this Vector3 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.z, v.x, n_0, n_1); }
        public static Vector4 zyxx(this Vector3 v) { return new Vector4(v.z, v.y, v.x, v.x); }
        public static Vector4 zyxy(this Vector3 v) { return new Vector4(v.z, v.y, v.x, v.y); }
        public static Vector4 zyxz(this Vector3 v) { return new Vector4(v.z, v.y, v.x, v.z); }
        public static Vector4 zyxn(this Vector3 v, float n_0 = 0) { return new Vector4(v.z, v.y, v.x, n_0); }
        public static Vector4 zyyx(this Vector3 v) { return new Vector4(v.z, v.y, v.y, v.x); }
        public static Vector4 zyyy(this Vector3 v) { return new Vector4(v.z, v.y, v.y, v.y); }
        public static Vector4 zyyz(this Vector3 v) { return new Vector4(v.z, v.y, v.y, v.z); }
        public static Vector4 zyyn(this Vector3 v, float n_0 = 0) { return new Vector4(v.z, v.y, v.y, n_0); }
        public static Vector4 zyzx(this Vector3 v) { return new Vector4(v.z, v.y, v.z, v.x); }
        public static Vector4 zyzy(this Vector3 v) { return new Vector4(v.z, v.y, v.z, v.y); }
        public static Vector4 zyzz(this Vector3 v) { return new Vector4(v.z, v.y, v.z, v.z); }
        public static Vector4 zyzn(this Vector3 v, float n_0 = 0) { return new Vector4(v.z, v.y, v.z, n_0); }
        public static Vector4 zynx(this Vector3 v, float n_0 = 0) { return new Vector4(v.z, v.y, n_0, v.x); }
        public static Vector4 zyny(this Vector3 v, float n_0 = 0) { return new Vector4(v.z, v.y, n_0, v.y); }
        public static Vector4 zynz(this Vector3 v, float n_0 = 0) { return new Vector4(v.z, v.y, n_0, v.z); }
        public static Vector4 zynn(this Vector3 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.z, v.y, n_0, n_1); }
        public static Vector4 zzxx(this Vector3 v) { return new Vector4(v.z, v.z, v.x, v.x); }
        public static Vector4 zzxy(this Vector3 v) { return new Vector4(v.z, v.z, v.x, v.y); }
        public static Vector4 zzxz(this Vector3 v) { return new Vector4(v.z, v.z, v.x, v.z); }
        public static Vector4 zzxn(this Vector3 v, float n_0 = 0) { return new Vector4(v.z, v.z, v.x, n_0); }
        public static Vector4 zzyx(this Vector3 v) { return new Vector4(v.z, v.z, v.y, v.x); }
        public static Vector4 zzyy(this Vector3 v) { return new Vector4(v.z, v.z, v.y, v.y); }
        public static Vector4 zzyz(this Vector3 v) { return new Vector4(v.z, v.z, v.y, v.z); }
        public static Vector4 zzyn(this Vector3 v, float n_0 = 0) { return new Vector4(v.z, v.z, v.y, n_0); }
        public static Vector4 zzzx(this Vector3 v) { return new Vector4(v.z, v.z, v.z, v.x); }
        public static Vector4 zzzy(this Vector3 v) { return new Vector4(v.z, v.z, v.z, v.y); }
        public static Vector4 zzzz(this Vector3 v) { return new Vector4(v.z, v.z, v.z, v.z); }
        public static Vector4 zzzn(this Vector3 v, float n_0 = 0) { return new Vector4(v.z, v.z, v.z, n_0); }
        public static Vector4 zznx(this Vector3 v, float n_0 = 0) { return new Vector4(v.z, v.z, n_0, v.x); }
        public static Vector4 zzny(this Vector3 v, float n_0 = 0) { return new Vector4(v.z, v.z, n_0, v.y); }
        public static Vector4 zznz(this Vector3 v, float n_0 = 0) { return new Vector4(v.z, v.z, n_0, v.z); }
        public static Vector4 zznn(this Vector3 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.z, v.z, n_0, n_1); }
        public static Vector4 znxx(this Vector3 v, float n_0 = 0) { return new Vector4(v.z, n_0, v.x, v.x); }
        public static Vector4 znxy(this Vector3 v, float n_0 = 0) { return new Vector4(v.z, n_0, v.x, v.y); }
        public static Vector4 znxz(this Vector3 v, float n_0 = 0) { return new Vector4(v.z, n_0, v.x, v.z); }
        public static Vector4 znxn(this Vector3 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.z, n_0, v.x, n_1); }
        public static Vector4 znyx(this Vector3 v, float n_0 = 0) { return new Vector4(v.z, n_0, v.y, v.x); }
        public static Vector4 znyy(this Vector3 v, float n_0 = 0) { return new Vector4(v.z, n_0, v.y, v.y); }
        public static Vector4 znyz(this Vector3 v, float n_0 = 0) { return new Vector4(v.z, n_0, v.y, v.z); }
        public static Vector4 znyn(this Vector3 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.z, n_0, v.y, n_1); }
        public static Vector4 znzx(this Vector3 v, float n_0 = 0) { return new Vector4(v.z, n_0, v.z, v.x); }
        public static Vector4 znzy(this Vector3 v, float n_0 = 0) { return new Vector4(v.z, n_0, v.z, v.y); }
        public static Vector4 znzz(this Vector3 v, float n_0 = 0) { return new Vector4(v.z, n_0, v.z, v.z); }
        public static Vector4 znzn(this Vector3 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.z, n_0, v.z, n_1); }
        public static Vector4 znnx(this Vector3 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.z, n_0, n_1, v.x); }
        public static Vector4 znny(this Vector3 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.z, n_0, n_1, v.y); }
        public static Vector4 znnz(this Vector3 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.z, n_0, n_1, v.z); }
        public static Vector4 znnn(this Vector3 v, float n_0 = 0, float n_1 = 0, float n_2 = 0) { return new Vector4(v.z, n_0, n_1, n_2); }
        public static Vector4 nxxx(this Vector3 v, float n_0 = 0) { return new Vector4(n_0, v.x, v.x, v.x); }
        public static Vector4 nxxy(this Vector3 v, float n_0 = 0) { return new Vector4(n_0, v.x, v.x, v.y); }
        public static Vector4 nxxz(this Vector3 v, float n_0 = 0) { return new Vector4(n_0, v.x, v.x, v.z); }
        public static Vector4 nxxn(this Vector3 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, v.x, v.x, n_1); }
        public static Vector4 nxyx(this Vector3 v, float n_0 = 0) { return new Vector4(n_0, v.x, v.y, v.x); }
        public static Vector4 nxyy(this Vector3 v, float n_0 = 0) { return new Vector4(n_0, v.x, v.y, v.y); }
        public static Vector4 nxyz(this Vector3 v, float n_0 = 0) { return new Vector4(n_0, v.x, v.y, v.z); }
        public static Vector4 nxyn(this Vector3 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, v.x, v.y, n_1); }
        public static Vector4 nxzx(this Vector3 v, float n_0 = 0) { return new Vector4(n_0, v.x, v.z, v.x); }
        public static Vector4 nxzy(this Vector3 v, float n_0 = 0) { return new Vector4(n_0, v.x, v.z, v.y); }
        public static Vector4 nxzz(this Vector3 v, float n_0 = 0) { return new Vector4(n_0, v.x, v.z, v.z); }
        public static Vector4 nxzn(this Vector3 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, v.x, v.z, n_1); }
        public static Vector4 nxnx(this Vector3 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, v.x, n_1, v.x); }
        public static Vector4 nxny(this Vector3 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, v.x, n_1, v.y); }
        public static Vector4 nxnz(this Vector3 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, v.x, n_1, v.z); }
        public static Vector4 nxnn(this Vector3 v, float n_0 = 0, float n_1 = 0, float n_2 = 0) { return new Vector4(n_0, v.x, n_1, n_2); }
        public static Vector4 nyxx(this Vector3 v, float n_0 = 0) { return new Vector4(n_0, v.y, v.x, v.x); }
        public static Vector4 nyxy(this Vector3 v, float n_0 = 0) { return new Vector4(n_0, v.y, v.x, v.y); }
        public static Vector4 nyxz(this Vector3 v, float n_0 = 0) { return new Vector4(n_0, v.y, v.x, v.z); }
        public static Vector4 nyxn(this Vector3 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, v.y, v.x, n_1); }
        public static Vector4 nyyx(this Vector3 v, float n_0 = 0) { return new Vector4(n_0, v.y, v.y, v.x); }
        public static Vector4 nyyy(this Vector3 v, float n_0 = 0) { return new Vector4(n_0, v.y, v.y, v.y); }
        public static Vector4 nyyz(this Vector3 v, float n_0 = 0) { return new Vector4(n_0, v.y, v.y, v.z); }
        public static Vector4 nyyn(this Vector3 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, v.y, v.y, n_1); }
        public static Vector4 nyzx(this Vector3 v, float n_0 = 0) { return new Vector4(n_0, v.y, v.z, v.x); }
        public static Vector4 nyzy(this Vector3 v, float n_0 = 0) { return new Vector4(n_0, v.y, v.z, v.y); }
        public static Vector4 nyzz(this Vector3 v, float n_0 = 0) { return new Vector4(n_0, v.y, v.z, v.z); }
        public static Vector4 nyzn(this Vector3 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, v.y, v.z, n_1); }
        public static Vector4 nynx(this Vector3 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, v.y, n_1, v.x); }
        public static Vector4 nyny(this Vector3 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, v.y, n_1, v.y); }
        public static Vector4 nynz(this Vector3 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, v.y, n_1, v.z); }
        public static Vector4 nynn(this Vector3 v, float n_0 = 0, float n_1 = 0, float n_2 = 0) { return new Vector4(n_0, v.y, n_1, n_2); }
        public static Vector4 nzxx(this Vector3 v, float n_0 = 0) { return new Vector4(n_0, v.z, v.x, v.x); }
        public static Vector4 nzxy(this Vector3 v, float n_0 = 0) { return new Vector4(n_0, v.z, v.x, v.y); }
        public static Vector4 nzxz(this Vector3 v, float n_0 = 0) { return new Vector4(n_0, v.z, v.x, v.z); }
        public static Vector4 nzxn(this Vector3 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, v.z, v.x, n_1); }
        public static Vector4 nzyx(this Vector3 v, float n_0 = 0) { return new Vector4(n_0, v.z, v.y, v.x); }
        public static Vector4 nzyy(this Vector3 v, float n_0 = 0) { return new Vector4(n_0, v.z, v.y, v.y); }
        public static Vector4 nzyz(this Vector3 v, float n_0 = 0) { return new Vector4(n_0, v.z, v.y, v.z); }
        public static Vector4 nzyn(this Vector3 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, v.z, v.y, n_1); }
        public static Vector4 nzzx(this Vector3 v, float n_0 = 0) { return new Vector4(n_0, v.z, v.z, v.x); }
        public static Vector4 nzzy(this Vector3 v, float n_0 = 0) { return new Vector4(n_0, v.z, v.z, v.y); }
        public static Vector4 nzzz(this Vector3 v, float n_0 = 0) { return new Vector4(n_0, v.z, v.z, v.z); }
        public static Vector4 nzzn(this Vector3 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, v.z, v.z, n_1); }
        public static Vector4 nznx(this Vector3 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, v.z, n_1, v.x); }
        public static Vector4 nzny(this Vector3 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, v.z, n_1, v.y); }
        public static Vector4 nznz(this Vector3 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, v.z, n_1, v.z); }
        public static Vector4 nznn(this Vector3 v, float n_0 = 0, float n_1 = 0, float n_2 = 0) { return new Vector4(n_0, v.z, n_1, n_2); }
        public static Vector4 nnxx(this Vector3 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, n_1, v.x, v.x); }
        public static Vector4 nnxy(this Vector3 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, n_1, v.x, v.y); }
        public static Vector4 nnxz(this Vector3 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, n_1, v.x, v.z); }
        public static Vector4 nnxn(this Vector3 v, float n_0 = 0, float n_1 = 0, float n_2 = 0) { return new Vector4(n_0, n_1, v.x, n_2); }
        public static Vector4 nnyx(this Vector3 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, n_1, v.y, v.x); }
        public static Vector4 nnyy(this Vector3 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, n_1, v.y, v.y); }
        public static Vector4 nnyz(this Vector3 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, n_1, v.y, v.z); }
        public static Vector4 nnyn(this Vector3 v, float n_0 = 0, float n_1 = 0, float n_2 = 0) { return new Vector4(n_0, n_1, v.y, n_2); }
        public static Vector4 nnzx(this Vector3 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, n_1, v.z, v.x); }
        public static Vector4 nnzy(this Vector3 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, n_1, v.z, v.y); }
        public static Vector4 nnzz(this Vector3 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, n_1, v.z, v.z); }
        public static Vector4 nnzn(this Vector3 v, float n_0 = 0, float n_1 = 0, float n_2 = 0) { return new Vector4(n_0, n_1, v.z, n_2); }
        public static Vector4 nnnx(this Vector3 v, float n_0 = 0, float n_1 = 0, float n_2 = 0) { return new Vector4(n_0, n_1, n_2, v.x); }
        public static Vector4 nnny(this Vector3 v, float n_0 = 0, float n_1 = 0, float n_2 = 0) { return new Vector4(n_0, n_1, n_2, v.y); }
        public static Vector4 nnnz(this Vector3 v, float n_0 = 0, float n_1 = 0, float n_2 = 0) { return new Vector4(n_0, n_1, n_2, v.z); }
        #endregion Swizzle Vector3 to Vector4
        
        //---------------------------------------------------------------------
        #region Swizzle Vector4 to Vector2
        public static Vector2 xx(this Vector4 v) { return new Vector2(v.x, v.x); }
        public static Vector2 xy(this Vector4 v) { return new Vector2(v.x, v.y); }
        public static Vector2 xz(this Vector4 v) { return new Vector2(v.x, v.z); }
        public static Vector2 xw(this Vector4 v) { return new Vector2(v.x, v.w); }
        public static Vector2 xn(this Vector4 v, float n_0 = 0) { return new Vector2(v.x, n_0); }
        public static Vector2 yx(this Vector4 v) { return new Vector2(v.y, v.x); }
        public static Vector2 yy(this Vector4 v) { return new Vector2(v.y, v.y); }
        public static Vector2 yz(this Vector4 v) { return new Vector2(v.y, v.z); }
        public static Vector2 yw(this Vector4 v) { return new Vector2(v.y, v.w); }
        public static Vector2 yn(this Vector4 v, float n_0 = 0) { return new Vector2(v.y, n_0); }
        public static Vector2 zx(this Vector4 v) { return new Vector2(v.z, v.x); }
        public static Vector2 zy(this Vector4 v) { return new Vector2(v.z, v.y); }
        public static Vector2 zz(this Vector4 v) { return new Vector2(v.z, v.z); }
        public static Vector2 zw(this Vector4 v) { return new Vector2(v.z, v.w); }
        public static Vector2 zn(this Vector4 v, float n_0 = 0) { return new Vector2(v.z, n_0); }
        public static Vector2 wx(this Vector4 v) { return new Vector2(v.w, v.x); }
        public static Vector2 wy(this Vector4 v) { return new Vector2(v.w, v.y); }
        public static Vector2 wz(this Vector4 v) { return new Vector2(v.w, v.z); }
        public static Vector2 ww(this Vector4 v) { return new Vector2(v.w, v.w); }
        public static Vector2 wn(this Vector4 v, float n_0 = 0) { return new Vector2(v.w, n_0); }
        public static Vector2 nx(this Vector4 v, float n_0 = 0) { return new Vector2(n_0, v.x); }
        public static Vector2 ny(this Vector4 v, float n_0 = 0) { return new Vector2(n_0, v.y); }
        public static Vector2 nz(this Vector4 v, float n_0 = 0) { return new Vector2(n_0, v.z); }
        public static Vector2 nw(this Vector4 v, float n_0 = 0) { return new Vector2(n_0, v.w); }
        #endregion Swizzle Vector4 to Vector2
        
        //---------------------------------------------------------------------
        #region Swizzle Vector4 to Vector3
        public static Vector3 xxx(this Vector4 v) { return new Vector3(v.x, v.x, v.x); }
        public static Vector3 xxy(this Vector4 v) { return new Vector3(v.x, v.x, v.y); }
        public static Vector3 xxz(this Vector4 v) { return new Vector3(v.x, v.x, v.z); }
        public static Vector3 xxw(this Vector4 v) { return new Vector3(v.x, v.x, v.w); }
        public static Vector3 xxn(this Vector4 v, float n_0 = 0) { return new Vector3(v.x, v.x, n_0); }
        public static Vector3 xyx(this Vector4 v) { return new Vector3(v.x, v.y, v.x); }
        public static Vector3 xyy(this Vector4 v) { return new Vector3(v.x, v.y, v.y); }
        public static Vector3 xyz(this Vector4 v) { return new Vector3(v.x, v.y, v.z); }
        public static Vector3 xyw(this Vector4 v) { return new Vector3(v.x, v.y, v.w); }
        public static Vector3 xyn(this Vector4 v, float n_0 = 0) { return new Vector3(v.x, v.y, n_0); }
        public static Vector3 xzx(this Vector4 v) { return new Vector3(v.x, v.z, v.x); }
        public static Vector3 xzy(this Vector4 v) { return new Vector3(v.x, v.z, v.y); }
        public static Vector3 xzz(this Vector4 v) { return new Vector3(v.x, v.z, v.z); }
        public static Vector3 xzw(this Vector4 v) { return new Vector3(v.x, v.z, v.w); }
        public static Vector3 xzn(this Vector4 v, float n_0 = 0) { return new Vector3(v.x, v.z, n_0); }
        public static Vector3 xwx(this Vector4 v) { return new Vector3(v.x, v.w, v.x); }
        public static Vector3 xwy(this Vector4 v) { return new Vector3(v.x, v.w, v.y); }
        public static Vector3 xwz(this Vector4 v) { return new Vector3(v.x, v.w, v.z); }
        public static Vector3 xww(this Vector4 v) { return new Vector3(v.x, v.w, v.w); }
        public static Vector3 xwn(this Vector4 v, float n_0 = 0) { return new Vector3(v.x, v.w, n_0); }
        public static Vector3 xnx(this Vector4 v, float n_0 = 0) { return new Vector3(v.x, n_0, v.x); }
        public static Vector3 xny(this Vector4 v, float n_0 = 0) { return new Vector3(v.x, n_0, v.y); }
        public static Vector3 xnz(this Vector4 v, float n_0 = 0) { return new Vector3(v.x, n_0, v.z); }
        public static Vector3 xnw(this Vector4 v, float n_0 = 0) { return new Vector3(v.x, n_0, v.w); }
        public static Vector3 xnn(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector3(v.x, n_0, n_1); }
        public static Vector3 yxx(this Vector4 v) { return new Vector3(v.y, v.x, v.x); }
        public static Vector3 yxy(this Vector4 v) { return new Vector3(v.y, v.x, v.y); }
        public static Vector3 yxz(this Vector4 v) { return new Vector3(v.y, v.x, v.z); }
        public static Vector3 yxw(this Vector4 v) { return new Vector3(v.y, v.x, v.w); }
        public static Vector3 yxn(this Vector4 v, float n_0 = 0) { return new Vector3(v.y, v.x, n_0); }
        public static Vector3 yyx(this Vector4 v) { return new Vector3(v.y, v.y, v.x); }
        public static Vector3 yyy(this Vector4 v) { return new Vector3(v.y, v.y, v.y); }
        public static Vector3 yyz(this Vector4 v) { return new Vector3(v.y, v.y, v.z); }
        public static Vector3 yyw(this Vector4 v) { return new Vector3(v.y, v.y, v.w); }
        public static Vector3 yyn(this Vector4 v, float n_0 = 0) { return new Vector3(v.y, v.y, n_0); }
        public static Vector3 yzx(this Vector4 v) { return new Vector3(v.y, v.z, v.x); }
        public static Vector3 yzy(this Vector4 v) { return new Vector3(v.y, v.z, v.y); }
        public static Vector3 yzz(this Vector4 v) { return new Vector3(v.y, v.z, v.z); }
        public static Vector3 yzw(this Vector4 v) { return new Vector3(v.y, v.z, v.w); }
        public static Vector3 yzn(this Vector4 v, float n_0 = 0) { return new Vector3(v.y, v.z, n_0); }
        public static Vector3 ywx(this Vector4 v) { return new Vector3(v.y, v.w, v.x); }
        public static Vector3 ywy(this Vector4 v) { return new Vector3(v.y, v.w, v.y); }
        public static Vector3 ywz(this Vector4 v) { return new Vector3(v.y, v.w, v.z); }
        public static Vector3 yww(this Vector4 v) { return new Vector3(v.y, v.w, v.w); }
        public static Vector3 ywn(this Vector4 v, float n_0 = 0) { return new Vector3(v.y, v.w, n_0); }
        public static Vector3 ynx(this Vector4 v, float n_0 = 0) { return new Vector3(v.y, n_0, v.x); }
        public static Vector3 yny(this Vector4 v, float n_0 = 0) { return new Vector3(v.y, n_0, v.y); }
        public static Vector3 ynz(this Vector4 v, float n_0 = 0) { return new Vector3(v.y, n_0, v.z); }
        public static Vector3 ynw(this Vector4 v, float n_0 = 0) { return new Vector3(v.y, n_0, v.w); }
        public static Vector3 ynn(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector3(v.y, n_0, n_1); }
        public static Vector3 zxx(this Vector4 v) { return new Vector3(v.z, v.x, v.x); }
        public static Vector3 zxy(this Vector4 v) { return new Vector3(v.z, v.x, v.y); }
        public static Vector3 zxz(this Vector4 v) { return new Vector3(v.z, v.x, v.z); }
        public static Vector3 zxw(this Vector4 v) { return new Vector3(v.z, v.x, v.w); }
        public static Vector3 zxn(this Vector4 v, float n_0 = 0) { return new Vector3(v.z, v.x, n_0); }
        public static Vector3 zyx(this Vector4 v) { return new Vector3(v.z, v.y, v.x); }
        public static Vector3 zyy(this Vector4 v) { return new Vector3(v.z, v.y, v.y); }
        public static Vector3 zyz(this Vector4 v) { return new Vector3(v.z, v.y, v.z); }
        public static Vector3 zyw(this Vector4 v) { return new Vector3(v.z, v.y, v.w); }
        public static Vector3 zyn(this Vector4 v, float n_0 = 0) { return new Vector3(v.z, v.y, n_0); }
        public static Vector3 zzx(this Vector4 v) { return new Vector3(v.z, v.z, v.x); }
        public static Vector3 zzy(this Vector4 v) { return new Vector3(v.z, v.z, v.y); }
        public static Vector3 zzz(this Vector4 v) { return new Vector3(v.z, v.z, v.z); }
        public static Vector3 zzw(this Vector4 v) { return new Vector3(v.z, v.z, v.w); }
        public static Vector3 zzn(this Vector4 v, float n_0 = 0) { return new Vector3(v.z, v.z, n_0); }
        public static Vector3 zwx(this Vector4 v) { return new Vector3(v.z, v.w, v.x); }
        public static Vector3 zwy(this Vector4 v) { return new Vector3(v.z, v.w, v.y); }
        public static Vector3 zwz(this Vector4 v) { return new Vector3(v.z, v.w, v.z); }
        public static Vector3 zww(this Vector4 v) { return new Vector3(v.z, v.w, v.w); }
        public static Vector3 zwn(this Vector4 v, float n_0 = 0) { return new Vector3(v.z, v.w, n_0); }
        public static Vector3 znx(this Vector4 v, float n_0 = 0) { return new Vector3(v.z, n_0, v.x); }
        public static Vector3 zny(this Vector4 v, float n_0 = 0) { return new Vector3(v.z, n_0, v.y); }
        public static Vector3 znz(this Vector4 v, float n_0 = 0) { return new Vector3(v.z, n_0, v.z); }
        public static Vector3 znw(this Vector4 v, float n_0 = 0) { return new Vector3(v.z, n_0, v.w); }
        public static Vector3 znn(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector3(v.z, n_0, n_1); }
        public static Vector3 wxx(this Vector4 v) { return new Vector3(v.w, v.x, v.x); }
        public static Vector3 wxy(this Vector4 v) { return new Vector3(v.w, v.x, v.y); }
        public static Vector3 wxz(this Vector4 v) { return new Vector3(v.w, v.x, v.z); }
        public static Vector3 wxw(this Vector4 v) { return new Vector3(v.w, v.x, v.w); }
        public static Vector3 wxn(this Vector4 v, float n_0 = 0) { return new Vector3(v.w, v.x, n_0); }
        public static Vector3 wyx(this Vector4 v) { return new Vector3(v.w, v.y, v.x); }
        public static Vector3 wyy(this Vector4 v) { return new Vector3(v.w, v.y, v.y); }
        public static Vector3 wyz(this Vector4 v) { return new Vector3(v.w, v.y, v.z); }
        public static Vector3 wyw(this Vector4 v) { return new Vector3(v.w, v.y, v.w); }
        public static Vector3 wyn(this Vector4 v, float n_0 = 0) { return new Vector3(v.w, v.y, n_0); }
        public static Vector3 wzx(this Vector4 v) { return new Vector3(v.w, v.z, v.x); }
        public static Vector3 wzy(this Vector4 v) { return new Vector3(v.w, v.z, v.y); }
        public static Vector3 wzz(this Vector4 v) { return new Vector3(v.w, v.z, v.z); }
        public static Vector3 wzw(this Vector4 v) { return new Vector3(v.w, v.z, v.w); }
        public static Vector3 wzn(this Vector4 v, float n_0 = 0) { return new Vector3(v.w, v.z, n_0); }
        public static Vector3 wwx(this Vector4 v) { return new Vector3(v.w, v.w, v.x); }
        public static Vector3 wwy(this Vector4 v) { return new Vector3(v.w, v.w, v.y); }
        public static Vector3 wwz(this Vector4 v) { return new Vector3(v.w, v.w, v.z); }
        public static Vector3 www(this Vector4 v) { return new Vector3(v.w, v.w, v.w); }
        public static Vector3 wwn(this Vector4 v, float n_0 = 0) { return new Vector3(v.w, v.w, n_0); }
        public static Vector3 wnx(this Vector4 v, float n_0 = 0) { return new Vector3(v.w, n_0, v.x); }
        public static Vector3 wny(this Vector4 v, float n_0 = 0) { return new Vector3(v.w, n_0, v.y); }
        public static Vector3 wnz(this Vector4 v, float n_0 = 0) { return new Vector3(v.w, n_0, v.z); }
        public static Vector3 wnw(this Vector4 v, float n_0 = 0) { return new Vector3(v.w, n_0, v.w); }
        public static Vector3 wnn(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector3(v.w, n_0, n_1); }
        public static Vector3 nxx(this Vector4 v, float n_0 = 0) { return new Vector3(n_0, v.x, v.x); }
        public static Vector3 nxy(this Vector4 v, float n_0 = 0) { return new Vector3(n_0, v.x, v.y); }
        public static Vector3 nxz(this Vector4 v, float n_0 = 0) { return new Vector3(n_0, v.x, v.z); }
        public static Vector3 nxw(this Vector4 v, float n_0 = 0) { return new Vector3(n_0, v.x, v.w); }
        public static Vector3 nxn(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector3(n_0, v.x, n_1); }
        public static Vector3 nyx(this Vector4 v, float n_0 = 0) { return new Vector3(n_0, v.y, v.x); }
        public static Vector3 nyy(this Vector4 v, float n_0 = 0) { return new Vector3(n_0, v.y, v.y); }
        public static Vector3 nyz(this Vector4 v, float n_0 = 0) { return new Vector3(n_0, v.y, v.z); }
        public static Vector3 nyw(this Vector4 v, float n_0 = 0) { return new Vector3(n_0, v.y, v.w); }
        public static Vector3 nyn(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector3(n_0, v.y, n_1); }
        public static Vector3 nzx(this Vector4 v, float n_0 = 0) { return new Vector3(n_0, v.z, v.x); }
        public static Vector3 nzy(this Vector4 v, float n_0 = 0) { return new Vector3(n_0, v.z, v.y); }
        public static Vector3 nzz(this Vector4 v, float n_0 = 0) { return new Vector3(n_0, v.z, v.z); }
        public static Vector3 nzw(this Vector4 v, float n_0 = 0) { return new Vector3(n_0, v.z, v.w); }
        public static Vector3 nzn(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector3(n_0, v.z, n_1); }
        public static Vector3 nwx(this Vector4 v, float n_0 = 0) { return new Vector3(n_0, v.w, v.x); }
        public static Vector3 nwy(this Vector4 v, float n_0 = 0) { return new Vector3(n_0, v.w, v.y); }
        public static Vector3 nwz(this Vector4 v, float n_0 = 0) { return new Vector3(n_0, v.w, v.z); }
        public static Vector3 nww(this Vector4 v, float n_0 = 0) { return new Vector3(n_0, v.w, v.w); }
        public static Vector3 nwn(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector3(n_0, v.w, n_1); }
        public static Vector3 nnx(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector3(n_0, n_1, v.x); }
        public static Vector3 nny(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector3(n_0, n_1, v.y); }
        public static Vector3 nnz(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector3(n_0, n_1, v.z); }
        public static Vector3 nnw(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector3(n_0, n_1, v.w); }
        #endregion Swizzle Vector4 to Vector3
        
        //---------------------------------------------------------------------
        #region Swizzle Vector4 to Vector4
        public static Vector4 xxxx(this Vector4 v) { return new Vector4(v.x, v.x, v.x, v.x); }
        public static Vector4 xxxy(this Vector4 v) { return new Vector4(v.x, v.x, v.x, v.y); }
        public static Vector4 xxxz(this Vector4 v) { return new Vector4(v.x, v.x, v.x, v.z); }
        public static Vector4 xxxw(this Vector4 v) { return new Vector4(v.x, v.x, v.x, v.w); }
        public static Vector4 xxxn(this Vector4 v, float n_0 = 0) { return new Vector4(v.x, v.x, v.x, n_0); }
        public static Vector4 xxyx(this Vector4 v) { return new Vector4(v.x, v.x, v.y, v.x); }
        public static Vector4 xxyy(this Vector4 v) { return new Vector4(v.x, v.x, v.y, v.y); }
        public static Vector4 xxyz(this Vector4 v) { return new Vector4(v.x, v.x, v.y, v.z); }
        public static Vector4 xxyw(this Vector4 v) { return new Vector4(v.x, v.x, v.y, v.w); }
        public static Vector4 xxyn(this Vector4 v, float n_0 = 0) { return new Vector4(v.x, v.x, v.y, n_0); }
        public static Vector4 xxzx(this Vector4 v) { return new Vector4(v.x, v.x, v.z, v.x); }
        public static Vector4 xxzy(this Vector4 v) { return new Vector4(v.x, v.x, v.z, v.y); }
        public static Vector4 xxzz(this Vector4 v) { return new Vector4(v.x, v.x, v.z, v.z); }
        public static Vector4 xxzw(this Vector4 v) { return new Vector4(v.x, v.x, v.z, v.w); }
        public static Vector4 xxzn(this Vector4 v, float n_0 = 0) { return new Vector4(v.x, v.x, v.z, n_0); }
        public static Vector4 xxwx(this Vector4 v) { return new Vector4(v.x, v.x, v.w, v.x); }
        public static Vector4 xxwy(this Vector4 v) { return new Vector4(v.x, v.x, v.w, v.y); }
        public static Vector4 xxwz(this Vector4 v) { return new Vector4(v.x, v.x, v.w, v.z); }
        public static Vector4 xxww(this Vector4 v) { return new Vector4(v.x, v.x, v.w, v.w); }
        public static Vector4 xxwn(this Vector4 v, float n_0 = 0) { return new Vector4(v.x, v.x, v.w, n_0); }
        public static Vector4 xxnx(this Vector4 v, float n_0 = 0) { return new Vector4(v.x, v.x, n_0, v.x); }
        public static Vector4 xxny(this Vector4 v, float n_0 = 0) { return new Vector4(v.x, v.x, n_0, v.y); }
        public static Vector4 xxnz(this Vector4 v, float n_0 = 0) { return new Vector4(v.x, v.x, n_0, v.z); }
        public static Vector4 xxnw(this Vector4 v, float n_0 = 0) { return new Vector4(v.x, v.x, n_0, v.w); }
        public static Vector4 xxnn(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.x, v.x, n_0, n_1); }
        public static Vector4 xyxx(this Vector4 v) { return new Vector4(v.x, v.y, v.x, v.x); }
        public static Vector4 xyxy(this Vector4 v) { return new Vector4(v.x, v.y, v.x, v.y); }
        public static Vector4 xyxz(this Vector4 v) { return new Vector4(v.x, v.y, v.x, v.z); }
        public static Vector4 xyxw(this Vector4 v) { return new Vector4(v.x, v.y, v.x, v.w); }
        public static Vector4 xyxn(this Vector4 v, float n_0 = 0) { return new Vector4(v.x, v.y, v.x, n_0); }
        public static Vector4 xyyx(this Vector4 v) { return new Vector4(v.x, v.y, v.y, v.x); }
        public static Vector4 xyyy(this Vector4 v) { return new Vector4(v.x, v.y, v.y, v.y); }
        public static Vector4 xyyz(this Vector4 v) { return new Vector4(v.x, v.y, v.y, v.z); }
        public static Vector4 xyyw(this Vector4 v) { return new Vector4(v.x, v.y, v.y, v.w); }
        public static Vector4 xyyn(this Vector4 v, float n_0 = 0) { return new Vector4(v.x, v.y, v.y, n_0); }
        public static Vector4 xyzx(this Vector4 v) { return new Vector4(v.x, v.y, v.z, v.x); }
        public static Vector4 xyzy(this Vector4 v) { return new Vector4(v.x, v.y, v.z, v.y); }
        public static Vector4 xyzz(this Vector4 v) { return new Vector4(v.x, v.y, v.z, v.z); }
        public static Vector4 xyzw(this Vector4 v) { return new Vector4(v.x, v.y, v.z, v.w); }
        public static Vector4 xyzn(this Vector4 v, float n_0 = 0) { return new Vector4(v.x, v.y, v.z, n_0); }
        public static Vector4 xywx(this Vector4 v) { return new Vector4(v.x, v.y, v.w, v.x); }
        public static Vector4 xywy(this Vector4 v) { return new Vector4(v.x, v.y, v.w, v.y); }
        public static Vector4 xywz(this Vector4 v) { return new Vector4(v.x, v.y, v.w, v.z); }
        public static Vector4 xyww(this Vector4 v) { return new Vector4(v.x, v.y, v.w, v.w); }
        public static Vector4 xywn(this Vector4 v, float n_0 = 0) { return new Vector4(v.x, v.y, v.w, n_0); }
        public static Vector4 xynx(this Vector4 v, float n_0 = 0) { return new Vector4(v.x, v.y, n_0, v.x); }
        public static Vector4 xyny(this Vector4 v, float n_0 = 0) { return new Vector4(v.x, v.y, n_0, v.y); }
        public static Vector4 xynz(this Vector4 v, float n_0 = 0) { return new Vector4(v.x, v.y, n_0, v.z); }
        public static Vector4 xynw(this Vector4 v, float n_0 = 0) { return new Vector4(v.x, v.y, n_0, v.w); }
        public static Vector4 xynn(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.x, v.y, n_0, n_1); }
        public static Vector4 xzxx(this Vector4 v) { return new Vector4(v.x, v.z, v.x, v.x); }
        public static Vector4 xzxy(this Vector4 v) { return new Vector4(v.x, v.z, v.x, v.y); }
        public static Vector4 xzxz(this Vector4 v) { return new Vector4(v.x, v.z, v.x, v.z); }
        public static Vector4 xzxw(this Vector4 v) { return new Vector4(v.x, v.z, v.x, v.w); }
        public static Vector4 xzxn(this Vector4 v, float n_0 = 0) { return new Vector4(v.x, v.z, v.x, n_0); }
        public static Vector4 xzyx(this Vector4 v) { return new Vector4(v.x, v.z, v.y, v.x); }
        public static Vector4 xzyy(this Vector4 v) { return new Vector4(v.x, v.z, v.y, v.y); }
        public static Vector4 xzyz(this Vector4 v) { return new Vector4(v.x, v.z, v.y, v.z); }
        public static Vector4 xzyw(this Vector4 v) { return new Vector4(v.x, v.z, v.y, v.w); }
        public static Vector4 xzyn(this Vector4 v, float n_0 = 0) { return new Vector4(v.x, v.z, v.y, n_0); }
        public static Vector4 xzzx(this Vector4 v) { return new Vector4(v.x, v.z, v.z, v.x); }
        public static Vector4 xzzy(this Vector4 v) { return new Vector4(v.x, v.z, v.z, v.y); }
        public static Vector4 xzzz(this Vector4 v) { return new Vector4(v.x, v.z, v.z, v.z); }
        public static Vector4 xzzw(this Vector4 v) { return new Vector4(v.x, v.z, v.z, v.w); }
        public static Vector4 xzzn(this Vector4 v, float n_0 = 0) { return new Vector4(v.x, v.z, v.z, n_0); }
        public static Vector4 xzwx(this Vector4 v) { return new Vector4(v.x, v.z, v.w, v.x); }
        public static Vector4 xzwy(this Vector4 v) { return new Vector4(v.x, v.z, v.w, v.y); }
        public static Vector4 xzwz(this Vector4 v) { return new Vector4(v.x, v.z, v.w, v.z); }
        public static Vector4 xzww(this Vector4 v) { return new Vector4(v.x, v.z, v.w, v.w); }
        public static Vector4 xzwn(this Vector4 v, float n_0 = 0) { return new Vector4(v.x, v.z, v.w, n_0); }
        public static Vector4 xznx(this Vector4 v, float n_0 = 0) { return new Vector4(v.x, v.z, n_0, v.x); }
        public static Vector4 xzny(this Vector4 v, float n_0 = 0) { return new Vector4(v.x, v.z, n_0, v.y); }
        public static Vector4 xznz(this Vector4 v, float n_0 = 0) { return new Vector4(v.x, v.z, n_0, v.z); }
        public static Vector4 xznw(this Vector4 v, float n_0 = 0) { return new Vector4(v.x, v.z, n_0, v.w); }
        public static Vector4 xznn(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.x, v.z, n_0, n_1); }
        public static Vector4 xwxx(this Vector4 v) { return new Vector4(v.x, v.w, v.x, v.x); }
        public static Vector4 xwxy(this Vector4 v) { return new Vector4(v.x, v.w, v.x, v.y); }
        public static Vector4 xwxz(this Vector4 v) { return new Vector4(v.x, v.w, v.x, v.z); }
        public static Vector4 xwxw(this Vector4 v) { return new Vector4(v.x, v.w, v.x, v.w); }
        public static Vector4 xwxn(this Vector4 v, float n_0 = 0) { return new Vector4(v.x, v.w, v.x, n_0); }
        public static Vector4 xwyx(this Vector4 v) { return new Vector4(v.x, v.w, v.y, v.x); }
        public static Vector4 xwyy(this Vector4 v) { return new Vector4(v.x, v.w, v.y, v.y); }
        public static Vector4 xwyz(this Vector4 v) { return new Vector4(v.x, v.w, v.y, v.z); }
        public static Vector4 xwyw(this Vector4 v) { return new Vector4(v.x, v.w, v.y, v.w); }
        public static Vector4 xwyn(this Vector4 v, float n_0 = 0) { return new Vector4(v.x, v.w, v.y, n_0); }
        public static Vector4 xwzx(this Vector4 v) { return new Vector4(v.x, v.w, v.z, v.x); }
        public static Vector4 xwzy(this Vector4 v) { return new Vector4(v.x, v.w, v.z, v.y); }
        public static Vector4 xwzz(this Vector4 v) { return new Vector4(v.x, v.w, v.z, v.z); }
        public static Vector4 xwzw(this Vector4 v) { return new Vector4(v.x, v.w, v.z, v.w); }
        public static Vector4 xwzn(this Vector4 v, float n_0 = 0) { return new Vector4(v.x, v.w, v.z, n_0); }
        public static Vector4 xwwx(this Vector4 v) { return new Vector4(v.x, v.w, v.w, v.x); }
        public static Vector4 xwwy(this Vector4 v) { return new Vector4(v.x, v.w, v.w, v.y); }
        public static Vector4 xwwz(this Vector4 v) { return new Vector4(v.x, v.w, v.w, v.z); }
        public static Vector4 xwww(this Vector4 v) { return new Vector4(v.x, v.w, v.w, v.w); }
        public static Vector4 xwwn(this Vector4 v, float n_0 = 0) { return new Vector4(v.x, v.w, v.w, n_0); }
        public static Vector4 xwnx(this Vector4 v, float n_0 = 0) { return new Vector4(v.x, v.w, n_0, v.x); }
        public static Vector4 xwny(this Vector4 v, float n_0 = 0) { return new Vector4(v.x, v.w, n_0, v.y); }
        public static Vector4 xwnz(this Vector4 v, float n_0 = 0) { return new Vector4(v.x, v.w, n_0, v.z); }
        public static Vector4 xwnw(this Vector4 v, float n_0 = 0) { return new Vector4(v.x, v.w, n_0, v.w); }
        public static Vector4 xwnn(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.x, v.w, n_0, n_1); }
        public static Vector4 xnxx(this Vector4 v, float n_0 = 0) { return new Vector4(v.x, n_0, v.x, v.x); }
        public static Vector4 xnxy(this Vector4 v, float n_0 = 0) { return new Vector4(v.x, n_0, v.x, v.y); }
        public static Vector4 xnxz(this Vector4 v, float n_0 = 0) { return new Vector4(v.x, n_0, v.x, v.z); }
        public static Vector4 xnxw(this Vector4 v, float n_0 = 0) { return new Vector4(v.x, n_0, v.x, v.w); }
        public static Vector4 xnxn(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.x, n_0, v.x, n_1); }
        public static Vector4 xnyx(this Vector4 v, float n_0 = 0) { return new Vector4(v.x, n_0, v.y, v.x); }
        public static Vector4 xnyy(this Vector4 v, float n_0 = 0) { return new Vector4(v.x, n_0, v.y, v.y); }
        public static Vector4 xnyz(this Vector4 v, float n_0 = 0) { return new Vector4(v.x, n_0, v.y, v.z); }
        public static Vector4 xnyw(this Vector4 v, float n_0 = 0) { return new Vector4(v.x, n_0, v.y, v.w); }
        public static Vector4 xnyn(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.x, n_0, v.y, n_1); }
        public static Vector4 xnzx(this Vector4 v, float n_0 = 0) { return new Vector4(v.x, n_0, v.z, v.x); }
        public static Vector4 xnzy(this Vector4 v, float n_0 = 0) { return new Vector4(v.x, n_0, v.z, v.y); }
        public static Vector4 xnzz(this Vector4 v, float n_0 = 0) { return new Vector4(v.x, n_0, v.z, v.z); }
        public static Vector4 xnzw(this Vector4 v, float n_0 = 0) { return new Vector4(v.x, n_0, v.z, v.w); }
        public static Vector4 xnzn(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.x, n_0, v.z, n_1); }
        public static Vector4 xnwx(this Vector4 v, float n_0 = 0) { return new Vector4(v.x, n_0, v.w, v.x); }
        public static Vector4 xnwy(this Vector4 v, float n_0 = 0) { return new Vector4(v.x, n_0, v.w, v.y); }
        public static Vector4 xnwz(this Vector4 v, float n_0 = 0) { return new Vector4(v.x, n_0, v.w, v.z); }
        public static Vector4 xnww(this Vector4 v, float n_0 = 0) { return new Vector4(v.x, n_0, v.w, v.w); }
        public static Vector4 xnwn(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.x, n_0, v.w, n_1); }
        public static Vector4 xnnx(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.x, n_0, n_1, v.x); }
        public static Vector4 xnny(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.x, n_0, n_1, v.y); }
        public static Vector4 xnnz(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.x, n_0, n_1, v.z); }
        public static Vector4 xnnw(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.x, n_0, n_1, v.w); }
        public static Vector4 xnnn(this Vector4 v, float n_0 = 0, float n_1 = 0, float n_2 = 0) { return new Vector4(v.x, n_0, n_1, n_2); }
        public static Vector4 yxxx(this Vector4 v) { return new Vector4(v.y, v.x, v.x, v.x); }
        public static Vector4 yxxy(this Vector4 v) { return new Vector4(v.y, v.x, v.x, v.y); }
        public static Vector4 yxxz(this Vector4 v) { return new Vector4(v.y, v.x, v.x, v.z); }
        public static Vector4 yxxw(this Vector4 v) { return new Vector4(v.y, v.x, v.x, v.w); }
        public static Vector4 yxxn(this Vector4 v, float n_0 = 0) { return new Vector4(v.y, v.x, v.x, n_0); }
        public static Vector4 yxyx(this Vector4 v) { return new Vector4(v.y, v.x, v.y, v.x); }
        public static Vector4 yxyy(this Vector4 v) { return new Vector4(v.y, v.x, v.y, v.y); }
        public static Vector4 yxyz(this Vector4 v) { return new Vector4(v.y, v.x, v.y, v.z); }
        public static Vector4 yxyw(this Vector4 v) { return new Vector4(v.y, v.x, v.y, v.w); }
        public static Vector4 yxyn(this Vector4 v, float n_0 = 0) { return new Vector4(v.y, v.x, v.y, n_0); }
        public static Vector4 yxzx(this Vector4 v) { return new Vector4(v.y, v.x, v.z, v.x); }
        public static Vector4 yxzy(this Vector4 v) { return new Vector4(v.y, v.x, v.z, v.y); }
        public static Vector4 yxzz(this Vector4 v) { return new Vector4(v.y, v.x, v.z, v.z); }
        public static Vector4 yxzw(this Vector4 v) { return new Vector4(v.y, v.x, v.z, v.w); }
        public static Vector4 yxzn(this Vector4 v, float n_0 = 0) { return new Vector4(v.y, v.x, v.z, n_0); }
        public static Vector4 yxwx(this Vector4 v) { return new Vector4(v.y, v.x, v.w, v.x); }
        public static Vector4 yxwy(this Vector4 v) { return new Vector4(v.y, v.x, v.w, v.y); }
        public static Vector4 yxwz(this Vector4 v) { return new Vector4(v.y, v.x, v.w, v.z); }
        public static Vector4 yxww(this Vector4 v) { return new Vector4(v.y, v.x, v.w, v.w); }
        public static Vector4 yxwn(this Vector4 v, float n_0 = 0) { return new Vector4(v.y, v.x, v.w, n_0); }
        public static Vector4 yxnx(this Vector4 v, float n_0 = 0) { return new Vector4(v.y, v.x, n_0, v.x); }
        public static Vector4 yxny(this Vector4 v, float n_0 = 0) { return new Vector4(v.y, v.x, n_0, v.y); }
        public static Vector4 yxnz(this Vector4 v, float n_0 = 0) { return new Vector4(v.y, v.x, n_0, v.z); }
        public static Vector4 yxnw(this Vector4 v, float n_0 = 0) { return new Vector4(v.y, v.x, n_0, v.w); }
        public static Vector4 yxnn(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.y, v.x, n_0, n_1); }
        public static Vector4 yyxx(this Vector4 v) { return new Vector4(v.y, v.y, v.x, v.x); }
        public static Vector4 yyxy(this Vector4 v) { return new Vector4(v.y, v.y, v.x, v.y); }
        public static Vector4 yyxz(this Vector4 v) { return new Vector4(v.y, v.y, v.x, v.z); }
        public static Vector4 yyxw(this Vector4 v) { return new Vector4(v.y, v.y, v.x, v.w); }
        public static Vector4 yyxn(this Vector4 v, float n_0 = 0) { return new Vector4(v.y, v.y, v.x, n_0); }
        public static Vector4 yyyx(this Vector4 v) { return new Vector4(v.y, v.y, v.y, v.x); }
        public static Vector4 yyyy(this Vector4 v) { return new Vector4(v.y, v.y, v.y, v.y); }
        public static Vector4 yyyz(this Vector4 v) { return new Vector4(v.y, v.y, v.y, v.z); }
        public static Vector4 yyyw(this Vector4 v) { return new Vector4(v.y, v.y, v.y, v.w); }
        public static Vector4 yyyn(this Vector4 v, float n_0 = 0) { return new Vector4(v.y, v.y, v.y, n_0); }
        public static Vector4 yyzx(this Vector4 v) { return new Vector4(v.y, v.y, v.z, v.x); }
        public static Vector4 yyzy(this Vector4 v) { return new Vector4(v.y, v.y, v.z, v.y); }
        public static Vector4 yyzz(this Vector4 v) { return new Vector4(v.y, v.y, v.z, v.z); }
        public static Vector4 yyzw(this Vector4 v) { return new Vector4(v.y, v.y, v.z, v.w); }
        public static Vector4 yyzn(this Vector4 v, float n_0 = 0) { return new Vector4(v.y, v.y, v.z, n_0); }
        public static Vector4 yywx(this Vector4 v) { return new Vector4(v.y, v.y, v.w, v.x); }
        public static Vector4 yywy(this Vector4 v) { return new Vector4(v.y, v.y, v.w, v.y); }
        public static Vector4 yywz(this Vector4 v) { return new Vector4(v.y, v.y, v.w, v.z); }
        public static Vector4 yyww(this Vector4 v) { return new Vector4(v.y, v.y, v.w, v.w); }
        public static Vector4 yywn(this Vector4 v, float n_0 = 0) { return new Vector4(v.y, v.y, v.w, n_0); }
        public static Vector4 yynx(this Vector4 v, float n_0 = 0) { return new Vector4(v.y, v.y, n_0, v.x); }
        public static Vector4 yyny(this Vector4 v, float n_0 = 0) { return new Vector4(v.y, v.y, n_0, v.y); }
        public static Vector4 yynz(this Vector4 v, float n_0 = 0) { return new Vector4(v.y, v.y, n_0, v.z); }
        public static Vector4 yynw(this Vector4 v, float n_0 = 0) { return new Vector4(v.y, v.y, n_0, v.w); }
        public static Vector4 yynn(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.y, v.y, n_0, n_1); }
        public static Vector4 yzxx(this Vector4 v) { return new Vector4(v.y, v.z, v.x, v.x); }
        public static Vector4 yzxy(this Vector4 v) { return new Vector4(v.y, v.z, v.x, v.y); }
        public static Vector4 yzxz(this Vector4 v) { return new Vector4(v.y, v.z, v.x, v.z); }
        public static Vector4 yzxw(this Vector4 v) { return new Vector4(v.y, v.z, v.x, v.w); }
        public static Vector4 yzxn(this Vector4 v, float n_0 = 0) { return new Vector4(v.y, v.z, v.x, n_0); }
        public static Vector4 yzyx(this Vector4 v) { return new Vector4(v.y, v.z, v.y, v.x); }
        public static Vector4 yzyy(this Vector4 v) { return new Vector4(v.y, v.z, v.y, v.y); }
        public static Vector4 yzyz(this Vector4 v) { return new Vector4(v.y, v.z, v.y, v.z); }
        public static Vector4 yzyw(this Vector4 v) { return new Vector4(v.y, v.z, v.y, v.w); }
        public static Vector4 yzyn(this Vector4 v, float n_0 = 0) { return new Vector4(v.y, v.z, v.y, n_0); }
        public static Vector4 yzzx(this Vector4 v) { return new Vector4(v.y, v.z, v.z, v.x); }
        public static Vector4 yzzy(this Vector4 v) { return new Vector4(v.y, v.z, v.z, v.y); }
        public static Vector4 yzzz(this Vector4 v) { return new Vector4(v.y, v.z, v.z, v.z); }
        public static Vector4 yzzw(this Vector4 v) { return new Vector4(v.y, v.z, v.z, v.w); }
        public static Vector4 yzzn(this Vector4 v, float n_0 = 0) { return new Vector4(v.y, v.z, v.z, n_0); }
        public static Vector4 yzwx(this Vector4 v) { return new Vector4(v.y, v.z, v.w, v.x); }
        public static Vector4 yzwy(this Vector4 v) { return new Vector4(v.y, v.z, v.w, v.y); }
        public static Vector4 yzwz(this Vector4 v) { return new Vector4(v.y, v.z, v.w, v.z); }
        public static Vector4 yzww(this Vector4 v) { return new Vector4(v.y, v.z, v.w, v.w); }
        public static Vector4 yzwn(this Vector4 v, float n_0 = 0) { return new Vector4(v.y, v.z, v.w, n_0); }
        public static Vector4 yznx(this Vector4 v, float n_0 = 0) { return new Vector4(v.y, v.z, n_0, v.x); }
        public static Vector4 yzny(this Vector4 v, float n_0 = 0) { return new Vector4(v.y, v.z, n_0, v.y); }
        public static Vector4 yznz(this Vector4 v, float n_0 = 0) { return new Vector4(v.y, v.z, n_0, v.z); }
        public static Vector4 yznw(this Vector4 v, float n_0 = 0) { return new Vector4(v.y, v.z, n_0, v.w); }
        public static Vector4 yznn(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.y, v.z, n_0, n_1); }
        public static Vector4 ywxx(this Vector4 v) { return new Vector4(v.y, v.w, v.x, v.x); }
        public static Vector4 ywxy(this Vector4 v) { return new Vector4(v.y, v.w, v.x, v.y); }
        public static Vector4 ywxz(this Vector4 v) { return new Vector4(v.y, v.w, v.x, v.z); }
        public static Vector4 ywxw(this Vector4 v) { return new Vector4(v.y, v.w, v.x, v.w); }
        public static Vector4 ywxn(this Vector4 v, float n_0 = 0) { return new Vector4(v.y, v.w, v.x, n_0); }
        public static Vector4 ywyx(this Vector4 v) { return new Vector4(v.y, v.w, v.y, v.x); }
        public static Vector4 ywyy(this Vector4 v) { return new Vector4(v.y, v.w, v.y, v.y); }
        public static Vector4 ywyz(this Vector4 v) { return new Vector4(v.y, v.w, v.y, v.z); }
        public static Vector4 ywyw(this Vector4 v) { return new Vector4(v.y, v.w, v.y, v.w); }
        public static Vector4 ywyn(this Vector4 v, float n_0 = 0) { return new Vector4(v.y, v.w, v.y, n_0); }
        public static Vector4 ywzx(this Vector4 v) { return new Vector4(v.y, v.w, v.z, v.x); }
        public static Vector4 ywzy(this Vector4 v) { return new Vector4(v.y, v.w, v.z, v.y); }
        public static Vector4 ywzz(this Vector4 v) { return new Vector4(v.y, v.w, v.z, v.z); }
        public static Vector4 ywzw(this Vector4 v) { return new Vector4(v.y, v.w, v.z, v.w); }
        public static Vector4 ywzn(this Vector4 v, float n_0 = 0) { return new Vector4(v.y, v.w, v.z, n_0); }
        public static Vector4 ywwx(this Vector4 v) { return new Vector4(v.y, v.w, v.w, v.x); }
        public static Vector4 ywwy(this Vector4 v) { return new Vector4(v.y, v.w, v.w, v.y); }
        public static Vector4 ywwz(this Vector4 v) { return new Vector4(v.y, v.w, v.w, v.z); }
        public static Vector4 ywww(this Vector4 v) { return new Vector4(v.y, v.w, v.w, v.w); }
        public static Vector4 ywwn(this Vector4 v, float n_0 = 0) { return new Vector4(v.y, v.w, v.w, n_0); }
        public static Vector4 ywnx(this Vector4 v, float n_0 = 0) { return new Vector4(v.y, v.w, n_0, v.x); }
        public static Vector4 ywny(this Vector4 v, float n_0 = 0) { return new Vector4(v.y, v.w, n_0, v.y); }
        public static Vector4 ywnz(this Vector4 v, float n_0 = 0) { return new Vector4(v.y, v.w, n_0, v.z); }
        public static Vector4 ywnw(this Vector4 v, float n_0 = 0) { return new Vector4(v.y, v.w, n_0, v.w); }
        public static Vector4 ywnn(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.y, v.w, n_0, n_1); }
        public static Vector4 ynxx(this Vector4 v, float n_0 = 0) { return new Vector4(v.y, n_0, v.x, v.x); }
        public static Vector4 ynxy(this Vector4 v, float n_0 = 0) { return new Vector4(v.y, n_0, v.x, v.y); }
        public static Vector4 ynxz(this Vector4 v, float n_0 = 0) { return new Vector4(v.y, n_0, v.x, v.z); }
        public static Vector4 ynxw(this Vector4 v, float n_0 = 0) { return new Vector4(v.y, n_0, v.x, v.w); }
        public static Vector4 ynxn(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.y, n_0, v.x, n_1); }
        public static Vector4 ynyx(this Vector4 v, float n_0 = 0) { return new Vector4(v.y, n_0, v.y, v.x); }
        public static Vector4 ynyy(this Vector4 v, float n_0 = 0) { return new Vector4(v.y, n_0, v.y, v.y); }
        public static Vector4 ynyz(this Vector4 v, float n_0 = 0) { return new Vector4(v.y, n_0, v.y, v.z); }
        public static Vector4 ynyw(this Vector4 v, float n_0 = 0) { return new Vector4(v.y, n_0, v.y, v.w); }
        public static Vector4 ynyn(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.y, n_0, v.y, n_1); }
        public static Vector4 ynzx(this Vector4 v, float n_0 = 0) { return new Vector4(v.y, n_0, v.z, v.x); }
        public static Vector4 ynzy(this Vector4 v, float n_0 = 0) { return new Vector4(v.y, n_0, v.z, v.y); }
        public static Vector4 ynzz(this Vector4 v, float n_0 = 0) { return new Vector4(v.y, n_0, v.z, v.z); }
        public static Vector4 ynzw(this Vector4 v, float n_0 = 0) { return new Vector4(v.y, n_0, v.z, v.w); }
        public static Vector4 ynzn(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.y, n_0, v.z, n_1); }
        public static Vector4 ynwx(this Vector4 v, float n_0 = 0) { return new Vector4(v.y, n_0, v.w, v.x); }
        public static Vector4 ynwy(this Vector4 v, float n_0 = 0) { return new Vector4(v.y, n_0, v.w, v.y); }
        public static Vector4 ynwz(this Vector4 v, float n_0 = 0) { return new Vector4(v.y, n_0, v.w, v.z); }
        public static Vector4 ynww(this Vector4 v, float n_0 = 0) { return new Vector4(v.y, n_0, v.w, v.w); }
        public static Vector4 ynwn(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.y, n_0, v.w, n_1); }
        public static Vector4 ynnx(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.y, n_0, n_1, v.x); }
        public static Vector4 ynny(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.y, n_0, n_1, v.y); }
        public static Vector4 ynnz(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.y, n_0, n_1, v.z); }
        public static Vector4 ynnw(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.y, n_0, n_1, v.w); }
        public static Vector4 ynnn(this Vector4 v, float n_0 = 0, float n_1 = 0, float n_2 = 0) { return new Vector4(v.y, n_0, n_1, n_2); }
        public static Vector4 zxxx(this Vector4 v) { return new Vector4(v.z, v.x, v.x, v.x); }
        public static Vector4 zxxy(this Vector4 v) { return new Vector4(v.z, v.x, v.x, v.y); }
        public static Vector4 zxxz(this Vector4 v) { return new Vector4(v.z, v.x, v.x, v.z); }
        public static Vector4 zxxw(this Vector4 v) { return new Vector4(v.z, v.x, v.x, v.w); }
        public static Vector4 zxxn(this Vector4 v, float n_0 = 0) { return new Vector4(v.z, v.x, v.x, n_0); }
        public static Vector4 zxyx(this Vector4 v) { return new Vector4(v.z, v.x, v.y, v.x); }
        public static Vector4 zxyy(this Vector4 v) { return new Vector4(v.z, v.x, v.y, v.y); }
        public static Vector4 zxyz(this Vector4 v) { return new Vector4(v.z, v.x, v.y, v.z); }
        public static Vector4 zxyw(this Vector4 v) { return new Vector4(v.z, v.x, v.y, v.w); }
        public static Vector4 zxyn(this Vector4 v, float n_0 = 0) { return new Vector4(v.z, v.x, v.y, n_0); }
        public static Vector4 zxzx(this Vector4 v) { return new Vector4(v.z, v.x, v.z, v.x); }
        public static Vector4 zxzy(this Vector4 v) { return new Vector4(v.z, v.x, v.z, v.y); }
        public static Vector4 zxzz(this Vector4 v) { return new Vector4(v.z, v.x, v.z, v.z); }
        public static Vector4 zxzw(this Vector4 v) { return new Vector4(v.z, v.x, v.z, v.w); }
        public static Vector4 zxzn(this Vector4 v, float n_0 = 0) { return new Vector4(v.z, v.x, v.z, n_0); }
        public static Vector4 zxwx(this Vector4 v) { return new Vector4(v.z, v.x, v.w, v.x); }
        public static Vector4 zxwy(this Vector4 v) { return new Vector4(v.z, v.x, v.w, v.y); }
        public static Vector4 zxwz(this Vector4 v) { return new Vector4(v.z, v.x, v.w, v.z); }
        public static Vector4 zxww(this Vector4 v) { return new Vector4(v.z, v.x, v.w, v.w); }
        public static Vector4 zxwn(this Vector4 v, float n_0 = 0) { return new Vector4(v.z, v.x, v.w, n_0); }
        public static Vector4 zxnx(this Vector4 v, float n_0 = 0) { return new Vector4(v.z, v.x, n_0, v.x); }
        public static Vector4 zxny(this Vector4 v, float n_0 = 0) { return new Vector4(v.z, v.x, n_0, v.y); }
        public static Vector4 zxnz(this Vector4 v, float n_0 = 0) { return new Vector4(v.z, v.x, n_0, v.z); }
        public static Vector4 zxnw(this Vector4 v, float n_0 = 0) { return new Vector4(v.z, v.x, n_0, v.w); }
        public static Vector4 zxnn(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.z, v.x, n_0, n_1); }
        public static Vector4 zyxx(this Vector4 v) { return new Vector4(v.z, v.y, v.x, v.x); }
        public static Vector4 zyxy(this Vector4 v) { return new Vector4(v.z, v.y, v.x, v.y); }
        public static Vector4 zyxz(this Vector4 v) { return new Vector4(v.z, v.y, v.x, v.z); }
        public static Vector4 zyxw(this Vector4 v) { return new Vector4(v.z, v.y, v.x, v.w); }
        public static Vector4 zyxn(this Vector4 v, float n_0 = 0) { return new Vector4(v.z, v.y, v.x, n_0); }
        public static Vector4 zyyx(this Vector4 v) { return new Vector4(v.z, v.y, v.y, v.x); }
        public static Vector4 zyyy(this Vector4 v) { return new Vector4(v.z, v.y, v.y, v.y); }
        public static Vector4 zyyz(this Vector4 v) { return new Vector4(v.z, v.y, v.y, v.z); }
        public static Vector4 zyyw(this Vector4 v) { return new Vector4(v.z, v.y, v.y, v.w); }
        public static Vector4 zyyn(this Vector4 v, float n_0 = 0) { return new Vector4(v.z, v.y, v.y, n_0); }
        public static Vector4 zyzx(this Vector4 v) { return new Vector4(v.z, v.y, v.z, v.x); }
        public static Vector4 zyzy(this Vector4 v) { return new Vector4(v.z, v.y, v.z, v.y); }
        public static Vector4 zyzz(this Vector4 v) { return new Vector4(v.z, v.y, v.z, v.z); }
        public static Vector4 zyzw(this Vector4 v) { return new Vector4(v.z, v.y, v.z, v.w); }
        public static Vector4 zyzn(this Vector4 v, float n_0 = 0) { return new Vector4(v.z, v.y, v.z, n_0); }
        public static Vector4 zywx(this Vector4 v) { return new Vector4(v.z, v.y, v.w, v.x); }
        public static Vector4 zywy(this Vector4 v) { return new Vector4(v.z, v.y, v.w, v.y); }
        public static Vector4 zywz(this Vector4 v) { return new Vector4(v.z, v.y, v.w, v.z); }
        public static Vector4 zyww(this Vector4 v) { return new Vector4(v.z, v.y, v.w, v.w); }
        public static Vector4 zywn(this Vector4 v, float n_0 = 0) { return new Vector4(v.z, v.y, v.w, n_0); }
        public static Vector4 zynx(this Vector4 v, float n_0 = 0) { return new Vector4(v.z, v.y, n_0, v.x); }
        public static Vector4 zyny(this Vector4 v, float n_0 = 0) { return new Vector4(v.z, v.y, n_0, v.y); }
        public static Vector4 zynz(this Vector4 v, float n_0 = 0) { return new Vector4(v.z, v.y, n_0, v.z); }
        public static Vector4 zynw(this Vector4 v, float n_0 = 0) { return new Vector4(v.z, v.y, n_0, v.w); }
        public static Vector4 zynn(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.z, v.y, n_0, n_1); }
        public static Vector4 zzxx(this Vector4 v) { return new Vector4(v.z, v.z, v.x, v.x); }
        public static Vector4 zzxy(this Vector4 v) { return new Vector4(v.z, v.z, v.x, v.y); }
        public static Vector4 zzxz(this Vector4 v) { return new Vector4(v.z, v.z, v.x, v.z); }
        public static Vector4 zzxw(this Vector4 v) { return new Vector4(v.z, v.z, v.x, v.w); }
        public static Vector4 zzxn(this Vector4 v, float n_0 = 0) { return new Vector4(v.z, v.z, v.x, n_0); }
        public static Vector4 zzyx(this Vector4 v) { return new Vector4(v.z, v.z, v.y, v.x); }
        public static Vector4 zzyy(this Vector4 v) { return new Vector4(v.z, v.z, v.y, v.y); }
        public static Vector4 zzyz(this Vector4 v) { return new Vector4(v.z, v.z, v.y, v.z); }
        public static Vector4 zzyw(this Vector4 v) { return new Vector4(v.z, v.z, v.y, v.w); }
        public static Vector4 zzyn(this Vector4 v, float n_0 = 0) { return new Vector4(v.z, v.z, v.y, n_0); }
        public static Vector4 zzzx(this Vector4 v) { return new Vector4(v.z, v.z, v.z, v.x); }
        public static Vector4 zzzy(this Vector4 v) { return new Vector4(v.z, v.z, v.z, v.y); }
        public static Vector4 zzzz(this Vector4 v) { return new Vector4(v.z, v.z, v.z, v.z); }
        public static Vector4 zzzw(this Vector4 v) { return new Vector4(v.z, v.z, v.z, v.w); }
        public static Vector4 zzzn(this Vector4 v, float n_0 = 0) { return new Vector4(v.z, v.z, v.z, n_0); }
        public static Vector4 zzwx(this Vector4 v) { return new Vector4(v.z, v.z, v.w, v.x); }
        public static Vector4 zzwy(this Vector4 v) { return new Vector4(v.z, v.z, v.w, v.y); }
        public static Vector4 zzwz(this Vector4 v) { return new Vector4(v.z, v.z, v.w, v.z); }
        public static Vector4 zzww(this Vector4 v) { return new Vector4(v.z, v.z, v.w, v.w); }
        public static Vector4 zzwn(this Vector4 v, float n_0 = 0) { return new Vector4(v.z, v.z, v.w, n_0); }
        public static Vector4 zznx(this Vector4 v, float n_0 = 0) { return new Vector4(v.z, v.z, n_0, v.x); }
        public static Vector4 zzny(this Vector4 v, float n_0 = 0) { return new Vector4(v.z, v.z, n_0, v.y); }
        public static Vector4 zznz(this Vector4 v, float n_0 = 0) { return new Vector4(v.z, v.z, n_0, v.z); }
        public static Vector4 zznw(this Vector4 v, float n_0 = 0) { return new Vector4(v.z, v.z, n_0, v.w); }
        public static Vector4 zznn(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.z, v.z, n_0, n_1); }
        public static Vector4 zwxx(this Vector4 v) { return new Vector4(v.z, v.w, v.x, v.x); }
        public static Vector4 zwxy(this Vector4 v) { return new Vector4(v.z, v.w, v.x, v.y); }
        public static Vector4 zwxz(this Vector4 v) { return new Vector4(v.z, v.w, v.x, v.z); }
        public static Vector4 zwxw(this Vector4 v) { return new Vector4(v.z, v.w, v.x, v.w); }
        public static Vector4 zwxn(this Vector4 v, float n_0 = 0) { return new Vector4(v.z, v.w, v.x, n_0); }
        public static Vector4 zwyx(this Vector4 v) { return new Vector4(v.z, v.w, v.y, v.x); }
        public static Vector4 zwyy(this Vector4 v) { return new Vector4(v.z, v.w, v.y, v.y); }
        public static Vector4 zwyz(this Vector4 v) { return new Vector4(v.z, v.w, v.y, v.z); }
        public static Vector4 zwyw(this Vector4 v) { return new Vector4(v.z, v.w, v.y, v.w); }
        public static Vector4 zwyn(this Vector4 v, float n_0 = 0) { return new Vector4(v.z, v.w, v.y, n_0); }
        public static Vector4 zwzx(this Vector4 v) { return new Vector4(v.z, v.w, v.z, v.x); }
        public static Vector4 zwzy(this Vector4 v) { return new Vector4(v.z, v.w, v.z, v.y); }
        public static Vector4 zwzz(this Vector4 v) { return new Vector4(v.z, v.w, v.z, v.z); }
        public static Vector4 zwzw(this Vector4 v) { return new Vector4(v.z, v.w, v.z, v.w); }
        public static Vector4 zwzn(this Vector4 v, float n_0 = 0) { return new Vector4(v.z, v.w, v.z, n_0); }
        public static Vector4 zwwx(this Vector4 v) { return new Vector4(v.z, v.w, v.w, v.x); }
        public static Vector4 zwwy(this Vector4 v) { return new Vector4(v.z, v.w, v.w, v.y); }
        public static Vector4 zwwz(this Vector4 v) { return new Vector4(v.z, v.w, v.w, v.z); }
        public static Vector4 zwww(this Vector4 v) { return new Vector4(v.z, v.w, v.w, v.w); }
        public static Vector4 zwwn(this Vector4 v, float n_0 = 0) { return new Vector4(v.z, v.w, v.w, n_0); }
        public static Vector4 zwnx(this Vector4 v, float n_0 = 0) { return new Vector4(v.z, v.w, n_0, v.x); }
        public static Vector4 zwny(this Vector4 v, float n_0 = 0) { return new Vector4(v.z, v.w, n_0, v.y); }
        public static Vector4 zwnz(this Vector4 v, float n_0 = 0) { return new Vector4(v.z, v.w, n_0, v.z); }
        public static Vector4 zwnw(this Vector4 v, float n_0 = 0) { return new Vector4(v.z, v.w, n_0, v.w); }
        public static Vector4 zwnn(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.z, v.w, n_0, n_1); }
        public static Vector4 znxx(this Vector4 v, float n_0 = 0) { return new Vector4(v.z, n_0, v.x, v.x); }
        public static Vector4 znxy(this Vector4 v, float n_0 = 0) { return new Vector4(v.z, n_0, v.x, v.y); }
        public static Vector4 znxz(this Vector4 v, float n_0 = 0) { return new Vector4(v.z, n_0, v.x, v.z); }
        public static Vector4 znxw(this Vector4 v, float n_0 = 0) { return new Vector4(v.z, n_0, v.x, v.w); }
        public static Vector4 znxn(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.z, n_0, v.x, n_1); }
        public static Vector4 znyx(this Vector4 v, float n_0 = 0) { return new Vector4(v.z, n_0, v.y, v.x); }
        public static Vector4 znyy(this Vector4 v, float n_0 = 0) { return new Vector4(v.z, n_0, v.y, v.y); }
        public static Vector4 znyz(this Vector4 v, float n_0 = 0) { return new Vector4(v.z, n_0, v.y, v.z); }
        public static Vector4 znyw(this Vector4 v, float n_0 = 0) { return new Vector4(v.z, n_0, v.y, v.w); }
        public static Vector4 znyn(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.z, n_0, v.y, n_1); }
        public static Vector4 znzx(this Vector4 v, float n_0 = 0) { return new Vector4(v.z, n_0, v.z, v.x); }
        public static Vector4 znzy(this Vector4 v, float n_0 = 0) { return new Vector4(v.z, n_0, v.z, v.y); }
        public static Vector4 znzz(this Vector4 v, float n_0 = 0) { return new Vector4(v.z, n_0, v.z, v.z); }
        public static Vector4 znzw(this Vector4 v, float n_0 = 0) { return new Vector4(v.z, n_0, v.z, v.w); }
        public static Vector4 znzn(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.z, n_0, v.z, n_1); }
        public static Vector4 znwx(this Vector4 v, float n_0 = 0) { return new Vector4(v.z, n_0, v.w, v.x); }
        public static Vector4 znwy(this Vector4 v, float n_0 = 0) { return new Vector4(v.z, n_0, v.w, v.y); }
        public static Vector4 znwz(this Vector4 v, float n_0 = 0) { return new Vector4(v.z, n_0, v.w, v.z); }
        public static Vector4 znww(this Vector4 v, float n_0 = 0) { return new Vector4(v.z, n_0, v.w, v.w); }
        public static Vector4 znwn(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.z, n_0, v.w, n_1); }
        public static Vector4 znnx(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.z, n_0, n_1, v.x); }
        public static Vector4 znny(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.z, n_0, n_1, v.y); }
        public static Vector4 znnz(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.z, n_0, n_1, v.z); }
        public static Vector4 znnw(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.z, n_0, n_1, v.w); }
        public static Vector4 znnn(this Vector4 v, float n_0 = 0, float n_1 = 0, float n_2 = 0) { return new Vector4(v.z, n_0, n_1, n_2); }
        public static Vector4 wxxx(this Vector4 v) { return new Vector4(v.w, v.x, v.x, v.x); }
        public static Vector4 wxxy(this Vector4 v) { return new Vector4(v.w, v.x, v.x, v.y); }
        public static Vector4 wxxz(this Vector4 v) { return new Vector4(v.w, v.x, v.x, v.z); }
        public static Vector4 wxxw(this Vector4 v) { return new Vector4(v.w, v.x, v.x, v.w); }
        public static Vector4 wxxn(this Vector4 v, float n_0 = 0) { return new Vector4(v.w, v.x, v.x, n_0); }
        public static Vector4 wxyx(this Vector4 v) { return new Vector4(v.w, v.x, v.y, v.x); }
        public static Vector4 wxyy(this Vector4 v) { return new Vector4(v.w, v.x, v.y, v.y); }
        public static Vector4 wxyz(this Vector4 v) { return new Vector4(v.w, v.x, v.y, v.z); }
        public static Vector4 wxyw(this Vector4 v) { return new Vector4(v.w, v.x, v.y, v.w); }
        public static Vector4 wxyn(this Vector4 v, float n_0 = 0) { return new Vector4(v.w, v.x, v.y, n_0); }
        public static Vector4 wxzx(this Vector4 v) { return new Vector4(v.w, v.x, v.z, v.x); }
        public static Vector4 wxzy(this Vector4 v) { return new Vector4(v.w, v.x, v.z, v.y); }
        public static Vector4 wxzz(this Vector4 v) { return new Vector4(v.w, v.x, v.z, v.z); }
        public static Vector4 wxzw(this Vector4 v) { return new Vector4(v.w, v.x, v.z, v.w); }
        public static Vector4 wxzn(this Vector4 v, float n_0 = 0) { return new Vector4(v.w, v.x, v.z, n_0); }
        public static Vector4 wxwx(this Vector4 v) { return new Vector4(v.w, v.x, v.w, v.x); }
        public static Vector4 wxwy(this Vector4 v) { return new Vector4(v.w, v.x, v.w, v.y); }
        public static Vector4 wxwz(this Vector4 v) { return new Vector4(v.w, v.x, v.w, v.z); }
        public static Vector4 wxww(this Vector4 v) { return new Vector4(v.w, v.x, v.w, v.w); }
        public static Vector4 wxwn(this Vector4 v, float n_0 = 0) { return new Vector4(v.w, v.x, v.w, n_0); }
        public static Vector4 wxnx(this Vector4 v, float n_0 = 0) { return new Vector4(v.w, v.x, n_0, v.x); }
        public static Vector4 wxny(this Vector4 v, float n_0 = 0) { return new Vector4(v.w, v.x, n_0, v.y); }
        public static Vector4 wxnz(this Vector4 v, float n_0 = 0) { return new Vector4(v.w, v.x, n_0, v.z); }
        public static Vector4 wxnw(this Vector4 v, float n_0 = 0) { return new Vector4(v.w, v.x, n_0, v.w); }
        public static Vector4 wxnn(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.w, v.x, n_0, n_1); }
        public static Vector4 wyxx(this Vector4 v) { return new Vector4(v.w, v.y, v.x, v.x); }
        public static Vector4 wyxy(this Vector4 v) { return new Vector4(v.w, v.y, v.x, v.y); }
        public static Vector4 wyxz(this Vector4 v) { return new Vector4(v.w, v.y, v.x, v.z); }
        public static Vector4 wyxw(this Vector4 v) { return new Vector4(v.w, v.y, v.x, v.w); }
        public static Vector4 wyxn(this Vector4 v, float n_0 = 0) { return new Vector4(v.w, v.y, v.x, n_0); }
        public static Vector4 wyyx(this Vector4 v) { return new Vector4(v.w, v.y, v.y, v.x); }
        public static Vector4 wyyy(this Vector4 v) { return new Vector4(v.w, v.y, v.y, v.y); }
        public static Vector4 wyyz(this Vector4 v) { return new Vector4(v.w, v.y, v.y, v.z); }
        public static Vector4 wyyw(this Vector4 v) { return new Vector4(v.w, v.y, v.y, v.w); }
        public static Vector4 wyyn(this Vector4 v, float n_0 = 0) { return new Vector4(v.w, v.y, v.y, n_0); }
        public static Vector4 wyzx(this Vector4 v) { return new Vector4(v.w, v.y, v.z, v.x); }
        public static Vector4 wyzy(this Vector4 v) { return new Vector4(v.w, v.y, v.z, v.y); }
        public static Vector4 wyzz(this Vector4 v) { return new Vector4(v.w, v.y, v.z, v.z); }
        public static Vector4 wyzw(this Vector4 v) { return new Vector4(v.w, v.y, v.z, v.w); }
        public static Vector4 wyzn(this Vector4 v, float n_0 = 0) { return new Vector4(v.w, v.y, v.z, n_0); }
        public static Vector4 wywx(this Vector4 v) { return new Vector4(v.w, v.y, v.w, v.x); }
        public static Vector4 wywy(this Vector4 v) { return new Vector4(v.w, v.y, v.w, v.y); }
        public static Vector4 wywz(this Vector4 v) { return new Vector4(v.w, v.y, v.w, v.z); }
        public static Vector4 wyww(this Vector4 v) { return new Vector4(v.w, v.y, v.w, v.w); }
        public static Vector4 wywn(this Vector4 v, float n_0 = 0) { return new Vector4(v.w, v.y, v.w, n_0); }
        public static Vector4 wynx(this Vector4 v, float n_0 = 0) { return new Vector4(v.w, v.y, n_0, v.x); }
        public static Vector4 wyny(this Vector4 v, float n_0 = 0) { return new Vector4(v.w, v.y, n_0, v.y); }
        public static Vector4 wynz(this Vector4 v, float n_0 = 0) { return new Vector4(v.w, v.y, n_0, v.z); }
        public static Vector4 wynw(this Vector4 v, float n_0 = 0) { return new Vector4(v.w, v.y, n_0, v.w); }
        public static Vector4 wynn(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.w, v.y, n_0, n_1); }
        public static Vector4 wzxx(this Vector4 v) { return new Vector4(v.w, v.z, v.x, v.x); }
        public static Vector4 wzxy(this Vector4 v) { return new Vector4(v.w, v.z, v.x, v.y); }
        public static Vector4 wzxz(this Vector4 v) { return new Vector4(v.w, v.z, v.x, v.z); }
        public static Vector4 wzxw(this Vector4 v) { return new Vector4(v.w, v.z, v.x, v.w); }
        public static Vector4 wzxn(this Vector4 v, float n_0 = 0) { return new Vector4(v.w, v.z, v.x, n_0); }
        public static Vector4 wzyx(this Vector4 v) { return new Vector4(v.w, v.z, v.y, v.x); }
        public static Vector4 wzyy(this Vector4 v) { return new Vector4(v.w, v.z, v.y, v.y); }
        public static Vector4 wzyz(this Vector4 v) { return new Vector4(v.w, v.z, v.y, v.z); }
        public static Vector4 wzyw(this Vector4 v) { return new Vector4(v.w, v.z, v.y, v.w); }
        public static Vector4 wzyn(this Vector4 v, float n_0 = 0) { return new Vector4(v.w, v.z, v.y, n_0); }
        public static Vector4 wzzx(this Vector4 v) { return new Vector4(v.w, v.z, v.z, v.x); }
        public static Vector4 wzzy(this Vector4 v) { return new Vector4(v.w, v.z, v.z, v.y); }
        public static Vector4 wzzz(this Vector4 v) { return new Vector4(v.w, v.z, v.z, v.z); }
        public static Vector4 wzzw(this Vector4 v) { return new Vector4(v.w, v.z, v.z, v.w); }
        public static Vector4 wzzn(this Vector4 v, float n_0 = 0) { return new Vector4(v.w, v.z, v.z, n_0); }
        public static Vector4 wzwx(this Vector4 v) { return new Vector4(v.w, v.z, v.w, v.x); }
        public static Vector4 wzwy(this Vector4 v) { return new Vector4(v.w, v.z, v.w, v.y); }
        public static Vector4 wzwz(this Vector4 v) { return new Vector4(v.w, v.z, v.w, v.z); }
        public static Vector4 wzww(this Vector4 v) { return new Vector4(v.w, v.z, v.w, v.w); }
        public static Vector4 wzwn(this Vector4 v, float n_0 = 0) { return new Vector4(v.w, v.z, v.w, n_0); }
        public static Vector4 wznx(this Vector4 v, float n_0 = 0) { return new Vector4(v.w, v.z, n_0, v.x); }
        public static Vector4 wzny(this Vector4 v, float n_0 = 0) { return new Vector4(v.w, v.z, n_0, v.y); }
        public static Vector4 wznz(this Vector4 v, float n_0 = 0) { return new Vector4(v.w, v.z, n_0, v.z); }
        public static Vector4 wznw(this Vector4 v, float n_0 = 0) { return new Vector4(v.w, v.z, n_0, v.w); }
        public static Vector4 wznn(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.w, v.z, n_0, n_1); }
        public static Vector4 wwxx(this Vector4 v) { return new Vector4(v.w, v.w, v.x, v.x); }
        public static Vector4 wwxy(this Vector4 v) { return new Vector4(v.w, v.w, v.x, v.y); }
        public static Vector4 wwxz(this Vector4 v) { return new Vector4(v.w, v.w, v.x, v.z); }
        public static Vector4 wwxw(this Vector4 v) { return new Vector4(v.w, v.w, v.x, v.w); }
        public static Vector4 wwxn(this Vector4 v, float n_0 = 0) { return new Vector4(v.w, v.w, v.x, n_0); }
        public static Vector4 wwyx(this Vector4 v) { return new Vector4(v.w, v.w, v.y, v.x); }
        public static Vector4 wwyy(this Vector4 v) { return new Vector4(v.w, v.w, v.y, v.y); }
        public static Vector4 wwyz(this Vector4 v) { return new Vector4(v.w, v.w, v.y, v.z); }
        public static Vector4 wwyw(this Vector4 v) { return new Vector4(v.w, v.w, v.y, v.w); }
        public static Vector4 wwyn(this Vector4 v, float n_0 = 0) { return new Vector4(v.w, v.w, v.y, n_0); }
        public static Vector4 wwzx(this Vector4 v) { return new Vector4(v.w, v.w, v.z, v.x); }
        public static Vector4 wwzy(this Vector4 v) { return new Vector4(v.w, v.w, v.z, v.y); }
        public static Vector4 wwzz(this Vector4 v) { return new Vector4(v.w, v.w, v.z, v.z); }
        public static Vector4 wwzw(this Vector4 v) { return new Vector4(v.w, v.w, v.z, v.w); }
        public static Vector4 wwzn(this Vector4 v, float n_0 = 0) { return new Vector4(v.w, v.w, v.z, n_0); }
        public static Vector4 wwwx(this Vector4 v) { return new Vector4(v.w, v.w, v.w, v.x); }
        public static Vector4 wwwy(this Vector4 v) { return new Vector4(v.w, v.w, v.w, v.y); }
        public static Vector4 wwwz(this Vector4 v) { return new Vector4(v.w, v.w, v.w, v.z); }
        public static Vector4 wwww(this Vector4 v) { return new Vector4(v.w, v.w, v.w, v.w); }
        public static Vector4 wwwn(this Vector4 v, float n_0 = 0) { return new Vector4(v.w, v.w, v.w, n_0); }
        public static Vector4 wwnx(this Vector4 v, float n_0 = 0) { return new Vector4(v.w, v.w, n_0, v.x); }
        public static Vector4 wwny(this Vector4 v, float n_0 = 0) { return new Vector4(v.w, v.w, n_0, v.y); }
        public static Vector4 wwnz(this Vector4 v, float n_0 = 0) { return new Vector4(v.w, v.w, n_0, v.z); }
        public static Vector4 wwnw(this Vector4 v, float n_0 = 0) { return new Vector4(v.w, v.w, n_0, v.w); }
        public static Vector4 wwnn(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.w, v.w, n_0, n_1); }
        public static Vector4 wnxx(this Vector4 v, float n_0 = 0) { return new Vector4(v.w, n_0, v.x, v.x); }
        public static Vector4 wnxy(this Vector4 v, float n_0 = 0) { return new Vector4(v.w, n_0, v.x, v.y); }
        public static Vector4 wnxz(this Vector4 v, float n_0 = 0) { return new Vector4(v.w, n_0, v.x, v.z); }
        public static Vector4 wnxw(this Vector4 v, float n_0 = 0) { return new Vector4(v.w, n_0, v.x, v.w); }
        public static Vector4 wnxn(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.w, n_0, v.x, n_1); }
        public static Vector4 wnyx(this Vector4 v, float n_0 = 0) { return new Vector4(v.w, n_0, v.y, v.x); }
        public static Vector4 wnyy(this Vector4 v, float n_0 = 0) { return new Vector4(v.w, n_0, v.y, v.y); }
        public static Vector4 wnyz(this Vector4 v, float n_0 = 0) { return new Vector4(v.w, n_0, v.y, v.z); }
        public static Vector4 wnyw(this Vector4 v, float n_0 = 0) { return new Vector4(v.w, n_0, v.y, v.w); }
        public static Vector4 wnyn(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.w, n_0, v.y, n_1); }
        public static Vector4 wnzx(this Vector4 v, float n_0 = 0) { return new Vector4(v.w, n_0, v.z, v.x); }
        public static Vector4 wnzy(this Vector4 v, float n_0 = 0) { return new Vector4(v.w, n_0, v.z, v.y); }
        public static Vector4 wnzz(this Vector4 v, float n_0 = 0) { return new Vector4(v.w, n_0, v.z, v.z); }
        public static Vector4 wnzw(this Vector4 v, float n_0 = 0) { return new Vector4(v.w, n_0, v.z, v.w); }
        public static Vector4 wnzn(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.w, n_0, v.z, n_1); }
        public static Vector4 wnwx(this Vector4 v, float n_0 = 0) { return new Vector4(v.w, n_0, v.w, v.x); }
        public static Vector4 wnwy(this Vector4 v, float n_0 = 0) { return new Vector4(v.w, n_0, v.w, v.y); }
        public static Vector4 wnwz(this Vector4 v, float n_0 = 0) { return new Vector4(v.w, n_0, v.w, v.z); }
        public static Vector4 wnww(this Vector4 v, float n_0 = 0) { return new Vector4(v.w, n_0, v.w, v.w); }
        public static Vector4 wnwn(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.w, n_0, v.w, n_1); }
        public static Vector4 wnnx(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.w, n_0, n_1, v.x); }
        public static Vector4 wnny(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.w, n_0, n_1, v.y); }
        public static Vector4 wnnz(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.w, n_0, n_1, v.z); }
        public static Vector4 wnnw(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(v.w, n_0, n_1, v.w); }
        public static Vector4 wnnn(this Vector4 v, float n_0 = 0, float n_1 = 0, float n_2 = 0) { return new Vector4(v.w, n_0, n_1, n_2); }
        public static Vector4 nxxx(this Vector4 v, float n_0 = 0) { return new Vector4(n_0, v.x, v.x, v.x); }
        public static Vector4 nxxy(this Vector4 v, float n_0 = 0) { return new Vector4(n_0, v.x, v.x, v.y); }
        public static Vector4 nxxz(this Vector4 v, float n_0 = 0) { return new Vector4(n_0, v.x, v.x, v.z); }
        public static Vector4 nxxw(this Vector4 v, float n_0 = 0) { return new Vector4(n_0, v.x, v.x, v.w); }
        public static Vector4 nxxn(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, v.x, v.x, n_1); }
        public static Vector4 nxyx(this Vector4 v, float n_0 = 0) { return new Vector4(n_0, v.x, v.y, v.x); }
        public static Vector4 nxyy(this Vector4 v, float n_0 = 0) { return new Vector4(n_0, v.x, v.y, v.y); }
        public static Vector4 nxyz(this Vector4 v, float n_0 = 0) { return new Vector4(n_0, v.x, v.y, v.z); }
        public static Vector4 nxyw(this Vector4 v, float n_0 = 0) { return new Vector4(n_0, v.x, v.y, v.w); }
        public static Vector4 nxyn(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, v.x, v.y, n_1); }
        public static Vector4 nxzx(this Vector4 v, float n_0 = 0) { return new Vector4(n_0, v.x, v.z, v.x); }
        public static Vector4 nxzy(this Vector4 v, float n_0 = 0) { return new Vector4(n_0, v.x, v.z, v.y); }
        public static Vector4 nxzz(this Vector4 v, float n_0 = 0) { return new Vector4(n_0, v.x, v.z, v.z); }
        public static Vector4 nxzw(this Vector4 v, float n_0 = 0) { return new Vector4(n_0, v.x, v.z, v.w); }
        public static Vector4 nxzn(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, v.x, v.z, n_1); }
        public static Vector4 nxwx(this Vector4 v, float n_0 = 0) { return new Vector4(n_0, v.x, v.w, v.x); }
        public static Vector4 nxwy(this Vector4 v, float n_0 = 0) { return new Vector4(n_0, v.x, v.w, v.y); }
        public static Vector4 nxwz(this Vector4 v, float n_0 = 0) { return new Vector4(n_0, v.x, v.w, v.z); }
        public static Vector4 nxww(this Vector4 v, float n_0 = 0) { return new Vector4(n_0, v.x, v.w, v.w); }
        public static Vector4 nxwn(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, v.x, v.w, n_1); }
        public static Vector4 nxnx(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, v.x, n_1, v.x); }
        public static Vector4 nxny(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, v.x, n_1, v.y); }
        public static Vector4 nxnz(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, v.x, n_1, v.z); }
        public static Vector4 nxnw(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, v.x, n_1, v.w); }
        public static Vector4 nxnn(this Vector4 v, float n_0 = 0, float n_1 = 0, float n_2 = 0) { return new Vector4(n_0, v.x, n_1, n_2); }
        public static Vector4 nyxx(this Vector4 v, float n_0 = 0) { return new Vector4(n_0, v.y, v.x, v.x); }
        public static Vector4 nyxy(this Vector4 v, float n_0 = 0) { return new Vector4(n_0, v.y, v.x, v.y); }
        public static Vector4 nyxz(this Vector4 v, float n_0 = 0) { return new Vector4(n_0, v.y, v.x, v.z); }
        public static Vector4 nyxw(this Vector4 v, float n_0 = 0) { return new Vector4(n_0, v.y, v.x, v.w); }
        public static Vector4 nyxn(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, v.y, v.x, n_1); }
        public static Vector4 nyyx(this Vector4 v, float n_0 = 0) { return new Vector4(n_0, v.y, v.y, v.x); }
        public static Vector4 nyyy(this Vector4 v, float n_0 = 0) { return new Vector4(n_0, v.y, v.y, v.y); }
        public static Vector4 nyyz(this Vector4 v, float n_0 = 0) { return new Vector4(n_0, v.y, v.y, v.z); }
        public static Vector4 nyyw(this Vector4 v, float n_0 = 0) { return new Vector4(n_0, v.y, v.y, v.w); }
        public static Vector4 nyyn(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, v.y, v.y, n_1); }
        public static Vector4 nyzx(this Vector4 v, float n_0 = 0) { return new Vector4(n_0, v.y, v.z, v.x); }
        public static Vector4 nyzy(this Vector4 v, float n_0 = 0) { return new Vector4(n_0, v.y, v.z, v.y); }
        public static Vector4 nyzz(this Vector4 v, float n_0 = 0) { return new Vector4(n_0, v.y, v.z, v.z); }
        public static Vector4 nyzw(this Vector4 v, float n_0 = 0) { return new Vector4(n_0, v.y, v.z, v.w); }
        public static Vector4 nyzn(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, v.y, v.z, n_1); }
        public static Vector4 nywx(this Vector4 v, float n_0 = 0) { return new Vector4(n_0, v.y, v.w, v.x); }
        public static Vector4 nywy(this Vector4 v, float n_0 = 0) { return new Vector4(n_0, v.y, v.w, v.y); }
        public static Vector4 nywz(this Vector4 v, float n_0 = 0) { return new Vector4(n_0, v.y, v.w, v.z); }
        public static Vector4 nyww(this Vector4 v, float n_0 = 0) { return new Vector4(n_0, v.y, v.w, v.w); }
        public static Vector4 nywn(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, v.y, v.w, n_1); }
        public static Vector4 nynx(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, v.y, n_1, v.x); }
        public static Vector4 nyny(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, v.y, n_1, v.y); }
        public static Vector4 nynz(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, v.y, n_1, v.z); }
        public static Vector4 nynw(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, v.y, n_1, v.w); }
        public static Vector4 nynn(this Vector4 v, float n_0 = 0, float n_1 = 0, float n_2 = 0) { return new Vector4(n_0, v.y, n_1, n_2); }
        public static Vector4 nzxx(this Vector4 v, float n_0 = 0) { return new Vector4(n_0, v.z, v.x, v.x); }
        public static Vector4 nzxy(this Vector4 v, float n_0 = 0) { return new Vector4(n_0, v.z, v.x, v.y); }
        public static Vector4 nzxz(this Vector4 v, float n_0 = 0) { return new Vector4(n_0, v.z, v.x, v.z); }
        public static Vector4 nzxw(this Vector4 v, float n_0 = 0) { return new Vector4(n_0, v.z, v.x, v.w); }
        public static Vector4 nzxn(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, v.z, v.x, n_1); }
        public static Vector4 nzyx(this Vector4 v, float n_0 = 0) { return new Vector4(n_0, v.z, v.y, v.x); }
        public static Vector4 nzyy(this Vector4 v, float n_0 = 0) { return new Vector4(n_0, v.z, v.y, v.y); }
        public static Vector4 nzyz(this Vector4 v, float n_0 = 0) { return new Vector4(n_0, v.z, v.y, v.z); }
        public static Vector4 nzyw(this Vector4 v, float n_0 = 0) { return new Vector4(n_0, v.z, v.y, v.w); }
        public static Vector4 nzyn(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, v.z, v.y, n_1); }
        public static Vector4 nzzx(this Vector4 v, float n_0 = 0) { return new Vector4(n_0, v.z, v.z, v.x); }
        public static Vector4 nzzy(this Vector4 v, float n_0 = 0) { return new Vector4(n_0, v.z, v.z, v.y); }
        public static Vector4 nzzz(this Vector4 v, float n_0 = 0) { return new Vector4(n_0, v.z, v.z, v.z); }
        public static Vector4 nzzw(this Vector4 v, float n_0 = 0) { return new Vector4(n_0, v.z, v.z, v.w); }
        public static Vector4 nzzn(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, v.z, v.z, n_1); }
        public static Vector4 nzwx(this Vector4 v, float n_0 = 0) { return new Vector4(n_0, v.z, v.w, v.x); }
        public static Vector4 nzwy(this Vector4 v, float n_0 = 0) { return new Vector4(n_0, v.z, v.w, v.y); }
        public static Vector4 nzwz(this Vector4 v, float n_0 = 0) { return new Vector4(n_0, v.z, v.w, v.z); }
        public static Vector4 nzww(this Vector4 v, float n_0 = 0) { return new Vector4(n_0, v.z, v.w, v.w); }
        public static Vector4 nzwn(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, v.z, v.w, n_1); }
        public static Vector4 nznx(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, v.z, n_1, v.x); }
        public static Vector4 nzny(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, v.z, n_1, v.y); }
        public static Vector4 nznz(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, v.z, n_1, v.z); }
        public static Vector4 nznw(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, v.z, n_1, v.w); }
        public static Vector4 nznn(this Vector4 v, float n_0 = 0, float n_1 = 0, float n_2 = 0) { return new Vector4(n_0, v.z, n_1, n_2); }
        public static Vector4 nwxx(this Vector4 v, float n_0 = 0) { return new Vector4(n_0, v.w, v.x, v.x); }
        public static Vector4 nwxy(this Vector4 v, float n_0 = 0) { return new Vector4(n_0, v.w, v.x, v.y); }
        public static Vector4 nwxz(this Vector4 v, float n_0 = 0) { return new Vector4(n_0, v.w, v.x, v.z); }
        public static Vector4 nwxw(this Vector4 v, float n_0 = 0) { return new Vector4(n_0, v.w, v.x, v.w); }
        public static Vector4 nwxn(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, v.w, v.x, n_1); }
        public static Vector4 nwyx(this Vector4 v, float n_0 = 0) { return new Vector4(n_0, v.w, v.y, v.x); }
        public static Vector4 nwyy(this Vector4 v, float n_0 = 0) { return new Vector4(n_0, v.w, v.y, v.y); }
        public static Vector4 nwyz(this Vector4 v, float n_0 = 0) { return new Vector4(n_0, v.w, v.y, v.z); }
        public static Vector4 nwyw(this Vector4 v, float n_0 = 0) { return new Vector4(n_0, v.w, v.y, v.w); }
        public static Vector4 nwyn(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, v.w, v.y, n_1); }
        public static Vector4 nwzx(this Vector4 v, float n_0 = 0) { return new Vector4(n_0, v.w, v.z, v.x); }
        public static Vector4 nwzy(this Vector4 v, float n_0 = 0) { return new Vector4(n_0, v.w, v.z, v.y); }
        public static Vector4 nwzz(this Vector4 v, float n_0 = 0) { return new Vector4(n_0, v.w, v.z, v.z); }
        public static Vector4 nwzw(this Vector4 v, float n_0 = 0) { return new Vector4(n_0, v.w, v.z, v.w); }
        public static Vector4 nwzn(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, v.w, v.z, n_1); }
        public static Vector4 nwwx(this Vector4 v, float n_0 = 0) { return new Vector4(n_0, v.w, v.w, v.x); }
        public static Vector4 nwwy(this Vector4 v, float n_0 = 0) { return new Vector4(n_0, v.w, v.w, v.y); }
        public static Vector4 nwwz(this Vector4 v, float n_0 = 0) { return new Vector4(n_0, v.w, v.w, v.z); }
        public static Vector4 nwww(this Vector4 v, float n_0 = 0) { return new Vector4(n_0, v.w, v.w, v.w); }
        public static Vector4 nwwn(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, v.w, v.w, n_1); }
        public static Vector4 nwnx(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, v.w, n_1, v.x); }
        public static Vector4 nwny(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, v.w, n_1, v.y); }
        public static Vector4 nwnz(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, v.w, n_1, v.z); }
        public static Vector4 nwnw(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, v.w, n_1, v.w); }
        public static Vector4 nwnn(this Vector4 v, float n_0 = 0, float n_1 = 0, float n_2 = 0) { return new Vector4(n_0, v.w, n_1, n_2); }
        public static Vector4 nnxx(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, n_1, v.x, v.x); }
        public static Vector4 nnxy(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, n_1, v.x, v.y); }
        public static Vector4 nnxz(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, n_1, v.x, v.z); }
        public static Vector4 nnxw(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, n_1, v.x, v.w); }
        public static Vector4 nnxn(this Vector4 v, float n_0 = 0, float n_1 = 0, float n_2 = 0) { return new Vector4(n_0, n_1, v.x, n_2); }
        public static Vector4 nnyx(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, n_1, v.y, v.x); }
        public static Vector4 nnyy(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, n_1, v.y, v.y); }
        public static Vector4 nnyz(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, n_1, v.y, v.z); }
        public static Vector4 nnyw(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, n_1, v.y, v.w); }
        public static Vector4 nnyn(this Vector4 v, float n_0 = 0, float n_1 = 0, float n_2 = 0) { return new Vector4(n_0, n_1, v.y, n_2); }
        public static Vector4 nnzx(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, n_1, v.z, v.x); }
        public static Vector4 nnzy(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, n_1, v.z, v.y); }
        public static Vector4 nnzz(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, n_1, v.z, v.z); }
        public static Vector4 nnzw(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, n_1, v.z, v.w); }
        public static Vector4 nnzn(this Vector4 v, float n_0 = 0, float n_1 = 0, float n_2 = 0) { return new Vector4(n_0, n_1, v.z, n_2); }
        public static Vector4 nnwx(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, n_1, v.w, v.x); }
        public static Vector4 nnwy(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, n_1, v.w, v.y); }
        public static Vector4 nnwz(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, n_1, v.w, v.z); }
        public static Vector4 nnww(this Vector4 v, float n_0 = 0, float n_1 = 0) { return new Vector4(n_0, n_1, v.w, v.w); }
        public static Vector4 nnwn(this Vector4 v, float n_0 = 0, float n_1 = 0, float n_2 = 0) { return new Vector4(n_0, n_1, v.w, n_2); }
        public static Vector4 nnnx(this Vector4 v, float n_0 = 0, float n_1 = 0, float n_2 = 0) { return new Vector4(n_0, n_1, n_2, v.x); }
        public static Vector4 nnny(this Vector4 v, float n_0 = 0, float n_1 = 0, float n_2 = 0) { return new Vector4(n_0, n_1, n_2, v.y); }
        public static Vector4 nnnz(this Vector4 v, float n_0 = 0, float n_1 = 0, float n_2 = 0) { return new Vector4(n_0, n_1, n_2, v.z); }
        public static Vector4 nnnw(this Vector4 v, float n_0 = 0, float n_1 = 0, float n_2 = 0) { return new Vector4(n_0, n_1, n_2, v.w); }
        #endregion Swizzle Vector4 to Vector4
        
        
    }
}

//-----------------------------------------------------------------------------
namespace Prateek.ShaderTo
{
    //-------------------------------------------------------------------------
    public static partial class CSharp
    {
        //---------------------------------------------------------------------
        #region Swizzle Vector2 to DST_CLASS
        public static Vector2 vec2(float n_0, float n_1) { return new Vector2(n_0, n_1); }
        public static Vector2 vec2(Vector2 v_0) { return new Vector2(v_0.x, v_0.y); }
        #endregion Swizzle Vector2 to DST_CLASS
        
        //---------------------------------------------------------------------
        #region Swizzle Vector3 to DST_CLASS
        public static Vector3 vec3(float n_0, float n_1, float n_2) { return new Vector3(n_0, n_1, n_2); }
        public static Vector3 vec3(float n_0, Vector2 v_0) { return new Vector3(n_0, v_0.x, v_0.y); }
        public static Vector3 vec3(Vector2 v_0, float n_0) { return new Vector3(v_0.x, v_0.y, n_0); }
        public static Vector3 vec3(Vector3 v_0) { return new Vector3(v_0.x, v_0.y, v_0.z); }
        #endregion Swizzle Vector3 to DST_CLASS
        
        //---------------------------------------------------------------------
        #region Swizzle Vector4 to DST_CLASS
        public static Vector4 vec4(float n_0, float n_1, float n_2, float n_3) { return new Vector4(n_0, n_1, n_2, n_3); }
        public static Vector4 vec4(float n_0, float n_1, Vector2 v_0) { return new Vector4(n_0, n_1, v_0.x, v_0.y); }
        public static Vector4 vec4(float n_0, Vector2 v_0, float n_1) { return new Vector4(n_0, v_0.x, v_0.y, n_1); }
        public static Vector4 vec4(float n_0, Vector3 v_0) { return new Vector4(n_0, v_0.x, v_0.y, v_0.z); }
        public static Vector4 vec4(Vector2 v_0, float n_0, float n_1) { return new Vector4(v_0.x, v_0.y, n_0, n_1); }
        public static Vector4 vec4(Vector2 v_0, Vector2 v_1) { return new Vector4(v_0.x, v_0.y, v_1.x, v_1.y); }
        public static Vector4 vec4(Vector3 v_0, float n_0) { return new Vector4(v_0.x, v_0.y, v_0.z, n_0); }
        public static Vector4 vec4(Vector4 v_0) { return new Vector4(v_0.x, v_0.y, v_0.z, v_0.w); }
        #endregion Swizzle Vector4 to DST_CLASS
        
        
    }
}
