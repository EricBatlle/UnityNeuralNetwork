using UnityEngine;

/// <summary>
/// Derived class from AgentsManager.
/// Implements the creation of SimpleAgent.
/// </summary>
public class SimpleManager : AgentsManager
{
    public GameObject rewardGO = null;

    //Override to create SimpleAgent agents
    protected override Agent CreateAgentGO(int agentNumber)
    {
        Agent agent = Instantiate(agentPrefab).GetComponent<Agent>();
        agent.transform.SetParent(agentsParentGO.transform);
        agent.Init(agentsNets[agentNumber], rewardGO.transform.localPosition);
        return agent;
    }
}
