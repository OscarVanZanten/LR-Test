﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace LR_Test.RL
{
    /// <summary>
    /// Tile types in the level
    /// </summary>
    public enum TileType { Empty, Wall, Goal, Fail }

    public abstract class RLGame
    {
        protected const double DEFAULT_ALPHA = 0.1;
        protected const double DEFAULT_EPSILON = 0.2;
        protected const double DEFAULT_GAMMA = 0.9;

        protected const int DEFAULT_WIDTH = 4;
        protected const int DEFAULT_HEIGHT = 3;
        protected const int DEFAULT_SPAWNX = 0;
        protected const int DEFAULT_SPAWNY = 2;

        protected static readonly int[] DEFAULT_LEVEL = new int[DEFAULT_WIDTH * DEFAULT_HEIGHT]
        {
            0,0,0,2,
            0,1,0,3,
            0,0,0,0
        };

        protected Random random;
        protected int width;
        protected int height;
        protected int spawnX = 0;
        protected int spawnY = 2;
        protected int agentX, agentY; // Agent location
        protected int[] level; // level map 

        protected double alpha; // Learning rate
        protected double epsilon; // Random chance of doing a random move
        protected double gamma; // Discount rate

        public bool Finished { get; protected set; }
        public bool Succes { get; protected set; }

        protected static int fails = 0;
        protected static int episode = 0;
        protected static int succeses = 0;
        protected static readonly int currentSuccesScale = 100;

        public RLGame(int width, int height, int[] level, int spawnX, int spawnY, double alpha, double epsilon, double gamma)
        {
            this.random = new Random();

            this.level = level;
            this.width = width;
            this.height = height;
            this.spawnX = spawnX;
            this.spawnY = spawnY;

            this.alpha = alpha;
            this.epsilon = epsilon;
            this.gamma = gamma;
            this.agentX = 0;
            this.agentY = 2;
        }

        public RLGame(double alpha, double epsilon, double gamma) : this(DEFAULT_WIDTH, DEFAULT_HEIGHT, DEFAULT_LEVEL, DEFAULT_SPAWNX, DEFAULT_SPAWNY, alpha, epsilon, gamma)
        {
        }

        public RLGame() : this(DEFAULT_ALPHA, DEFAULT_EPSILON, DEFAULT_GAMMA)
        {
        }

        /// <summary>
        /// Training loop
        /// </summary>
        public virtual void Run()
        {
            //succes/fail results from last few sprints
            Queue<bool> result = new Queue<bool>();

            while (true)
            {
                Console.Clear();

                //Print board
                Console.WriteLine(this);

                //Check if the episode is finished
                if (Finished)
                {
                    episode++;
                    result.Enqueue(Succes);

                    while (result.Count > currentSuccesScale)
                    {
                        result.Dequeue();
                    }

                    succeses += Succes ? 1 : 0;
                    fails += Succes ? 0 : 1;

                    // Reset game back to start game
                    Reset();
                }

                // Print the current succes rate and episode count
                int count = result.Where(v => v == true).Count();
                int currentmax = Math.Min(episode, currentSuccesScale);
                double percentage = (currentmax > 0 ? ((count / (currentmax * 1.0)) * 100.0) : 0);
                Console.WriteLine($"Episode: {episode}, {succeses}/{fails} {percentage} {(Finished ? Succes ? "Succes" : "Fail" : "")}");

                // AI takes turn
                TakeTurn(episode);

                // Slow down game
                Thread.Sleep(50);
            }
        }

        /// <summary>
        /// AI takes turn in the game
        /// </summary>
        /// <param name="episode"></param>
        public abstract void TakeTurn(int episode);

        /// <summary>
        /// Move deterimened by policy
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="episode"></param>
        /// <returns></returns>
        protected abstract int DetermineMove(int x, int y, int episode);

        /// <summary>
        /// AI makes move
        /// </summary>
        /// <param name="move"></param>
        /// <returns>reward</returns>
        protected double MakeMove(int move)
        {
            // Change position depending on move
            switch (move)
            {
                case 0:
                    if (agentY > 0 && GetTile(agentX, agentY - 1) != TileType.Wall)
                    {
                        agentY--;
                    }
                    break;
                case 1:
                    if (agentY < height - 1 && GetTile(agentX, agentY + 1) != TileType.Wall)
                    {
                        agentY++;
                    }
                    break;
                case 2:
                    if (agentX > 0 && GetTile(agentX - 1, agentY) != TileType.Wall)
                    {
                        agentX--;
                    }
                    break;
                case 3:
                    if (agentX < width - 1 && GetTile(agentX + 1, agentY) != TileType.Wall)
                    {
                        agentX++;
                    }
                    break;
            }
            // Returns reward depending on which tile it landed
            switch (CurrentTile())
            {
                case TileType.Goal:
                    return 1;
                case TileType.Fail:
                    return -1;
                default:
                    return -0.1;

            }
        }

        /// <summary>
        /// Get tile for x and y
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        protected TileType GetTile(int x, int y)
        {
            return (TileType)level[x + y * width];
        }

        /// <summary>
        /// Gets current tile the AI is on
        /// </summary>
        /// <returns></returns>
        protected TileType CurrentTile()
        {
            return GetTile(agentX, agentY);
        }

        /// <summary>
        /// Resets the game
        /// </summary>
        public virtual void Reset()
        {
            agentX = spawnX;
            agentY = spawnY;
            Finished = false;
        }

        /// <summary>
        /// Checks if games is over ornot
        /// </summary>
        /// <param name="reward"></param>
        public void CheckGameState(double reward)
        {
            if (reward == 1 || reward == -1)
            {
                Finished = true;
                Succes = reward == 1;
            }
        }

        /// <summary>
        /// Prints the board
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string result = "";

            for (int x = 0; x < width + 2; x++)
            {
                Console.Write("-");
            }
            Console.WriteLine();


            for (int y = 0; y < height; y++)
            {
                Console.Write("|");

                for (int x = 0; x < width; x++)
                {
                    if (x == agentX && y == agentY)
                    {
                        Console.Write("X");
                    }
                    else
                    {
                        switch (GetTile(x, y))
                        {
                            case TileType.Goal:
                                Console.Write("G");
                                break;
                            case TileType.Fail:
                                Console.Write("F");
                                break;
                            case TileType.Empty:
                                Console.Write("=");
                                break;
                            case TileType.Wall:
                                Console.Write("#");
                                break;
                            default:
                                Console.Write("=");
                                break;

                        }
                    }
                }
                Console.WriteLine("|");
            }

            for (int x = 0; x < width + 2; x++)
            {
                Console.Write("-");
            }
            Console.WriteLine();


            return result;
        }
    }
}
