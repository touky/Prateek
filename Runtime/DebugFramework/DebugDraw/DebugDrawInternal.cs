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
namespace Prateek.Runtime.DebugFramework
{
    using System.Collections.Generic;
    using Prateek.Runtime.Core.Extensions;
    using UnityEngine;
    using static Prateek.Runtime.Core.Extensions.Statics;

    ///-------------------------------------------------------------------------
    internal class DebugDrawInternal
    {
        ///---------------------------------------------------------------------
        internal struct LineSetup
        {
            public Vector3 start;
            public Vector3 end;
            public DebugStyle setup;
        }

        ///---------------------------------------------------------------------
        #region Fields
        private static List<DebugScope> scopes = new List<DebugScope>();
        #endregion Fields

        ///---------------------------------------------------------------------
        #region Scopes
        internal static DebugStyle ActiveSetup
        {
            get
            {
                if (scopes.Count == 0)
                    return new DebugStyle(DebugStyle.InitMode.Reset);
                return scopes.Last().Setup;
            }
        }

        ///---------------------------------------------------------------------
        internal static void Add(DebugScope scope)
        {
            scopes.Add(scope);
        }

        ///---------------------------------------------------------------------
        internal static void Remove(DebugScope scope)
        {
            scopes.Remove(scope);
        }
        #endregion Scopes

        ///---------------------------------------------------------------------
        protected static void Add(DebugPrimitiveSetup debugPrimitive)
        {
            DebugDisplayRegistry.Add(debugPrimitive);
        }

        ///---------------------------------------------------------------------
        #region Primitives render
        internal static void Render(DebugLineDisplayer d, DebugPrimitiveSetup prim)
        {
            var pos = prim.pos;
            var wRt = prim.rot * Vector3.right;
            var wUp = prim.rot * Vector3.up;
            var wFw = prim.rot * Vector3.forward;

            pos = prim.setup.Matrix.MultiplyPoint(pos);
            wFw = prim.setup.Matrix.MultiplyPoint(prim.pos + wFw) - pos;
            wRt = prim.setup.Matrix.MultiplyPoint(prim.pos + wRt) - pos;
            wUp = prim.setup.Matrix.MultiplyPoint(prim.pos + wUp) - pos;
            prim.setup.Matrix = Matrix4x4.identity;

            //Adjust axis size if required
            switch (prim.type)
            {
                case DebugPrimitiveType.Point:
                case DebugPrimitiveType.Box:
                case DebugPrimitiveType.Plane:
                case DebugPrimitiveType.Arc:
                case DebugPrimitiveType.Cone:
                {
                    wRt *= prim.extents.x;
                    wUp *= prim.extents.y;
                    wFw *= prim.extents.z;
                    break;
                }
            }

            //Draw the actual primitive
            switch (prim.type)
            {
                case DebugPrimitiveType.Line:
                {
                    d.RenderLine(prim.setup, pos, pos + wFw * prim.extents.z);
                    break;
                }
                case DebugPrimitiveType.Point:
                {
                    d.RenderLine(prim.setup, pos - wFw, pos + wFw);
                    d.RenderLine(prim.setup, pos - wUp, pos + wUp);
                    d.RenderLine(prim.setup, pos - wRt, pos + wRt);

                    prim.setup.Color = Color.red;
                    d.RenderLine(prim.setup, pos + wRt, pos + wRt * 0.75f + wUp * 0.25f);
                    d.RenderLine(prim.setup, pos + wRt * 0.9f, pos + wRt * 0.75f + wUp * 0.25f);
                    d.RenderLine(prim.setup, pos + wRt, pos + wRt * 0.75f + wFw * 0.25f);
                    d.RenderLine(prim.setup, pos + wRt * 0.9f, pos + wRt * 0.75f + wFw * 0.25f);

                    prim.setup.Color = Color.green;
                    d.RenderLine(prim.setup, pos + wUp, pos + wUp * 0.75f + wFw * 0.25f);
                    d.RenderLine(prim.setup, pos + wUp * 0.9f, pos + wUp * 0.75f + wFw * 0.25f);
                    d.RenderLine(prim.setup, pos + wUp, pos + wUp * 0.75f + wRt * 0.25f);
                    d.RenderLine(prim.setup, pos + wUp * 0.9f, pos + wUp * 0.75f + wRt * 0.25f);

                    prim.setup.Color = Color.blue;
                    d.RenderLine(prim.setup, pos + wFw, pos + wFw * 0.75f + wUp * 0.25f);
                    d.RenderLine(prim.setup, pos + wFw * 0.9f, pos + wFw * 0.75f + wUp * 0.25f);
                    d.RenderLine(prim.setup, pos + wFw, pos + wFw * 0.75f + wRt * 0.25f);
                    d.RenderLine(prim.setup, pos + wFw * 0.9f, pos + wFw * 0.75f + wRt * 0.25f);
                    break;
                }
                case DebugPrimitiveType.Box:
                {
                    var posUp = pos + wUp;
                    var posDn = pos - wUp;

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
                case DebugPrimitiveType.Arc:
                {
                    var segments = prim.setup.Precision;
                    var step = (prim.range.y - prim.range.x) / segments;
                    for (var i = 0; i < segments; i++)
                    {
                        var j = i + 1;
                        var angle0 = prim.range.x + i * step;
                        var angle1 = prim.range.x + j * step;
                        var p0 = wRt * Mathf.Cos(angle0 * Mathf.Deg2Rad) + wUp * Mathf.Sin(angle0 * Mathf.Deg2Rad);
                        var p1 = wRt * Mathf.Cos(angle1 * Mathf.Deg2Rad) + wUp * Mathf.Sin(angle1 * Mathf.Deg2Rad);
                        d.RenderLine(prim.setup, pos + p0, pos + p1);
                    }
                    break;
                }
                case DebugPrimitiveType.Circle:
                {
                    var other = prim;
                    other.type = DebugPrimitiveType.Arc;
                    other.range = vec2(0, 360);
                    Render(d, other);
                    break;
                }
                case DebugPrimitiveType.Sphere:
                {
                    var other = prim;
                    other.type = DebugPrimitiveType.Arc;
                    other.range = vec2(0, 360);
                    Render(d, other);
                    other.rot = Quaternion.LookRotation(wUp, wFw);
                    other.extents = prim.extents.xzy();
                    Render(d, other);
                    other.rot = Quaternion.LookRotation(wRt, wFw);
                    other.extents = prim.extents.zxy();
                    Render(d, other);
                    break;
                }
                case DebugPrimitiveType.Capsule:
                {
                    var z = min(min(prim.extents.x, prim.extents.y), abs(prim.extents.z));
                    var h = max(0, prim.extents.z - z);

                    var other0 = prim;
                    other0.type = DebugPrimitiveType.Arc;
                    other0.range = vec2(0, 360);
                    var other1 = other0;
                    other0.pos -= other0.rot * vec3(0, 0, h);
                    Render(d, other0);
                    other0.rot = Quaternion.LookRotation(wUp, wFw);
                    other0.extents = prim.extents.xny(z);
                    other0.range = vec2(180, 360);
                    Render(d, other0);
                    other0.rot = Quaternion.LookRotation(wRt, wFw);
                    Render(d, other0);

                    other1.pos += other1.rot * vec3(0, 0, h);
                    Render(d, other1);
                    other1.rot = Quaternion.LookRotation(wUp, wFw);
                    other1.extents = prim.extents.xny(z);
                    other1.range = vec2(0, 180);
                    Render(d, other1);
                    other1.rot = Quaternion.LookRotation(wRt, wFw);
                    Render(d, other1);

                    wUp = wUp * prim.extents.y;
                    wRt = wRt * prim.extents.x;
                    d.RenderLine(prim.setup, other0.pos + wUp, other1.pos + wUp);
                    d.RenderLine(prim.setup, other0.pos - wUp, other1.pos - wUp);
                    d.RenderLine(prim.setup, other0.pos + wRt, other1.pos + wRt);
                    d.RenderLine(prim.setup, other0.pos - wRt, other1.pos - wRt);

                    break;
                }

                case DebugPrimitiveType.Cone:
                {
                    var start = pos - wFw;
                    for (int i = 0; i < prim.setup.Precision; ++i)
                    {
                        var a = ((float)i / (float)prim.setup.Precision) * Mathf.PI * 2.0f;
                        var x = Mathf.Sin(a) * wRt;
                        var y = Mathf.Cos(a) * wUp;

                        d.RenderLine(prim.setup, start, pos + wFw + x + y);
                    }

                    var other = prim;
                    other.type = DebugPrimitiveType.Arc;
                    other.range = vec2(0, 360);
                    other.pos = pos + wFw;
                    Render(d, other);
                    break;
                }
                case DebugPrimitiveType.Pie:
                {
                    var pieDiff = abs(prim.range.y - prim.range.x);
                    if (pieDiff >= 360 && prim.extents.z == 0)
                    {
                        var other = prim;
                        other.type = DebugPrimitiveType.Arc;
                        other.range = vec2(0, 360);
                        Render(d, other);
                        break;
                    }

                    //Draw pie arcs
                    {
                        var other = prim;
                        other.type = DebugPrimitiveType.Arc;
                        other.pos -= wFw * prim.extents.z;
                        Render(d, other);
                        other.pos += wFw * prim.extents.z * 2f;
                        Render(d, other);
                    }

                    //Center line
                    d.RenderLine(prim.setup, pos - wFw * prim.extents.z, pos + wFw * prim.extents.z);

                    //Draw sides and inner lines
                    var segments = prim.setup.Precision;
                    var step = (prim.range.y - prim.range.x) / segments;
                    for (var i = 0; i <= segments; i++)
                    {
                        var j = i + 1;
                        var angle = prim.range.x + i * step;
                        var p = wRt * prim.extents.x * Mathf.Cos(angle * Mathf.Deg2Rad) + wUp * prim.extents.y * Mathf.Sin(angle * Mathf.Deg2Rad);
                        d.RenderLine(prim.setup, pos + p - wFw * prim.extents.z, pos + p + wFw * prim.extents.z);

                        if (i == 0 || i == segments)
                        {
                            d.RenderLine(prim.setup, pos - wFw * prim.extents.z, pos + p - wFw * prim.extents.z);
                            d.RenderLine(prim.setup, pos + wFw * prim.extents.z, pos + p + wFw * prim.extents.z);
                        }
                    }
                    break;
                }
                case DebugPrimitiveType.Arrow:
                {
                    var end = pos + wFw * prim.extents.z;
                    d.RenderLine(prim.setup, end, pos);
                    d.RenderLine(prim.setup, end, end - wFw * prim.extents.y - wRt * prim.extents.x);
                    d.RenderLine(prim.setup, end, end - wFw * prim.extents.y + wRt * prim.extents.x);
                    d.RenderLine(prim.setup, end, end - wFw * prim.extents.y - wUp * prim.extents.x);
                    d.RenderLine(prim.setup, end, end - wFw * prim.extents.y + wUp * prim.extents.x);

                    var end0 = end - wFw * prim.extents.y * 0.25f;
                    d.RenderLine(prim.setup, end0, end - wFw * prim.extents.y - wRt * prim.extents.x);
                    d.RenderLine(prim.setup, end0, end - wFw * prim.extents.y + wRt * prim.extents.x);
                    d.RenderLine(prim.setup, end0, end - wFw * prim.extents.y - wUp * prim.extents.x);
                    d.RenderLine(prim.setup, end0, end - wFw * prim.extents.y + wUp * prim.extents.x);
                    break;
                }

                case DebugPrimitiveType.Plane:
                {
                    d.RenderLine(prim.setup, pos - wRt - wUp, pos + wRt - wUp);
                    d.RenderLine(prim.setup, pos - wRt + wUp, pos + wRt + wUp);
                    d.RenderLine(prim.setup, pos - wRt - wUp, pos - wRt + wUp);
                    d.RenderLine(prim.setup, pos + wRt - wUp, pos + wRt + wUp);
                    break;
                }
                case DebugPrimitiveType.LineCast:
                {
                    var a = pos - wFw * prim.extents.z;
                    var b = pos + wFw * prim.extents.z;
                    d.RenderLine(prim.setup, a, b);

                    var other = prim;
                    other.type = DebugPrimitiveType.Sphere;
                    other.extents = vec3(prim.extents.x);
                    other.pos = a;
                    Render(d, other);
                    other.pos = b;
                    Render(d, other);
                    break;
                }
                case DebugPrimitiveType.SphereCast:
                {
                    var pos_ = pos;
                    var pos0 = pos_ - wFw * (prim.extents.z - prim.extents.x);
                    var pos1 = pos_ + wFw * (prim.extents.z - prim.extents.x);
                    var other = prim;
                    other.type = DebugPrimitiveType.Sphere;
                    other.extents = vec3(prim.extents.x);
                    other.pos = pos0;
                    Render(d, other);
                    other.pos = pos1;
                    Render(d, other);

                    d.RenderLine(prim.setup, pos0 - wRt * prim.extents.x, pos1 - wRt * prim.extents.x);
                    d.RenderLine(prim.setup, pos0 + wRt * prim.extents.x, pos1 + wRt * prim.extents.x);
                    d.RenderLine(prim.setup, pos0 + wUp * prim.extents.y, pos1 + wUp * prim.extents.y);
                    d.RenderLine(prim.setup, pos0 - wUp * prim.extents.y, pos1 - wUp * prim.extents.y);

                    d.RenderLine(prim.setup, pos0 + wFw * prim.extents.x, pos1 - wFw * prim.extents.x);
                    break;
                }
            }
        }
        #endregion Primitives render

        ///---------------------------------------------------------------------
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

        ///---------------------------------------------------------------------
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

        //        ///-----------------------------------------------------------
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

        ///---------------------------------------------------------------------
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
