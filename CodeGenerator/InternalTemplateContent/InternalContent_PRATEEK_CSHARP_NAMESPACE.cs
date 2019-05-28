
//
//-----------------------------------------------------------------------------
#region C# Prateek Namespaces

//Auto activate some of the prateek defines
#if UNITY_EDITOR

#if !PRATEEK_DEBUG
#define PRATEEK_DEBUG
#endif //!PRATEEK_DEBUG

#endif //UNITY_EDITOR && !PRATEEK_DEBUG

//-----------------------------------------------------------------------------
#region System
using System;
using System.Collections;
using System.Collections.Generic;
#endregion System

//-----------------------------------------------------------------------------
#region Unity
using Unity.Jobs;
using Unity.Collections;

//using Unity.Mathematics;
//using static Unity.Mathematics.math;
//using vec2 = Unity.Mathematics.float2;
//using vec3 = Unity.Mathematics.float3;
//using vec4 = Unity.Mathematics.float4;
//using ivec2 = Unity.Mathematics.int2;
//using ivec3 = Unity.Mathematics.int3;
//using ivec4 = Unity.Mathematics.int4;
//using bvec2 = Unity.Mathematics.bool2;
//using bvec3 = Unity.Mathematics.bool3;
//using bvec4 = Unity.Mathematics.bool4;
//using mat2 = Unity.Mathematics.float2x2;
//using mat3 = Unity.Mathematics.float3x3;
//using mat4 = Unity.Mathematics.float4x4;

//-----------------------------------------------------------------------------
#region Engine
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.Serialization;

//-----------------------------------------------------------------------------
#if UNITY_PROFILING
using UnityEngine.Profiling;
#endif //UNITY_PROFILING

#endregion Engine

//-----------------------------------------------------------------------------
#if UNITY_EDITOR
using UnityEditor;
#endif //UNITY_EDITOR

#endregion Unity

//-----------------------------------------------------------------------------
#region Prateek
using Prateek;
using Prateek.Base;
using Prateek.Extensions;
using Prateek.Helpers;
using Prateek.Attributes;
using Prateek.Manager;

//-----------------------------------------------------------------------------
#region Using static
using static Prateek.ShaderTo.CSharp;
#endregion Using static

//-----------------------------------------------------------------------------
#if UNITY_EDITOR
using Prateek.CodeGeneration;
#endif //UNITY_EDITOR

//-----------------------------------------------------------------------------
#if PRATEEK_DEBUG
using Prateek.Debug;
using static Prateek.Debug.DebugDraw.DebugStyle.QuickCTor;
using DebugDraw = Prateek.Debug.DebugDraw;
using DebugPlace = Prateek.Debug.DebugDraw.DebugPlace;
using DebugStyle = Prateek.Debug.DebugDraw.DebugStyle;
#endif //PRATEEK_DEBUG

#endregion Prateek

#endregion C# Prateek Namespaces
//
