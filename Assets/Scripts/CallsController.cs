using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Networking;

namespace Stand
{
    public class CallsController : Singleton<CallsController>
    {
        private TimeSpan TimeToCall;
        private bool TimeSet = false;

        void FixedUpdate()
        {
            if (TimeSet)
            {
                if (DateTime.Now.TimeOfDay >= TimeToCall)
                {
                    Call();
                    TimeSet = false;
                }
            }
            else
            {
                TimeSpan? Next = CallsParser.Instance.NextCall();
                if (Next != null)
                {
                    TimeToCall = Next.Value;
                    TimeSet = true;
                    Loger.Log("звонок", $"время звонка установлено на:{TimeToCall}");
                }
            }
        }

        public void Call(int t = 2000)
        {
            UnityWebRequest uwr = UnityWebRequest.Get("http://192.168.1.211/Call?c=" + t);
            uwr.SendWebRequest();
            Loger.Log("звонок", "включен звонок продолжительность:" + t);
        }
    }
}