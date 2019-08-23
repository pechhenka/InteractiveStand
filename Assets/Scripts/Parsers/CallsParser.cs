using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Stand
{
    public class CallsParser : Parser<CallsParser>, IReceive<SignalCallsMatrixChanged>, IReceive<SignalChangeCallsMatrixChanged>
    {
        public bool Changes()
        {
            throw new NotImplementedException();
        }

        public TimeSpan? NextCall() => BordersCalls().Next;

        public TimeSpan? LastCall() => BordersCalls().Last;

        public (TimeSpan? Last, TimeSpan? Next) BordersCalls()
        {
            (TimeSpan? Last, TimeSpan? Next) res = (null, null);
            TimeSpan TimeSpanNow = DateTime.Now.ToTimeSpan();

            List<TimeSpan> gap = GetCurrentColumn();

            int len = gap.Count;
            if (TimeSpanNow <= gap[0]) { res.Next = gap[0]; return res; }
            if (TimeSpanNow >= gap[len - 1]) { res.Last = gap[len - 1]; return res; }
            for (int j = 0; j < len; j++)
                if (TimeSpanNow <= gap[j])
                {
                    res = (gap[j - 1], gap[j]);
                    break;
                }

            return res;
        }

        public bool CheckClassroomHour() => CheckClassroomHour(GetFirstLineCurrentColumn());

        /// <summary>
        /// &lt;0 номер перемены;
        /// <para/>=0 ничего;
        /// <para/>&gt;0 номер урока;
        /// </summary>
        /// <returns>int</returns>
        public int WhatNow()
        {
            List<TimeSpan> gap = GetCurrentColumn();
            TimeSpan TimeSpanNow = DateTime.Now.ToTimeSpan();

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

            return (Difference, TimeLeft);
        }

        public List<TimeSpan> GetCurrentColumnWithoutChanges()
        {
            List<TimeSpan> res = new List<TimeSpan>();

            DateTime DateTimeNow = DateTime.Now;

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

            int len = Data.Instance.CallsMatrix.LastRowNum;
            for (int j = 1; j <= len; j++)
            {
                string s = Data.Instance.CallsMatrix.Cell(j, index);
                if (s == "") continue;
                res.AddRange(Times(s));
            }

            return res;
        }

        public List<TimeSpan> GetColumnWithoutChanges(int index)
        {
            List<string> res = new List<string>();

            if (Data.Instance.CallsMatrix == null) return new List<TimeSpan>();

            int len = Data.Instance.CallsMatrix.LastRowNum;
            for (int j = 1; j <= len; j++)
            {
                string s = Data.Instance.CallsMatrix.Cell(j, index);
                if (s == "") continue;
                res.Add(s);
            }

            return Times(res);
        }

        public List<TimeSpan> GetCurrentColumn()
        {
            List<TimeSpan> res = new List<TimeSpan>();

            if (Data.Instance.ChangeCallsMatrix != null)
            {
                IRow DatesChangeRow = Data.Instance.ChangeCallsMatrix.GetRow(0);
                if (DatesChangeRow != null)
                {
                    DateTime DateTimeNow = DateTime.Now;
                    int len = DatesChangeRow.LastCellNum;
                    for (int i = 0; i <= len; i++)
                        if (ContainsDate(DatesChangeRow.Cell(i), DateTimeNow))
                        {
                            len = Data.Instance.ChangeCallsMatrix.LastRowNum;

                            for (int j = 1; j <= len; j++)
                            {
                                string s = Data.Instance.ChangeCallsMatrix.Cell(j, i);
                                if (s == "") continue;
                                res.AddRange(Times(s));
                            }

                            return res;
                        }
                }
            }

            return GetCurrentColumnWithoutChanges();
        }

        public List<(DateRange date, List<TimeSpan> times)> GetListChangesCalls()
        {
            var res = new List<(DateRange date, List<TimeSpan> times)>();

            if (Data.Instance.ChangeCallsMatrix == null) return res;

            IRow DatesRow = Data.Instance.ChangeCallsMatrix.GetRow(0);
            int LastCell = DatesRow.LastCellNum;
            int len = Data.Instance.ChangeCallsMatrix.LastRowNum;
            for (int i = 0; i <= LastCell; i++)
            {
                string DatesVal = DatesRow.Cell(i);
                if (DatesVal == "") continue;
                List<DateRange> dates = DatesRange(DatesVal);
                List<TimeSpan> calls = new List<TimeSpan>();
                for (int j = 1; j <= len; j++)
                {
                    string s = Data.Instance.ChangeCallsMatrix.Cell(j, i);
                    if (s == "") continue;
                    calls.AddRange(Times(s));
                }

                foreach (DateRange item in dates)
                    res.Add((item, calls));
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

        private string GetFirstLineCurrentColumn()
        {
            DateTime DateTimeNow = DateTime.Now;
            int len;

            if (Data.Instance.ChangeCallsMatrix != null)
            {
                IRow DatesChangeRow = Data.Instance.ChangeCallsMatrix.GetRow(0);
                if (DatesChangeRow != null)
                {
                    len = DatesChangeRow.LastCellNum;
                    for (int i = 0; i <= len; i++)
                        if (ContainsDate(DatesChangeRow.Cell(i), DateTimeNow))
                        {
                            len = Data.Instance.ChangeCallsMatrix.LastRowNum;

                            for (int j = 1; j <= len; j++)
                            {
                                string s = Data.Instance.ChangeCallsMatrix.Cell(j, i);
                                if (s == "") continue;
                                return s;
                            }

                            return "";
                        }
                }
            }


            if (Data.Instance.CallsMatrix == null) return "";

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
                return s;
            }

            return "";
        }
    }
}