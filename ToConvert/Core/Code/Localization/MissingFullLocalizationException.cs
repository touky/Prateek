using System;

namespace Mayfair.Core.Code.Localization
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class MissingFullLocalizationException : Exception
    {
        public MissingFullLocalizationException(SystemLanguage language) : base($"Full Localization for {language} was not found")
        {
        }
    }
}