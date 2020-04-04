namespace Mayfair.Core.Code.Utils.Debug
{
    using System;
    using System.Diagnostics;
    using System.Text;
    using Mayfair.Core.Code.Messaging.Communicator;
    using Mayfair.Core.Code.Messaging.Messages;
    using Mayfair.Core.Code.Service;
    using Mayfair.Core.Code.Utils.Helpers;
    using Prateek.DaemonCore.Code.Branches;
    using Prateek.DaemonCore.Code.Interfaces;
    using UnityEngine;

    public static class StringBuilderExtensions
    {
        #region Static and Constants
        private const byte colorOffset = 37;
        #endregion

        #region Class Methods
        private static Color DoColor(float hue)
        {
            return Color.HSVToRGB(hue, 0.5f, 1);
        }

        [Conditional("NVIZZIO_DEV")]
        public static void AddLogHeader(this StringBuilder builder, IDaemonCore daemonCore, string message)
        {
            if (builder == null)
            {
                throw new Exception($"Builder is null for {daemonCore.GetType().Name}");
            }

            builder.Append($"SERVICE<{daemonCore.GetType().Name}>");
            builder.ColorCodeMessage(DoColor(ColorHelper.TypeToHue(daemonCore.GetType(), colorOffset)));
            builder.AppendLine(message);
        }

        [Conditional("NVIZZIO_DEV")]
        public static void AddLogHeader(this StringBuilder builder, IDaemonBranch branch, string message)
        {
            if (builder == null)
            {
                throw new Exception($"Builder is null for {branch.GetType().Name}");
            }

            builder.Append($"SERVICE<{branch.Name} ({branch.GetType().Name})>");
            builder.ColorCodeMessage(DoColor(ColorHelper.TypeToHue(branch.GetType(), colorOffset)));
            builder.AppendLine(message);
        }

        [Conditional("NVIZZIO_DEV")]
        public static void AddReceivedMessage(this StringBuilder builder, IDaemonCore daemonCore, Message message)
        {
            if (builder == null)
            {
                builder = new StringBuilder();
                builder.AddLogHeader(daemonCore, ", Messages received this frame:");
            }

            builder.AppendLine($"> Message {message.ToString()} from {message.Sender.Owner.Name} was received by:");
        }

        [Conditional("NVIZZIO_DEV")]
        public static void AddCommunicator(this StringBuilder builder, IDaemonCore daemonCore, ILightMessageCommunicator communicator)
        {
            if (builder == null)
            {
                builder = new StringBuilder();
                builder.AddLogHeader(daemonCore, ", live Communicators:");
            }

            builder.AppendLine($"  - Owner: {communicator.Owner.Name}");
        }
        #endregion
    }
}
