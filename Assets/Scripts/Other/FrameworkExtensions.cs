using NPOI.SS.UserModel;
using System;
using UnityEngine;

/* Методы расширения
 */
public static partial class FrameworkExtensions
{
    public static DayOfWeek ToDayOfWeek(this int n)
    {
        return n == 6 ? DayOfWeek.Sunday : (DayOfWeek)(n+1);
    }
    public static string ToTimeString(this TimeSpan a)
    {
        string res = "";
        if (a.Hours < 10) res += "0";
        res += a.Hours + ":";
        if (a.Minutes < 10) res += "0";
        res += a.Minutes;
        return res;
    }
    public static string ToTimeString(this TimeSpan? a)
    {
        if (a == null) return "--:--";
        return a.Value.ToTimeString();
    }
    public static TimeSpan ToTimeSpan(this DateTime dt)
    {
        TimeSpan res = new TimeSpan(0, dt.Hour, dt.Minute, dt.Second, 0);
        return res;
    }
    public static string Clear(this string s)
    {
        return s.Replace(" ", string.Empty)
            .Replace(Environment.NewLine, string.Empty);
    }

    public static string Cell (this ICell c)
    {
        if (c == null) return "";

        switch (c.CellType)
        {
            case CellType.Blank: return "";
            case CellType.Boolean: return c.BooleanCellValue.ToString();
            case CellType.Error: return "";
            case CellType.Formula: return "";
            case CellType.Numeric: return c.NumericCellValue.ToString();
            case CellType.String: return c.StringCellValue;
            case CellType.Unknown: return "";
            default: return "";
        }
    }

    public static string Cell(this IRow r, int id)
    {
        if (r == null) return "";
        ICell c = r.GetCell(id);
        if (c == null) return "";
        return c.Cell();
    }

    public static string Cell(this ISheet s, int row, int id)
    {
        if (s == null) return "";
        IRow r = s.GetRow(row);
        if (r == null) return "";
        return r.Cell(id);
    }

    public static string GetCell(this ISheet s, int row, int id) => Cell(s, row, id);

    public static string ConvertToString(this DayOfWeek dow, bool abbreviated = true)
    {
        switch (dow)
        {
            case DayOfWeek.Monday:
                return (abbreviated ? "пн" : "Понедельник");
            case DayOfWeek.Tuesday:
                return (abbreviated ? "вт" : "Вторник");
            case DayOfWeek.Wednesday:
                return (abbreviated ? "ср" : "Среда");
            case DayOfWeek.Thursday:
                return (abbreviated ? "чт" : "Четверг");
            case DayOfWeek.Friday:
                return (abbreviated ? "пт" : "Пятница");
            case DayOfWeek.Saturday:
                return (abbreviated ? "сб" : "Суббота");
            case DayOfWeek.Sunday:
                return (abbreviated ? "вс" : "Воскресенье");
            default:
                return null;
        }
    }

    public static string ConvertToString(this int dow, bool abbreviated = true)
    {
        switch (dow)
        {
            case 0:
                return (abbreviated ? "пн" : "Понедельник");
            case 1:
                return (abbreviated ? "вт" : "Вторник");
            case 2:
                return (abbreviated ? "ср" : "Среда");
            case 3:
                return (abbreviated ? "чт" : "Четверг");
            case 4:
                return (abbreviated ? "пт" : "Пятница");
            case 5:
                return (abbreviated ? "сб" : "Суббота");
            case 6:
                return (abbreviated ? "вс" : "Воскресенье");
            default:
                return null;
        }
    }

    public static int Normalising(this DayOfWeek dow)
    {
        if (dow == 0)
            return 6;
        else
            return (int)dow - 1;
    }

    public static Vector3 NegativeX(this Vector3 v)
    {
        return new Vector3(-v.x, v.y, v.z);
    }
}