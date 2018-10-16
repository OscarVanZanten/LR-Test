using System;
using System.Collections.Generic;
using System.Text;

namespace LR_Test.Util
{
    public class MathHelper
    {
        public static double Sigmoid(double x)
        {
            return 2 / (1 + Math.Exp(-2 * x)) - 1;
        }

        public static double Derivative(double x)
        {
            double s = Sigmoid(x);
            return (1 - (Math.Pow(s, 2)));
        }
    }
}
