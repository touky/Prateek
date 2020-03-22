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
namespace Prateek.Base
{
    //-------------------------------------------------------------------------
    public interface IGlobalManager
    {
    }

    //-------------------------------------------------------------------------
    public interface IUpdatable
    {
        // Object Lifetime Messages
        /// <summary>
        /// OnInitialize is called when the object is created. OnInitialize is not called during deserialization.
        /// </summary>
        void OnInitialize();
        /// <summary>
        /// On Start is called at the start of the next frame after the object has been created. OnStart is not called during deserialization.
        /// </summary>
        void OnStart();
        /// <summary>
        /// OnUpdate is called every frame.
        /// </summary>
        /// <param name="deltaTime"></param>
        void OnUpdate(Registry.TickEvent tickEvent, float seconds);
        /// <summary>
        /// OnTimescaleIndependantUpdate is called every frame, after every OnUpdate has been called but before any OnLateUpdate has been called. It's deltaTime is timscale independant.
        /// </summary>
        /// <param name="deltaTime"></param>
        void OnUpdateUnscaled(Registry.TickEvent tickEvent, float seconds);
        /// <summary>
        /// OnLateUpdate is called every frame after the OnUpdate for every object has been called.
        /// </summary>
        /// <param name="deltaTime"></param>
        void OnLateUpdate(Registry.TickEvent tickEvent, float seconds);
        /// <summary>
        /// OnFixedUpdate is called every fixed physics engine update.
        /// </summary>
        /// <param name="deltaTime"></param>
        void OnFixedUpdate(Registry.TickEvent tickEvent, float seconds);
        /// <summary>
        /// OnDispose is called just before the object is destroyed.
        /// </summary>
        void OnDispose();
        /// <summary>
        /// Called when registering the object for updates. Unlike OnInitialize and OnStart, OnRegister is called during serialization. (I couldn't call it OnEnable because it is a Unity message that ScriptableObjects receive)
        /// </summary>
        void OnRegister();
        /// <summary>
        /// Called when unregistering the object for updates. (I couldn't call it OnDisable because it is a Unity message that ScriptableObjects receive)
        /// </summary>
        void OnUnregister();

        // Application Messages
        void OnApplicationFocus(bool focusStatus);
        void OnApplicationPause(bool pauseStatus);
        void OnApplicationQuit();

        // Ui Messages
        void OnGUI();
    }

    //-------------------------------------------------------------------------
    public interface ISingleton
    {
        // Don't force to implement any function since we can't force static functions.
    }
}
