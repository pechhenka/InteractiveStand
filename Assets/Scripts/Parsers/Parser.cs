using System;
using System.Collections.Generic;

namespace Stand
{
    public class Parser<T> where T : new()
    {
        private static T _instance;
        private static object _lock = new object();

        public static T Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                        _instance = new T();

                    return _instance;
                }
            }
        }

        private readonly char[] SeparatorTime = { ':', '.' };
        private readonly char[] SeparatorCommand = { ';' };
        private readonly char[] SeparatorDashes = { '-', '—' };
        private readonly char[] SeparatorDate = { '.', ',' };

        #region Dates
        public List<DateRange> DatesRange(in string s)
        {
            string[] gap = s.Clear().Split(SeparatorCommand);

            DateRange dt = new DateRange(false, default, default);
            int len;
            string[] a;
            List<DateRange> res = new List<DateRange>();
            foreach (string c in gap)
            {
                if (c == string.Empty) continue;

                a = c.Split(SeparatorDashes);
                len = a.Length;

                if (len == 1)
                {
                    dt.Twins = false;
                    dt.Left = ToDate(a[0]);
                    res.Add(dt);
                }
                else if (len == 2)
                {
                    dt.Twins = true;
                    dt.Left = ToDate(a[0]);
                    dt.Right = ToDate(a[1]);

                    res.Add(dt);
                }
                else
                    Loger.Warning("Parser", "неправильная часть команды:" + s + "&" + c);
            }

            return res;
        }

        public List<DateTime> Dates(in string s)
        {
            string[] gap = s.Clear().Split(SeparatorCommand);

            DateTime dt0, dt1;
            int len;
            string[] a;
            List<DateTime> res = new List<DateTime>();
            foreach (string c in gap)
            {
                if (c == string.Empty) continue;

                a = c.Split(SeparatorDashes);
                len = a.Length;

                if (len == 1)
                {
                    res.Add(ToDate(a[0]));
                }
                else if (len == 2)
                {
                    dt0 = ToDate(a[0]);
                    dt1 = ToDate(a[1]);

                    for (var day = dt0.Date; day.Date <= dt1.Date; day = day.AddDays(1))
                        res.Add(day);
                }
                else
                    Loger.Warning("Parser", "неправильная часть команды:" + s + "&" + c);
            }

            return res;
        }

        public bool ContainsDate(in string s, in DateTime dt)
        {
            string[] gap = s.Clear().Split(SeparatorCommand);

            DateTime dt0, dt1;
            int len;
            string[] a;
            foreach (string c in gap)
            {
                if (c == string.Empty) continue;

                a = c.Split(SeparatorDashes);
                len = a.Length;

                if (len == 1)
                {
                    dt0 = ToDate(a[0]);

                    if (dt0.Date == dt.Date)
                        return true;
                }
                else if (len == 2)
                {
                    dt0 = ToDate(a[0]);
                    dt1 = ToDate(a[1]);

                    if (dt0.Date <= dt.Date && dt.Date <= dt1.Date)
                        return true;
                }
                else
                    Loger.Warning("Parser", "неправильная часть команды:" + s + "&" + c);
            }

            return false;
        }

        public DateTime ToDate(in string s)
        {
            string[] gap = s.Clear()
                .Split(SeparatorDate);

            DateTime res = new DateTime(
                Convert.ToInt32(gap[2]),
                Convert.ToInt32(gap[1]),
                Convert.ToInt32(gap[0])
                );

            return res;
        }
        #endregion

        #region Times
        public TimeSpan LeftCall(in string s)
        {
            return ToTime(s.Clear()
                .Split(SeparatorDashes)[0]);
        }

        public TimeSpan RightCall(in string s)
        {
            return ToTime(s.Clear()
                .Split(SeparatorDashes)[1]);
        }

        public List<TimeSpan> Times(in string s)
        {
            string[] gap = s.Clear().Split(SeparatorCommand);

            int len;
            string[] a;
            List<TimeSpan> res = new List<TimeSpan>();
            foreach (string c in gap)
            {
                if (c == string.Empty) continue;

                a = c.Split(SeparatorDashes);
                len = a.Length;

                if (len == 1)
                {
                    res.Add(ToTime(a[0]));
                }
                else if (len == 2)
                {
                    res.Add(ToTime(a[0]));
                    res.Add(ToTime(a[1]));
                }
                else
                    Loger.Warning("Parser", "неправильная часть команды:" + s + "&" + c);
            }

            return res;
        }

        public List<TimeSpan> Times(in List<string> l)
        {
            List<TimeSpan> gap = new List<TimeSpan>();

            l.ForEach(x => gap.AddRange(Times(x)));

            return gap;
        }

        public bool ContainsCall(in string s, in TimeSpan ts)
        {
            string[] gap = s.Clear().Split(SeparatorCommand);

            TimeSpan ts0, ts1;
            int len;
            string[] a;
            foreach (string c in gap)
            {
                if (c == string.Empty) continue;

                a = c.Split(SeparatorDashes);
                len = a.Length;

                if (len == 1)
                {
                    ts0 = ToTime(a[0]);

                    if (ts0 == ts)
                        return true;
                }
                else if (len == 2)
                {
                    ts0 = ToTime(a[0]);
                    ts1 = ToTime(a[1]);

                    if (ts0 <= ts && ts <= ts1)
                        return true;
                }
                else
                    Loger.Warning("Parser", "неправильная часть команды:" + s + "&" + c);
            }

            return false;
        }

        public bool CheckClassroomHour(in string s) => s.Contains(".");

        public TimeSpan ToTime(in string s)
        {
            string[] gap = s.Clear()
                .Split(SeparatorTime);

            TimeSpan res = new TimeSpan(
                Convert.ToInt32(gap[0]),
                Convert.ToInt32(gap[1]),
                0
                );

            return res;
        }
        #endregion
    }
}