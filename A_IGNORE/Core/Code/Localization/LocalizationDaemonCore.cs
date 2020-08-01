using Mayfair.Core.Code.LoadingProcess;
using Mayfair.Core.Code.Service;

namespace Mayfair.Core.Code.Localization
{
    using System.Collections;
    using System.Collections.Generic;
    using Prateek.DaemonFramework.Code;
    using UnityEngine;

    public class LocalizationDaemon : Daemon<LocalizationDaemon, LocalizationServant>
    {
        #region Methods

        public static bool SetLanguage(SystemLanguage language, bool useMinimalSet)
        {
            return Instance.GetFirstAliveBranch().SetLanguage(language, useMinimalSet);
        }

        public static bool TryGetStaticValue(string key, out string value)
        {
            return Instance.GetFirstAliveBranch().TryGetStaticValue(key, out value);
        }

        public static string GetDynamicValue(string key, params string[] formatItems)
        {
            return Instance.GetFirstAliveBranch().GetDynamicValue(key, formatItems);
        }

        protected override void OnAwake()
        {
        }

        #endregion
    }
}