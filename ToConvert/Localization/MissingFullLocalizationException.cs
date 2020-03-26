namespace Assets.Prateek.ToConvert.Localization
{
    using System;
    using UnityEngine;

    public class MissingFullLocalizationException : Exception
    {
        #region Constructors
        public MissingFullLocalizationException(SystemLanguage language) : base($"Full Localization for {language} was not found") { }
        #endregion
    }
}
