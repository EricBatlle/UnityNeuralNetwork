using UnityEngine;

/// <summary>
/// Derived class from AgentsManager.
/// Implements the creation of the BoomerangAgents and the control of the hexagonPointer
/// </summary>
public class BoomerangsManager : AgentsManager
{
    [Header("Boomerang")]
    public GameObject hex;
    [SerializeField] private bool leftMouseDown = false;

    //Hexagon Controller
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            leftMouseDown = true;
        else if (Input.GetMouseButtonUp(0))
            leftMouseDown = false;

        if (leftMouseDown == true)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            hex.transform.position = mousePosition;
        }
    }

    //Override to create BoomerangAgent agents
    protected override Agent CreateAgentGO(int agentNumber)
    {
        BoomerangAgent boomer = ((GameObject)Instantiate(agentPrefab, new Vector3(UnityEngine.Random.Range(-10f, 10f), UnityEngine.Random.Range(-10f, 10f), 0), agentPrefab.transform.rotation)).GetComponent<BoomerangAgent>();
        boomer.transform.SetParent(agentsParentGO.transform);
        boomer.Init(agentsNets[agentNumber], hex.transform);

        return boomer;
    }
}