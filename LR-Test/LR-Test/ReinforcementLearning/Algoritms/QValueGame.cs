using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace LR_Test.ReinforcementLearning.Algoritms
{
    public abstract class QValueGame : RLGame
    {
        public QValueGame(int width, int height, int[] level, int spawnX, int spawnY, double alpha, double epsilon, double gamma) : base(width, height, level, spawnX, spawnY, alpha, epsilon, gamma) { }

        public QValueGame(double alpha, double epsilon, double gamma) : base(alpha, epsilon, gamma) { }

        public override void TakeTurn(int episode)
        {
            int agentx1 = agentX, agenty1 = agentY;

            var move = DetermineMove(agentX, agentY, episode);

            var qvals1 = QValues(agentX, agentY);
            var maxQ1 = HighestQValues(qvals1);

            var reward = MakeMove(move);

            var qvals2 = QValues(agentX, agentY);
            var maxQ2 = HighestQValues(qvals2);

            var update = CalculateUpdatedQValue(qvals1[move], maxQ2, (float)reward);

            QValues(agentx1, agenty1);
            Console.WriteLine($"Q-Values: {qvals1[0]}, {qvals1[1]}, {qvals1[2]}, {qvals1[3]}");
            qvals1[move] = update;
            Console.WriteLine($"Q-Values: {qvals1[0]}, {qvals1[1]}, {qvals1[2]}, {qvals1[3]}");
            SetQValues(agentx1, agenty1, qvals1);
            Console.WriteLine($"UpdatedQValue: {update}");

            CheckGameState(reward);
        }

        protected abstract double[] QValues(int x, int y);
        protected abstract void SetQValues(int x, int y, double[] values);

        protected double HighestQValues(int x, int y)
        {
            var qvalues = QValues(x, y);
            return qvalues.OrderByDescending(q => q).First();
        }

        protected double HighestQValues(double[] qvalues)
        {
            return qvalues.OrderByDescending(q => q).First();
        }

        protected abstract double CalculateUpdatedQValue(double qvalue1, double qvalue2, double reward);


    }
}
