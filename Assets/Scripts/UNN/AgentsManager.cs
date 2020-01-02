using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manager to control agents generations and training
/// </summary>
public abstract class AgentsManager : MonoBehaviour
{
    [SerializeField] protected GameObject agentPrefab;
    [Space()]
    [SerializeField] protected float newGenerationRateTime = 15f;//Time to wait until make a new generation
    [Tooltip("Is recommended to have an even number of agents")]
    [SerializeField] protected int populationSize = 50;         //...to speed training cause training use the half of the population(population/2)
    [SerializeField] protected int currentGenerationNumber = 0;
    [SerializeField] protected int[] layers = new int[] { 1, 10, 10, 1 }; //1 input and 1 output
    [Space()]
    [SerializeField] protected List<NeuralNetwork> agentsNets;
    [SerializeField] protected List<Agent> agentsList = null;

    protected GameObject agentsParentGO; //Used as agents parent on Unity's hierarchy

    /// <summary>
    /// Start the training process every <c>newGenerationRateTime</c> and creates agent's parent Game Object
    /// </summary>
    private void Start()
    {
        CreateAgentsParentGO();
        this.InvokeRepeating(TrainAgents, newGenerationRateTime);
    }

    /// <summary>
    /// Create agents GameObjects and initialize agents neural networks.
    /// Mutate the worst half and reset fitness values.
    /// </summary>
    private void TrainAgents ()
    {
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
                
            for (int i = 0; i < populationSize / 2; i++)
            {
                //For the first half of the population (the dumbest), mutate their neural network
                agentsNets[i] = new NeuralNetwork(agentsNets[i+(populationSize / 2)]);
                agentsNets[i].Mutate();

                //too lazy to write a reset neuron matrix values method....so just going to make a deepcopy
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
        for (int i = 0; i < populationSize; i++)        
            agentsList.Add(CreateAgentGO(i));                    
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