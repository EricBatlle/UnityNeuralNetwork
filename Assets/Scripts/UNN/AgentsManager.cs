using System;
using System.Collections.Generic;
using UnityEngine;
using static NeuralNetworkSerializer;

/// <summary>
/// Manager to control agents generations and training
/// </summary>
public abstract class AgentsManager : MonoBehaviour
{
    [Header("Manager")]
    public bool trainOnStart = true;   //flag to start the train when the scene plays without button    
    [SerializeField] protected GameObject agentPrefab;
    [Space()]
    [SerializeField][Range(0f, 20f)]
    protected float newTimeScale = 10f; //Increase Scale time speed all processes, increase it to train faster the agents
    public float NewTimeScale
    {
        get { return newTimeScale; }
        set { newTimeScale = value; Time.timeScale = newTimeScale; }
    }
    [SerializeField] protected float newGenerationRateTime = 15f;   //Time to wait until make a new generation
    [DisplayWithoutEdit][SerializeField] protected int currentGenerationNumber = 0;
    [Space()]
    [Tooltip("Is recommended to have an even number of agents")]
    [SerializeField] protected int populationSize = 50;                     //...to speed training cause training use the half of the population(population/2)
    [SerializeField] protected int[] layers = new int[] { 1, 10, 10, 1 };   //1 input and 1 output
    [Header("Neural Network to Load")]
    public TextAsset neuralNetworkToLoad = null;
    public bool createNetxGenerationWithThisNet = false;

    protected List<NeuralNetwork> agentsNets = null;
    protected List<Agent> agentsList = null;        
    protected GameObject agentsParentGO;                    //Used as agents parent on Unity's hierarchy
    [HideInInspector] public bool isTraining = false;       //True when the training process is on
    [HideInInspector] public bool isTrainingStoped = false; //True when the training process is on
    private Coroutine trainingCoroutine = null;

    public Action OnStartTrainAgents = null;

    /// <summary>
    /// This is needed cause properties aren't called by UnityEditor changes
    /// </summary>
    private void OnValidate()
    {
        NewTimeScale = NewTimeScale;
    }

    /// <summary>
    /// Setter for Manager layers, to allow more ways of setting it, not only Editor
    /// </summary>
    /// <param name="newLayers"></param>
    protected virtual int[] SetLayers()
    {
        return this.layers;
    }

    /// <summary>
    /// Start training when loading the scene
    /// </summary>
    private void Start()
    {
        if (trainOnStart)
            StartTraining();
    }    

    /// <summary>
    /// Start the training process every <c>newGenerationRateTime</c> and creates agent's parent Game Object
    /// </summary>
    public void StartTraining()
    {
        isTraining = true;
        isTrainingStoped = false;

        this.layers = SetLayers();

        CreateAgentsParentGO();
        trainingCoroutine = this.InvokeRepeatingAndGetCoroutine(TrainAgents, newGenerationRateTime);
    }

    /// <summary>
    /// Stop training process
    /// </summary>
    public void StopTraining()
    {
        isTraining = false;
        isTrainingStoped = true;
        StopCoroutine(trainingCoroutine);
    }

    /// <summary>
    /// Reset training process from 0
    /// </summary>
    public void ResetTraining()
    {
        this.StopTraining();

        //Clear variables
        currentGenerationNumber = 0;
        agentsNets = new List<NeuralNetwork>();
        agentsList = new List<Agent>();
        if (agentsParentGO != null)
            DestroyImmediate(agentsParentGO);

        this.StartTraining();
    }    
   
    /// <summary>
    /// Create agents GameObjects and initialize agents neural networks.
    /// Mutate the worst half and reset fitness values.
    /// </summary>
    private void TrainAgents ()
    {
        OnStartTrainAgents?.Invoke();
        //It it is the first generation, instantiate all neural networks
        if (currentGenerationNumber == 0)
        {
            //Create the list with the different neural networks for the Agents
            InitAgentsNeuralNetworks();
        }
        else
        {
            //Ascending Sort (lower fitness on the first positions)
            agentsNets.Sort();
                
            for (int i = 0; i < (populationSize / 2); i++)
            {
                //For the first half of the population (the dumbest):
                //1. Assign them the best half of the neural networks
                agentsNets[i] = new NeuralNetwork(agentsNets[i + (populationSize / 2)]);
                //2. Mutate their neural network
                agentsNets[i].Mutate();

                //too lazy to write a reset neuron matrix for the second half of the population (the smartest) values method....so just going to make a deepcopy
                agentsNets[i + (populationSize / 2)] = new NeuralNetwork(agentsNets[i + (populationSize / 2)]);
            }

            //Reset all fitness values
            for (int i = 0; i < populationSize; i++)
            {
                agentsNets[i].SetFitness(0f);
            }
        }
           
        currentGenerationNumber++;        
        CreateAllAgentsGO();        
    }

    /// <summary>
    /// Destroy previous game object agents and create new ones to add to agentsList
    /// </summary>
    private void CreateAllAgentsGO()
    {
        //Destroy previous Agents
        DestroyAllAgentsGO();

        //Create new ones
        agentsList = new List<Agent>();

        //Check if the creation of agents starts from scratch or loading a pre-existant neural network
        bool createAgentsFromExternalNeuralNetwork = (neuralNetworkToLoad != null && createNetxGenerationWithThisNet) ? true : false;        
        if (neuralNetworkToLoad == null && createNetxGenerationWithThisNet)
            Debug.Log("Set Neural Network to load");
        
        //Create the agents Game Objects
        if (createAgentsFromExternalNeuralNetwork)
        {
            Debug.Log("Created loaded generation");
            for (int i = 0; i < populationSize; i++)
            {
                agentsList.Add(CreateAgentGO(i, neuralNetworkToLoad));
            }
            createNetxGenerationWithThisNet = false;
        }
        else
        {
            for (int i = 0; i < populationSize; i++)
            {
                agentsList.Add(CreateAgentGO(i));
            }
        }    
    }

    /// <summary>
    /// Create agent Game Object and set his parent.
    /// Also set his neural network and the inputs it will use.
    /// </summary>
    /// <param name="agentNumber">Position of the agent in the current agents list</param>
    /// <returns></returns>
    protected virtual Agent CreateAgentGO(int agentNumber)
    {
        Agent agent = Instantiate(agentPrefab).GetComponent<Agent>();
        agent.transform.SetParent(agentsParentGO.transform);
        agent.Init(agentsNets[agentNumber]);
        return agent;
    }

    /// <summary>
    /// Create agent Game Object and set his parent.
    /// Also set his neural network and the inputs it will use.
    /// The neural network is initially setted from an external neural network file.
    /// </summary>
    /// <param name="agentNumber"></param>
    /// <param name="neuralNetworkToLoad"></param>
    /// <returns></returns>
    protected virtual Agent CreateAgentGO(int agentNumber, TextAsset neuralNetworkToLoad)
    {
        Agent agent = Instantiate(agentPrefab).GetComponent<Agent>();
        agent.transform.SetParent(agentsParentGO.transform);
        agent.Init(NeuralNetwork.LoadFromTextAsset(neuralNetworkToLoad));
        return agent;
    }

    /// <summary>
    /// This method allows the manager to create an Agent which is excluded of the manager training cycle.
    /// It can be created with an external Neural Network or pick the smartest of the current cycle.
    /// </summary>
    /// <param name="neuralNetworkToLoad">Serialized neural network to load into the new agent</param>
    public virtual void CreateSeparatedAgentGO()
    {
        if (neuralNetworkToLoad == null)
            print("There is no Neural Network TextAsset attached");
        else
        {
            Agent agent = Instantiate(agentPrefab).GetComponent<Agent>();
            string jsonString = JsonManager.ReadJSONFile(neuralNetworkToLoad);
            SerializableNeuralNetwork sNet = JsonManager.DeserializeFromJson<SerializableNeuralNetwork>(jsonString);
            agent.Init(sNet.Deserialized());
        }
    }

    /// <summary>
    /// Destroy all agents game objects
    /// </summary>
    private void DestroyAllAgentsGO()
    {
        if (agentsList != null)
        {
            for (int i = 0; i < agentsList.Count; i++)
            {
                GameObject.Destroy(agentsList[i].gameObject);
            }
        }
    }

    /// <summary>
    /// Initialize agents neural networks    
    /// </summary>
    private void InitAgentsNeuralNetworks()
    {        
        agentsNets = new List<NeuralNetwork>();        

        for (int i = 0; i < populationSize; i++)
        {
            NeuralNetwork net = new NeuralNetwork(layers);
            net.Mutate();
            agentsNets.Add(net);
        }
    }

    /// <summary>
    /// Create parent Game Object for all agents
    /// </summary>
    private void CreateAgentsParentGO()
    {
        agentsParentGO = new GameObject();
        agentsParentGO.name = "Agents";
    }    
}