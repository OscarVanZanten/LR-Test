using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace LR_Test.RL.Algoritms
{
    public abstract class ValueGame : RLGame
    {
        public ValueGame(int width, int height, int[] level, int spawnX, int spawnY, double alpha, double epsilon, double gamma) : base(width, height, level, spawnX, spawnY, alpha, epsilon, gamma) { }

        public ValueGame(double alpha, double epsilon, double gamma) : base(alpha, epsilon, gamma) { }

        /// <summary>
        /// Gets values for position x and y 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        protected abstract double Value(int x, int y);

        /// <summary>
        /// Sets values for position x and y 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        protected abstract void SetValue(int x, int y, double value);

        /// <summary>
        /// Calculate updated value
        /// </summary>
        /// <param name="previousvalue"></param>
        /// <param name="nextValue"></param>
        /// <param name="reward"></param>
        /// <param name="elegibility"></param>
        /// <returns></returns>
        protected double CalculateUpdatedValue(double previousvalue, double nextValue, double reward, double elegibility)
        {
            return previousvalue + alpha * (reward + gamma * nextValue - nextValue) * elegibility;
        }
    }
}
