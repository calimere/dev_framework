using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class DoubleExtension
    {
        public static double GetPourcentage(this int value, int reference)
        {
            return GetPourcentage(Convert.ToDouble(value), Convert.ToDouble(reference));
        }

        public static double GetPourcentage(this int value, double reference)
        {
            return GetPourcentage(Convert.ToDouble(value), reference);
        }

        public static double GetPourcentage(this double value, double reference)
        {
            if (reference > 0)
                return (value * 100) / reference;

            return 0;
        }
        public static double ToTTC(this double value, double reference)
        {
            return Math.Round(value * (1 + (reference / 100)),2);
        }
        public static double GetTVA(this double value, double reference)
        {
            return (value * reference / 100);
        }
        public static string ToEuro(this double value)
        {
            return value.ToString("C2", CultureInfo.CreateSpecificCulture("fr-fr"));
        }
        public static string ToPourcentage(this double value)
        {
            return string.Format("{0} %", value);
        }

    }
}
