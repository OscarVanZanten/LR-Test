using System;
using System.Threading;
using System.Linq;
using System.Collections.Generic;

using LR_Test.RL;
using LR_Test.RL.NeuralNetwork;
using LR_Test.RL.Algoritms.SARSA;
using LR_Test.RL.Algoritms.QLearning;
using LR_Test.RL.Algoritms.Temporal_Difference;

namespace LR_Test
{
    public class Program
    {
        private const string XORTestName = "XOR BackPropagation test";

        private static readonly double alpha = .3;
        private static readonly double epsilon = .2;
        private static readonly double gamma = .8;

        private static int fails = 0;
        private static int episode = 0;
        private static int succeses = 0;
        private static Dictionary<string, RLGame> methodes;
        private static readonly int currentSuccesScale = 1000;

        static void Main(string[] args)
        {
            Init();
            Console.WriteLine(MethodesList());

            while (true)
            {
                int index = SelectIndex();
                ExecuteMethode(index);
            }
        }

        public static void Init()
        {
            methodes = new Dictionary<string, RLGame>()
            {
                {"QLearning Tabulair", new QLearningTabulair(alpha,epsilon,gamma) },
                {"QLearning NeuralNetwork", new QLearningNeuralNetwork(1,epsilon,gamma) },
                {"QLearning NeuralNetwork v2", new QLearningNeuralNetworkQuad(1,epsilon,gamma) },
                {"SARSA Tabulair", new SARSATabulair(alpha,epsilon,gamma) },
                {"SARSA NeuralNetwork", new SARSANeuralNetwork(1,epsilon,gamma) },
                {"Temporal Difference Tabulair", new TemporalDifferenceTabulair(alpha,epsilon,gamma) },
                {"Temporal Difference NeuralNetwork", new TemporalDifferenceNeuralNetwork(1,epsilon,gamma) },
            };
        }

        public static string MethodesList()
        {
            string result = "Available methodes:\n";

            int i = 0;
            result += $"{i}: {XORTestName}\n";

            foreach (var methode in methodes)
            {
                i++;
                result += $"{i}: {methode.Key}\n";
            }

            return result;
        }

        public static int SelectIndex()
        {
            Console.WriteLine("Select index to use that methode");
            int index = 0;
            string result = Console.ReadLine();
            while (!int.TryParse(result, out index))
            {
                Console.WriteLine("Invalid index");
                result = Console.ReadLine();
            }

            return index;
        }

        public static void ExecuteMethode(int index)
        {
            if (index == 0)
            {
                XORTest();
            }

            if (index <= methodes.Count)
            {
                RunGame(methodes.ElementAt(index - 1).Value);
            }
        }

        public static void XORTest()
        {
            SimpleNN network = new SimpleNN(0.5, 2, 3, 1);
            while (true)
            {
                Thread.Sleep(10);
                Console.Clear();
                Console.WriteLine($"XOR NN Backpropagation");

                var result = network.FeedForward(1f, 1f);
                Console.WriteLine($"(1,1): result={result[0]} expected=0");
                network.BackPropagate(0);

                result = network.FeedForward(0f, 1f);
                Console.WriteLine($"(0,1): result={result[0]} expected=1");
                network.BackPropagate(1);

                result = network.FeedForward(1f, 0f);
                Console.WriteLine($"(1,0): result={result[0]} expected=1");
                network.BackPropagate(1);

                result = network.FeedForward(0, 0);
                Console.WriteLine($"(0,0): result={result[0]} expected=0");
                network.BackPropagate(0);

                Console.WriteLine();
            }
        }

        public static void RunGame(RLGame game)
        {
            Queue<bool> result = new Queue<bool>();

            Thread.Sleep(100);

            while (true)
            {
                Console.Clear();
                game.TakeTurn(episode);

                Console.WriteLine(game);

                if (game.Finished)
                {
                    episode++;
                    result.Enqueue(game.Succes);

                    while (result.Count > currentSuccesScale)
                    {
                        result.Dequeue();
                    }

                    if (game.Succes)
                    {
                        succeses++;
                    }
                    else
                    {
                        fails++;
                    }

                    //Thread.Sleep(1000);
                    game.Reset();
                }
                int count = result.Where(v => v == true).Count();

                int currentmax = Math.Min(episode, currentSuccesScale);
                double percentage = (currentmax > 0 ? ((count / (currentmax * 1.0)) * 100.0) : 0);
                Console.WriteLine($"Episode: {episode}, {succeses}/{fails} {percentage} {(game.Finished ? game.Succes ? "Succes" : "Fail" : "")}");

                Thread.Sleep(50);
            }
        }
    }
}
