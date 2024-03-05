using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class massIncreaseCol : MonoBehaviour
{
    Particle2D[] particles;
    Particle2D thisParticle;

    private void Start()
    {
        thisParticle = gameObject.GetComponent<Particle2D>();
    }

    private void Update()
    {
        detectCollision();
    }

    void detectCollision()
    {
        particles = FindObjectsOfType<Particle2D>();

        for (int i = 0; i < particles.Length; i++)
        {
            if (particles[i].transform.position != transform.position)
            {
                if ((particles[i].transform.position - transform.position).magnitude < (particles[i].transform.localScale.x / 2) + (transform.localScale.x / 2))
                {
                    changeMasses(particles[i]);
                }
            }
        }
    }

    void changeMasses(Particle2D otherParticle)
    {
        //Increase this objects mass/size
        thisParticle.setMass(thisParticle.mass + 1.0f);
        thisParticle.transform.localScale = new Vector3(thisParticle.transform.localScale.x + 0.1f, thisParticle.transform.localScale.y + 0.1f, thisParticle.transform.localScale.z + 0.1f);

        //Decreases other objects mass/size
        otherParticle.setMass(thisParticle.mass - 1.0f);
        otherParticle.transform.localScale = new Vector3(otherParticle.transform.localScale.x - 0.1f, otherParticle.transform.localScale.y - 0.1f, otherParticle.transform.localScale.z - 0.1f);
    }
}
