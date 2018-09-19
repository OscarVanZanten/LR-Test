using LR_Test.Game;
using LR_Test.ReinforcementLearning.NeuralNetwork;
using System;

namespace LR_Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //RunGame();
            TestNeuralNetwork();
        }

        public static void TestNeuralNetwork()
        {
            Console.Out.WriteLine("Neural Network");
            NeuralNetwork network = NeuralNetworkBuilder.Generate(";", 16, 8, 8, 4);
            Console.Out.WriteLine(network);
            var key = Console.ReadKey();
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
