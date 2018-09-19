using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
namespace LR_Test.ReinforcementLearning.Training.History
{
    public class TrainingHistoryItem
    {
        public string Data { get; set; }
        public long[] Scores { get; set; }
        public double AverageScore { get { return Scores.Average(); } }
    }
}
