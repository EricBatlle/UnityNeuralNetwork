using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simple3DAgent : SimpleAgent
{
    private float distanceToReward = 0f;
    private Vector2 distanceToRewardVector = new Vector2(1,1);
    private Vector2 direction = new Vector2(1,1);

    //Get direction depending of the distance between the agent and the reward
    protected override void CollectEnvironmentInformation()
    {
        distanceToReward = Vector3.Distance(transform.position, rewardGO.transform.position);
        distanceToRewardVector.x = transform.position.x - rewardGO.transform.position.x;
        distanceToRewardVector.y = transform.position.z - rewardGO.transform.position.z;

        direction = new Vector2(1,1);

        if (distanceToRewardVector.x < 0)
            direction.x = 1;
        else if (distanceToRewardVector.x > 0)
            direction.x = -1;
        else
            direction.x = 0;

        if (distanceToRewardVector.y < 0)
            direction.y = 1;
        else if (distanceToRewardVector.y > 0)
            direction.y = -1;
        else
            direction.y = 0;
    }

    //Set direction as new input
    protected override void SetNewInputs()
    {
        this.inputs = new float[2];
        this.inputs[0] = direction.x;
        this.inputs[1] = direction.y;
    }

    //Set velocity direction on X depending of the output
    protected override void AgentAction()
    {
        ChangeAgentColor();
        rBody.velocity = 2.5f * new Vector3(outputs[0], 0, outputs[1]);
    }

    //Assign fitness depending of the distance, less distance = greater fitness gain
    protected override float CalculateFitnessGain()
    {
        return (1f - Mathf.Abs(distanceToReward));
    }
}
