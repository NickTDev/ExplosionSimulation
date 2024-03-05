using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle3D : MonoBehaviour
{
    //Object data
    [SerializeField]
    public float mass;
    float inverseMass;

    [SerializeField]
    Vector3 velocity;
    [SerializeField]
    public Vector3 acceleration;
    Vector3 accumulatedForces;

    [SerializeField]
    float dampingConstant;

    Vector3 pos;

    [SerializeField]
    float timeLeft;
    [SerializeField]
    bool canDespawn;

    GameObject muzzlePos;
    GameObject spawnPos;

    float restitution = 1.0f;
    Vector3 contactNormal;
    float penetration;

    GamePause pauseManager;
    SpatialPartitioning spManager;
    int numCollisions = 0;

    public bool isFirstScene = true;

    private void Awake()
    {
        //muzzlePos = GameObject.Find("Cannon");
        //spawnPos = GameObject.Find("SpawnPoint");
        pauseManager = GameObject.Find("GameStateManager").GetComponent<GamePause>();
        //spManager = GameObject.Find("GameStateManager").GetComponent<SpatialPartitioning>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //Calculate the direction to shoot
        //Vector3 direction;
        //direction.x = spawnPos.transform.position.x - muzzlePos.transform.position.x;
        //direction.y = spawnPos.transform.position.y - muzzlePos.transform.position.y;
        //direction.z = spawnPos.transform.position.z - muzzlePos.transform.position.z;

        //direction.x = Random.Range(-1.0f, 1.0f);
        //direction.y = Random.Range(-1.0f, 1.0f);
        //direction.z = Random.Range(-1.0f, 1.0f);

        //Apply the speed to the direction
        //direction *= velocity.magnitude;
        //velocity = direction;
        velocity = Vector3.zero;
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

    private void FixedUpdate()
    {
        //foreach (ParticleForceGenerator gen in GetComponents<ParticleForceGenerator>())
        //{
        //    Debug.Log("Force Gen");
        //    gen.updateForce(this);
        //}

        if (isFirstScene)
            GameObject.Find("ExplosionGenerator").GetComponent<explosiveForce>().updateForce(this);
        else if (!isFirstScene)
            GameObject.Find("ExplosionGenerator").GetComponent<outwardExplosiveForce>().updateForce(this);

        if (!pauseManager.getPauseState())
        {
            pos = transform.position;
            Integrator3D.integrate(ref pos, ref velocity, acceleration, accumulatedForces, dampingConstant, getInverseMass());
            transform.position = pos;
        }

        clearAccumulator();

        //if (!spManager.getSP())
        //{
        //    detectCollisions();
        //}

        //Temporarily removed to fix jumping cannon issue
        //detectPlaneCollisions();
        //detectAABBColliderCollisions();
        //detectOBBColliderCollisions();
    }

    void detectCollisions()
    {
        Particle3D[] particles = FindObjectsOfType<Particle3D>();

        for (int i = 0; i < particles.Length; i++)
        {
            if (particles[i].transform.position != transform.position)
            {
                if ((particles[i].transform.position - transform.position).magnitude < (particles[i].transform.localScale.x / 2) + (transform.localScale.x / 2))
                {
                    resolve(particles[i], this);
                }
                numCollisions++;
            }
        }
    }

    void resolve(Particle3D particle1, Particle3D particle2)
    {
        contactNormal = particle1.getPosition() - particle2.getPosition();
        contactNormal.Normalize();

        penetration = ((particle1.transform.localScale.x / 2) + (particle2.transform.localScale.x / 2)) - (particle1.transform.position - particle2.transform.position).magnitude;

        resolveVelocity(particle1, particle2);
        resolveInterpenetration(particle1, particle2);
    }

    float calculateSeparatingVelocity(Particle3D particle1, Particle3D particle2)
    {
        Vector3 relativeVelocity = particle1.getVelocity();
        relativeVelocity -= particle2.getVelocity();
        return Vector3.Dot(relativeVelocity, contactNormal);
    }

    void resolveVelocity(Particle3D particle1, Particle3D particle2)
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

        Vector3 impulsePerIMass = contactNormal * impulse;

        particle1.setVelocity(particle1.getVelocity() + impulsePerIMass * particle1.getInverseMass());
        particle2.setVelocity(particle2.getVelocity() + impulsePerIMass * -particle2.getInverseMass());
    }

    void resolveInterpenetration(Particle3D particle1, Particle3D particle2)
    {
        if (penetration <= 0)
            return;

        float totalInverseMass = particle1.getInverseMass() + particle2.getInverseMass();

        if (totalInverseMass <= 0)
            return;

        Vector3 movePerIMass = contactNormal * (penetration / totalInverseMass);

        Vector3 particle1Movement = movePerIMass * particle1.getInverseMass();
        Vector3 particle2Movement = movePerIMass * -particle2.getInverseMass();

        particle1.setPosition(particle1.getPosition() + particle1Movement);
        particle2.setPosition(particle2.getPosition() + particle2Movement);
    }

    void detectPlaneCollisions()
    {
        Plane[] planes = FindObjectsOfType<Plane>();
        Vector3 collisionNormal = Vector3.zero;

        for (int i = 0; i < planes.Length; i++)
        {
            float sphereOffset = Vector3.Dot(planes[i].getNormal(), transform.position);
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

    void resolvePlaneCollision(Plane plane, Particle3D particle, Vector3 colNormal, float pen)
    {
        resolvePlaneVelocity(particle, colNormal);
        resolvePlaneInterpenetration(particle, colNormal, pen);
    }

    float calculatePlaneSeparatingVelocity(Particle3D particle, Vector3 normal)
    {
        Vector3 relativeVelocity = particle.getVelocity();
        return Vector3.Dot(relativeVelocity, normal);
    }

    void resolvePlaneVelocity(Particle3D particle, Vector3 normal)
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

        Vector3 impulsePerIMass = normal * impulse;

        particle.setVelocity(particle.getVelocity() + impulsePerIMass * particle.getInverseMass());
    }

    void resolvePlaneInterpenetration(Particle3D particle, Vector3 normal, float pen)
    {
        if (pen <= 0)
            return;

        float totalInverseMass = particle.getInverseMass();

        if (totalInverseMass <= 0)
            return;

        Vector3 movePerIMass = normal * (pen / totalInverseMass);

        Vector3 particle1Movement = movePerIMass * particle.getInverseMass();

        particle.setPosition(particle.getPosition() + particle1Movement);
    }

    void detectAABBColliderCollisions()
    {
        Particle3D[] particles = FindObjectsOfType<Particle3D>();
        AABB[] boxes = FindObjectsOfType<AABB>();
    
        for (int i = 0; i < particles.Length; i++)
        {
            for (int j = 0; j < boxes.Length; j++)
            {
                if (boxes[j].testSphereCollision(particles[i]))
                {
                    resolveAABBColliderCollision(particles[i], boxes[j]);
                }
            }
        }
    }
    
    void resolveAABBColliderCollision(Particle3D particle, AABB box)
    {
        Vector3 collisionNormal = particle.getPosition() - box.getClosestPoint(particle);
        collisionNormal.Normalize();

        float pen = (transform.localScale.x / 2) - (box.getClosestPoint(particle) - transform.position).magnitude;
    
        resolveAABBVelocity(particle, collisionNormal);
        resolveAABBInterpenetration(particle, collisionNormal, pen);
    }
    
    float calculateAABBSeparatingVelocity(Particle3D particle, Vector3 col)
    {
        Vector3 relativeVelocity = particle.getVelocity();
        return Vector3.Dot(relativeVelocity, col);
    }

    void resolveAABBVelocity(Particle3D particle, Vector3 normal)
    {
        float separatingVelocity = calculateAABBSeparatingVelocity(particle, normal);

        if (separatingVelocity > 0)
            return;

        float newSeparatingVelocity = -separatingVelocity * restitution;
        float deltaVelocity = newSeparatingVelocity - separatingVelocity;

        float totalInverseMass = particle.getInverseMass();

        if (totalInverseMass <= 0)
            return;

        float impulse = deltaVelocity / totalInverseMass;

        Vector3 impulsePerIMass = normal * impulse;

        particle.setVelocity(particle.getVelocity() + impulsePerIMass * particle.getInverseMass());
    }

    void resolveAABBInterpenetration(Particle3D particle, Vector3 normal, float pen)
    {
        if (pen <= 0)
            return;

        float totalInverseMass = particle.getInverseMass();

        if (totalInverseMass <= 0)
            return;

        Vector3 movePerIMass = normal * (pen / totalInverseMass);

        Vector3 particle1Movement = movePerIMass * particle.getInverseMass();

        particle.setPosition(particle.getPosition() + particle1Movement);
    }

    void detectOBBColliderCollisions()
    {
        Particle3D[] particles = FindObjectsOfType<Particle3D>();
        OBB[] boxes = FindObjectsOfType<OBB>();

        for (int i = 0; i < particles.Length; i++)
        {
            for (int j = 0; j < boxes.Length; j++)
            {
                if (boxes[j].testSphereCollision(particles[i]))
                {
                    resolveOBBColliderCollision(particles[i], boxes[j]);
                }
            }
        }
    }

    void resolveOBBColliderCollision(Particle3D particle, OBB box)
    {
        Vector3 collisionNormal = particle.getPosition() - box.getClosestPoint(particle);
        collisionNormal.Normalize();

        float pen = (transform.localScale.x / 2) - (box.getClosestPoint(particle) - transform.position).magnitude;

        resolveOBBVelocity(particle, collisionNormal);
        resolveOBBInterpenetration(particle, collisionNormal, pen);
    }

    float calculateOBBSeparatingVelocity(Particle3D particle, Vector3 col)
    {
        Vector3 relativeVelocity = particle.getVelocity();
        return Vector3.Dot(relativeVelocity, col);
    }

    void resolveOBBVelocity(Particle3D particle, Vector3 normal)
    {
        float separatingVelocity = calculateAABBSeparatingVelocity(particle, normal);

        if (separatingVelocity > 0)
            return;

        float newSeparatingVelocity = -separatingVelocity * restitution;
        float deltaVelocity = newSeparatingVelocity - separatingVelocity;

        float totalInverseMass = particle.getInverseMass();

        if (totalInverseMass <= 0)
            return;

        float impulse = deltaVelocity / totalInverseMass;

        Vector3 impulsePerIMass = normal * impulse;

        particle.setVelocity(particle.getVelocity() + impulsePerIMass * particle.getInverseMass());
    }

    void resolveOBBInterpenetration(Particle3D particle, Vector3 normal, float pen)
    {
        if (pen <= 0)
            return;

        float totalInverseMass = particle.getInverseMass();

        if (totalInverseMass <= 0)
            return;

        Vector3 movePerIMass = normal * (pen / totalInverseMass);

        Vector3 particle1Movement = movePerIMass * particle.getInverseMass();

        particle.setPosition(particle.getPosition() + particle1Movement);
    }

    public void addForce(Vector3 force)
    {
        if (force.sqrMagnitude > 0.001)
            accumulatedForces += force;
    }

    void clearAccumulator()
    {
        accumulatedForces = Vector3.zero;
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

    public float getMass()
    {
        return mass;
    }

    public void setVelocity(Vector3 v)
    {
        velocity = v;
    }

    public Vector3 getVelocity()
    {
        return velocity;
    }

    public Vector3 getAcceleration()
    {
        return acceleration;
    }

    public void setAcceleration(Vector3 a)
    {
        acceleration = a;
    }

    public void setDamping(float d)
    {
        dampingConstant = d;
    }

    public int getNumCollisions()
    {
        return numCollisions;
    }

    public void resetNumCollisions()
    {
        numCollisions = 0;
    }
}
