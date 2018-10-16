using LR_Test.ReinforcementLearning.NeuralNetwork;
using System;
using System.Linq;

namespace LR_Test.ReinforcementLearning.Algoritms.QLearning
{

    public class QLearningNeuralNetworkQuad
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
        private MyNeuralNetwork[] neuralNetworks;
        // private double[][] qtable; // Q value table

        private readonly double alpha; // Learning rate
        private readonly double epsilon; // Random chance of doing a random move
        private readonly double gamma; // Discount rate

        public bool Finished { get; private set; }
        public bool Succes { get; private set; }

        public QLearningNeuralNetworkQuad(int width, int height, int[] level, int spawnX, int spawnY, int goalx, int goaly, double alpha, double epsilon, double gamma)
        {

            this.random = new Random();

            this.level = level;
            this.width = width;
            this.height = height;
            this.spawnX = spawnX;
            this.spawnY = spawnY;
            this.goalX = goaly;
            this.goalY = goaly;

            this.neuralNetworks = new MyNeuralNetwork[4]
            {
                new MyNeuralNetwork(true, width * height,256,64, 1),
                new MyNeuralNetwork(true, width * height,256,64, 1),
                new MyNeuralNetwork(true, width * height,256,64, 1),
                new MyNeuralNetwork(true, width * height,256,64, 1),
            };
           
            this.alpha = alpha;
            this.epsilon = epsilon;
            this.gamma = gamma;
            this.agentX = 0;
            this.agentY = 2;
        }

        public QLearningNeuralNetworkQuad(double alpha, double epsilon, double gamma) : this(DEFAULT_WIDTH, DEFAULT_HEIGHT, DEFAULT_LEVEL, DEFAULT_SPAWNX, DEFAULT_SPAWNY, DEFAULT_GOALX, DEFAULT_GOALY, alpha, epsilon, gamma)
        {
        }

        public QLearningNeuralNetworkQuad(int width, int height, int[] level, int spawnX, int spawnY, int goalx, int goaly) : this(width, height, level, spawnX, spawnY, goalx, goaly, DEFAULT_ALPHA, DEFAULT_EPSILON, DEFAULT_GAMMA)
        {
        }

        public QLearningNeuralNetworkQuad() : this(DEFAULT_ALPHA, DEFAULT_EPSILON, DEFAULT_GAMMA)
        {
        }

        public void TakeTurn(int episode)
        {
            int agentx1 = agentX, agenty1 = agentY;

            var move = DetermineMove(agentX, agentY, episode);

            var qvals1 = QValues(agentX, agentY);
            var maxQ1 = HighestQValue(qvals1);

            var reward = MakeMove(move);

            var qvals2 = QValues(agentX, agentY);
            var maxQ2 = HighestQValue(qvals2);
            //int highestValueIndex = Array.IndexOf<double>(qvals2, maxQ2);

            var update = CalculateUpdatedQValue(qvals1[move], maxQ2, (float)reward);

            QValues(agentx1, agenty1);
          // Console.WriteLine($"Q-Values: {qvals1[0]}, {qvals1[1]}, {qvals1[2]}, {qvals1[3]}");
            qvals1[move] = update;
           // Console.WriteLine($"Q-Values: {qvals1[0]}, {qvals1[1]}, {qvals1[2]}, {qvals1[3]}");
            neuralNetworks[move].BackProp(qvals1[move]);
          //  Console.WriteLine($"UpdatedQValue: {update}");

            if (reward == 1 || reward == -1)
            {
                Finished = true;
                Succes = reward == 1;
            }
        }

        private int DetermineMove(int x, int y, int episode)
        {
            bool randomMove = random.NextDouble() < (epsilon);

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

        public double Distance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow(Math.Abs(x2 - x1), 2) + Math.Pow(Math.Abs(y2 - y1), 2));
        }

        private double MakeMove(int move)
        {
            var dist = Distance(agentX, agentY, goalX, goalY);
            bool moved = false;
            switch (move)
            {
                case 0:
                    if (agentY > 0 && GetTile(agentX, agentY - 1) != TileType.Wall)
                    {
                        agentY--;
                        moved = true;
                    }
                    break;
                case 1:
                    if (agentY < height - 1 && GetTile(agentX, agentY + 1) != TileType.Wall)
                    {
                        agentY++;
                        moved = true;
                    }
                    break;
                case 2:
                    if (agentX > 0 && GetTile(agentX - 1, agentY) != TileType.Wall)
                    {
                        agentX--;
                        moved = true;
                    }
                    break;
                case 3:
                    if (agentX < width - 1 && GetTile(agentX + 1, agentY) != TileType.Wall)
                    {
                        agentX++;
                        moved = true;
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
                    if (!moved)
                    {
                        return -.5;
                    }
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

        private float CalculateUpdatedQValue(double qvalue1, double qvalue2, float reward)
        {
            return (float)(qvalue1 + alpha * (reward + gamma * qvalue2 - qvalue1));
        }

        private double HighestQValue(int x, int y)
        {
            var qvalues = QValues(x, y);
            return qvalues.OrderByDescending(q => q).First();
        }

        private double HighestQValue(double[] qvalues)
        {
            return qvalues.OrderByDescending(q => q).First();
        }

        private double[] QValues(int x, int y)
        {
            double[] data = new double[level.Length];
            for (int i = 0; i < data.Length; i++)
            {
                if (i == x + y * width)
                {
                    data[i] = int.MaxValue;
                }
                else
                {
                    data[i] = level[i];
                }
            }

            double[] result = new double[4];
            for (int i = 0; i < neuralNetworks.Length; i++)
            {
                result[i] = neuralNetworks[i].FeedForward(data)[0];
            }

            return result;
        }

        private void SetQValuesForLocationAndMove(double[] value)
        {

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

            for (int x = 0; x < width + 2; x++)
            {
                result +="-";
            }
            result += '\n';

            for (int y = 0; y < height; y++)
            {
                result += '|';

                for (int x = 0; x < width; x++)
                {
                    if (x == agentX && y == agentY)
                    {
                        result += 'X';
                    }
                    else
                    {
                        switch (GetTile(x, y))
                        {
                            case TileType.Goal:
                                result += 'G';
                                break;
                            case TileType.Fail:
                                result += 'F';
                                break;
                            case TileType.Empty:
                                result += '=';
                                break;
                            case TileType.Wall:
                                result += '#';
                                break;
                            default:
                                result += '=';
                                break;

                        }
                    }
                }
                result += '|';
                result += '\n';
            }

            for (int x = 0; x < width + 2; x++)
            {
                result +="-";
            }

            return result;
        }
    }
}
