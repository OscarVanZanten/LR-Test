using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace LR_Test.RL.Algoritms
{
    public abstract class QValueGame : RLGame
    {
        public QValueGame(int width, int height, int[] level, int spawnX, int spawnY, double alpha, double epsilon, double gamma) : base(width, height, level, spawnX, spawnY, alpha, epsilon, gamma) { }

        public QValueGame(double alpha, double epsilon, double gamma) : base(alpha, epsilon, gamma) { }

       

        protected abstract double[] QValues(int x, int y);
        protected abstract void SetQValues(int x, int y, double[] values);

        protected double HighestQValues(int x, int y)
        {
            var qvalues = QValues(x, y);
            return qvalues.OrderByDescending(q => q).First();
        }

        protected double HighestQValues(double[] qvalues)
        {
            return qvalues.OrderByDescending(q => q).First();
        }


        protected double CalculateUpdatedQValue(double qvalue1, double qvalue2, double reward)
        {
            return qvalue1 + alpha * (reward + gamma * qvalue2 - qvalue1);
        }

    }
}
