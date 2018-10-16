using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Threading;

namespace LR_Test.ReinforcementLearning.NeuralNetwork
{
    public class MyNeuralNetwork
    {
        private Layer[] layers;

        public MyNeuralNetwork(bool randomWeights, params int[] format)
        {
            this.layers = new Layer[format.Length - 1];
            CreateLayers(format, randomWeights);
        }

        private void CreateLayers(int[] format, bool randomWeights)
        {
            for (int i = 0; i < layers.Length; i++)
            {
                layers[i] = new Layer(format[i], format[i + 1], randomWeights);
            }
        }

        public double[] FeedForward(params double[] inputs)
        {
            double[] output = layers[0].FeedForward(inputs);

            for (int i = 1; i < layers.Length; i++)
            {
                output = layers[i].FeedForward(output);
            }

            return output; //return output of last layer
        }

        public void BackProp(params double[] expected)
        {
            Layer previous = null;
            // run over all layers backwards
            for (int i = layers.Length - 1; i >= 0; i--)
            {
                if (i == layers.Length - 1)
                {
                    previous = layers[i].BackPropOutput(expected); //back prop output
                }
                else
                {
                    layers[i].BackPropHidden(previous); //back prop hidden
                }
            }

            //Update weights
            for (int i = 0; i < layers.Length; i++)
            {
                layers[i].UpdateWeights();
            }
        }

    }
}
