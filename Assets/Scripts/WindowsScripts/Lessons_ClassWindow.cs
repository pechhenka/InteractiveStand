using System;
using UnityEngine;
using UnityEngine.UI;

namespace Stand
{
    public class Lessons_ClassWindow : IWindow
    {
        public Text HeadlineWeeklySchedule;
        public Text WeeklyLessons;
        public Text WeeklyClassrooms;

        private int CurrentWeeklyDay = 0;
        private int CurrentWeeklyClass = 0;

        public override void ChooseClass(string Class)
        {
            Loger.Log("Окно занятий", "открыли");
            int number = int.Parse(Class);
            ChooseDay(DateTime.Now.DayOfWeek.Normalising());
            HeadlineWeeklySchedule.text = CurrentWeeklyDay.ConvertToString(false) + " " + Data.Instance.LessonsMatrix[2][number];
            _UIController.MergeTimePanel(true);
            CurrentWeeklyClass = number;
            Fill();
        }

        public override void ChooseDay(int id)
        {
            if (id != CurrentWeeklyDay)
            {
                if (id > 5 || id < 0) id = 0;
                CurrentWeeklyDay = id;
                string NameClass = HeadlineWeeklySchedule.text.Split()[1];
                HeadlineWeeklySchedule.text = ((DayOfWeek)(id + 1)).ConvertToString(false) + " " + NameClass;
                Fill();
            }
            else
            {
                Loger.Log($"Окно занятий&Class:{Data.Instance.LessonsMatrix[2][CurrentWeeklyClass]}&IdClass:{CurrentWeeklyClass}&IdDay:{CurrentWeeklyDay}", "нервное касание");
            }
        }

        public override void Fill()
        {
            WeeklyClassrooms.text = "";
            WeeklyLessons.text = "";
            if (CurrentWeeklyDay > 5 || CurrentWeeklyDay < 0) CurrentWeeklyDay = 0;
            int CWD = CurrentWeeklyDay * 16 + 3;
            int CWC = CurrentWeeklyClass;
            bool NullLesson = true;

            Loger.Log($"Окно занятий&Class:{Data.Instance.LessonsMatrix[2][CurrentWeeklyClass]}&IdClass:{CurrentWeeklyClass}&IdDay:{CurrentWeeklyDay}", "открыли");

            for (int i = 0; i < 8; i++)
            {
                if (Data.Instance.LessonsMatrix[CWD + i * 2][CWC] == "")
                {
                    if (NullLesson)
                    {
                        WeeklyLessons.text += "--" + '\n';
                        WeeklyClassrooms.text += "--" + '\n';
                        continue;
                    }
                    else
                        break;
                }
                else
                    NullLesson = false;
                WeeklyLessons.text += Data.Instance.LessonsMatrix[CWD + i * 2][CWC] + '\n';

                string Classrooms = "";
                if (Data.Instance.LessonsMatrix[CWD + i * 2 + 1][CWC + 1] != "")
                {
                    Classrooms += Data.Instance.LessonsMatrix[CWD + i * 2 + 1][CWC + 1];
                    if (Data.Instance.LessonsMatrix[CWD + i * 2][CWC + 2] == "/")
                        Classrooms += "/" + Data.Instance.LessonsMatrix[CWD + i * 2 + 1][CWC + 3];
                }
                WeeklyClassrooms.text += Classrooms + '\n';
            }
        }

        public override void PrimaryFill() => Fill();
        public override void Refill() => Fill();
        public override void Fill(int id) => Fill();
        public override void Fill(GameObject gameObject) => Fill();

        public override void Merge(bool Status) { }
    }
}