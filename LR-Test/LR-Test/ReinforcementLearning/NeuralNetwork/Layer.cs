using LR_Test.Util;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace LR_Test.ReinforcementLearning.NeuralNetwork
{
    public class Layer
    {
        private Node[] nodes;
        private readonly int numberOfInputs;
        private readonly int numberOfOutputs;
        private double[] output;
        private double[] inputs;

        public double[,] TotalWeights
        {
            get
            {
                double[,] result = new double[nodes.Length, numberOfInputs];

                for (int i = 0; i < nodes.Length; i++)
                {
                    Node node = nodes[i];
                    for (int j = 0; j < node.Weights.Length; j++)
                    {
                        result[i, j] = node.Weights[j];
                    }
                }

                return result;
            }
        }

        public Layer(int numberOfInputs, int numberOfOutputs, bool randomWeights)
        {
            this.nodes = new Node[numberOfOutputs];
            this.numberOfInputs = numberOfInputs;
            this.numberOfOutputs = numberOfOutputs;

            InstantiateNodes(randomWeights);
        }

        private void InstantiateNodes(bool randomWeights)
        {
            for (int i = 0; i < nodes.Length; i++)
            {
                nodes[i] = new Node(numberOfInputs, randomWeights);
            }
        }

        public double[] FeedForward(double[] inputs)
        {
            if (inputs.Length != numberOfInputs)
            {
                throw new ArgumentException("Input length does not match");
            }

            this.inputs = inputs;

            double[] result = new double[numberOfOutputs];

            for (int i = 0; i < numberOfOutputs; i++)
            {
                result[i] = nodes[i].FeedForward(inputs);
            }

            this.output = result;

            return result;
        }


        /// <summary>
        /// Back propagation for the output layer
        /// </summary>
        /// <param name="expected">The expected output</param>
        public Layer BackPropOutput(double[] expected)
        {
            //Error dervative of the cost function
            for (int i = 0; i < numberOfOutputs; i++)
            {
                nodes[i].Error = nodes[i].Output * (1 - nodes[i].Output) * (expected[i] - nodes[i].Output);
            }

            return this;
        }

        /// <summary>
        /// Back propagation for the hidden layers
        /// </summary>
        /// <param name="gammaForward">the gamma value of the forward layer</param>
        /// <param name="weightsFoward">the weights of the forward layer</param>
        public void BackPropHidden(Layer previous)
        {


            //Caluclate new gamma using gamma sums of the forward layer
            for (int i = 0; i < numberOfOutputs; i++)
            {
                double sum = 0;

                foreach (Node n in previous.nodes)
                {
                    sum += (n.Error * n.Weights[i]);
                }

                nodes[i].Error = nodes[i].Output * (1 - nodes[i].Output) * sum;
            }
        }

        public void UpdateWeights()
        {
           // Console.WriteLine($"numberOfOutputs: {numberOfOutputs}");
            //Console.WriteLine($"numberOfInputs: {numberOfInputs}");
            //Console.WriteLine($"nodeslength: {layer.nodes.Length}");

            for (int i = 0; i < numberOfOutputs; i++)
            {
                for (int j = 0; j < numberOfInputs; j++)
                {

                    var error = nodes[i].Error;
                    var output = this.output[i];
                    if (error != 0)
                    {
                        nodes[i].Weights[j] += 0.05f * error * output;
                    }
                }
            }
        }


        public double TanHDer(double value)
        {
            return 1 - (value * value);
        }
    }
}
