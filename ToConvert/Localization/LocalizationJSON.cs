namespace Assets.Prateek.ToConvert.Localization
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public sealed class LocalizationJSON
    {
        #region Fields
        public List<LocalizationJSONKeyValuePair> kvps = new List<LocalizationJSONKeyValuePair>(); //"Key Value Pairs"
        #endregion

        #region Class Methods
        #region Methods
        public void Add(string key, string value)
        {
            kvps.Add(new LocalizationJSONKeyValuePair {k = key, v = value});
        }
        #endregion
        #endregion
    }
}
