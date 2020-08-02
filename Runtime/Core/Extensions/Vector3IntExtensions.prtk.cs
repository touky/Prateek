// -BEGIN_PRATEEK_COPYRIGHT-
//
//  Prateek, a library that is "bien pratique"
//  Header last update date: 02/08/2020
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
    using static Prateek.Runtime.Core.Extensions.Statics;
    #endregion Prateek Code Namespaces
// -END_PRATEEK_CSHARP_NAMESPACE_CODE-
    
    

    ///------------------------------------------------------------------------
    public static class Vector3IntExtensions
    {
        
        //--
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
        
        //--
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
