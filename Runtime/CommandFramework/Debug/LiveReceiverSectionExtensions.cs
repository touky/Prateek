namespace Prateek.Runtime.CommandFramework.Debug
{
    using System;

    internal static class LiveReceiverSectionExtensions
    {
        #region Class Methods
        public static void AddCommandType(this LiveReceiverSection section, Type commandType)
        {
            if (section == null)
            {
                return;
            }

            section.AddType(commandType);
        }
        #endregion
    }
}
