using LR_Test.Game;
using System;

namespace LR_Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Game2048 game = new Game2048();



            while (true)
            {
                game.PlaceNewNumber();
                Console.WriteLine(game);
                Console.ReadKey();
            }
        }
    }
}
