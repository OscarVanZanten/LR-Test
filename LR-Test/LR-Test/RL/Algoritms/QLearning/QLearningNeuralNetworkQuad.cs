using System;
using System.Linq;

using LR_Test.RL.NeuralNetwork;

namespace LR_Test.RL.Algoritms.QLearning
{

    public class QLearningNeuralNetworkQuad : QValueGame
    {
        private SimpleNN[] neuralNetworks;

        private readonly int maxPolicyEpisodes;

        public QLearningNeuralNetworkQuad(int width, int height, int[] level, int spawnX, int spawnY, double alpha, double epsilon, double gamma, int maxPolicyEpisodes) : base(width, height, level, spawnX, spawnY, alpha, epsilon, gamma)
        {
            this.maxPolicyEpisodes = 10000;
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
            this.maxPolicyEpisodes = 10000;
            this.neuralNetworks = new SimpleNN[4]
            {
                new SimpleNN(.5,  width * height,64,8, 1),
                new SimpleNN(.5,  width * height,64,8, 1),
                new SimpleNN( .5, width * height,64,8, 1),
                new SimpleNN(.5,  width * height,64,8, 1),
            };
        }

        public override void TakeTurn(int episode)
        {
            int agentx1 = agentX, agenty1 = agentY;

            var move = DetermineMove(agentX, agentY, episode);

            var qvals1 = QValues(agentX, agentY);
            var maxQ1 = HighestQValues(qvals1);

            var reward = MakeMove(move);

            var qvals2 = QValues(agentX, agentY);
            var maxQ2 = HighestQValues(qvals2);

            var update = CalculateUpdatedQValue(qvals1[move], maxQ2, (float)reward);

            QValues(agentx1, agenty1);
            Console.WriteLine($"Q-Values: {qvals1[0]}, {qvals1[1]}, {qvals1[2]}, {qvals1[3]}");
            qvals1[move] = update;
            Console.WriteLine($"Q-Values: {qvals1[0]}, {qvals1[1]}, {qvals1[2]}, {qvals1[3]}");
            SetQValues(agentx1, agenty1, qvals1);
            Console.WriteLine($"UpdatedQValue: {update}");

            CheckGameState(reward);
        }

        protected override int DetermineMove(int x, int y, int episode)
        {
            double epsilonDiscount = episode > 0 ? (episode / (maxPolicyEpisodes * 1.0)) * epsilon : 0;
            double finalEpsilon = epsilon - (epsilonDiscount > epsilon ? epsilon : epsilonDiscount);
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
                neuralNetworks[move].BackPropagate(values[move]);
            }
        }
    }
}
