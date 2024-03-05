using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class switchExplosion : MonoBehaviour
{
    Particle3D[] particles;

    private void Start()
    {
        particles = FindObjectsOfType<Particle3D>();

        for (int i = 0; i < particles.Length; i++)
        {
            particles[i].isFirstScene = false;
        }
    }
}
