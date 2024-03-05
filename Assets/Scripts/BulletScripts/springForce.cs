using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class springForce : ParticleForceGenerator
{
    [SerializeField]
    GameObject toSpringTo;
    Particle2D other;

    float springConstant = 10.0f;
    float restLength = 0.5f;

    Vector3 sprForce;

    public override void updateForce(Particle2D particle)
    {
        other = GameObject.Find(toSpringTo.name + "(Clone)").GetComponent<Particle2D>();

        sprForce = particle.transform.position;
        sprForce -= other.transform.position;

        float magnitude = sprForce.magnitude;
        magnitude = Mathf.Abs(magnitude - restLength);
        magnitude *= springConstant;

        sprForce = sprForce.normalized;
        sprForce *= -magnitude;
        particle.addForce(sprForce);
    }

    public override void updateForce(Particle3D particle)
    {
        other = GameObject.Find(toSpringTo.name + "(Clone)").GetComponent<Particle2D>();

        sprForce = particle.transform.position;
        sprForce -= other.transform.position;

        float magnitude = sprForce.magnitude;
        magnitude = Mathf.Abs(magnitude - restLength);
        magnitude *= springConstant;

        sprForce = sprForce.normalized;
        sprForce *= -magnitude;
        particle.addForce(sprForce);
    }
}
