using System;
using System.Collections.Generic;
using System.Text;


namespace LR_Test.Game
{
    public class Board
    {
        public const int BOARDSIZE = 4;
        public int[] State { get; internal set; }
      
        /// <summary>
        /// Constructor
        /// </summary>
        public Board()
        {
            this.State = new int[BOARDSIZE * BOARDSIZE];
        }

        /// <summary>
        /// sets a value for coords
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="value"></param>
        public void SetValue(int x, int y, int value)
        {
            State[x + y * BOARDSIZE] = value;
        }

        /// <summary>
        /// sets a value for a point
        /// </summary>
        /// <param name="point"></param>
        /// <param name="value"></param>
        public void SetValue(Point point, int value)
        {
            SetValue(point.X, point.Y, value);
        }

        /// <summary>
        /// Returns a value for coords
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int GetValue(int x, int y)
        {
            return State[x + y * BOARDSIZE];
        }

        /// <summary>
        /// Returns a value for a point
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public int GetValue(Point point)
        {
            return GetValue(point.X, point.Y);
        }

        /// <summary>
        /// Gets a list of value with a certain value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public List<Point> GetPointsForValue(int value)
        {
            List<Point> possiblePoints = new List<Point>();

            for (int y = 0; y < BOARDSIZE; y++)
            {
                for (int x = 0; x < BOARDSIZE; x++)
                {
                    int foundValue = State[x + y * BOARDSIZE];
                    if (foundValue == value)
                    {
                        Point found = new Point(x, y);
                        possiblePoints.Add(found);
                    }
                }
            }

            return possiblePoints;
        }

        /// <summary>
        /// Draws the board
        /// </summary>
        /// <returns></returns>
        public string DrawBoard()
        {
            string result = "------------\n";

            for (int y = 0; y < BOARDSIZE; y++)
            {
                result += "[";
                for (int x = 0; x < BOARDSIZE; x++)
                {
                    double value = Math.Pow(2, State[x + y * BOARDSIZE]);
                    value = value == 1 ? 0 : value;
                    result += $"{value}";
                    if (x + 1 < BOARDSIZE)
                    {
                        result += ", ";
                    }
                }
                result += "]\n";
            }

            result += "------------\n";

            return result;
        }

        /// <summary>
        /// Prints the object
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return DrawBoard();
        }
    }
}
