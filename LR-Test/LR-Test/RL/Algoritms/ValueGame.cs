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

        protected abstract double Value(int x, int y);

        protected abstract void SetValue(int x, int y, double value);

        protected double CalculateUpdatedValue(double value1, double value2, double reward)
        {
            return (1- alpha) *value1 + alpha * (reward + gamma * value2);
        }
    }
}
