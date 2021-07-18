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
    public static partial class Vector2IntExtensions
    {
        
        //--
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
        
        //--
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
        
    }
}
