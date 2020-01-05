using UnityEngine;
using static NeuralNetworkSerializer;

/// <summary>
/// Component that makes the decision of what action to take given certain inputs
/// </summary>
public abstract class Agent : MonoBehaviour
{
    [Header("Agent")]
    [SerializeField] public bool initilized = false;           //flag to know if the agent have been activated
    [SerializeField] public NeuralNetwork net = null;               //neural network associated to the agent
    [SerializeField] protected object[] info = null;            //info passed to the agent to generate inputs
    [SerializeField] protected float[] inputs = new float[1];   //actions that the agent can make
    [SerializeField] protected float[] outputs = null;          //outputs calculated by agent neural network    
    [Header("Loaded Neural Network")]
    [SerializeField] protected TextAsset neuralNetworkToLoad = null;

    public SerializableNeuralNetwork sNet;

    /// <summary>
    /// Initialize Agent behaviour
    /// </summary>
    /// <param name="net">Neural network assigned to the agent</param>
    /// <param name="info">Information passed to the agent to help him calculate his actions</param>
    public virtual void Init(NeuralNetwork net, params object[] info)
    {
        if (neuralNetworkToLoad != null)
            LoadAgentNeuralNetwork();
        else
            this.net = net;

        this.initilized = true;
        this.info = info;
    }

    #region AgentCycle
    /// <summary>
    /// Agent behaviour cycle.
    /// <para>
    /// Starts collecting information from the moment + the information passed.
    /// Then sets the inputs used to feed the neural network, receiving an output used to decide the next action.
    /// Finally rewards the agent to increase/decrease it's fitness.
    /// </para>   
    /// </summary>
    private void FixedUpdate()
    {
        if(initilized)
        {
            //Collect Environment Information
            CollectEnvironmentInformation();
            //Set Inputs
            SetNewInputs();
            //Generate Outputs
            GenerateOutputs();
            //Do the action with the information of the new given output
            AgentAction();
            //Calculate fitness gain adjust after the action and add it to the agent neural network
            AddNeuralNetworkFitness(CalculateFitnessGain());
        }
    }

    #region Methods to implement on every class that inherits from Agent
    protected abstract void CollectEnvironmentInformation();
    protected abstract void SetNewInputs();
    protected abstract void AgentAction();
    protected abstract float CalculateFitnessGain();
    #endregion

    /// <summary>
    /// Generate network output based on the inputs received
    /// </summary>
    private void GenerateOutputs()
    {
        outputs = this.net.FeedForward(inputs);
    }

    /// <summary>
    /// Add neural network fitness value
    /// </summary>
    /// <param name="fit">New fit adjust to the fitness</param>
    private void AddNeuralNetworkFitness(float fit = 0f)
    {
        this.net.AddFitness(fit);
    }
    #endregion

    #region Save/Load agent neural network
    /// <summary>
    /// Save agent neural network information into JSON file
    /// </summary>
    public void SaveAgentNeuralNetwork()
    {
        string jsonString = JsonManager.SerializeToJson<SerializableNeuralNetwork>(this.net.Serialized());
        JsonManager.WriteJSONFile("NeuralNetworkJSON", jsonString);
    }
    public void LoadAgentNeuralNetwork()
    {
        if (neuralNetworkToLoad == null)     
            print("There is no Neural Network TextAsset attached");
        else
        {
            string jsonString = JsonManager.ReadJSONFile(neuralNetworkToLoad);
            SerializableNeuralNetwork sNet = JsonManager.DeserializeFromJson<SerializableNeuralNetwork>(jsonString);
            this.net = sNet.Deserialized();
            this.sNet = sNet;
        }
        this.initilized = true;
        SetInfo();
    }
    #endregion

    protected virtual void SetInfo()
    {

    }
}