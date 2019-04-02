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
using static Prateek.Debug.Draw.Style.QuickCTor;
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
        #region Declarations
        public enum Space
        {
            World,
            CameraLocal,
            CameraRef,
            CameraViewRatio,
            CameraViewPixel,
        }

        //---------------------------------------------------------------------
        public class Scope : GUI.Scope
        {
            //-----------------------------------------------------------------
            #region Fields
            private Style setup;
            #endregion Fields

            //-----------------------------------------------------------------
            #region Properties
            public Style Setup { get { return setup; } }
            #endregion Properties

            //-----------------------------------------------------------------
            #region Scope
            protected Scope(Style setup) : base()
            {
                this.setup = setup;
                Add(this);
            }

            //-----------------------------------------------------------------
            protected override void CloseScope()
            {
                Remove(this);
            }
            #endregion Scope
        }

        //---------------------------------------------------------------------
        public partial struct Style
        {
            //-----------------------------------------------------------------
            public enum InitMode
            {
                Reset,
                UseLast,
            }

            //-----------------------------------------------------------------
            private Space space;
            private Matrix4x4 matrix;
            private Color color;
            private float duration;
            private bool depthTest;
            private int precision;

            //-----------------------------------------------------------------
            public Space Space { get { return space; } set { space = value; } }
            public Matrix4x4 Matrix { get { return matrix; } set { matrix = value; } }
            public Color Color { get { return color; } set { color = value; } }
            public float Duration { get { return duration; } set { duration = value; } }
            public bool DepthTest { get { return depthTest; } set { depthTest = value; } }
            public int Precision { get { return precision; } set { precision = value; } }

            //-----------------------------------------------------------------
            public Style(InitMode initMode)
            {
                switch (initMode)
                {
                    case InitMode.UseLast:
                    {
                        this = ActiveSetup;
                        break;
                    }
                    default:
                    {
                        space = Space.World;
                        matrix = Matrix4x4.identity;
                        color = Color.white;
                        duration = -1;
                        depthTest = false;
                        precision = 8;
                        break;
                    }
                }
            }
        }

        //---------------------------------------------------------------------
        public enum PrimitiveType
        {
            Line,
            Point,
            Box,
            Arrow,
            Plane,
            Arc,
            Circle,
            Sphere,
            SphereCast,
            Cone,
            Pie,

            MAX
        }

        //---------------------------------------------------------------------
        public struct PrimitiveSetup
        {
            //-----------------------------------------------------------------
            public PrimitiveType type;
            public Style setup;
            public Space endSpace;
            public Vector3 pos;
            public Quaternion rot;
            public Vector4 size;
            public Vector2 range;

            //-----------------------------------------------------------------
            public PrimitiveSetup(PrimitiveType type, Style setup)
            {
                this.type = type;
                this.setup = setup;
                endSpace = Space.World;
                pos = Vector3.zero;
                rot = Quaternion.identity;
                size = Vector3.one;
                range = vec2(0, 1);
            }
    }

        //---------------------------------------------------------------------
        public struct LineSetup
        {
            public Vector3 start;
            public Vector3 end;
            public Style setup;
        }
        #endregion Declarations

        //---------------------------------------------------------------------
        #region Fields
        private static List<Scope> scopes = new List<Scope>();
        #endregion Fields

        //---------------------------------------------------------------------
        #region Scopes
        public static Style ActiveSetup
        {
            get
            {
                if (scopes.Count == 0)
                    return new Style(Style.InitMode.Reset);
                return scopes.Last().Setup;
            }
        }

        //---------------------------------------------------------------------
        public static void Add(Scope scope)
        {
            scopes.Add(scope);
        }

        //---------------------------------------------------------------------
        public static void Remove(Scope scope)
        {
            scopes.Remove(scope);
        }
        #endregion Scopes

        //---------------------------------------------------------------------
        protected static void Add(PrimitiveSetup primitive)
        {
            DebugDisplayManager.Add(primitive);
        }

        //---------------------------------------------------------------------
        #region Primitives render
        public static void Render(DebugLineDisplayer d, PrimitiveSetup prim)
        {
            var wRt = prim.rot * Vector3.right;
            var wUp = prim.rot * Vector3.up;
            var wFw = prim.rot * Vector3.forward;

            //Adjust axis size if required
            switch (prim.type)
            {
                case PrimitiveType.Point:
                case PrimitiveType.Box:
                case PrimitiveType.Plane:
                case PrimitiveType.Arc:
                case PrimitiveType.Cone:
                {
                    wRt *= prim.size.x;
                    wUp *= prim.size.y;
                    wFw *= prim.size.z;
                    break;
                }
            }

            //Draw the actual primitive
            switch (prim.type)
            {
                case PrimitiveType.Line:
                {
                    d.RenderLine(prim.setup, prim.pos, prim.pos + prim.rot * prim.size);
                    break;
                }
                case PrimitiveType.Point:
                {
                    d.RenderLine(prim.setup, prim.pos - wRt, prim.pos + wRt);
                    d.RenderLine(prim.setup, prim.pos - wUp, prim.pos + wUp);
                    d.RenderLine(prim.setup, prim.pos - wFw, prim.pos + wFw);
                    break;
                }
                case PrimitiveType.Box:
                {
                    var posUp = prim.pos + wUp;
                    var posDn = prim.pos - wUp;

                    d.RenderLine(prim.setup, posUp - wRt + wFw, posUp + wRt + wFw);
                    d.RenderLine(prim.setup, posUp - wRt - wFw, posUp + wRt - wFw);
                    d.RenderLine(prim.setup, posUp - wRt - wFw, posUp - wRt + wFw);
                    d.RenderLine(prim.setup, posUp + wRt - wFw, posUp + wRt + wFw);

                    d.RenderLine(prim.setup, posDn - wRt + wFw, posDn + wRt + wFw);
                    d.RenderLine(prim.setup, posDn - wRt - wFw, posDn + wRt - wFw);
                    d.RenderLine(prim.setup, posDn - wRt - wFw, posDn - wRt + wFw);
                    d.RenderLine(prim.setup, posDn + wRt - wFw, posDn + wRt + wFw);

                    d.RenderLine(prim.setup, posUp + wRt + wFw, posDn + wRt + wFw);
                    d.RenderLine(prim.setup, posUp - wRt - wFw, posDn - wRt - wFw);
                    d.RenderLine(prim.setup, posUp - wRt + wFw, posDn - wRt + wFw);
                    d.RenderLine(prim.setup, posUp + wRt - wFw, posDn + wRt - wFw);
                    break;
                }
                case PrimitiveType.Arrow:
                {
                    var end = prim.pos + wFw * prim.size.z;
                    d.RenderLine(prim.setup, end, prim.pos);
                    d.RenderLine(prim.setup, end, end - wFw * prim.size.y - wRt * prim.size.x);
                    d.RenderLine(prim.setup, end, end - wFw * prim.size.y + wRt * prim.size.x);
                    d.RenderLine(prim.setup, end, end - wFw * prim.size.y - wUp * prim.size.x);
                    d.RenderLine(prim.setup, end, end - wFw * prim.size.y + wUp * prim.size.x);

                    var end0 = end - wFw * prim.size.y * 0.25f;
                    d.RenderLine(prim.setup, end0, end - wFw * prim.size.y - wRt * prim.size.x);
                    d.RenderLine(prim.setup, end0, end - wFw * prim.size.y + wRt * prim.size.x);
                    d.RenderLine(prim.setup, end0, end - wFw * prim.size.y - wUp * prim.size.x);
                    d.RenderLine(prim.setup, end0, end - wFw * prim.size.y + wUp * prim.size.x);
                    break;
                }
                case PrimitiveType.Plane:
                {
                    d.RenderLine(prim.setup, prim.pos - wRt - wUp, prim.pos + wRt - wUp);
                    d.RenderLine(prim.setup, prim.pos - wRt + wUp, prim.pos + wRt + wUp);
                    d.RenderLine(prim.setup, prim.pos - wRt - wUp, prim.pos - wRt + wUp);
                    d.RenderLine(prim.setup, prim.pos + wRt - wUp, prim.pos + wRt + wUp);
                    break;
                }
                case PrimitiveType.Arc:
                {
                    var segments = prim.setup.Precision;
                    var step = (prim.range.y - prim.range.x) / segments;
                    for (var i = 0; i < segments; i++)
                    {
                        var j = i + 1;
                        var angle0 = prim.range.x + i * step;
                        var angle1 = prim.range.x + j * step;
                        var p0 = wRt * Mathf.Cos(angle0 * Mathf.Deg2Rad) + wFw * Mathf.Sin(angle0 * Mathf.Deg2Rad);
                        var p1 = wRt * Mathf.Cos(angle1 * Mathf.Deg2Rad) + wFw * Mathf.Sin(angle1 * Mathf.Deg2Rad);
                        d.RenderLine(prim.setup, prim.pos + p0, prim.pos + p1);
                    }
                    break;
                }
                case PrimitiveType.Circle:
                {
                    var other = prim;
                    other.type = PrimitiveType.Arc;
                    other.range = vec2(0, 360);
                    Render(d, other);
                    break;
                }
                case PrimitiveType.Sphere:
                {
                    var other = prim;
                    other.type = PrimitiveType.Arc;
                    other.range = vec2(0, 360);
                    Render(d, other);
                    other.rot = Quaternion.LookRotation(wUp, wFw);
                    other.size = prim.size.xzy();
                    Render(d, other);
                    other.rot = Quaternion.LookRotation(wUp, wRt);
                    other.size = prim.size.zxy();
                    Render(d, other);
                    break;
                }
                case PrimitiveType.SphereCast:
                {
                    var other = prim;
                    other.type = PrimitiveType.Sphere;
                    other.size = vec3(prim.size.x);
                    Render(d, other);
                    other.pos = prim.pos + wFw * prim.size.z;
                    Render(d, other);

                    d.RenderLine(prim.setup, prim.pos - wRt * prim.size.x, prim.pos - wRt * prim.size.x + wFw * prim.size.z);
                    d.RenderLine(prim.setup, prim.pos + wRt * prim.size.x, prim.pos + wRt * prim.size.x + wFw * prim.size.z);
                    d.RenderLine(prim.setup, prim.pos + wUp * prim.size.y, prim.pos + wUp * prim.size.y + wFw * prim.size.z);
                    d.RenderLine(prim.setup, prim.pos - wUp * prim.size.y, prim.pos - wUp * prim.size.y + wFw * prim.size.z);

                    d.RenderLine(prim.setup, prim.pos + wFw * prim.size.x, prim.pos + wFw * (prim.size.z - prim.size.x));
                    break;
                }
                case PrimitiveType.Cone:
                {
                    for (int i = 0; i < prim.setup.Precision; ++i)
                    {
                        var a = ((float)i / (float)prim.setup.Precision) * Mathf.PI * 2.0f;
                        var x = Mathf.Sin(a) * wRt;
                        var y = Mathf.Cos(a) * wUp;

                        d.RenderLine(prim.setup, prim.pos, prim.pos + wFw + x + y); 
                    }

                    var other = prim;
                    other.type = PrimitiveType.Arc;
                    other.range = vec2(0, 360);
                    other.pos = prim.pos + wFw;
                    other.rot = Quaternion.LookRotation(wUp, wFw);
                    Render(d, other);
                    break;
                }
                case PrimitiveType.Pie:
                {
                    var pieDiff = abs(prim.range.y - prim.range.x);
                    if (pieDiff >= 360 && prim.size.y == 0)
                    {
                        var other = prim;
                        other.type = PrimitiveType.Arc;
                        other.range = vec2(0, 360);
                        Render(d, other);
                        break;
                    }

                    //Draw pie arcs
                    {
                        var other = prim;
                        other.type = PrimitiveType.Arc;
                        other.pos -= wUp * prim.size.y;
                        Render(d, other);
                        other.pos += wUp * prim.size.y * 2f;
                        Render(d, other);
                    }

                    //Center line
                    d.RenderLine(prim.setup, prim.pos - wUp * prim.size.y, prim.pos + wUp * prim.size.y);


                    //Draw sides and inner lines
                    var segments = prim.setup.Precision;
                    var step = (prim.range.y - prim.range.x) / segments;
                    for (var i = 0; i < segments; i++)
                    {
                        var j = i + 1;
                        var angle = prim.range.x + i * step;
                        var p = wRt * Mathf.Cos(angle * Mathf.Deg2Rad) + wFw * Mathf.Sin(angle * Mathf.Deg2Rad);
                        d.RenderLine(prim.setup, prim.pos + p - wUp * prim.size.y, prim.pos + p + wUp * prim.size.y);

                        if (i == 0 || i == segments - 1)
                        {
                            d.RenderLine(prim.setup, prim.pos - wUp * prim.size.y, prim.pos + p - wUp * prim.size.y);
                            d.RenderLine(prim.setup, prim.pos + wUp * prim.size.y, prim.pos + p + wUp * prim.size.y);
                        }
                    }
                    break;
                }
            }
        }
        #endregion Primitives render

        //---------------------------------------------------------------------
        //public static void EndFrame()
        //{
        //    if (!FrameRecorder.PlaybackActive || FrameRecorder.FrameCount == 0)
        //    {
        //        for (int i = 0; i < screenSpaceLines.Count; ++i)
        //        {
        //            var data = screenSpaceLines[i];
        //            DelayedLine(ref data);
        //        }
        //        screenSpaceLines.Clear();
        //    }

        //    FrameRecorder.EndFrame();
        //    if (FrameRecorder.PlaybackActive)
        //    {
        //        var currentFrame = FrameRecorder.CurrentFrame;
        //        for (int i = 0; i < currentFrame.m_lines.Count; ++i)
        //        {
        //            var data = currentFrame.m_lines[i];
        //            DelayedLine(ref data);
        //        }
        //    }
        //}

        //---------------------------------------------------------------------
        #region Line operations
        //        private static void DelayedLine(ref LineSetup newData, bool useDuration = true)
        //        {
        //            var camera = UnityEngine.Camera.current;
        //            var data = newData;
        //            Matrix4x4 mx = data.setup.Matrix;
        //            switch (data.setup.Space)
        //            {
        //                case Space.CameraLocal: { mx = camera.transform.localToWorldMatrix * mx; break; }
        //                case Space.CameraRef: { mx = camera.cameraToWorldMatrix * mx; break; }
        //                case Space.CameraViewRatio:
        //                {
        //                    var mxInv = camera.localToCameraMatrix().inverse;
        //                    var scale = (float)camera.pixelHeight / (float)camera.pixelWidth;
        //                    var vScale = new Vector3(scale, 1, -1);
        //                    data.start = mxInv.MultiplyPoint(Vector3.Scale(data.start, vScale));
        //                    data.end = mxInv.MultiplyPoint(Vector3.Scale(data.end, vScale));
        //                    //TODO BHU -------------------------------
        //                    data.start.z = Mathf.Max(-data.start.z, .01f /*MainManager.Instance.Objects.MainCamera.nearClipPlane*/);
        //                    data.end.z = Mathf.Max(-data.end.z, 10000f /*MainManager.Instance.Objects.MainCamera.nearClipPlane*/);
        //                    mx = camera.transform.localToWorldMatrix * mx;
        //                    break;
        //                }
        //                case Space.CameraViewPixel:
        //                {
        //                    var mxInv = camera.localToCameraMatrix().inverse;
        //                    var vScale = new Vector3(1.0f / (float)camera.pixelWidth, 1.0f / (float)camera.pixelHeight, -1);
        //                    data.start = mxInv.MultiplyPoint(Vector3.Scale(data.start, vScale));
        //                    data.end = mxInv.MultiplyPoint(Vector3.Scale(data.end, vScale));
        //                    data.start.z = Mathf.Max(-data.start.z, .01f /*MainManager.Instance.Objects.MainCamera.nearClipPlane*/);
        //                    data.end.z = Mathf.Max(-data.end.z, 10000f /*MainManager.Instance.Objects.MainCamera.nearClipPlane*/);
        //                    mx = camera.transform.localToWorldMatrix * mx;
        //                    break;
        //                }
        //            }

        //            data.start = mx.MultiplyPoint(data.start);
        //            data.end = mx.MultiplyPoint(data.end);

        //            data.setup.Space = Space.World;

        //            FrameRecorder.AddLine(data);

        //            data.setup.Duration = useDuration ? data.setup.Duration : -1f;
        //            ImmediateLine(ref data);

        //        }

        //        //-----------------------------------------------------------
        //#if UNITY_EDITOR
        //        private static void ImmediateLine(ref LineSetup data)
        //        {
        //            UnityEngine.Debug.DrawLine(data.start, data.end, data.setup.Color, data.setup.Duration, data.setup.DepthTest);
        //        }
        //#else //UNITY_EDITOR
        //        private static void ImmediateLine(ref LineSetup data)
        //        {
        //            var line = MainManager.Instance.DebugDisplayManager.GetLine();
        //            line.SetColor(data.setup.Color);
        //            line.SetDebugLine(data.setup.start, data.setup.end);
        //        }
        //#endif //UNITY_EDITOR
        #endregion Line operations

        //---------------------------------------------------------------------
        #region Base primitives
        //public static void Line(Vector3 start, Vector3 end, Setup? custom_setup = null)
        //{
        //    var localSetup = new Setup(custom_setup != null ? custom_setup.Value.Space : currentSetup.Space);
        //    localSetup.Color = currentSetup.Color;
        //    localSetup.Duration = currentSetup.Duration;
        //    localSetup.DepthTest = currentSetup.DepthTest;
        //    localSetup.Matrix = currentSetup.Matrix;

        //    if (custom_setup != null)
        //    {
        //        localSetup.Color = custom_setup.Value.Color;
        //        localSetup.Duration = custom_setup.Value.Duration;
        //        localSetup.DepthTest = custom_setup.Value.DepthTest;
        //        localSetup.Matrix = custom_setup.Value.Matrix;
        //    }

        //    //Delay screen-space lines to end of frame 
        //    if (localSetup.Space != Space.World)
        //    {
        //        screenSpaceLines.Add(new LineSetup() { start = start, end = end, setup = localSetup });
        //    }
        //    else
        //    {
        //        if (FrameRecorder.PlaybackActive)
        //            return;

        //        FrameRecorder.AddLine(new LineSetup() { start = start, end = end, setup = localSetup });

        //        start = localSetup.Matrix.MultiplyPoint(start);
        //        end = localSetup.Matrix.MultiplyPoint(end);

        //        var line = new LineSetup() { start = start, end = end, setup = localSetup };
        //        ImmediateLine(ref line);
        //    }
        //}
        #endregion //Base primitives
    }
}
#endif //PRATEEK_DEBUG
