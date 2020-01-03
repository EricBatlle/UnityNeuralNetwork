using System;
using System.Collections;
using System.Collections.Generic;
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
}
