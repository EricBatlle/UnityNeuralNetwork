using System.Collections.Generic;
using System;
using UnityEngine;
using System.Runtime.Serialization;
using static NeuralNetworkSerializer;

/// <summary>
/// Neural Network (Genetic, Unsupervised)
/// </summary>
[Serializable]
public class NeuralNetwork : IComparable<NeuralNetwork>
{
    [DisplayWithoutEdit]public int[] layers;    //layers
    public float[][] neurons;                   //neuron matix
    public float[][][] weights;                 //weight matrix
    public float fitness;                       //fitness of the network

    /// <summary>
    /// Default empty constructor used mainly when loading different neuralNetworks
    /// </summary>
    public NeuralNetwork()
    {
    }

    /// <summary>
    /// Deep copy constructor 
    /// </summary>
    /// <param name="copyNetwork">Network to deep copy</param>
    public NeuralNetwork(NeuralNetwork copyNetwork)
    {
        //Deep copy of layers of this network 
        this.layers = new int[copyNetwork.layers.Length];
        for (int i = 0; i < copyNetwork.layers.Length; i++)
        {
            this.layers[i] = copyNetwork.layers[i];
        }

        //Generate matrixs
        InitNeurons();
        InitWeights();

        CopyWeights(copyNetwork.weights);
    }

    /// <summary>
    /// Initilizes neural network with random weights
    /// </summary>
    /// <param name="layers">layers to the neural network</param>
    public NeuralNetwork(int[] layers)
    {
        //Deep copy of layers of this network 
        this.layers = new int[layers.Length];
        for (int i = 0; i < layers.Length; i++)
        {
            this.layers[i] = layers[i];
        }

        //Generate matrixs
        InitNeurons();
        InitWeights();
    }

    /// <summary>
    /// Copy weights into the current neural network
    /// </summary>
    /// <param name="copyWeights">weights to copy</param>
    private void CopyWeights(float[][][] copyWeights)
    {
        for (int i = 0; i < weights.Length; i++)
        {
            for (int j = 0; j < weights[i].Length; j++)
            {
                for (int k = 0; k < weights[i][j].Length; k++)
                {
                    weights[i][j][k] = copyWeights[i][j][k];
                }
            }
        }
    }    

    /// <summary>
    /// Create neuron matrix
    /// </summary>
    private void InitNeurons()
    {
        //Neuron matrix Initilization
        List<float[]> neuronsList = new List<float[]>();

        //Add all layers neurons to the neuronList
        for (int i = 0; i < layers.Length; i++)        
            neuronsList.Add(new float[layers[i]]);        

        neurons = neuronsList.ToArray();
    }

    /// <summary>
    /// Create weights matrix.
    /// </summary>
    private void InitWeights()
    {
        //Weights list which will later will converted into a weights 3D array
        List<float[][]> weightsList = new List<float[][]>(); 

        //Itterate over all neurons that have a weight connection
        for (int i = 1; i < layers.Length; i++)
        {
            //Layer weight list for this current layer (will be converted to 2D array)
            List<float[]> layerWeightsList = new List<float[]>(); 

            int neuronsInPreviousLayer = layers[i - 1]; 

            //Itterate over all neurons in this current layer
            for (int j = 0; j < neurons[i].Length; j++)
            {
                float[] neuronWeights = new float[neuronsInPreviousLayer];

                //Itterate over all neurons in the previous layer and set the weights randomly between 0.5f and -0.5
                for (int k = 0; k < neuronsInPreviousLayer; k++)
                {
                    //Give random weights to neuron weights
                    neuronWeights[k] = UnityEngine.Random.Range(-0.5f,0.5f);
                }

                //Add neuron weights of this current layer to layer weights
                layerWeightsList.Add(neuronWeights); 
            }

            //Add this layers weights converted into 2D array into weights list
            weightsList.Add(layerWeightsList.ToArray()); 
        }
        //Convert to 3D array
        weights = weightsList.ToArray(); 
    }

    /// <summary>
    /// Feed forward this neural network with a given input array
    /// </summary>
    /// <param name="inputs">Inputs to network</param>
    /// <returns></returns>
    public float[] FeedForward(float[] inputs)
    {
        //Add inputs to the neuron matrix
        for (int i = 0; i < inputs.Length; i++)
        {
            neurons[0][i] = inputs[i];
        }

        //Itterate over all neurons and compute feedforward values 
        for (int i = 1; i < layers.Length; i++)
        {
            for (int j = 0; j < neurons[i].Length; j++)
            {
                float value = 0f;

                for (int k = 0; k < neurons[i-1].Length; k++)
                {
                    //Sum off all weights connections of this neuron weights for their values in previous layer
                    value += weights[i - 1][j][k] * neurons[i - 1][k]; 
                }

                //Hyperbolic tangent activation
                neurons[i][j] = (float)Math.Tanh(value); 
            }
        }

        //Return output layer
        return neurons[neurons.Length-1]; 
    }

    /// <summary>
    /// Mutate neural network weights
    /// </summary>
    public void Mutate()
    {
        for (int i = 0; i < weights.Length; i++)
        {
            for (int j = 0; j < weights[i].Length; j++)
            {
                for (int k = 0; k < weights[i][j].Length; k++)
                {
                    float weight = weights[i][j][k];

                    //Mutate weight value 
                    float randomNumber = UnityEngine.Random.Range(0f,100f);

                    //Apply ONE of those 4 mutations
                    if (randomNumber <= 2f)
                    { 
                        //Flip sign of weight
                        weight *= -1f;
                    }
                    else if (randomNumber <= 4f)
                    { 
                        //Pick random weight between -1 and 1
                        weight = UnityEngine.Random.Range(-0.5f, 0.5f);
                    }
                    else if (randomNumber <= 6f)
                    { 
                        //Randomly increase by 0% to 100%
                        float factor = UnityEngine.Random.Range(0f, 1f) + 1f;
                        weight *= factor;
                    }
                    else if (randomNumber <= 8f)
                    { 
                        //Randomly decrease by 0% to 100%
                        float factor = UnityEngine.Random.Range(0f, 1f);
                        weight *= factor;
                    }

                    weights[i][j][k] = weight;
                }
            }
        }
    }

    #region Fitness Getters/Setters
    public void AddFitness(float fit)
    {
        fitness += fit;
    }
    public void SetFitness(float fit)
    {
        fitness = fit;
    }
    public float GetFitness()
    {
        return fitness;
    }
    #endregion

    #region Save/Load neural network
    /// <summary>
    /// Save Neural Network serialized information to JSON file
    /// </summary>
    public void SaveNeuralNetworkToJSON()
    {
        string jsonString = JsonManager.SerializeToJson<SerializableNeuralNetwork>(this.Serialized());
        JsonManager.WriteJSONFile("NeuralNetworkJSON", jsonString);
    }
    /// <summary>
    /// Static saving Neural Network serialized information to JSON file
    /// </summary>
    public static void SaveNeuralNetworkToJSON(NeuralNetwork net)
    {
        string jsonString = JsonManager.SerializeToJson<SerializableNeuralNetwork>(net.Serialized());
        JsonManager.WriteJSONFile("NeuralNetworkJSON", jsonString);
    }

    /// <summary>
    /// Static load of a serialized NeuralNetwork from TextAsset
    /// </summary>
    /// <param name="neuralNetworkTextAsset">TextAsset from which information is read</param>
    /// <returns></returns>
    public static NeuralNetwork LoadFromTextAsset(TextAsset neuralNetworkTextAsset)
    {
        if (neuralNetworkTextAsset == null)
            Debug.Log("There is no Neural Network TextAsset attached");
        else
        {
            string jsonString = JsonManager.ReadJSONFile(neuralNetworkTextAsset);
            SerializableNeuralNetwork sNet = JsonManager.DeserializeFromJson<SerializableNeuralNetwork>(jsonString);
            return sNet.Deserialized();
        }
        return null;
    }
    #endregion

    /// <summary>
    /// Compare two neural networks and sort (ASCENDING) based on fitness
    /// </summary>
    /// <param name="other">Network to be compared to</param>
    /// <returns></returns>
    public int CompareTo(NeuralNetwork other)
    {                
        if (other == null) return 1;

        if (fitness > other.fitness)
            return 1;
        else if (fitness < other.fitness)
            return -1;
        else
            return 0;        
    }    
}
