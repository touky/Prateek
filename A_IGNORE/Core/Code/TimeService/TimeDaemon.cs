namespace Mayfair.Core.Code.TimeService
{
    using System;
    using System.Collections.Generic;
    using Code.DebugMenu.Content;
    using DebugMenu;
    using Interfaces;
    using JetBrains.Annotations;
    using Messages;
    using Prateek.A_TODO.Runtime.CommandFramework;
    using Prateek.A_TODO.Runtime.CommandFramework.Commands.Core;
    using Prateek.Runtime.DaemonFramework;
    using Prateek.Runtime.TickableFramework.Enums;
    using Service;
    using ServiceProviders;
    using UnityEngine;

    public class TimeDaemon
        : DaemonOverseer<TimeDaemon, BaseTimeServant>
        , IDebugMenuNotebookOwner
    {
        public const float DEFAULT_RATE = 1f;

        private GameTime gameTime;
        private List<WeakReference<ITimeTracker<ICountdownTimer>>> activeTimers;

        public override TickableSetup TickableSetup
        {
            get
            {
                return TickableSetup.Nothing;
            }
        }

        protected override void OnServantRegistered(BaseTimeServant servant)
        {
            base.OnServantRegistered(servant);

            //todo fix ?
            //DateTime newReferenceTime = Servants[Servants.Count - 1].GetCurrentTime();
            //PreserveScheduledTimers(newReferenceTime);
            //ChangeTimeReference(newReferenceTime);
        }

        protected override void OnAwake()
        {
            gameTime = new GameTime();
            activeTimers = new List<WeakReference<ITimeTracker<ICountdownTimer>>>();
        }

        private void Start()
        {
            TimeServiceMenuPage.CreatePage(this);
        }

        private void ChangeTimeReference(DateTime newReferenceTime)
        {
            ReferenceTimeChanged notice = Command.Create<ReferenceTimeChanged>();
            notice.Init(gameTime.CurrentTime, newReferenceTime);

            gameTime.CacheTimeReference(newReferenceTime);

            CommandDaemon.DefaultCommandEmitter.Broadcast(notice);
        }

        private void PreserveScheduledTimers(DateTime newReferenceTime)
        {
            for (int i = activeTimers.Count - 1; i >= 0; i--)
            {
                activeTimers[i].TryGetTarget(out ITimeTracker<ICountdownTimer> countdown);

                if (countdown == null)
                {
                    activeTimers.RemoveAt(i);
                    continue;
                }

                countdown.Interest.PreserveCountdownProgress(newReferenceTime);
            }
        }

        [UsedImplicitly]
        private void ChangeTimeRate(float timeRate)
        {
            gameTime.SetIncrementationRate(timeRate);
        }

        public static ITimeTracker<T> CreateTimeTracker<T>() where T : ITimeInterest
        {
            Type tType = typeof(T);
            ITimeTracker<T> tracker = CreateTimeTrackerInternal<T>(tType);

            return tracker;
        }

        private static ITimeTracker<T> CreateTimeTrackerInternal<T>(Type tType) where T : ITimeInterest
        {
            ITimeTracker<T> tracker;
            if (tType == typeof(ICountdownTimer))
            {
                tracker = (ITimeTracker<T>)new CountdownTimeTracker();
            }
            // ITimeInterest is the base interface, it should be the last value element in the chain
            else if (tType == typeof(ITimeInterest))
            {
                tracker = (ITimeTracker<T>)new TimeTracker();
            }
            else
            {
                throw new NotImplementedException($"{tType} is not supported");
            }

            return tracker;
        }

        private class TimeTracker : ITimeTracker<ITimeInterest>, ITimeInterest
        {
            public DateTime CurrentTime
            {
                get => Instance.gameTime.CurrentTime;
            }

            public ITimeInterest Interest
            {
                get => this;
            }
        }

        private class CountdownTimeTracker : ITimeTracker<ICountdownTimer>, ICountdownTimer
        {
            private static readonly DateTime UNSCHEDULED = DateTime.MinValue;

            private DateTime callbackTime = UNSCHEDULED;

            public DateTime CurrentTime
            {
                get => Instance.gameTime.CurrentTime;
            }

            public bool TimeReached
            {
                get => TimeRemaining.TotalSeconds < 0;
            }

            public TimeSpan TimeRemaining
            {
                get => GetTimeRemainingTillCallback();
            }

            public void SetCountdownDuration(int seconds)
            {
                callbackTime = Instance.gameTime.GetFutureTime(seconds);
            }

            public void PreserveCountdownProgress(DateTime newReferenceTime)
            {
                if (callbackTime != UNSCHEDULED)
                {
                    callbackTime = newReferenceTime.AddSeconds(TimeRemaining.Seconds);
                }
            }

            public void Clear()
            {
                callbackTime = UNSCHEDULED;
            }

            public ICountdownTimer Interest
            {
                get => this;
            }

            public CountdownTimeTracker()
            {
                Instance.activeTimers.Add(new WeakReference<ITimeTracker<ICountdownTimer>>(this));
            }

            private TimeSpan GetTimeRemainingTillCallback()
            {
                if (callbackTime == UNSCHEDULED)
                {
                    return TimeSpan.MaxValue;
                }

                return callbackTime - Instance.gameTime.CurrentTime;
            }
        }

        private class GameTime
        {
            private DateTime cachedTimeReference;

            private float secondsAtLastCache;

            private float incrementationRate;

            public DateTime CurrentTime
            {
                // By calculating the current time when needed we do not need to maintain an update loop and counter
                get => cachedTimeReference.AddSeconds((Time.time - secondsAtLastCache) * incrementationRate);
            }

            public GameTime()
            {
                incrementationRate = 1f;
                cachedTimeReference = DateTime.MinValue;
            }

            public void CacheTimeReference(DateTime time)
            {
                CacheTime(time);
            }

            public void SetIncrementationRate(float incrementationRate)
            {
                // If the incrementation rate changes, we do not want to lose track of the previously elapsed time
                // we cache the current time of the game as our new point of reference and then update the incrementation rate
                CacheTime(CurrentTime);
                this.incrementationRate = incrementationRate;
            }

            public DateTime GetFutureTime(float seconds)
            {
                Debug.Assert(seconds > 0f);
                return CurrentTime.AddSeconds(seconds);
            }

            private void CacheTime(DateTime time)
            {
                cachedTimeReference = time;
                secondsAtLastCache = Time.time;
            }
        }
    }
}