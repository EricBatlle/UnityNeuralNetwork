using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// This class collects all the extensions used to improve or facilitate the use of in-build methods
/// </summary>
public static class SimpleExtensions
{
    //Invoke
    public static void Invoke(this MonoBehaviour mono, Action action, float delay)
    {
        mono.StartCoroutine(ExecuteAfterTime(action, delay));
    }
    public static void InvokeAndGetCoroutine(this MonoBehaviour mono, Action action, float delay)
    {
        Coroutine coroutine = mono.StartCoroutine(ExecuteAfterTime(action, delay));
        return coroutine;
    }    
    private static IEnumerator ExecuteAfterTime(Action action, float delay)
    {
        yield return new WaitForSeconds(delay);
        action();
    }

    //Invoke Repeating
    public static void InvokeRepeating(this MonoBehaviour mono, Action action, float repeatRate = 1, float initialDelay = 0)
    {
        mono.StartCoroutine(ExecuteRepeatedlyAfterTime(action, repeatRate, initialDelay));
    }
    public static Coroutine InvokeRepeatingAndGetCoroutine(this MonoBehaviour mono, Action action, float repeatRate = 1, float initialDelay = 0)
    {
        Coroutine coroutine = mono.StartCoroutine(ExecuteRepeatedlyAfterTime(action, repeatRate, initialDelay));
        return coroutine;
    }
    private static IEnumerator ExecuteRepeatedlyAfterTime(Action action, float repeatRate, float initialDelay)
    {
        yield return new WaitForSeconds(initialDelay);

        while(true)
        {
            action();
            yield return new WaitForSeconds(repeatRate);
        }
    }    
}