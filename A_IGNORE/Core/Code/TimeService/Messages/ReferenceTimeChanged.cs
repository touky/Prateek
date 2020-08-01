namespace Mayfair.Core.Code.TimeService.Messages
{
    using System;
    using Commands.Core;

    public class ReferenceTimeChanged : BroadcastCommand
    {
        public DateTime OldTime { get; private set; }
        public DateTime NewTime { get; private set; }

        public void Init(DateTime oldTime, DateTime newTime)
        {
            OldTime = oldTime;
            NewTime = newTime;
        }
    }
}