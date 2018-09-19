using LR_Test.Game;
using LR_Test.ReinforcementLearning.NeuralNetwork;
using System;
using System.Threading;
using System.Linq;

namespace LR_Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //RunGame();
            //TestNeuralNetwork();
            RunNeuralNetworkGame();
        }

        public static void TestNeuralNetwork()
        {
            Console.Out.WriteLine("Neural Network");

            int[] networkFormat = new int[] { 16, 8, 8, 4 };

            string data = NeuralNetworkBuilder.GenerateRandomNeuralNetworkData(networkFormat);
            Console.Out.WriteLine(data);

            NeuralNetwork network = NeuralNetworkBuilder.GenerateNeuralNetwork(data, networkFormat);
            Console.Out.WriteLine(network);

            var key = Console.ReadKey();
        }

        public static void RunNeuralNetworkGame()
        {
            Random random = new Random();
            int[] networkFormat = new int[] { 16,8, 8, 4 };

            string data = NeuralNetworkBuilder.GenerateRandomNeuralNetworkData(networkFormat);

            Game2048 game = new Game2048();
            game.Start();
            Console.WriteLine(game);

            int lastValue =-1;

            NeuralNetwork network = NeuralNetworkBuilder.GenerateNeuralNetwork(data, networkFormat);
            while (!game.IsGameOver)
            {
                var result = network.Execute(game.Board.State);
                var highest = result.Max();
                var action = Array.IndexOf<double>(result, highest);

                if (lastValue == action)
                {
                    action = random.Next(4);
                }
                else
                {
                    lastValue = action;
                }

                Move move = (Move)action;

                game.MakeMove(move);
                Console.Clear();

                Console.WriteLine(game);
                Console.WriteLine(highest);
                Console.WriteLine(move);

                Thread.Sleep(10);

            }
            Console.ReadKey();
        }

        public static void RunGame()
        {
            Game2048 game = new Game2048();
            game.Start();
            Console.WriteLine(game);

            while (true)
            {
                var key = Console.ReadKey();
                switch (key.KeyChar)
                {
                    case 'w':
                        game.MakeMove(Move.Up);
                        break;
                    case 's':
                        game.MakeMove(Move.Down);
                        break;
                    case 'a':
                        game.MakeMove(Move.Left);
                        break;
                    case 'd':
                        game.MakeMove(Move.Right);
                        break;
                }
                Console.Clear();
                Console.WriteLine(game);
            }
        }
    }
}
