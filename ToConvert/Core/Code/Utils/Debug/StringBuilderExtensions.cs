namespace Mayfair.Core.Code.Utils.Debug
{
    using System;
    using System.Diagnostics;
    using System.Text;
    using Mayfair.Core.Code.Messaging.Communicator;
    using Mayfair.Core.Code.Messaging.Messages;
    using Mayfair.Core.Code.Service;
    using Mayfair.Core.Code.Service.Interfaces;
    using Mayfair.Core.Code.Utils.Helpers;
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
        public static void AddLogHeader(this StringBuilder builder, IService service, string message)
        {
            if (builder == null)
            {
                throw new Exception($"Builder is null for {service.GetType().Name}");
            }

            builder.Append($"SERVICE<{service.GetType().Name}>");
            builder.ColorCodeMessage(DoColor(ColorHelper.TypeToHue(service.GetType(), colorOffset)));
            builder.AppendLine(message);
        }

        [Conditional("NVIZZIO_DEV")]
        public static void AddLogHeader(this StringBuilder builder, ServiceProvider provider, string message)
        {
            if (builder == null)
            {
                throw new Exception($"Builder is null for {provider.GetType().Name}");
            }

            builder.Append($"SERVICE<{provider.GetType().Name}>");
            builder.ColorCodeMessage(DoColor(ColorHelper.TypeToHue(provider.GetType(), colorOffset)));
            builder.AppendLine(message);
        }

        [Conditional("NVIZZIO_DEV")]
        public static void AddLogHeader(this StringBuilder builder, ServiceProviderBehaviour provider, string message)
        {
            if (builder == null)
            {
                throw new Exception($"Builder is null for {provider.GetType().Name}");
            }

            builder.Append($"SERVICE<{provider.name} ({provider.GetType().Name})>");
            builder.ColorCodeMessage(DoColor(ColorHelper.TypeToHue(provider.GetType(), colorOffset)));
            builder.AppendLine(message);
        }

        [Conditional("NVIZZIO_DEV")]
        public static void AddReceivedMessage(this StringBuilder builder, IService service, Message message)
        {
            if (builder == null)
            {
                builder = new StringBuilder();
                builder.AddLogHeader(service, ", Messages received this frame:");
            }

            builder.AppendLine($"> Message {message.ToString()} from {message.Sender.Owner.Name} was received by:");
        }

        [Conditional("NVIZZIO_DEV")]
        public static void AddCommunicator(this StringBuilder builder, IService service, ILightMessageCommunicator communicator)
        {
            if (builder == null)
            {
                builder = new StringBuilder();
                builder.AddLogHeader(service, ", live Communicators:");
            }

            builder.AppendLine($"  - Owner: {communicator.Owner.Name}");
        }
        #endregion
    }
}
