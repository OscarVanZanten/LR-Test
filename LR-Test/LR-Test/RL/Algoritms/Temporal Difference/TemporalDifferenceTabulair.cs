using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using LR_Test.RL.NeuralNetwork;

namespace LR_Test.RL.Algoritms.Temporal_Difference
{
    public class TemporalDifferenceTabulair : ValueGame
    {
        private readonly double[] vtable;

        public TemporalDifferenceTabulair(double alpha, double epsilon, double gamma) : base(alpha, epsilon, gamma)
        {
            this.vtable = new double[width * height];
        }

        public TemporalDifferenceTabulair(int width, int height, int[] level, int spawnX, int spawnY, double alpha, double epsilon, double gamma) : base(width, height, level, spawnX, spawnY, alpha, epsilon, gamma)
        {
            this.vtable = new double[width * height];
        }

        public override void TakeTurn(int episode)
        {
            int agentx1 = agentX;
            int agenty1 = agentY;
            var move = DetermineMove(agentX, agentY, episode);

            var value1 = Value(agentX, agentY);
            var reward = MakeMove(move);

            var value2 = Value(agentX, agentY);

            var update = CalculateUpdatedValue(value1, value2, reward);

            Value(agentx1, agenty1);
            var valueNorth = Value(agentx1, agenty1 - 1);
            var valueSouth = Value(agentx1, agenty1 + 1);
            var valueWest = Value(agentx1 - 1, agenty1);
            var valueEast = Value(agentx1 + 1, agenty1);

            //Console.WriteLine($"Values: {valueNorth} {valueSouth} {valueWest} {valueEast}");
            //Console.WriteLine($"Values: {value1}");
            //Console.WriteLine($"Values: {update}");
            SetValue(agentx1, agenty1, update);

            CheckGameState(reward);
        }

        protected override int DetermineMove(int x, int y, int episode)
        {
            bool randomMove = random.NextDouble() < epsilon;

            var values = new double[]
               {
                    Value(x, y-1),
                    Value(x, y+1),
                    Value(x-1, y),
                    Value(x+1, y),
               };

            if (randomMove)
            {
                int move = random.Next(0, 4);

                while (values[move] == -1)
                {
                    Console.WriteLine($"{move} {values[move]}");
                    move = random.Next(0, 4);
                }
                //Console.WriteLine($"{move} {values[move]}");
                return move;
            }
            else
            {
                double highestValue = values.OrderByDescending(v => v).First();
                int highestValueIndex = Array.IndexOf<double>(values, highestValue);

                if (highestValue == 0)
                {
                    return random.Next(0, 4);
                }

                return highestValueIndex;
            }
        }

        protected override double Value(int x, int y)
        {
            if (x < 0 || y < 0 || x > (width - 1) || y > (height - 1))
            {
                return -1;
            }
            if (level[x + y * width] == 1) { return -1; }
            return vtable[x + y * width];
        }

        protected override void SetValue(int x, int y, double value)
        {
            vtable[x + y * width] = value;
        }
    }
}
