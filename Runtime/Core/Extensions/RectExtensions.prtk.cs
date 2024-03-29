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
    public static partial class RectExtensions
    {
        
        //--
        #region Swizzle Rect
        public static Rect xxxx(this Rect v) { return new Rect(v.x, v.x, v.x, v.x); }
        public static Rect xxxy(this Rect v) { return new Rect(v.x, v.x, v.x, v.y); }
        public static Rect xxxw(this Rect v) { return new Rect(v.x, v.x, v.x, v.width); }
        public static Rect xxxh(this Rect v) { return new Rect(v.x, v.x, v.x, v.height); }
        public static Rect xxxn(this Rect v, float n_0 = 0) { return new Rect(v.x, v.x, v.x, n_0); }
        public static Rect xxyx(this Rect v) { return new Rect(v.x, v.x, v.y, v.x); }
        public static Rect xxyy(this Rect v) { return new Rect(v.x, v.x, v.y, v.y); }
        public static Rect xxyw(this Rect v) { return new Rect(v.x, v.x, v.y, v.width); }
        public static Rect xxyh(this Rect v) { return new Rect(v.x, v.x, v.y, v.height); }
        public static Rect xxyn(this Rect v, float n_0 = 0) { return new Rect(v.x, v.x, v.y, n_0); }
        public static Rect xxwx(this Rect v) { return new Rect(v.x, v.x, v.width, v.x); }
        public static Rect xxwy(this Rect v) { return new Rect(v.x, v.x, v.width, v.y); }
        public static Rect xxww(this Rect v) { return new Rect(v.x, v.x, v.width, v.width); }
        public static Rect xxwh(this Rect v) { return new Rect(v.x, v.x, v.width, v.height); }
        public static Rect xxwn(this Rect v, float n_0 = 0) { return new Rect(v.x, v.x, v.width, n_0); }
        public static Rect xxhx(this Rect v) { return new Rect(v.x, v.x, v.height, v.x); }
        public static Rect xxhy(this Rect v) { return new Rect(v.x, v.x, v.height, v.y); }
        public static Rect xxhw(this Rect v) { return new Rect(v.x, v.x, v.height, v.width); }
        public static Rect xxhh(this Rect v) { return new Rect(v.x, v.x, v.height, v.height); }
        public static Rect xxhn(this Rect v, float n_0 = 0) { return new Rect(v.x, v.x, v.height, n_0); }
        public static Rect xxnx(this Rect v, float n_0 = 0) { return new Rect(v.x, v.x, n_0, v.x); }
        public static Rect xxny(this Rect v, float n_0 = 0) { return new Rect(v.x, v.x, n_0, v.y); }
        public static Rect xxnw(this Rect v, float n_0 = 0) { return new Rect(v.x, v.x, n_0, v.width); }
        public static Rect xxnh(this Rect v, float n_0 = 0) { return new Rect(v.x, v.x, n_0, v.height); }
        public static Rect xxnn(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(v.x, v.x, n_0, n_1); }
        public static Rect xyxx(this Rect v) { return new Rect(v.x, v.y, v.x, v.x); }
        public static Rect xyxy(this Rect v) { return new Rect(v.x, v.y, v.x, v.y); }
        public static Rect xyxw(this Rect v) { return new Rect(v.x, v.y, v.x, v.width); }
        public static Rect xyxh(this Rect v) { return new Rect(v.x, v.y, v.x, v.height); }
        public static Rect xyxn(this Rect v, float n_0 = 0) { return new Rect(v.x, v.y, v.x, n_0); }
        public static Rect xyyx(this Rect v) { return new Rect(v.x, v.y, v.y, v.x); }
        public static Rect xyyy(this Rect v) { return new Rect(v.x, v.y, v.y, v.y); }
        public static Rect xyyw(this Rect v) { return new Rect(v.x, v.y, v.y, v.width); }
        public static Rect xyyh(this Rect v) { return new Rect(v.x, v.y, v.y, v.height); }
        public static Rect xyyn(this Rect v, float n_0 = 0) { return new Rect(v.x, v.y, v.y, n_0); }
        public static Rect xywx(this Rect v) { return new Rect(v.x, v.y, v.width, v.x); }
        public static Rect xywy(this Rect v) { return new Rect(v.x, v.y, v.width, v.y); }
        public static Rect xyww(this Rect v) { return new Rect(v.x, v.y, v.width, v.width); }
        public static Rect xywh(this Rect v) { return new Rect(v.x, v.y, v.width, v.height); }
        public static Rect xywn(this Rect v, float n_0 = 0) { return new Rect(v.x, v.y, v.width, n_0); }
        public static Rect xyhx(this Rect v) { return new Rect(v.x, v.y, v.height, v.x); }
        public static Rect xyhy(this Rect v) { return new Rect(v.x, v.y, v.height, v.y); }
        public static Rect xyhw(this Rect v) { return new Rect(v.x, v.y, v.height, v.width); }
        public static Rect xyhh(this Rect v) { return new Rect(v.x, v.y, v.height, v.height); }
        public static Rect xyhn(this Rect v, float n_0 = 0) { return new Rect(v.x, v.y, v.height, n_0); }
        public static Rect xynx(this Rect v, float n_0 = 0) { return new Rect(v.x, v.y, n_0, v.x); }
        public static Rect xyny(this Rect v, float n_0 = 0) { return new Rect(v.x, v.y, n_0, v.y); }
        public static Rect xynw(this Rect v, float n_0 = 0) { return new Rect(v.x, v.y, n_0, v.width); }
        public static Rect xynh(this Rect v, float n_0 = 0) { return new Rect(v.x, v.y, n_0, v.height); }
        public static Rect xynn(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(v.x, v.y, n_0, n_1); }
        public static Rect xwxx(this Rect v) { return new Rect(v.x, v.width, v.x, v.x); }
        public static Rect xwxy(this Rect v) { return new Rect(v.x, v.width, v.x, v.y); }
        public static Rect xwxw(this Rect v) { return new Rect(v.x, v.width, v.x, v.width); }
        public static Rect xwxh(this Rect v) { return new Rect(v.x, v.width, v.x, v.height); }
        public static Rect xwxn(this Rect v, float n_0 = 0) { return new Rect(v.x, v.width, v.x, n_0); }
        public static Rect xwyx(this Rect v) { return new Rect(v.x, v.width, v.y, v.x); }
        public static Rect xwyy(this Rect v) { return new Rect(v.x, v.width, v.y, v.y); }
        public static Rect xwyw(this Rect v) { return new Rect(v.x, v.width, v.y, v.width); }
        public static Rect xwyh(this Rect v) { return new Rect(v.x, v.width, v.y, v.height); }
        public static Rect xwyn(this Rect v, float n_0 = 0) { return new Rect(v.x, v.width, v.y, n_0); }
        public static Rect xwwx(this Rect v) { return new Rect(v.x, v.width, v.width, v.x); }
        public static Rect xwwy(this Rect v) { return new Rect(v.x, v.width, v.width, v.y); }
        public static Rect xwww(this Rect v) { return new Rect(v.x, v.width, v.width, v.width); }
        public static Rect xwwh(this Rect v) { return new Rect(v.x, v.width, v.width, v.height); }
        public static Rect xwwn(this Rect v, float n_0 = 0) { return new Rect(v.x, v.width, v.width, n_0); }
        public static Rect xwhx(this Rect v) { return new Rect(v.x, v.width, v.height, v.x); }
        public static Rect xwhy(this Rect v) { return new Rect(v.x, v.width, v.height, v.y); }
        public static Rect xwhw(this Rect v) { return new Rect(v.x, v.width, v.height, v.width); }
        public static Rect xwhh(this Rect v) { return new Rect(v.x, v.width, v.height, v.height); }
        public static Rect xwhn(this Rect v, float n_0 = 0) { return new Rect(v.x, v.width, v.height, n_0); }
        public static Rect xwnx(this Rect v, float n_0 = 0) { return new Rect(v.x, v.width, n_0, v.x); }
        public static Rect xwny(this Rect v, float n_0 = 0) { return new Rect(v.x, v.width, n_0, v.y); }
        public static Rect xwnw(this Rect v, float n_0 = 0) { return new Rect(v.x, v.width, n_0, v.width); }
        public static Rect xwnh(this Rect v, float n_0 = 0) { return new Rect(v.x, v.width, n_0, v.height); }
        public static Rect xwnn(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(v.x, v.width, n_0, n_1); }
        public static Rect xhxx(this Rect v) { return new Rect(v.x, v.height, v.x, v.x); }
        public static Rect xhxy(this Rect v) { return new Rect(v.x, v.height, v.x, v.y); }
        public static Rect xhxw(this Rect v) { return new Rect(v.x, v.height, v.x, v.width); }
        public static Rect xhxh(this Rect v) { return new Rect(v.x, v.height, v.x, v.height); }
        public static Rect xhxn(this Rect v, float n_0 = 0) { return new Rect(v.x, v.height, v.x, n_0); }
        public static Rect xhyx(this Rect v) { return new Rect(v.x, v.height, v.y, v.x); }
        public static Rect xhyy(this Rect v) { return new Rect(v.x, v.height, v.y, v.y); }
        public static Rect xhyw(this Rect v) { return new Rect(v.x, v.height, v.y, v.width); }
        public static Rect xhyh(this Rect v) { return new Rect(v.x, v.height, v.y, v.height); }
        public static Rect xhyn(this Rect v, float n_0 = 0) { return new Rect(v.x, v.height, v.y, n_0); }
        public static Rect xhwx(this Rect v) { return new Rect(v.x, v.height, v.width, v.x); }
        public static Rect xhwy(this Rect v) { return new Rect(v.x, v.height, v.width, v.y); }
        public static Rect xhww(this Rect v) { return new Rect(v.x, v.height, v.width, v.width); }
        public static Rect xhwh(this Rect v) { return new Rect(v.x, v.height, v.width, v.height); }
        public static Rect xhwn(this Rect v, float n_0 = 0) { return new Rect(v.x, v.height, v.width, n_0); }
        public static Rect xhhx(this Rect v) { return new Rect(v.x, v.height, v.height, v.x); }
        public static Rect xhhy(this Rect v) { return new Rect(v.x, v.height, v.height, v.y); }
        public static Rect xhhw(this Rect v) { return new Rect(v.x, v.height, v.height, v.width); }
        public static Rect xhhh(this Rect v) { return new Rect(v.x, v.height, v.height, v.height); }
        public static Rect xhhn(this Rect v, float n_0 = 0) { return new Rect(v.x, v.height, v.height, n_0); }
        public static Rect xhnx(this Rect v, float n_0 = 0) { return new Rect(v.x, v.height, n_0, v.x); }
        public static Rect xhny(this Rect v, float n_0 = 0) { return new Rect(v.x, v.height, n_0, v.y); }
        public static Rect xhnw(this Rect v, float n_0 = 0) { return new Rect(v.x, v.height, n_0, v.width); }
        public static Rect xhnh(this Rect v, float n_0 = 0) { return new Rect(v.x, v.height, n_0, v.height); }
        public static Rect xhnn(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(v.x, v.height, n_0, n_1); }
        public static Rect xnxx(this Rect v, float n_0 = 0) { return new Rect(v.x, n_0, v.x, v.x); }
        public static Rect xnxy(this Rect v, float n_0 = 0) { return new Rect(v.x, n_0, v.x, v.y); }
        public static Rect xnxw(this Rect v, float n_0 = 0) { return new Rect(v.x, n_0, v.x, v.width); }
        public static Rect xnxh(this Rect v, float n_0 = 0) { return new Rect(v.x, n_0, v.x, v.height); }
        public static Rect xnxn(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(v.x, n_0, v.x, n_1); }
        public static Rect xnyx(this Rect v, float n_0 = 0) { return new Rect(v.x, n_0, v.y, v.x); }
        public static Rect xnyy(this Rect v, float n_0 = 0) { return new Rect(v.x, n_0, v.y, v.y); }
        public static Rect xnyw(this Rect v, float n_0 = 0) { return new Rect(v.x, n_0, v.y, v.width); }
        public static Rect xnyh(this Rect v, float n_0 = 0) { return new Rect(v.x, n_0, v.y, v.height); }
        public static Rect xnyn(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(v.x, n_0, v.y, n_1); }
        public static Rect xnwx(this Rect v, float n_0 = 0) { return new Rect(v.x, n_0, v.width, v.x); }
        public static Rect xnwy(this Rect v, float n_0 = 0) { return new Rect(v.x, n_0, v.width, v.y); }
        public static Rect xnww(this Rect v, float n_0 = 0) { return new Rect(v.x, n_0, v.width, v.width); }
        public static Rect xnwh(this Rect v, float n_0 = 0) { return new Rect(v.x, n_0, v.width, v.height); }
        public static Rect xnwn(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(v.x, n_0, v.width, n_1); }
        public static Rect xnhx(this Rect v, float n_0 = 0) { return new Rect(v.x, n_0, v.height, v.x); }
        public static Rect xnhy(this Rect v, float n_0 = 0) { return new Rect(v.x, n_0, v.height, v.y); }
        public static Rect xnhw(this Rect v, float n_0 = 0) { return new Rect(v.x, n_0, v.height, v.width); }
        public static Rect xnhh(this Rect v, float n_0 = 0) { return new Rect(v.x, n_0, v.height, v.height); }
        public static Rect xnhn(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(v.x, n_0, v.height, n_1); }
        public static Rect xnnx(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(v.x, n_0, n_1, v.x); }
        public static Rect xnny(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(v.x, n_0, n_1, v.y); }
        public static Rect xnnw(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(v.x, n_0, n_1, v.width); }
        public static Rect xnnh(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(v.x, n_0, n_1, v.height); }
        public static Rect xnnn(this Rect v, float n_0 = 0, float n_1 = 0, float n_2 = 0) { return new Rect(v.x, n_0, n_1, n_2); }
        public static Rect yxxx(this Rect v) { return new Rect(v.y, v.x, v.x, v.x); }
        public static Rect yxxy(this Rect v) { return new Rect(v.y, v.x, v.x, v.y); }
        public static Rect yxxw(this Rect v) { return new Rect(v.y, v.x, v.x, v.width); }
        public static Rect yxxh(this Rect v) { return new Rect(v.y, v.x, v.x, v.height); }
        public static Rect yxxn(this Rect v, float n_0 = 0) { return new Rect(v.y, v.x, v.x, n_0); }
        public static Rect yxyx(this Rect v) { return new Rect(v.y, v.x, v.y, v.x); }
        public static Rect yxyy(this Rect v) { return new Rect(v.y, v.x, v.y, v.y); }
        public static Rect yxyw(this Rect v) { return new Rect(v.y, v.x, v.y, v.width); }
        public static Rect yxyh(this Rect v) { return new Rect(v.y, v.x, v.y, v.height); }
        public static Rect yxyn(this Rect v, float n_0 = 0) { return new Rect(v.y, v.x, v.y, n_0); }
        public static Rect yxwx(this Rect v) { return new Rect(v.y, v.x, v.width, v.x); }
        public static Rect yxwy(this Rect v) { return new Rect(v.y, v.x, v.width, v.y); }
        public static Rect yxww(this Rect v) { return new Rect(v.y, v.x, v.width, v.width); }
        public static Rect yxwh(this Rect v) { return new Rect(v.y, v.x, v.width, v.height); }
        public static Rect yxwn(this Rect v, float n_0 = 0) { return new Rect(v.y, v.x, v.width, n_0); }
        public static Rect yxhx(this Rect v) { return new Rect(v.y, v.x, v.height, v.x); }
        public static Rect yxhy(this Rect v) { return new Rect(v.y, v.x, v.height, v.y); }
        public static Rect yxhw(this Rect v) { return new Rect(v.y, v.x, v.height, v.width); }
        public static Rect yxhh(this Rect v) { return new Rect(v.y, v.x, v.height, v.height); }
        public static Rect yxhn(this Rect v, float n_0 = 0) { return new Rect(v.y, v.x, v.height, n_0); }
        public static Rect yxnx(this Rect v, float n_0 = 0) { return new Rect(v.y, v.x, n_0, v.x); }
        public static Rect yxny(this Rect v, float n_0 = 0) { return new Rect(v.y, v.x, n_0, v.y); }
        public static Rect yxnw(this Rect v, float n_0 = 0) { return new Rect(v.y, v.x, n_0, v.width); }
        public static Rect yxnh(this Rect v, float n_0 = 0) { return new Rect(v.y, v.x, n_0, v.height); }
        public static Rect yxnn(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(v.y, v.x, n_0, n_1); }
        public static Rect yyxx(this Rect v) { return new Rect(v.y, v.y, v.x, v.x); }
        public static Rect yyxy(this Rect v) { return new Rect(v.y, v.y, v.x, v.y); }
        public static Rect yyxw(this Rect v) { return new Rect(v.y, v.y, v.x, v.width); }
        public static Rect yyxh(this Rect v) { return new Rect(v.y, v.y, v.x, v.height); }
        public static Rect yyxn(this Rect v, float n_0 = 0) { return new Rect(v.y, v.y, v.x, n_0); }
        public static Rect yyyx(this Rect v) { return new Rect(v.y, v.y, v.y, v.x); }
        public static Rect yyyy(this Rect v) { return new Rect(v.y, v.y, v.y, v.y); }
        public static Rect yyyw(this Rect v) { return new Rect(v.y, v.y, v.y, v.width); }
        public static Rect yyyh(this Rect v) { return new Rect(v.y, v.y, v.y, v.height); }
        public static Rect yyyn(this Rect v, float n_0 = 0) { return new Rect(v.y, v.y, v.y, n_0); }
        public static Rect yywx(this Rect v) { return new Rect(v.y, v.y, v.width, v.x); }
        public static Rect yywy(this Rect v) { return new Rect(v.y, v.y, v.width, v.y); }
        public static Rect yyww(this Rect v) { return new Rect(v.y, v.y, v.width, v.width); }
        public static Rect yywh(this Rect v) { return new Rect(v.y, v.y, v.width, v.height); }
        public static Rect yywn(this Rect v, float n_0 = 0) { return new Rect(v.y, v.y, v.width, n_0); }
        public static Rect yyhx(this Rect v) { return new Rect(v.y, v.y, v.height, v.x); }
        public static Rect yyhy(this Rect v) { return new Rect(v.y, v.y, v.height, v.y); }
        public static Rect yyhw(this Rect v) { return new Rect(v.y, v.y, v.height, v.width); }
        public static Rect yyhh(this Rect v) { return new Rect(v.y, v.y, v.height, v.height); }
        public static Rect yyhn(this Rect v, float n_0 = 0) { return new Rect(v.y, v.y, v.height, n_0); }
        public static Rect yynx(this Rect v, float n_0 = 0) { return new Rect(v.y, v.y, n_0, v.x); }
        public static Rect yyny(this Rect v, float n_0 = 0) { return new Rect(v.y, v.y, n_0, v.y); }
        public static Rect yynw(this Rect v, float n_0 = 0) { return new Rect(v.y, v.y, n_0, v.width); }
        public static Rect yynh(this Rect v, float n_0 = 0) { return new Rect(v.y, v.y, n_0, v.height); }
        public static Rect yynn(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(v.y, v.y, n_0, n_1); }
        public static Rect ywxx(this Rect v) { return new Rect(v.y, v.width, v.x, v.x); }
        public static Rect ywxy(this Rect v) { return new Rect(v.y, v.width, v.x, v.y); }
        public static Rect ywxw(this Rect v) { return new Rect(v.y, v.width, v.x, v.width); }
        public static Rect ywxh(this Rect v) { return new Rect(v.y, v.width, v.x, v.height); }
        public static Rect ywxn(this Rect v, float n_0 = 0) { return new Rect(v.y, v.width, v.x, n_0); }
        public static Rect ywyx(this Rect v) { return new Rect(v.y, v.width, v.y, v.x); }
        public static Rect ywyy(this Rect v) { return new Rect(v.y, v.width, v.y, v.y); }
        public static Rect ywyw(this Rect v) { return new Rect(v.y, v.width, v.y, v.width); }
        public static Rect ywyh(this Rect v) { return new Rect(v.y, v.width, v.y, v.height); }
        public static Rect ywyn(this Rect v, float n_0 = 0) { return new Rect(v.y, v.width, v.y, n_0); }
        public static Rect ywwx(this Rect v) { return new Rect(v.y, v.width, v.width, v.x); }
        public static Rect ywwy(this Rect v) { return new Rect(v.y, v.width, v.width, v.y); }
        public static Rect ywww(this Rect v) { return new Rect(v.y, v.width, v.width, v.width); }
        public static Rect ywwh(this Rect v) { return new Rect(v.y, v.width, v.width, v.height); }
        public static Rect ywwn(this Rect v, float n_0 = 0) { return new Rect(v.y, v.width, v.width, n_0); }
        public static Rect ywhx(this Rect v) { return new Rect(v.y, v.width, v.height, v.x); }
        public static Rect ywhy(this Rect v) { return new Rect(v.y, v.width, v.height, v.y); }
        public static Rect ywhw(this Rect v) { return new Rect(v.y, v.width, v.height, v.width); }
        public static Rect ywhh(this Rect v) { return new Rect(v.y, v.width, v.height, v.height); }
        public static Rect ywhn(this Rect v, float n_0 = 0) { return new Rect(v.y, v.width, v.height, n_0); }
        public static Rect ywnx(this Rect v, float n_0 = 0) { return new Rect(v.y, v.width, n_0, v.x); }
        public static Rect ywny(this Rect v, float n_0 = 0) { return new Rect(v.y, v.width, n_0, v.y); }
        public static Rect ywnw(this Rect v, float n_0 = 0) { return new Rect(v.y, v.width, n_0, v.width); }
        public static Rect ywnh(this Rect v, float n_0 = 0) { return new Rect(v.y, v.width, n_0, v.height); }
        public static Rect ywnn(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(v.y, v.width, n_0, n_1); }
        public static Rect yhxx(this Rect v) { return new Rect(v.y, v.height, v.x, v.x); }
        public static Rect yhxy(this Rect v) { return new Rect(v.y, v.height, v.x, v.y); }
        public static Rect yhxw(this Rect v) { return new Rect(v.y, v.height, v.x, v.width); }
        public static Rect yhxh(this Rect v) { return new Rect(v.y, v.height, v.x, v.height); }
        public static Rect yhxn(this Rect v, float n_0 = 0) { return new Rect(v.y, v.height, v.x, n_0); }
        public static Rect yhyx(this Rect v) { return new Rect(v.y, v.height, v.y, v.x); }
        public static Rect yhyy(this Rect v) { return new Rect(v.y, v.height, v.y, v.y); }
        public static Rect yhyw(this Rect v) { return new Rect(v.y, v.height, v.y, v.width); }
        public static Rect yhyh(this Rect v) { return new Rect(v.y, v.height, v.y, v.height); }
        public static Rect yhyn(this Rect v, float n_0 = 0) { return new Rect(v.y, v.height, v.y, n_0); }
        public static Rect yhwx(this Rect v) { return new Rect(v.y, v.height, v.width, v.x); }
        public static Rect yhwy(this Rect v) { return new Rect(v.y, v.height, v.width, v.y); }
        public static Rect yhww(this Rect v) { return new Rect(v.y, v.height, v.width, v.width); }
        public static Rect yhwh(this Rect v) { return new Rect(v.y, v.height, v.width, v.height); }
        public static Rect yhwn(this Rect v, float n_0 = 0) { return new Rect(v.y, v.height, v.width, n_0); }
        public static Rect yhhx(this Rect v) { return new Rect(v.y, v.height, v.height, v.x); }
        public static Rect yhhy(this Rect v) { return new Rect(v.y, v.height, v.height, v.y); }
        public static Rect yhhw(this Rect v) { return new Rect(v.y, v.height, v.height, v.width); }
        public static Rect yhhh(this Rect v) { return new Rect(v.y, v.height, v.height, v.height); }
        public static Rect yhhn(this Rect v, float n_0 = 0) { return new Rect(v.y, v.height, v.height, n_0); }
        public static Rect yhnx(this Rect v, float n_0 = 0) { return new Rect(v.y, v.height, n_0, v.x); }
        public static Rect yhny(this Rect v, float n_0 = 0) { return new Rect(v.y, v.height, n_0, v.y); }
        public static Rect yhnw(this Rect v, float n_0 = 0) { return new Rect(v.y, v.height, n_0, v.width); }
        public static Rect yhnh(this Rect v, float n_0 = 0) { return new Rect(v.y, v.height, n_0, v.height); }
        public static Rect yhnn(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(v.y, v.height, n_0, n_1); }
        public static Rect ynxx(this Rect v, float n_0 = 0) { return new Rect(v.y, n_0, v.x, v.x); }
        public static Rect ynxy(this Rect v, float n_0 = 0) { return new Rect(v.y, n_0, v.x, v.y); }
        public static Rect ynxw(this Rect v, float n_0 = 0) { return new Rect(v.y, n_0, v.x, v.width); }
        public static Rect ynxh(this Rect v, float n_0 = 0) { return new Rect(v.y, n_0, v.x, v.height); }
        public static Rect ynxn(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(v.y, n_0, v.x, n_1); }
        public static Rect ynyx(this Rect v, float n_0 = 0) { return new Rect(v.y, n_0, v.y, v.x); }
        public static Rect ynyy(this Rect v, float n_0 = 0) { return new Rect(v.y, n_0, v.y, v.y); }
        public static Rect ynyw(this Rect v, float n_0 = 0) { return new Rect(v.y, n_0, v.y, v.width); }
        public static Rect ynyh(this Rect v, float n_0 = 0) { return new Rect(v.y, n_0, v.y, v.height); }
        public static Rect ynyn(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(v.y, n_0, v.y, n_1); }
        public static Rect ynwx(this Rect v, float n_0 = 0) { return new Rect(v.y, n_0, v.width, v.x); }
        public static Rect ynwy(this Rect v, float n_0 = 0) { return new Rect(v.y, n_0, v.width, v.y); }
        public static Rect ynww(this Rect v, float n_0 = 0) { return new Rect(v.y, n_0, v.width, v.width); }
        public static Rect ynwh(this Rect v, float n_0 = 0) { return new Rect(v.y, n_0, v.width, v.height); }
        public static Rect ynwn(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(v.y, n_0, v.width, n_1); }
        public static Rect ynhx(this Rect v, float n_0 = 0) { return new Rect(v.y, n_0, v.height, v.x); }
        public static Rect ynhy(this Rect v, float n_0 = 0) { return new Rect(v.y, n_0, v.height, v.y); }
        public static Rect ynhw(this Rect v, float n_0 = 0) { return new Rect(v.y, n_0, v.height, v.width); }
        public static Rect ynhh(this Rect v, float n_0 = 0) { return new Rect(v.y, n_0, v.height, v.height); }
        public static Rect ynhn(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(v.y, n_0, v.height, n_1); }
        public static Rect ynnx(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(v.y, n_0, n_1, v.x); }
        public static Rect ynny(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(v.y, n_0, n_1, v.y); }
        public static Rect ynnw(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(v.y, n_0, n_1, v.width); }
        public static Rect ynnh(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(v.y, n_0, n_1, v.height); }
        public static Rect ynnn(this Rect v, float n_0 = 0, float n_1 = 0, float n_2 = 0) { return new Rect(v.y, n_0, n_1, n_2); }
        public static Rect wxxx(this Rect v) { return new Rect(v.width, v.x, v.x, v.x); }
        public static Rect wxxy(this Rect v) { return new Rect(v.width, v.x, v.x, v.y); }
        public static Rect wxxw(this Rect v) { return new Rect(v.width, v.x, v.x, v.width); }
        public static Rect wxxh(this Rect v) { return new Rect(v.width, v.x, v.x, v.height); }
        public static Rect wxxn(this Rect v, float n_0 = 0) { return new Rect(v.width, v.x, v.x, n_0); }
        public static Rect wxyx(this Rect v) { return new Rect(v.width, v.x, v.y, v.x); }
        public static Rect wxyy(this Rect v) { return new Rect(v.width, v.x, v.y, v.y); }
        public static Rect wxyw(this Rect v) { return new Rect(v.width, v.x, v.y, v.width); }
        public static Rect wxyh(this Rect v) { return new Rect(v.width, v.x, v.y, v.height); }
        public static Rect wxyn(this Rect v, float n_0 = 0) { return new Rect(v.width, v.x, v.y, n_0); }
        public static Rect wxwx(this Rect v) { return new Rect(v.width, v.x, v.width, v.x); }
        public static Rect wxwy(this Rect v) { return new Rect(v.width, v.x, v.width, v.y); }
        public static Rect wxww(this Rect v) { return new Rect(v.width, v.x, v.width, v.width); }
        public static Rect wxwh(this Rect v) { return new Rect(v.width, v.x, v.width, v.height); }
        public static Rect wxwn(this Rect v, float n_0 = 0) { return new Rect(v.width, v.x, v.width, n_0); }
        public static Rect wxhx(this Rect v) { return new Rect(v.width, v.x, v.height, v.x); }
        public static Rect wxhy(this Rect v) { return new Rect(v.width, v.x, v.height, v.y); }
        public static Rect wxhw(this Rect v) { return new Rect(v.width, v.x, v.height, v.width); }
        public static Rect wxhh(this Rect v) { return new Rect(v.width, v.x, v.height, v.height); }
        public static Rect wxhn(this Rect v, float n_0 = 0) { return new Rect(v.width, v.x, v.height, n_0); }
        public static Rect wxnx(this Rect v, float n_0 = 0) { return new Rect(v.width, v.x, n_0, v.x); }
        public static Rect wxny(this Rect v, float n_0 = 0) { return new Rect(v.width, v.x, n_0, v.y); }
        public static Rect wxnw(this Rect v, float n_0 = 0) { return new Rect(v.width, v.x, n_0, v.width); }
        public static Rect wxnh(this Rect v, float n_0 = 0) { return new Rect(v.width, v.x, n_0, v.height); }
        public static Rect wxnn(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(v.width, v.x, n_0, n_1); }
        public static Rect wyxx(this Rect v) { return new Rect(v.width, v.y, v.x, v.x); }
        public static Rect wyxy(this Rect v) { return new Rect(v.width, v.y, v.x, v.y); }
        public static Rect wyxw(this Rect v) { return new Rect(v.width, v.y, v.x, v.width); }
        public static Rect wyxh(this Rect v) { return new Rect(v.width, v.y, v.x, v.height); }
        public static Rect wyxn(this Rect v, float n_0 = 0) { return new Rect(v.width, v.y, v.x, n_0); }
        public static Rect wyyx(this Rect v) { return new Rect(v.width, v.y, v.y, v.x); }
        public static Rect wyyy(this Rect v) { return new Rect(v.width, v.y, v.y, v.y); }
        public static Rect wyyw(this Rect v) { return new Rect(v.width, v.y, v.y, v.width); }
        public static Rect wyyh(this Rect v) { return new Rect(v.width, v.y, v.y, v.height); }
        public static Rect wyyn(this Rect v, float n_0 = 0) { return new Rect(v.width, v.y, v.y, n_0); }
        public static Rect wywx(this Rect v) { return new Rect(v.width, v.y, v.width, v.x); }
        public static Rect wywy(this Rect v) { return new Rect(v.width, v.y, v.width, v.y); }
        public static Rect wyww(this Rect v) { return new Rect(v.width, v.y, v.width, v.width); }
        public static Rect wywh(this Rect v) { return new Rect(v.width, v.y, v.width, v.height); }
        public static Rect wywn(this Rect v, float n_0 = 0) { return new Rect(v.width, v.y, v.width, n_0); }
        public static Rect wyhx(this Rect v) { return new Rect(v.width, v.y, v.height, v.x); }
        public static Rect wyhy(this Rect v) { return new Rect(v.width, v.y, v.height, v.y); }
        public static Rect wyhw(this Rect v) { return new Rect(v.width, v.y, v.height, v.width); }
        public static Rect wyhh(this Rect v) { return new Rect(v.width, v.y, v.height, v.height); }
        public static Rect wyhn(this Rect v, float n_0 = 0) { return new Rect(v.width, v.y, v.height, n_0); }
        public static Rect wynx(this Rect v, float n_0 = 0) { return new Rect(v.width, v.y, n_0, v.x); }
        public static Rect wyny(this Rect v, float n_0 = 0) { return new Rect(v.width, v.y, n_0, v.y); }
        public static Rect wynw(this Rect v, float n_0 = 0) { return new Rect(v.width, v.y, n_0, v.width); }
        public static Rect wynh(this Rect v, float n_0 = 0) { return new Rect(v.width, v.y, n_0, v.height); }
        public static Rect wynn(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(v.width, v.y, n_0, n_1); }
        public static Rect wwxx(this Rect v) { return new Rect(v.width, v.width, v.x, v.x); }
        public static Rect wwxy(this Rect v) { return new Rect(v.width, v.width, v.x, v.y); }
        public static Rect wwxw(this Rect v) { return new Rect(v.width, v.width, v.x, v.width); }
        public static Rect wwxh(this Rect v) { return new Rect(v.width, v.width, v.x, v.height); }
        public static Rect wwxn(this Rect v, float n_0 = 0) { return new Rect(v.width, v.width, v.x, n_0); }
        public static Rect wwyx(this Rect v) { return new Rect(v.width, v.width, v.y, v.x); }
        public static Rect wwyy(this Rect v) { return new Rect(v.width, v.width, v.y, v.y); }
        public static Rect wwyw(this Rect v) { return new Rect(v.width, v.width, v.y, v.width); }
        public static Rect wwyh(this Rect v) { return new Rect(v.width, v.width, v.y, v.height); }
        public static Rect wwyn(this Rect v, float n_0 = 0) { return new Rect(v.width, v.width, v.y, n_0); }
        public static Rect wwwx(this Rect v) { return new Rect(v.width, v.width, v.width, v.x); }
        public static Rect wwwy(this Rect v) { return new Rect(v.width, v.width, v.width, v.y); }
        public static Rect wwww(this Rect v) { return new Rect(v.width, v.width, v.width, v.width); }
        public static Rect wwwh(this Rect v) { return new Rect(v.width, v.width, v.width, v.height); }
        public static Rect wwwn(this Rect v, float n_0 = 0) { return new Rect(v.width, v.width, v.width, n_0); }
        public static Rect wwhx(this Rect v) { return new Rect(v.width, v.width, v.height, v.x); }
        public static Rect wwhy(this Rect v) { return new Rect(v.width, v.width, v.height, v.y); }
        public static Rect wwhw(this Rect v) { return new Rect(v.width, v.width, v.height, v.width); }
        public static Rect wwhh(this Rect v) { return new Rect(v.width, v.width, v.height, v.height); }
        public static Rect wwhn(this Rect v, float n_0 = 0) { return new Rect(v.width, v.width, v.height, n_0); }
        public static Rect wwnx(this Rect v, float n_0 = 0) { return new Rect(v.width, v.width, n_0, v.x); }
        public static Rect wwny(this Rect v, float n_0 = 0) { return new Rect(v.width, v.width, n_0, v.y); }
        public static Rect wwnw(this Rect v, float n_0 = 0) { return new Rect(v.width, v.width, n_0, v.width); }
        public static Rect wwnh(this Rect v, float n_0 = 0) { return new Rect(v.width, v.width, n_0, v.height); }
        public static Rect wwnn(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(v.width, v.width, n_0, n_1); }
        public static Rect whxx(this Rect v) { return new Rect(v.width, v.height, v.x, v.x); }
        public static Rect whxy(this Rect v) { return new Rect(v.width, v.height, v.x, v.y); }
        public static Rect whxw(this Rect v) { return new Rect(v.width, v.height, v.x, v.width); }
        public static Rect whxh(this Rect v) { return new Rect(v.width, v.height, v.x, v.height); }
        public static Rect whxn(this Rect v, float n_0 = 0) { return new Rect(v.width, v.height, v.x, n_0); }
        public static Rect whyx(this Rect v) { return new Rect(v.width, v.height, v.y, v.x); }
        public static Rect whyy(this Rect v) { return new Rect(v.width, v.height, v.y, v.y); }
        public static Rect whyw(this Rect v) { return new Rect(v.width, v.height, v.y, v.width); }
        public static Rect whyh(this Rect v) { return new Rect(v.width, v.height, v.y, v.height); }
        public static Rect whyn(this Rect v, float n_0 = 0) { return new Rect(v.width, v.height, v.y, n_0); }
        public static Rect whwx(this Rect v) { return new Rect(v.width, v.height, v.width, v.x); }
        public static Rect whwy(this Rect v) { return new Rect(v.width, v.height, v.width, v.y); }
        public static Rect whww(this Rect v) { return new Rect(v.width, v.height, v.width, v.width); }
        public static Rect whwh(this Rect v) { return new Rect(v.width, v.height, v.width, v.height); }
        public static Rect whwn(this Rect v, float n_0 = 0) { return new Rect(v.width, v.height, v.width, n_0); }
        public static Rect whhx(this Rect v) { return new Rect(v.width, v.height, v.height, v.x); }
        public static Rect whhy(this Rect v) { return new Rect(v.width, v.height, v.height, v.y); }
        public static Rect whhw(this Rect v) { return new Rect(v.width, v.height, v.height, v.width); }
        public static Rect whhh(this Rect v) { return new Rect(v.width, v.height, v.height, v.height); }
        public static Rect whhn(this Rect v, float n_0 = 0) { return new Rect(v.width, v.height, v.height, n_0); }
        public static Rect whnx(this Rect v, float n_0 = 0) { return new Rect(v.width, v.height, n_0, v.x); }
        public static Rect whny(this Rect v, float n_0 = 0) { return new Rect(v.width, v.height, n_0, v.y); }
        public static Rect whnw(this Rect v, float n_0 = 0) { return new Rect(v.width, v.height, n_0, v.width); }
        public static Rect whnh(this Rect v, float n_0 = 0) { return new Rect(v.width, v.height, n_0, v.height); }
        public static Rect whnn(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(v.width, v.height, n_0, n_1); }
        public static Rect wnxx(this Rect v, float n_0 = 0) { return new Rect(v.width, n_0, v.x, v.x); }
        public static Rect wnxy(this Rect v, float n_0 = 0) { return new Rect(v.width, n_0, v.x, v.y); }
        public static Rect wnxw(this Rect v, float n_0 = 0) { return new Rect(v.width, n_0, v.x, v.width); }
        public static Rect wnxh(this Rect v, float n_0 = 0) { return new Rect(v.width, n_0, v.x, v.height); }
        public static Rect wnxn(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(v.width, n_0, v.x, n_1); }
        public static Rect wnyx(this Rect v, float n_0 = 0) { return new Rect(v.width, n_0, v.y, v.x); }
        public static Rect wnyy(this Rect v, float n_0 = 0) { return new Rect(v.width, n_0, v.y, v.y); }
        public static Rect wnyw(this Rect v, float n_0 = 0) { return new Rect(v.width, n_0, v.y, v.width); }
        public static Rect wnyh(this Rect v, float n_0 = 0) { return new Rect(v.width, n_0, v.y, v.height); }
        public static Rect wnyn(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(v.width, n_0, v.y, n_1); }
        public static Rect wnwx(this Rect v, float n_0 = 0) { return new Rect(v.width, n_0, v.width, v.x); }
        public static Rect wnwy(this Rect v, float n_0 = 0) { return new Rect(v.width, n_0, v.width, v.y); }
        public static Rect wnww(this Rect v, float n_0 = 0) { return new Rect(v.width, n_0, v.width, v.width); }
        public static Rect wnwh(this Rect v, float n_0 = 0) { return new Rect(v.width, n_0, v.width, v.height); }
        public static Rect wnwn(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(v.width, n_0, v.width, n_1); }
        public static Rect wnhx(this Rect v, float n_0 = 0) { return new Rect(v.width, n_0, v.height, v.x); }
        public static Rect wnhy(this Rect v, float n_0 = 0) { return new Rect(v.width, n_0, v.height, v.y); }
        public static Rect wnhw(this Rect v, float n_0 = 0) { return new Rect(v.width, n_0, v.height, v.width); }
        public static Rect wnhh(this Rect v, float n_0 = 0) { return new Rect(v.width, n_0, v.height, v.height); }
        public static Rect wnhn(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(v.width, n_0, v.height, n_1); }
        public static Rect wnnx(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(v.width, n_0, n_1, v.x); }
        public static Rect wnny(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(v.width, n_0, n_1, v.y); }
        public static Rect wnnw(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(v.width, n_0, n_1, v.width); }
        public static Rect wnnh(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(v.width, n_0, n_1, v.height); }
        public static Rect wnnn(this Rect v, float n_0 = 0, float n_1 = 0, float n_2 = 0) { return new Rect(v.width, n_0, n_1, n_2); }
        public static Rect hxxx(this Rect v) { return new Rect(v.height, v.x, v.x, v.x); }
        public static Rect hxxy(this Rect v) { return new Rect(v.height, v.x, v.x, v.y); }
        public static Rect hxxw(this Rect v) { return new Rect(v.height, v.x, v.x, v.width); }
        public static Rect hxxh(this Rect v) { return new Rect(v.height, v.x, v.x, v.height); }
        public static Rect hxxn(this Rect v, float n_0 = 0) { return new Rect(v.height, v.x, v.x, n_0); }
        public static Rect hxyx(this Rect v) { return new Rect(v.height, v.x, v.y, v.x); }
        public static Rect hxyy(this Rect v) { return new Rect(v.height, v.x, v.y, v.y); }
        public static Rect hxyw(this Rect v) { return new Rect(v.height, v.x, v.y, v.width); }
        public static Rect hxyh(this Rect v) { return new Rect(v.height, v.x, v.y, v.height); }
        public static Rect hxyn(this Rect v, float n_0 = 0) { return new Rect(v.height, v.x, v.y, n_0); }
        public static Rect hxwx(this Rect v) { return new Rect(v.height, v.x, v.width, v.x); }
        public static Rect hxwy(this Rect v) { return new Rect(v.height, v.x, v.width, v.y); }
        public static Rect hxww(this Rect v) { return new Rect(v.height, v.x, v.width, v.width); }
        public static Rect hxwh(this Rect v) { return new Rect(v.height, v.x, v.width, v.height); }
        public static Rect hxwn(this Rect v, float n_0 = 0) { return new Rect(v.height, v.x, v.width, n_0); }
        public static Rect hxhx(this Rect v) { return new Rect(v.height, v.x, v.height, v.x); }
        public static Rect hxhy(this Rect v) { return new Rect(v.height, v.x, v.height, v.y); }
        public static Rect hxhw(this Rect v) { return new Rect(v.height, v.x, v.height, v.width); }
        public static Rect hxhh(this Rect v) { return new Rect(v.height, v.x, v.height, v.height); }
        public static Rect hxhn(this Rect v, float n_0 = 0) { return new Rect(v.height, v.x, v.height, n_0); }
        public static Rect hxnx(this Rect v, float n_0 = 0) { return new Rect(v.height, v.x, n_0, v.x); }
        public static Rect hxny(this Rect v, float n_0 = 0) { return new Rect(v.height, v.x, n_0, v.y); }
        public static Rect hxnw(this Rect v, float n_0 = 0) { return new Rect(v.height, v.x, n_0, v.width); }
        public static Rect hxnh(this Rect v, float n_0 = 0) { return new Rect(v.height, v.x, n_0, v.height); }
        public static Rect hxnn(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(v.height, v.x, n_0, n_1); }
        public static Rect hyxx(this Rect v) { return new Rect(v.height, v.y, v.x, v.x); }
        public static Rect hyxy(this Rect v) { return new Rect(v.height, v.y, v.x, v.y); }
        public static Rect hyxw(this Rect v) { return new Rect(v.height, v.y, v.x, v.width); }
        public static Rect hyxh(this Rect v) { return new Rect(v.height, v.y, v.x, v.height); }
        public static Rect hyxn(this Rect v, float n_0 = 0) { return new Rect(v.height, v.y, v.x, n_0); }
        public static Rect hyyx(this Rect v) { return new Rect(v.height, v.y, v.y, v.x); }
        public static Rect hyyy(this Rect v) { return new Rect(v.height, v.y, v.y, v.y); }
        public static Rect hyyw(this Rect v) { return new Rect(v.height, v.y, v.y, v.width); }
        public static Rect hyyh(this Rect v) { return new Rect(v.height, v.y, v.y, v.height); }
        public static Rect hyyn(this Rect v, float n_0 = 0) { return new Rect(v.height, v.y, v.y, n_0); }
        public static Rect hywx(this Rect v) { return new Rect(v.height, v.y, v.width, v.x); }
        public static Rect hywy(this Rect v) { return new Rect(v.height, v.y, v.width, v.y); }
        public static Rect hyww(this Rect v) { return new Rect(v.height, v.y, v.width, v.width); }
        public static Rect hywh(this Rect v) { return new Rect(v.height, v.y, v.width, v.height); }
        public static Rect hywn(this Rect v, float n_0 = 0) { return new Rect(v.height, v.y, v.width, n_0); }
        public static Rect hyhx(this Rect v) { return new Rect(v.height, v.y, v.height, v.x); }
        public static Rect hyhy(this Rect v) { return new Rect(v.height, v.y, v.height, v.y); }
        public static Rect hyhw(this Rect v) { return new Rect(v.height, v.y, v.height, v.width); }
        public static Rect hyhh(this Rect v) { return new Rect(v.height, v.y, v.height, v.height); }
        public static Rect hyhn(this Rect v, float n_0 = 0) { return new Rect(v.height, v.y, v.height, n_0); }
        public static Rect hynx(this Rect v, float n_0 = 0) { return new Rect(v.height, v.y, n_0, v.x); }
        public static Rect hyny(this Rect v, float n_0 = 0) { return new Rect(v.height, v.y, n_0, v.y); }
        public static Rect hynw(this Rect v, float n_0 = 0) { return new Rect(v.height, v.y, n_0, v.width); }
        public static Rect hynh(this Rect v, float n_0 = 0) { return new Rect(v.height, v.y, n_0, v.height); }
        public static Rect hynn(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(v.height, v.y, n_0, n_1); }
        public static Rect hwxx(this Rect v) { return new Rect(v.height, v.width, v.x, v.x); }
        public static Rect hwxy(this Rect v) { return new Rect(v.height, v.width, v.x, v.y); }
        public static Rect hwxw(this Rect v) { return new Rect(v.height, v.width, v.x, v.width); }
        public static Rect hwxh(this Rect v) { return new Rect(v.height, v.width, v.x, v.height); }
        public static Rect hwxn(this Rect v, float n_0 = 0) { return new Rect(v.height, v.width, v.x, n_0); }
        public static Rect hwyx(this Rect v) { return new Rect(v.height, v.width, v.y, v.x); }
        public static Rect hwyy(this Rect v) { return new Rect(v.height, v.width, v.y, v.y); }
        public static Rect hwyw(this Rect v) { return new Rect(v.height, v.width, v.y, v.width); }
        public static Rect hwyh(this Rect v) { return new Rect(v.height, v.width, v.y, v.height); }
        public static Rect hwyn(this Rect v, float n_0 = 0) { return new Rect(v.height, v.width, v.y, n_0); }
        public static Rect hwwx(this Rect v) { return new Rect(v.height, v.width, v.width, v.x); }
        public static Rect hwwy(this Rect v) { return new Rect(v.height, v.width, v.width, v.y); }
        public static Rect hwww(this Rect v) { return new Rect(v.height, v.width, v.width, v.width); }
        public static Rect hwwh(this Rect v) { return new Rect(v.height, v.width, v.width, v.height); }
        public static Rect hwwn(this Rect v, float n_0 = 0) { return new Rect(v.height, v.width, v.width, n_0); }
        public static Rect hwhx(this Rect v) { return new Rect(v.height, v.width, v.height, v.x); }
        public static Rect hwhy(this Rect v) { return new Rect(v.height, v.width, v.height, v.y); }
        public static Rect hwhw(this Rect v) { return new Rect(v.height, v.width, v.height, v.width); }
        public static Rect hwhh(this Rect v) { return new Rect(v.height, v.width, v.height, v.height); }
        public static Rect hwhn(this Rect v, float n_0 = 0) { return new Rect(v.height, v.width, v.height, n_0); }
        public static Rect hwnx(this Rect v, float n_0 = 0) { return new Rect(v.height, v.width, n_0, v.x); }
        public static Rect hwny(this Rect v, float n_0 = 0) { return new Rect(v.height, v.width, n_0, v.y); }
        public static Rect hwnw(this Rect v, float n_0 = 0) { return new Rect(v.height, v.width, n_0, v.width); }
        public static Rect hwnh(this Rect v, float n_0 = 0) { return new Rect(v.height, v.width, n_0, v.height); }
        public static Rect hwnn(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(v.height, v.width, n_0, n_1); }
        public static Rect hhxx(this Rect v) { return new Rect(v.height, v.height, v.x, v.x); }
        public static Rect hhxy(this Rect v) { return new Rect(v.height, v.height, v.x, v.y); }
        public static Rect hhxw(this Rect v) { return new Rect(v.height, v.height, v.x, v.width); }
        public static Rect hhxh(this Rect v) { return new Rect(v.height, v.height, v.x, v.height); }
        public static Rect hhxn(this Rect v, float n_0 = 0) { return new Rect(v.height, v.height, v.x, n_0); }
        public static Rect hhyx(this Rect v) { return new Rect(v.height, v.height, v.y, v.x); }
        public static Rect hhyy(this Rect v) { return new Rect(v.height, v.height, v.y, v.y); }
        public static Rect hhyw(this Rect v) { return new Rect(v.height, v.height, v.y, v.width); }
        public static Rect hhyh(this Rect v) { return new Rect(v.height, v.height, v.y, v.height); }
        public static Rect hhyn(this Rect v, float n_0 = 0) { return new Rect(v.height, v.height, v.y, n_0); }
        public static Rect hhwx(this Rect v) { return new Rect(v.height, v.height, v.width, v.x); }
        public static Rect hhwy(this Rect v) { return new Rect(v.height, v.height, v.width, v.y); }
        public static Rect hhww(this Rect v) { return new Rect(v.height, v.height, v.width, v.width); }
        public static Rect hhwh(this Rect v) { return new Rect(v.height, v.height, v.width, v.height); }
        public static Rect hhwn(this Rect v, float n_0 = 0) { return new Rect(v.height, v.height, v.width, n_0); }
        public static Rect hhhx(this Rect v) { return new Rect(v.height, v.height, v.height, v.x); }
        public static Rect hhhy(this Rect v) { return new Rect(v.height, v.height, v.height, v.y); }
        public static Rect hhhw(this Rect v) { return new Rect(v.height, v.height, v.height, v.width); }
        public static Rect hhhh(this Rect v) { return new Rect(v.height, v.height, v.height, v.height); }
        public static Rect hhhn(this Rect v, float n_0 = 0) { return new Rect(v.height, v.height, v.height, n_0); }
        public static Rect hhnx(this Rect v, float n_0 = 0) { return new Rect(v.height, v.height, n_0, v.x); }
        public static Rect hhny(this Rect v, float n_0 = 0) { return new Rect(v.height, v.height, n_0, v.y); }
        public static Rect hhnw(this Rect v, float n_0 = 0) { return new Rect(v.height, v.height, n_0, v.width); }
        public static Rect hhnh(this Rect v, float n_0 = 0) { return new Rect(v.height, v.height, n_0, v.height); }
        public static Rect hhnn(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(v.height, v.height, n_0, n_1); }
        public static Rect hnxx(this Rect v, float n_0 = 0) { return new Rect(v.height, n_0, v.x, v.x); }
        public static Rect hnxy(this Rect v, float n_0 = 0) { return new Rect(v.height, n_0, v.x, v.y); }
        public static Rect hnxw(this Rect v, float n_0 = 0) { return new Rect(v.height, n_0, v.x, v.width); }
        public static Rect hnxh(this Rect v, float n_0 = 0) { return new Rect(v.height, n_0, v.x, v.height); }
        public static Rect hnxn(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(v.height, n_0, v.x, n_1); }
        public static Rect hnyx(this Rect v, float n_0 = 0) { return new Rect(v.height, n_0, v.y, v.x); }
        public static Rect hnyy(this Rect v, float n_0 = 0) { return new Rect(v.height, n_0, v.y, v.y); }
        public static Rect hnyw(this Rect v, float n_0 = 0) { return new Rect(v.height, n_0, v.y, v.width); }
        public static Rect hnyh(this Rect v, float n_0 = 0) { return new Rect(v.height, n_0, v.y, v.height); }
        public static Rect hnyn(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(v.height, n_0, v.y, n_1); }
        public static Rect hnwx(this Rect v, float n_0 = 0) { return new Rect(v.height, n_0, v.width, v.x); }
        public static Rect hnwy(this Rect v, float n_0 = 0) { return new Rect(v.height, n_0, v.width, v.y); }
        public static Rect hnww(this Rect v, float n_0 = 0) { return new Rect(v.height, n_0, v.width, v.width); }
        public static Rect hnwh(this Rect v, float n_0 = 0) { return new Rect(v.height, n_0, v.width, v.height); }
        public static Rect hnwn(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(v.height, n_0, v.width, n_1); }
        public static Rect hnhx(this Rect v, float n_0 = 0) { return new Rect(v.height, n_0, v.height, v.x); }
        public static Rect hnhy(this Rect v, float n_0 = 0) { return new Rect(v.height, n_0, v.height, v.y); }
        public static Rect hnhw(this Rect v, float n_0 = 0) { return new Rect(v.height, n_0, v.height, v.width); }
        public static Rect hnhh(this Rect v, float n_0 = 0) { return new Rect(v.height, n_0, v.height, v.height); }
        public static Rect hnhn(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(v.height, n_0, v.height, n_1); }
        public static Rect hnnx(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(v.height, n_0, n_1, v.x); }
        public static Rect hnny(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(v.height, n_0, n_1, v.y); }
        public static Rect hnnw(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(v.height, n_0, n_1, v.width); }
        public static Rect hnnh(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(v.height, n_0, n_1, v.height); }
        public static Rect hnnn(this Rect v, float n_0 = 0, float n_1 = 0, float n_2 = 0) { return new Rect(v.height, n_0, n_1, n_2); }
        public static Rect nxxx(this Rect v, float n_0 = 0) { return new Rect(n_0, v.x, v.x, v.x); }
        public static Rect nxxy(this Rect v, float n_0 = 0) { return new Rect(n_0, v.x, v.x, v.y); }
        public static Rect nxxw(this Rect v, float n_0 = 0) { return new Rect(n_0, v.x, v.x, v.width); }
        public static Rect nxxh(this Rect v, float n_0 = 0) { return new Rect(n_0, v.x, v.x, v.height); }
        public static Rect nxxn(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(n_0, v.x, v.x, n_1); }
        public static Rect nxyx(this Rect v, float n_0 = 0) { return new Rect(n_0, v.x, v.y, v.x); }
        public static Rect nxyy(this Rect v, float n_0 = 0) { return new Rect(n_0, v.x, v.y, v.y); }
        public static Rect nxyw(this Rect v, float n_0 = 0) { return new Rect(n_0, v.x, v.y, v.width); }
        public static Rect nxyh(this Rect v, float n_0 = 0) { return new Rect(n_0, v.x, v.y, v.height); }
        public static Rect nxyn(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(n_0, v.x, v.y, n_1); }
        public static Rect nxwx(this Rect v, float n_0 = 0) { return new Rect(n_0, v.x, v.width, v.x); }
        public static Rect nxwy(this Rect v, float n_0 = 0) { return new Rect(n_0, v.x, v.width, v.y); }
        public static Rect nxww(this Rect v, float n_0 = 0) { return new Rect(n_0, v.x, v.width, v.width); }
        public static Rect nxwh(this Rect v, float n_0 = 0) { return new Rect(n_0, v.x, v.width, v.height); }
        public static Rect nxwn(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(n_0, v.x, v.width, n_1); }
        public static Rect nxhx(this Rect v, float n_0 = 0) { return new Rect(n_0, v.x, v.height, v.x); }
        public static Rect nxhy(this Rect v, float n_0 = 0) { return new Rect(n_0, v.x, v.height, v.y); }
        public static Rect nxhw(this Rect v, float n_0 = 0) { return new Rect(n_0, v.x, v.height, v.width); }
        public static Rect nxhh(this Rect v, float n_0 = 0) { return new Rect(n_0, v.x, v.height, v.height); }
        public static Rect nxhn(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(n_0, v.x, v.height, n_1); }
        public static Rect nxnx(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(n_0, v.x, n_1, v.x); }
        public static Rect nxny(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(n_0, v.x, n_1, v.y); }
        public static Rect nxnw(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(n_0, v.x, n_1, v.width); }
        public static Rect nxnh(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(n_0, v.x, n_1, v.height); }
        public static Rect nxnn(this Rect v, float n_0 = 0, float n_1 = 0, float n_2 = 0) { return new Rect(n_0, v.x, n_1, n_2); }
        public static Rect nyxx(this Rect v, float n_0 = 0) { return new Rect(n_0, v.y, v.x, v.x); }
        public static Rect nyxy(this Rect v, float n_0 = 0) { return new Rect(n_0, v.y, v.x, v.y); }
        public static Rect nyxw(this Rect v, float n_0 = 0) { return new Rect(n_0, v.y, v.x, v.width); }
        public static Rect nyxh(this Rect v, float n_0 = 0) { return new Rect(n_0, v.y, v.x, v.height); }
        public static Rect nyxn(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(n_0, v.y, v.x, n_1); }
        public static Rect nyyx(this Rect v, float n_0 = 0) { return new Rect(n_0, v.y, v.y, v.x); }
        public static Rect nyyy(this Rect v, float n_0 = 0) { return new Rect(n_0, v.y, v.y, v.y); }
        public static Rect nyyw(this Rect v, float n_0 = 0) { return new Rect(n_0, v.y, v.y, v.width); }
        public static Rect nyyh(this Rect v, float n_0 = 0) { return new Rect(n_0, v.y, v.y, v.height); }
        public static Rect nyyn(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(n_0, v.y, v.y, n_1); }
        public static Rect nywx(this Rect v, float n_0 = 0) { return new Rect(n_0, v.y, v.width, v.x); }
        public static Rect nywy(this Rect v, float n_0 = 0) { return new Rect(n_0, v.y, v.width, v.y); }
        public static Rect nyww(this Rect v, float n_0 = 0) { return new Rect(n_0, v.y, v.width, v.width); }
        public static Rect nywh(this Rect v, float n_0 = 0) { return new Rect(n_0, v.y, v.width, v.height); }
        public static Rect nywn(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(n_0, v.y, v.width, n_1); }
        public static Rect nyhx(this Rect v, float n_0 = 0) { return new Rect(n_0, v.y, v.height, v.x); }
        public static Rect nyhy(this Rect v, float n_0 = 0) { return new Rect(n_0, v.y, v.height, v.y); }
        public static Rect nyhw(this Rect v, float n_0 = 0) { return new Rect(n_0, v.y, v.height, v.width); }
        public static Rect nyhh(this Rect v, float n_0 = 0) { return new Rect(n_0, v.y, v.height, v.height); }
        public static Rect nyhn(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(n_0, v.y, v.height, n_1); }
        public static Rect nynx(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(n_0, v.y, n_1, v.x); }
        public static Rect nyny(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(n_0, v.y, n_1, v.y); }
        public static Rect nynw(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(n_0, v.y, n_1, v.width); }
        public static Rect nynh(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(n_0, v.y, n_1, v.height); }
        public static Rect nynn(this Rect v, float n_0 = 0, float n_1 = 0, float n_2 = 0) { return new Rect(n_0, v.y, n_1, n_2); }
        public static Rect nwxx(this Rect v, float n_0 = 0) { return new Rect(n_0, v.width, v.x, v.x); }
        public static Rect nwxy(this Rect v, float n_0 = 0) { return new Rect(n_0, v.width, v.x, v.y); }
        public static Rect nwxw(this Rect v, float n_0 = 0) { return new Rect(n_0, v.width, v.x, v.width); }
        public static Rect nwxh(this Rect v, float n_0 = 0) { return new Rect(n_0, v.width, v.x, v.height); }
        public static Rect nwxn(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(n_0, v.width, v.x, n_1); }
        public static Rect nwyx(this Rect v, float n_0 = 0) { return new Rect(n_0, v.width, v.y, v.x); }
        public static Rect nwyy(this Rect v, float n_0 = 0) { return new Rect(n_0, v.width, v.y, v.y); }
        public static Rect nwyw(this Rect v, float n_0 = 0) { return new Rect(n_0, v.width, v.y, v.width); }
        public static Rect nwyh(this Rect v, float n_0 = 0) { return new Rect(n_0, v.width, v.y, v.height); }
        public static Rect nwyn(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(n_0, v.width, v.y, n_1); }
        public static Rect nwwx(this Rect v, float n_0 = 0) { return new Rect(n_0, v.width, v.width, v.x); }
        public static Rect nwwy(this Rect v, float n_0 = 0) { return new Rect(n_0, v.width, v.width, v.y); }
        public static Rect nwww(this Rect v, float n_0 = 0) { return new Rect(n_0, v.width, v.width, v.width); }
        public static Rect nwwh(this Rect v, float n_0 = 0) { return new Rect(n_0, v.width, v.width, v.height); }
        public static Rect nwwn(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(n_0, v.width, v.width, n_1); }
        public static Rect nwhx(this Rect v, float n_0 = 0) { return new Rect(n_0, v.width, v.height, v.x); }
        public static Rect nwhy(this Rect v, float n_0 = 0) { return new Rect(n_0, v.width, v.height, v.y); }
        public static Rect nwhw(this Rect v, float n_0 = 0) { return new Rect(n_0, v.width, v.height, v.width); }
        public static Rect nwhh(this Rect v, float n_0 = 0) { return new Rect(n_0, v.width, v.height, v.height); }
        public static Rect nwhn(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(n_0, v.width, v.height, n_1); }
        public static Rect nwnx(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(n_0, v.width, n_1, v.x); }
        public static Rect nwny(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(n_0, v.width, n_1, v.y); }
        public static Rect nwnw(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(n_0, v.width, n_1, v.width); }
        public static Rect nwnh(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(n_0, v.width, n_1, v.height); }
        public static Rect nwnn(this Rect v, float n_0 = 0, float n_1 = 0, float n_2 = 0) { return new Rect(n_0, v.width, n_1, n_2); }
        public static Rect nhxx(this Rect v, float n_0 = 0) { return new Rect(n_0, v.height, v.x, v.x); }
        public static Rect nhxy(this Rect v, float n_0 = 0) { return new Rect(n_0, v.height, v.x, v.y); }
        public static Rect nhxw(this Rect v, float n_0 = 0) { return new Rect(n_0, v.height, v.x, v.width); }
        public static Rect nhxh(this Rect v, float n_0 = 0) { return new Rect(n_0, v.height, v.x, v.height); }
        public static Rect nhxn(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(n_0, v.height, v.x, n_1); }
        public static Rect nhyx(this Rect v, float n_0 = 0) { return new Rect(n_0, v.height, v.y, v.x); }
        public static Rect nhyy(this Rect v, float n_0 = 0) { return new Rect(n_0, v.height, v.y, v.y); }
        public static Rect nhyw(this Rect v, float n_0 = 0) { return new Rect(n_0, v.height, v.y, v.width); }
        public static Rect nhyh(this Rect v, float n_0 = 0) { return new Rect(n_0, v.height, v.y, v.height); }
        public static Rect nhyn(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(n_0, v.height, v.y, n_1); }
        public static Rect nhwx(this Rect v, float n_0 = 0) { return new Rect(n_0, v.height, v.width, v.x); }
        public static Rect nhwy(this Rect v, float n_0 = 0) { return new Rect(n_0, v.height, v.width, v.y); }
        public static Rect nhww(this Rect v, float n_0 = 0) { return new Rect(n_0, v.height, v.width, v.width); }
        public static Rect nhwh(this Rect v, float n_0 = 0) { return new Rect(n_0, v.height, v.width, v.height); }
        public static Rect nhwn(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(n_0, v.height, v.width, n_1); }
        public static Rect nhhx(this Rect v, float n_0 = 0) { return new Rect(n_0, v.height, v.height, v.x); }
        public static Rect nhhy(this Rect v, float n_0 = 0) { return new Rect(n_0, v.height, v.height, v.y); }
        public static Rect nhhw(this Rect v, float n_0 = 0) { return new Rect(n_0, v.height, v.height, v.width); }
        public static Rect nhhh(this Rect v, float n_0 = 0) { return new Rect(n_0, v.height, v.height, v.height); }
        public static Rect nhhn(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(n_0, v.height, v.height, n_1); }
        public static Rect nhnx(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(n_0, v.height, n_1, v.x); }
        public static Rect nhny(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(n_0, v.height, n_1, v.y); }
        public static Rect nhnw(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(n_0, v.height, n_1, v.width); }
        public static Rect nhnh(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(n_0, v.height, n_1, v.height); }
        public static Rect nhnn(this Rect v, float n_0 = 0, float n_1 = 0, float n_2 = 0) { return new Rect(n_0, v.height, n_1, n_2); }
        public static Rect nnxx(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(n_0, n_1, v.x, v.x); }
        public static Rect nnxy(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(n_0, n_1, v.x, v.y); }
        public static Rect nnxw(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(n_0, n_1, v.x, v.width); }
        public static Rect nnxh(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(n_0, n_1, v.x, v.height); }
        public static Rect nnxn(this Rect v, float n_0 = 0, float n_1 = 0, float n_2 = 0) { return new Rect(n_0, n_1, v.x, n_2); }
        public static Rect nnyx(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(n_0, n_1, v.y, v.x); }
        public static Rect nnyy(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(n_0, n_1, v.y, v.y); }
        public static Rect nnyw(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(n_0, n_1, v.y, v.width); }
        public static Rect nnyh(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(n_0, n_1, v.y, v.height); }
        public static Rect nnyn(this Rect v, float n_0 = 0, float n_1 = 0, float n_2 = 0) { return new Rect(n_0, n_1, v.y, n_2); }
        public static Rect nnwx(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(n_0, n_1, v.width, v.x); }
        public static Rect nnwy(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(n_0, n_1, v.width, v.y); }
        public static Rect nnww(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(n_0, n_1, v.width, v.width); }
        public static Rect nnwh(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(n_0, n_1, v.width, v.height); }
        public static Rect nnwn(this Rect v, float n_0 = 0, float n_1 = 0, float n_2 = 0) { return new Rect(n_0, n_1, v.width, n_2); }
        public static Rect nnhx(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(n_0, n_1, v.height, v.x); }
        public static Rect nnhy(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(n_0, n_1, v.height, v.y); }
        public static Rect nnhw(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(n_0, n_1, v.height, v.width); }
        public static Rect nnhh(this Rect v, float n_0 = 0, float n_1 = 0) { return new Rect(n_0, n_1, v.height, v.height); }
        public static Rect nnhn(this Rect v, float n_0 = 0, float n_1 = 0, float n_2 = 0) { return new Rect(n_0, n_1, v.height, n_2); }
        public static Rect nnnx(this Rect v, float n_0 = 0, float n_1 = 0, float n_2 = 0) { return new Rect(n_0, n_1, n_2, v.x); }
        public static Rect nnny(this Rect v, float n_0 = 0, float n_1 = 0, float n_2 = 0) { return new Rect(n_0, n_1, n_2, v.y); }
        public static Rect nnnw(this Rect v, float n_0 = 0, float n_1 = 0, float n_2 = 0) { return new Rect(n_0, n_1, n_2, v.width); }
        public static Rect nnnh(this Rect v, float n_0 = 0, float n_1 = 0, float n_2 = 0) { return new Rect(n_0, n_1, n_2, v.height); }
        #endregion Swizzle Rect
        
    }
}




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
    public static partial class RectExtensions
    {
        
        //--
        #region Swizzle Rect
        public static Rect xx(this Rect v) { return new Rect(vec2(v.x, v.x), v.size); }
        public static Rect xy(this Rect v) { return new Rect(vec2(v.x, v.y), v.size); }
        public static Rect xn(this Rect v, float n_0 = 0) { return new Rect(vec2(v.x, n_0), v.size); }
        public static Rect yx(this Rect v) { return new Rect(vec2(v.y, v.x), v.size); }
        public static Rect yy(this Rect v) { return new Rect(vec2(v.y, v.y), v.size); }
        public static Rect yn(this Rect v, float n_0 = 0) { return new Rect(vec2(v.y, n_0), v.size); }
        public static Rect nx(this Rect v, float n_0 = 0) { return new Rect(vec2(n_0, v.x), v.size); }
        public static Rect ny(this Rect v, float n_0 = 0) { return new Rect(vec2(n_0, v.y), v.size); }
        #endregion Swizzle Rect
        
    }
}




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
    public static partial class RectExtensions
    {
        
        //--
        #region Swizzle Rect
        public static Rect ww(this Rect v) { return new Rect(v.position, vec2(v.width, v.width)); }
        public static Rect wh(this Rect v) { return new Rect(v.position, vec2(v.width, v.height)); }
        public static Rect wn(this Rect v, float n_0 = 0) { return new Rect(v.position, vec2(v.width, n_0)); }
        public static Rect hw(this Rect v) { return new Rect(v.position, vec2(v.height, v.width)); }
        public static Rect hh(this Rect v) { return new Rect(v.position, vec2(v.height, v.height)); }
        public static Rect hn(this Rect v, float n_0 = 0) { return new Rect(v.position, vec2(v.height, n_0)); }
        public static Rect nw(this Rect v, float n_0 = 0) { return new Rect(v.position, vec2(n_0, v.width)); }
        public static Rect nh(this Rect v, float n_0 = 0) { return new Rect(v.position, vec2(n_0, v.height)); }
        #endregion Swizzle Rect
        
    }
}
