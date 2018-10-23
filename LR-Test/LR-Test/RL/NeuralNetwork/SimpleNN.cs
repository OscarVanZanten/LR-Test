using LR_Test.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace LR_Test.RL.NeuralNetwork
{
    /// <summary>
    /// A simple Neural network with back propagation
    /// </summary>
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

        /// <summary>
        /// Setup the data arrays for the NN
        /// </summary>
        private void SetupNN()
        {
            //Setup Node values
            this.outputs = new double[format.Length][];
            for (int layer = 0; layer < format.Length; layer++)
            {
                outputs[layer] = new double[format[layer]];
            }

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
                        weights[layer][fromNode][toNode] = randomGen.NextDouble() - 0.5;
                    }
                }
            }

            //Setup node errors
            this.errors = new double[format.Length][];
            for (int layer = 0; layer < format.Length; layer++)
            {
                errors[layer] = new double[format[layer]];
            }

            //Setup bias
            this.bias = new double[format.Length][];
            for (int layer = 0; layer < format.Length; layer++)
            {
                bias[layer] = new double[format[layer]];
            }
        }

        /// <summary>
        /// Feed forward through the NN given inputs
        /// </summary>
        /// <param name="inputs">input into the NN</param>
        /// <returns>The output of the NN</returns>
        public double[] FeedForward(params double[] inputs)
        {
            //Check validity
            if (inputs.Length != outputs[0].Length)
            {
                throw new ArgumentException("Length does not match");
            }

            // Shallow copy the input values into the input layer
            outputs[0] = (double[])inputs.Clone();

            // Feed forward for the layers after the input layer
            for (int layer = 1; layer < format.Length; layer++)
            {
                FeedForward(layer);
            }

            //return shallow copy of outputs
            return (double[])outputs[outputs.Length - 1].Clone();
        }

        /// <summary>
        /// Feed forwards for a specific layer, it grabs the pervious layer to calculate the current output
        /// Shouldnt be used for the input layer
        /// </summary>
        /// <param name="layer">Current layer to feed forward</param>
        private void FeedForward(int layer)
        {
            // Iterate through all current nodes in the layer to calculate their output values
            for (int toNode = 0; toNode < format[layer]; toNode++)
            {
                // Calculate the output value of the current node
                double sum = bias[layer][toNode];

                // Cycle through nodes of previous layer
                for (int fromNode = 0; fromNode < format[layer - 1]; fromNode++)
                {
                    double weightValue = weights[layer - 1][fromNode][toNode];
                    double outputValue = outputs[layer - 1][fromNode];

                    // Calculate value from previous node
                    sum += weightValue * outputValue;
                }

                // Apply function to output value gained from previous layer
                if (layer == format.Length - 1)
                {
                    outputs[layer][toNode] = sum;
                }
                else
                {
                    outputs[layer][toNode] = MathHelper.Sigmoid(sum);
                }
            }
        }

        /// <summary>
        /// Backpropagate though the NN
        /// </summary>
        /// <param name="expected">expected output</param>
        public void BackPropagate(params double[] expected)
        {
            //Check validity
            if (expected.Length != outputs[outputs.Length - 1].Length)
            {
                throw new ArgumentException("Length does not match");
            }

            // Backpropagate output layer
            BackPropagateOutputLayer(expected);

            // Backpropagate through layers
            for (int layer = format.Length - 2; layer > 0; layer--)
            {
                BackPropagateLayer(layer);
            }

            // Update weights for all layers
            for (int layer = format.Length - 1; layer > 0; layer--)
            {
                UpdateWeights(layer);
            }
        }

        /// <summary>
        /// Back propagates through the output layer
        /// </summary>
        /// <param name="expected">expected output</param>
        private void BackPropagateOutputLayer(double[] expected)
        {
            // cycle through all nodes in the output layer
            for (int outputError = 0; outputError < expected.Length; outputError++)
            {
                double outputValue = outputs[format.Length - 1][outputError];
                double expectedValue = expected[outputError];

                // Calcuate and store error
                double error = (expectedValue - outputValue);
                errors[format.Length - 1][outputError] = error;
            }
        }

        /// <summary>
        /// Backpropagate specific layer
        /// </summary>
        /// <param name="layer">layer to backpropagate</param>
        private void BackPropagateLayer(int layer)
        {
            // cycle through all nodes in the layer
            for (int node = 0; node < format[layer]; node++)
            {
                double sum = 0;

                // cycle through all nodes in the next layer
                for (int nodeTo = 0; nodeTo < format[layer + 1]; nodeTo++)
                {
                    double errorValue = errors[layer + 1][nodeTo];
                    double weightValue = weights[layer][node][nodeTo];
                    sum += errorValue * weightValue;
                }

                double outputValue = outputs[layer][node];

                // Calculate and store error
                double error = outputValue * (1 - outputValue) * sum;
                errors[layer][node] = error;
            }
        }

        /// <summary>
        /// Update weights with earlier calculated errors
        /// </summary>
        /// <param name="layer">Layer to weights to be updated</param>
        private void UpdateWeights(int layer)
        {
            // cycle through all nodes in the layer
            for (int node = 0; node < format[layer]; node++)
            {
                // cycle through all nodes in the previous layer
                for (int nodeTo = 0; nodeTo < format[layer - 1]; nodeTo++)
                {
                    double errorValue = errors[layer][node];
                    double outputValue = outputs[layer - 1][nodeTo];

                    // Calculate weight adjustment and store
                    double weightDelta = learningrate * errorValue * outputValue;
                    weights[layer - 1][nodeTo][node] += weightDelta;
                }
            }
        }
    }
}
