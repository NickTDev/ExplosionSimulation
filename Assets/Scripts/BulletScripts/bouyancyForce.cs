using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bouyancyForce : ParticleForceGenerator
{
    [SerializeField]
    float maxDepth;
    [SerializeField]
    float volume;
    [SerializeField]
    float waterHeight;
    [SerializeField]
    float liquidDensity;

    Vector3 bouyForce;

    public override void updateForce(Particle2D particle)
    {
        float depth = particle.transform.position.y;

        if (depth >= waterHeight + maxDepth)
        {
            return;
        }
        bouyForce = Vector3.zero;

        //Max Depth
        if (depth <= waterHeight - maxDepth)
        {
            bouyForce.y = liquidDensity * volume;
            particle.addForce(bouyForce);
            return;
        }

        //Partly submerged
        bouyForce.y = liquidDensity * volume * (depth - maxDepth - waterHeight) / 2 * maxDepth;
        particle.addForce(bouyForce);
    }

    public override void updateForce(Particle3D particle)
    {
        float depth = particle.transform.position.y;

        if (depth >= waterHeight + maxDepth)
        {
            return;
        }
        bouyForce = Vector3.zero;

        //Max Depth
        if (depth <= waterHeight - maxDepth)
        {
            bouyForce.y = liquidDensity * volume;
            particle.addForce(bouyForce);
            return;
        }

        //Partly submerged
        bouyForce.y = liquidDensity * volume * (depth - maxDepth - waterHeight) / 2 * maxDepth;
        particle.addForce(bouyForce);
    }
}
