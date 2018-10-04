using LR_Test.ReinforcementLearning.NeuralNetwork;
using System;
using System.Threading;
using System.Linq;
using LR_Test.ReinforcementLearning.Algoritms.QLearning;

namespace LR_Test
{
    class Program
    {


        static void Main(string[] args)
        {
            //TestRewardFunction();
            //StartTrainer();
            const int width = 10;
            const int height = 10;
            const int spawnx = 0;
            const int spawny = 0;
            //int[] level = new int[width * height]
            //{
            //    0,0,0,0,0,0,3,0,0,0,
            //    0,1,1,0,1,0,0,0,1,1,
            //    0,1,3,0,1,0,1,0,0,1,
            //    0,0,0,0,1,0,1,1,0,0,
            //    0,0,0,0,1,0,0,0,0,0,
            //    0,0,0,0,0,0,3,0,0,0,
            //    0,1,1,0,1,0,0,0,1,1,
            //    0,1,3,0,1,0,1,0,0,1,
            //    0,0,0,0,1,0,1,1,0,0,
            //    0,0,0,0,1,0,0,0,0,2
            //};
            int[] level = new int[width * height]
            {
                0,0,0,0,0,0,0,0,0,0,
                0,0,0,0,0,0,0,0,0,0,
                0,0,0,0,0,0,0,0,0,0,
                0,0,0,0,0,0,0,0,0,0,
                0,0,0,0,0,0,0,0,0,0,
                0,0,0,0,2,0,0,0,0,0,
                0,0,0,0,0,0,0,0,0,0,
                0,0,0,0,0,0,0,0,0,0,
                0,0,0,0,0,0,0,0,0,0,
                0,0,0,0,0,0,0,0,0,0,
            };

            QLearningNeuralNetwork tabulair = new QLearningNeuralNetwork(width, height, level, 0, 0, 4, 5, 0.1, 0.3, 0.7);
            int episode = 0;

            Console.WriteLine(tabulair);
            Thread.Sleep(100);

            while (true)
            {

                Console.Clear();
                tabulair.TakeTurn(episode);
                Console.WriteLine($"Episode: {episode}");
                Console.WriteLine(tabulair);

                Thread.Sleep(100);

                if (tabulair.Finished)
                {
                    tabulair.Reset();
                    episode++;
                }
            }
        }



    }
}
