using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class BoilerplateUNNWindow : EditorWindow
{
    [SerializeField] private string scenarioName = "newScenario";
    [SerializeField] private string newScenariosDirectory = "Assets/Scripts/Examples";
    [SerializeField] private static string AGENT = "Agent";
    [SerializeField] private static string MANAGER = "Manager";
    [SerializeField] private static string EDITOR = "Editor";

    [MenuItem("Window/BoilerplateUNN")]
    public static void ShowWindow()
    {
        GetWindow<BoilerplateUNNWindow>("BoilerplateUNN");
    }

    private void OnGUI()
    {
        GUILayout.Label("Create fast new boilerplate scripts for new UNN scenarios", EditorStyles.boldLabel);
        scenarioName = EditorGUILayout.TextField("NewScenario Name", scenarioName);
        newScenariosDirectory = EditorGUILayout.TextField("NewScenario Directory", newScenariosDirectory);

        if (GUILayout.Button("Generate UNN Boilerplate"))
        {
            GenerateUNNBoilerplateElements();
            Debug.Log("<b>Generated "+scenarioName+" directories and scripts</b>");
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

        string agentCS = scenarioName + AGENT + ".cs";
        CreateFile(newScenarioDirectory + "/" + agentCS, GenerateAgentContent());
        string agentEditorCS = scenarioName + AGENT + EDITOR + ".cs";
        CreateFile(newScenarioEditorDirectory + "/" + agentEditorCS, GenerateAgentEditorContent());

        string managerCS = scenarioName + MANAGER + ".cs";
        CreateFile(newScenarioDirectory + "/" + managerCS, GenerateManagerContent());
        string managerEditorCS = scenarioName + MANAGER + EDITOR + ".cs";
        CreateFile(newScenarioEditorDirectory + "/" + managerEditorCS, GenerateManagerEditorContent());



    }

    #region GenerateContent
    //Agent
    private string GenerateAgentContent()
    {
        return
@"using UnityEngine;

/// <summary>
/// Derived class from Agent. 
/// His goal is ********
/// </summary>
public class " + scenarioName+@"Agent : Agent
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
    
    //Manager
    private string GenerateManagerContent()
    {
        return
            @"using UnityEngine;
using static NeuralNetworkSerializer;
/// <summary>
/// Derived class from AgentsManager.
/// Implements the creation of SimpleAgent.
/// </summary>
public class "+scenarioName+@"Manager : AgentsManager
{
    [Header("+'"'+scenarioName + '"'+ @")]
    public GameObject rewardGO = null;

    //Override to create SimpleAgent agents
    protected override Agent CreateAgentGO(int agentNumber)
    {
        Agent agent = Instantiate(agentPrefab).GetComponent<Agent>();
        agent.gameObject.name = " + '"' + scenarioName + @"Agent" + '"' + @";
        agent.transform.SetParent(agentsParentGO.transform);
        agent.Init(agentsNets[agentNumber], rewardGO);
        return agent;
    }
    //Override to create SimpleAgent agents
    protected override Agent CreateAgentGO(int agentNumber, TextAsset neuralNetworkToLoad)
    {
        Agent agent = Instantiate(agentPrefab).GetComponent<Agent>();
        agent.gameObject.name = " + '"' + scenarioName + @"Agent" + '"' + @";
        agent.transform.SetParent(agentsParentGO.transform);
        agent.Init(NeuralNetwork.LoadFromTextAsset(neuralNetworkToLoad), rewardGO);
        return agent;
    }

    //Override to pass rewardGameObject information to separated Agents
    public override void CreateSeparatedAgentGO()
    {
        if (neuralNetworkToLoad == null)
            print(" + '"' + @"There is no Neural Network TextAsset attached" + '"'+ @");
        else
        {
            Agent agent = Instantiate(agentPrefab).GetComponent<Agent>();
            agent.gameObject.name = "+ '"' + "Separated_" + scenarioName + "Agent" + '"'+ @";
            string jsonString = JsonManager.ReadJSONFile(neuralNetworkToLoad);
            SerializableNeuralNetwork sNet = JsonManager.DeserializeFromJson<SerializableNeuralNetwork>(jsonString);
            agent.Init(sNet.Deserialized(), rewardGO);
        }
    }
}";
    }
    private string GenerateManagerEditorContent()
    {
        return
            @"using UnityEditor;

/// <summary>
/// Even if it's empty, this class is needed to use AgentEditor functionalities
/// </summary>
[CustomEditor(typeof(" + scenarioName + @"Manager))]
public class " + scenarioName + @"ManagerEditor : AgentsManagerEditor
{
}";
    }
    #endregion
    
}
