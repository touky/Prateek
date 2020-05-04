namespace Mayfair.Core.Code.Utils.Debug
{
    using System;
    using UnityEngine;

    public static class DebugError
    {
        #region Methods
        public static void LogError(Exception exception)
        {
            Debug.LogError("               " + exception.GetType() + " / " + exception.Message + "\n"
                         + "               " + exception.StackTrace + "\n"
                         + "               " + exception.TargetSite);
        }
        #endregion
    }
}