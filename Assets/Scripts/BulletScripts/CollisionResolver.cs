using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionResolver : MonoBehaviour
{
    float restitution = 1.0f;
    Vector2 contactNormal;
    float penetration;

    private void FixedUpdate()
    {
        detectCollisions();
        detectPlaneCollisions();
    }

    public void detectCollisions()
    {
        Particle2D[] particles = FindObjectsOfType<Particle2D>();

        for (int i = 0; i < particles.Length; i++)
        {
            for (int j = i + 1; j < particles.Length; j++)
            {
                if ((particles[i].transform.position - particles[j].transform.position).magnitude < (particles[i].transform.localScale.x / 2) + (particles[j].transform.localScale.x / 2))
                {
                    resolve(particles[i], particles[j]);
                }
            }
        }
    }

    void resolve(Particle2D particle1, Particle2D particle2)
    {
        contactNormal = particle1.getPosition() - particle2.getPosition();
        contactNormal.Normalize();

        penetration = ((particle1.transform.localScale.x / 2) + (particle2.transform.localScale.x / 2)) - (particle1.transform.position - particle2.transform.position).magnitude;

        resolveVelocity(particle1, particle2);
        resolveInterpenetration(particle1, particle2);
    }

    float calculateSeparatingVelocity(Particle2D particle1, Particle2D particle2)
    {
        Vector2 relativeVelocity = particle1.getVelocity();
        relativeVelocity -= particle2.getVelocity();
        return Vector2.Dot(relativeVelocity, contactNormal);
    }

    void resolveVelocity(Particle2D particle1, Particle2D particle2)
    {
        float separatingVelocity = calculateSeparatingVelocity(particle1, particle2);

        if (separatingVelocity > 0)
            return;

        float newSeparatingVelocity = -separatingVelocity * restitution;
        float deltaVelocity = newSeparatingVelocity - separatingVelocity;

        float totalInverseMass = particle1.getInverseMass() + particle2.getInverseMass();

        if (totalInverseMass <= 0)
            return;

        float impulse = deltaVelocity / totalInverseMass;

        Vector2 impulsePerIMass = contactNormal * impulse;

        particle1.setVelocity(particle1.getVelocity() + impulsePerIMass * particle1.getInverseMass());
        particle2.setVelocity(particle2.getVelocity() + impulsePerIMass * -particle2.getInverseMass());
    }

    void resolveInterpenetration(Particle2D particle1, Particle2D particle2)
    {
        if (penetration <= 0)
            return;

        float totalInverseMass = particle1.getInverseMass() + particle2.getInverseMass();

        if (totalInverseMass <= 0)
            return;

        Vector2 movePerIMass = contactNormal * (penetration / totalInverseMass);

        Vector3 particle1Movement = movePerIMass * particle1.getInverseMass();
        particle1Movement.z = 0;
        Vector3 particle2Movement = movePerIMass * -particle2.getInverseMass();
        particle2Movement.z = 0;

        particle1.setPosition(particle1.getPosition() + particle1Movement);
        particle2.setPosition(particle2.getPosition() + particle2Movement);
    }

    void detectPlaneCollisions()
    {
        Particle2D[] particles = FindObjectsOfType<Particle2D>();
        Plane[] planes = FindObjectsOfType<Plane>();
        Vector2 collisionNormal = Vector2.zero;

        for (int i = 0; i < particles.Length; i++)
        {
            for (int j = 0; j < planes.Length; j++)
            {
                float sphereOffset = Vector2.Dot(planes[j].getNormal(), particles[i].transform.position);
                float spherePlaneDistance = sphereOffset - planes[j].getOffset();
                if (Mathf.Abs(spherePlaneDistance) <= (particles[i].transform.localScale.x / 2))
                {
                    if (spherePlaneDistance < 0)
                    {
                        collisionNormal = -(planes[j].getNormal());
                    } else if (spherePlaneDistance > 0)
                    {
                        collisionNormal = planes[j].getNormal();
                    }
                    float spherePlanePenetration = (particles[i].transform.localScale.x / 2) - Mathf.Abs(spherePlaneDistance);

                    resolvePlaneCollision(planes[j], particles[i], collisionNormal, spherePlanePenetration);
                }
            }
        }
    }
    
    void resolvePlaneCollision(Plane plane, Particle2D particle, Vector2 colNormal, float pen)
    {
        resolvePlaneVelocity(particle, colNormal);
        resolvePlaneInterpenetration(particle, colNormal, pen);
    }

    float calculatePlaneSeparatingVelocity(Particle2D particle, Vector2 normal)
    {
        Vector2 relativeVelocity = particle.getVelocity();
        return Vector2.Dot(relativeVelocity, normal);
    }

    void resolvePlaneVelocity(Particle2D particle, Vector2 normal)
    {
        float separatingVelocity = calculatePlaneSeparatingVelocity(particle, normal);

        if (separatingVelocity > 0)
            return;

        float newSeparatingVelocity = -separatingVelocity * restitution;
        float deltaVelocity = newSeparatingVelocity - separatingVelocity;

        float totalInverseMass = particle.getInverseMass();

        if (totalInverseMass <= 0)
            return;

        float impulse = deltaVelocity / totalInverseMass;

        Vector2 impulsePerIMass = normal * impulse;

        particle.setVelocity(particle.getVelocity() + impulsePerIMass * particle.getInverseMass());
    }

    void resolvePlaneInterpenetration(Particle2D particle, Vector2 normal, float pen)
    {
        if (pen <= 0)
            return;

        float totalInverseMass = particle.getInverseMass();

        if (totalInverseMass <= 0)
            return;

        Vector2 movePerIMass = normal * (pen / totalInverseMass);

        Vector3 particle1Movement = movePerIMass * particle.getInverseMass();
        particle1Movement.z = 0;

        particle.setPosition(particle.getPosition() + particle1Movement);
    }
}
