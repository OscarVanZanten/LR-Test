using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using LR_Test.RL.Algoritms.Models;

namespace LR_Test.RL.Algoritms
{
    /// <summary>
    /// Sample Game
    /// </summary>
    public abstract class SampleGame : RLGame
    {
        /// <summary>
        /// List of samples
        /// </summary>
        protected List<Sample> CurrentEpisodeSample { get; set; }

        public SampleGame()
        {
            CurrentEpisodeSample = new List<Sample>();
        }

        public SampleGame(double alpha, double epsilon, double gamma) : base(alpha, epsilon, gamma)
        {
            CurrentEpisodeSample = new List<Sample>();
        }

        public SampleGame(int width, int height, int[] level, int spawnX, int spawnY, double alpha, double epsilon, double gamma) : base(width, height, level, spawnX, spawnY, alpha, epsilon, gamma)
        {
            CurrentEpisodeSample = new List<Sample>();
        }

        /// <summary>
        /// Training loop
        /// </summary>
        public override void Run()
        {
            //succes/fail results from last few sprints
            Queue<bool> result = new Queue<bool>();

            while (true)
            {
                //Limit of steps taken within the episode
                for (int i = 0; i < 1000; i++)
                {
                    //Take the turn
                    TakeTurn(episode);

                    //Print the board and results
                    Console.Clear();
                    Console.WriteLine(this);
                    int count = result.Where(v => v == true).Count();
                    int currentmax = Math.Min(episode, currentSuccesScale);
                    double percentage = (currentmax > 0 ? ((count / (currentmax * 1.0)) * 100.0) : 0);
                    Console.WriteLine($"Episode: {episode}, {succeses}/{fails} {percentage} {(Finished ? Succes ? "Succes" : "Fail" : "")}");

                    // Slow down game
                    Thread.Sleep(50);

                    // Check if the episode is finished and stops the game
                    if (Finished)
                    {
                        break;
                    }
                }

                //Process the samples
                ProcessEpisodeSamples(CurrentEpisodeSample);
                CurrentEpisodeSample.Clear();

                // Update result track record
                episode++;
                result.Enqueue(Succes);

                while (result.Count > currentSuccesScale)
                {
                    result.Dequeue();
                }
                succeses += Succes ? 1 : 0;
                fails += Succes ? 0 : 1;

                //Reset game
                Reset();
            }
        }

        /// <summary>
        /// Process samples from episode
        /// </summary>
        /// <param name="samples"></param>
        protected abstract void ProcessEpisodeSamples(List<Sample> samples);

        /// <summary>
        /// Get value from state, position x and y
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        protected abstract double Value(int x, int y);
    }
}
