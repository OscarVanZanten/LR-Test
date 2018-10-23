using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace LR_Test.RL.Algoritms.SARSA
{
    public class SARSATabulair : QValueGame
    {
        private readonly double[][] qtable; // Q value table

        public SARSATabulair(int width, int height, int[] level, int spawnX, int spawnY, double alpha, double epsilon, double gamma) : base(width, height, level, spawnX, spawnY, alpha, epsilon, gamma)
        {
            this.qtable = new double[width * height][];
        }

        public SARSATabulair(double alpha, double epsilon, double gamma) : base(alpha, epsilon, gamma)
        {
            this.qtable = new double[width * height][];
        }

        public override void TakeTurn(int episode)
        {
            int agentx1 = agentX, agenty1 = agentY;

            var move = DetermineMove(agentX, agentY, episode);

            var qvals1 = QValues(agentX, agentY);
            var maxQ1 = HighestQValues(qvals1);

            var reward = MakeMove(move);

            var qvals2 = QValues(agentX, agentY);
            var maxQ2 = qvals2[DetermineMove(agentX, agentY, episode)];

            var update = CalculateUpdatedQValue(qvals1[move], maxQ2, (float)reward);

            QValues(agentx1, agenty1);
            Console.WriteLine($"Q-Values: {qvals1[0]}, {qvals1[1]}, {qvals1[2]}, {qvals1[3]}");
            qvals1[move] = update;
            Console.WriteLine($"Q-Values: {qvals1[0]}, {qvals1[1]}, {qvals1[2]}, {qvals1[3]}");
            SetQValues(agentx1, agenty1, qvals1);
            Console.WriteLine($"UpdatedQValue: {update}");

            CheckGameState(reward);
        }

        protected override int DetermineMove(int x, int y, int episode)
        {
            bool randomMove = random.NextDouble() < epsilon;

            if (randomMove)
            {
                return random.Next(0, 4);
            }
            else
            {
                var qvalues = QValues(x, y);
                double highestValue = HighestQValues(x, y);
                int highestValueIndex = Array.IndexOf<double>(qvalues, highestValue);

                if (highestValue == 0)
                {
                    return random.Next(0, 4);
                }

                return highestValueIndex;
            }
        }

        protected override double[] QValues(int x, int y)
        {
            if (qtable[x + y * width] == null)
            {
                qtable[x + y * width] = new double[4];
            }
            return qtable[x + y * width];
        }

        protected override void SetQValues(int x, int y ,double[] values)
        {
            qtable[x + y * width] = values;
        }
    }
}
