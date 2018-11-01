using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using LR_Test.RL.NeuralNetwork;
using LR_Test.RL.Algoritms.Models;

namespace LR_Test.RL.Algoritms.Temporal_Difference
{
    public class TemporalDifferenceNeuralNetwork : ValueGame
    {
        private SimpleNN neuralNetwork;

        private readonly double[] etable;
        private readonly double lambda;

        private readonly int maxPolicyEpisodes;

        public TemporalDifferenceNeuralNetwork(double alpha, double epsilon, double gamma, double lambda) : base(alpha, epsilon, gamma)
        {
            this.lambda = lambda;
            this.neuralNetwork = new SimpleNN(.033f, width * height, 8, 1);
            this.maxPolicyEpisodes = 1000;
            this.etable = new double[width * height];
        }

        public TemporalDifferenceNeuralNetwork(int width, int height, int[] level, int spawnX, int spawnY, double alpha, double epsilon, double gamma, double lambda) : base(width, height, level, spawnX, spawnY, alpha, epsilon, gamma)
        {
            this.lambda = lambda;
            this.neuralNetwork = new SimpleNN(.033f, width * height, 8, 1);
            this.maxPolicyEpisodes = 1000;
            this.etable = new double[width * height];
        }

        protected double CalculateUpdatedValue(double value1, double value2, double reward, double elegibility)
        {
            return value1 + alpha * (reward + gamma * value2 - value2) * elegibility;
        }

        public override void TakeTurn(int episode)
        {
            SetElegbility(agentX, agentY, Elegbility(agentX, agentY) + 1);

            var state_1 = Value(agentX, agentY);
            var move = DetermineMove(agentX, agentY, episode);
            var reward = MakeMove(move);
            var state_2 = Value(agentX, agentY);

            var delta = reward + gamma * state_2 - state_1;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    double value = Value(x, y) + alpha * delta * etable[x + y * width];
                    Value(x, y);
                    SetValue(x, y, value);
                    SetElegbility(agentX, agentY, Elegbility(agentX, agentY) * lambda * gamma);
                }
            }

            CheckGameState(reward);
        }

        public override void Reset()
        {
            base.Reset();
            for (int i = 0; i < etable.Length; i++)
            {
                etable[i] = 0;
            }
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
            double[] data = new double[level.Length];
            for (int i = 0; i < data.Length; i++)
            {
                if (i == x + y * width)
                {
                    data[i] = int.MaxValue;
                }
                else
                {
                    data[i] = level[i];
                }
            }
            if (!(x < 0 || y < 0 || x > (width - 1) || y > (height - 1)))
            {
                if (level[x + y * width] == 2)
                {
                    neuralNetwork.FeedForward(data);
                    return 0;
                }
                if (level[x + y * width] == 1)
                {
                    neuralNetwork.FeedForward(data);
                    return -1;
                }
            }
            return neuralNetwork.FeedForward(data)[0];
        }

        protected override void SetValue(int x, int y, double values)
        {
            neuralNetwork.BackPropagate(values);
        }

        private double Elegbility(int x, int y)
        {
            if (x < 0 || y < 0 || x > (width - 1) || y > (height - 1))
            {
                return -1;
            }
            return etable[x + y * width];
        }

        private void SetElegbility(int x, int y, double e)
        {
            etable[x + y * width] = e;
        }

        public override string ToString()
        {
            string result = base.ToString() + '\n' + '\n';

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    result += $" {Value(x,y).ToString("F3")}";
                }
                result += '\n';
            }

            return result;
        }
    }
}
