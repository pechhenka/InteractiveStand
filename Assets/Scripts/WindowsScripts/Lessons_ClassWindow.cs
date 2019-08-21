using System;
using UnityEngine;
using UnityEngine.UI;

namespace Stand
{
    public class Lessons_ClassWindow : WindowBase
    {
        public Text HeadlineWeeklySchedule;
        public Text WeeklyLessons;
        public Text WeeklyClassrooms;

        private DayOfWeek CurrentDay;
        private Class CurrentClass;

        public override void ChooseClass(string Class)
        {
            Loger.Log("Окно занятий", "открыли");
            CurrentClass = new Class(Class);
            ChooseDay(DateTime.Now.DayOfWeek);
            HeadlineWeeklySchedule.text = CurrentDay.ConvertToString(false) + " " + CurrentClass.ToString();
            _UIController.MergeTimePanel(true);
            Fill();
        }

        public override void ChooseDay(DayOfWeek d)
        {
            if (d != CurrentDay)
            {
                if (d.Normalising() > 5 || d.Normalising() < 0) d = DayOfWeek.Monday;
                CurrentDay = d;
                string NameClass = HeadlineWeeklySchedule.text.Split()[1];
                HeadlineWeeklySchedule.text = d.ConvertToString(false) + " " + NameClass;
                Fill();
            }
            else
            {
                Loger.Log($"Окно занятий&Class:{CurrentClass.ToString()}&IdDay:{CurrentDay}", "нервное касание");
            }
        }

        public void ChooseDay(int d) => ChooseDay(d.ToDayOfWeek());

        public override void Fill()
        {
            WeeklyClassrooms.text = "";
            WeeklyLessons.text = "";

            TableLessons tl = LessonsParser.Instance.GetTableLessonsWithoutChanges(CurrentClass,CurrentDay);

            foreach (string item in tl.Lesson)
                WeeklyLessons.text += item + Environment.NewLine;

            foreach (string item in tl.Cabinet)
                WeeklyClassrooms.text += item + Environment.NewLine;

            Loger.Log($"Окно занятий&Class:{CurrentClass.ToString()}&IdDay:{CurrentDay.Normalising()}", "открыли");
        }

        public override void PrimaryFill() => Fill();
        public override void Refill() => Fill();
        public override void Fill(int id) => Fill();
        public override void Fill(GameObject gameObject) => Fill();

        public override void Merge(bool Status) { }
    }
}