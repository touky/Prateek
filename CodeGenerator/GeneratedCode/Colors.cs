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
    public static partial class ColorExt
    {
        //---------------------------------------------------------------------
        #region Swizzle Color
        public static Color rrrr(this Color v) { return new Color(v.r, v.r, v.r, v.r); }
        public static Color rrrg(this Color v) { return new Color(v.r, v.r, v.r, v.g); }
        public static Color rrrb(this Color v) { return new Color(v.r, v.r, v.r, v.b); }
        public static Color rrra(this Color v) { return new Color(v.r, v.r, v.r, v.a); }
        public static Color rrrn(this Color v, float n_0 = 0) { return new Color(v.r, v.r, v.r, n_0); }
        public static Color rrgr(this Color v) { return new Color(v.r, v.r, v.g, v.r); }
        public static Color rrgg(this Color v) { return new Color(v.r, v.r, v.g, v.g); }
        public static Color rrgb(this Color v) { return new Color(v.r, v.r, v.g, v.b); }
        public static Color rrga(this Color v) { return new Color(v.r, v.r, v.g, v.a); }
        public static Color rrgn(this Color v, float n_0 = 0) { return new Color(v.r, v.r, v.g, n_0); }
        public static Color rrbr(this Color v) { return new Color(v.r, v.r, v.b, v.r); }
        public static Color rrbg(this Color v) { return new Color(v.r, v.r, v.b, v.g); }
        public static Color rrbb(this Color v) { return new Color(v.r, v.r, v.b, v.b); }
        public static Color rrba(this Color v) { return new Color(v.r, v.r, v.b, v.a); }
        public static Color rrbn(this Color v, float n_0 = 0) { return new Color(v.r, v.r, v.b, n_0); }
        public static Color rrar(this Color v) { return new Color(v.r, v.r, v.a, v.r); }
        public static Color rrag(this Color v) { return new Color(v.r, v.r, v.a, v.g); }
        public static Color rrab(this Color v) { return new Color(v.r, v.r, v.a, v.b); }
        public static Color rraa(this Color v) { return new Color(v.r, v.r, v.a, v.a); }
        public static Color rran(this Color v, float n_0 = 0) { return new Color(v.r, v.r, v.a, n_0); }
        public static Color rrnr(this Color v, float n_0 = 0) { return new Color(v.r, v.r, n_0, v.r); }
        public static Color rrng(this Color v, float n_0 = 0) { return new Color(v.r, v.r, n_0, v.g); }
        public static Color rrnb(this Color v, float n_0 = 0) { return new Color(v.r, v.r, n_0, v.b); }
        public static Color rrna(this Color v, float n_0 = 0) { return new Color(v.r, v.r, n_0, v.a); }
        public static Color rrnn(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(v.r, v.r, n_0, n_1); }
        public static Color rgrr(this Color v) { return new Color(v.r, v.g, v.r, v.r); }
        public static Color rgrg(this Color v) { return new Color(v.r, v.g, v.r, v.g); }
        public static Color rgrb(this Color v) { return new Color(v.r, v.g, v.r, v.b); }
        public static Color rgra(this Color v) { return new Color(v.r, v.g, v.r, v.a); }
        public static Color rgrn(this Color v, float n_0 = 0) { return new Color(v.r, v.g, v.r, n_0); }
        public static Color rggr(this Color v) { return new Color(v.r, v.g, v.g, v.r); }
        public static Color rggg(this Color v) { return new Color(v.r, v.g, v.g, v.g); }
        public static Color rggb(this Color v) { return new Color(v.r, v.g, v.g, v.b); }
        public static Color rgga(this Color v) { return new Color(v.r, v.g, v.g, v.a); }
        public static Color rggn(this Color v, float n_0 = 0) { return new Color(v.r, v.g, v.g, n_0); }
        public static Color rgbr(this Color v) { return new Color(v.r, v.g, v.b, v.r); }
        public static Color rgbg(this Color v) { return new Color(v.r, v.g, v.b, v.g); }
        public static Color rgbb(this Color v) { return new Color(v.r, v.g, v.b, v.b); }
        public static Color rgba(this Color v) { return new Color(v.r, v.g, v.b, v.a); }
        public static Color rgbn(this Color v, float n_0 = 0) { return new Color(v.r, v.g, v.b, n_0); }
        public static Color rgar(this Color v) { return new Color(v.r, v.g, v.a, v.r); }
        public static Color rgag(this Color v) { return new Color(v.r, v.g, v.a, v.g); }
        public static Color rgab(this Color v) { return new Color(v.r, v.g, v.a, v.b); }
        public static Color rgaa(this Color v) { return new Color(v.r, v.g, v.a, v.a); }
        public static Color rgan(this Color v, float n_0 = 0) { return new Color(v.r, v.g, v.a, n_0); }
        public static Color rgnr(this Color v, float n_0 = 0) { return new Color(v.r, v.g, n_0, v.r); }
        public static Color rgng(this Color v, float n_0 = 0) { return new Color(v.r, v.g, n_0, v.g); }
        public static Color rgnb(this Color v, float n_0 = 0) { return new Color(v.r, v.g, n_0, v.b); }
        public static Color rgna(this Color v, float n_0 = 0) { return new Color(v.r, v.g, n_0, v.a); }
        public static Color rgnn(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(v.r, v.g, n_0, n_1); }
        public static Color rbrr(this Color v) { return new Color(v.r, v.b, v.r, v.r); }
        public static Color rbrg(this Color v) { return new Color(v.r, v.b, v.r, v.g); }
        public static Color rbrb(this Color v) { return new Color(v.r, v.b, v.r, v.b); }
        public static Color rbra(this Color v) { return new Color(v.r, v.b, v.r, v.a); }
        public static Color rbrn(this Color v, float n_0 = 0) { return new Color(v.r, v.b, v.r, n_0); }
        public static Color rbgr(this Color v) { return new Color(v.r, v.b, v.g, v.r); }
        public static Color rbgg(this Color v) { return new Color(v.r, v.b, v.g, v.g); }
        public static Color rbgb(this Color v) { return new Color(v.r, v.b, v.g, v.b); }
        public static Color rbga(this Color v) { return new Color(v.r, v.b, v.g, v.a); }
        public static Color rbgn(this Color v, float n_0 = 0) { return new Color(v.r, v.b, v.g, n_0); }
        public static Color rbbr(this Color v) { return new Color(v.r, v.b, v.b, v.r); }
        public static Color rbbg(this Color v) { return new Color(v.r, v.b, v.b, v.g); }
        public static Color rbbb(this Color v) { return new Color(v.r, v.b, v.b, v.b); }
        public static Color rbba(this Color v) { return new Color(v.r, v.b, v.b, v.a); }
        public static Color rbbn(this Color v, float n_0 = 0) { return new Color(v.r, v.b, v.b, n_0); }
        public static Color rbar(this Color v) { return new Color(v.r, v.b, v.a, v.r); }
        public static Color rbag(this Color v) { return new Color(v.r, v.b, v.a, v.g); }
        public static Color rbab(this Color v) { return new Color(v.r, v.b, v.a, v.b); }
        public static Color rbaa(this Color v) { return new Color(v.r, v.b, v.a, v.a); }
        public static Color rban(this Color v, float n_0 = 0) { return new Color(v.r, v.b, v.a, n_0); }
        public static Color rbnr(this Color v, float n_0 = 0) { return new Color(v.r, v.b, n_0, v.r); }
        public static Color rbng(this Color v, float n_0 = 0) { return new Color(v.r, v.b, n_0, v.g); }
        public static Color rbnb(this Color v, float n_0 = 0) { return new Color(v.r, v.b, n_0, v.b); }
        public static Color rbna(this Color v, float n_0 = 0) { return new Color(v.r, v.b, n_0, v.a); }
        public static Color rbnn(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(v.r, v.b, n_0, n_1); }
        public static Color rarr(this Color v) { return new Color(v.r, v.a, v.r, v.r); }
        public static Color rarg(this Color v) { return new Color(v.r, v.a, v.r, v.g); }
        public static Color rarb(this Color v) { return new Color(v.r, v.a, v.r, v.b); }
        public static Color rara(this Color v) { return new Color(v.r, v.a, v.r, v.a); }
        public static Color rarn(this Color v, float n_0 = 0) { return new Color(v.r, v.a, v.r, n_0); }
        public static Color ragr(this Color v) { return new Color(v.r, v.a, v.g, v.r); }
        public static Color ragg(this Color v) { return new Color(v.r, v.a, v.g, v.g); }
        public static Color ragb(this Color v) { return new Color(v.r, v.a, v.g, v.b); }
        public static Color raga(this Color v) { return new Color(v.r, v.a, v.g, v.a); }
        public static Color ragn(this Color v, float n_0 = 0) { return new Color(v.r, v.a, v.g, n_0); }
        public static Color rabr(this Color v) { return new Color(v.r, v.a, v.b, v.r); }
        public static Color rabg(this Color v) { return new Color(v.r, v.a, v.b, v.g); }
        public static Color rabb(this Color v) { return new Color(v.r, v.a, v.b, v.b); }
        public static Color raba(this Color v) { return new Color(v.r, v.a, v.b, v.a); }
        public static Color rabn(this Color v, float n_0 = 0) { return new Color(v.r, v.a, v.b, n_0); }
        public static Color raar(this Color v) { return new Color(v.r, v.a, v.a, v.r); }
        public static Color raag(this Color v) { return new Color(v.r, v.a, v.a, v.g); }
        public static Color raab(this Color v) { return new Color(v.r, v.a, v.a, v.b); }
        public static Color raaa(this Color v) { return new Color(v.r, v.a, v.a, v.a); }
        public static Color raan(this Color v, float n_0 = 0) { return new Color(v.r, v.a, v.a, n_0); }
        public static Color ranr(this Color v, float n_0 = 0) { return new Color(v.r, v.a, n_0, v.r); }
        public static Color rang(this Color v, float n_0 = 0) { return new Color(v.r, v.a, n_0, v.g); }
        public static Color ranb(this Color v, float n_0 = 0) { return new Color(v.r, v.a, n_0, v.b); }
        public static Color rana(this Color v, float n_0 = 0) { return new Color(v.r, v.a, n_0, v.a); }
        public static Color rann(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(v.r, v.a, n_0, n_1); }
        public static Color rnrr(this Color v, float n_0 = 0) { return new Color(v.r, n_0, v.r, v.r); }
        public static Color rnrg(this Color v, float n_0 = 0) { return new Color(v.r, n_0, v.r, v.g); }
        public static Color rnrb(this Color v, float n_0 = 0) { return new Color(v.r, n_0, v.r, v.b); }
        public static Color rnra(this Color v, float n_0 = 0) { return new Color(v.r, n_0, v.r, v.a); }
        public static Color rnrn(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(v.r, n_0, v.r, n_1); }
        public static Color rngr(this Color v, float n_0 = 0) { return new Color(v.r, n_0, v.g, v.r); }
        public static Color rngg(this Color v, float n_0 = 0) { return new Color(v.r, n_0, v.g, v.g); }
        public static Color rngb(this Color v, float n_0 = 0) { return new Color(v.r, n_0, v.g, v.b); }
        public static Color rnga(this Color v, float n_0 = 0) { return new Color(v.r, n_0, v.g, v.a); }
        public static Color rngn(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(v.r, n_0, v.g, n_1); }
        public static Color rnbr(this Color v, float n_0 = 0) { return new Color(v.r, n_0, v.b, v.r); }
        public static Color rnbg(this Color v, float n_0 = 0) { return new Color(v.r, n_0, v.b, v.g); }
        public static Color rnbb(this Color v, float n_0 = 0) { return new Color(v.r, n_0, v.b, v.b); }
        public static Color rnba(this Color v, float n_0 = 0) { return new Color(v.r, n_0, v.b, v.a); }
        public static Color rnbn(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(v.r, n_0, v.b, n_1); }
        public static Color rnar(this Color v, float n_0 = 0) { return new Color(v.r, n_0, v.a, v.r); }
        public static Color rnag(this Color v, float n_0 = 0) { return new Color(v.r, n_0, v.a, v.g); }
        public static Color rnab(this Color v, float n_0 = 0) { return new Color(v.r, n_0, v.a, v.b); }
        public static Color rnaa(this Color v, float n_0 = 0) { return new Color(v.r, n_0, v.a, v.a); }
        public static Color rnan(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(v.r, n_0, v.a, n_1); }
        public static Color rnnr(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(v.r, n_0, n_1, v.r); }
        public static Color rnng(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(v.r, n_0, n_1, v.g); }
        public static Color rnnb(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(v.r, n_0, n_1, v.b); }
        public static Color rnna(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(v.r, n_0, n_1, v.a); }
        public static Color rnnn(this Color v, float n_0 = 0, float n_1 = 0, float n_2 = 0) { return new Color(v.r, n_0, n_1, n_2); }
        public static Color grrr(this Color v) { return new Color(v.g, v.r, v.r, v.r); }
        public static Color grrg(this Color v) { return new Color(v.g, v.r, v.r, v.g); }
        public static Color grrb(this Color v) { return new Color(v.g, v.r, v.r, v.b); }
        public static Color grra(this Color v) { return new Color(v.g, v.r, v.r, v.a); }
        public static Color grrn(this Color v, float n_0 = 0) { return new Color(v.g, v.r, v.r, n_0); }
        public static Color grgr(this Color v) { return new Color(v.g, v.r, v.g, v.r); }
        public static Color grgg(this Color v) { return new Color(v.g, v.r, v.g, v.g); }
        public static Color grgb(this Color v) { return new Color(v.g, v.r, v.g, v.b); }
        public static Color grga(this Color v) { return new Color(v.g, v.r, v.g, v.a); }
        public static Color grgn(this Color v, float n_0 = 0) { return new Color(v.g, v.r, v.g, n_0); }
        public static Color grbr(this Color v) { return new Color(v.g, v.r, v.b, v.r); }
        public static Color grbg(this Color v) { return new Color(v.g, v.r, v.b, v.g); }
        public static Color grbb(this Color v) { return new Color(v.g, v.r, v.b, v.b); }
        public static Color grba(this Color v) { return new Color(v.g, v.r, v.b, v.a); }
        public static Color grbn(this Color v, float n_0 = 0) { return new Color(v.g, v.r, v.b, n_0); }
        public static Color grar(this Color v) { return new Color(v.g, v.r, v.a, v.r); }
        public static Color grag(this Color v) { return new Color(v.g, v.r, v.a, v.g); }
        public static Color grab(this Color v) { return new Color(v.g, v.r, v.a, v.b); }
        public static Color graa(this Color v) { return new Color(v.g, v.r, v.a, v.a); }
        public static Color gran(this Color v, float n_0 = 0) { return new Color(v.g, v.r, v.a, n_0); }
        public static Color grnr(this Color v, float n_0 = 0) { return new Color(v.g, v.r, n_0, v.r); }
        public static Color grng(this Color v, float n_0 = 0) { return new Color(v.g, v.r, n_0, v.g); }
        public static Color grnb(this Color v, float n_0 = 0) { return new Color(v.g, v.r, n_0, v.b); }
        public static Color grna(this Color v, float n_0 = 0) { return new Color(v.g, v.r, n_0, v.a); }
        public static Color grnn(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(v.g, v.r, n_0, n_1); }
        public static Color ggrr(this Color v) { return new Color(v.g, v.g, v.r, v.r); }
        public static Color ggrg(this Color v) { return new Color(v.g, v.g, v.r, v.g); }
        public static Color ggrb(this Color v) { return new Color(v.g, v.g, v.r, v.b); }
        public static Color ggra(this Color v) { return new Color(v.g, v.g, v.r, v.a); }
        public static Color ggrn(this Color v, float n_0 = 0) { return new Color(v.g, v.g, v.r, n_0); }
        public static Color gggr(this Color v) { return new Color(v.g, v.g, v.g, v.r); }
        public static Color gggg(this Color v) { return new Color(v.g, v.g, v.g, v.g); }
        public static Color gggb(this Color v) { return new Color(v.g, v.g, v.g, v.b); }
        public static Color ggga(this Color v) { return new Color(v.g, v.g, v.g, v.a); }
        public static Color gggn(this Color v, float n_0 = 0) { return new Color(v.g, v.g, v.g, n_0); }
        public static Color ggbr(this Color v) { return new Color(v.g, v.g, v.b, v.r); }
        public static Color ggbg(this Color v) { return new Color(v.g, v.g, v.b, v.g); }
        public static Color ggbb(this Color v) { return new Color(v.g, v.g, v.b, v.b); }
        public static Color ggba(this Color v) { return new Color(v.g, v.g, v.b, v.a); }
        public static Color ggbn(this Color v, float n_0 = 0) { return new Color(v.g, v.g, v.b, n_0); }
        public static Color ggar(this Color v) { return new Color(v.g, v.g, v.a, v.r); }
        public static Color ggag(this Color v) { return new Color(v.g, v.g, v.a, v.g); }
        public static Color ggab(this Color v) { return new Color(v.g, v.g, v.a, v.b); }
        public static Color ggaa(this Color v) { return new Color(v.g, v.g, v.a, v.a); }
        public static Color ggan(this Color v, float n_0 = 0) { return new Color(v.g, v.g, v.a, n_0); }
        public static Color ggnr(this Color v, float n_0 = 0) { return new Color(v.g, v.g, n_0, v.r); }
        public static Color ggng(this Color v, float n_0 = 0) { return new Color(v.g, v.g, n_0, v.g); }
        public static Color ggnb(this Color v, float n_0 = 0) { return new Color(v.g, v.g, n_0, v.b); }
        public static Color ggna(this Color v, float n_0 = 0) { return new Color(v.g, v.g, n_0, v.a); }
        public static Color ggnn(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(v.g, v.g, n_0, n_1); }
        public static Color gbrr(this Color v) { return new Color(v.g, v.b, v.r, v.r); }
        public static Color gbrg(this Color v) { return new Color(v.g, v.b, v.r, v.g); }
        public static Color gbrb(this Color v) { return new Color(v.g, v.b, v.r, v.b); }
        public static Color gbra(this Color v) { return new Color(v.g, v.b, v.r, v.a); }
        public static Color gbrn(this Color v, float n_0 = 0) { return new Color(v.g, v.b, v.r, n_0); }
        public static Color gbgr(this Color v) { return new Color(v.g, v.b, v.g, v.r); }
        public static Color gbgg(this Color v) { return new Color(v.g, v.b, v.g, v.g); }
        public static Color gbgb(this Color v) { return new Color(v.g, v.b, v.g, v.b); }
        public static Color gbga(this Color v) { return new Color(v.g, v.b, v.g, v.a); }
        public static Color gbgn(this Color v, float n_0 = 0) { return new Color(v.g, v.b, v.g, n_0); }
        public static Color gbbr(this Color v) { return new Color(v.g, v.b, v.b, v.r); }
        public static Color gbbg(this Color v) { return new Color(v.g, v.b, v.b, v.g); }
        public static Color gbbb(this Color v) { return new Color(v.g, v.b, v.b, v.b); }
        public static Color gbba(this Color v) { return new Color(v.g, v.b, v.b, v.a); }
        public static Color gbbn(this Color v, float n_0 = 0) { return new Color(v.g, v.b, v.b, n_0); }
        public static Color gbar(this Color v) { return new Color(v.g, v.b, v.a, v.r); }
        public static Color gbag(this Color v) { return new Color(v.g, v.b, v.a, v.g); }
        public static Color gbab(this Color v) { return new Color(v.g, v.b, v.a, v.b); }
        public static Color gbaa(this Color v) { return new Color(v.g, v.b, v.a, v.a); }
        public static Color gban(this Color v, float n_0 = 0) { return new Color(v.g, v.b, v.a, n_0); }
        public static Color gbnr(this Color v, float n_0 = 0) { return new Color(v.g, v.b, n_0, v.r); }
        public static Color gbng(this Color v, float n_0 = 0) { return new Color(v.g, v.b, n_0, v.g); }
        public static Color gbnb(this Color v, float n_0 = 0) { return new Color(v.g, v.b, n_0, v.b); }
        public static Color gbna(this Color v, float n_0 = 0) { return new Color(v.g, v.b, n_0, v.a); }
        public static Color gbnn(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(v.g, v.b, n_0, n_1); }
        public static Color garr(this Color v) { return new Color(v.g, v.a, v.r, v.r); }
        public static Color garg(this Color v) { return new Color(v.g, v.a, v.r, v.g); }
        public static Color garb(this Color v) { return new Color(v.g, v.a, v.r, v.b); }
        public static Color gara(this Color v) { return new Color(v.g, v.a, v.r, v.a); }
        public static Color garn(this Color v, float n_0 = 0) { return new Color(v.g, v.a, v.r, n_0); }
        public static Color gagr(this Color v) { return new Color(v.g, v.a, v.g, v.r); }
        public static Color gagg(this Color v) { return new Color(v.g, v.a, v.g, v.g); }
        public static Color gagb(this Color v) { return new Color(v.g, v.a, v.g, v.b); }
        public static Color gaga(this Color v) { return new Color(v.g, v.a, v.g, v.a); }
        public static Color gagn(this Color v, float n_0 = 0) { return new Color(v.g, v.a, v.g, n_0); }
        public static Color gabr(this Color v) { return new Color(v.g, v.a, v.b, v.r); }
        public static Color gabg(this Color v) { return new Color(v.g, v.a, v.b, v.g); }
        public static Color gabb(this Color v) { return new Color(v.g, v.a, v.b, v.b); }
        public static Color gaba(this Color v) { return new Color(v.g, v.a, v.b, v.a); }
        public static Color gabn(this Color v, float n_0 = 0) { return new Color(v.g, v.a, v.b, n_0); }
        public static Color gaar(this Color v) { return new Color(v.g, v.a, v.a, v.r); }
        public static Color gaag(this Color v) { return new Color(v.g, v.a, v.a, v.g); }
        public static Color gaab(this Color v) { return new Color(v.g, v.a, v.a, v.b); }
        public static Color gaaa(this Color v) { return new Color(v.g, v.a, v.a, v.a); }
        public static Color gaan(this Color v, float n_0 = 0) { return new Color(v.g, v.a, v.a, n_0); }
        public static Color ganr(this Color v, float n_0 = 0) { return new Color(v.g, v.a, n_0, v.r); }
        public static Color gang(this Color v, float n_0 = 0) { return new Color(v.g, v.a, n_0, v.g); }
        public static Color ganb(this Color v, float n_0 = 0) { return new Color(v.g, v.a, n_0, v.b); }
        public static Color gana(this Color v, float n_0 = 0) { return new Color(v.g, v.a, n_0, v.a); }
        public static Color gann(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(v.g, v.a, n_0, n_1); }
        public static Color gnrr(this Color v, float n_0 = 0) { return new Color(v.g, n_0, v.r, v.r); }
        public static Color gnrg(this Color v, float n_0 = 0) { return new Color(v.g, n_0, v.r, v.g); }
        public static Color gnrb(this Color v, float n_0 = 0) { return new Color(v.g, n_0, v.r, v.b); }
        public static Color gnra(this Color v, float n_0 = 0) { return new Color(v.g, n_0, v.r, v.a); }
        public static Color gnrn(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(v.g, n_0, v.r, n_1); }
        public static Color gngr(this Color v, float n_0 = 0) { return new Color(v.g, n_0, v.g, v.r); }
        public static Color gngg(this Color v, float n_0 = 0) { return new Color(v.g, n_0, v.g, v.g); }
        public static Color gngb(this Color v, float n_0 = 0) { return new Color(v.g, n_0, v.g, v.b); }
        public static Color gnga(this Color v, float n_0 = 0) { return new Color(v.g, n_0, v.g, v.a); }
        public static Color gngn(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(v.g, n_0, v.g, n_1); }
        public static Color gnbr(this Color v, float n_0 = 0) { return new Color(v.g, n_0, v.b, v.r); }
        public static Color gnbg(this Color v, float n_0 = 0) { return new Color(v.g, n_0, v.b, v.g); }
        public static Color gnbb(this Color v, float n_0 = 0) { return new Color(v.g, n_0, v.b, v.b); }
        public static Color gnba(this Color v, float n_0 = 0) { return new Color(v.g, n_0, v.b, v.a); }
        public static Color gnbn(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(v.g, n_0, v.b, n_1); }
        public static Color gnar(this Color v, float n_0 = 0) { return new Color(v.g, n_0, v.a, v.r); }
        public static Color gnag(this Color v, float n_0 = 0) { return new Color(v.g, n_0, v.a, v.g); }
        public static Color gnab(this Color v, float n_0 = 0) { return new Color(v.g, n_0, v.a, v.b); }
        public static Color gnaa(this Color v, float n_0 = 0) { return new Color(v.g, n_0, v.a, v.a); }
        public static Color gnan(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(v.g, n_0, v.a, n_1); }
        public static Color gnnr(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(v.g, n_0, n_1, v.r); }
        public static Color gnng(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(v.g, n_0, n_1, v.g); }
        public static Color gnnb(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(v.g, n_0, n_1, v.b); }
        public static Color gnna(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(v.g, n_0, n_1, v.a); }
        public static Color gnnn(this Color v, float n_0 = 0, float n_1 = 0, float n_2 = 0) { return new Color(v.g, n_0, n_1, n_2); }
        public static Color brrr(this Color v) { return new Color(v.b, v.r, v.r, v.r); }
        public static Color brrg(this Color v) { return new Color(v.b, v.r, v.r, v.g); }
        public static Color brrb(this Color v) { return new Color(v.b, v.r, v.r, v.b); }
        public static Color brra(this Color v) { return new Color(v.b, v.r, v.r, v.a); }
        public static Color brrn(this Color v, float n_0 = 0) { return new Color(v.b, v.r, v.r, n_0); }
        public static Color brgr(this Color v) { return new Color(v.b, v.r, v.g, v.r); }
        public static Color brgg(this Color v) { return new Color(v.b, v.r, v.g, v.g); }
        public static Color brgb(this Color v) { return new Color(v.b, v.r, v.g, v.b); }
        public static Color brga(this Color v) { return new Color(v.b, v.r, v.g, v.a); }
        public static Color brgn(this Color v, float n_0 = 0) { return new Color(v.b, v.r, v.g, n_0); }
        public static Color brbr(this Color v) { return new Color(v.b, v.r, v.b, v.r); }
        public static Color brbg(this Color v) { return new Color(v.b, v.r, v.b, v.g); }
        public static Color brbb(this Color v) { return new Color(v.b, v.r, v.b, v.b); }
        public static Color brba(this Color v) { return new Color(v.b, v.r, v.b, v.a); }
        public static Color brbn(this Color v, float n_0 = 0) { return new Color(v.b, v.r, v.b, n_0); }
        public static Color brar(this Color v) { return new Color(v.b, v.r, v.a, v.r); }
        public static Color brag(this Color v) { return new Color(v.b, v.r, v.a, v.g); }
        public static Color brab(this Color v) { return new Color(v.b, v.r, v.a, v.b); }
        public static Color braa(this Color v) { return new Color(v.b, v.r, v.a, v.a); }
        public static Color bran(this Color v, float n_0 = 0) { return new Color(v.b, v.r, v.a, n_0); }
        public static Color brnr(this Color v, float n_0 = 0) { return new Color(v.b, v.r, n_0, v.r); }
        public static Color brng(this Color v, float n_0 = 0) { return new Color(v.b, v.r, n_0, v.g); }
        public static Color brnb(this Color v, float n_0 = 0) { return new Color(v.b, v.r, n_0, v.b); }
        public static Color brna(this Color v, float n_0 = 0) { return new Color(v.b, v.r, n_0, v.a); }
        public static Color brnn(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(v.b, v.r, n_0, n_1); }
        public static Color bgrr(this Color v) { return new Color(v.b, v.g, v.r, v.r); }
        public static Color bgrg(this Color v) { return new Color(v.b, v.g, v.r, v.g); }
        public static Color bgrb(this Color v) { return new Color(v.b, v.g, v.r, v.b); }
        public static Color bgra(this Color v) { return new Color(v.b, v.g, v.r, v.a); }
        public static Color bgrn(this Color v, float n_0 = 0) { return new Color(v.b, v.g, v.r, n_0); }
        public static Color bggr(this Color v) { return new Color(v.b, v.g, v.g, v.r); }
        public static Color bggg(this Color v) { return new Color(v.b, v.g, v.g, v.g); }
        public static Color bggb(this Color v) { return new Color(v.b, v.g, v.g, v.b); }
        public static Color bgga(this Color v) { return new Color(v.b, v.g, v.g, v.a); }
        public static Color bggn(this Color v, float n_0 = 0) { return new Color(v.b, v.g, v.g, n_0); }
        public static Color bgbr(this Color v) { return new Color(v.b, v.g, v.b, v.r); }
        public static Color bgbg(this Color v) { return new Color(v.b, v.g, v.b, v.g); }
        public static Color bgbb(this Color v) { return new Color(v.b, v.g, v.b, v.b); }
        public static Color bgba(this Color v) { return new Color(v.b, v.g, v.b, v.a); }
        public static Color bgbn(this Color v, float n_0 = 0) { return new Color(v.b, v.g, v.b, n_0); }
        public static Color bgar(this Color v) { return new Color(v.b, v.g, v.a, v.r); }
        public static Color bgag(this Color v) { return new Color(v.b, v.g, v.a, v.g); }
        public static Color bgab(this Color v) { return new Color(v.b, v.g, v.a, v.b); }
        public static Color bgaa(this Color v) { return new Color(v.b, v.g, v.a, v.a); }
        public static Color bgan(this Color v, float n_0 = 0) { return new Color(v.b, v.g, v.a, n_0); }
        public static Color bgnr(this Color v, float n_0 = 0) { return new Color(v.b, v.g, n_0, v.r); }
        public static Color bgng(this Color v, float n_0 = 0) { return new Color(v.b, v.g, n_0, v.g); }
        public static Color bgnb(this Color v, float n_0 = 0) { return new Color(v.b, v.g, n_0, v.b); }
        public static Color bgna(this Color v, float n_0 = 0) { return new Color(v.b, v.g, n_0, v.a); }
        public static Color bgnn(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(v.b, v.g, n_0, n_1); }
        public static Color bbrr(this Color v) { return new Color(v.b, v.b, v.r, v.r); }
        public static Color bbrg(this Color v) { return new Color(v.b, v.b, v.r, v.g); }
        public static Color bbrb(this Color v) { return new Color(v.b, v.b, v.r, v.b); }
        public static Color bbra(this Color v) { return new Color(v.b, v.b, v.r, v.a); }
        public static Color bbrn(this Color v, float n_0 = 0) { return new Color(v.b, v.b, v.r, n_0); }
        public static Color bbgr(this Color v) { return new Color(v.b, v.b, v.g, v.r); }
        public static Color bbgg(this Color v) { return new Color(v.b, v.b, v.g, v.g); }
        public static Color bbgb(this Color v) { return new Color(v.b, v.b, v.g, v.b); }
        public static Color bbga(this Color v) { return new Color(v.b, v.b, v.g, v.a); }
        public static Color bbgn(this Color v, float n_0 = 0) { return new Color(v.b, v.b, v.g, n_0); }
        public static Color bbbr(this Color v) { return new Color(v.b, v.b, v.b, v.r); }
        public static Color bbbg(this Color v) { return new Color(v.b, v.b, v.b, v.g); }
        public static Color bbbb(this Color v) { return new Color(v.b, v.b, v.b, v.b); }
        public static Color bbba(this Color v) { return new Color(v.b, v.b, v.b, v.a); }
        public static Color bbbn(this Color v, float n_0 = 0) { return new Color(v.b, v.b, v.b, n_0); }
        public static Color bbar(this Color v) { return new Color(v.b, v.b, v.a, v.r); }
        public static Color bbag(this Color v) { return new Color(v.b, v.b, v.a, v.g); }
        public static Color bbab(this Color v) { return new Color(v.b, v.b, v.a, v.b); }
        public static Color bbaa(this Color v) { return new Color(v.b, v.b, v.a, v.a); }
        public static Color bban(this Color v, float n_0 = 0) { return new Color(v.b, v.b, v.a, n_0); }
        public static Color bbnr(this Color v, float n_0 = 0) { return new Color(v.b, v.b, n_0, v.r); }
        public static Color bbng(this Color v, float n_0 = 0) { return new Color(v.b, v.b, n_0, v.g); }
        public static Color bbnb(this Color v, float n_0 = 0) { return new Color(v.b, v.b, n_0, v.b); }
        public static Color bbna(this Color v, float n_0 = 0) { return new Color(v.b, v.b, n_0, v.a); }
        public static Color bbnn(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(v.b, v.b, n_0, n_1); }
        public static Color barr(this Color v) { return new Color(v.b, v.a, v.r, v.r); }
        public static Color barg(this Color v) { return new Color(v.b, v.a, v.r, v.g); }
        public static Color barb(this Color v) { return new Color(v.b, v.a, v.r, v.b); }
        public static Color bara(this Color v) { return new Color(v.b, v.a, v.r, v.a); }
        public static Color barn(this Color v, float n_0 = 0) { return new Color(v.b, v.a, v.r, n_0); }
        public static Color bagr(this Color v) { return new Color(v.b, v.a, v.g, v.r); }
        public static Color bagg(this Color v) { return new Color(v.b, v.a, v.g, v.g); }
        public static Color bagb(this Color v) { return new Color(v.b, v.a, v.g, v.b); }
        public static Color baga(this Color v) { return new Color(v.b, v.a, v.g, v.a); }
        public static Color bagn(this Color v, float n_0 = 0) { return new Color(v.b, v.a, v.g, n_0); }
        public static Color babr(this Color v) { return new Color(v.b, v.a, v.b, v.r); }
        public static Color babg(this Color v) { return new Color(v.b, v.a, v.b, v.g); }
        public static Color babb(this Color v) { return new Color(v.b, v.a, v.b, v.b); }
        public static Color baba(this Color v) { return new Color(v.b, v.a, v.b, v.a); }
        public static Color babn(this Color v, float n_0 = 0) { return new Color(v.b, v.a, v.b, n_0); }
        public static Color baar(this Color v) { return new Color(v.b, v.a, v.a, v.r); }
        public static Color baag(this Color v) { return new Color(v.b, v.a, v.a, v.g); }
        public static Color baab(this Color v) { return new Color(v.b, v.a, v.a, v.b); }
        public static Color baaa(this Color v) { return new Color(v.b, v.a, v.a, v.a); }
        public static Color baan(this Color v, float n_0 = 0) { return new Color(v.b, v.a, v.a, n_0); }
        public static Color banr(this Color v, float n_0 = 0) { return new Color(v.b, v.a, n_0, v.r); }
        public static Color bang(this Color v, float n_0 = 0) { return new Color(v.b, v.a, n_0, v.g); }
        public static Color banb(this Color v, float n_0 = 0) { return new Color(v.b, v.a, n_0, v.b); }
        public static Color bana(this Color v, float n_0 = 0) { return new Color(v.b, v.a, n_0, v.a); }
        public static Color bann(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(v.b, v.a, n_0, n_1); }
        public static Color bnrr(this Color v, float n_0 = 0) { return new Color(v.b, n_0, v.r, v.r); }
        public static Color bnrg(this Color v, float n_0 = 0) { return new Color(v.b, n_0, v.r, v.g); }
        public static Color bnrb(this Color v, float n_0 = 0) { return new Color(v.b, n_0, v.r, v.b); }
        public static Color bnra(this Color v, float n_0 = 0) { return new Color(v.b, n_0, v.r, v.a); }
        public static Color bnrn(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(v.b, n_0, v.r, n_1); }
        public static Color bngr(this Color v, float n_0 = 0) { return new Color(v.b, n_0, v.g, v.r); }
        public static Color bngg(this Color v, float n_0 = 0) { return new Color(v.b, n_0, v.g, v.g); }
        public static Color bngb(this Color v, float n_0 = 0) { return new Color(v.b, n_0, v.g, v.b); }
        public static Color bnga(this Color v, float n_0 = 0) { return new Color(v.b, n_0, v.g, v.a); }
        public static Color bngn(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(v.b, n_0, v.g, n_1); }
        public static Color bnbr(this Color v, float n_0 = 0) { return new Color(v.b, n_0, v.b, v.r); }
        public static Color bnbg(this Color v, float n_0 = 0) { return new Color(v.b, n_0, v.b, v.g); }
        public static Color bnbb(this Color v, float n_0 = 0) { return new Color(v.b, n_0, v.b, v.b); }
        public static Color bnba(this Color v, float n_0 = 0) { return new Color(v.b, n_0, v.b, v.a); }
        public static Color bnbn(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(v.b, n_0, v.b, n_1); }
        public static Color bnar(this Color v, float n_0 = 0) { return new Color(v.b, n_0, v.a, v.r); }
        public static Color bnag(this Color v, float n_0 = 0) { return new Color(v.b, n_0, v.a, v.g); }
        public static Color bnab(this Color v, float n_0 = 0) { return new Color(v.b, n_0, v.a, v.b); }
        public static Color bnaa(this Color v, float n_0 = 0) { return new Color(v.b, n_0, v.a, v.a); }
        public static Color bnan(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(v.b, n_0, v.a, n_1); }
        public static Color bnnr(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(v.b, n_0, n_1, v.r); }
        public static Color bnng(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(v.b, n_0, n_1, v.g); }
        public static Color bnnb(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(v.b, n_0, n_1, v.b); }
        public static Color bnna(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(v.b, n_0, n_1, v.a); }
        public static Color bnnn(this Color v, float n_0 = 0, float n_1 = 0, float n_2 = 0) { return new Color(v.b, n_0, n_1, n_2); }
        public static Color arrr(this Color v) { return new Color(v.a, v.r, v.r, v.r); }
        public static Color arrg(this Color v) { return new Color(v.a, v.r, v.r, v.g); }
        public static Color arrb(this Color v) { return new Color(v.a, v.r, v.r, v.b); }
        public static Color arra(this Color v) { return new Color(v.a, v.r, v.r, v.a); }
        public static Color arrn(this Color v, float n_0 = 0) { return new Color(v.a, v.r, v.r, n_0); }
        public static Color argr(this Color v) { return new Color(v.a, v.r, v.g, v.r); }
        public static Color argg(this Color v) { return new Color(v.a, v.r, v.g, v.g); }
        public static Color argb(this Color v) { return new Color(v.a, v.r, v.g, v.b); }
        public static Color arga(this Color v) { return new Color(v.a, v.r, v.g, v.a); }
        public static Color argn(this Color v, float n_0 = 0) { return new Color(v.a, v.r, v.g, n_0); }
        public static Color arbr(this Color v) { return new Color(v.a, v.r, v.b, v.r); }
        public static Color arbg(this Color v) { return new Color(v.a, v.r, v.b, v.g); }
        public static Color arbb(this Color v) { return new Color(v.a, v.r, v.b, v.b); }
        public static Color arba(this Color v) { return new Color(v.a, v.r, v.b, v.a); }
        public static Color arbn(this Color v, float n_0 = 0) { return new Color(v.a, v.r, v.b, n_0); }
        public static Color arar(this Color v) { return new Color(v.a, v.r, v.a, v.r); }
        public static Color arag(this Color v) { return new Color(v.a, v.r, v.a, v.g); }
        public static Color arab(this Color v) { return new Color(v.a, v.r, v.a, v.b); }
        public static Color araa(this Color v) { return new Color(v.a, v.r, v.a, v.a); }
        public static Color aran(this Color v, float n_0 = 0) { return new Color(v.a, v.r, v.a, n_0); }
        public static Color arnr(this Color v, float n_0 = 0) { return new Color(v.a, v.r, n_0, v.r); }
        public static Color arng(this Color v, float n_0 = 0) { return new Color(v.a, v.r, n_0, v.g); }
        public static Color arnb(this Color v, float n_0 = 0) { return new Color(v.a, v.r, n_0, v.b); }
        public static Color arna(this Color v, float n_0 = 0) { return new Color(v.a, v.r, n_0, v.a); }
        public static Color arnn(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(v.a, v.r, n_0, n_1); }
        public static Color agrr(this Color v) { return new Color(v.a, v.g, v.r, v.r); }
        public static Color agrg(this Color v) { return new Color(v.a, v.g, v.r, v.g); }
        public static Color agrb(this Color v) { return new Color(v.a, v.g, v.r, v.b); }
        public static Color agra(this Color v) { return new Color(v.a, v.g, v.r, v.a); }
        public static Color agrn(this Color v, float n_0 = 0) { return new Color(v.a, v.g, v.r, n_0); }
        public static Color aggr(this Color v) { return new Color(v.a, v.g, v.g, v.r); }
        public static Color aggg(this Color v) { return new Color(v.a, v.g, v.g, v.g); }
        public static Color aggb(this Color v) { return new Color(v.a, v.g, v.g, v.b); }
        public static Color agga(this Color v) { return new Color(v.a, v.g, v.g, v.a); }
        public static Color aggn(this Color v, float n_0 = 0) { return new Color(v.a, v.g, v.g, n_0); }
        public static Color agbr(this Color v) { return new Color(v.a, v.g, v.b, v.r); }
        public static Color agbg(this Color v) { return new Color(v.a, v.g, v.b, v.g); }
        public static Color agbb(this Color v) { return new Color(v.a, v.g, v.b, v.b); }
        public static Color agba(this Color v) { return new Color(v.a, v.g, v.b, v.a); }
        public static Color agbn(this Color v, float n_0 = 0) { return new Color(v.a, v.g, v.b, n_0); }
        public static Color agar(this Color v) { return new Color(v.a, v.g, v.a, v.r); }
        public static Color agag(this Color v) { return new Color(v.a, v.g, v.a, v.g); }
        public static Color agab(this Color v) { return new Color(v.a, v.g, v.a, v.b); }
        public static Color agaa(this Color v) { return new Color(v.a, v.g, v.a, v.a); }
        public static Color agan(this Color v, float n_0 = 0) { return new Color(v.a, v.g, v.a, n_0); }
        public static Color agnr(this Color v, float n_0 = 0) { return new Color(v.a, v.g, n_0, v.r); }
        public static Color agng(this Color v, float n_0 = 0) { return new Color(v.a, v.g, n_0, v.g); }
        public static Color agnb(this Color v, float n_0 = 0) { return new Color(v.a, v.g, n_0, v.b); }
        public static Color agna(this Color v, float n_0 = 0) { return new Color(v.a, v.g, n_0, v.a); }
        public static Color agnn(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(v.a, v.g, n_0, n_1); }
        public static Color abrr(this Color v) { return new Color(v.a, v.b, v.r, v.r); }
        public static Color abrg(this Color v) { return new Color(v.a, v.b, v.r, v.g); }
        public static Color abrb(this Color v) { return new Color(v.a, v.b, v.r, v.b); }
        public static Color abra(this Color v) { return new Color(v.a, v.b, v.r, v.a); }
        public static Color abrn(this Color v, float n_0 = 0) { return new Color(v.a, v.b, v.r, n_0); }
        public static Color abgr(this Color v) { return new Color(v.a, v.b, v.g, v.r); }
        public static Color abgg(this Color v) { return new Color(v.a, v.b, v.g, v.g); }
        public static Color abgb(this Color v) { return new Color(v.a, v.b, v.g, v.b); }
        public static Color abga(this Color v) { return new Color(v.a, v.b, v.g, v.a); }
        public static Color abgn(this Color v, float n_0 = 0) { return new Color(v.a, v.b, v.g, n_0); }
        public static Color abbr(this Color v) { return new Color(v.a, v.b, v.b, v.r); }
        public static Color abbg(this Color v) { return new Color(v.a, v.b, v.b, v.g); }
        public static Color abbb(this Color v) { return new Color(v.a, v.b, v.b, v.b); }
        public static Color abba(this Color v) { return new Color(v.a, v.b, v.b, v.a); }
        public static Color abbn(this Color v, float n_0 = 0) { return new Color(v.a, v.b, v.b, n_0); }
        public static Color abar(this Color v) { return new Color(v.a, v.b, v.a, v.r); }
        public static Color abag(this Color v) { return new Color(v.a, v.b, v.a, v.g); }
        public static Color abab(this Color v) { return new Color(v.a, v.b, v.a, v.b); }
        public static Color abaa(this Color v) { return new Color(v.a, v.b, v.a, v.a); }
        public static Color aban(this Color v, float n_0 = 0) { return new Color(v.a, v.b, v.a, n_0); }
        public static Color abnr(this Color v, float n_0 = 0) { return new Color(v.a, v.b, n_0, v.r); }
        public static Color abng(this Color v, float n_0 = 0) { return new Color(v.a, v.b, n_0, v.g); }
        public static Color abnb(this Color v, float n_0 = 0) { return new Color(v.a, v.b, n_0, v.b); }
        public static Color abna(this Color v, float n_0 = 0) { return new Color(v.a, v.b, n_0, v.a); }
        public static Color abnn(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(v.a, v.b, n_0, n_1); }
        public static Color aarr(this Color v) { return new Color(v.a, v.a, v.r, v.r); }
        public static Color aarg(this Color v) { return new Color(v.a, v.a, v.r, v.g); }
        public static Color aarb(this Color v) { return new Color(v.a, v.a, v.r, v.b); }
        public static Color aara(this Color v) { return new Color(v.a, v.a, v.r, v.a); }
        public static Color aarn(this Color v, float n_0 = 0) { return new Color(v.a, v.a, v.r, n_0); }
        public static Color aagr(this Color v) { return new Color(v.a, v.a, v.g, v.r); }
        public static Color aagg(this Color v) { return new Color(v.a, v.a, v.g, v.g); }
        public static Color aagb(this Color v) { return new Color(v.a, v.a, v.g, v.b); }
        public static Color aaga(this Color v) { return new Color(v.a, v.a, v.g, v.a); }
        public static Color aagn(this Color v, float n_0 = 0) { return new Color(v.a, v.a, v.g, n_0); }
        public static Color aabr(this Color v) { return new Color(v.a, v.a, v.b, v.r); }
        public static Color aabg(this Color v) { return new Color(v.a, v.a, v.b, v.g); }
        public static Color aabb(this Color v) { return new Color(v.a, v.a, v.b, v.b); }
        public static Color aaba(this Color v) { return new Color(v.a, v.a, v.b, v.a); }
        public static Color aabn(this Color v, float n_0 = 0) { return new Color(v.a, v.a, v.b, n_0); }
        public static Color aaar(this Color v) { return new Color(v.a, v.a, v.a, v.r); }
        public static Color aaag(this Color v) { return new Color(v.a, v.a, v.a, v.g); }
        public static Color aaab(this Color v) { return new Color(v.a, v.a, v.a, v.b); }
        public static Color aaaa(this Color v) { return new Color(v.a, v.a, v.a, v.a); }
        public static Color aaan(this Color v, float n_0 = 0) { return new Color(v.a, v.a, v.a, n_0); }
        public static Color aanr(this Color v, float n_0 = 0) { return new Color(v.a, v.a, n_0, v.r); }
        public static Color aang(this Color v, float n_0 = 0) { return new Color(v.a, v.a, n_0, v.g); }
        public static Color aanb(this Color v, float n_0 = 0) { return new Color(v.a, v.a, n_0, v.b); }
        public static Color aana(this Color v, float n_0 = 0) { return new Color(v.a, v.a, n_0, v.a); }
        public static Color aann(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(v.a, v.a, n_0, n_1); }
        public static Color anrr(this Color v, float n_0 = 0) { return new Color(v.a, n_0, v.r, v.r); }
        public static Color anrg(this Color v, float n_0 = 0) { return new Color(v.a, n_0, v.r, v.g); }
        public static Color anrb(this Color v, float n_0 = 0) { return new Color(v.a, n_0, v.r, v.b); }
        public static Color anra(this Color v, float n_0 = 0) { return new Color(v.a, n_0, v.r, v.a); }
        public static Color anrn(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(v.a, n_0, v.r, n_1); }
        public static Color angr(this Color v, float n_0 = 0) { return new Color(v.a, n_0, v.g, v.r); }
        public static Color angg(this Color v, float n_0 = 0) { return new Color(v.a, n_0, v.g, v.g); }
        public static Color angb(this Color v, float n_0 = 0) { return new Color(v.a, n_0, v.g, v.b); }
        public static Color anga(this Color v, float n_0 = 0) { return new Color(v.a, n_0, v.g, v.a); }
        public static Color angn(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(v.a, n_0, v.g, n_1); }
        public static Color anbr(this Color v, float n_0 = 0) { return new Color(v.a, n_0, v.b, v.r); }
        public static Color anbg(this Color v, float n_0 = 0) { return new Color(v.a, n_0, v.b, v.g); }
        public static Color anbb(this Color v, float n_0 = 0) { return new Color(v.a, n_0, v.b, v.b); }
        public static Color anba(this Color v, float n_0 = 0) { return new Color(v.a, n_0, v.b, v.a); }
        public static Color anbn(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(v.a, n_0, v.b, n_1); }
        public static Color anar(this Color v, float n_0 = 0) { return new Color(v.a, n_0, v.a, v.r); }
        public static Color anag(this Color v, float n_0 = 0) { return new Color(v.a, n_0, v.a, v.g); }
        public static Color anab(this Color v, float n_0 = 0) { return new Color(v.a, n_0, v.a, v.b); }
        public static Color anaa(this Color v, float n_0 = 0) { return new Color(v.a, n_0, v.a, v.a); }
        public static Color anan(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(v.a, n_0, v.a, n_1); }
        public static Color annr(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(v.a, n_0, n_1, v.r); }
        public static Color anng(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(v.a, n_0, n_1, v.g); }
        public static Color annb(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(v.a, n_0, n_1, v.b); }
        public static Color anna(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(v.a, n_0, n_1, v.a); }
        public static Color annn(this Color v, float n_0 = 0, float n_1 = 0, float n_2 = 0) { return new Color(v.a, n_0, n_1, n_2); }
        public static Color nrrr(this Color v, float n_0 = 0) { return new Color(n_0, v.r, v.r, v.r); }
        public static Color nrrg(this Color v, float n_0 = 0) { return new Color(n_0, v.r, v.r, v.g); }
        public static Color nrrb(this Color v, float n_0 = 0) { return new Color(n_0, v.r, v.r, v.b); }
        public static Color nrra(this Color v, float n_0 = 0) { return new Color(n_0, v.r, v.r, v.a); }
        public static Color nrrn(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(n_0, v.r, v.r, n_1); }
        public static Color nrgr(this Color v, float n_0 = 0) { return new Color(n_0, v.r, v.g, v.r); }
        public static Color nrgg(this Color v, float n_0 = 0) { return new Color(n_0, v.r, v.g, v.g); }
        public static Color nrgb(this Color v, float n_0 = 0) { return new Color(n_0, v.r, v.g, v.b); }
        public static Color nrga(this Color v, float n_0 = 0) { return new Color(n_0, v.r, v.g, v.a); }
        public static Color nrgn(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(n_0, v.r, v.g, n_1); }
        public static Color nrbr(this Color v, float n_0 = 0) { return new Color(n_0, v.r, v.b, v.r); }
        public static Color nrbg(this Color v, float n_0 = 0) { return new Color(n_0, v.r, v.b, v.g); }
        public static Color nrbb(this Color v, float n_0 = 0) { return new Color(n_0, v.r, v.b, v.b); }
        public static Color nrba(this Color v, float n_0 = 0) { return new Color(n_0, v.r, v.b, v.a); }
        public static Color nrbn(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(n_0, v.r, v.b, n_1); }
        public static Color nrar(this Color v, float n_0 = 0) { return new Color(n_0, v.r, v.a, v.r); }
        public static Color nrag(this Color v, float n_0 = 0) { return new Color(n_0, v.r, v.a, v.g); }
        public static Color nrab(this Color v, float n_0 = 0) { return new Color(n_0, v.r, v.a, v.b); }
        public static Color nraa(this Color v, float n_0 = 0) { return new Color(n_0, v.r, v.a, v.a); }
        public static Color nran(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(n_0, v.r, v.a, n_1); }
        public static Color nrnr(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(n_0, v.r, n_1, v.r); }
        public static Color nrng(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(n_0, v.r, n_1, v.g); }
        public static Color nrnb(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(n_0, v.r, n_1, v.b); }
        public static Color nrna(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(n_0, v.r, n_1, v.a); }
        public static Color nrnn(this Color v, float n_0 = 0, float n_1 = 0, float n_2 = 0) { return new Color(n_0, v.r, n_1, n_2); }
        public static Color ngrr(this Color v, float n_0 = 0) { return new Color(n_0, v.g, v.r, v.r); }
        public static Color ngrg(this Color v, float n_0 = 0) { return new Color(n_0, v.g, v.r, v.g); }
        public static Color ngrb(this Color v, float n_0 = 0) { return new Color(n_0, v.g, v.r, v.b); }
        public static Color ngra(this Color v, float n_0 = 0) { return new Color(n_0, v.g, v.r, v.a); }
        public static Color ngrn(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(n_0, v.g, v.r, n_1); }
        public static Color nggr(this Color v, float n_0 = 0) { return new Color(n_0, v.g, v.g, v.r); }
        public static Color nggg(this Color v, float n_0 = 0) { return new Color(n_0, v.g, v.g, v.g); }
        public static Color nggb(this Color v, float n_0 = 0) { return new Color(n_0, v.g, v.g, v.b); }
        public static Color ngga(this Color v, float n_0 = 0) { return new Color(n_0, v.g, v.g, v.a); }
        public static Color nggn(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(n_0, v.g, v.g, n_1); }
        public static Color ngbr(this Color v, float n_0 = 0) { return new Color(n_0, v.g, v.b, v.r); }
        public static Color ngbg(this Color v, float n_0 = 0) { return new Color(n_0, v.g, v.b, v.g); }
        public static Color ngbb(this Color v, float n_0 = 0) { return new Color(n_0, v.g, v.b, v.b); }
        public static Color ngba(this Color v, float n_0 = 0) { return new Color(n_0, v.g, v.b, v.a); }
        public static Color ngbn(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(n_0, v.g, v.b, n_1); }
        public static Color ngar(this Color v, float n_0 = 0) { return new Color(n_0, v.g, v.a, v.r); }
        public static Color ngag(this Color v, float n_0 = 0) { return new Color(n_0, v.g, v.a, v.g); }
        public static Color ngab(this Color v, float n_0 = 0) { return new Color(n_0, v.g, v.a, v.b); }
        public static Color ngaa(this Color v, float n_0 = 0) { return new Color(n_0, v.g, v.a, v.a); }
        public static Color ngan(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(n_0, v.g, v.a, n_1); }
        public static Color ngnr(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(n_0, v.g, n_1, v.r); }
        public static Color ngng(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(n_0, v.g, n_1, v.g); }
        public static Color ngnb(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(n_0, v.g, n_1, v.b); }
        public static Color ngna(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(n_0, v.g, n_1, v.a); }
        public static Color ngnn(this Color v, float n_0 = 0, float n_1 = 0, float n_2 = 0) { return new Color(n_0, v.g, n_1, n_2); }
        public static Color nbrr(this Color v, float n_0 = 0) { return new Color(n_0, v.b, v.r, v.r); }
        public static Color nbrg(this Color v, float n_0 = 0) { return new Color(n_0, v.b, v.r, v.g); }
        public static Color nbrb(this Color v, float n_0 = 0) { return new Color(n_0, v.b, v.r, v.b); }
        public static Color nbra(this Color v, float n_0 = 0) { return new Color(n_0, v.b, v.r, v.a); }
        public static Color nbrn(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(n_0, v.b, v.r, n_1); }
        public static Color nbgr(this Color v, float n_0 = 0) { return new Color(n_0, v.b, v.g, v.r); }
        public static Color nbgg(this Color v, float n_0 = 0) { return new Color(n_0, v.b, v.g, v.g); }
        public static Color nbgb(this Color v, float n_0 = 0) { return new Color(n_0, v.b, v.g, v.b); }
        public static Color nbga(this Color v, float n_0 = 0) { return new Color(n_0, v.b, v.g, v.a); }
        public static Color nbgn(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(n_0, v.b, v.g, n_1); }
        public static Color nbbr(this Color v, float n_0 = 0) { return new Color(n_0, v.b, v.b, v.r); }
        public static Color nbbg(this Color v, float n_0 = 0) { return new Color(n_0, v.b, v.b, v.g); }
        public static Color nbbb(this Color v, float n_0 = 0) { return new Color(n_0, v.b, v.b, v.b); }
        public static Color nbba(this Color v, float n_0 = 0) { return new Color(n_0, v.b, v.b, v.a); }
        public static Color nbbn(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(n_0, v.b, v.b, n_1); }
        public static Color nbar(this Color v, float n_0 = 0) { return new Color(n_0, v.b, v.a, v.r); }
        public static Color nbag(this Color v, float n_0 = 0) { return new Color(n_0, v.b, v.a, v.g); }
        public static Color nbab(this Color v, float n_0 = 0) { return new Color(n_0, v.b, v.a, v.b); }
        public static Color nbaa(this Color v, float n_0 = 0) { return new Color(n_0, v.b, v.a, v.a); }
        public static Color nban(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(n_0, v.b, v.a, n_1); }
        public static Color nbnr(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(n_0, v.b, n_1, v.r); }
        public static Color nbng(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(n_0, v.b, n_1, v.g); }
        public static Color nbnb(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(n_0, v.b, n_1, v.b); }
        public static Color nbna(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(n_0, v.b, n_1, v.a); }
        public static Color nbnn(this Color v, float n_0 = 0, float n_1 = 0, float n_2 = 0) { return new Color(n_0, v.b, n_1, n_2); }
        public static Color narr(this Color v, float n_0 = 0) { return new Color(n_0, v.a, v.r, v.r); }
        public static Color narg(this Color v, float n_0 = 0) { return new Color(n_0, v.a, v.r, v.g); }
        public static Color narb(this Color v, float n_0 = 0) { return new Color(n_0, v.a, v.r, v.b); }
        public static Color nara(this Color v, float n_0 = 0) { return new Color(n_0, v.a, v.r, v.a); }
        public static Color narn(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(n_0, v.a, v.r, n_1); }
        public static Color nagr(this Color v, float n_0 = 0) { return new Color(n_0, v.a, v.g, v.r); }
        public static Color nagg(this Color v, float n_0 = 0) { return new Color(n_0, v.a, v.g, v.g); }
        public static Color nagb(this Color v, float n_0 = 0) { return new Color(n_0, v.a, v.g, v.b); }
        public static Color naga(this Color v, float n_0 = 0) { return new Color(n_0, v.a, v.g, v.a); }
        public static Color nagn(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(n_0, v.a, v.g, n_1); }
        public static Color nabr(this Color v, float n_0 = 0) { return new Color(n_0, v.a, v.b, v.r); }
        public static Color nabg(this Color v, float n_0 = 0) { return new Color(n_0, v.a, v.b, v.g); }
        public static Color nabb(this Color v, float n_0 = 0) { return new Color(n_0, v.a, v.b, v.b); }
        public static Color naba(this Color v, float n_0 = 0) { return new Color(n_0, v.a, v.b, v.a); }
        public static Color nabn(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(n_0, v.a, v.b, n_1); }
        public static Color naar(this Color v, float n_0 = 0) { return new Color(n_0, v.a, v.a, v.r); }
        public static Color naag(this Color v, float n_0 = 0) { return new Color(n_0, v.a, v.a, v.g); }
        public static Color naab(this Color v, float n_0 = 0) { return new Color(n_0, v.a, v.a, v.b); }
        public static Color naaa(this Color v, float n_0 = 0) { return new Color(n_0, v.a, v.a, v.a); }
        public static Color naan(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(n_0, v.a, v.a, n_1); }
        public static Color nanr(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(n_0, v.a, n_1, v.r); }
        public static Color nang(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(n_0, v.a, n_1, v.g); }
        public static Color nanb(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(n_0, v.a, n_1, v.b); }
        public static Color nana(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(n_0, v.a, n_1, v.a); }
        public static Color nann(this Color v, float n_0 = 0, float n_1 = 0, float n_2 = 0) { return new Color(n_0, v.a, n_1, n_2); }
        public static Color nnrr(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(n_0, n_1, v.r, v.r); }
        public static Color nnrg(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(n_0, n_1, v.r, v.g); }
        public static Color nnrb(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(n_0, n_1, v.r, v.b); }
        public static Color nnra(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(n_0, n_1, v.r, v.a); }
        public static Color nnrn(this Color v, float n_0 = 0, float n_1 = 0, float n_2 = 0) { return new Color(n_0, n_1, v.r, n_2); }
        public static Color nngr(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(n_0, n_1, v.g, v.r); }
        public static Color nngg(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(n_0, n_1, v.g, v.g); }
        public static Color nngb(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(n_0, n_1, v.g, v.b); }
        public static Color nnga(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(n_0, n_1, v.g, v.a); }
        public static Color nngn(this Color v, float n_0 = 0, float n_1 = 0, float n_2 = 0) { return new Color(n_0, n_1, v.g, n_2); }
        public static Color nnbr(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(n_0, n_1, v.b, v.r); }
        public static Color nnbg(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(n_0, n_1, v.b, v.g); }
        public static Color nnbb(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(n_0, n_1, v.b, v.b); }
        public static Color nnba(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(n_0, n_1, v.b, v.a); }
        public static Color nnbn(this Color v, float n_0 = 0, float n_1 = 0, float n_2 = 0) { return new Color(n_0, n_1, v.b, n_2); }
        public static Color nnar(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(n_0, n_1, v.a, v.r); }
        public static Color nnag(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(n_0, n_1, v.a, v.g); }
        public static Color nnab(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(n_0, n_1, v.a, v.b); }
        public static Color nnaa(this Color v, float n_0 = 0, float n_1 = 0) { return new Color(n_0, n_1, v.a, v.a); }
        public static Color nnan(this Color v, float n_0 = 0, float n_1 = 0, float n_2 = 0) { return new Color(n_0, n_1, v.a, n_2); }
        public static Color nnnr(this Color v, float n_0 = 0, float n_1 = 0, float n_2 = 0) { return new Color(n_0, n_1, n_2, v.r); }
        public static Color nnng(this Color v, float n_0 = 0, float n_1 = 0, float n_2 = 0) { return new Color(n_0, n_1, n_2, v.g); }
        public static Color nnnb(this Color v, float n_0 = 0, float n_1 = 0, float n_2 = 0) { return new Color(n_0, n_1, n_2, v.b); }
        public static Color nnna(this Color v, float n_0 = 0, float n_1 = 0, float n_2 = 0) { return new Color(n_0, n_1, n_2, v.a); }
        #endregion Swizzle PRTK_Color
        
        
    }
}
