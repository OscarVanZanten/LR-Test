using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using LR_Test.RL.Algoritms.Models;

namespace LR_Test.RL.Algoritms
{
    public abstract class SampleGame : RLGame
    {
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

        public override void Run()
        {
            Queue<bool> result = new Queue<bool>();

            Console.WriteLine(this);
            Thread.Sleep(100);

            while (true)
            {

               for(int i =0; i< 1000; i++)
                {
                    Console.Clear();
                    TakeTurn(episode);

                    Console.WriteLine(this);
                    int count = result.Where(v => v == true).Count();
                    int currentmax = Math.Min(episode, currentSuccesScale);
                    double percentage = (currentmax > 0 ? ((count / (currentmax * 1.0)) * 100.0) : 0);
                    Console.WriteLine($"Episode: {episode}, {succeses}/{fails} {percentage} {(Finished ? Succes ? "Succes" : "Fail" : "")}");

                    if (Finished)
                    {
                        break;
                    }
                }

                ProcessEpisodeSamples(CurrentEpisodeSample);
                CurrentEpisodeSample.Clear();

                episode++;
                result.Enqueue(Succes);

                while (result.Count > currentSuccesScale)
                {
                    result.Dequeue();
                }
                succeses += Succes ? 1 : 0;
                fails += Succes ? 0 : 1;
                Reset();
            }
        }

        protected abstract void ProcessEpisodeSamples(List<Sample> samples);
        protected abstract double Value(int x, int y);
    }
}
