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
namespace Prateek.Core.Code.Attributes
{
    using System;
    using UnityEngine;

    //-------------------------------------------------------------------------
    public abstract class MathAttribute : PropertyAttribute
    {
        public enum ValueType
        {
            Int,
            Float,

            MAX
        }

        #region Fields
        protected ValueType valueType = ValueType.Int;

        protected int iMinValue;
        protected int iMaxValue;

        protected float fMinValue;
        protected float fMaxValue;
        #endregion Fields

        #region Fields
        public ValueType Type { get { return valueType; } }

        public int iMin { get { return iMinValue; } }
        public int iMax { get { return iMaxValue; } }

        public float fMin { get { return fMinValue; } }
        public float fMax { get { return fMaxValue; } }
        #endregion Fields

        //---------------------------------------------------------------------
        protected MathAttribute(int minValue, int maxValue)
        {
            valueType = ValueType.Int;
            iMinValue = minValue;
            iMaxValue = maxValue;
        }

        //---------------------------------------------------------------------
        protected MathAttribute(float minValue, float maxValue)
        {
            valueType = ValueType.Float;
            fMinValue = minValue;
            fMaxValue = maxValue;
        }
    }

    //-------------------------------------------------------------------------
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class MaxAttribute : MathAttribute
    {
        public MaxAttribute(float value) : base(float.MinValue, value) { }
        public MaxAttribute(double value) : base(float.MinValue, (float)value) { }
        public MaxAttribute(int value) : base(int.MinValue, value) { }
    }

    //-------------------------------------------------------------------------
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class ClampAttribute : MathAttribute
    {
        public ClampAttribute(float min, float max) : base(min, max) { }
        public ClampAttribute(double min, double max) : base((float)min, (float)max) { }
        public ClampAttribute(int min, int max) : base(min, max) { }
    }
}
