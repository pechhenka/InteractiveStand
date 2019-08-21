using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Stand
{
    public class ChangeCallsWindow : WindowBase
    {
        [Header("Prefabs")]
        public GameObject DateCallButton;
        [Header("Other")]
        public GameObject DateButtons;
        public Text Calls;
        public override void PrimaryFill() => Fill();

        public override void Refill() => PrimaryFill();
        public override void Fill()
        {
            foreach (Transform child in DateButtons.transform)
                Destroy(child.gameObject);
            Calls.text = "";

            List<(DateTime date, List<TimeSpan> times)> Table = CallsParser.Instance.GetListChangesCalls();
            Table.Sort((a, b) => a.date.CompareTo(b.date));

            GameObject go = null;
            bool First = true;
            DateTime Today = DateTime.Now;
            foreach(var item in Table)
            {
                if (item.date < Today) continue;

                go = Instantiate(DateCallButton, DateButtons.transform);
                go.GetComponentInChildren<Text>().text = item.date.ToShortDate();
                go.GetComponentInChildren<DateCallButton>().item = item;

                if (First)
                {
                    go.GetComponentInChildren<DateCallButton>().OnClick();
                    First = false;
                }
            }
        }

        public void Choose((DateTime date, List<TimeSpan> times) item)
        {
            Calls.text = item.date.ToShortDate() + Environment.NewLine;

            bool Switch = false;
            foreach (TimeSpan it in item.times)
            {
                Calls.text += it.ToTimeString() + (Switch ? Environment.NewLine : " - ");
                Switch = !Switch;
            }
        }
        public override void Fill(int id) => PrimaryFill();
        public override void Fill(GameObject gameObject) => PrimaryFill();
        public override void ChooseClass(string Class) => PrimaryFill();
        public override void ChooseDay(DayOfWeek d) => PrimaryFill();

        public override void Merge(bool Status) { }
    }
}