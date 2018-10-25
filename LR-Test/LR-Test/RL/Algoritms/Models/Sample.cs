using System;
using System.Collections.Generic;
using System.Text;

namespace LR_Test.RL.Algoritms.Models
{
    public struct Sample
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Action { get; set; }
        public double Reward { get; set; }
    }
}
