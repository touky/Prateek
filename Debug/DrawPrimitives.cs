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
using static Prateek.Debug.Draw.Setup.QuickCTor;
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
    public partial class Draw
    {
        //---------------------------------------------------------------------
        //Point: Three line to mark each axis
        ////public static void Line(Vector3 start, Vector3 dir, float distance) { Line(start, dir, distance, ActiveSetup); }
        ////public static void Line(Vector3 start, Vector3 dir, float distance, Setup setup) { Line(start, start + dir * distance, setup); }
        ////public static void Line(Vector3 start, Vector3 end) { Line(start, end, ActiveSetup); }
        public static void Line(Vector3 start, Vector3 end, Setup setup)
        {
            var dir = end - start;
            var prim = new PrimitiveSetup(PrimitiveType.Line, setup);
            prim.pos = start;
            prim.rot = Quaternion.LookRotation(dir.normalized);
            prim.size = vec3(0, 0, dir.magnitude);
            Add(prim);
        }

        //---------------------------------------------------------------------
        //Point: Three line to mark each axis
        public static void Point(Vector3 position, float size, Setup setup)
        {
            var prim = new PrimitiveSetup(PrimitiveType.Point, setup);
            prim.pos = position;
            prim.rot = Quaternion.identity;
            prim.size = vec3(size * 0.5f);
            Add(prim);
        }

        //---------------------------------------------------------------------
        //Box: A box, several flavors available
        #region Box
        ////public static void Box(Bounds bounds, Setup setup)
        ////{
        ////    Box(bounds.center, bounds.extents, setup);
        ////}

        //////-----------------------------------------------------------------
        ////public static void Box(Vector3 position, float extends, Setup setup)
        ////{
        ////    Box(position, Vector3.one * extends, setup);
        ////}

        //////-----------------------------------------------------------------
        ////public static void Box(Vector3 position, Vector3 extends, Setup setup)
        ////{
        ////    Box(position, Quaternion.identity, extends, setup);
        ////}

        //---------------------------------------------------------------------
        public static void Box(Vector3 position, Quaternion rotation, Vector3 size, Setup setup)
        {
            var prim = new PrimitiveSetup(PrimitiveType.Box, setup);
            prim.pos = position;
            prim.rot = rotation;
            prim.size = size * 0.5f;
            Add(prim);
        }

        //---------------------------------------------------------------------
        public static void Box(Vector3[] points, Setup setup)
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
        #endregion //Box

        //---------------------------------------------------------------------
        #region Arrow
        public static void Arrow(Vector3 start, Vector3 end, Vector3 up, float width, float length, Setup setup)
        {
            var dir = end - start;
            var prim = new PrimitiveSetup(PrimitiveType.Arrow, setup);
            prim.pos = start;
            prim.rot = Quaternion.LookRotation(dir, up);
            prim.size = vec3(width, length, dir.magnitude);
            Add(prim);
        }
        #endregion Arrow

        //---------------------------------------------------------------------
        #region Plane
        public static void Plane(Plane plane, Vector2 offset, float size, Vector3 up, Setup setup)
        {
            var q = Quaternion.LookRotation(plane.normal, up);
            var x = q * Vector3.right;
            var y = q * Vector3.up;
            var p = (plane.normal * -plane.distance) + (x * offset.x) + (y * offset.y);

            var prim = new PrimitiveSetup(PrimitiveType.Plane, setup);
            prim.pos = p;
            prim.rot = q;
            prim.size = vec3(size);
            Add(prim);

            Arrow(p, p + plane.normal * size * 0.2f, up, size * 0.02f, size * 0.02f, setup);
        }
        #endregion Plane

        //---------------------------------------------------------------------
        //Arc: Vertically aligned if drawn in world, aligned on z-axis if other
        #region Arc
        ////public static void Arc(Vector3 position, Quaternion rotation, float radius, float start, float end, int segments = 8, Setup setup)
        ////{
        ////    Arc(position, rotation, Vector2.one * radius, start, end, segments, setup);
        ////}

        //---------------------------------------------------------------------
        public static void Arc(Vector3 position, Quaternion rotation, Vector2 radius, Vector2 degrees, Setup setup)
        {
            var prim = new PrimitiveSetup(PrimitiveType.Arc, setup);
            prim.pos = position;
            prim.rot = rotation;
            prim.size = radius.xny();
            prim.range = degrees;
            Add(prim);
        }
        #endregion //Arc

        //---------------------------------------------------------------------
        //Circle/Ellipse: Vertically aligned if drawn in world, aligned on z-axis if other
        #region Circle/Ellipse/Sphere/Ellipsoid
        #region Circle
        ////public static void Circle(Vector3 position, float radius, int segments = 8, Setup setup)
        ////{
        ////    Arc(position, Quaternion.identity, radius, 0, 360, segments, setup);
        ////}

        //---------------------------------------------------------------------
        public static void Circle(Vector3 position, Quaternion rotation, float radius, Setup setup)
        {
            var prim = new PrimitiveSetup(PrimitiveType.Circle, setup);
            prim.pos = position;
            prim.rot = rotation;
            prim.size = vec3(radius);
            Add(prim);
        }
        #endregion //Circle

        //---------------------------------------------------------------------
        #region Sphere
        ////public static void Sphere(Vector3 position, float radius, int segments = 8, Setup setup)
        ////{
        ////    Sphere(position, Quaternion.identity, radius, segments, setup);
        ////}

        //---------------------------------------------------------------------
        public static void Sphere(Vector3 position, Quaternion rotation, float radius, Setup setup)
        {
            var prim = new PrimitiveSetup(PrimitiveType.Sphere, setup);
            prim.pos = position;
            prim.rot = rotation;
            prim.size = vec3(radius);
            Add(prim);
        }
        #endregion //Sphere

        //---------------------------------------------------------------------
        public static void SphereCast(Ray ray, float radius, float distance, Setup setup)
        {
            var prim = new PrimitiveSetup(PrimitiveType.SphereCast, setup);
            prim.pos = ray.origin;
            prim.rot = Quaternion.LookRotation(ray.direction);
            prim.size = vec3(radius, radius, distance);
            Add(prim);
        }

        #endregion //Circle/Ellipse/Sphere/Ellipsoid

        //---------------------------------------------------------------------
        #region Cone
        public static void Cone(Vector3 position, Quaternion rotation, float length, Vector2 radius, Setup setup)
        {
            var prim = new PrimitiveSetup(PrimitiveType.SphereCast, setup);
            prim.pos = position;
            prim.rot = rotation;
            prim.size = vec3(radius, length);
            Add(prim);
        }
        #endregion Cone

        //---------------------------------------------------------------------
        #region Pie
        public static void Pie(Vector3 position, Quaternion rotation, float radius, float thickness, Vector2 degrees, Setup setup)
        {
            var prim = new PrimitiveSetup(PrimitiveType.SphereCast, setup);
            prim.pos = position;
            prim.rot = rotation;
            prim.size = vec3(radius, thickness, radius);
            prim.range = degrees;
            Add(prim);
        }
        #endregion Pie
    }
}
#endif //PRATEEK_DEBUG
