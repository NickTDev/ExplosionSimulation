using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeGravity : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        Particle2D[] particles = FindObjectsOfType<Particle2D>();

        if (Input.GetKey(KeyCode.UpArrow))
        {
            for (int i = 0; i < particles.Length; i++)
            {
                Vector2 newAccel = particles[i].getAcceleration();
                newAccel.y += 0.2f;
                particles[i].setAcceleration(newAccel);
            }
        } else if (Input.GetKey(KeyCode.DownArrow))
        {
            for (int i = 0; i < particles.Length; i++)
            {
                Vector2 newAccel = particles[i].getAcceleration();
                newAccel.y -= 0.2f;
                particles[i].setAcceleration(newAccel);
            }
        }
    }
}
