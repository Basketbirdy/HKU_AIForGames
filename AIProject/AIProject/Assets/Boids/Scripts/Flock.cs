using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[System.Serializable]
public class Flock
{
    [Header("Flock Options")]
    [SerializeField] private int amount;
    [SerializeField] private float seperationDistance;
    [SerializeField] private BoidData[] possibleBoids;

    [Header("Extra")]
    [SerializeField] private bool followsTarget;
    [SerializeField] private Transform target;
    [Range(0, 100)] [SerializeField] private int targetWeight = 1;
    [Space]
    [SerializeField] private bool isContained = false;
    [SerializeField] private float containStrength = 10f;
    [SerializeField] Vector2 xMinMax;
    [SerializeField] Vector2 yMinMax;
    [SerializeField] Vector2 zMinMax;

    [Header("Debugging")]
    [SerializeField] private int flockId;
    private Boid[] boids;

    public void Setup(int _id)
    {
        flockId = _id;

        // spawn boids
        boids = new Boid[amount];
        for(int i = 0; i < boids.Length; i++)
        {
            // run boid setup
            boids[i] = new Boid(GetBoidData());
        }
    }

    public void UpdateFlock()
    {
        for(int i = 0;i < boids.Length; i++)
        {
            boids[i].ApplyVelocity(CalculateVelocity(boids[i]));
        }
    }
    
    // boid functions

    private Vector3 CalculateVelocity(Boid _currentBoid)
    {
        return Seperation(_currentBoid) + Cohesion(_currentBoid) + Alignment(_currentBoid) + TargetPoint(_currentBoid) + ContainPosition(_currentBoid);
    }

    private Vector3 Seperation(Boid _currentBoid)
    {
        Vector3 seperationVelocity = Vector3.zero;

        for(int i = 0; i < boids.Length; i++)
        {
            if (_currentBoid == boids[i]) { continue; }

            float distance = Vector3.Distance(boids[i].BoidObj.transform.position, _currentBoid.BoidObj.transform.position);

            if (distance < seperationDistance)
            {
                seperationVelocity = seperationVelocity - (boids[i].BoidObj.transform.position - _currentBoid.BoidObj.transform.position);
                seperationVelocity = seperationVelocity * (1 / distance);
            }
        }

        //Debug.Log($"Seperation velocity: {seperationVelocity}");
        return seperationVelocity;
    }

    private Vector3 Alignment(Boid _currentBoid)
    {
        Vector3 avgVelocity = Vector3.zero;

        for (int i = 0; i < boids.Length; i++)
        {
            if(_currentBoid == boids[i]) { continue; }

            avgVelocity = avgVelocity + boids[i].Velocity;
        }

        avgVelocity = avgVelocity / (boids.Length - 1);

        //Debug.Log($"Alignment velocity: {avgVelocity}");
        return (avgVelocity - _currentBoid.Velocity / 8);
    }

    private Vector3 Cohesion(Boid _currentBoid)
    {
        Vector3 avgPos = Vector3.zero;

        for (int i = 0; i < boids.Length; i++)
        {
            if(_currentBoid == boids[i]) { continue; }

            avgPos = avgPos + boids[i].BoidObj.transform.position;
        }

        avgPos =  avgPos / (boids.Length - 1);
        //Debug.Log($"cohesion velocity: {(avgPos - _currentBoid.BoidObj.transform.position) / 100}");

        return (avgPos - _currentBoid.BoidObj.transform.position) / 100;
    }

    private Vector3 TargetPoint(Boid _currentBoid)
    {
        if (!followsTarget) { return Vector3.zero; }

        return (target.position - _currentBoid.BoidObj.transform.position) / (100 - targetWeight);
    }

    private Vector3 ContainPosition(Boid _currentBoid)
    {
        if (!isContained) { return Vector3.zero; }

        Vector3 returnVector = Vector3.zero;
        Vector3 boidPos = _currentBoid.BoidObj.transform.position;

        if(boidPos.x < xMinMax.x) { returnVector.x = containStrength; }
        else if(boidPos.x > xMinMax.y) { returnVector.x = -containStrength; }

        if (boidPos.y < yMinMax.x) { returnVector.y = containStrength; }
        else if (boidPos.y > yMinMax.y) { returnVector.y = -containStrength; }

        if (boidPos.z < zMinMax.x) { returnVector.z = containStrength; }
        else if (boidPos.z > zMinMax.y) { returnVector.z = -containStrength; }

        return returnVector;
    }

    public BoidData GetBoidData() 
    { 
        if(possibleBoids.Length > 0)
        {
            return possibleBoids[Random.Range(0, possibleBoids.Length)];
        }

        return possibleBoids[0];
    }
}
