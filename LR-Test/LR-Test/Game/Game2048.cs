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

        public ulong Score { get; internal set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public Game2048()
        {
            this.board = new Board();
            this.random = new Random();
        }

        public void Start()
        {
            PlaceNewNumber();
            PlaceNewNumber();
        }

        /// <summary>
        /// Makes a move on the board
        /// </summary>
        public void MakeMove(Move move)
        {
            if (!CanMakeMove(move)) { return; }

            switch (move)
            {
                case Move.Down:
                    MoveDown();
                    break;
                case Move.Right:
                    MoveRight();
                    break;
                case Move.Left:
                    MoveLeft();
                    break;
                case Move.Up:
                    MoveUp();
                    break;
            }

            PlaceNewNumber();
        }

        /// <summary>
        /// Moves the tiles downward
        /// </summary>
        private void MoveDown()
        {
            for (int x = 0; x < Board.BOARDSIZE; x++)
            {
                for (int y = Board.BOARDSIZE - 1; y >= 0; y--)
                {
                    if (board.GetValue(x, y) == 0) { continue; }

                    bool placed = false;
                    Point lastEmpty = null;

                    for (int yy = y + 1; yy < Board.BOARDSIZE; yy++)
                    {
                        if (board.GetValue(x, yy) == 0)
                        {
                            lastEmpty = new Point(x, yy);
                        }
                        else if (board.GetValue(x, yy) == board.GetValue(x, y))
                        {
                            uint newValue = board.GetValue(x, yy) + 1;
                            Score += (ulong)Math.Pow(2, newValue);

                            board.SetValue(x, yy, newValue);
                            board.SetValue(x, y, 0);
                            placed = true;
                            break;
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (!placed && lastEmpty != null)
                    {
                        board.SetValue(lastEmpty.X, lastEmpty.Y, board.GetValue(x, y));
                        board.SetValue(x, y, 0);
                    }
                }
            }
        }

        /// <summary>
        /// Moves the tiles right
        /// </summary>
        private void MoveRight()
        {
            for (int y = 0; y < Board.BOARDSIZE; y++)
            {
                for (int x = Board.BOARDSIZE - 1; x >= 0; x--)
                {
                    if (board.GetValue(x, y) == 0) { continue; }

                    bool placed = false;
                    Point lastEmpty = null;

                    for (int xx = x + 1; xx < Board.BOARDSIZE; xx++)
                    {
                        if (board.GetValue(xx, y) == 0)
                        {
                            lastEmpty = new Point(xx, y);
                        }
                        else if (board.GetValue(xx, y) == board.GetValue(x, y))
                        {
                            uint newValue = board.GetValue(xx, y) + 1;
                            Score += (ulong)Math.Pow(2, newValue);

                            board.SetValue(xx, y, newValue);
                            board.SetValue(x, y, 0);
                            placed = true;
                            break;
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (!placed && lastEmpty != null)
                    {
                        board.SetValue(lastEmpty.X, lastEmpty.Y, board.GetValue(x, y));
                        board.SetValue(x, y, 0);
                    }
                }
            }
        }

        /// <summary>
        /// Moves the tiles left
        /// </summary>
        private void MoveLeft()
        {
            for (int y = 0; y < Board.BOARDSIZE; y++)
            {
                for (int x = 0; x < Board.BOARDSIZE; x++)
                {
                    if (board.GetValue(x, y) == 0) { continue; }

                    bool placed = false;
                    Point lastEmpty = null;

                    for (int xx = x - 1; xx >= 0; xx--)
                    {
                        if (board.GetValue(xx, y) == 0)
                        {
                            lastEmpty = new Point(xx, y);
                        }
                        else if (board.GetValue(xx, y) == board.GetValue(x, y))
                        {
                            uint newValue = board.GetValue(xx, y) + 1;
                            Score += (ulong)Math.Pow(2, newValue);

                            board.SetValue(xx, y, newValue);
                            board.SetValue(x, y, 0);
                            placed = true;
                            break;
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (!placed && lastEmpty != null)
                    {
                        board.SetValue(lastEmpty.X, lastEmpty.Y, board.GetValue(x, y));
                        board.SetValue(x, y, 0);
                    }
                }
            }
        }

        /// <summary>
        /// Moves the tiles upwards
        /// </summary>
        private void MoveUp()
        {
            for (int x = 0; x < Board.BOARDSIZE; x++)
            {
                for (int y = 0; y < Board.BOARDSIZE; y++)
                {
                    if (board.GetValue(x, y) == 0) { continue; }

                    bool placed = false;
                    Point lastEmpty = null;

                    for (int yy = y - 1; yy >= 0; yy--)
                    {
                        if (board.GetValue(x, yy) == 0)
                        {
                            lastEmpty = new Point(x, yy);
                        }
                        else if (board.GetValue(x, yy) == board.GetValue(x, y))
                        {
                            uint newValue = board.GetValue(x, yy) + 1;
                            Score += (ulong)Math.Pow(2, newValue);

                            board.SetValue(x, yy, newValue);
                            board.SetValue(x, y, 0);
                            placed = true;
                            break;
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (!placed && lastEmpty != null)
                    {
                        board.SetValue(lastEmpty.X, lastEmpty.Y, board.GetValue(x, y));
                        board.SetValue(x, y, 0);
                    }
                }
            }
        }

        /// <summary>
        /// Checks whether a move is possible
        /// </summary>
        /// <returns>if a move is possible</returns>
        public bool CanMakeMove(Move move)
        {
            for (int x = 0; x < Board.BOARDSIZE; x++)
            {
                for (int y = 0; y < Board.BOARDSIZE; y++)
                {
                    if (CanMakeMove(move, x, y))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Checks whether a move is possible for a certain point
        /// </summary>
        /// <returns>if a move is possible</returns>
        public bool CanMakeMove(Move move, Point point)
        {
            return CanMakeMove(move, point.X, point.Y);
        }

        /// <summary>
        /// Checks whether a move is possible for a certain coords
        /// </summary>
        /// <returns>if a move is possible</returns>
        public bool CanMakeMove(Move move, int x, int y)
        {
            if (board.GetValue(x, y) == 0)
            {
                return false;
            }

            switch (move)
            {
                case Move.Down:
                    for (int yy = y; yy < Board.BOARDSIZE; yy++)
                    {
                        if (board.GetValue(x, yy) == 0) { return true; }
                        if (yy + 1 < Board.BOARDSIZE)
                        {
                            if (board.GetValue(x, yy) == board.GetValue(x, yy + 1)) { return true; }
                        }
                    }
                    break;
                case Move.Right:
                    for (int xx = x; xx < Board.BOARDSIZE; xx++)
                    {
                        if (board.GetValue(xx, y) == 0) { return true; }
                        if (xx + 1 < Board.BOARDSIZE)
                        {
                            if (board.GetValue(xx, y) == board.GetValue(xx + 1, y)) { return true; }
                        }
                    }
                    break;
                case Move.Left:
                    for (int xx = x; xx >= 0; xx--)
                    {
                        if (board.GetValue(xx, y) == 0) { return true; }
                        if (xx - 1 >= 0)
                        {
                            if (board.GetValue(xx, y) == board.GetValue(xx - 1, y)) { return true; }
                        }
                    }
                    break;
                case Move.Up:
                    for (int yy = y; yy >= 0; yy--)
                    {
                        if (board.GetValue(x, yy) == 0) { return true; }
                        if (yy - 1 >= 0)
                        {
                            if (board.GetValue(x, yy) == board.GetValue(x, yy - 1)) { return true; }
                        }
                    }
                    break;
            }
            return false;
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
            string result = Score + "\n" + board.ToString();

            return result;
        }
    }
}
