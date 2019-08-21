using UnityEngine;
using UnityEngine.UI;
using System;

namespace Stand
{
    public class LessonsWindow : WindowBase
    {
        [Header("Prefabs")]
        public GameObject ClassesButton;

        [Header("Others")]
        public GameObject[] ClassButtons;

        public override void PrimaryFill()
        {
            foreach(Class c in LessonsParser.Instance.GetClassesWithoutChanges())
            {
                GameObject go = Instantiate(ClassesButton, ClassButtons[c.Number - 5].transform);
                go.name = c.ToString();
                go.GetComponentInChildren<Text>().text = c.ToString();
            }
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