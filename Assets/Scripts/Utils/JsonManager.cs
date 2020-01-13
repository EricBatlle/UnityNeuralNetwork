using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class JsonManager
{
    //Wrapper made it to deal with Nested jsons
    [Serializable]
    private class Wrapper<T>
    {
        public T[] array = null;
    }

    #region Deserialize
    public static T[] DeserializeFromJsonArray<T>(string json)
    {
        string newJson = "{ \"array\": " + json + "}";
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
        return wrapper.array;
    }
    public static T DeserializeFromJson<T>(string jsonInfo)
    {
        return JsonUtility.FromJson<T>(jsonInfo);
    }
    #endregion

    #region Serialize
    public static string SerializeToJson<T>(T objectToSerialize)
    {
        return JsonUtility.ToJson(objectToSerialize);
    }
    public static string SerializeToJsonArray<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.array = array;
        return JsonUtility.ToJson(wrapper);
    }
    #endregion

    #region Write/Read Json files
    //Write to JSON
    public static void WriteJSONFile(string fileName, string content, string storingPath ="Assets")
    {
        //Check if storingPath exists, if not, create the directories and subdirectories
        if(!Directory.Exists(storingPath))
        {
            Debug.Log(string.Format("Path directory {0} does not exist, created now.", storingPath));
            Directory.CreateDirectory(storingPath);
        }

        //Give the fileName an index in case the name already exists file--> file_1
        string completePath = storingPath + "/" + fileName+".json";
        string newFilename = fileName + "_";
        int i = 0;
        do
        {
            if (File.Exists(completePath))
            {
                fileName = newFilename + i;
            }
            completePath = storingPath + "/" + fileName + ".json";
            i++;
        } while (File.Exists(completePath));
        
        //Write the JSON file
        StreamWriter writer = new StreamWriter(completePath, true);
        writer.Write(content);
        writer.Close();
        Debug.Log(string.Format("Created new JSON file {0} in {1}",fileName,completePath));
    }

    //Read from JSON
    public static string ReadJSONFile(string filePath)
    {
        StreamReader reader = new StreamReader(filePath);
        string content = reader.ReadToEnd();
        reader.Close();

        return content;
    }
    public static string ReadJSONFile(TextAsset textAsset)
    {        
        return textAsset.text;
    }
    #endregion
}
