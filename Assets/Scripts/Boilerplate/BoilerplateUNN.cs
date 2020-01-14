using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class BoilerplateUNN : EditorWindow
{
    [SerializeField] private string scenarioName = "newScenario";
    [SerializeField] private string newScenariosDirectory = "Assets/Scripts/Examples";
    [SerializeField] private static string AGENT = "Agent";
    [SerializeField] private static string MANAGER = "Manager";
    [SerializeField] private static string EDITOR = "Editor";

    [MenuItem("Window/BoilerplateUNN")]
    public static void ShowWindow()
    {
        GetWindow<BoilerplateUNN>("BoilerplateUNN");
    }

    private void OnGUI()
    {
        GUILayout.Label("Create fast new boilerplate scripts for new UNN scenarios", EditorStyles.boldLabel);
        scenarioName = EditorGUILayout.TextField("NewScenarioName", scenarioName);
        newScenariosDirectory = EditorGUILayout.TextField("NewScenarioName", newScenariosDirectory);

        if (GUILayout.Button("Generate UNN Boilerplate"))
        {
            GenerateUNNBoilerplateElements();
            Debug.Log("Generated");
        }
    }

    private void CreateDirectory(string path)
    {
        // Specify the directory you want to manipulate.        
        try
        {
            // Determine whether the directory exists.
            if (Directory.Exists(path))
            {
                Debug.Log("That path exists already.");
                return;
            }
            else
            {
                // Try to create the directory.
                DirectoryInfo di = Directory.CreateDirectory(path);
                Debug.Log("The directory was created successfully");
            }
        }
        catch (Exception e)
        {
            Debug.Log("The directory creation process failed: " + e.ToString());
        }
        finally { }
    }
    private void CreateFile(string path, string content)
    {
        try
        {
            if (File.Exists(path))
            {
                Debug.Log("That file exists already.");
                return;
            }
            else
            {
                //Write the file
                StreamWriter writer = new StreamWriter(path, true);
                writer.Write(content);
                writer.Close();
                Debug.Log("Created new file in " + path);
            }
        }
        catch (Exception e)
        {
            Debug.Log("The file creation process failed: " + e.ToString());
        }
    }

    private void GenerateUNNBoilerplateElements()
    {
        string newScenarioDirectory = newScenariosDirectory + "/" + scenarioName;
        string newScenarioEditorDirectory = newScenarioDirectory + "/Editor";

        CreateDirectory(newScenarioDirectory);
        CreateDirectory(newScenarioEditorDirectory);

        string agentEditorCS = scenarioName + AGENT + EDITOR + ".cs";
        CreateFile(newScenarioEditorDirectory + "/" + agentEditorCS, GenerateAgentEditorContent());

        string agentCS = scenarioName + AGENT + EDITOR + ".cs";
        CreateFile(newScenarioEditorDirectory + "/" + agentCS, GenerateAgentContent());
    }

    #region GenerateContent
    private string GenerateAgentEditorContent()
    {
        return
@"using UnityEditor;
            
/// <summary>
/// Even if it's empty, this class is needed to use AgentEditor functionalities
/// </summary>
[CustomEditor(typeof(" + scenarioName + @"Agent))]
public class "+scenarioName+@"AgentEditor : AgentEditor
{        
}";
    }

    private string GenerateAgentContent()
    {
        return
@"using UnityEngine;

public class "+scenarioName+@"Agent : Agent
{
    protected override void AgentAction()
    {
        throw new System.NotImplementedException();
    }

    protected override float CalculateFitnessGain()
    {
        throw new System.NotImplementedException();
    }

    protected override void CollectEnvironmentInformation()
    {
        throw new System.NotImplementedException();
    }

    protected override void LoadInfo()
    {
        throw new System.NotImplementedException();
    }

    protected override void SetNewInputs()
    {
        throw new System.NotImplementedException();
    }
}";
    }
    #endregion
    
}
