using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Class used to serialize NeuralNetwork.
/// Serializes NeuralNetwork to SerializableNeuralNetwork struct so it can be serialized.
/// </summary>
public static class NeuralNetworkSerializer
{
    #region Structs
    //Neural Network
    [Serializable]
    public struct SerializableNeuralNetwork
    {
        public int[] layers;                        //layers
        public List<SerializableNeuron> neurons;    //neuron matix    
        public List<SerializableWeigthList> weigths;//weight matrix        
        public float fitness;                       //fitness of the network
    }

    //Neurons
    [Serializable]
    public struct SerializableNeuron
    {
        public float[] neuron;
    }
    
    //Weigths
    [Serializable]
    public struct SerializableWeigthList
    {
        public List<SerializableWeigth> weightList;
    }
    [Serializable]
    public struct SerializableWeigth
    {
        public float[] weigth;
    }
    #endregion

    /// <summary>
    /// Extension method who returns the serialized neural network
    /// </summary>
    /// <param name="net">Neural network to serialize</param>
    /// <returns></returns>
    public static SerializableNeuralNetwork Serializable(this NeuralNetwork net)
    {        
        //Creation of SerializableNeuralNetwork and assign the direct-serialize fields
        SerializableNeuralNetwork sNeuralNetwork;
        sNeuralNetwork.layers = net.layers;
        sNeuralNetwork.fitness = net.fitness;

        //Serialize neurons from float[][]
        sNeuralNetwork.neurons = new List<SerializableNeuron>();
        List<float[]> auxNeuronList = net.neurons.ToList();
        foreach (float[] neuron in auxNeuronList)
        {
            SerializableNeuron sNeuron;
            sNeuron.neuron = neuron;
            sNeuralNetwork.neurons.Add(sNeuron);
        }

        //Serialize weights from float[][][]
        sNeuralNetwork.weigths = new List<SerializableWeigthList>();
        List<float[][]> auxWeigthList = net.weights.ToList();
        foreach (float[][] weigthList in auxWeigthList)
        {
            SerializableWeigthList sWeigthList;
            sWeigthList.weightList = new List<SerializableWeigth>();

            List<float[]> auxWeigth = weigthList.ToList();
            foreach (float[] weigth in auxWeigth)
            {
                SerializableWeigth sWeigth;
                sWeigth.weigth = weigth;
                sWeigthList.weightList.Add(sWeigth);
            }
            sNeuralNetwork.weigths.Add(sWeigthList);
        }

        return sNeuralNetwork;        
    }
}
