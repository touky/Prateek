// -BEGIN_PRATEEK_COPYRIGHT-
//
//  Prateek, a library that is "bien pratique"
//  Header last update date: 30/03/2019
//
//  Copyright � 2017-2019 "Touky" <touky@prateek.top>
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
using Prateek.Manager;

#region Using static
using static Prateek.ShaderTo.CSharp;
#endregion Using static

#region Editor
#if UNITY_EDITOR
using Prateek.CodeGeneration;
#endif //UNITY_EDITOR
#endregion Editor

#if PRATEEK_DEBUG
using Prateek.Debug;
using static Prateek.Debug.DebugDraw.DebugStyle.QuickCTor;
#endif //PRATEEK_DEBUG
#endregion Prateek

#endregion C# Prateek Namespaces
//
// -END_PRATEEK_CSHARP_NAMESPACE-

//-----------------------------------------------------------------------------
#region File namespaces
#endregion File namespaces

//-----------------------------------------------------------------------------
#if PRATEEK_DEBUG
namespace Prateek.Debug
{
    //-------------------------------------------------------------------------
    public partial class DebugDraw
    {
        //---------------------------------------------------------------------
        //Point: Three line to mark each axis
        private static void Line(Vector3 start, Vector3 end, DebugStyle setup)
        {
            var dir = end - start;
            var prim = new PrimitiveSetup(PrimitiveType.Line, setup);
            prim.pos = start;
            prim.rot = Quaternion.LookRotation(dir.normalized);
            prim.extents = vec3(0, 0, dir.magnitude);
            Add(prim);
        }

        //---------------------------------------------------------------------
        #region Standard Primitives
        //Point: Three line to mark each axis
        public static void Line(DebugPlace place) { Line(place, ActiveSetup); }
        public static void Line(DebugPlace place, DebugStyle setup)
        {
            var prim = new PrimitiveSetup(PrimitiveType.Line, setup);
            prim.pos = place.Start;
            prim.rot = place.Rotation;
            prim.extents = place.Size;
            Add(prim);
        }

        //---------------------------------------------------------------------
        //Point: Three line to mark each axis
        public static void Point(DebugPlace place) { Point(place, ActiveSetup); }
        public static void Point(DebugPlace place, DebugStyle setup)
        {
            var prim = new PrimitiveSetup(PrimitiveType.Point, setup);
            prim.pos = place.Position;
            prim.rot = Quaternion.identity;
            prim.extents = place.Extents;
            Add(prim);
        }

        //---------------------------------------------------------------------
        //Box: A box, several flavors available
        public static void Box(DebugPlace place) { Box(place, ActiveSetup); }
        public static void Box(DebugPlace place, DebugStyle setup)
        {
            var prim = new PrimitiveSetup(PrimitiveType.Box, setup);
            prim.pos = place.Position;
            prim.rot = place.Rotation;
            prim.extents = place.Extents;
            Add(prim);
        }

        //---------------------------------------------------------------------
        //Arc: Vertically aligned if drawn in world, aligned on z-axis if other
        public static void Arc(DebugPlace place, Vector2 degrees) { Arc(place, degrees, ActiveSetup); }
        public static void Arc(DebugPlace place, Vector2 degrees, DebugStyle setup)
        {
            var prim = new PrimitiveSetup(PrimitiveType.Arc, setup);
            prim.pos = place.Position;
            prim.rot = place.Rotation;
            prim.extents = place.Extents;
            prim.range = degrees;
            Add(prim);
        }

        //---------------------------------------------------------------------
        //Circle/Ellipse: Vertically aligned if drawn in world, aligned on z-axis if other
        public static void Circle(DebugPlace place) { Circle(place, ActiveSetup); }
        public static void Circle(DebugPlace place, DebugStyle setup)
        {
            var prim = new PrimitiveSetup(PrimitiveType.Circle, setup);
            prim.pos = place.Position;
            prim.rot = place.Rotation;
            prim.extents = place.Extents;
            Add(prim);
        }

        //---------------------------------------------------------------------
        public static void Sphere(DebugPlace place) { Sphere(place, ActiveSetup); }
        public static void Sphere(DebugPlace place, DebugStyle setup)
        {
            var prim = new PrimitiveSetup(PrimitiveType.Sphere, setup);
            prim.pos = place.Position;
            prim.rot = place.Rotation;
            prim.extents = place.Extents;
            Add(prim);
        }
        #endregion Standard Primitives

        //---------------------------------------------------------------------
        #region Complex Primitives
        public static void Cone(DebugPlace place) { Cone(place, ActiveSetup); }
        public static void Cone(DebugPlace place, DebugStyle setup)
        {
            var prim = new PrimitiveSetup(PrimitiveType.SphereCast, setup);
            prim.pos = place.Position;
            prim.rot = place.Rotation;
            prim.extents = place.Extents;
            Add(prim);
        }

        //---------------------------------------------------------------------
        public static void Pie(DebugPlace place, Vector2 degrees) { Pie(place, degrees, ActiveSetup); }
        public static void Pie(DebugPlace place, Vector2 degrees, DebugStyle setup)
        {
            var prim = new PrimitiveSetup(PrimitiveType.SphereCast, setup);
            prim.pos = place.Position;
            prim.rot = place.Rotation;
            prim.extents = place.Extents;
            prim.range = degrees;
            Add(prim);
        }

        //---------------------------------------------------------------------
        public static void Arrow(DebugPlace place) { Arrow(place, ActiveSetup); }
        public static void Arrow(DebugPlace place, DebugStyle setup)
        {
            var prim = new PrimitiveSetup(PrimitiveType.Arrow, setup);
            prim.pos = place.Start;
            prim.rot = place.Rotation;
            prim.extents = vec3(place.Extents.x, place.Extents.y, place.Size.z);
            Add(prim);
        }
        #endregion Complex Primitives

        //---------------------------------------------------------------------
        #region Custom Primitives
        public static void Box(Vector3[] points) { Box(points, ActiveSetup); }
        public static void Box(Vector3[] points, DebugStyle setup)
        {
            if (points.Length != 8)
                return;

            Line(points[0], points[1], setup);
            Line(points[1], points[2], setup);
            Line(points[2], points[3], setup);
            Line(points[3], points[0], setup);

            Line(points[4], points[5], setup);
            Line(points[5], points[6], setup);
            Line(points[6], points[7], setup);
            Line(points[7], points[4], setup);

            Line(points[0], points[7], setup);
            Line(points[1], points[6], setup);
            Line(points[2], points[5], setup);
            Line(points[3], points[4], setup);
        }

        //---------------------------------------------------------------------
        public static void Plane(Plane plane, DebugPlace place) { Plane(plane, place, ActiveSetup); }
        public static void Plane(Plane plane, DebugPlace place, DebugStyle setup)
        {
            var q = place.Rotation;
            var x = place.Right;
            var y = place.Up;
            var p = (plane.normal * -plane.distance) + (x * place.Position.x) + (y * place.Position.y);

            var prim = new PrimitiveSetup(PrimitiveType.Plane, setup);
            prim.pos = p;
            prim.rot = q;
            prim.extents = place.Size;
            Add(prim);

            Arrow(place, setup);
        }

        //---------------------------------------------------------------------
        public static void SphereCast(Ray ray, float radius, float distance) { SphereCast(ray, radius, distance, ActiveSetup); }
        public static void SphereCast(Ray ray, float radius, float distance, DebugStyle setup)
        {
            var prim = new PrimitiveSetup(PrimitiveType.SphereCast, setup);
            prim.pos = ray.origin;
            prim.rot = Quaternion.LookRotation(ray.direction);
            prim.extents = vec3(radius, radius, distance);
            Add(prim);
        }
        #endregion Custom Primitives
    }
}
#endif //PRATEEK_DEBUG
