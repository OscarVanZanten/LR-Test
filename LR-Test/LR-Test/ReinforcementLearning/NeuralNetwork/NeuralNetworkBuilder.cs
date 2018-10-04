using System;
using System.Collections.Generic;
using System.Text;

namespace LR_Test.ReinforcementLearning.NeuralNetwork
{
    public class NeuralNetworkBuilder
    {
        public static string GenerateEmptyNeuralNetworkData(params int[] format)
        {
            Random random = new Random();

            int biasCount = 0;
            int weightCount = 0;

            for (int i = 0; i < format.Length; i++)
            {
                if (i > 0)
                {
                    biasCount += format[i];
                    weightCount += format[i - 1] * format[i];
                }
            }

            string result = "";

            for (int i = 0; i < biasCount; i++)
            {
                result += 1 + (biasCount - 1 != i ? ";" : "");// random.NextDouble() * 10 - 5 + (biasCount - 1 != i ? ";" : "");
            }

            result += "|";


            for (int i = 0; i < weightCount; i++)
            {
                result += random.NextDouble() * 2 - 1 + (weightCount - 1 != i ? ";" : "");
            }

            return result;
        }

        /// <summary>
        /// Generates random data for a neural network with a certain format
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string GenerateRandomNeuralNetworkData(params int[] format)
        {
            Random random = new Random();

            int biasCount = 0;
            int weightCount = 0;

            for (int i = 0; i < format.Length; i++)
            {
                if (i > 0)
                {
                    biasCount += format[i];
                    weightCount += format[i - 1] * format[i];
                }
            }

            string result = "";

            for (int i = 0; i < biasCount; i++)
            {
                result += 1 + (biasCount - 1 != i ? ";" : "");// random.NextDouble() * 10 - 5 + (biasCount - 1 != i ? ";" : "");
            }

            result += "|";


            for (int i = 0; i < weightCount; i++)
            {
                result += random.NextDouble() * 2 - 1 + (weightCount - 1 != i ? ";" : "");
            }

            return result;
        }

        /// <summary>
        /// Generates a neural network with a specific bias/weight distrubution 
        /// </summary>
        /// <param name="data">AI data biases/weights. Format: "bias1, bias2, bias3...... ; weight1, weight2, weight3"</param>
        /// <param name="format">format of the neural network</param>
        /// <returns></returns>
        public static NeuralNetwork GenerateNeuralNetwork(string data, params int[] format)
        {
            if (format.Length < 2)
            {
                throw new Exception("Size of Neural Network needs to have input and output nodes");
            }

            if (!data.Contains("|"))
            {
                throw new Exception("Illegal format");
            }

            string[] splitData = data.Split('|');
            string[] biases = splitData[0].Split(";");
            string[] weights = splitData[1].Split(";");

            int biasCount = 0;
            int weightCount = 0;

            // Create network space
            Node[][] nodes = new Node[format.Length][];

            for (int i = 0; i < format.Length; i++)
            {
                if (format[i] == 0)
                {
                    throw new Exception("new line of notes need to actually contain nodes");
                }

                // Create layer space
                nodes[i] = new Node[format[i]];

                // Fill layer
                for (int j = 0; j < format[i]; j++)
                {
                    if (biasCount >= biases.Length)
                    {
                        throw new Exception("Missing bias");
                    }

                    double bias = 0;

                    if (i > 0) {

                        string b = biases[biasCount++];
                        bool valid = double.TryParse(b, out bias);
                        // TODO: get these out of format
                        if (!valid)
                        {
                            throw new Exception("Invalid bias");
                        }
                    }

                    // Create node
                    nodes[i][j] = new Node()
                    {
                        Bias = bias
                    };

                }

                // Create connections
                if (i > 0)
                {
                    for (int k = 0; k < format[i - 1]; k++)
                    {
                        for (int l = 0; l < format[i]; l++)
                        {
                            Node from = nodes[i - 1][k];
                            Node to = nodes[i][l];

                            if (weightCount >= weights.Length)
                            {
                                throw new Exception("Missing weights");
                            }

                            double weight = 0;
                            if (i > 0)
                            {
                                bool valid = double.TryParse(weights[weightCount++], out weight);

                                if (!valid)
                                {
                                    throw new Exception("Invalid weight");
                                }
                            }
                            Connection connection = new Connection()
                            {
                                From = from,
                                To = to,
                                Weight = weight
                            };

                            from.Out.Add(connection);
                            to.In.Add(connection);
                        }
                    }
                }
            }

            return new NeuralNetwork(nodes);
        }
    }
}
