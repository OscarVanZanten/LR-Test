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
    }
}
