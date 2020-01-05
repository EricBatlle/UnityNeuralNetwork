using UnityEditor;
using UnityEngine;

/// <summary>
/// Base class used to facilitate sharing Editor functionalities through all Agent Editor derivates
/// </summary>
[CustomEditor(typeof(AgentsManager))]
public class AgentsManagerEditor : Editor
{
    AgentsManager manager = null;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        manager = (AgentsManager)target;
        GUILayout.Space(10);

        if(EditorApplication.isPlaying)
        {
            if(manager.isTraining)
            {
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Reset Training"))
                    manager.ResetTraining();
                if (GUILayout.Button("Stop Training"))
                    manager.StopTraining();
                GUILayout.EndHorizontal();
            }
            else if(manager.isTrainingStoped)
            {
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Reset Training"))
                        manager.ResetTraining();
                if (GUILayout.Button("ReStart Train"))
                    manager.StartTraining();
                GUILayout.EndHorizontal();
            }
            else
            {
                if (GUILayout.Button("Start Train"))
                    manager.StartTraining();
            }                

            if (manager.neuralNetworkToLoad != null)
            {
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Create Loaded Agent"))
                    manager.CreateSeparatedAgentGO();
                GUILayout.EndHorizontal();
            }
        }

        GUILayout.Space(10);
    }

}