// -BEGIN_PRATEEK_COPYRIGHT-
//
//  Prateek, a library that is "bien pratique"
//  Header last update date: 20/03/2019
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
#region File namespaces
#endregion File namespaces

//-----------------------------------------------------------------------------
namespace Prateek.Helpers
{
    //-------------------------------------------------------------------------
    public partial class PersonalLogger : MonoBehaviour
    {
        //---------------------------------------------------------------------
        #region Declarations
        public enum LogType
        {
            Nothing,

            Error,
            Warning,

            //Info importance
            Minor,
            Medium,
            Major,
            Extreme,

            BlockStart,
            BlockEndSuccess,
            BlockEndFail,

            MAX
        };

        //---------------------------------------------------------------------
        public struct LogData
        {
            //-----------------------------------------------------------------------------------
            public struct OwnerData
            {
                public object Owner0;
                public object Owner1;
                public object Owner2;

                public int Count
                {
                    get
                    {
                        if (Owner2 != null)
                            return 3;
                        if (Owner1 != null)
                            return 2;
                        if (Owner0 != null)
                            return 1;
                        return 0;
                    }
                }

                public object this[int key]
                {
                    get
                    {
                        switch (key)
                        {
                            case 0: return Owner0;
                            case 1: return Owner1;
                            case 2: return Owner2;
                        }
                        return null;
                    }
                }

                //-----------------------------------------------------------------------------------
                public void Copy(OwnerData other)
                {
                    Owner0 = other.Owner0;
                    Owner1 = other.Owner1;
                    Owner2 = other.Owner2;
                }
            }

            //-----------------------------------------------------------------------------------
            public LogType Type;
            public OwnerData Owners;
            public Helpers.StringBlurp Log;
            public float Timestamp;

            //-----------------------------------------------------------------------------------
            public LogData(object owner0, object owner1, object owner2, LogType type, Helpers.StringBlurp log)
            {
                Owners = new OwnerData() { Owner0 = owner0, Owner1 = owner1, Owner2 = owner2 };
                Type = type;
                Log = log;
                Timestamp = Time.realtimeSinceStartup;
            }
        };
        #endregion //Declarations

        //---------------------------------------------------------------------
        #region Fields
        private Queue<LogData> m_log_history = new Queue<LogData>();
        private bool m_is_buffering_log = false;
        private Queue<LogData> m_log_buffer = new Queue<LogData>();
        #endregion //Fields

        //---------------------------------------------------------------------
        #region Settings
        [SerializeField]
        private int m_log_max_length = 100;

        [SerializeField]
        private bool m_silent_mode = false;
        #endregion //Settings

        //---------------------------------------------------------------------
        #region Properties
        public int LogMaxLength { get { return m_log_max_length; } }
        public bool SilentMode { get { return m_silent_mode; } set { m_silent_mode = value; } }
        #endregion //Properties

        //---------------------------------------------------------------------
        #region Ocp methods
        protected void Start()
        {
            //TODO BHU
            //MainManager.Instance.DebugDisplayManager.Register(this);
        }

        //---------------------------------------------------------------------
        protected void OnDestroy()
        {
            //TODO BHU
            //MainManager.Instance.DebugDisplayManager.Unregister(this);
        }
        #endregion //Ocp methods

        //---------------------------------------------------------------------
        #region Logging

        //---------------------------------------------------------------------
        #region Logging TextBlob
        public void LogWarning(object owner, Helpers.StringBlurp log) { Log(owner, null, LogType.Warning, log); }
        public void LogWarning(object owner0, object owner1, Helpers.StringBlurp log) { Log(owner0, owner1, null, LogType.Warning, log); }
        public void LogWarning(object owner0, object owner1, object owner2, Helpers.StringBlurp log) { Log(owner0, owner1, owner2, LogType.Warning, log); }

        public void LogError(object owner, Helpers.StringBlurp log) { Log(owner, null, LogType.Error, log); }
        public void LogError(object owner0, object owner1, Helpers.StringBlurp log) { Log(owner0, owner1, null, LogType.Error, log); }
        public void LogError(object owner0, object owner1, object owner2, Helpers.StringBlurp log) { Log(owner0, owner1, owner2, LogType.Error, log); }

        public void LogTaskStart(object owner, Helpers.StringBlurp log) { Log(owner, null, LogType.BlockStart, log); }
        public void LogTaskStart(object owner0, object owner1, Helpers.StringBlurp log) { Log(owner0, owner1, null, LogType.BlockStart, log); }
        public void LogTaskStart(object owner0, object owner1, object owner2, Helpers.StringBlurp log) { Log(owner0, owner1, owner2, LogType.BlockStart, log); }

        public void LogMinor(object owner, Helpers.StringBlurp log) { Log(owner, null, LogType.Minor, log); }
        public void LogMinor(object owner0, object owner1, Helpers.StringBlurp log) { Log(owner0, owner1, null, LogType.Minor, log); }
        public void LogMinor(object owner0, object owner1, object owner2, Helpers.StringBlurp log) { Log(owner0, owner1, owner2, LogType.Minor, log); }

        public void LogMedium(object owner, Helpers.StringBlurp log) { Log(owner, null, LogType.Medium, log); }
        public void LogMedium(object owner0, object owner1, Helpers.StringBlurp log) { Log(owner0, owner1, null, LogType.Medium, log); }
        public void LogMedium(object owner0, object owner1, object owner2, Helpers.StringBlurp log) { Log(owner0, owner1, owner2, LogType.Medium, log); }

        public void LogMajor(object owner, Helpers.StringBlurp log) { Log(owner, null, LogType.Major, log); }
        public void LogMajor(object owner0, object owner1, Helpers.StringBlurp log) { Log(owner0, owner1, null, LogType.Major, log); }
        public void LogMajor(object owner0, object owner1, object owner2, Helpers.StringBlurp log) { Log(owner0, owner1, owner2, LogType.Major, log); }

        public void LogExtreme(object owner, Helpers.StringBlurp log) { Log(owner, null, LogType.Extreme, log); }
        public void LogExtreme(object owner0, object owner1, Helpers.StringBlurp log) { Log(owner0, owner1, null, LogType.Extreme, log); }
        public void LogExtreme(object owner0, object owner1, object owner2, Helpers.StringBlurp log) { Log(owner0, owner1, owner2, LogType.Extreme, log); }

        public void LogTaskEnd(object owner, Helpers.StringBlurp log) { Log(owner, null, LogType.BlockEndSuccess, log); }
        public void LogTaskEnd(object owner0, object owner1, Helpers.StringBlurp log) { Log(owner0, owner1, null, LogType.BlockEndSuccess, log); }
        public void LogTaskEnd(object owner0, object owner1, object owner2, Helpers.StringBlurp log) { Log(owner0, owner1, owner2, LogType.BlockEndSuccess, log); }

        public void LogTaskEndFail(object owner, Helpers.StringBlurp log) { Log(owner, null, LogType.BlockEndFail, log); }
        public void LogTaskEndFail(object owner0, object owner1, Helpers.StringBlurp log) { Log(owner0, owner1, null, LogType.BlockEndFail, log); }
        public void LogTaskEndFail(object owner0, object owner1, object owner2, Helpers.StringBlurp log) { Log(owner0, owner1, owner2, LogType.BlockEndFail, log); }
        #endregion //Logging TextBlob

        //---------------------------------------------------------------------
        public void Log(object owner, LogType type, Helpers.StringBlurp log)
        {
            Log(owner, null, type, log);
        }

        //---------------------------------------------------------------------
        public void Log(object owner0, object owner1, LogType type, Helpers.StringBlurp log)
        {
            Log(owner0, owner1, null, type, log);
        }

        //---------------------------------------------------------------------
        public void Log(object owner0, object owner1, object owner2, LogType type, Helpers.StringBlurp log)
        {
            if (m_silent_mode)
                return;

            Log(new LogData(owner0, owner1, owner2, type, log));
        }

        //---------------------------------------------------------------------
        private void Log(LogData newLog)
        {
            var logs = m_is_buffering_log ? m_log_buffer : m_log_history;
            if (!m_is_buffering_log)
            {
                while (m_log_history.Count >= m_log_max_length)
                {
                    logs.Dequeue();
                }
            }
            logs.Enqueue(newLog);
        }

        //---------------------------------------------------------------------
        public void StartBuffering()
        {
            m_is_buffering_log = true;
        }

        //---------------------------------------------------------------------
        public void CommitBuffering()
        {
            m_is_buffering_log = false;
            while (m_log_buffer.Count > 0)
            {
                Log(m_log_buffer.Dequeue());
            }
            m_log_buffer.Clear();
        }

        //---------------------------------------------------------------------
        public void CancelBuffered()
        {
            m_is_buffering_log = false;
            m_log_buffer.Clear();
        }
        #endregion //Logging
    }
}
