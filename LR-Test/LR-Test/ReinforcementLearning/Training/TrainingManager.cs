using LR_Test.ReinforcementLearning.NeuralNetwork;
using LR_Test.ReinforcementLearning.Training.History;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace LR_Test.ReinforcementLearning.Training
{
    public class TrainingManager
    {

        private const int TRAINCOUNT = 10;
        private const int DELAY = 100;
        private const int SHOWBESTCOUNT = 50;

        private List<TrainingHistoryItem> history;

        private int[] networkFormat = new int[] { 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 4 };


        public TrainingManager()
        {
            this.history = new List<TrainingHistoryItem>();
        }

        public void StartTraining()
        {
            while (true)
            {
                string data = NeuralNetworkBuilder.GenerateRandomNeuralNetworkData(networkFormat);
                var network = NeuralNetworkBuilder.GenerateNeuralNetwork(data, networkFormat);

                long[] scores = new long[TRAINCOUNT];

                for (int i = 0; i < TRAINCOUNT; i++)
                {
                    scores[i] = Train(network);
                }

                TrainingHistoryItem historyItem = new TrainingHistoryItem()
                {
                    Data = data,
                    Scores = scores
                };

                history.Add(historyItem);

                Console.Clear();
                Console.WriteLine(historyItem.AverageScore);

                history = history.OrderBy(h => h.AverageScore * -1).ToList();
                
                for (int i = 0; i < SHOWBESTCOUNT; i++)
                {
                    if (i >= history.Count) { break; }
                    Console.WriteLine($"{i + 1}: {history[i].AverageScore} | {String.Join(", ", history[i].Scores)}");
                }

                if (history.Count > SHOWBESTCOUNT)
                {
                    history.RemoveRange(SHOWBESTCOUNT, history.Count - SHOWBESTCOUNT);
                }
                // Thread.Sleep(DELAY);
            }
        }

        public long Train(NeuralNetwork.NeuralNetwork network)
        {
            return 0;         
        }
    }
}
