// -BEGIN_PRATEEK_COPYRIGHT-
//
//  Prateek, a library that is "bien pratique"
//  Header last update date: 05/03/19
//
//  Copyright © 2017—2019 Benjamin "Touky" Huet <huet.benjamin@gmail.com>
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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine.Jobs;

#if UNITY_EDITOR
using UnityEditor;
#endif //UNITY_EDITOR

#if UNITY_PROFILING
using UnityEngine.Profiling;
#endif //UNITY_PROFILING

using Prateek;
using Prateek.Base;
using Prateek.Extensions;
using Prateek.Helpers;
using Prateek.Attributes;

#if PRATEEK_DEBUGS
using Prateek.Debug;
#endif //PRATEEK_DEBUG
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
    public static partial class Draw
    {
        //---------------------------------------------------------------------
        //Point: Three line to mark each axis
        public static void Point(Vector3 position, float size, RenderSetup? custom_setup = null)
        {
            size *= 0.5f;
            Line(position - Vector3.up * size, position + Vector3.up * size, custom_setup);
            Line(position - Vector3.forward * size, position + Vector3.forward * size, custom_setup);
            Line(position - Vector3.left * size, position + Vector3.left * size, custom_setup);
        }

        //---------------------------------------------------------------------
        //Box: A box, several flavors available
#region Box
        public static void Box(Bounds bounds, RenderSetup? custom_setup = null)
        {
            Box(bounds.center, bounds.extents, custom_setup);
        }

        //---------------------------------------------------------------------
        public static void Box(Vector3 position, float extends, RenderSetup? custom_setup = null)
        {
            Box(position, Vector3.one * extends, custom_setup);
        }

        //---------------------------------------------------------------------
        public static void Box(Vector3 position, Vector3 extends, RenderSetup? custom_setup = null)
        {
            Box(position, Quaternion.identity, extends, custom_setup);
        }

        //---------------------------------------------------------------------
        public static void Box(Vector3 position, Quaternion rotation, Vector3 extends, RenderSetup? custom_setup = null)
        {
            Vector3 x = rotation * Vector3.right * extends.x;
            Vector3 y = rotation * Vector3.up * extends.y;
            Vector3 z = rotation * Vector3.forward * extends.z;

            var posUp = position + y;
            Line(posUp - x + z, posUp + x + z, custom_setup);
            Line(posUp - x - z, posUp + x - z, custom_setup);
            Line(posUp - x - z, posUp - x + z, custom_setup);
            Line(posUp + x - z, posUp + x + z, custom_setup);

            var posDn = position - y;
            Line(posDn - x + z, posDn + x + z, custom_setup);
            Line(posDn - x - z, posDn + x - z, custom_setup);
            Line(posDn - x - z, posDn - x + z, custom_setup);
            Line(posDn + x - z, posDn + x + z, custom_setup);

            Line(posUp + x + z, posDn + x + z, custom_setup);
            Line(posUp - x - z, posDn - x - z, custom_setup);
            Line(posUp - x + z, posDn - x + z, custom_setup);
            Line(posUp + x - z, posDn + x - z, custom_setup);
        }

        //---------------------------------------------------------------------
        public static void Box(Vector3[] points, RenderSetup? custom_setup = null)
        {
            if (points.Length != 8)
                return;

            Line(points[0], points[1], custom_setup);
            Line(points[1], points[2], custom_setup);
            Line(points[2], points[3], custom_setup);
            Line(points[3], points[0], custom_setup);

            Line(points[4], points[5], custom_setup);
            Line(points[5], points[6], custom_setup);
            Line(points[6], points[7], custom_setup);
            Line(points[7], points[4], custom_setup);

            Line(points[0], points[7], custom_setup);
            Line(points[1], points[6], custom_setup);
            Line(points[2], points[5], custom_setup);
            Line(points[3], points[4], custom_setup);
        }
#endregion //Box

        //---------------------------------------------------------------------
#region Cone
        public static void Cone(Vector3 position, Quaternion rotation, float length, Vector2 radius, int sideLine = 8, int segments = 8, RenderSetup? custom_setup = null)
        {
            Vector3 fw = Vector3.forward * length;
            Vector3 rt = Vector3.right * radius.x;
            Vector3 up = Vector3.up * radius.y;

            for (int i = 0; i < sideLine; ++i)
            {
                var a = (((float)i) / (float)sideLine) * Mathf.PI * 2.0f;
                var x = Mathf.Sin(a) * rt;
                var y = Mathf.Cos(a) * up;

                Line(position, position + rotation * (fw + x + y), custom_setup);
            }
            Ellipse(position + rotation * fw, rotation, radius, segments, custom_setup);
        }
#endregion Cone

        //---------------------------------------------------------------------
        //Circle/Ellipse: Vertically aligned if drawn in world, aligned on z-axis if other
#region Circle/Ellipse/Sphere/Ellipsoid
#region Circle
        public static void Circle(Vector3 position, float radius, int segments = 8, RenderSetup? custom_setup = null)
        {
            Arc(position, Quaternion.identity, radius, 0, 360, segments, custom_setup);
        }

        //---------------------------------------------------------------------
        public static void Circle(Vector3 position, Quaternion rotation, float radius, int segments = 8, RenderSetup? custom_setup = null)
        {
            var space = custom_setup != null ? custom_setup.Value.space : m_current_setup.space;
            var spaceRotation = space != SpaceType.World ? Quaternion.identity : Quaternion.FromToRotation(Vector3.forward, Vector3.up);
            Arc(position, spaceRotation * rotation, radius, 0, 360, segments, custom_setup);
        }
#endregion //Circle

        //---------------------------------------------------------------------
#region Ellipse
        public static void Ellipse(Vector3 position, Vector2 radius, int segments = 8, RenderSetup? custom_setup = null)
        {
            Arc(position, Quaternion.identity, radius, 0, 360, segments, custom_setup);
        }

        //---------------------------------------------------------------------
        public static void Ellipse(Vector3 position, Quaternion rotation, Vector2 radius, int segments = 8, RenderSetup? custom_setup = null)
        {
            var space = custom_setup != null ? custom_setup.Value.space : m_current_setup.space;
            var spaceRotation = space != SpaceType.World ? Quaternion.identity : Quaternion.FromToRotation(Vector3.forward, Vector3.up);
            Arc(position, spaceRotation * rotation, radius, 0, 360, segments, custom_setup);
        }
#endregion //Ellipse

        //---------------------------------------------------------------------
#region Sphere
        public static void Sphere(Vector3 position, float radius, int segments = 8, RenderSetup? custom_setup = null)
        {
            Sphere(position, Quaternion.identity, radius, segments, custom_setup);
        }

        //---------------------------------------------------------------------
        public static void Sphere(Vector3 position, Quaternion rotation, float radius, int segments = 8, RenderSetup? custom_setup = null)
        {
            Ellipsoid(position, rotation, Vector3.one * radius, segments, custom_setup);
        }
#endregion //Sphere

        //---------------------------------------------------------------------
#region Ellipsoid
        public static void Ellipsoid(Vector3 position, Vector3 radius, int segments = 8, RenderSetup? custom_setup = null)
        {
            Ellipsoid(position, Quaternion.identity, radius, segments, custom_setup);
        }

        //---------------------------------------------------------------------
        public static void Ellipsoid(Vector3 position, Quaternion rotation, Vector3 radius, int segments = 8, RenderSetup? custom_setup = null)
        {
            Arc(position, rotation, radius.xy(), 0, 360, segments, custom_setup);
            Arc(position, rotation * Quaternion.Euler(90, 0, 0), radius.xz(), 0, 360, segments, custom_setup);
            Arc(position, rotation * Quaternion.Euler(0, 90, 0), radius.zy(), 0, 360, segments, custom_setup);
        }
#endregion //Ellipsoid

        //---------------------------------------------------------------------
        public static void SphereCast(Ray position, float radius, float length, int segments = 8, RenderSetup? custom_setup = null)
        {
            Sphere(position.origin, Quaternion.identity, radius, segments, custom_setup);
            Sphere(position.origin + position.direction * length, Quaternion.identity, radius, segments, custom_setup);

            Line(position.origin + Vector3.left * radius, position.origin + Vector3.left * radius + position.direction * length, custom_setup);
            Line(position.origin + Vector3.forward * radius, position.origin + Vector3.forward * radius + position.direction * length, custom_setup);
            Line(position.origin + Vector3.right * radius, position.origin + Vector3.right * radius + position.direction * length, custom_setup);
            Line(position.origin + Vector3.back * radius, position.origin + Vector3.back * radius + position.direction * length, custom_setup);
            Line(position.origin + Vector3.up * radius, position.origin + Vector3.up * radius + position.direction * length, custom_setup);
            Line(position.origin + Vector3.down * radius, position.origin + Vector3.down * radius + position.direction * length, custom_setup);
        }

#endregion //Circle/Ellipse/Sphere/Ellipsoid

        //---------------------------------------------------------------------
        //Arc: Vertically aligned if drawn in world, aligned on z-axis if other
#region Arc
        public static void Arc(Vector3 position, Quaternion rotation, float radius, float start, float end, int segments = 8, RenderSetup? custom_setup = null)
        {
            Arc(position, rotation, Vector2.one * radius, start, end, segments, custom_setup);
        }

        //---------------------------------------------------------------------
        public static void Arc(Vector3 position, Quaternion rotation, Vector2 radius, float start, float end, int segments = 8, RenderSetup? custom_setup = null)
        {
            if (segments < 1)
                return;

            float step = (end - start) / segments;
            for (uint i = 0; i < segments; i++)
            {
                uint j = i + 1;
                float angle0 = start + i * step;
                float angle1 = start + j * step;
                Vector4 p0 = new Vector3(radius.x * Mathf.Cos(angle0 * Mathf.Deg2Rad), radius.y * Mathf.Sin(angle0 * Mathf.Deg2Rad), 0.0f);
                Vector4 p1 = new Vector3(radius.x * Mathf.Cos(angle1 * Mathf.Deg2Rad), radius.y * Mathf.Sin(angle1 * Mathf.Deg2Rad), 0.0f);
                Line(position + rotation * p0, position + rotation * p1, custom_setup);
            }
        }
#endregion //Arc


        //---------------------------------------------------------------------
        public static void Arrow(Vector3 p0, Vector3 p1, Vector3 up, float width, float length, RenderSetup? custom_setup = null)
        {
            var forward = (p1 - p0).normalized;
            Vector3 right;
            Helpers.Geometry.CreateBasis(forward, up, out right, out up);

            Line(p0, p1, custom_setup);
            Line(p1, p1 - forward * length - right * width, custom_setup);
            Line(p1, p1 - forward * length + right * width, custom_setup);
            Line(p1, p1 - forward * length - up * width, custom_setup);
            Line(p1, p1 - forward * length + up * width, custom_setup);
        }

        //---------------------------------------------------------------------
        public static void Pie(Vector3 position, Quaternion rotation, float radius, float degreeStart, float degreeEnd, int segments = 8, RenderSetup? custom_setup = null)
        {
            if (segments < 1)
                return;

            if ((degreeEnd - degreeStart) >= 360)
            {
                Circle(position, rotation, radius, segments, custom_setup);
                return;
            }

            float step = (degreeEnd - degreeStart) / segments;
            for (uint i = 0; i < segments; i++)
            {
                uint j = i + 1;
                float angle0 = degreeStart + (i * step);
                float angle1 = degreeStart + (j * step);
                Vector3 p0 = new Vector3(radius * Mathf.Sin(angle0 * Mathf.Deg2Rad), 0.0f, radius * Mathf.Cos(angle0 * Mathf.Deg2Rad));
                Vector3 p1 = new Vector3(radius * Mathf.Sin(angle1 * Mathf.Deg2Rad), 0.0f, radius * Mathf.Cos(angle1 * Mathf.Deg2Rad));
                Line(position + rotation * p0, position + rotation * p1, custom_setup);

                if (i == 0)
                {
                    Line(position, position + rotation * p0, custom_setup);
                }

                if (i == segments - 1)
                {
                    Line(position, position + rotation * p1, custom_setup);
                }
            }
        }

        //---------------------------------------------------------------------
        public static void Plane(Plane plane, Vector2 offset, float size, Vector3 up, RenderSetup? custom_setup = null)
        {
            var q = Quaternion.LookRotation(plane.normal, up);
            var x = q * Vector3.right;
            var y = q * Vector3.up;

            var p = (plane.normal * -plane.distance) + (x * offset.x) + (y * offset.y);
            var sx = x * size;
            var sy = y * size;

            Line(p - sx - sy, p + sx - sy, custom_setup);
            Line(p - sx + sy, p + sx + sy, custom_setup);
            Line(p - sx - sy, p - sx + sy, custom_setup);
            Line(p + sx - sy, p + sx + sy, custom_setup);
            Arrow(p, p + plane.normal * size * 0.2f, up, size * 0.02f, size * 0.02f, custom_setup);

        }
    }
}
#endif //PRATEEK_DEBUG
