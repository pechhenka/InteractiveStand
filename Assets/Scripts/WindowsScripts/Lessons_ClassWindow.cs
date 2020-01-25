using System;
using System.Collections.Generic;
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
            Loger.Warning("Окно занятий", new List<KeyValuePair<string, string>> {
                new KeyValuePair<string, string>("Открыли", null)
            });

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
                Loger.Log("Окно занятий", new List<KeyValuePair<string, string>> {
                    new KeyValuePair<string, string>("Class", CurrentClass.ToString()),
                    new KeyValuePair<string, string>("IdDay", CurrentDay.ToString()),
                    new KeyValuePair<string, string>("Повтор", null),
                });
            }
        }

        public void ChooseDay(int d) => ChooseDay(d.ToDayOfWeek());

        public override void Fill()
        {
            WeeklyClassrooms.text = "";
            WeeklyLessons.text = "";

            TableLessons tl = LessonsParser.Instance.GetTableLessonsWithoutChanges(CurrentClass, CurrentDay);

            foreach (string item in tl.Lesson)
                WeeklyLessons.text += item + Environment.NewLine;

            foreach (string item in tl.Cabinet)
                WeeklyClassrooms.text += item + Environment.NewLine;

            Loger.Log("Окно занятий", new List<KeyValuePair<string, string>> {
                    new KeyValuePair<string, string>("Class", CurrentClass.ToString()),
                    new KeyValuePair<string, string>("IdDay", CurrentDay.ToString()),
                    new KeyValuePair<string, string>("Открыли", null),
                });
        }

        public override void PrimaryFill() => Fill();
        public override void Refill() => Fill();
        public override void Fill(int id) => Fill();
        public override void Fill(GameObject gameObject) => Fill();

        public override void Merge(bool Status) { }
    }
}