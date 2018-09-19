using LR_Test.Game;
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

        private const int TRAINCOUNT = 5;
        private Game2048 game;

        private List<TrainingHistoryItem> history;

        private int[] networkFormat = new int[] { 16, 8, 8, 4 };


        public TrainingManager()
        {
            this.history = new List<TrainingHistoryItem>();
            this.game = new Game2048();
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

                Console.WriteLine(historyItem.AverageScore);
                Thread.Sleep(1000);
            }
        }

        public long Train(NeuralNetwork.NeuralNetwork network)
        {
            game.Start();
            int lastValue = -1;

            while (!game.IsGameOver)
            {
                var result = network.Execute(game.Board.State);
                var action = Array.IndexOf<double>(result, result.Max());
                Move move = (Move)action;

                if (!game.MakeMove(move))
                {
                    break;
                }

            }

            return game.Score;
        }
    }
}
