using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;

namespace Stand
{
    public class CallsParser : Parser<CallsParser>, IReceive<SignalCallsMatrixChanged>, IReceive<SignalChangeCallsMatrixChanged>
    {
        public TimeSpan? NextCall() => BordersCalls().Next;

        public TimeSpan? LastCall() => BordersCalls().Last;

        public (TimeSpan? Last,TimeSpan? Next) BordersCalls()
        {
            (TimeSpan? Last, TimeSpan? Next) res = (null, null);
            TimeSpan TimeSpanNow = DateTime.Now.ToTimeSpan();

            List<TimeSpan> gap = new List<TimeSpan>();
            GetCurrentColumn().ForEach(x => gap.AddRange(Times(x)));

            int len = gap.Count;
            if (TimeSpanNow <= gap[0]) { res.Next = gap[0]; return res; }
            if (TimeSpanNow >= gap[len - 1]) { res.Last = gap[len - 1]; return res; }
            for (int j = 0; j < len; j++)
                if (TimeSpanNow <= gap[j])
                    res = (gap[j - 1], gap[j]);

            return res;
        }

        public bool CheckClassroomHour() => CheckClassroomHour(GetCurrentColumn()[0]);

        /// <summary>
        /// &lt;0 номер перемены;
        /// <para/>=0 ничего;
        /// <para/>&gt;0 номер урока;
        /// </summary>
        /// <returns>int</returns>
        public int WhatNow()
        {
            List<TimeSpan> gap = new List<TimeSpan>();
            TimeSpan TimeSpanNow = DateTime.Now.ToTimeSpan();

            GetCurrentColumn().ForEach(x => gap.AddRange(Times(x)));

            if (TimeSpanNow < gap[0]) return 0;

            int len = gap.Count;
            for (int i = 0; i < len; i++)
            {
                if (TimeSpanNow >= gap[i]) continue;
                return (i % 2 == 0) ? i / -2 : (i + 1) / 2;
            }

            return len / -2;
        }

        public (int Difference, int TimeLeft) AttitudeCalls()
        {
            (TimeSpan? Last, TimeSpan? Next) res = BordersCalls();

            int Difference = 0;
            int TimeLeft = 0;

            TimeSpan RightBorderTS = res.Next ?? new TimeSpan();
            TimeSpan LeftBorderTS = res.Last ?? new TimeSpan();

            if (res.Next != null)
            {
                TimeSpan TimeCurrentTS = DateTime.Now.ToTimeSpan();
                TimeLeft = (int)(RightBorderTS - TimeCurrentTS).TotalSeconds;
                if (res.Last != null)
                    Difference = (int)(RightBorderTS - LeftBorderTS).TotalSeconds;
            }                

            return ( Difference, TimeLeft );
        }

        public List<string> GetCurrentColumn()
        {
            List<string> res = new List<string>();

            DateTime DateTimeNow = DateTime.Now;
            int len;

            if (Data.Instance.ChangeCallsMatrix != null)
            {
                IRow DatesChangeRow = Data.Instance.ChangeCallsMatrix.GetRow(0);
                if (DatesChangeRow != null)
                {
                    len = DatesChangeRow.LastCellNum;
                    for (int i = 1; i <= len; i++)
                        if (ContainsDate(DatesChangeRow.Cell(i), DateTimeNow))
                        {
                            len = Data.Instance.ChangeCallsMatrix.LastRowNum;

                            for (int j = 0; j <= len; j++)
                            {
                                string s = Data.Instance.ChangeCallsMatrix.Cell(j, i);
                                if (s == "") continue;
                                res.Add(s);
                            }

                            return res;
                        }
                }
            }

            if (Data.Instance.CallsMatrix == null) return res;

            int index = 0;
            switch (DateTimeNow.DayOfWeek)
            {
                case DayOfWeek.Tuesday:
                    index = 1;
                    break;
                case DayOfWeek.Saturday:
                    index = 2;
                    break;
            }

            len = Data.Instance.CallsMatrix.LastRowNum;
            for (int j = 1; j <= len; j++)
            {
                string s = Data.Instance.CallsMatrix.Cell(j, index);
                if (s == "") continue;
                res.Add(s);
            }

            return res;
        }

        void IReceive<SignalCallsMatrixChanged>.HandleSignal(SignalCallsMatrixChanged arg)
        {
            throw new NotImplementedException();
        }

        void IReceive<SignalChangeCallsMatrixChanged>.HandleSignal(SignalChangeCallsMatrixChanged arg)
        {
            throw new NotImplementedException();
        }
    }
}