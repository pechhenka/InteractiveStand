using UnityEngine;
using UnityEngine.UI;

namespace Stand
{
    public class Lessons_ClassWindows : IWindow
    {
        public GameObject WeeklySchedule;
        public Text HeadlineWeeklySchedule;
        public Text WeeklyLessons;
        public Text WeeklyClassrooms;

        private int CurrentWeeklyDay = 0;
        private int CurrentWeeklyClass = 0;

        public override void PrimaryFill()
        {

        }

        public override void Refill() => PrimaryFill();
        public override void Fill()
        {

        }
        public override void Fill(int id)
        {

        }
        public override void Fill(GameObject gameObject)
        {

        }
    }
}