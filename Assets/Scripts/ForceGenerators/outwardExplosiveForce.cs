using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class outwardExplosiveForce : ParticleForceGenerator
{
    //Implosion Data
    Vector3 detonationPoint;
    [SerializeField] float implosionMaxRadius; //Max radius at which something is effected by implosion
    [SerializeField] float implosionMinRadius; //Min radius at which something is effected by implosion
    [SerializeField] float implosionDuration; //How long the implosion occurs
    [SerializeField] float implosionForce; //Force at which the objects are pulled towards the detonation point

    //Shockwave Data
    [SerializeField] float explosionMaxRadius;
    [SerializeField] float explosionMinRadius;
    [SerializeField] float explosionForce;
    float shockwaveRadius;

    //Convection Data
    float convectionStartTime;
    float convectionDuration;
    [SerializeField] float convectionForce;
    [SerializeField] float chimneyRadius;
    [SerializeField] float chimneyHeight;
    //[SerializeField] GameObject chimney;

    //Other Data
    bool isImploding = false;
    bool isExploding = false;
    bool isConvectioning = false;

    private void Start()
    {
        detonationPoint = transform.position;
        shockwaveRadius = explosionMinRadius;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            activateImploding();
        }

        if (isExploding)
        {
            shockwaveRadius += 0.1f;
            transform.localScale = new Vector3(shockwaveRadius, shockwaveRadius, shockwaveRadius);

            if (shockwaveRadius > explosionMaxRadius)
                isExploding = false;
        }
    }

    public override void updateForce(Particle3D particle)
    {
        if (isImploding)
        {
            if (Vector3.Distance(particle.transform.position, detonationPoint) < implosionMaxRadius && Vector3.Distance(particle.transform.position, detonationPoint) > implosionMinRadius)
            {
                Vector3 implosionVector = (detonationPoint - particle.transform.position).normalized * (implosionForce / particle.getMass());
                particle.addForce(implosionVector);
            }
        }
        if (isExploding)
        {
            if (Vector3.Distance(particle.transform.position, detonationPoint) < shockwaveRadius)
            {
                Vector3 explosionVector = (particle.transform.position - detonationPoint).normalized * (explosionForce / shockwaveRadius);
                particle.addForce(explosionVector);
            }
        }
        if (isConvectioning)
        {
            Vector2 distance1 = new Vector2(particle.transform.position.x, particle.transform.position.z);
            Vector2 distance2 = new Vector2(transform.position.x, transform.position.z);
            if (Vector2.Distance(distance1, distance2) < chimneyRadius && particle.transform.position.y < (transform.position.y + chimneyHeight))
            {
                Vector3 convectionVector = new Vector3(0, (convectionForce / particle.getMass()), 0);
                particle.addForce(convectionVector);
            }
        }
    }

    public override void updateForce(Particle2D particle)
    {
    }

    void activateImploding()
    {
        isImploding = true;
        Invoke("activateExploding", implosionDuration);
    }

    void activateExploding()
    {
        isImploding = false;
        isExploding = true;
        Invoke("activateConvectioning", convectionStartTime);
    }

    void activateConvectioning()
    {
        isConvectioning = true;
        Invoke("endExplosion", convectionDuration);
    }

    void endExplosion()
    {
        isConvectioning = false;
    }
}
