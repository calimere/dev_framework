using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class ObjectExtension
    {
        public static Dictionary<string, object> PropertyToDictionary(this object obj)
        {
            var type = obj.GetType();
            return type.GetProperties().Where(m => m.PropertyType.IsValueType || m.PropertyType.Name == "String").ToDictionary(property => property.Name, property => property.GetValue(obj));
        }
        public static IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }
        public static IEnumerable<DateTime> EachMonth(DateTime from, DateTime thru)
        {
            var d = new List<DateTime>();
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
            {
                var date = new DateTime(day.Year, day.Month, 1);
                if (!d.Contains(date))
                    d.Add(date);
            }
            return d.ToArray();
        }
        public static IEnumerable<DateTime> EachYear(DateTime from, DateTime thru)
        {
            var d = new List<DateTime>();
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
            {
                var date = new DateTime(day.Year, 1, 1);
                if (!d.Contains(date))
                    d.Add(date);
            }
            return d.ToArray();
        }
        public static WeekPeriod[] EachWeek(DateTime from, DateTime thru)
        {
            var d = new List<WeekPeriod>();
            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
            Calendar cal = dfi.Calendar;

            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
            {
                var v = cal.GetWeekOfYear(day, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);

                if (!d.Any(m => m.WeekNumber == v))
                    d.Add(new WeekPeriod { WeekNumber = v, Start = GetStartOfWeek(day), End = GetEndOfWeek(day) });
            }
            return d.ToArray();
        }
        private static DateTime GetStartOfWeek(DateTime d)
        {
            int delta = DayOfWeek.Monday - d.DayOfWeek;
            DateTime monday = d.AddDays(delta);
            return monday;
        }
        private static DateTime GetEndOfWeek(DateTime d)
        {
            int delta = d.DayOfWeek-DayOfWeek.Sunday;
            DateTime monday = d.AddDays(7 -delta);
            return monday;
        }
        public static SelectListItem[] ToSelectListItems<T>(this IEnumerable<T> obj, string value, string text)
        {
            return obj.Select(m => new SelectListItem { Text = m.GetType().GetProperty(text).GetValue(m).ToString(), Value = m.GetType().GetProperty(value).GetValue(m).ToString() }).ToArray();
        }
        

        public class WeekPeriod
        {
            public int WeekNumber { get; set; }
            public DateTime Start { get; set; }
            public DateTime End { get; set; }
        }
    }
}
