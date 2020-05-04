using System;

namespace Mayfair.Core.Code.Localization
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class MissingMinimalLocalizationException : Exception
    {
        public MissingMinimalLocalizationException(SystemLanguage language) : base($"Minimal Localization for {language} was not found")
        {
        }
    }
}