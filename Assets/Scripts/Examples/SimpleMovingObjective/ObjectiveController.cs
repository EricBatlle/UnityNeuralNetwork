using System.Collections.Generic;
using UnityEngine;

public class ObjectiveController : MonoBehaviour
{
    [SerializeField] private float changeLocationRate = 4f;
    [SerializeField] private List<GameObject> spawnLocations = new List<GameObject>();

    private void Start()
    {
        this.InvokeRepeating(ChangeObjectiveLocation, changeLocationRate);
    }

    private void ChangeObjectiveLocation()
    {
        //Get one random spawn location from the list
        this.transform.localPosition = spawnLocations[Random.Range(0, spawnLocations.Count)].transform.localPosition;
    }
}