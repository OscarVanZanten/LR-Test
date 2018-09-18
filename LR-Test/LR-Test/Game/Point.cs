using System;
using System.Collections.Generic;
using System.Text;

namespace LR_Test.Game
{
    public class Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Point() : this(0,0)
        {

        }

        public Point(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }
}
