using LR_Test.ReinforcementLearning.NeuralNetwork;
using System;
using System.Threading;
using System.Linq;
using LR_Test.ReinforcementLearning.Algoritms.QLearning;
using TensorFlow;

namespace LR_Test
{
    class Program
    {


        static void Main(string[] args)
        {
             XORTest();
            //RunGame();

            //TensorFlow();
        }

        public static void XORTest()
        {
            //MyNeuralNetwork network = new MyNeuralNetwork(true, 2, 3, 1);
            SimpleNN network = new SimpleNN( 2, 3, 1);
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
        public static void RunGame()
        {
            //TestRewardFunction();
            //StartTrainer();
            const int width = 10;
            const int height = 5;
            const int spawnx = 0;
            const int spawny = 0;
            int[] level = new int[width * height]
            {
                0,0,0,0,0,0,1,0,0,3,
                0,1,1,0,1,0,0,0,1,1,
                0,1,3,0,1,0,1,0,0,3,
                0,1,1,0,0,0,1,1,0,0,
                0,0,0,0,1,0,0,0,0,2,

            };
            //int[] level = new int[width * height]
            //{
            //    0,0,0,0,0,0,0,0,0,0,
            //    0,0,0,0,0,0,0,0,0,0,
            //    0,0,0,0,0,0,0,0,0,0,
            //    0,0,0,0,0,0,0,0,0,0,
            //    0,0,0,0,0,2,0,0,0,0,
            //    0,0,0,0,0,0,0,0,0,0,
            //    0,0,0,0,0,0,0,0,0,0,
            //    0,0,0,0,0,0,0,0,0,0,
            //    0,0,0,0,0,0,0,0,0,0,
            //    0,0,0,0,0,0,0,0,0,0,
            //};

            // QLearningNeuralNetwork tabulair = new QLearningNeuralNetwork(width, height, level, 0, 0, 9, 9, 0.5, 0.3, 0.7);
            QLearningNeuralNetworkQuad tabulair = new QLearningNeuralNetworkQuad(width,height,level,spawnx, spawny, 9,4, .2, .1, .8);


            Console.WriteLine(tabulair);
            Thread.Sleep(100);

            while (true)
            {
                tabulair.TakeTurn(episode);

                //  Thread.Sleep(20);
                if (tabulair.Finished)
                {
                    Console.Clear();
                    Console.WriteLine(tabulair);

                    episode++;
                    if (tabulair.Succes) { succeses++; }
                    else { fails++; }
                    //Thread.Sleep(1000);

                    Console.WriteLine($"Episode: {episode}, {succeses}/{fails}");
                    Console.WriteLine(tabulair.Succes ? "Succes" : "Fail");
                    tabulair.Reset();
                }
            }
        }
    }
}
