using LR_Test.Game;
using LR_Test.ReinforcementLearning.NeuralNetwork;
using System;
using System.Threading;
using System.Linq;
using LR_Test.ReinforcementLearning.Training;

namespace LR_Test
{
    class Program
    {
        static void Main(string[] args)
        {
            RunGame();
            //TestRewardFunction();
            //StartTrainer();
        }

        public static void TestRewardFunction()
        {
            double discount = 0.7d;
            int maxT = 10;
            int currentT = 1;

            Game2048 game = new Game2048();
            game.Start();

            Console.WriteLine(game.CanMove());

            while (game.CanMove())
            {
               // Console.Clear();
                int[] state = new int[16];
                Array.Copy(game.Board.State, state, 16);
                Board hypothetical = new Board(state);

                double[] results = new double[4];
                for (int action = 0; action < 4; action++)
                {
                    Move nextmove = (Move)action;

                    var value = game.RewardFunction(hypothetical, nextmove, discount, currentT, maxT);
                    results[action] = value;
                    Console.WriteLine($"{nextmove}: {value}");
                }

                Move move = (Move)(Array.IndexOf<double>(results, results.Max())); 
                game.MakeMove(move);
                game.PlaceNewNumber();
                Console.WriteLine(move);
                Console.WriteLine(game);

              //  Thread.Sleep(1000);
                
            }

            Console.WriteLine("Game over");
            var key = Console.ReadKey();
        }

        public static void StartTrainer()
        {
            TrainingManager manager = new TrainingManager();
            manager.StartTraining();
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
                game.PlaceNewNumber();
                Console.Clear();
                Console.WriteLine(game);
            }
        }
    }
}
