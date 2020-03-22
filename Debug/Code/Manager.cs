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
    using System.Collections.Generic;
    using Prateek.Helpers;
    using UnityEngine;

    public class DebugDisplayManager_ : MonoBehaviour
    {
        //---------------------------------------------------------------------
        #region Fields
        private PersonalLoggerManager loggerManager = new PersonalLoggerManager();
        private List<Helpers.StringBlurp> blurps = new List<Helpers.StringBlurp>();

        #endregion //Fields

        //---------------------------------------------------------------------
        #region Unity Defaults
        void LateUpdate()
        {
            loggerManager.DisplayDebug();
        }

        //---------------------------------------------------------------------
        void OnGUI()
        {
            loggerManager.DisplayGUI();
        }
        #endregion //Unity Defaults

        //---------------------------------------------------------------------
        #region Logger
        public void Register(Helpers.PersonalLogger logger)
        {
            if (loggerManager == null)
                loggerManager = new PersonalLoggerManager();
            loggerManager.Register(logger);
        }

        //---------------------------------------------------------------------
        public void Unregister(Helpers.PersonalLogger logger)
        {
            loggerManager.Unregister(logger);
        }
        #endregion //Logger
    }
}
#endif //PRATEEK_DEBUG
