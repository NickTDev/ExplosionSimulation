using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BSPChildType
{
    NODE,
    OBJECTS
}

public class BSPChild
{
    public BSPChildType type;

    public BSPNode node;
    public List<Particle3D> BSPObjectSet;
}

public class BSPNode
{
    public PartitioningPlane plane;
    public BSPChild front;
    public BSPChild back;
}

public class BinarySpacePartitioning : MonoBehaviour
{
    [SerializeField]
    PartitioningPlane plane1;
    [SerializeField]
    PartitioningPlane plane2;
    [SerializeField]
    PartitioningPlane plane3;

    BSPNode head;

    Particle3D[] particles;
    float restitution = 1.0f;
    Vector3 contactNormal;
    float penetration;

    GamePause pauseManager;
    SpatialPartitioning spManager;
    int numCollisions = 0;

    private void Awake()
    {
        pauseManager = GameObject.Find("GameStateManager").GetComponent<GamePause>();
        spManager = GameObject.Find("GameStateManager").GetComponent<SpatialPartitioning>();
    }

    private void Start()
    {
        particles = FindObjectsOfType<Particle3D>();

        int layerCount = 1;
        head = new BSPNode();
        setUpNode(head, layerCount);
    }

    private void FixedUpdate()
    {
        if (!pauseManager.getPauseState())
        {
            if (spManager.getSP())
            {
                for (int i = 0; i < particles.Length; i++)
                {
                    loadBSPTree(head, particles[i], 1);
                }

                iterateThroughList(head);
            }
        }
    }

    void setUpNode(BSPNode head, int layerCount)
    {
        if (layerCount == 1)
        {
            BSPChild frontChild = new BSPChild();
            BSPChild backChild = new BSPChild();

            BSPNode frontNode = new BSPNode();
            BSPNode backNode = new BSPNode();

            head.plane = plane1;
            head.front = frontChild;
            head.back = backChild;

            frontChild.type = BSPChildType.NODE;
            frontChild.node = frontNode;
            backChild.type = BSPChildType.NODE;
            backChild.node = backNode;

            setUpNode(frontNode, layerCount + 1);
            setUpNode(backNode, layerCount + 1);
        } 
        else if (layerCount == 2)
        {
            BSPChild frontChild = new BSPChild();
            BSPChild backChild = new BSPChild();

            BSPNode frontNode = new BSPNode();
            BSPNode backNode = new BSPNode();

            head.plane = plane2;
            head.front = frontChild;
            head.back = backChild;

            frontChild.type = BSPChildType.NODE;
            frontChild.node = frontNode;
            backChild.type = BSPChildType.NODE;
            backChild.node = backNode;

            setUpNode(frontNode, layerCount + 1);
            setUpNode(backNode, layerCount + 1);
        }
        else if (layerCount == 3)
        {
            BSPChild frontChild = new BSPChild();
            BSPChild backChild = new BSPChild();

            List<Particle3D> frontList = new List<Particle3D>();
            List<Particle3D> backList = new List<Particle3D>();

            head.plane = plane3;
            head.front = frontChild;
            head.back = backChild;

            frontChild.type = BSPChildType.OBJECTS;
            frontChild.BSPObjectSet = frontList;
            backChild.type = BSPChildType.OBJECTS;
            backChild.BSPObjectSet = backList;
        }
    }

    void loadBSPTree(BSPNode head, Particle3D particle, int layer)
    {
        float c = 0.0f; ;

        //Determines where the particle lies in relation to the plane
        if (layer == 1)
        {
            c = Vector3.Dot((particle.transform.position - plane1.position), plane1.direction);
        }
        else if (layer == 2)
        {
            c = Vector3.Dot((particle.transform.position - plane2.position), plane2.direction);
        }
        else if (layer == 3)
        {
            c = Vector3.Dot((particle.transform.position - plane3.position), plane3.direction);
        }

        //Checks if the particle lies on top of the plane
        if (Mathf.Abs(c) <= particle.transform.localScale.x)
        {
            if (head.front.type == BSPChildType.NODE)
            {
                loadBSPTree(head.front.node, particle, layer + 1);
            }
            else if (head.front.type == BSPChildType.OBJECTS)
            {
                head.front.BSPObjectSet.Add(particle);
            }

            if (head.back.type == BSPChildType.NODE)
            {
                loadBSPTree(head.back.node, particle, layer + 1);
            }
            else if (head.back.type == BSPChildType.OBJECTS)
            {
                head.back.BSPObjectSet.Add(particle);
            }
        }
        else
        {
            //Moves the particle through based on whether it is in front of the plane (positive) behind the plane (negative)
            if (c > 0.0f)
            {
                //Either moves the particle through the tree or add its to the list
                if (head.front.type == BSPChildType.NODE)
                {
                    loadBSPTree(head.front.node, particle, layer + 1);
                }
                else if (head.front.type == BSPChildType.OBJECTS)
                {
                    head.front.BSPObjectSet.Add(particle);
                }
            }
            else if (c < 0.0f)
            {
                //Either moves the particle through the tree or add its to the list
                if (head.back.type == BSPChildType.NODE)
                {
                    loadBSPTree(head.back.node, particle, layer + 1);
                }
                else if (head.back.type == BSPChildType.OBJECTS)
                {
                    head.back.BSPObjectSet.Add(particle);
                }
            }
        }
    }

    void iterateThroughList(BSPNode head)
    {
        //Iterate through tree to find object lists
        //run detect collisions on each list

        if (head.front.type == BSPChildType.NODE)
        {
            iterateThroughList(head.front.node);
        }
        else if (head.front.type == BSPChildType.OBJECTS)
        {
            detectCollisions(head.front.BSPObjectSet);
            head.front.BSPObjectSet.Clear();
        }

        if (head.back.type == BSPChildType.NODE)
        {
            iterateThroughList(head.back.node);
        }
        else if (head.back.type == BSPChildType.OBJECTS)
        {
            detectCollisions(head.back.BSPObjectSet);
            head.back.BSPObjectSet.Clear();
        }
    }
    void detectCollisions(List<Particle3D> particles)
    {
        for (int i = 0; i < particles.Count; i++)
        {
            for (int j = i + 1; j < particles.Count; j++)
            {
                if ((particles[i].transform.position - particles[j].transform.position).magnitude < (particles[i].transform.localScale.x / 2) + (particles[j].transform.localScale.x / 2))
                {
                    resolve(particles[i], particles[j]);
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

    public int getNumCollisions()
    {
        return numCollisions;
    }

    public void resetNumCollisions()
    {
        numCollisions = 0;
    }
}
