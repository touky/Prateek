
namespace Mayfair.Core.Code.Localization
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public sealed class LocalizationJSON
    {
        #region Fields
        public List<LocalizationJSONKeyValuePair> kvps = new List<LocalizationJSONKeyValuePair>(); //"Key Value Pairs"
        #endregion

        #region Properties
        #endregion

        #region Methods
        public void Add(string key, string value)
        {
            kvps.Add(new LocalizationJSONKeyValuePair { k = key, v = value });
        }
        #endregion
    }
}