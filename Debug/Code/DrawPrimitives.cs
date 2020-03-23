// -BEGIN_PRATEEK_COPYRIGHT-
//
//  Prateek, a library that is "bien pratique"
//  Header last update date: 22/03/2020
//
//  Copyright � 2017-2020 "Touky" <touky@prateek.top>
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

//-----------------------------------------------------------------------------
#if PRATEEK_DEBUG
namespace Prateek.Debug.Code
{
    using Prateek.Core.Code;
    using UnityEngine;

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
            prim.extents = CSharp.vec3(0, 0, dir.magnitude);
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

        //---------------------------------------------------------------------
        public static void Capsule(DebugPlace place) { Capsule(place, ActiveSetup); }
        public static void Capsule(DebugPlace place, DebugStyle setup)
        {
            var prim = new PrimitiveSetup(PrimitiveType.Capsule, setup);
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
            var prim = new PrimitiveSetup(PrimitiveType.Cone, setup);
            prim.pos = place.Position;
            prim.rot = place.Rotation;
            prim.extents = place.Extents;
            Add(prim);
        }

        //---------------------------------------------------------------------
        public static void Pie(DebugPlace place, Vector2 degrees) { Pie(place, degrees, ActiveSetup); }
        public static void Pie(DebugPlace place, Vector2 degrees, DebugStyle setup)
        {
            var prim = new PrimitiveSetup(PrimitiveType.Pie, setup);
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
            prim.extents = CSharp.vec3(place.Extents.x, place.Extents.y, place.Size.z);
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
        public static void Light(Light light) { var setup = ActiveSetup; setup.Color = light.color; Light(light, setup); }
        public static void Light(Light light, DebugStyle setup)
        {
            var tr = light.transform;
            if (light.type == LightType.Point)
            {
                Sphere(DebugPlace.At(tr.position, tr.rotation, light.range), setup);
            }
            else if (light.type == LightType.Spot)
            {
                var t = Mathf.Tan(light.spotAngle) * light.range;
                Cone(DebugPlace.AToB(tr.position, tr.rotation * CSharp.vec3(0, 0, light.range), CSharp.vec3(t, t, 0), tr.rotation * Vector3.up), setup);
            }
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
        public static void LineCast(Ray ray, float radius, float distance) { SphereCast(ray, radius, distance, ActiveSetup); }
        public static void LineCast(Ray ray, float radius, float distance, DebugStyle setup)
        {
            var place = DebugPlace.AToB(ray.origin, ray.origin + ray.direction * distance);
            var prim = new PrimitiveSetup(PrimitiveType.LineCast, setup);
            prim.pos = place.Position;
            prim.rot = place.Rotation;
            prim.extents = CSharp.vec3(radius, radius, place.Extents.z);
            Add(prim);
        }

        //---------------------------------------------------------------------
        public static void LineCastList(RaycastHit[] hits, Ray[] ray, float radius, float distance, bool doLoop = false) { LineCastList(hits, ray, radius, distance, ActiveSetup, doLoop); }
        public static void LineCastList(RaycastHit[] hits, Ray[] rays, float radius, float distance, DebugStyle setup, bool doLoop = false)
        {
            bool hasHit = false;
            for (int h = 0; h < hits.Length; h++)
            {
                if (hits[h].transform != null)
                {
                    hasHit = true;
                    break;
                }
            }

            if (!hasHit)
            {
                LineCastList(rays, radius, distance, setup, doLoop);
                return;
            }

            var greySetup = setup;
            greySetup.Color = Color.grey;
            for (int h = 0; h < hits.Length; h++)
            {
                LineCast(hits[h], rays[h], radius, distance, setup);

                if (doLoop || h < hits.Length - 1)
                {
                    var h1 = (h + 1) % hits.Length;

                    var d0 = hits[h].transform != null ? hits[h].distance : distance;
                    var d1 = hits[h1].transform != null ? hits[h1].distance : distance;
                    Line(DebugPlace.AToB(rays[h].origin + rays[h].direction * d0, rays[h1].origin + rays[h1].direction * d1), setup);

                    if (hits[h].transform != null || hits[h1].transform != null)
                    {
                        Line(DebugPlace.AToB(rays[h].origin + rays[h].direction * distance, rays[h1].origin + rays[h1].direction * distance), greySetup);
                    }
                }
            }
        }

        //---------------------------------------------------------------------
        public static void LineCastList(Ray[] ray, float radius, float distance, bool doLoop = false) { LineCastList(ray, radius, distance, ActiveSetup, doLoop); }
        public static void LineCastList(Ray[] rays, float radius, float distance, DebugStyle setup, bool doLoop = false)
        {
            for (int h = 0; h < rays.Length; h++)
            {
                LineCast(rays[h], radius, distance, setup);

                if (doLoop || h < rays.Length - 1)
                {
                    var h1 = (h + 1) % rays.Length;
                    Line(DebugPlace.AToB(rays[h].origin + rays[h].direction * distance, rays[h1].origin + rays[h1].direction * distance), setup);
                }
            }
        }

        //---------------------------------------------------------------------
        public static void LineCast(RaycastHit hit, Ray ray, float radius, float distance) { LineCast(hit, ray, radius, distance, ActiveSetup); }
        public static void LineCast(RaycastHit hit, Ray ray, float radius, float distance, DebugStyle setup)
        {
            if (hit.transform == null)
            {
                LineCast(ray, radius, distance, setup);
                return;
            }

            {
                var prim = new PrimitiveSetup(PrimitiveType.Point, setup);
                prim.pos = hit.point;
                prim.rot = Quaternion.LookRotation(hit.normal);
                prim.extents = CSharp.vec3(radius * 2);
                Add(prim);
            }

            var place0 = DebugPlace.AToB(ray.origin, ray.origin + ray.direction * hit.distance);
            {
                var prim = new PrimitiveSetup(PrimitiveType.LineCast, setup);
                prim.pos = place0.Position;
                prim.rot = place0.Rotation;
                prim.extents = CSharp.vec3(radius, radius, place0.Extents.z);
                Add(prim);
            }

            var place1 = DebugPlace.AToB(place0.End, ray.origin + ray.direction * distance);
            {
                setup.Color = Color.grey;
                var prim = new PrimitiveSetup(PrimitiveType.LineCast, setup);
                prim.pos = place1.Position;
                prim.rot = place1.Rotation;
                prim.extents = CSharp.vec3(radius * 0.9f, radius * 0.9f, place1.Extents.z);
                Add(prim);
            }
        }

        //---------------------------------------------------------------------
        public static void SphereCast(Ray ray, float radius, float distance) { SphereCast(ray, radius, distance, ActiveSetup); }
        public static void SphereCast(Ray ray, float radius, float distance, DebugStyle setup)
        {
            var place = DebugPlace.AToB(ray.origin, ray.origin + ray.direction * distance);
            var prim = new PrimitiveSetup(PrimitiveType.SphereCast, setup);
            prim.pos = place.Position;
            prim.rot = place.Rotation;
            prim.extents = CSharp.vec3(radius, radius, place.Extents.z);
            Add(prim);
        }

        //---------------------------------------------------------------------
        public static void SphereCast(RaycastHit hit, Ray ray, float radius, float distance) { SphereCast(hit, ray, radius, distance, ActiveSetup); }
        public static void SphereCast(RaycastHit hit, Ray ray, float radius, float distance, DebugStyle setup)
        {
            {
                var prim = new PrimitiveSetup(PrimitiveType.Point, setup);
                prim.pos = hit.point;
                prim.rot = Quaternion.LookRotation(hit.normal);
                prim.extents = CSharp.vec3(radius * 0.4f);
                Add(prim);
            }

            var place0 = DebugPlace.AToB(ray.origin, ray.origin + ray.direction * (hit.distance + radius));
            {
                var prim = new PrimitiveSetup(PrimitiveType.SphereCast, setup);
                prim.pos = place0.Position;
                prim.rot = place0.Rotation;
                prim.extents = CSharp.vec3(radius, radius, place0.Extents.z);
                Add(prim);
            }

            var place1 = DebugPlace.AToB(place0.End - ray.direction * radius * 1.9f, ray.origin + ray.direction * distance);
            {
                setup.Color = Color.grey;
                var prim = new PrimitiveSetup(PrimitiveType.SphereCast, setup);
                prim.pos = place1.Position;
                prim.rot = place1.Rotation;
                prim.extents = CSharp.vec3(radius * 0.9f, radius * 0.9f, place1.Extents.z);
                Add(prim);
            }
        }
        #endregion Custom Primitives
    }
}
#endif //PRATEEK_DEBUG
