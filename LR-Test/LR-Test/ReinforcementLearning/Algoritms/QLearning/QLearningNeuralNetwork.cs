using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using LR_Test.ReinforcementLearning.NeuralNetwork;
using LR_Test.Util;

namespace LR_Test.ReinforcementLearning.Algoritms.QLearning
{

    public class QLearningNeuralNetwork
    {
        private Random random;

        private const double DEFAULT_ALPHA = 0.2;
        private const double DEFAULT_EPSILON = 0.4;
        private const double DEFAULT_GAMMA = 0.9;

        private const int DEFAULT_WIDTH = 4;
        private const int DEFAULT_HEIGHT = 3;
        private const int DEFAULT_SPAWNX = 0;
        private const int DEFAULT_SPAWNY = 2;
        private const int DEFAULT_GOALX = 2;
        private const int DEFAULT_GOALY = 0;

        private static readonly int[] DEFAULT_LEVEL = new int[DEFAULT_WIDTH * DEFAULT_HEIGHT]
            {
                0,0,0,2,
                0,1,0,3,
                0,0,0,0
            };

        private readonly int width;
        private readonly int height;
        private readonly int spawnX = 0;
        private readonly int spawnY = 2;
        private readonly int goalX = 0;
        private readonly int goalY = 0;
        private int agentX, agentY; // Agent location
        private readonly int[] level; // level map 
        private NeuralNetwork.NeuralNetwork neuralNetwork;
        // private double[][] qtable; // Q value table

        private readonly double alpha; // Learning rate
        private readonly double epsilon; // Random chance of doing a random move
        private readonly double gamma; // Discount rate

        public bool Finished { get; private set; }

        public QLearningNeuralNetwork(int width, int height, int[] level, int spawnX, int spawnY, int goalx, int goaly, double alpha, double epsilon, double gamma)
        {

            this.random = new Random();

            this.level = level;
            this.width = width;
            this.height = height;
            this.spawnX = spawnX;
            this.spawnY = spawnY;
            this.goalX = goaly;
            this.goalY = goaly;

            this.neuralNetwork = NeuralNetworkBuilder.GenerateNeuralNetwork(NeuralNetworkBuilder.GenerateEmptyNeuralNetworkData(8, 2, 1), 8,2, 1);
            this.alpha = alpha;
            this.epsilon = epsilon;
            this.gamma = gamma;
            this.agentX = 0;
            this.agentY = 2;
        }

        public QLearningNeuralNetwork(double alpha, double epsilon, double gamma) : this(DEFAULT_WIDTH, DEFAULT_HEIGHT, DEFAULT_LEVEL, DEFAULT_SPAWNX, DEFAULT_SPAWNY, DEFAULT_GOALX, DEFAULT_GOALY, alpha, epsilon, gamma)
        {
        }

        public QLearningNeuralNetwork(int width, int height, int[] level, int spawnX, int spawnY, int goalx, int goaly) : this(width, height, level, spawnX, spawnY, goalx, goaly, DEFAULT_ALPHA, DEFAULT_EPSILON, DEFAULT_GAMMA)
        {
        }

        public QLearningNeuralNetwork() : this(DEFAULT_ALPHA, DEFAULT_EPSILON, DEFAULT_GAMMA)
        {
        }

        public void TakeTurn(int episode)
        {
            int agentx1 = agentX;
            int agenty1 = agentY;

            var move = DetermineMove(agentX, agentY, episode);

            Console.WriteLine($"Move: {move}");

            var reward = MakeMove(move);

            var qvalue1 = QValue(agentx1, agenty1, move);

            var qvalue2 = HighestQValue(agentX, agentY);

            var updatedQValue = CalculateUpdatedQValue(qvalue1, qvalue2, reward);

            var values = QValuesForLocation(agentX, agentY);

            Console.WriteLine($"Q-Values: {values[0]}, {values[1]}, {values[2]}, {values[3]}");
            Console.WriteLine($"Values: {qvalue1}, {qvalue2}");
            SetQValuesForLocationAndMove(updatedQValue);
            Console.WriteLine($"UpdatedQValue: {updatedQValue}");

            if (reward == 1 || reward == -1)
            {
                Finished = true;
            }
        }

        private int DetermineMove(int x, int y, int episode)
        {
            bool randomMove = random.NextDouble() < (epsilon - (episode/100));

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
            return (TileType)level[x + y * width];
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
            double[] qvalues = new double[4];

            for (int move = 0; move < 4; move++)
            {
                qvalues[move] = QValue(x, y, move);
            }
          
            return qvalues;
        }

        private double HighestQValue(int x, int y)
        {
            var qvalues = QValuesForLocation(x, y);
            return qvalues.OrderByDescending(q => q).First();
        }

        private double QValue(int x, int y, int move)
        {
            return neuralNetwork.Execute(MathHelper.Sigmoid(x), MathHelper.Sigmoid(y), (move != 0 ? 0 : 1), (move != 1 ? 0 : 1), (move != 2 ? 0 : 1), (move != 3 ? 0 : 1), MathHelper.Sigmoid(goalX), MathHelper.Sigmoid(goalY))[0];
        }

        private void SetQValuesForLocationAndMove(double value)
        {
            neuralNetwork.BackPropagation( value);
        }

        public void Reset()
        {
            agentX = spawnX;
            agentY = spawnY;
            Finished = false;
        }

        public override string ToString()
        {
            string result = "";

            for (int x = 0; x < width +2; x++)
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

            for (int x = 0; x < width+2; x++)
            {
                Console.Write("-");
            }
            Console.WriteLine();

            Console.WriteLine(neuralNetwork);

            return result;
        }
    }
}
