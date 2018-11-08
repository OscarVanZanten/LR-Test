using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace LR_Test.RL.Algoritms.QLearning
{
    public class QLearningTabulair : QValueGame
    {
        //table of Q-Values
        private readonly double[][] qtable; // Q value table

        public QLearningTabulair(int width, int height, int[] level, int spawnX, int spawnY, double alpha, double epsilon, double gamma) : base(width, height, level, spawnX, spawnY, alpha, epsilon, gamma)
        {
            this.qtable = new double[width * height][];
        }

        public QLearningTabulair(double alpha, double epsilon, double gamma) : base(alpha, epsilon, gamma)
        {
            this.qtable = new double[width * height][];
        }

        /// <summary>
        /// AI takes turn in the game
        /// </summary>
        /// <param name="episode"></param>
        public override void TakeTurn(int episode)
        {
            int agentx1 = agentX, agenty1 = agentY;

            // Get move
            var move = DetermineMove(agentX, agentY, episode);

            var qvals1 = QValues(agentX, agentY);
            var maxQ1 = HighestQValue(qvals1);

            //get reward from move
            var reward = MakeMove(move);

            var qvals2 = QValues(agentX, agentY);
            var maxQ2 = HighestQValue(qvals2);

            // Update qvalue
            var update = CalculateUpdatedQValue(qvals1[move], maxQ2, (float)reward);

            QValues(agentx1, agenty1);
            Console.WriteLine($"Q-Values: {qvals1[0]}, {qvals1[1]}, {qvals1[2]}, {qvals1[3]}");
            qvals1[move] = update;
            Console.WriteLine($"Q-Values: {qvals1[0]}, {qvals1[1]}, {qvals1[2]}, {qvals1[3]}");
            SetQValues(agentx1, agenty1, qvals1);
            Console.WriteLine($"UpdatedQValue: {update}");

            CheckGameState(reward);
        }

        /// <summary>
        /// Move deterimened by policy
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="episode"></param>
        /// <returns></returns>
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
                double highestValue = HighestQValue(x, y);
                int highestValueIndex = Array.IndexOf<double>(qvalues, highestValue);

                if (highestValue == 0)
                {
                    return random.Next(0, 4);
                }

                return highestValueIndex;
            }
        }

        /// <summary>
        /// Gets Q-Values for position x and y for every action
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        protected override double[] QValues(int x, int y)
        {
            if (qtable[x + y * width] == null)
            {
                qtable[x + y * width] = new double[4];
            }
            return qtable[x + y * width];
        }

        // <summary>
        /// Sets Q-Values for position x and y for every action
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="values"></param>
        protected override void SetQValues(int x, int y ,double[] values)
        {
            qtable[x + y * width] = values;
        }
    }
}
