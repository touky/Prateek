// -BEGIN_PRATEEK_COPYRIGHT-
//
//  Prateek, a library that is "bien pratique"
//  Header last update date: 21/08/2020
//
//  Copyright � 2017-2020 "Touky" <touky at prateek dot top>
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





///----------------------------------------------------------------------------
namespace Prateek.Runtime.Core.Extensions
{
    // -BEGIN_PRATEEK_CSHARP_NAMESPACE_CODE-
    ///------------------------------------------------------------------------
    #region Prateek Code Namespaces
    using System;
    using System.Collections;
    using System.Collections.Generic;
    
    using UnityEngine;
    
    using Prateek;
    using static Core.Statics.Statics;
    #endregion Prateek Code Namespaces
// -END_PRATEEK_CSHARP_NAMESPACE_CODE-
    
    

    ///------------------------------------------------------------------------
    public static partial class Vector3Extensions
    {
        //--
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
        
        //--
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
        
        //--
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
        
        
    }
}
