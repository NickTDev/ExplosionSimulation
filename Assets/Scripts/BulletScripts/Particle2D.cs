using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle2D : MonoBehaviour
{
    //Object data
    [SerializeField]
    public float mass;
    float inverseMass;

    [SerializeField]
    Vector2 velocity;
    [SerializeField]
    public Vector2 acceleration;
    Vector2 accumulatedForces;

    [SerializeField]
    float dampingConstant;

    Vector2 pos;

    [SerializeField]
    float timeLeft;
    [SerializeField]
    bool canDespawn;

    GameObject muzzlePos;
    GameObject spawnPos;

    float restitution = 1.0f;
    Vector2 contactNormal;
    float penetration;

    private void Awake()
    {
        muzzlePos = GameObject.Find("Cannon");
        spawnPos = GameObject.Find("SpawnPoint");
    }

    // Start is called before the first frame update
    void Start()
    {
        //Calculate the direction to shoot
        Vector2 direction;
        direction.x = spawnPos.transform.position.x - muzzlePos.transform.position.x;
        direction.y = spawnPos.transform.position.y - muzzlePos.transform.position.y;

        //Apply the speed to the direction
        direction *= velocity.magnitude;
        velocity = direction;
    }

    // Update is called once per frame
    void Update()
    {
        if (canDespawn)
        {
            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
            }
            else if (timeLeft <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    void FixedUpdate()
    {
        foreach (ParticleForceGenerator gen in GetComponents<ParticleForceGenerator>())
        {
            gen.updateForce(this);
        }

        pos = transform.position;
        Integrator.integrate(ref pos, ref velocity, acceleration, accumulatedForces, dampingConstant, getInverseMass());
        transform.position = pos;

        clearAccumulator();

        //Collisions are detected and resolved in CollisionResolver, attached to CollisionDetector Object, done in the FixedUpdate there
        detectCollisions();
        detectPlaneCollisions();
    }

    void detectCollisions()
    {
        Particle2D[] particles = FindObjectsOfType<Particle2D>();

        for (int i = 0; i < particles.Length; i++)
        {
            if (particles[i].transform.position != transform.position)
            {
                if ((particles[i].transform.position - transform.position).magnitude < (particles[i].transform.localScale.x / 2) + (transform.localScale.x / 2))
                {
                    resolve(particles[i], this);
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
        Plane[] planes = FindObjectsOfType<Plane>();
        Vector2 collisionNormal = Vector2.zero;

        for (int i = 0; i < planes.Length; i++)
        {
            float sphereOffset = Vector2.Dot(planes[i].getNormal(), transform.position);
            float spherePlaneDistance = sphereOffset - planes[i].getOffset();
            if (Mathf.Abs(spherePlaneDistance) <= (transform.localScale.x / 2))
            {
                if (spherePlaneDistance < 0)
                {
                    collisionNormal = -(planes[i].getNormal());
                }
                else if (spherePlaneDistance > 0)
                {
                    collisionNormal = planes[i].getNormal();
                }
                float spherePlanePenetration = (transform.localScale.x / 2) - Mathf.Abs(spherePlaneDistance);

                resolvePlaneCollision(planes[i], this, collisionNormal, spherePlanePenetration);
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

    public void addForce(Vector2 force)
    {
        if (force.sqrMagnitude > 0.001)
            accumulatedForces += force;
    }

    void clearAccumulator()
    {
        accumulatedForces = Vector2.zero;
    }

    public void setPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    public Vector3 getPosition()
    {
        return transform.position;
    }

    public void setMass(float m)
    {
        mass = m;
    }

    public void setInverseMass(float m)
    {
        inverseMass = m;
    }

    public float getInverseMass()
    {
        return (1 / mass);
    }

    public void setVelocity(Vector2 v)
    {
        velocity = v;
    }

    public Vector2 getVelocity()
    {
        return velocity;
    }

    public Vector2 getAcceleration()
    {
        return acceleration;
    }

    public void setAcceleration(Vector2 a)
    {
        acceleration = a;
    }

    public void setDamping(float d)
    {
        dampingConstant = d;
    }
}
