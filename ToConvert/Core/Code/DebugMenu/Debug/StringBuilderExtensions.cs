namespace Mayfair.Core.Code.Utils.Debug
{
    using System;
    using System.Diagnostics;
    using System.Text;
    using Prateek.DaemonCore.Code.Interfaces;
    using UnityEditor.VersionControl;
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
        public static void AddLogHeader(this StringBuilder builder, IDaemonCore daemonCore, string notice)
        {
            if (builder == null)
            {
                throw new Exception($"Builder is null for {daemonCore.GetType().Name}");
            }

            builder.Append($"SERVICE<{daemonCore.GetType().Name}>");
            //todo builder.ColorCodeMessage(DoColor(ColorHelper.TypeToHue(daemonCore.GetType(), colorOffset)));
            builder.AppendLine(notice);
        }

        [Conditional("NVIZZIO_DEV")]
        public static void AddLogHeader(this StringBuilder builder, IDaemonBranch branch, string notice)
        {
            if (builder == null)
            {
                throw new Exception($"Builder is null for {branch.GetType().Name}");
            }

            builder.Append($"SERVICE<{branch.Name} ({branch.GetType().Name})>");
            //todo builder.ColorCodeMessage(DoColor(ColorHelper.TypeToHue(branch.GetType(), colorOffset)));
            builder.AppendLine(notice);
        }

        [Conditional("NVIZZIO_DEV")]
        public static void AddReceivedMessage(this StringBuilder builder, IDaemonCore daemonCore, Message notice)
        {
            if (builder == null)
            {
                builder = new StringBuilder();
                builder.AddLogHeader(daemonCore, ", Messages received this frame:");
            }

            //todo builder.AppendLine($"> Message {notice.ToString()} from {notice.Sender.Owner.Name} was received by:");
        }

        //todo [Conditional("NVIZZIO_DEV")]
        //todo public static void AddCommunicator(this StringBuilder builder, IDaemonCore daemonCore, ILightMessageCommunicator noticeReceiver)
        //todo {
        //todo     if (builder == null)
        //todo     {
        //todo         builder = new StringBuilder();
        //todo         builder.AddLogHeader(daemonCore, ", live Communicators:");
        //todo     }
        //todo 
        //todo     builder.AppendLine($"  - Owner: {noticeReceiver.Owner.Name}");
        //todo }
        #endregion
    }
}
