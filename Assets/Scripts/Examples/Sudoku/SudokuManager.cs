using UnityEngine;
using static NeuralNetworkSerializer;
/// <summary>
/// Derived class from AgentsManager.
/// Implements the creation of SimpleAgent.
/// </summary>
public class SudokuManager : AgentsManager
{
    [Header("Sudoku")]
    [SerializeField] private SudokuGenerator sudokuGenerator = null;
    [HideInInspector] public SudokuController sudokuController = null;

    private void Awake()
    {
        OnStartTrainAgents = () => { sudokuGenerator.sudokuCount = 0; };
    }

    #region AgentsManager
    protected override int[] SetLayers()
    {
        return new int[] { (sudokuGenerator.cellsInSquare * sudokuGenerator.cellsInSquare), 10, 10, 2 };
    }
    //Override to create SimpleAgent agents
    protected override Agent CreateAgentGO(int agentNumber)
    {
        CreateSudokuForTheAgent();
        Agent agent = Instantiate(agentPrefab).GetComponent<Agent>();
        agent.gameObject.name = "SudokuAgent";
        agent.transform.SetParent(agentsParentGO.transform);
        agent.Init(agentsNets[agentNumber], sudokuController);
        return agent;
    }
    //Override to create SimpleAgent agents
    protected override Agent CreateAgentGO(int agentNumber, TextAsset neuralNetworkToLoad)
    {
        CreateSudokuForTheAgent();
        Agent agent = Instantiate(agentPrefab).GetComponent<Agent>();
        agent.gameObject.name = "SudokuAgent";
        agent.transform.SetParent(agentsParentGO.transform);
        agent.Init(NeuralNetwork.LoadFromTextAsset(neuralNetworkToLoad), sudokuController);
        return agent;
    }

    //Override to pass rewardGameObject information to separated Agents
    public override void CreateSeparatedAgentGO()
    {
        if (neuralNetworkToLoad == null)
            print("There is no Neural Network TextAsset attached");
        else
        {
            CreateSudokuForTheAgent();
            Agent agent = Instantiate(agentPrefab).GetComponent<Agent>();
            agent.gameObject.name = "Separated_SudokuAgent";
            string jsonString = JsonManager.ReadJSONFile(neuralNetworkToLoad);
            SerializableNeuralNetwork sNet = JsonManager.DeserializeFromJson<SerializableNeuralNetwork>(jsonString);
            agent.Init(sNet.Deserialized(), sudokuController);
        }
    }
    #endregion

    #region SudokuManager
    private void CreateSudokuForTheAgent()
    {
        this.sudokuController = sudokuGenerator.GenerateSudokuAndGetController();
    }    
    #endregion
}