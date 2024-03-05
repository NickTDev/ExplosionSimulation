using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallsCollision : MonoBehaviour
{
    private void FixedUpdate()
    {
        detectWallCollisions();
    }

    void detectWallCollisions()
    {
        Particle2D[] particles = FindObjectsOfType<Particle2D>();

        for (int i = 0; i < particles.Length; i++)
        {
            if (particles[i].transform.position.x >= 12)
                resolve(particles[i], "right");
            else if (particles[i].transform.position.x <= -12)
                resolve(particles[i], "left");
            else if (particles[i].transform.position.y >= 5.5)
                resolve(particles[i], "top");
            else if (particles[i].transform.position.y <= -4)
                resolve(particles[i], "bottom");
        }
    }

    void resolve(Particle2D particle, string direction)
    {
        Vector2 newVelocity;
        Vector2 newPosition;

        if (direction == "right")
        {
            newVelocity = particle.getVelocity();
            newVelocity.x *= -1;
            particle.setVelocity(newVelocity);
            newPosition = particle.getPosition();
            newPosition.x -= 0.1f;
            particle.setPosition(newPosition);
        }
        else if (direction == "left")
        {
            newVelocity = particle.getVelocity();
            newVelocity.x *= -1;
            particle.setVelocity(newVelocity);
            newPosition = particle.getPosition();
            newPosition.x += 0.1f;
            particle.setPosition(newPosition);
        }
        else if (direction == "bottom")
        {
            newVelocity = particle.getVelocity();
            newVelocity.y *= -1;
            particle.setVelocity(newVelocity);
            newPosition = particle.getPosition();
            newPosition.y += 0.1f;
            particle.setPosition(newPosition);
        }
        else if (direction == "top")
        {
            newVelocity = particle.getVelocity();
            newVelocity.y *= -1;
            particle.setVelocity(newVelocity);
            newPosition = particle.getPosition();
            newPosition.y -= 0.1f;
            particle.setPosition(newPosition);
        }
    }
}
