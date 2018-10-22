using LR_Test.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace LR_Test.RL.NeuralNetwork
{
    public class SimpleNN
    {
        private Random randomGen;

        private readonly double learningrate = .01;

        // Format
        private int[] format;
        // Values[layer][node]
        private double[][] outputs;
        // Weight[layer][node][toNode]
        private double[][][] weights;
        // bias[layer][node]
        private double[][] bias;
        // errors[layer][node]
        private double[][] errors;

        public SimpleNN(params int[] format)
        {
            this.format = format;
            this.randomGen = new Random();
            SetupNN();
        }

        public SimpleNN(double learningrate, params int[] format) : this(format)
        {
            this.learningrate = learningrate;
        }

        private void SetupNN()
        {
            //Setup Node values
            this.outputs = new double[format.Length][];
            for (int layer = 0; layer < format.Length; layer++) { outputs[layer] = new double[format[layer]]; }

            //Setup node weights
            this.weights = new double[format.Length - 1][][];
            for (int layer = 0; layer < format.Length - 1; layer++)
            {
                weights[layer] = new double[format[layer]][];
                for (int fromNode = 0; fromNode < format[layer]; fromNode++)
                {
                    weights[layer][fromNode] = new double[format[layer + 1]];

                    for (int toNode = 0; toNode < format[layer + 1]; toNode++)
                    {
                        double randWeight = randomGen.NextDouble()-0.5;
                        weights[layer][fromNode][toNode] = randWeight;
                    }
                }
            }

            //Setup node errors
            this.errors = new double[format.Length][];
            for (int layer = 0; layer < format.Length; layer++) { errors[layer] = new double[format[layer]]; }

            //Setup bias
            this.bias = new double[format.Length][];
            for (int layer = 0; layer < format.Length; layer++) { bias[layer] = new double[format[layer]]; }
        }

        public double[] FeedForward(params double[] inputs)
        {
            if (inputs.Length != outputs[0].Length) { throw new ArgumentException("Length does not match"); }

            outputs[0] = (double[])inputs.Clone();

            for (int layer = 1; layer < format.Length; layer++)
            {
                for (int toNode = 0; toNode < format[layer]; toNode++)
                {
                    double sum = bias[layer][toNode];

                    for (int fromNode = 0; fromNode < format[layer - 1]; fromNode++)
                    {
                        double weight = weights[layer - 1][fromNode][toNode];
                        double outputValue = outputs[layer - 1][fromNode];

                        double added = weight * outputValue; 

                        sum += added;
                    }
                    double result = 0;
                    if (layer == format.Length - 1)
                    {
                        result = sum;
                    } else
                    {
                        result = MathHelper.Sigmoid(sum);
                    }

                    outputs[layer][toNode] = result;
                }
            }

            return (double[])outputs[outputs.Length - 1].Clone();
        }

        public void BackProp(params double[] expected)
        {
            if (expected.Length != outputs[outputs.Length - 1].Length) { throw new ArgumentException("Length does not match"); }

            for (int outputError = 0; outputError < expected.Length; outputError++)
            {
                double outputValue = outputs[format.Length - 1][outputError];
                double expectedValue = expected[outputError];

                double error =  (expectedValue - outputValue);

                errors[format.Length - 1][outputError] = error;
            }

            for (int layer = format.Length - 2; layer > 0; layer--)
            {
                for (int node = 0; node < format[layer]; node++)
                {
                    double sum = 0;

                    for (int nodeTo = 0; nodeTo < format[layer + 1]; nodeTo++)
                    {
                        double errorValue = errors[layer + 1][nodeTo];
                        double weightValue = weights[layer][node][nodeTo];
                        sum += errorValue * weightValue;
                    }

                    double outputValue = outputs[layer][node];

                    double error = outputValue * (1 - outputValue) * sum;

                    errors[layer][node] = error;
                }
            }

            for (int layer = format.Length - 1; layer > 0; layer--)
            {
                for (int node = 0; node < format[layer]; node++)
                {

                    for (int nodeTo = 0; nodeTo < format[layer-1]; nodeTo++)
                    {
                        double errorValue = errors[layer][node];
                        double outputValue = outputs[layer-1][nodeTo];

                        double weightDelta = learningrate * errorValue * outputValue;

                        weights[layer-1][nodeTo][node] += weightDelta;
                    }
                }
            }
        }
    }
}
