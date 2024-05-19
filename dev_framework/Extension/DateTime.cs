using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class DateTimeExtension
    {
        public static string ToSqlDate(this DateTime date, bool? endOfDay = null)
        {
            var month = date.Month > 9 ? date.Month.ToString() : string.Format("0{0}", date.Month.ToString());
            var day = date.Day > 9 ? date.Day.ToString() : string.Format("0{0}", date.Day.ToString());


            var hour = endOfDay.HasValue
                ? endOfDay.Value ? "23" : "00"
                : date.Hour > 9
                    ? date.Hour.ToString()
                    : string.Format("0{0}", date.Hour.ToString());

            var minutes = endOfDay.HasValue
                ? endOfDay.Value ? "59" : "00"
                : date.Minute > 9
                    ? date.Minute.ToString()
                    : string.Format("0{0}", date.Minute.ToString());

            var seconds = endOfDay.HasValue
                ? endOfDay.Value ? "59" : "00"
                : date.Second > 9
                    ? date.Second.ToString()
                    : string.Format("0{0}", date.Second.ToString());

            var millisecondes = endOfDay.HasValue
                ? endOfDay.Value ? "9999999" : "0000000"
                : date.Ticks.ToString().Substring(0, 7);

            return string.Format("{0}-{1}-{2} {3}:{4}:{5}.{6}", date.Year, month, day, hour, minutes, seconds, millisecondes);
        }

        public static DateTime ToEndOfDay(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59, 999);
        }

        public static DateTime ToStartOfDay(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, 0);
        }
    }
}
