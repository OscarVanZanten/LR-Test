using System;
using System.Collections.Generic;
using System.Text;

namespace LR_Test.Util
{
    public class MathHelper
    {
        public static double Sigmoid(double x)
        {
            return Math.Pow(Math.E, x) / (1 + Math.Pow(Math.E, x)) ;
        }

        public static double Derivative(double x)
        {
            double s = Sigmoid(x);
            return (1 - (Math.Pow(s, 2)));
        }
    }
}
