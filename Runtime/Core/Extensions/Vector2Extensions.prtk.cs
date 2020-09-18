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
    using static Prateek.Runtime.Core.Extensions.Statics;
    #endregion Prateek Code Namespaces
// -END_PRATEEK_CSHARP_NAMESPACE_CODE-
    
    

    ///------------------------------------------------------------------------
    public static partial class Vector2Extensions
    {
        //--
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
        
        //--
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
        
        //--
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
        
        
    }
}
