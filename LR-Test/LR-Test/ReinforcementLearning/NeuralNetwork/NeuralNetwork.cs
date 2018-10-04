using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace LR_Test.ReinforcementLearning.NeuralNetwork
{
    public class NeuralNetwork
    {
        private Node[][] Nodes;

        public NeuralNetwork(Node[][] Nodes)
        {
            this.Nodes = Nodes;
        }

        public double[] Execute(params double[] values)
        {
            if (values.Length != Nodes[0].Length)
            {
                throw new Exception($"Invalid input length to inputnodes, Nodes: { Nodes[0].Length}, Values: {values.Length}");
            }

            for (int i = 0; i < Nodes[0].Length; i++)
            {
                Nodes[0][i].Value = values[i];
            }

            double[] result = new double[Nodes[Nodes.Length - 1].Length];


            for (int i = 1; i < Nodes.Length; i++)
            {
                for (int j = 0; j < Nodes[i].Length; j++)
                {
                    Nodes[i][j].Calculate();
                }
            }

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = Nodes[Nodes.Length - 1][i].Value;
            }

            return result;
        }

        public void BackPropagation(params double[] targets)
        {
            if (targets.Length != Nodes[Nodes.Length - 1].Length)
            {
                throw new ArgumentException("Error is not the same size as the output layer");
            }

            Dictionary<Node, double> errors = new Dictionary<Node, double>();

            for (int i = 0; i < targets.Length; i++)
            {
                double target = targets[i];
                Node currentNode = Nodes[Nodes.Length - 1][i];
                double current = currentNode.Value;
                double err = (target - current) * current * (1 - current);

                errors.Add(currentNode, err);
            }

            for (int layer = Nodes.Length - 2; layer > 0; layer--)
            {
                var layerNodes = Nodes[layer];
                Dictionary<Node, double> innerLayerError = new Dictionary<Node, double>();

                for (int i = 0; i < layerNodes.Length; i++)
                {
                    Node node = layerNodes[i];
                    double nErr = 0;

                    foreach (Connection conn in node.Out)
                    {
                        nErr += (errors[conn.To] * conn.Weight) * node.Value * (1 - node.Value);
                    }

                    nErr /= node.Out.Count;
                    innerLayerError.Add(node, nErr);


                    foreach (Connection conn in node.Out)
                    {
                        var wErr = node.Value * errors[conn.To];
                        conn.Weight += wErr;
                    }

                }
                errors = innerLayerError;
            }
        }

        public override string ToString()
        {
            string result = "";

            for (int layerIndex = 0; layerIndex < Nodes.Length; layerIndex++)
            {
                Node[] layer = Nodes[layerIndex];
                //double[,,] weights = new double[layer.Length, node.In.Count, 2];
                for (int nodeIndex = 0; nodeIndex < layer.Length; nodeIndex++)
                {
                    Node node = layer[nodeIndex];

                    if (node.In.Count > 0)
                    {
                        foreach (Connection conn in node.In)
                        {
                            result += $"{conn.Result}({conn.Weight}), ";
                        }
                        result += "   ";
                    }
                    result += "\n";
                }
                foreach (Node node in layer)
                {
                    result += $" ({node.Bias}/ {node.Value})     ";
                }

                result += "\n";
            }

            return result;
        }
    }
}
