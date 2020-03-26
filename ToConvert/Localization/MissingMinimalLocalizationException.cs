namespace Assets.Prateek.ToConvert.Localization
{
    using System;
    using UnityEngine;

    public class MissingMinimalLocalizationException : Exception
    {
        #region Constructors
        public MissingMinimalLocalizationException(SystemLanguage language) : base($"Minimal Localization for {language} was not found") { }
        #endregion
    }
}
