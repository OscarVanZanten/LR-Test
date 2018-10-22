using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace LR_Test.ReinforcementLearning.Algoritms.QLearning
{
    public class QLearningTabulair : QValueGame
    {
        private readonly double[][] qtable; // Q value table

        public QLearningTabulair(int width, int height, int[] level, int spawnX, int spawnY, double alpha, double epsilon, double gamma) : base(width, height, level, spawnX, spawnY, alpha, epsilon, gamma)
        {
            this.qtable = new double[width * height][];
        }

        public QLearningTabulair(double alpha, double epsilon, double gamma) : base(alpha, epsilon, gamma)
        {
            this.qtable = new double[width * height][];
        }

        protected override int DetermineMove(int x, int y, int episode)
        {
            bool randomMove = random.NextDouble() < epsilon;

            if (randomMove)
            {
                return random.Next(0, 4);
            }
            else
            {
                var qvalues = QValues(x, y);
                double highestValue = HighestQValues(x, y);
                int highestValueIndex = Array.IndexOf<double>(qvalues, highestValue);

                if (highestValue == 0)
                {
                    return random.Next(0, 4);
                }

                return highestValueIndex;
            }
        }

        protected override double CalculateUpdatedQValue(double qvalue1, double qvalue2, double reward)
        {
            return qvalue1 + alpha * (reward + gamma * qvalue2 - qvalue1);
        }

        protected override double[] QValues(int x, int y)
        {
            if (qtable[x + y * width] == null)
            {
                qtable[x + y * width] = new double[4];
            }
            return qtable[x + y * width];
        }

        protected override void SetQValues(int x, int y ,double[] values)
        {
            qtable[x + y * width] = values;
        }
    }
}
