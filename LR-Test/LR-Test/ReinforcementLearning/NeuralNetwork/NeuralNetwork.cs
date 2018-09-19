using System;
using System.Collections.Generic;
using System.Text;

namespace LR_Test.ReinforcementLearning.NeuralNetwork
{
    public class NeuralNetwork
    {
        private Node[][] Nodes;

        public NeuralNetwork(Node[][] Nodes)
        {
            this.Nodes = Nodes;
        }

        public double[] Execute(params int[] values)
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
                for (int j = 1; j < Nodes[i].Length; j++)
                {
                    Nodes[i][j].Calculate();
                }
            }

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = Nodes[Nodes.Length-1][i].Value;
            }

            return result;
        }

        public override string ToString()
        {
            string result = "";

            foreach (Node[] layer in Nodes)
            {
                foreach (Node node in layer)
                {
                    result += $" ({node.Out.Count}/{node.In.Count}) ";
                }

                result += "\n";
            }

            return result;
        }
    }
}
