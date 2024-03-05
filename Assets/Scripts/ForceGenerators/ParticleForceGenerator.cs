using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ParticleForceGenerator : MonoBehaviour
{
    public abstract void updateForce(Particle2D particle);
    public abstract void updateForce(Particle3D particle);
}
