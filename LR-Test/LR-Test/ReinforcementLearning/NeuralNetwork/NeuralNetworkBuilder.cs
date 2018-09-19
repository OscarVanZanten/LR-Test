using System;
using System.Collections.Generic;
using System.Text;

namespace LR_Test.ReinforcementLearning.NeuralNetwork
{
    public class NeuralNetworkBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data">AI data biases/weights. Format: "bias1, bias2, bias3...... ; weight1, weight2, weight3"</param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static NeuralNetwork Generate(string data, params int[] size)
        {
            if (size.Length < 2)
            {
                throw new Exception("Size of Neural Network needs to have input and output nodes");
            }

            if (!data.Contains(";"))
            {
                throw new Exception("Illegal format");
            }

            string[] splitData = data.Split(';');
            string[] biases = splitData[0].Split(",");
            string[] weights = splitData[1].Split(",");

            int biasCount = 0;
            int weightCount = 0;

            // Create network space
            Node[][] nodes = new Node[size.Length][];

            for (int i = 0; i < size.Length; i++)
            {
                if (size[i] == 0)
                {
                    throw new Exception("new line of notes need to actually contain nodes");
                }

                // Create layer space
                nodes[i] = new Node[size[i]];

                // Fill layer
                for (int j = 0; j < size[i]; j++)
                {
                    if (biasCount >= biases.Length)
                    {
                        throw new Exception("Missing bias");
                    }

                    // TODO: get these out of format
                    if (!int.TryParse(biases[biasCount++], out var bias))
                    {
                        throw new Exception("Invalid bias");
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
                    for (int k = 0; k < size[i - 1]; k++)
                    {
                        for (int l = 0; l < size[i]; l++)
                        {
                            Node from = nodes[i - 1][k];
                            Node to = nodes[i][l];

                            if (weightCount >= weights.Length)
                            {
                                throw new Exception("Missing weights");
                            }

                            if (!int.TryParse(weights[weightCount++], out var weight))
                            {
                                throw new Exception("Invalid weight");
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
