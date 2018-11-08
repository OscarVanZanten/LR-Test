using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using LR_Test.RL.NeuralNetwork;
using LR_Test.RL.Algoritms.Models;

namespace LR_Test.RL.Algoritms.Temporal_Difference
{
    /// <summary>
    /// Temporal difference Lambda
    /// </summary>
    public class TemporalDifferenceTabulair : ValueGame
    {
        /// <summary>
        /// Value table for states
        /// </summary>
        private readonly double[] vtable;

        /// <summary>
        /// Elegbility table for states
        /// </summary>
        private readonly double[] etable;

        /// <summary>
        /// Lambda
        /// </summary>
        private readonly double lambda;

        private readonly int maxPolicyEpisodes;

        public TemporalDifferenceTabulair(double alpha, double epsilon, double gamma, double lambda) : base(alpha, epsilon, gamma)
        {
            this.lambda = lambda;
            this.maxPolicyEpisodes = 1000;
            this.vtable = new double[width * height];
            this.etable = new double[width * height];
        }

        public TemporalDifferenceTabulair(int width, int height, int[] level, int spawnX, int spawnY, double alpha, double epsilon, double gamma, double lambda) : base(width, height, level, spawnX, spawnY, alpha, epsilon, gamma)
        {
            this.lambda = lambda;
            this.maxPolicyEpisodes = 1000;
            this.vtable = new double[width * height];
            this.etable = new double[width * height];
        }

        /// <summary>
        /// Ai takes a turn
        /// </summary>
        /// <param name="episode"></param>
        public override void TakeTurn(int episode)
        {
            // Update elegbility for current state
            SetElegbility(agentX, agentY, Elegbility(agentX, agentY) + 1);

            var state_1 = Value(agentX, agentY);
            var move = DetermineMove(agentX, agentY, episode);
            var reward = MakeMove(move);
            var state_2 = Value(agentX, agentY);

            // Calculate TD delta
            var delta = reward + gamma * state_2 - state_1;

            // update values
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    double value = Value(x, y) + alpha * delta * etable[x + y * width];
                    SetValue(x, y, value);
                    SetElegbility(agentX, agentY, Elegbility(agentX, agentY) * lambda * gamma);
                }
            }
            //Check state
            CheckGameState(reward);
        }

        /// <summary>
        /// Resets the game
        /// </summary>
        public override void Reset()
        {
            base.Reset();
            for (int i = 0; i < etable.Length; i++)
            {
                etable[i] = 0;
            }
        }

        /// <summary>
        /// Deterime move (epsilon-Greedy)
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="episode"></param>
        /// <returns></returns>
        protected override int DetermineMove(int x, int y, int episode)
        {
            //Calculate chance for random move
            double epsilonDiscount = episode > 0 ? (episode / (maxPolicyEpisodes * 1.0)) * epsilon : 0;
            double finalEpsilon = epsilon - (epsilonDiscount > epsilon ? epsilon : epsilonDiscount);
            bool randomMove = random.NextDouble() < (finalEpsilon);
            Console.WriteLine($"{finalEpsilon}");

            //Get surounding values
            var values = new double[]
            {
                Value(x, y-1),
                Value(x, y+1),
                Value(x-1, y),
                Value(x+1, y),
            };

            if (randomMove)
            {
                //Take random move
                int move = random.Next(0, 4);

                while (values[move] == -1)
                {
                    Console.WriteLine($"{move} {values[move]}");
                    move = random.Next(0, 4);
                }
                return move;
            }
            else
            {
                // Take move on highest value
                double highestValue = values.OrderByDescending(v => v).First();
                int highestValueIndex = Array.IndexOf<double>(values, highestValue);

                if (highestValue == 0)
                {
                    return random.Next(0, 4);
                }

                return highestValueIndex;
            }
        }

        /// <summary>
        /// Gets values for position x and y 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        protected override double Value(int x, int y)
        {
            if (x < 0 || y < 0 || x > (width - 1) || y > (height - 1))
            {
                return -1;
            }
            if (level[x + y * width] == 1) { return -1; }
            return vtable[x + y * width];
        }

        /// <summary>
        /// Sets values for position x and y 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        protected override void SetValue(int x, int y, double value)
        {
            vtable[x + y * width] = value;
        }

        /// <summary>
        /// Gets Elegbility for position x and y 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private double Elegbility(int x, int y)
        {
            if (x < 0 || y < 0 || x > (width - 1) || y > (height - 1))
            {
                return -1;
            }
            return etable[x + y * width];
        }

        /// <summary>
        /// Sets Elegbility for position x and y 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private void SetElegbility(int x, int y, double e)
        {
            etable[x + y * width] = e;
        }
    }
}
