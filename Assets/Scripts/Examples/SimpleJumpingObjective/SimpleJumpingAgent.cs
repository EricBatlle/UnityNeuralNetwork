using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleJumpingAgent : SimpleAgent
{
    //Set velocity direction on X depending of the output
    protected override void AgentAction()
    {
        ChangeAgentColor();
        rBody.velocity = 2.5f * outputs[0] * new Vector3(1, 0, 0);
        rBody.AddForce(new Vector3(0, outputs[1], 0), ForceMode.Impulse);
    }
}
