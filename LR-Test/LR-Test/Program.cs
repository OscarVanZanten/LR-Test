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

            Tabulair tabulair = new Tabulair(0.1,0.1, 0.9);
            int episode = 0;

            Console.WriteLine(tabulair);
            Thread.Sleep(100);

            while (true)
            {

                tabulair.TakeTurn();
                Console.Clear();
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
