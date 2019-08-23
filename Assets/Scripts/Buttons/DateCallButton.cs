using System;
using System.Collections.Generic;
using UnityEngine;

namespace Stand
{
    public class DateCallButton : MonoBehaviour
    {
        public (DateRange date, List<TimeSpan> times) item;
        UIController UIC;
        bool FirstEnable = false;

        void OnEnable()
        {
            if (FirstEnable)
                return;

            FirstEnable = true;
            UIC = GameObject.Find("[UI]").GetComponent<UIController>();
        }

        public void OnClick()
        {
            (UIC.ChangeCallsWindow as ChangeCallsWindow).Choose(item);
        }
    }
}