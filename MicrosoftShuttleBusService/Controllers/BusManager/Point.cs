using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusManager
{
    public class Point
    {
        public double LocX;

        public double LocY;

        public Point(double x = 0, double y = 0)
        {
            LocX = x;
            LocY = y;
        }

        public static double ApproxDistance(Point s1, Point s2)
        {
            double PoleRadius = 6356.755;
            double EquaRadius = 6378.140;

            double x = 2 * 3.14 * 3284.985 * Math.Abs(s1.LocX - s2.LocX) / 360;
            double y = 2 * 3.14 * PoleRadius * Math.Abs(s1.LocY - s2.LocY) / 360;

            return Math.Sqrt(x * x + y * y);
        }

    }
}
