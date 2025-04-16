using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3_WPF
{
    public static class Cassini
    {
        public static double CassiniUpper(double x, double a, double c)
        {
            double innerSqrt = Math.Pow(a, 4) + 4 * c * c * x * x;
            if (innerSqrt < 0) return double.NaN;

            double sqrtPart = Math.Sqrt(innerSqrt);
            double resultInside = sqrtPart - x * x - c * c;
            if (resultInside < 0) return double.NaN;

            return Math.Sqrt(resultInside);
        }

        public static double CassiniLower(double x, double a, double c)
        {
            double innerSqrt = Math.Pow(a, 4) + 4 * c * c * x * x;
            if (innerSqrt < 0) return double.NaN;

            double sqrtPart = Math.Sqrt(innerSqrt);
            double resultInside = sqrtPart - x * x - c * c;
            if (resultInside < 0) return double.NaN;

            return -Math.Sqrt(resultInside);
        }
    }
}
