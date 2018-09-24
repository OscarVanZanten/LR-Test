using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace LR_Test.ReinforcementLearning.Algoritms.QLearning
{
    public enum TileType { Empty, Wall, Goal, Fail }

    public class Tabulair
    {
        private Random random;

        private const int WIDTH = 4;
        private const int HEIGHT = 3;

        private const int SPAWNX = 0, SPAWNY = 2;

        private int agentX, agentY; // Agent location
        private int[] level; // level map 
        private double[][] qtable; // Q value table

        private double alpha; // Learning rate
        private double epsilon; // Random chance of doing a random move
        private double gamma; // Discount rate

        public bool Finished { get; private set; }

        public Tabulair(double alpha, double epsilon, double gamma)
        {
            this.random = new Random();

            this.level = new int[WIDTH * HEIGHT] 
            {
                0,0,0,2,
                0,1,0,3,
                0,0,0,0
            };

            this.qtable = new double[WIDTH * HEIGHT][];
            this.alpha = alpha;
            this.epsilon = epsilon;
            this.gamma = gamma;
            this.agentX = 0;
            this.agentY = 2;
        }

        public void TakeTurn()
        {
            int agentx1 = agentX;
            int agenty1 = agentY;

            var move = DetermineMove(agentX, agentY);

            Console.WriteLine($"Move: {move}");

            var reward = MakeMove(move);

            var qvalue1 = QValuesForLocationAndMove(agentx1, agenty1, move);

            var qvalue2 = HighestQValue(agentX, agentY);

            var updatedQValue = CalculateUpdatedQValue(qvalue1, qvalue2, reward);

            SetQValuesForLocationAndMove(agentx1, agenty1, move, updatedQValue);
            Console.WriteLine($"UpdatedQValue: {updatedQValue}");

            if (reward == 1 || reward == -1)
            {
                Finished = true;
            }
        }

        private int DetermineMove(int x, int y)
        {
            bool randomMove = random.NextDouble() < epsilon;

            if (randomMove)
            {
                return random.Next(0, 4);
            }
            else
            {
                var qvalues = QValuesForLocation(x, y);
                double highestValue = HighestQValue(x, y);
                int highestValueIndex = Array.IndexOf<double>(qvalues, highestValue);

                if (highestValue == 0)
                {
                    return random.Next(0, 4);
                }

                return highestValueIndex;
            }
        }

        private double HighestQValue(int x, int y)
        {
            var qvalues = QValuesForLocation(x, y);
            return qvalues.OrderByDescending(q => q).First();
        }

        private double MakeMove(int move)
        {
            switch (move)
            {
                case 0:
                    if (agentY > 0 && GetTile(agentX, agentY - 1) != TileType.Wall)
                    {
                        agentY--;
                    }
                    break;
                case 1:
                    if (agentY < HEIGHT - 1 && GetTile(agentX , agentY + 1) != TileType.Wall)
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
                    if (agentX < WIDTH - 1 && GetTile(agentX+1, agentY) != TileType.Wall)
                    {
                        agentX++;
                    }
                    break;
            }
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

        private TileType GetTile(int x, int y)
        {
            return (TileType)level[x + y * WIDTH];
        }

        public TileType CurrentTile()
        {
            return GetTile(agentX, agentY);
        }

        private double CalculateUpdatedQValue(double qvalue1, double qvalue2, double reward)
        {
            return qvalue1 + alpha * (reward + gamma * qvalue2 - qvalue1);
        }

        private double[] QValuesForLocation(int x, int y)
        {
            if (qtable[x + y * WIDTH] == null)
            {
                qtable[x + y * WIDTH] = new double[4];
            }
            return qtable[x + y * WIDTH];
        }

        private double QValuesForLocationAndMove(int x, int y, int move)
        {
            return qtable[x + y * WIDTH][move];
        }

        private void SetQValuesForLocationAndMove(int x, int y, int move, double value)
        {
            qtable[x + y * WIDTH][move] = value;
        }

        public void Reset()
        {
            agentX = SPAWNX;
            agentY = SPAWNY;
            Finished = false;
        }

        public override string ToString()
        {
            string result = "";

            Console.WriteLine("--------------");

                for (int y = 0; y < HEIGHT; y++)
            {
                Console.Write("|");

            for (int x = 0; x < WIDTH; x++)
                {
                    if (x == agentX && y == agentY)
                    {
                        Console.Write("[X]");
                    }
                    else
                    {
                        switch (GetTile(x,y))
                        {
                            case TileType.Goal:
                                Console.Write(" G ");
                                break;
                            case TileType.Fail:
                                Console.Write(" F ");
                                break;
                            case TileType.Empty:
                                Console.Write("   ");
                                break;
                            case TileType.Wall:
                                Console.Write(" W ");
                                break;
                            default:
                                Console.Write("   ");
                                break;

                        }
                    }
                }
                Console.WriteLine("|");
            }

            Console.WriteLine("--------------");

            return result;
        }
    }
}
