namespace Mayfair.Core.Code.TimeService.DebugMenu
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using Code.DebugMenu;
    using Code.DebugMenu.Content;
    using Code.DebugMenu.Fields;
    using Code.DebugMenu.Pages;
    using Interfaces;
    using UnityEngine;
    using Utils.Debug.Reflection;
    using Object = UnityEngine.Object;

    public class TimeServiceMenuPage : DebugMenuPage<TimeService>
    {
        private const float MIN_PER_SECOND = 60f;
        private const string TIME_RATE_FORMAT = "N1";

        private ITimeTracker<ITimeInterest> timeTracker;

        private TimeControlData timeData;

        private InputField monthInput;
        private InputField dayInput;
        private InputField yearInput;
        private InputField hourInput;
        private InputField minuteInput;

        private FloatSliderField timeRateSlider;
        private InputField timeRateInput;
        private float timeRateValue;

        public TimeServiceMenuPage(TimeService owner, string title) : base(owner, title)
        {
            timeTracker = TimeService.CreateTimeTracker<ITimeInterest>();
            timeData = new TimeControlData(timeTracker.Interest.CurrentTime);
            InitializeTimeInputFields();

            timeRateValue = TimeService.DEFAULT_RATE;
            timeRateSlider = new FloatSliderField("Time Rate Slider", new Vector2(TimeService.DEFAULT_RATE, MIN_PER_SECOND), TimeService.DEFAULT_RATE);
            timeRateInput = new InputField(TimeService.DEFAULT_RATE.ToString(TIME_RATE_FORMAT));
        }

        [Conditional("NVIZZIO_DEV")]
        public static void CreatePage(TimeService owner)
        {
            DebugMenuNotebook debugNotebook = new DebugMenuNotebook("TIMS", "Time Service");

            debugNotebook.AddPages(new TimeServiceMenuPage(owner, "Time Controls"));
            debugNotebook.Register();
        }

        public void InitializeTimeInputFields()
        {
            monthInput = new InputField(timeData.month.ToString());
            dayInput = new InputField(timeData.day.ToString());
            yearInput = new InputField(timeData.year.ToString());
            hourInput = new InputField(timeData.hour.ToString());
            minuteInput = new InputField(timeData.minute.ToString());
        }

        protected override void Draw(TimeService owner, DebugMenuContext context)
        {
            DrawGameTime(context);
            DrawTimeTools(context);
        }

        private void DrawTimeTools(DebugMenuContext context)
        {
            LabelField titleField = GetField<LabelField>();
            titleField.Draw(context, "Tools");

            using (new ContextIndentScope(context, 1))
            {
                DrawSetTimeFields(context);
                DrawSetTimeButton(context);
                // todo: Ask Ben how to add a space between elements
                DrawTimeRateUi(context);
            }
        }

        private void DrawGameTime(DebugMenuContext context)
        {
            // todo: Ask Ben about making this Bold
            LabelField titleField = GetField<LabelField>();
            titleField.Draw(context, "Current Time");

            using (new ContextIndentScope(context, 1))
            {
                LabelField timeField = GetField<LabelField>();
                timeField.Draw(context, timeTracker.Interest.CurrentTime.ToString("G", CultureInfo.CurrentCulture));
            }
        }

        private void DrawSetTimeFields(DebugMenuContext context)
        {
            string month = timeData.month.ToString();
            string day = timeData.day.ToString();
            string year = timeData.year.ToString();
            string hour = timeData.hour.ToString();
            string minute = timeData.minute.ToString();

            // todo: Ask Ben if its possible to lay these out horizontally
            monthInput.Draw(context, out month);
            dayInput.Draw(context, out day);
            yearInput.Draw(context, out year);
            hourInput.Draw(context, out hour);
            minuteInput.Draw(context, out minute);

            timeData.SetTime(month, day, year, hour, minute);
        }

        private void DrawSetTimeButton(DebugMenuContext context)
        {
            // todo: Ask Ben if its possible to right-align this
            ButtonField applyTimeButton = GetField<ButtonField>();
            if (applyTimeButton.Draw(context, "Apply"))
            {
                DispatchSetReferenceTime(timeData.DateTime);
            }
        }

        private void DrawTimeRateUi(DebugMenuContext context)
        {
            // todo: Ask Ben if its possible to put the title and slider on separate lines
            timeRateSlider.Draw(context);
            timeRateInput.Draw(context, out string inputFieldTimeRate);

            UnifyTimeRateUi(inputFieldTimeRate);

            // todo: Ask Ben if its possible to right-align this
            ButtonField applyRateButton = GetField<ButtonField>();
            if (applyRateButton.Draw(context, "Apply"))
            {
                DispatchChangeTimeRate(timeRateValue);
            }
        }

        private void UnifyTimeRateUi(string inputFieldTimeRate)
        {
            if (float.TryParse(inputFieldTimeRate, out float inputTimeRate) &&
                !Mathf.Approximately(inputTimeRate, timeRateValue))
            {
                timeRateSlider.Value = inputTimeRate;
                timeRateValue = inputTimeRate;
            }
            else
            {
                timeRateInput.Value = timeRateSlider.Value.ToString();
                timeRateValue = timeRateSlider.Value;
            }
        }

        private void DispatchSetReferenceTime(DateTime newTime)
        {
            TimeService timeService = Object.FindObjectOfType<TimeService>();
            ReflectionUtils.CallMethod("ChangeTimeReference", timeService, newTime);
        }

        private void DispatchChangeTimeRate(float value)
        {
            TimeService timeService = Object.FindObjectOfType<TimeService>();
            ReflectionUtils.CallMethod("ChangeTimeRate", timeService, value);
        }

        private struct TimeControlData
        {
            public int month;
            public int day;
            public int year;
            public int hour;
            public int minute;

            public DateTime DateTime
            {
                get => new DateTime(year, month, day, hour, minute, 0);
            }

            public TimeControlData(DateTime time)
            {
                month = time.Month;
                day = time.Day;
                year = time.Year;
                hour = 0;
                minute = 0;
            }

            public void SetTime(string month, string day, string year, string hour, string minute)
            {
                int.TryParse(month, out this.month);
                int.TryParse(day, out this.day);
                int.TryParse(year, out this.year);
                int.TryParse(hour, out this.hour);
                int.TryParse(minute, out this.minute);
            }
        }
    }
}