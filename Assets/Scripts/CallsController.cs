using System;
using UnityEngine.Networking;

namespace Stand
{
    public class CallsController : Singleton<CallsController>
    {
        private TimeSpan TimeToCall;
        private bool TimeSet = false;

        void FixedUpdate()
        {
            if (!Data.Instance.CurrentManifest.SupportAutomaticCalling)
            {
                TimeSet = false;
                return;
            }

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
            UnityWebRequest.Get(Data.Instance.CurrentManifest.WebsiteAddress + "Call?c=" + t).SendWebRequest();

            Loger.Log("звонок", "включен звонок продолжительность:" + t);
        }
    }
}