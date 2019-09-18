using System;
using UnityEngine.Networking;

namespace Stand
{
    public class CallsController : Singleton<CallsController>, IReceive<SignalCallsMatrixChanged>, IReceive<SignalChangeCallsMatrixChanged>
    {
        private TimeSpan TimeToCall;
        private bool TimeSetToCall = false;

        private TimeSpan TimeToPrecedingCall;
        private bool TimeSetToPrecedingCall = false;

        void Start()
        {
            ProcessingSignals.Default.Add(this);
        }
        void FixedUpdate()
        {
            if (!Data.Instance.CurrentManifest.SupportAutomaticCalling)
            {
                TimeSetToCall = false;
                return;
            }

            if (TimeSetToCall || TimeSetToPrecedingCall)
            {
                if (TimeSetToCall && DateTime.Now.TimeOfDay >= TimeToCall)
                {
                    Call();
                    TimeSetToCall = false;
                }
                if (TimeSetToPrecedingCall && DateTime.Now.TimeOfDay >= TimeToPrecedingCall)
                {
                    PrecedingCall();
                    TimeSetToCall = false;
                }
            }
            else
            {
                TimeSpan? Next = CallsParser.Instance.NextCall();
                if (Next != null)
                {
                    TimeToCall = Next.Value;
                    if (CallsParser.Instance.WhatNow() <= 0)
                    {
                        TimeToPrecedingCall = TimeToCall - TimeSpan.FromSeconds(Data.Instance.CurrentManifest.PrecedingCallTime);
                        TimeSetToPrecedingCall = true;
                    }
                    TimeSetToCall = true;
                    Loger.Log("звонок", $"время звонка установлено на:{TimeToCall}");
                }
            }
        }

        public void PrecedingCall()
        {
            UnityWebRequest.Get(Data.Instance.CurrentManifest.WebsiteAddress + "Call?" + Data.Instance.CurrentManifest.PrecedingCallOptions).SendWebRequest();

            Loger.Log("звонок", "включен предшествующий звонок параметр:" + Data.Instance.CurrentManifest.PrecedingCallOptions);
        }
        public void Call()
        {
            UnityWebRequest.Get(Data.Instance.CurrentManifest.WebsiteAddress + "Call?c=" + Data.Instance.CurrentManifest.CallOptions).SendWebRequest();

            Loger.Log("звонок", "включен звонок параметр:" + Data.Instance.CurrentManifest.CallOptions);
        }

        void IReceive<SignalCallsMatrixChanged>.HandleSignal(SignalCallsMatrixChanged arg) => TimeSetToCall = false;
        void IReceive<SignalChangeCallsMatrixChanged>.HandleSignal(SignalChangeCallsMatrixChanged arg) => TimeSetToCall = false;
    }
}