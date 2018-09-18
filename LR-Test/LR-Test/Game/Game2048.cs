using System;
using System.Collections.Generic;
using System.Text;

namespace LR_Test.Game
{
    public class Game2048
    {
        private const double HIGHERNUMBERPLACEDCHANCE = 0.8d;

        private Board board;
        private Random random;

        /// <summary>
        /// Constructor
        /// </summary>
        public Game2048()
        {
            this.board = new Board();
            this.random = new Random();
        }

        /// <summary>
        /// Checks whether a move is possible
        /// </summary>
        /// <returns>if a move is possible</returns>
        public bool CanMakeMove(Move move)
        {
            return false;
        }

        /// <summary>
        /// Makes a move on the board
        /// </summary>
        public void MakeMove(Move move)
        {

        }

        /// <summary>
        /// Places a new number in a empty spot
        /// </summary>
        /// <returns>if a new number could have been placed</returns>
        public bool PlaceNewNumber()
        {
            List<Point> emptyPoints = board.GetPointsForValue(0);

            // Return false if no points are available
            if (emptyPoints.Count == 0)
            {
                return false;
            }

            int randomPointIndex = random.Next(emptyPoints.Count);
            Point selected = emptyPoints[randomPointIndex];

            // Places a higher number randomly if higher than the predefined chance
            bool higherNumberPlaced = random.NextDouble() > HIGHERNUMBERPLACEDCHANCE;

            if (higherNumberPlaced)
            {
                board.SetValue(selected, 2);
            }
            else
            {
                board.SetValue(selected, 1);
            }

            return true;
        }

        /// <summary>
        /// Prints the game
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return board.ToString();
        }
    }
}
