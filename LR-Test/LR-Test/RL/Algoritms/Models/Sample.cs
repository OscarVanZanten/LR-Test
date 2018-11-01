using System;
using System.Collections.Generic;
using System.Text;

namespace LR_Test.RL.Algoritms.Models
{
    public struct Sample
    {
        public int XTo { get; set; }
        public int YTo { get; set; }
        public int XFrom { get; set; }
        public int YFrom { get; set; }
        public int Action { get; set; }
        public double Reward { get; set; }
    }
}
