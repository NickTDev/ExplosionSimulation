using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class anchorSpringForce : ParticleForceGenerator
{
    [SerializeField]
    Vector3 anchor;

    float springConstant = 10.0f;
    float restLength = 0.5f;

    Vector3 anchorSprForce;

    public override void updateForce(Particle2D particle)
    {
        anchorSprForce = particle.transform.position;
        anchorSprForce -= anchor;

        float magnitude = anchorSprForce.magnitude;
        magnitude = (restLength - magnitude) * springConstant;

        anchorSprForce = anchorSprForce.normalized;
        anchorSprForce *= magnitude;

        particle.addForce(anchorSprForce);
    }

    public override void updateForce(Particle3D particle)
    {
        anchorSprForce = particle.transform.position;
        anchorSprForce -= anchor;

        float magnitude = anchorSprForce.magnitude;
        magnitude = (restLength - magnitude) * springConstant;

        anchorSprForce = anchorSprForce.normalized;
        anchorSprForce *= magnitude;

        particle.addForce(anchorSprForce);
    }
}
