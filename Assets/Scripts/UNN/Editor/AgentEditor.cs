using UnityEditor;
using UnityEngine;

/// <summary>
/// Base class used to facilitate sharing Editor functionalities through all Agent Editor derivates
/// </summary>
[CustomEditor(typeof(Agent))]
public class AgentEditor : Editor
{
    Agent agent = null;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        agent = (Agent)target;
        GUILayout.Space(10);
        if (agent.initilized)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Save Neural Network"))
                agent.SaveAgentNeuralNetwork();            
            GUILayout.EndHorizontal();            
        }
        if (GUILayout.Button("Load Neural Network"))
            agent.LoadAgentNeuralNetwork();
        GUILayout.Space(10);
    }

}
