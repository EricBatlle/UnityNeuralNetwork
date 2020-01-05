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

    #region Serialize/Deserialize NeuralNetwork
    public static NeuralNetwork Deserialized(this SerializableNeuralNetwork sNet)
    {
        //Creation of NeuralNetwork using sNet layers to initialize structure and assign the direct-serialize fields
        NeuralNetwork net = new NeuralNetwork();
        net.layers = sNet.layers;
        net.fitness = sNet.fitness;

        //Deserialize Neurons from List<SerializableNueron>
        List<float[]> neuronsList = new List<float[]>();
        foreach (SerializableNeuron sNeuron in sNet.neurons)
        {
            neuronsList.Add(sNeuron.neuron);
        }
        net.neurons = neuronsList.ToArray();

        //Deserialize Weights from List<SerializableWeightsList>
        List<float[][]> weigthsList = new List<float[][]>();
        foreach (SerializableWeigthList sWeightList in sNet.weigths)
        {
            List<float[]> layerWeigthsList = new List<float[]>();
            foreach (SerializableWeigth sWeight in sWeightList.weightList)
            {
                layerWeigthsList.Add(sWeight.weigth);
            }
            weigthsList.Add(layerWeigthsList.ToArray());            
        }
        net.weights = weigthsList.ToArray();
        
        return new NeuralNetwork(net);
    }

    /// <summary>
    /// Save agent neural network information into JSON file
    /// </summary>
    public static void SaveNeuralNetwork(this NeuralNetwork net)
    {
        string jsonString = JsonManager.SerializeToJson<SerializableNeuralNetwork>(net.Serialized());
        JsonManager.WriteJSONFile("NeuralNetworkJSON", jsonString);
    }

    /// <summary>
    /// Extension method who returns the serialized neural network
    /// </summary>
    /// <param name="net">Neural network to serialize</param>
    /// <returns></returns>
    public static SerializableNeuralNetwork Serialized(this NeuralNetwork net)
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

        //Original Weigths

        return sNeuralNetwork;        
    }
    #endregion
}
