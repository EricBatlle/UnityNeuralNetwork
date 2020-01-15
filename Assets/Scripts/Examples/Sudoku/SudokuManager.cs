using UnityEngine;
using static NeuralNetworkSerializer;
/// <summary>
/// Derived class from AgentsManager.
/// Implements the creation of SimpleAgent.
/// </summary>
public class SudokuManager : AgentsManager
{
    [Header("Sudoku")]
    public GameObject rewardGO = null;

    //Override to create SimpleAgent agents
    protected override Agent CreateAgentGO(int agentNumber)
    {
        Agent agent = Instantiate(agentPrefab).GetComponent<Agent>();
        agent.gameObject.name = "SudokuAgent";
        agent.transform.SetParent(agentsParentGO.transform);
        agent.Init(agentsNets[agentNumber], rewardGO);
        return agent;
    }
    //Override to create SimpleAgent agents
    protected override Agent CreateAgentGO(int agentNumber, TextAsset neuralNetworkToLoad)
    {
        Agent agent = Instantiate(agentPrefab).GetComponent<Agent>();
        agent.gameObject.name = "SudokuAgent";
        agent.transform.SetParent(agentsParentGO.transform);
        agent.Init(NeuralNetwork.LoadFromTextAsset(neuralNetworkToLoad), rewardGO);
        return agent;
    }

    //Override to pass rewardGameObject information to separated Agents
    public override void CreateSeparatedAgentGO()
    {
        if (neuralNetworkToLoad == null)
            print("There is no Neural Network TextAsset attached");
        else
        {
            Agent agent = Instantiate(agentPrefab).GetComponent<Agent>();
            agent.gameObject.name = "Separated_SudokuAgent";
            string jsonString = JsonManager.ReadJSONFile(neuralNetworkToLoad);
            SerializableNeuralNetwork sNet = JsonManager.DeserializeFromJson<SerializableNeuralNetwork>(jsonString);
            agent.Init(sNet.Deserialized(), rewardGO);
        }
    }
}