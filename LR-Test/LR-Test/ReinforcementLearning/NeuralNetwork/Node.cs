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
        public List<Connection> In { get; set; }
        public List<Connection> Out { get; set; }

        public Node()
        {
            this.In = new List<Connection>();
            this.Out = new List<Connection>();
        }

        public void Calculate()
        {
            double sum = Bias;

            foreach (Connection connection in In)
            {
                sum += connection.Result;
            }

            double res = MathHelper.Sigmoid(sum);
           // double res = Math.Max(0, sum);

            Value = res;
        }

      
    }
}
