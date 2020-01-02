using UnityEngine;

/// <summary>
/// Derived class from Agent. 
/// His goal is to reach the position of the hexagon pointer changing his angular velocity.
/// </summary>
public class BoomerangAgent : Agent
{
    private Transform hexagonPointer;
    private float rad = 0f;

    private Rigidbody2D rBody;
    private Material[] mats;

    //Get component references
    private void Start()
    {
        rBody = GetComponent<Rigidbody2D>();
        mats = new Material[transform.childCount];
        for (int i = 0; i < mats.Length; i++)
            mats[i] = transform.GetChild(i).GetComponent<Renderer>().material;
    }

    //Pass hexagon pointer position
    public override void Init(NeuralNetwork net, params object[] info)
    {
        base.Init(net, info);
        hexagonPointer = (Transform)info[0];
    }

    //Get rotation-direction depending of the position between the agent and the hexagonPointer
    protected override void CollectEnvironmentInformation()
    {
        float angle = transform.eulerAngles.z % 360f;
        if (angle < 0f)
            angle += 360f;

        Vector2 deltaVector = (hexagonPointer.position - transform.position).normalized;

        rad = Mathf.Atan2(deltaVector.y, deltaVector.x);
        rad *= Mathf.Rad2Deg;

        rad = rad % 360;
        if (rad < 0)        
            rad = 360 + rad;        

        rad = 90f - rad;
        if (rad < 0f)        
            rad += 360f;
        
        rad = 360 - rad;
        rad -= angle;
        if (rad < 0)
            rad = 360 + rad;
        if (rad >= 180f)
        {
            rad = 360 - rad;
            rad *= -1f;
        }
        rad *= Mathf.Deg2Rad;
    }

    //Set rotation as new input
    protected override void SetNewInputs()
    {
        this.inputs = new float[1];
        this.inputs[0] = rad / (Mathf.PI);
    }

    //Set angular velocity depending of the output
    protected override void AgentAction()
    {
        ChangeAgentColor();
        rBody.velocity = 2.5f * transform.up;
        rBody.angularVelocity = 500f * outputs[0];
    }

    //Assign fitness depending of the orientation, if it's facing directly the hexagon = grater gain
    protected override float CalculateFitnessGain()
    {
        return (1f - Mathf.Abs(inputs[0]));
    }

    //Change Color of the boomerang depending of the distance to the hexagon
    private void ChangeAgentColor()
    {
        float distance = Vector2.Distance(transform.position, hexagonPointer.position);
        if (distance > 20f)
            distance = 20f;
        for (int i = 0; i < mats.Length; i++)
            mats[i].color = new Color(distance / 20f, (1f - (distance / 20f)), (1f - (distance / 20f)));
    }
}