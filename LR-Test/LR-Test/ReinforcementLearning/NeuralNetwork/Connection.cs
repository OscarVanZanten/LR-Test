using System;
using System.Collections.Generic;
using System.Text;

namespace LR_Test.ReinforcementLearning.NeuralNetwork
{
    public class Connection
    {
        public Node From { get; set; }
        public Node To { get; set; }
        public double Weight { get; set; }

        public double Result { get { return From.Value * Weight; } }
    }
}
