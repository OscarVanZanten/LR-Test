using LR_Test.Game;
using System;

namespace LR_Test
{
    class Program
    {
        static void Main(string[] args)
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
