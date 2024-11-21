using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boids
{

    [CreateAssetMenu(menuName = "Boids/BoidData", fileName = "New BoidData")]
    public class BoidData : ScriptableObject
    {
        [Header("Variables")]
        public float maxSpeed;

        [Header("Visuals")]
        public GameObject obj;
    }
}

