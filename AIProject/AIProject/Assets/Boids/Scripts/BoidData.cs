using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boids/BoidData", fileName = "New BoidData")]
public class BoidData : ScriptableObject
{
    [Header("Variables")]
    public float maxSpeed;
    public float maxForce;

    [Header("Visuals")]
    public GameObject obj;
}
