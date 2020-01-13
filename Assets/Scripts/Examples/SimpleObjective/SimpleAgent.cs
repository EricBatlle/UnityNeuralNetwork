using UnityEngine;

/// <summary>
/// Derived class from Agent. 
/// His goal is to reach the position of the reward changing his velocity direction.
/// </summary>
public class SimpleAgent : Agent
{
    [Header("Simple")]
    [SerializeField] protected Rigidbody rBody = null;
    [SerializeField] private Renderer rendererComponent = null;
    private Material mat;

    protected GameObject rewardGO = null;
    private float direction = 0f;
    private float distanceToReward = 0f;

    //Get component references and set initial position
    private void Start()
    {        
        mat = rendererComponent.material;
        this.transform.position = new Vector3(0, 0, 0);
    }

    //Pass reward Game Object
    public override void Init(NeuralNetwork net, params object[] info)
    {
        base.Init(net, info);
        rewardGO = (GameObject)info[0];
    }

    //Load reward gameObject from SimpleManager
    protected override void LoadInfo()
    {
        SimpleManager manager = (SimpleManager)FindObjectOfType(typeof(SimpleManager));
        rewardGO = manager.rewardGO;
    }

    //Get direction depending of the distance between the agent and the reward
    protected override void CollectEnvironmentInformation()
    {
        distanceToReward = transform.position.x - rewardGO.transform.position.x;
        direction = 1f;

        if (distanceToReward < 0)        
            direction = 1;        
        else if (distanceToReward > 0)        
            direction = -1;        
        else        
            direction = 0;
    }

    //Set direction as new input
    protected override void SetNewInputs()
    {
        this.inputs = new float[1];
        this.inputs[0] = direction;
    }

    //Set velocity direction on X depending of the output
    protected override void AgentAction()
    {
        ChangeAgentColor();
        rBody.velocity = 2.5f * outputs[0] * new Vector3(1, 0, 0);
    }

    //Assign fitness depending of the distance, less distance = greater fitness gain
    protected override float CalculateFitnessGain()
    {
        return (1f - distanceToReward);
    }

    //Change Color of the agent depending of the distance to the reward
    protected void ChangeAgentColor()
    {          
        mat.color = new Color(distanceToReward / 20f, (1f - (distanceToReward / 20f)), (1f - (distanceToReward / 20f)));
    }
}