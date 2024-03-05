using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosiveForce : ParticleForceGenerator
{
    float radius;
    [SerializeField]
    public float explosionStrength;
    [SerializeField]
    bool exploded;
    Particle2D[] particle2D;
    Particle3D[] particle3D;

    float explosForce;
    //Vector2 direction2D;
    Vector3 direction3D;
    int explosionCount = 0;

    [SerializeField] GameObject explosionObject;

    // Start is called before the first frame update
    void Start()
    {
        particle3D = FindObjectsOfType<Particle3D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.X) && exploded == false)
        {
            Instantiate(explosionObject, transform.position, Quaternion.identity);
            exploded = true;
        }
    }

    public override void updateForce(Particle2D particle)
    {
        if (exploded)
        {
            explosForce = explosionStrength * particle.getInverseMass();
            direction3D = (particle.getPosition() - gameObject.transform.position).normalized * explosForce;

            particle.addForce(direction3D);
        }
    }

    public override void updateForce(Particle3D particle)
    {
        if (exploded && explosionCount < particle3D.Length)
        {
            explosForce = explosionStrength;
            direction3D = (particle.getPosition() - gameObject.transform.position).normalized * explosForce;

            particle.addForce(direction3D);
            explosionCount++;
        }
    }
}
