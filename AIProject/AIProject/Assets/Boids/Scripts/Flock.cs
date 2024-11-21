using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Boids
{
    [System.Serializable]
    public class Flock
    {
        [Header("Flock Options")]
        [SerializeField] private int amount;
        [SerializeField] private BoidData[] possibleBoids;

        [Header("Rules")]
        [Header("Main")]
        [SerializeField] private float cohesionWeight = 1f;
        [SerializeField] private float seperationWeight = 1f;
        [SerializeField] private float seperationDistance;
        [SerializeField] private float alignmentWeight = 1f;
        [Header("Target follow")]
        [SerializeField] private bool followsTarget;
        [SerializeField] private Transform target;
        [SerializeField] private float targetWeight = 1f;
        [Space]
        [Header("Contain")]
        [SerializeField] private bool isContained = false;
        [SerializeField] private float containWeight = 10f;
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
            for (int i = 0; i < boids.Length; i++)
            {
                // run boid setup
                boids[i] = new Boid(GetBoidData());
            }
        }

        public void UpdateFlock()
        {
            for (int i = 0; i < boids.Length; i++)
            {
                boids[i].ApplyVelocity(CalculateVelocity(boids[i]));
            }
        }

        // boid functions

        private Vector3 CalculateVelocity(Boid _currentBoid)
        {
            Vector3 v = Vector3.zero;
            v = v + Cohesion(_currentBoid) * cohesionWeight;
            v = v + Seperation(_currentBoid) * seperationWeight;
            v = v + Alignment(_currentBoid) * alignmentWeight;

            // optional rules
            v = v + TargetPoint(_currentBoid) * targetWeight;
            v = v + ContainPosition(_currentBoid) * containWeight;

            return v / 100;
        }

        private Vector3 Seperation(Boid _currentBoid)
        {
            Vector3 seperationVelocity = Vector3.zero;

            for (int i = 0; i < boids.Length; i++)
            {
                if (_currentBoid == boids[i]) { continue; }

                float distance = Vector3.Distance(boids[i].BoidObj.transform.position, _currentBoid.BoidObj.transform.position);

                if (distance < seperationDistance)
                {
                    seperationVelocity = seperationVelocity - (boids[i].BoidObj.transform.position - _currentBoid.BoidObj.transform.position);
                    //seperationVelocity = seperationVelocity * (1 / distance);
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
                if (_currentBoid == boids[i]) { continue; }

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
                if (_currentBoid == boids[i]) { continue; }

                avgPos = avgPos + boids[i].BoidObj.transform.position;
            }

            avgPos = avgPos / (boids.Length - 1);
            //Debug.Log($"cohesion velocity: {(avgPos - _currentBoid.BoidObj.transform.position) / 100}");

            return (avgPos - _currentBoid.BoidObj.transform.position);
        }

        private Vector3 TargetPoint(Boid _currentBoid)
        {
            if (!followsTarget) { return Vector3.zero; }

            return (target.position - _currentBoid.BoidObj.transform.position);
        }

        private Vector3 ContainPosition(Boid _currentBoid)
        {
            if (!isContained) { return Vector3.zero; }

            Vector3 returnVector = Vector3.zero;
            Vector3 boidPos = _currentBoid.BoidObj.transform.position;

            if (boidPos.x < xMinMax.x) { returnVector.x = containWeight; }
            else if (boidPos.x > xMinMax.y) { returnVector.x = -containWeight; }

            if (boidPos.y < yMinMax.x) { returnVector.y = containWeight; }
            else if (boidPos.y > yMinMax.y) { returnVector.y = -containWeight; }

            if (boidPos.z < zMinMax.x) { returnVector.z = containWeight; }
            else if (boidPos.z > zMinMax.y) { returnVector.z = -containWeight; }

            return returnVector;
        }

        public BoidData GetBoidData()
        {
            if (possibleBoids.Length > 0)
            {
                return possibleBoids[Random.Range(0, possibleBoids.Length)];
            }

            return possibleBoids[0];
        }

        public Vector3 GetContainArea()
        {
            return new Vector3(Mathf.Abs(xMinMax.x) + xMinMax.y, Mathf.Abs(yMinMax.x) + yMinMax.y, Mathf.Abs(zMinMax.x) + zMinMax.y);
        }
    }
}
