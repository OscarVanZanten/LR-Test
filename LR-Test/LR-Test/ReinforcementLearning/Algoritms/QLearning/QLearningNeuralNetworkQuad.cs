using LR_Test.ReinforcementLearning.NeuralNetwork;
using System;
using System.Linq;

namespace LR_Test.ReinforcementLearning.Algoritms.QLearning
{

    public class QLearningNeuralNetworkQuad : QValueGame
    {
        private SimpleNN[] neuralNetworks;

        private readonly int maxPolicyEpisodes;

        public QLearningNeuralNetworkQuad(int width, int height, int[] level, int spawnX, int spawnY, double alpha, double epsilon, double gamma, int maxPolicyEpisodes) : base(width, height, level, spawnX, spawnY, alpha, epsilon, gamma)
        {
            this.neuralNetworks = new SimpleNN[4]
            {
                new SimpleNN( width * height,64,8, 1),
                new SimpleNN( width * height,64,8, 1),
                new SimpleNN( width * height,64,8, 1),
                new SimpleNN( width * height,64,8, 1),
            };
        }

        public QLearningNeuralNetworkQuad(double alpha, double epsilon, double gamma) : base(alpha, epsilon, gamma)
        {
            this.neuralNetworks = new SimpleNN[4]
           {
                new SimpleNN( width * height,64,8, 1),
                new SimpleNN( width * height,64,8, 1),
                new SimpleNN( width * height,64,8, 1),
                new SimpleNN( width * height,64,8, 1),
           };
        }

        protected override int DetermineMove(int x, int y, int episode)
        {
            double epsilonDiscount = episode > 0 ? (episode/(maxPolicyEpisodes*1.0) ) * epsilon : 0;
            double finalEpsilon = epsilon - (epsilonDiscount > epsilon ? epsilon : epsilonDiscount);
            Console.WriteLine($"{epsilon} {epsilonDiscount} {finalEpsilon}");

            bool randomMove = random.NextDouble() < (finalEpsilon);

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
            return (qvalue1 + alpha * (reward + gamma * qvalue2 - qvalue1));
        }

        protected override double[] QValues(int x, int y)
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

            double[] result = new double[4];
            for (int i = 0; i < neuralNetworks.Length; i++)
            {
                result[i] = neuralNetworks[i].FeedForward(data)[0];
            }

            return result;
        }

        protected override void SetQValues(int x, int y, double[] values)
        {
            for (int move = 0; move < 4; move++)
            {
                neuralNetworks[move].BackProp(values[move]);
            }
        }
    }
}
