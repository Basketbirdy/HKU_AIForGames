using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boids
{

    [System.Serializable]
    public class Boid
    {
        private float randomOffset = 25f;

        public Vector3 Velocity { get; private set; }
        public BoidData Data { get; private set; }
        public GameObject BoidObj { get; private set; }

        public Boid(BoidData _data)
        {
            // insertion
            Data = _data;

            // creation
            BoidObj = GameObject.Instantiate(Data.obj);
            Vector3 offset = new Vector3(Random.Range(-randomOffset, randomOffset), Random.Range(-randomOffset, randomOffset), Random.Range(-randomOffset, randomOffset));
            BoidObj.transform.position += offset;
        }

        // boid functions

        public void ApplyVelocity(Vector3 _velocity)
        {
            Velocity += _velocity;
            LimitVelocity();

            BoidObj.transform.position += Velocity;
        }

        private void LimitVelocity()
        {
            if(Velocity.magnitude > Data.maxSpeed)
            {
                Velocity = (Velocity / Velocity.magnitude) * Data.maxSpeed;
            }
        }
    }

}

