using UnityEngine;
using System;

namespace Stand
{
    public class MainChangesSchedulesWindow : WindowBase
    {
        public override void PrimaryFill()
        {

        }

        public override void Refill() => PrimaryFill();
        public override void Fill() => PrimaryFill();
        public override void Fill(int id) => PrimaryFill();
        public override void Fill(GameObject gameObject) => PrimaryFill();
        public override void ChooseClass(string Class) => PrimaryFill();
        public override void ChooseDay(DayOfWeek d) => PrimaryFill();

        public override void Merge(bool Status) { }
    }
}