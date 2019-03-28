// -BEGIN_PRATEEK_COPYRIGHT-
//
//  Prateek, a library that is "bien pratique"
//  Header last update date: 24/03/2019
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

#region Using static
using static Prateek.ShaderTo.CSharp;
#endregion Using static

#region Editor
#if UNITY_EDITOR
using Prateek.CodeGeneration;
#endif //UNITY_EDITOR
#endregion Editor

#if PRATEEK_DEBUGS
using Prateek.Debug;
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
    public static partial class Draw
    {
        //---------------------------------------------------------------------
        #region Declarations
        public class Scope : GUI.Scope
        {
            //---------------------------------------------------------------------
            #region Fields
            private Setup setup;
            #endregion Fields

            //---------------------------------------------------------------------
            #region Properties
            public Setup Setup { get { return setup; } }
            #endregion Properties

            //---------------------------------------------------------------------
            #region Scope
            protected Scope(Setup setup) : base()
            {
                this.setup = setup;
                Add(this);
            }

            //---------------------------------------------------------------------
            protected override void CloseScope()
            {
                Remove(this);
            }
            #endregion Scope
        }

        //---------------------------------------------------------------------
        public enum Space
        {
            World,
            CameraLocal,
            CameraRef,
            CameraViewRatio,
            CameraViewPixel,
        }

        //---------------------------------------------------------------------
        public partial struct Setup
        {
            //---------------------------------------------------------------------
            public enum InitMode
            {
                Reset,
                UseLast,
            }

            //---------------------------------------------------------------------
            private Space space;
            private Matrix4x4 matrix;
            private Color color;
            private float duration;
            private bool depthTest;

            //---------------------------------------------------------------------
            public Space Space { get { return space; } set { space = value; } }
            public Matrix4x4 Matrix { get { return matrix; } set { matrix = value; } }
            public Color Color { get { return color; } set { color = value; } }
            public float Duration { get { return duration; } set { duration = value; } }
            public bool DepthTest { get { return depthTest; } set { depthTest = value; } }

            //---------------------------------------------------------------------
            public Setup(InitMode initMode)
            {
                switch(initMode)
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
                        break;
                    }
                }
            }
        }

        //---------------------------------------------------------------------
        public struct LineSetup
        {
            public Vector3 start;
            public Vector3 end;
            public Setup setup;
        }
        #endregion Declarations

        //---------------------------------------------------------------------
        #region Fields
        private static List<Scope> scopes = new List<Scope>();
        private static Setup currentSetup = new Setup(Setup.InitMode.Reset);
        private static List<LineSetup> screenSpaceLines = new List<LineSetup>();
        #endregion Fields

        //---------------------------------------------------------------------
        #region Scopes
        public static Setup ActiveSetup
        {
            get
            {
                if (scopes.Count == 0)
                    return new Setup(Setup.InitMode.Reset);
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
        public static void EndFrame()
        {
            if (!FrameRecorder.playback_active || FrameRecorder.frame_count == 0)
            {
                for (int i = 0; i < screenSpaceLines.Count; ++i)
                {
                    var data = screenSpaceLines[i];
                    DelayedLine(ref data);
                }
                screenSpaceLines.Clear();
            }

            FrameRecorder.EndFrame();
            if (FrameRecorder.playback_active)
            {
                var currentFrame = FrameRecorder.CurrentFrame;
                for (int i = 0; i < currentFrame.m_lines.Count; ++i)
                {
                    var data = currentFrame.m_lines[i];
                    DelayedLine(ref data);
                }
            }
        }

        //---------------------------------------------------------------------
        #region Line operations
        private static void DelayedLine(ref LineSetup newData, bool useDuration = true)
        {
            var camera = UnityEngine.Camera.current;
            var data = newData;
            Matrix4x4 mx = data.setup.Matrix;
            switch (data.setup.Space)
            {
                case Space.CameraLocal: { mx = camera.transform.localToWorldMatrix * mx; break; }
                case Space.CameraRef: { mx = camera.cameraToWorldMatrix * mx; break; }
                case Space.CameraViewRatio:
                {
                    var mxInv = camera.localToCameraMatrix().inverse;
                    var scale = (float)camera.pixelHeight / (float)camera.pixelWidth;
                    var vScale = new Vector3(scale, 1, -1);
                    data.start = mxInv.MultiplyPoint(Vector3.Scale(data.start, vScale));
                    data.end = mxInv.MultiplyPoint(Vector3.Scale(data.end, vScale));
                    //TODO BHU -------------------------------
                    data.start.z = Mathf.Max(-data.start.z, .01f /*MainManager.Instance.Objects.MainCamera.nearClipPlane*/);
                    data.end.z = Mathf.Max(-data.end.z, 10000f /*MainManager.Instance.Objects.MainCamera.nearClipPlane*/);
                    mx = camera.transform.localToWorldMatrix * mx;
                    break;
                }
                case Space.CameraViewPixel:
                {
                    var mxInv = camera.localToCameraMatrix().inverse;
                    var vScale = new Vector3(1.0f / (float)camera.pixelWidth, 1.0f / (float)camera.pixelHeight, -1);
                    data.start = mxInv.MultiplyPoint(Vector3.Scale(data.start, vScale));
                    data.end = mxInv.MultiplyPoint(Vector3.Scale(data.end, vScale));
                    data.start.z = Mathf.Max(-data.start.z, .01f /*MainManager.Instance.Objects.MainCamera.nearClipPlane*/);
                    data.end.z = Mathf.Max(-data.end.z, 10000f /*MainManager.Instance.Objects.MainCamera.nearClipPlane*/);
                    mx = camera.transform.localToWorldMatrix * mx;
                    break;
                }
            }

            data.start = mx.MultiplyPoint(data.start);
            data.end = mx.MultiplyPoint(data.end);

            data.setup.Space = Space.World;

            FrameRecorder.AddLine(data);

            data.setup.Duration = useDuration ? data.setup.Duration : -1f;
            ImmediateLine(ref data);

        }

        //---------------------------------------------------------------------
#if UNITY_EDITOR
        private static void ImmediateLine(ref LineSetup data)
        {
            UnityEngine.Debug.DrawLine(data.start, data.end, data.setup.Color, data.setup.Duration, data.setup.DepthTest);
        }
#else //UNITY_EDITOR
        private static void ImmediateLine(ref LineSetup data)
        {
            var line = MainManager.Instance.DebugDisplayManager.GetLine();
            line.SetColor(data.setup.Color);
            line.SetDebugLine(data.setup.start, data.setup.end);
        }
#endif //UNITY_EDITOR
        #endregion Line operations

        //---------------------------------------------------------------------
        #region Base primitives
        public static void Line(Vector3 start, Vector3 end, Setup? custom_setup = null)
        {
            var localSetup = new Setup(custom_setup != null ? custom_setup.Value.Space : currentSetup.Space);
            localSetup.Color = currentSetup.Color;
            localSetup.Duration = currentSetup.Duration;
            localSetup.DepthTest = currentSetup.DepthTest;
            localSetup.Matrix = currentSetup.Matrix;

            if (custom_setup != null)
            {
                localSetup.Color = custom_setup.Value.Color;
                localSetup.Duration = custom_setup.Value.Duration;
                localSetup.DepthTest = custom_setup.Value.DepthTest;
                localSetup.Matrix = custom_setup.Value.Matrix;
            }

            //Delay screen-space lines to end of frame 
            if (localSetup.Space != Space.World)
            {
                screenSpaceLines.Add(new LineSetup() { start = start, end = end, setup = localSetup });
            }
            else
            {
                if (FrameRecorder.playback_active)
                    return;

                FrameRecorder.AddLine(new LineSetup() { start = start, end = end, setup = localSetup });

                start = localSetup.Matrix.MultiplyPoint(start);
                end = localSetup.Matrix.MultiplyPoint(end);

                var line = new LineSetup() { start = start, end = end, setup = localSetup };
                ImmediateLine(ref line);
            }
        }
        #endregion //Base primitives
    }
}
#endif //PRATEEK_DEBUG
