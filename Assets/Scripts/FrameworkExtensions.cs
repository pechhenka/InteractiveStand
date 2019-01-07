using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace WorkBrew
{
    public static partial class FrameworkExtensions
    {

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
    }
}