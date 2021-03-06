﻿using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Stand
{
    public class ChangeCallsWindow : WindowBase,IReceive<SignalChangeCallsMatrixChanged>
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

            List<(DateRange date, List<TimeSpan> times)> Table = CallsParser.Instance.GetListChangesCalls();
            Table.Sort((a, b) => a.date.Left.CompareTo(b.date.Left));

            GameObject go = null;
            bool First = true;
            DateTime Today = DateTime.Now.Date;
            foreach(var item in Table)
            {
                if ((item.date.Twins && item.date.Right.Date < Today) || (!item.date.Twins && item.date.Left.Date < Today)) continue;

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

        public bool GetChanges()
        {
            List<(DateRange date, List<TimeSpan> times)> Table = CallsParser.Instance.GetListChangesCalls();
            DateTime Today = DateTime.Now.Date;
            foreach (var (date, times) in Table)
            {
                if ((date.Twins && date.Right.Date < Today) || (!date.Twins && date.Left.Date < Today)) continue;
                return true;
            }
            return false;
        }

        public void Choose((DateRange date, List<TimeSpan> times) item)
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

        void IReceive<SignalChangeCallsMatrixChanged>.HandleSignal(SignalChangeCallsMatrixChanged arg) => Fill();
    }
}