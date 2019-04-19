using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Stand
{
    public class TimePanelWindow : IWindow
    {
        public Text TimeUI;
        public Text DateUI;
        public Image TimePanelImage;

        public override void PrimaryFill() => Fill();

        public override void Refill() => Fill();
        public override void Fill()
        {
            string day = DateTime.Now.DayOfWeek.ConvertToString();
            TimeUI.text = DateTime.Now.ToString("HH:mm ") + day;
            DateUI.text = DateTime.Now.ToString("dd.MM.yyyy");
        }
        public override void Fill(int id) => Fill();
        public override void Fill(GameObject gameObject) => Fill();

        public override void Merge(bool Status)
        {
            TimePanelImage.color = Status ? _UIController.VeryGray : _UIController.Gray;
        }

        public override void ChooseClass(string Class) => Fill();
        public override void ChooseDay(int id) => Fill();
    }
}