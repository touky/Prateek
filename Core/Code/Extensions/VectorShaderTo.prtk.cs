// -BEGIN_PRATEEK_COPYRIGHT-
//
//  Prateek, a library that is "bien pratique"
//  Header last update date: 08/07/2020
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
namespace Prateek.Core.Code.ShaderTo
{
    // -BEGIN_PRATEEK_CSHARP_NAMESPACE_CODE-
    ///------------------------------------------------------------------------
    #region Prateek Code Namespaces
    using System;
    using System.Collections;
    using System.Collections.Generic;
    
    using UnityEngine;
    
    using Prateek;
    using static Prateek.Core.Code.ShaderTo.CSharp;
    #endregion Prateek Code Namespaces
// -END_PRATEEK_CSHARP_NAMESPACE_CODE-
    

    ///------------------------------------------------------------------------
    public static partial class CSharp
    {
        
        //--
        #region Mixed Ctor Vector2
        public static Vector2 vec2(float n_0, float n_1) { return new Vector2(n_0, n_1); }
        public static Vector2 vec2(Vector2 v_0) { return new Vector2(v_0.x, v_0.y); }
        public static Vector2 vec2(float n_0) { return new Vector2(n_0, n_0); }
        #endregion Mixed Ctor Vector2
        
        //--
        #region Mixed Ctor Vector3
        public static Vector3 vec3(float n_0, float n_1, float n_2) { return new Vector3(n_0, n_1, n_2); }
        public static Vector3 vec3(float n_0, Vector2 v_0) { return new Vector3(n_0, v_0.x, v_0.y); }
        public static Vector3 vec3(Vector2 v_0, float n_0) { return new Vector3(v_0.x, v_0.y, n_0); }
        public static Vector3 vec3(Vector3 v_0) { return new Vector3(v_0.x, v_0.y, v_0.z); }
        public static Vector3 vec3(float n_0) { return new Vector3(n_0, n_0, n_0); }
        #endregion Mixed Ctor Vector3
        
        //--
        #region Mixed Ctor Vector4
        public static Vector4 vec4(float n_0, float n_1, float n_2, float n_3) { return new Vector4(n_0, n_1, n_2, n_3); }
        public static Vector4 vec4(float n_0, float n_1, Vector2 v_0) { return new Vector4(n_0, n_1, v_0.x, v_0.y); }
        public static Vector4 vec4(float n_0, Vector2 v_0, float n_1) { return new Vector4(n_0, v_0.x, v_0.y, n_1); }
        public static Vector4 vec4(float n_0, Vector3 v_0) { return new Vector4(n_0, v_0.x, v_0.y, v_0.z); }
        public static Vector4 vec4(Vector2 v_0, float n_0, float n_1) { return new Vector4(v_0.x, v_0.y, n_0, n_1); }
        public static Vector4 vec4(Vector2 v_0, Vector2 v_1) { return new Vector4(v_0.x, v_0.y, v_1.x, v_1.y); }
        public static Vector4 vec4(Vector3 v_0, float n_0) { return new Vector4(v_0.x, v_0.y, v_0.z, n_0); }
        public static Vector4 vec4(Vector4 v_0) { return new Vector4(v_0.x, v_0.y, v_0.z, v_0.w); }
        public static Vector4 vec4(float n_0) { return new Vector4(n_0, n_0, n_0, n_0); }
        #endregion Mixed Ctor Vector4
        
    }
}



///----------------------------------------------------------------------------
namespace Prateek.Core.Code.ShaderTo
{
    // -BEGIN_PRATEEK_CSHARP_NAMESPACE_CODE-
    ///------------------------------------------------------------------------
    #region Prateek Code Namespaces
    using System;
    using System.Collections;
    using System.Collections.Generic;
    
    using UnityEngine;
    
    using Prateek;
    using static Prateek.Core.Code.ShaderTo.CSharp;
    #endregion Prateek Code Namespaces
// -END_PRATEEK_CSHARP_NAMESPACE_CODE-
    

    ///------------------------------------------------------------------------
    public static partial class CSharp
    {
        
        //--
        #region Mixed Func float
        public static float clamp(float n_0, float n_1, float n_2) { return Mathf.Clamp(n_0, n_1, n_2); }
        public static float saturate(float n_0) { return Mathf.Clamp01(n_0); }
        public static float random(float n_0, float n_1) { return UnityEngine.Random.Range(n_0, n_1); }
        public static float max(float n_0, float n_1) { return Mathf.Max(n_0, n_1); }
        public static float min(float n_0, float n_1) { return Mathf.Min(n_0, n_1); }
        public static float mul(float n_0, float n_1) { return n_0 * n_1; }
        public static float div(float n_0, float n_1) { return n_0 / n_1; }
        public static float mod(float n_0, float n_1) { return (n_0 + n_1) % n_1; }
        public static float fract(float n_0) { return n_0 - floor(n_0); }
        public static float abs(float n_0) { return Mathf.Abs(n_0); }
        public static float sign(float n_0) { return Mathf.Sign(n_0); }
        public static float exp(float n_0) { return Mathf.Exp(n_0); }
        public static float cos(float n_0) { return Mathf.Cos(n_0); }
        public static float sin(float n_0) { return Mathf.Sin(n_0); }
        public static float tan(float n_0) { return Mathf.Tan(n_0); }
        public static float acos(float n_0) { return Mathf.Acos(n_0); }
        public static float asin(float n_0) { return Mathf.Asin(n_0); }
        public static float atan(float n_0) { return Mathf.Atan(n_0); }
        public static float atan2(float n_0, float n_1) { return Mathf.Atan2(n_0, n_1); }
        public static float ceil(float n_0) { return Mathf.Ceil(n_0); }
        public static float floor(float n_0) { return Mathf.Floor(n_0); }
        public static float sqrt(float n_0) { return Mathf.Sqrt(n_0); }
        #endregion Mixed Func float
        
        //--
        #region Mixed Func Vector2
        public static Vector2 clamp(Vector2 v_0, Vector2 v_1, Vector2 v_2) { return new Vector2(Mathf.Clamp(v_0.x, v_1.x, v_2.x), Mathf.Clamp(v_0.y, v_1.y, v_2.y)); }
        public static Vector2 clamp(Vector2 v_0, float n_1, float n_2) { return new Vector2(Mathf.Clamp(v_0.x, n_1, n_2), Mathf.Clamp(v_0.y, n_1, n_2)); }
        public static Vector2 saturate(Vector2 v_0) { return new Vector2(Mathf.Clamp01(v_0.x), Mathf.Clamp01(v_0.y)); }
        public static Vector2 random(Vector2 v_0, Vector2 v_1) { return new Vector2(UnityEngine.Random.Range(v_0.x, v_1.x), UnityEngine.Random.Range(v_0.y, v_1.y)); }
        public static Vector2 random(Vector2 v_0, float n_1) { return new Vector2(UnityEngine.Random.Range(v_0.x, n_1), UnityEngine.Random.Range(v_0.y, n_1)); }
        public static Vector2 max(Vector2 v_0, Vector2 v_1) { return new Vector2(Mathf.Max(v_0.x, v_1.x), Mathf.Max(v_0.y, v_1.y)); }
        public static Vector2 max(Vector2 v_0, float n_1) { return new Vector2(Mathf.Max(v_0.x, n_1), Mathf.Max(v_0.y, n_1)); }
        public static Vector2 min(Vector2 v_0, Vector2 v_1) { return new Vector2(Mathf.Min(v_0.x, v_1.x), Mathf.Min(v_0.y, v_1.y)); }
        public static Vector2 min(Vector2 v_0, float n_1) { return new Vector2(Mathf.Min(v_0.x, n_1), Mathf.Min(v_0.y, n_1)); }
        public static Vector2 mul(Vector2 v_0, Vector2 v_1) { return new Vector2(v_0.x * v_1.x, v_0.y * v_1.y); }
        public static Vector2 mul(Vector2 v_0, float n_1) { return new Vector2(v_0.x * n_1, v_0.y * n_1); }
        public static Vector2 div(Vector2 v_0, Vector2 v_1) { return new Vector2(v_0.x / v_1.x, v_0.y / v_1.y); }
        public static Vector2 div(Vector2 v_0, float n_1) { return new Vector2(v_0.x / n_1, v_0.y / n_1); }
        public static Vector2 mod(Vector2 v_0, Vector2 v_1) { return new Vector2((v_0.x + v_1.x) % v_1.x, (v_0.y + v_1.y) % v_1.y); }
        public static Vector2 mod(Vector2 v_0, float n_1) { return new Vector2((v_0.x + n_1) % n_1, (v_0.y + n_1) % n_1); }
        public static Vector2 fract(Vector2 v_0) { return new Vector2(v_0.x - floor(v_0.x), v_0.y - floor(v_0.y)); }
        public static Vector2 abs(Vector2 v_0) { return new Vector2(Mathf.Abs(v_0.x), Mathf.Abs(v_0.y)); }
        public static Vector2 sign(Vector2 v_0) { return new Vector2(Mathf.Sign(v_0.x), Mathf.Sign(v_0.y)); }
        public static Vector2 exp(Vector2 v_0) { return new Vector2(Mathf.Exp(v_0.x), Mathf.Exp(v_0.y)); }
        public static Vector2 cos(Vector2 v_0) { return new Vector2(Mathf.Cos(v_0.x), Mathf.Cos(v_0.y)); }
        public static Vector2 sin(Vector2 v_0) { return new Vector2(Mathf.Sin(v_0.x), Mathf.Sin(v_0.y)); }
        public static Vector2 tan(Vector2 v_0) { return new Vector2(Mathf.Tan(v_0.x), Mathf.Tan(v_0.y)); }
        public static Vector2 acos(Vector2 v_0) { return new Vector2(Mathf.Acos(v_0.x), Mathf.Acos(v_0.y)); }
        public static Vector2 asin(Vector2 v_0) { return new Vector2(Mathf.Asin(v_0.x), Mathf.Asin(v_0.y)); }
        public static Vector2 atan(Vector2 v_0) { return new Vector2(Mathf.Atan(v_0.x), Mathf.Atan(v_0.y)); }
        public static Vector2 atan2(Vector2 v_0, Vector2 v_1) { return new Vector2(Mathf.Atan2(v_0.x, v_1.x), Mathf.Atan2(v_0.y, v_1.y)); }
        public static Vector2 atan2(Vector2 v_0, float n_1) { return new Vector2(Mathf.Atan2(v_0.x, n_1), Mathf.Atan2(v_0.y, n_1)); }
        public static Vector2 ceil(Vector2 v_0) { return new Vector2(Mathf.Ceil(v_0.x), Mathf.Ceil(v_0.y)); }
        public static Vector2 floor(Vector2 v_0) { return new Vector2(Mathf.Floor(v_0.x), Mathf.Floor(v_0.y)); }
        public static Vector2 sqrt(Vector2 v_0) { return new Vector2(Mathf.Sqrt(v_0.x), Mathf.Sqrt(v_0.y)); }
        #endregion Mixed Func Vector2
        
        //--
        #region Mixed Func Vector3
        public static Vector3 clamp(Vector3 v_0, Vector3 v_1, Vector3 v_2) { return new Vector3(Mathf.Clamp(v_0.x, v_1.x, v_2.x), Mathf.Clamp(v_0.y, v_1.y, v_2.y), Mathf.Clamp(v_0.z, v_1.z, v_2.z)); }
        public static Vector3 clamp(Vector3 v_0, float n_1, float n_2) { return new Vector3(Mathf.Clamp(v_0.x, n_1, n_2), Mathf.Clamp(v_0.y, n_1, n_2), Mathf.Clamp(v_0.z, n_1, n_2)); }
        public static Vector3 saturate(Vector3 v_0) { return new Vector3(Mathf.Clamp01(v_0.x), Mathf.Clamp01(v_0.y), Mathf.Clamp01(v_0.z)); }
        public static Vector3 random(Vector3 v_0, Vector3 v_1) { return new Vector3(UnityEngine.Random.Range(v_0.x, v_1.x), UnityEngine.Random.Range(v_0.y, v_1.y), UnityEngine.Random.Range(v_0.z, v_1.z)); }
        public static Vector3 random(Vector3 v_0, float n_1) { return new Vector3(UnityEngine.Random.Range(v_0.x, n_1), UnityEngine.Random.Range(v_0.y, n_1), UnityEngine.Random.Range(v_0.z, n_1)); }
        public static Vector3 max(Vector3 v_0, Vector3 v_1) { return new Vector3(Mathf.Max(v_0.x, v_1.x), Mathf.Max(v_0.y, v_1.y), Mathf.Max(v_0.z, v_1.z)); }
        public static Vector3 max(Vector3 v_0, float n_1) { return new Vector3(Mathf.Max(v_0.x, n_1), Mathf.Max(v_0.y, n_1), Mathf.Max(v_0.z, n_1)); }
        public static Vector3 min(Vector3 v_0, Vector3 v_1) { return new Vector3(Mathf.Min(v_0.x, v_1.x), Mathf.Min(v_0.y, v_1.y), Mathf.Min(v_0.z, v_1.z)); }
        public static Vector3 min(Vector3 v_0, float n_1) { return new Vector3(Mathf.Min(v_0.x, n_1), Mathf.Min(v_0.y, n_1), Mathf.Min(v_0.z, n_1)); }
        public static Vector3 mul(Vector3 v_0, Vector3 v_1) { return new Vector3(v_0.x * v_1.x, v_0.y * v_1.y, v_0.z * v_1.z); }
        public static Vector3 mul(Vector3 v_0, float n_1) { return new Vector3(v_0.x * n_1, v_0.y * n_1, v_0.z * n_1); }
        public static Vector3 div(Vector3 v_0, Vector3 v_1) { return new Vector3(v_0.x / v_1.x, v_0.y / v_1.y, v_0.z / v_1.z); }
        public static Vector3 div(Vector3 v_0, float n_1) { return new Vector3(v_0.x / n_1, v_0.y / n_1, v_0.z / n_1); }
        public static Vector3 mod(Vector3 v_0, Vector3 v_1) { return new Vector3((v_0.x + v_1.x) % v_1.x, (v_0.y + v_1.y) % v_1.y, (v_0.z + v_1.z) % v_1.z); }
        public static Vector3 mod(Vector3 v_0, float n_1) { return new Vector3((v_0.x + n_1) % n_1, (v_0.y + n_1) % n_1, (v_0.z + n_1) % n_1); }
        public static Vector3 fract(Vector3 v_0) { return new Vector3(v_0.x - floor(v_0.x), v_0.y - floor(v_0.y), v_0.z - floor(v_0.z)); }
        public static Vector3 abs(Vector3 v_0) { return new Vector3(Mathf.Abs(v_0.x), Mathf.Abs(v_0.y), Mathf.Abs(v_0.z)); }
        public static Vector3 sign(Vector3 v_0) { return new Vector3(Mathf.Sign(v_0.x), Mathf.Sign(v_0.y), Mathf.Sign(v_0.z)); }
        public static Vector3 exp(Vector3 v_0) { return new Vector3(Mathf.Exp(v_0.x), Mathf.Exp(v_0.y), Mathf.Exp(v_0.z)); }
        public static Vector3 cos(Vector3 v_0) { return new Vector3(Mathf.Cos(v_0.x), Mathf.Cos(v_0.y), Mathf.Cos(v_0.z)); }
        public static Vector3 sin(Vector3 v_0) { return new Vector3(Mathf.Sin(v_0.x), Mathf.Sin(v_0.y), Mathf.Sin(v_0.z)); }
        public static Vector3 tan(Vector3 v_0) { return new Vector3(Mathf.Tan(v_0.x), Mathf.Tan(v_0.y), Mathf.Tan(v_0.z)); }
        public static Vector3 acos(Vector3 v_0) { return new Vector3(Mathf.Acos(v_0.x), Mathf.Acos(v_0.y), Mathf.Acos(v_0.z)); }
        public static Vector3 asin(Vector3 v_0) { return new Vector3(Mathf.Asin(v_0.x), Mathf.Asin(v_0.y), Mathf.Asin(v_0.z)); }
        public static Vector3 atan(Vector3 v_0) { return new Vector3(Mathf.Atan(v_0.x), Mathf.Atan(v_0.y), Mathf.Atan(v_0.z)); }
        public static Vector3 atan2(Vector3 v_0, Vector3 v_1) { return new Vector3(Mathf.Atan2(v_0.x, v_1.x), Mathf.Atan2(v_0.y, v_1.y), Mathf.Atan2(v_0.z, v_1.z)); }
        public static Vector3 atan2(Vector3 v_0, float n_1) { return new Vector3(Mathf.Atan2(v_0.x, n_1), Mathf.Atan2(v_0.y, n_1), Mathf.Atan2(v_0.z, n_1)); }
        public static Vector3 ceil(Vector3 v_0) { return new Vector3(Mathf.Ceil(v_0.x), Mathf.Ceil(v_0.y), Mathf.Ceil(v_0.z)); }
        public static Vector3 floor(Vector3 v_0) { return new Vector3(Mathf.Floor(v_0.x), Mathf.Floor(v_0.y), Mathf.Floor(v_0.z)); }
        public static Vector3 sqrt(Vector3 v_0) { return new Vector3(Mathf.Sqrt(v_0.x), Mathf.Sqrt(v_0.y), Mathf.Sqrt(v_0.z)); }
        #endregion Mixed Func Vector3
        
        //--
        #region Mixed Func Vector4
        public static Vector4 clamp(Vector4 v_0, Vector4 v_1, Vector4 v_2) { return new Vector4(Mathf.Clamp(v_0.x, v_1.x, v_2.x), Mathf.Clamp(v_0.y, v_1.y, v_2.y), Mathf.Clamp(v_0.z, v_1.z, v_2.z), Mathf.Clamp(v_0.w, v_1.w, v_2.w)); }
        public static Vector4 clamp(Vector4 v_0, float n_1, float n_2) { return new Vector4(Mathf.Clamp(v_0.x, n_1, n_2), Mathf.Clamp(v_0.y, n_1, n_2), Mathf.Clamp(v_0.z, n_1, n_2), Mathf.Clamp(v_0.w, n_1, n_2)); }
        public static Vector4 saturate(Vector4 v_0) { return new Vector4(Mathf.Clamp01(v_0.x), Mathf.Clamp01(v_0.y), Mathf.Clamp01(v_0.z), Mathf.Clamp01(v_0.w)); }
        public static Vector4 random(Vector4 v_0, Vector4 v_1) { return new Vector4(UnityEngine.Random.Range(v_0.x, v_1.x), UnityEngine.Random.Range(v_0.y, v_1.y), UnityEngine.Random.Range(v_0.z, v_1.z), UnityEngine.Random.Range(v_0.w, v_1.w)); }
        public static Vector4 random(Vector4 v_0, float n_1) { return new Vector4(UnityEngine.Random.Range(v_0.x, n_1), UnityEngine.Random.Range(v_0.y, n_1), UnityEngine.Random.Range(v_0.z, n_1), UnityEngine.Random.Range(v_0.w, n_1)); }
        public static Vector4 max(Vector4 v_0, Vector4 v_1) { return new Vector4(Mathf.Max(v_0.x, v_1.x), Mathf.Max(v_0.y, v_1.y), Mathf.Max(v_0.z, v_1.z), Mathf.Max(v_0.w, v_1.w)); }
        public static Vector4 max(Vector4 v_0, float n_1) { return new Vector4(Mathf.Max(v_0.x, n_1), Mathf.Max(v_0.y, n_1), Mathf.Max(v_0.z, n_1), Mathf.Max(v_0.w, n_1)); }
        public static Vector4 min(Vector4 v_0, Vector4 v_1) { return new Vector4(Mathf.Min(v_0.x, v_1.x), Mathf.Min(v_0.y, v_1.y), Mathf.Min(v_0.z, v_1.z), Mathf.Min(v_0.w, v_1.w)); }
        public static Vector4 min(Vector4 v_0, float n_1) { return new Vector4(Mathf.Min(v_0.x, n_1), Mathf.Min(v_0.y, n_1), Mathf.Min(v_0.z, n_1), Mathf.Min(v_0.w, n_1)); }
        public static Vector4 mul(Vector4 v_0, Vector4 v_1) { return new Vector4(v_0.x * v_1.x, v_0.y * v_1.y, v_0.z * v_1.z, v_0.w * v_1.w); }
        public static Vector4 mul(Vector4 v_0, float n_1) { return new Vector4(v_0.x * n_1, v_0.y * n_1, v_0.z * n_1, v_0.w * n_1); }
        public static Vector4 div(Vector4 v_0, Vector4 v_1) { return new Vector4(v_0.x / v_1.x, v_0.y / v_1.y, v_0.z / v_1.z, v_0.w / v_1.w); }
        public static Vector4 div(Vector4 v_0, float n_1) { return new Vector4(v_0.x / n_1, v_0.y / n_1, v_0.z / n_1, v_0.w / n_1); }
        public static Vector4 mod(Vector4 v_0, Vector4 v_1) { return new Vector4((v_0.x + v_1.x) % v_1.x, (v_0.y + v_1.y) % v_1.y, (v_0.z + v_1.z) % v_1.z, (v_0.w + v_1.w) % v_1.w); }
        public static Vector4 mod(Vector4 v_0, float n_1) { return new Vector4((v_0.x + n_1) % n_1, (v_0.y + n_1) % n_1, (v_0.z + n_1) % n_1, (v_0.w + n_1) % n_1); }
        public static Vector4 fract(Vector4 v_0) { return new Vector4(v_0.x - floor(v_0.x), v_0.y - floor(v_0.y), v_0.z - floor(v_0.z), v_0.w - floor(v_0.w)); }
        public static Vector4 abs(Vector4 v_0) { return new Vector4(Mathf.Abs(v_0.x), Mathf.Abs(v_0.y), Mathf.Abs(v_0.z), Mathf.Abs(v_0.w)); }
        public static Vector4 sign(Vector4 v_0) { return new Vector4(Mathf.Sign(v_0.x), Mathf.Sign(v_0.y), Mathf.Sign(v_0.z), Mathf.Sign(v_0.w)); }
        public static Vector4 exp(Vector4 v_0) { return new Vector4(Mathf.Exp(v_0.x), Mathf.Exp(v_0.y), Mathf.Exp(v_0.z), Mathf.Exp(v_0.w)); }
        public static Vector4 cos(Vector4 v_0) { return new Vector4(Mathf.Cos(v_0.x), Mathf.Cos(v_0.y), Mathf.Cos(v_0.z), Mathf.Cos(v_0.w)); }
        public static Vector4 sin(Vector4 v_0) { return new Vector4(Mathf.Sin(v_0.x), Mathf.Sin(v_0.y), Mathf.Sin(v_0.z), Mathf.Sin(v_0.w)); }
        public static Vector4 tan(Vector4 v_0) { return new Vector4(Mathf.Tan(v_0.x), Mathf.Tan(v_0.y), Mathf.Tan(v_0.z), Mathf.Tan(v_0.w)); }
        public static Vector4 acos(Vector4 v_0) { return new Vector4(Mathf.Acos(v_0.x), Mathf.Acos(v_0.y), Mathf.Acos(v_0.z), Mathf.Acos(v_0.w)); }
        public static Vector4 asin(Vector4 v_0) { return new Vector4(Mathf.Asin(v_0.x), Mathf.Asin(v_0.y), Mathf.Asin(v_0.z), Mathf.Asin(v_0.w)); }
        public static Vector4 atan(Vector4 v_0) { return new Vector4(Mathf.Atan(v_0.x), Mathf.Atan(v_0.y), Mathf.Atan(v_0.z), Mathf.Atan(v_0.w)); }
        public static Vector4 atan2(Vector4 v_0, Vector4 v_1) { return new Vector4(Mathf.Atan2(v_0.x, v_1.x), Mathf.Atan2(v_0.y, v_1.y), Mathf.Atan2(v_0.z, v_1.z), Mathf.Atan2(v_0.w, v_1.w)); }
        public static Vector4 atan2(Vector4 v_0, float n_1) { return new Vector4(Mathf.Atan2(v_0.x, n_1), Mathf.Atan2(v_0.y, n_1), Mathf.Atan2(v_0.z, n_1), Mathf.Atan2(v_0.w, n_1)); }
        public static Vector4 ceil(Vector4 v_0) { return new Vector4(Mathf.Ceil(v_0.x), Mathf.Ceil(v_0.y), Mathf.Ceil(v_0.z), Mathf.Ceil(v_0.w)); }
        public static Vector4 floor(Vector4 v_0) { return new Vector4(Mathf.Floor(v_0.x), Mathf.Floor(v_0.y), Mathf.Floor(v_0.z), Mathf.Floor(v_0.w)); }
        public static Vector4 sqrt(Vector4 v_0) { return new Vector4(Mathf.Sqrt(v_0.x), Mathf.Sqrt(v_0.y), Mathf.Sqrt(v_0.z), Mathf.Sqrt(v_0.w)); }
        #endregion Mixed Func Vector4
        
    }
}
