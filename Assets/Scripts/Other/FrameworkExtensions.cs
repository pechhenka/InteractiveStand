using NPOI.SS.UserModel;
using System;
using UnityEngine;

/* Класс содержит только методы расширения
 * Просто облегчает жизнь при написании кода :)
 * Вот измените мне альфа канал в Color без него :)
 * Нет, вы конечно можете изменить его, но это не круто каждыйраз писать одно и тоже
 */
public static partial class FrameworkExtensions
{

    public static string GetCell(this ISheet s, int row, int id)
    {
        IRow r = s.GetRow(row);
        if (r == null) return "";
        ICell c = r.GetCell(id);
        if (c == null) return "";

        switch(c.CellType)
        {
            case CellType.Blank: return "";
            case CellType.Boolean: return c.BooleanCellValue.ToString();
            case CellType.Error: return "";
            case CellType.Formula: return "";
            case CellType.Numeric: return c.NumericCellValue.ToString();
            case CellType.String: return c.StringCellValue;
            case CellType.Unknown: return "";
        }
        return c == null ? "" : c.StringCellValue;
    }

    public static Color SetAlpha(this Color c, float alpha)
    {
        return new Color(c.r, c.g, c.b, alpha);
    }

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

    public static Vector3 SetX(this Vector3 v, float x)
    {
        return new Vector3(x, v.y, v.z);
    }

    public static Vector3 SetY(this Vector3 v, float y)
    {
        return new Vector3(v.x, y, v.z);
    }

    public static Vector3 SetZ(this Vector3 v, float z)
    {
        return new Vector3(v.x, v.y, z);
    }

    public static Vector3 NegativeX(this Vector3 v)
    {
        return new Vector3(-v.x, v.y, v.z);
    }

    public static Vector2 SetX(this Vector2 v, float x)
    {
        return new Vector3(x, v.y);
    }

    public static Vector2 SetY(this Vector2 v, float y)
    {
        return new Vector2(v.x, y);
    }

    public static float GetH(this Color c)
    {
        float h;
        float s;
        float v;
        Color.RGBToHSV(c, out h, out s, out v);
        return h;
    }

    public static float GetS(this Color c)
    {
        float h;
        float s;
        float v;
        Color.RGBToHSV(c, out h, out s, out v);
        return s;
    }

    public static float GetV(this Color c)
    {
        float h;
        float s;
        float v;
        Color.RGBToHSV(c, out h, out s, out v);
        return v;
    }
}