using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using LR_Test.RL.NeuralNetwork;

namespace LR_Test.RL.Algoritms.Temporal_Difference
{
    public class TemporalDifferenceTabulair : ValueGame
    {
        private readonly double[] vtable;
        private readonly int maxPolicyEpisodes;

        public TemporalDifferenceTabulair(double alpha, double epsilon, double gamma) : base(alpha, epsilon, gamma)
        {
            this.maxPolicyEpisodes = 10000;
            this.vtable = new double[width * height];
        }

        public TemporalDifferenceTabulair(int width, int height, int[] level, int spawnX, int spawnY, double alpha, double epsilon, double gamma) : base(width, height, level, spawnX, spawnY, alpha, epsilon, gamma)
        {
            this.maxPolicyEpisodes = 10000;
            this.vtable = new double[width * height];
        }

        public override void TakeTurn(int episode)
        {
            int agentx1 = agentX;
            int agenty1 = agentY;
            var move = DetermineMove(agentX, agentY, episode);

            var value1 = Value(agentX, agentY);
            var reward = MakeMove(move);

            var value2 = Value(agentX, agentY);

            var update = CalculateUpdatedValue(value1, value2, reward);

            SetValue(agentx1, agenty1, update);

            CheckGameState(reward);
        }

        protected override int DetermineMove(int x, int y, int episode)
        {
            double epsilonDiscount = episode > 0 ? (episode / (maxPolicyEpisodes * 1.0)) * epsilon : 0;
            double finalEpsilon = epsilon - (epsilonDiscount > epsilon ? epsilon : epsilonDiscount);
            bool randomMove = random.NextDouble() < (finalEpsilon);
            Console.WriteLine($"{finalEpsilon}");

            var values = new double[]
               {
                    Value(x, y-1),
                    Value(x, y+1),
                    Value(x-1, y),
                    Value(x+1, y),
               };

            if (randomMove)
            {
                int move = random.Next(0, 4);

                while (values[move] == -1)
                {
                    Console.WriteLine($"{move} {values[move]}");
                    move = random.Next(0, 4);
                }
                //Console.WriteLine($"{move} {values[move]}");
                return move;
            }
            else
            {
                double highestValue = values.OrderByDescending(v => v).First();
                int highestValueIndex = Array.IndexOf<double>(values, highestValue);

                if (highestValue == 0)
                {
                    return random.Next(0, 4);
                }

                return highestValueIndex;
            }
        }

        protected override double Value(int x, int y)
        {
            if (x < 0 || y < 0 || x > (width - 1) || y > (height - 1))
            {
                return -1;
            }
            if (level[x + y * width] == 1) { return -1; }
            return vtable[x + y * width];
        }

        protected override void SetValue(int x, int y, double value)
        {
            vtable[x + y * width] = value;
        }

        public override string ToString()
        {
            string result = base.ToString() + '\n' + '\n';

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    result += $" {vtable[x + y * width].ToString("G3")}";
                }
                result += '\n';
            }

            return result;
        }
    }
}
