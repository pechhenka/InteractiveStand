using System;
using UnityEngine;
using UnityEngine.UI;

namespace Stand
{
    public class TimeLineWindow : WindowBase
    {
        public RectTransform ProgressLine;
        public Text LeftBorder;
        public Text RightBorder;
        public Text WhatNow;
        public Text TimeToCall;

        private TimeSpan TimeToCallTS = new TimeSpan();

        private float progressBar = 0f;
        public float ProgressBar
        {
            get
            {
                return progressBar;
            }
            set
            {
                bool flag = value <= 0.038f || value <= 0f || value >= 1f;
                if (value <= 0.038f)
                    progressBar = 0.038f;
                if (value <= 0f)
                    progressBar = 0f;
                if (value >= 1f)
                    progressBar = 1f;
                ProgressLine.anchorMax = new Vector2(flag ? progressBar : value, 1);
            }
        }

        public override void PrimaryFill() => Fill();

        public override void Refill() => PrimaryFill();

        public override void Fill()
        {
            (TimeSpan? Last, TimeSpan? Next) = CallsParser.Instance.BordersCalls();

            LeftBorder.text = Last.ToTimeString();
            RightBorder.text = Next.ToTimeString();

            (int Difference, int TimeLeft) = CallsParser.Instance.AttitudeCalls();

            if (Difference == 0)
                ProgressBar = 0;
            else if (TimeLeft == 0)
                ProgressBar = 1;
            else
                ProgressBar = 1 - ((float)TimeLeft) / Difference;

            TimeToCallTS = TimeSpan.FromSeconds(TimeLeft);
            string TimeToCallText = "До звонка";
            if (TimeToCallTS.TotalSeconds <= 0) TimeToCallText = "--";
            else
            {
                if (TimeToCallTS.Days > 0) TimeToCallText += " " + TimeToCallTS.Days + "д";
                if (TimeToCallTS.Hours > 0) TimeToCallText += " " + TimeToCallTS.Hours + "ч";
                if (TimeToCallTS.Minutes > 0) TimeToCallText += " " + TimeToCallTS.Minutes + "м";
                if (TimeToCallTS.Seconds > 0) TimeToCallText += " " + TimeToCallTS.Seconds + "с";
            }

            TimeToCall.text = TimeToCallText;

            int WhatNowIndex = CallsParser.Instance.WhatNow();
            bool flag = CallsParser.Instance.CheckClassroomHour();

            if (flag)
            {
                if (WhatNowIndex == 1) WhatNow.text = "Идёт классный час";
                else if (WhatNowIndex == -1) WhatNow.text = "Закончился классный час";
                else if (WhatNowIndex > 0) WhatNow.text = "Идёт " + (WhatNowIndex - 1) + NumberDeclination(WhatNowIndex - 1) + " урок";
                else if (WhatNowIndex < 0) WhatNow.text = "Закончился " + (Math.Abs(WhatNowIndex) - 1) + NumberDeclination(Math.Abs(WhatNowIndex) - 1) + " урок";
                else WhatNow.text = "--";
            }
            else
            {
                if (WhatNowIndex > 0) WhatNow.text = "Идёт " + (WhatNowIndex) + NumberDeclination(WhatNowIndex) + " урок";
                else if (WhatNowIndex < 0) WhatNow.text = "Закончился " + (Math.Abs(WhatNowIndex)) + NumberDeclination(Math.Abs(WhatNowIndex)) + " урок";
                else WhatNow.text = "--";
            }
        }

        string NumberDeclination(int number)
        {
            if (number == 1 || number == 4 || number == 5 || number == 9 || number == 10)
                return "-ый";
            if (number == 2 || number == 6 || number == 7 || number == 8)
                return "-ой";
            if (number == 3)
                return "-ий";
            return "";
        }

        public override void Fill(int id) => PrimaryFill();
        public override void Fill(GameObject gameObject) => PrimaryFill();

        public override void Merge(bool Status) { }

        public override void ChooseClass(string Class) => PrimaryFill();
        public override void ChooseDay(DayOfWeek d) => PrimaryFill();
    }
}