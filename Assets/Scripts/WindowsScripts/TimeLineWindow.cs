using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stand
    {
public class TimeLineWindow : IWindow
{
    
        public override void PrimaryFill()
        {

        }

        public override void Refill() => PrimaryFill();
        public override void Fill() => PrimaryFill();
        public override void Fill(int id) => PrimaryFill();
        public override void Fill(GameObject gameObject) => PrimaryFill();

        public override void Merge(bool Status) { }

        public override void ChooseClass(string Class) => PrimaryFill();
        public override void ChooseDay(int id) => PrimaryFill();
    }
}