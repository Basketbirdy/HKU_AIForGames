using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidManager : MonoBehaviour
{
    [SerializeField] private GameObject boidPrefab;
    [SerializeField] private Flock[] flocks;

    private void Start()
    {
        System.DateTime start = System.DateTime.Now;

        // foreach flock
        for(int i = 0; i < flocks.Length; i++)
        {
            flocks[i].Setup(i);
        }

        System.TimeSpan duration = System.DateTime.Now - start;
        Debug.Log($"Duration: {duration.Milliseconds} milliseconds");
    }

    private void FixedUpdate()
    {
        for(int i = 0; i < flocks.Length; i++)
        {
            flocks[i].UpdateFlock();
        }
    }

}
