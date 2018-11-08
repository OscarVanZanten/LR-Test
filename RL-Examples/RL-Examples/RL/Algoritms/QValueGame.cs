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

        /// <summary>
        /// Gets Q-Values for position x and y for every action
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        protected abstract double[] QValues(int x, int y);

        /// <summary>
        /// Sets Q-Values for position x and y for every action
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="values"></param>
        protected abstract void SetQValues(int x, int y, double[] values);

        /// <summary>
        /// Gets highest Q-Value for int x and y
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>Highest Q-Value</returns>
        protected double HighestQValue(int x, int y)
        {
            var qvalues = QValues(x, y);
            return qvalues.OrderByDescending(q => q).First();
        }

        /// <summary>
        /// Gets highest Q-Value for a list of Q-Values
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>Highest Q-Value</returns>
        protected double HighestQValue(double[] qvalues)
        {
            return qvalues.OrderByDescending(q => q).First();
        }

        /// <summary>
        /// Calculate updated Q-value
        /// </summary>
        /// <param name="previousQValue"></param>
        /// <param name="nextQValue"></param>
        /// <param name="reward"></param>
        /// <returns>Updated Q-Value</returns>
        protected double CalculateUpdatedQValue(double previousQValue, double nextQValue, double reward)
        {
            return previousQValue + alpha * (reward + gamma * nextQValue - previousQValue);
        }
    }
}
