using System;
using System.Collections.Generic;
using System.Text;

namespace LR_Test.Game
{
    public class Game2048
    {
        private const double HIGHERNUMBERPLACEDCHANCE = 0.8d;

        private Random random;

        public Board Board { get; internal set; }

        public long Score { get; internal set; }


        /// <summary>
        /// Constructor
        /// </summary>
        public Game2048()
        {
            this.Board = new Board();
            this.random = new Random();
        }

        public bool CanMove(Board board)
        {
            for (int x = 0; x < Board.BOARDSIZE; x++)
            {
                for (int y = 0; y < Board.BOARDSIZE; y++)
                {
                    var selected = board.GetValue(x, y);

                    if (selected == 0)
                    {
                        return true;
                    }
                    if (x > 0)
                    {
                        if (selected == board.GetValue(x - 1, y))
                        {
                            return true;
                        }
                    }
                    if (y > 0)
                    {
                        if (selected == board.GetValue(x, y - 1))
                        {
                            return true;
                        }

                    }
                }
            }
            return false;
        }

        public bool CanMove()
        {
            return CanMove(Board);
        }

        /// <summary>
        /// Starts the game
        /// </summary>
        public void Start()
        {
            Score = 0;
            Board.Clear();
            PlaceNewNumber();
            PlaceNewNumber();
        }

        public long MakeMove(Move move)
        {
            var scoreFromMove = MakeMove(move, Board);
            scoreFromMove = scoreFromMove >= 0 ? scoreFromMove : 0;
            this.Score += scoreFromMove;

            return scoreFromMove;
        }

        /// <summary>
        /// Makes a move on the board
        /// </summary>
        public long MakeMove(Move move, Board board)
        {

            long scoreFromMove = 0;

            switch (move)
            {
                case Move.Down:
                    scoreFromMove += MoveDown(board);
                    break;
                case Move.Right:
                    scoreFromMove += MoveRight(board);
                    break;
                case Move.Left:
                    scoreFromMove += MoveLeft(board);
                    break;
                case Move.Up:
                    scoreFromMove += MoveUp(board);
                    break;
            }


            return scoreFromMove;
        }

        /// <summary>
        /// Moves the tiles downward
        /// </summary>
        private long MoveDown(Board board)
        {
            long score = 0;

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
                            int newValue = board.GetValue(x, yy) + 1;
                            score += (long)Math.Pow(2, newValue);

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

            return score > 0 ? score : -1;
        }

        /// <summary>
        /// Moves the tiles right
        /// </summary>
        private long MoveRight(Board board)
        {
            long score = 0;

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
                            int newValue = board.GetValue(xx, y) + 1;
                            score += (long)Math.Pow(2, newValue);

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
            return score > 0 ? score : -1;
        }

        /// <summary>
        /// Moves the tiles left
        /// </summary>
        private long MoveLeft(Board board)
        {
            long score = 0;

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
                            int newValue = board.GetValue(xx, y) + 1;
                            score += (long)Math.Pow(2, newValue);

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
            return score > 0 ? score : -1;
        }

        /// <summary>
        /// Moves the tiles upwards
        /// </summary>
        private long MoveUp(Board board)
        {
            long score = 0;
            bool moved = false;
            for (int y = 0; y < Board.BOARDSIZE; y++)
            {
                int i = 0;
                int last = 0;

                for (int x = 0; x < Board.BOARDSIZE; x++)
                {
                    var value = board.GetValue(x, y);

                    if (value == last)
                    {
                        score += (long)Math.Pow(2, value + 1);
                        board.SetValue(i-1, y, value + 1);
                        last = 0;
                        moved = true;
                    }
                    else
                    {
                        moved |= (i != x);
                        board.SetValue(i, y, value );
                        last = value;
                        i += 1;
                    }
                }
                while (i < Board.BOARDSIZE)
                {
                    board.SetValue(i, y, 0);
                    i += 1;
                }
            }
            return moved ? score : -1;
        }




        public bool PlaceNewNumber()
        {
            return PlaceNewNumber(Board);
        }

        /// <summary>
        /// Places a new number in a empty spot
        /// </summary>
        /// <returns>if a new number could have been placed</returns>
        public bool PlaceNewNumber(Board board)
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


        public double RewardFunction(Board board, Move move, double discount, int currentT, int maxT)
        {
            double score = 0;
            var moved = MakeMove(move, board);
            for (int i = 0; i < currentT; i++)
            {
                Console.Write(" ");
            }
            Console.WriteLine($"{currentT}: {move} / {moved}");
            if (moved > 0)
            {
                score += moved * Math.Pow(discount, currentT);

                for (int action = 0; action < 4; action++)
                {
                    int[] state = new int[16];
                    Array.Copy(board.State, state, 16);
                    Board newboard = new Board(state);
                    Move nextmove = (Move)action;

                    score += RewardFunction(newboard, nextmove, discount, currentT + 1, maxT);
                }
            }
            else
            {
                score += 0 * Math.Pow(discount, currentT);
            }

            return score;
        }

        /// <summary>
        /// Prints the game
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string result = Score + "\n" + Board.ToString();

            return result;
        }
    }
}
