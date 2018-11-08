using System;
using System.Collections.Generic;
using System.Text;

namespace LR_Test.Util
{
    public class MathHelper
    {
        /// <summary>
        /// Sigmoid function onto x
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static double Sigmoid(double x)
        {
            return 1 / (1 + Math.Pow(Math.E, -x)) ;
        }
    }
}
