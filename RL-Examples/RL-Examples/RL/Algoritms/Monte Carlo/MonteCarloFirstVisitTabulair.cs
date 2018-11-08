using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LR_Test.RL.Algoritms.Models;

namespace LR_Test.RL.Algoritms.Monte_Carlo
{
    public class MonteCarloFistVisitTabulair : SampleGame
    {
        /// <summary>
        /// table of values of states
        /// </summary>
        private readonly double[] value;
        /// <summary>
        /// table of visits to states
        /// </summary>
        private readonly int[] visits;
        /// <summary>
        /// table of total rewards for states
        /// </summary>
        private readonly double[] totalrewards;
        private readonly int maxPolicyEpisodes;

        public MonteCarloFistVisitTabulair()
        {
            this.maxPolicyEpisodes = 10000;
            value = new double[width * height];
            visits = new int[width * height];
            totalrewards = new double[width * height];

        }

        public MonteCarloFistVisitTabulair(double alpha, double epsilon, double gamma) : base(alpha, epsilon, gamma)
        {
            this.maxPolicyEpisodes = 10000;
            value = new double[width * height];
            visits = new int[width * height];
            totalrewards = new double[width * height];
        }

        public MonteCarloFistVisitTabulair(int width, int height, int[] level, int spawnX, int spawnY, double alpha, double epsilon, double gamma) : base(width, height, level, spawnX, spawnY, alpha, epsilon, gamma)
        {
            this.maxPolicyEpisodes = 10000;
            value = new double[width * height];
            visits = new int[width * height];
            totalrewards = new double[width * height];
        }

        /// <summary>
        /// AI takes turn in the game
        /// </summary>
        /// <param name="episode"></param>
        public override void TakeTurn(int episode)
        {
            int agentX_s1 = agentX;
            int agentY_s1 = agentY;
            int move = DetermineMove(agentX, agentY, episode);
            double reward = MakeMove(move);

            CurrentEpisodeSample.Add(new Sample()
            {
                XTo = agentX,
                YTo = agentY,
                Action = move,
                Reward = reward
            });

            CheckGameState(reward);
        }

        /// <summary>
        /// Processes the samples of an episode
        /// </summary>
        /// <param name="samples"></param>
        protected override void ProcessEpisodeSamples(List<Sample> samples)
        {
            var processed = new bool[width, height];

            //iterate through every process
            foreach (var sample in samples)
            {
                // Check if its processed
                if (processed[sample.XTo, sample.YTo] == false)
                {
                    ///comparing tool
                    bool from(Sample s)
                    {
                        return s.XTo == sample.XTo && s.YTo == sample.YTo;
                    }

                    //finda ll samples after
                    var allSamples = samples.FindIndex(from);// samples.Where(s => s.X == sample.X && s.Y == sample.Y).ToList();

                    double totalReward = 0;
                    int counter = 0;

                    //calculate total reward
                    for (int i = allSamples; i < samples.Count; i++)
                    {
                        totalReward += samples[i].Reward * (Math.Pow(gamma, counter));
                        counter++;
                    }

                    //update values
                    totalrewards[sample.XTo + sample.YTo * width] += totalReward;
                    visits[sample.XTo + sample.YTo * width] += 1;
                    value[sample.XTo + sample.YTo * width] = totalrewards[sample.XTo + sample.YTo * width] / visits[sample.XTo + sample.YTo * width];
                    processed[sample.XTo, sample.YTo] = true;
                }
            }
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
                return random.Next(0, 4);
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
        /// Process samples from episode
        /// </summary>
        /// <param name="samples"></param>
        protected override double Value(int x, int y)
        {
            if (x < 0 || y < 0 || x > (width - 1) || y > (height - 1))
            {
                return -1;
            }
            if (level[x + y * width] == 1) { return -1; }


            return this.value[x + y * width];
        }

        /// <summary>
        /// Get value from state, position x and y
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public override string ToString()
        {
            string result = base.ToString() + '\n' + '\n';

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    result += $" {value[x + y * width].ToString("G3")}";
                }
                result += '\n';
            }

            return result;
        }
    }
}
