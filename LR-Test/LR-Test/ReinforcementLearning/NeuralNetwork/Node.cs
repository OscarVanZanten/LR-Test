using LR_Test.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace LR_Test.ReinforcementLearning.NeuralNetwork
{
    public class Node
    {
        public double Value { get; set; }
        public double Bias { get; set; }
        public double[] Weights { get; set; }
        public double Output { get; internal set; }
        public double Error { get; set; }

        private Random random = new Random();
        private readonly int numberOfInputs;

        public Node(int numberOfInputs, bool randomWeights)
        {
            this.numberOfInputs = numberOfInputs;
            this.Weights = new double[numberOfInputs];

            if (randomWeights)
            {
                InitilizeWeights();
            }
        }

        public void InitilizeWeights()
        {
            for (int j = 0; j < numberOfInputs; j++)
            {
                Weights[j] = (float)random.NextDouble() - 0.5f;
            }
        }


        public double FeedForward(double[] inputs)
        {
            if (inputs.Length != numberOfInputs)
            {
                throw new ArgumentException("Input length does not match");
            }

            double result = 0;

            for (int i = 0; i < numberOfInputs; i++)
            {
                result += inputs[i] * Weights[i];
            }

            result = MathHelper.Sigmoid(result);

            this.Output = result;

            return result;
        }

    }
}
