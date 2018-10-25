using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LR_Test.RL.Algoritms.Models;

namespace LR_Test.RL.Algoritms.Monte_Carlo
{
    public class MonteCarloFistVisit : SampleGame
    {
        private readonly double[] value;
        private readonly int[] visits;
        private readonly double[] totalrewards;
        private readonly int maxPolicyEpisodes;

        public MonteCarloFistVisit()
        {
            this.maxPolicyEpisodes = 10000;
            value = new double[width * height];
            visits = new int[width * height];
            totalrewards = new double[width * height];

        }

        public MonteCarloFistVisit(double alpha, double epsilon, double gamma) : base(alpha, epsilon, gamma)
        {
            this.maxPolicyEpisodes = 10000;
            value = new double[width * height];
            visits = new int[width * height];
            totalrewards = new double[width * height];
        }

        public MonteCarloFistVisit(int width, int height, int[] level, int spawnX, int spawnY, double alpha, double epsilon, double gamma) : base(width, height, level, spawnX, spawnY, alpha, epsilon, gamma)
        {
            this.maxPolicyEpisodes = 10000;
            value = new double[width * height];
            visits = new int[width * height];
            totalrewards = new double[width * height];
        }


        public override void TakeTurn(int episode)
        {
            int agentX_s1 = agentX;
            int agentY_s1 = agentY;
            int move = DetermineMove(agentX, agentY, episode);
            double reward = MakeMove(move);

            CurrentEpisodeSample.Add(new Sample()
            {
                X = agentX,
                Y = agentY,
                Action = move,
                Reward = reward
            });

            CheckGameState(reward);
        }

        protected override void ProcessEpisodeSamples(List<Sample> samples)
        {
            var processed = new bool[width, height];

            foreach (var sample in samples)
            {
                if (processed[sample.X, sample.Y] == false)
                {
                    bool from(Sample s)
                    {
                        return s.X == sample.X && s.Y == sample.Y;
                    }

                    var allSamples = samples.FindIndex(from);// samples.Where(s => s.X == sample.X && s.Y == sample.Y).ToList();

                    double totalReward = 0;
                    int counter = 0;
                    for (int i = allSamples; i < samples.Count; i++)
                    {
                        totalReward += samples[i].Reward * (Math.Pow(gamma, counter));
                        counter++;
                    }

                    totalrewards[sample.X + sample.Y * width] += totalReward;
                    visits[sample.X + sample.Y * width] += 1;
                    value[sample.X + sample.Y * width] = totalrewards[sample.X + sample.Y * width] / visits[sample.X + sample.Y * width];
                    processed[sample.X, sample.Y] = true;
                }
            }
        }

        protected override int DetermineMove(int x, int y, int episode)
        {
            double epsilonDiscount = episode > 0 ? (episode / (maxPolicyEpisodes * 1.0)) * epsilon : 0;
            double finalEpsilon = epsilon - (epsilonDiscount > epsilon ? epsilon : epsilonDiscount);
            bool randomMove = random.NextDouble() < (finalEpsilon);

            Console.WriteLine($"{finalEpsilon}");

            var values = new double[]
              {
                    Value(x, y-1),
                    Value(x, y+1),
                    Value(x-1, y),
                    Value(x+1, y),
              };

            if (randomMove)
            {
                return random.Next(0, 4);
            }
            else
            {
                double highestValue = values.OrderByDescending(v => v).First();
                int highestValueIndex = Array.IndexOf<double>(values, highestValue);
                Console.WriteLine($"{values[0]} {values[1]} {values[2]} {values[3]}");
                Console.WriteLine($"{highestValue} {x} {y}");
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


            return this.value[x + y * width];
        }

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
