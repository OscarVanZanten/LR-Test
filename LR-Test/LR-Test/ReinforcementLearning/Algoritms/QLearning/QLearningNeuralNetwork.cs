using LR_Test.ReinforcementLearning.NeuralNetwork;
using System;
using System.Linq;

namespace LR_Test.ReinforcementLearning.Algoritms.QLearning
{
    public class QLearningNeuralNetwork : QValueGame
    {
        private SimpleNN neuralNetwork;

        public QLearningNeuralNetwork(int width, int height, int[] level, int spawnX, int spawnY, double alpha, double epsilon, double gamma) : base(width, height, level, spawnX, spawnY, alpha, epsilon, gamma)
        {
            this.neuralNetwork = new SimpleNN( width * height,64,64, 4);
        }

        public QLearningNeuralNetwork(double alpha, double epsilon, double gamma) : base(alpha, epsilon, gamma)
        {
            this.neuralNetwork = new SimpleNN(width * height, 64, 64, 4);
        }

        protected override int DetermineMove(int x, int y, int episode)
        {
            bool randomMove = random.NextDouble() < (epsilon);

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
            return (float)(qvalue1 + alpha * (reward + gamma * qvalue2 - qvalue1));
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

            return neuralNetwork.FeedForward(data);
        }

        protected override void SetQValues(int x, int y, double[] values)
        {
            neuralNetwork.BackProp(values);
        }
    }
}
