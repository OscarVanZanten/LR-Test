using LR_Test.ReinforcementLearning.NeuralNetwork;
using System;
using System.Threading;
using System.Linq;
using LR_Test.ReinforcementLearning.Algoritms.QLearning;
using TensorFlow;
using System.Collections.Generic;
using LR_Test.ReinforcementLearning;

namespace LR_Test
{
    class Program
    {


        static void Main(string[] args)
        {
            // XORTest();
            RunGame();

            //TensorFlow();
        }

        public static void XORTest()
        {
            //MyNeuralNetwork network = new MyNeuralNetwork(true, 2, 3, 1);
            SimpleNN network = new SimpleNN(2, 3, 1);
            while (true)
            {
                //Thread.Sleep(5);
                //  Console.Clear();

                var result = network.FeedForward(1f, 1f);
                Console.WriteLine($"(1,1): result={result[0]} expected=0");
                network.BackProp(0);

                result = network.FeedForward(0f, 1f);
                Console.WriteLine($"(0,1): result={result[0]} expected=1");
                network.BackProp(1);

                result = network.FeedForward(1f, 0f);
                Console.WriteLine($"(1,0): result={result[0]} expected=1");
                network.BackProp(1);

                result = network.FeedForward(0, 0);
                Console.WriteLine($"(0,0): result={result[0]} expected=0");
                network.BackProp(0);

                Console.WriteLine();
            }
        }


        public static void TensorFlow()
        {
            using (var session = new TFSession())
            {
                var graph = session.Graph;

                var a = graph.Const(2);
                var b = graph.Const(3);
                Console.WriteLine("a=2 b=3");

                // Add two constants
                var addingResults = session.GetRunner().Run(graph.Add(a, b));
                var addingResultValue = addingResults.GetValue();
                Console.WriteLine("a+b={0}", addingResultValue);

                // Multiply two constants
                var multiplyResults = session.GetRunner().Run(graph.Mul(a, b));
                var multiplyResultValue = multiplyResults.GetValue();
                Console.WriteLine("a*b={0}", multiplyResultValue);
            }

            Console.ReadKey();
        }
        static int episode = 0;
        static int succeses = 0;
        static int fails = 0;
        static readonly int currentSuccesScale = 1000;
        public static void RunGame()
        {
            //TestRewardFunction();
            //StartTrainer();
            const int width = 10;
            const int height = 10;
            const int spawnx = 0;
            const int spawny = 0;
            //int[] level = new int[width * height]
            //{
            //    0,0,0,0,0,0,1,0,0,3,
            //    0,1,1,0,1,0,0,0,1,1,
            //    0,1,3,0,1,0,1,0,0,3,
            //    0,1,1,0,0,0,1,1,0,0,
            //    0,0,0,0,1,0,0,0,0,2,

            //};
            int[] level = new int[width * height]
            {
                0,0,0,0,0,0,0,0,0,3,
                0,0,0,0,0,0,0,0,0,0,
                0,0,0,3,0,0,0,0,0,0,
                0,0,0,0,0,0,0,0,0,0,
                0,3,0,0,0,2,0,3,0,0,
                0,0,0,0,0,0,0,0,0,0,
                0,0,0,0,0,0,0,0,0,0,
                0,3,0,0,0,0,3,0,0,0,
                0,0,0,0,0,0,0,0,0,0,
                3,0,0,0,0,0,0,0,0,3,
            };

             RLGame tabulair = new QLearningTabulair(1, 0.3, 0.7);
            //QLearningNeuralNetworkQuad tabulair = new QLearningNeuralNetworkQuad(1, .1, .8);

            Queue<bool> result = new Queue<bool>();

            Thread.Sleep(100);

            while (true)
            {
                //Console.Clear();
                tabulair.TakeTurn(episode);

                // Thread.Sleep(20);
                Console.WriteLine(tabulair);
                if (tabulair.Finished)
                {

                    result.Enqueue(tabulair.Succes);
                    while (result.Count > currentSuccesScale) { result.Dequeue(); }
                    episode++;
                    if (tabulair.Succes) { succeses++; }
                    else { fails++; }
                    //Thread.Sleep(1000);


                    tabulair.Reset();
                }
                int count = result.Where(v => v == true).Count();
                
                int currentmax = Math.Min(episode, currentSuccesScale);
                double percentage = (currentmax > 0 ? ((count / (currentmax * 1.0)) * 100.0) : 0);
                Console.WriteLine($"Episode: {episode}, {succeses}/{fails} {percentage} {(tabulair.Finished ? tabulair.Succes ? "Succes" : "Fail" : "")}");
            }
        }
    }
}
